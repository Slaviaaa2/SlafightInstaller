using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Linq;

namespace SlafightInstaller
{
    public static class UpdateChecker
    {
        public const string LatestReleaseApiUrl =
            "https://api.github.com/repos/Slaviaaa2/SlafightInstaller/releases/latest";

        // ★手動管理（ここだけ編集）
        public static readonly Version CurrentVersion = new Version(2, 2, 0, 0);
        public static readonly bool IsCurrentBeta = false;   // true=pre-release
        public static readonly bool IsCurrentDev = false;    // true=開発版（更新無視）

        public static string GetCurrentVersionDisplay()
        {
            var baseVer = CurrentVersion.Build == -1 
                ? $"{CurrentVersion.Major}.{CurrentVersion.Minor}"
                : CurrentVersion.Revision == -1 
                    ? $"{CurrentVersion.Major}.{CurrentVersion.Minor}.{CurrentVersion.Build}"
                    : $"{CurrentVersion.Major}.{CurrentVersion.Minor}.{CurrentVersion.Build}.{CurrentVersion.Revision}";

            // ★カスタムラベル（手動編集）
            string label = "";
            if (IsCurrentDev) label = "dev";
            else if (IsCurrentBeta) label = "prerelease";
            else label = "stable";

            // ★固有ラベル上書き（例: "beta2", "rc1" など）
            const string CUSTOM_LABEL = "";  // ここに"cand1"とか入れる

            return CUSTOM_LABEL != "" 
                ? $"v{baseVer} ({CUSTOM_LABEL})"
                : label == "" 
                    ? $"v{baseVer}"
                    : $"v{baseVer} ({label})";
        }

        public static void CheckForUpdates()
        {
            // 開発版ならスキップ
            if (IsCurrentDev)
            {
                ConsoleUI.Info("現在開発者用バージョンを使用中の為、スキップします...");
                return;
            }

            try
            {
                var req = (HttpWebRequest)WebRequest.Create(LatestReleaseApiUrl);
                req.Method    = "GET";
                req.UserAgent = "SlafightInstaller/SlafightInstaller.Updater";
                req.Accept    = "application/vnd.github+json";

                using (var resp = (HttpWebResponse)req.GetResponse())
                using (var stream = resp.GetResponseStream())
                {
                    if (stream == null) return;
                    using (var reader = new StreamReader(stream, Encoding.UTF8))
                    {
                        var json = reader.ReadToEnd();

                        var tagName    = ExtractString(json, "tag_name");
                        var htmlUrl    = ExtractString(json, "html_url");
                        var prerelease = ExtractBool(json, "prerelease");
                        // 新方式: SlafightInstaller.zip、旧方式: UpdateFile.exe（フォールバック）
                        var assetUrl = ExtractAssetDownloadUrlByName(json, "SlafightInstaller.zip");
                        var isZipUpdate = !string.IsNullOrEmpty(assetUrl);
                        if (!isZipUpdate)
                            assetUrl = ExtractAssetDownloadUrlByName(json, "UpdateFile.exe");

                        if (string.IsNullOrEmpty(tagName)) return;

                        var latestVer = ParseTagToVersion(tagName);
                        var isSameNumber = latestVer == CurrentVersion;
                        var isCurrentStable = !IsCurrentBeta;

                        // 数値的に古い → スキップ
                        if (latestVer < CurrentVersion) return;

                        // 同ver & 両方stable → スキップ
                        if (isSameNumber && isCurrentStable && !prerelease) return;

                        // 同ver & current=beta → stable版通知
                        if (isSameNumber && IsCurrentBeta && !prerelease)
                        {
                            NotifyUpdate(json, tagName, htmlUrl, prerelease, "正式版がリリースされました！");
                            return;
                        }

                        // 同ver & current=stable → beta無視
                        if (isSameNumber && isCurrentStable && prerelease) return;

                        // 新しいver → 通知
                        NotifyUpdate(json, tagName, htmlUrl, prerelease);
                    }
                }
            }
            catch (WebException wex)
            {
                ConsoleUI.Warn($"Update check failed: {wex.Message}");
            }
            catch (Exception ex)
            {
                ConsoleUI.Warn($"Update check error: {ex.Message}");
            }
        }

        private static void NotifyUpdate(string json, string tagName, string htmlUrl, bool prerelease, string extraMsg = "")
        {
            var currentVerStr = GetCurrentVersionDisplay();
            var assetUrl = ExtractAssetDownloadUrlByName(json, "SlafightInstaller.zip");
            if (string.IsNullOrEmpty(assetUrl))
                assetUrl = ExtractAssetDownloadUrlByName(json, "UpdateFile.exe");

            ConsoleUI.Divider();
            if (!prerelease)
            {
                ConsoleUI.Info($"{extraMsg}\n{string.Format(Messages.Get("Update_Stable"), tagName, currentVerStr)}");
                ConsoleUI.Info(string.Format(Messages.Get("Update_StableUrl"), htmlUrl));
            }
            else
            {
                ConsoleUI.Warn(string.Format(Messages.Get("Update_Pre"), tagName, currentVerStr));
                ConsoleUI.Warn(Messages.Get("Update_PreWarn"));
                ConsoleUI.Info(string.Format(Messages.Get("Update_StableUrl"), htmlUrl));
            }

            if (string.IsNullOrEmpty(assetUrl))
            {
                ConsoleUI.Warn(Messages.Get("Update_NoAssets"));
                ConsoleUI.Divider();
                return;
            }

            ConsoleUI.Prompt(Messages.Get("Update_DownloadAsk"));
            var ans = Console.ReadLine()?.Trim().ToLower();
            if (ans != "y")
            {
                ConsoleUI.Info(Messages.Get("Update_Skip"));
                ConsoleUI.Divider();
                return;
            }

            DownloadAndRunUpdater(assetUrl, tagName);
            ConsoleUI.Divider();
        }

        private static Version ParseTagToVersion(string tagName)
        {
            var num = Regex.Replace(tagName, @"[^0-9.]", "");
            return string.IsNullOrEmpty(num) || !Version.TryParse(num, out var v) 
                ? new Version(0, 0) : v;
        }

        private static string ExtractAssetDownloadUrlByName(string json, string targetName)
        {
            if (json.IndexOf($"\"name\":\"{targetName}\"", StringComparison.Ordinal) < 0)
                return "";

            var nameIdx = json.IndexOf($"\"name\":\"{targetName}\"", StringComparison.Ordinal);
            var urlStart = json.IndexOf("\"browser_download_url\"", nameIdx, StringComparison.Ordinal);
            if (urlStart == -1) return "";

            var urlMatch = Regex.Match(json.Substring(urlStart), @"""browser_download_url""\s*:\s*""([^""]+)""");
            return urlMatch.Success ? UnescapeJsonString(urlMatch.Groups[1].Value) : "";
        }

        private static string ExtractString(string json, string key)
        {
            var pattern = $"\"{key}\"\\s*:\\s*\"(.*?)\"";
            var m = Regex.Match(json, pattern);
            return m.Success ? UnescapeJsonString(m.Groups[1].Value) : "";
        }

        private static bool ExtractBool(string json, string key)
        {
            var pattern = $"\"{key}\"\\s*:\\s*(true|false)";
            var m = Regex.Match(json, pattern, RegexOptions.IgnoreCase);
            if (!m.Success) return false;
            return string.Equals(m.Groups[1].Value, "true", StringComparison.OrdinalIgnoreCase);
        }

        private static void DownloadAndRunUpdater(string assetUrl, string tagName)
        {
            try
            {
                var tmpDir   = Path.GetTempPath();
                var safeTag  = tagName.Replace('/', '_').Replace('\\', '_');
                var isZip    = assetUrl.EndsWith(".zip", StringComparison.OrdinalIgnoreCase);

                var currentExePath = Assembly.GetExecutingAssembly().Location;
                var installDir     = Path.GetDirectoryName(currentExePath)!;
                var updaterExePath = Path.Combine(installDir, "SlafightInstaller.Updater.exe");

                ConsoleUI.Info(Messages.Get("Update_Downloading"));

                if (isZip)
                {
                    // 新方式: zip をダウンロード → 展開 → Updater を先に上書き → フォルダモードで起動
                    var zipPath    = Path.Combine(tmpDir, $"SlafightInstaller_update_{safeTag}.zip");
                    var extractDir = Path.Combine(tmpDir, $"SlafightInstaller_update_{safeTag}");

                    using (var wc = new WebClient())
                    {
                        wc.Headers.Add("User-Agent", "SlafightInstaller/UpdateDownloader");
                        wc.DownloadFile(assetUrl, zipPath);
                    }

                    // 展開先を掃除してから展開
                    if (Directory.Exists(extractDir))
                        Directory.Delete(extractDir, true);
                    ZipFile.ExtractToDirectory(zipPath, extractDir);

                    // zip を掃除
                    try { File.Delete(zipPath); } catch { }

                    ConsoleUI.Success(Messages.Get("Update_DownloadDone"));

                    // 展開フォルダに新しい Updater があれば、起動前に上書き
                    var newUpdater = Path.Combine(extractDir, "SlafightInstaller.Updater.exe");
                    if (File.Exists(newUpdater) && File.Exists(updaterExePath))
                    {
                        try { File.Copy(newUpdater, updaterExePath, overwrite: true); }
                        catch { /* Updater がロック中の場合は旧版で続行 */ }
                    }

                    if (!File.Exists(updaterExePath))
                    {
                        ConsoleUI.Error("SlafightInstaller.Updater.exe not found. Cannot self-update.");
                        return;
                    }

                    // Updater をフォルダモードで起動: args[0]=installDir, args[1]=extractDir
                    Process.Start(new ProcessStartInfo
                    {
                        FileName    = updaterExePath,
                        Arguments   = $"\"{installDir}\" \"{extractDir}\"",
                        UseShellExecute = false
                    });
                }
                else
                {
                    // 旧方式: 単一 exe ダウンロード → exe モードで Updater 起動
                    var newExePath = Path.Combine(tmpDir, $"SlafightInstaller_new_{safeTag}.exe");
                    using (var wc = new WebClient())
                    {
                        wc.Headers.Add("User-Agent", "SlafightInstaller/UpdateDownloader");
                        wc.DownloadFile(assetUrl, newExePath);
                    }
                    ConsoleUI.Success(Messages.Get("Update_DownloadDone"));

                    if (!File.Exists(updaterExePath))
                    {
                        ConsoleUI.Error("SlafightInstaller.Updater.exe not found. Cannot self-update.");
                        return;
                    }

                    Process.Start(new ProcessStartInfo
                    {
                        FileName    = updaterExePath,
                        Arguments   = $"\"{currentExePath}\" \"{newExePath}\"",
                        UseShellExecute = false
                    });
                }

                Environment.Exit(0);
            }
            catch (Exception ex)
            {
                ConsoleUI.Error(string.Format(Messages.Get("Update_DownloadFailed"), ex.Message));
            }
        }

        private static string UnescapeJsonString(string s)
        {
            return s.Replace("\\\"", "\"")
                    .Replace("\\\\", "\\")
                    .Replace("\\n", "\n")
                    .Replace("\\r", "\r")
                    .Replace("\\t", "\t");
        }
    }
}
