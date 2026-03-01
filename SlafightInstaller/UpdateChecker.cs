using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace SlafightInstaller
{
    public static class UpdateChecker
    {
        public const string LatestReleaseApiUrl =
            "https://api.github.com/repos/Slaviaaa2/SlafightInstaller/releases/latest";

        // 数値バージョン（ここだけ手動で上げる）
        public static readonly Version CurrentVersion = new Version(2, 1);

        public static string GetCurrentVersionDisplay()
        {
            return $"v{CurrentVersion.Major}.{CurrentVersion.Minor}";
        }

        public static void CheckForUpdates()
        {
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
                        var assetUrl   = ExtractFirstAssetDownloadUrl(json);

                        if (string.IsNullOrEmpty(tagName))
                            return;

                        var latestVer    = ParseTagToVersion(tagName);
                        var isSameNumber = latestVer == CurrentVersion;

                        // 数値的に古い → 何もしない
                        if (latestVer < CurrentVersion)
                            return;

                        // 数値は同じ & pre-release → 無視（同じ系列のプレ版）
                        if (isSameNumber && prerelease)
                            return;

                        // ここまで来たら:
                        // ・latest > current  → 純粋に新しい
                        // ・または latest == current & prerelease == false → pre → 正式版になった
                        var currentVerStr = GetCurrentVersionDisplay();

                        ConsoleUI.Divider();
                        if (!prerelease)
                        {
                            ConsoleUI.Info(string.Format(Messages.Get("Update_Stable"), tagName, currentVerStr));
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

        // tag_name（例: "v2", "v2.1", "v2.1.3-beta"）→ Version(2,1,3)
        private static Version ParseTagToVersion(string tagName)
        {
            var sb = new StringBuilder();
            foreach (var ch in tagName)
            {
                if ((ch >= '0' && ch <= '9') || ch == '.')
                    sb.Append(ch);
                else if (sb.Length > 0)
                    break;
            }

            var num = sb.ToString();
            if (string.IsNullOrEmpty(num))
                return CurrentVersion;

            var parts = num.Split('.');
            while (parts.Length < 3)
            {
                num += ".0";
                parts = num.Split('.');
            }

            Version v;
            if (Version.TryParse(num, out v))
                return v;

            return CurrentVersion;
        }

        private static void DownloadAndRunUpdater(string assetUrl, string tagName)
        {
            try
            {
                var tmpDir  = Path.GetTempPath();
                var fileExt = Path.GetExtension(assetUrl);
                if (string.IsNullOrEmpty(fileExt)) fileExt = ".exe";

                var safeTag    = tagName.Replace('/', '_').Replace('\\', '_');
                var newExePath = Path.Combine(tmpDir, $"SlafightInstaller_new_{safeTag}{fileExt}");

                ConsoleUI.Info(Messages.Get("Update_Downloading"));
                using (var wc = new WebClient())
                {
                    wc.Headers.Add("User-Agent", "SlafightInstaller/UpdateDownloader");
                    wc.DownloadFile(assetUrl, newExePath);
                }
                ConsoleUI.Success(Messages.Get("Update_DownloadDone"));

                // 現在動いている exe のフルパス
                var currentExePath = Assembly.GetExecutingAssembly().Location;

                // 同じフォルダに置いた SlafightInstaller.Updater.exe を使う
                var updaterExePath = Path.Combine(
                    Path.GetDirectoryName(currentExePath)!,
                    "SlafightInstaller.Updater.exe"
                );

                if (!File.Exists(updaterExePath))
                {
                    ConsoleUI.Error("SlafightInstaller.Updater.exe not found. Cannot self-update.");
                    ConsoleUI.Info($"Expected: {updaterExePath}");
                    return;
                }

                // 引数: "<currentExePath>" "<newExePath>"
                var psi = new ProcessStartInfo
                {
                    FileName  = updaterExePath,
                    Arguments = $"\"{currentExePath}\" \"{newExePath}\"",
                    UseShellExecute = false
                };

                Process.Start(psi);

                // 自分は終了して updater に任せる
                Environment.Exit(0);
            }
            catch (Exception ex)
            {
                ConsoleUI.Error(string.Format(Messages.Get("Update_DownloadFailed"), ex.Message));
            }
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

        private static string ExtractFirstAssetDownloadUrl(string json)
        {
            var idxAssets = json.IndexOf("\"assets\"", StringComparison.OrdinalIgnoreCase);
            if (idxAssets < 0) return "";

            var idxBracket = json.IndexOf('[', idxAssets);
            if (idxBracket < 0) return "";

            var endBracket = FindMatchingBracket(json, idxBracket);
            if (endBracket < 0) return "";

            var assetsBlock = json.Substring(idxBracket, endBracket - idxBracket + 1);

            var m = Regex.Match(assetsBlock, "\"browser_download_url\"\\s*:\\s*\"(.*?)\"");
            return m.Success ? UnescapeJsonString(m.Groups[1].Value) : "";
        }

        private static int FindMatchingBracket(string s, int open)
        {
            int depth = 0;
            bool inString = false;
            for (int i = open; i < s.Length; i++)
            {
                if (s[i] == '"' && (i == 0 || s[i - 1] != '\\')) inString = !inString;
                if (inString) continue;
                if (s[i] == '[') depth++;
                else if (s[i] == ']')
                {
                    depth--;
                    if (depth == 0) return i;
                }
            }
            return -1;
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
