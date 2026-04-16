using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.Win32;

namespace SlafightInstaller.Utils
{
    public static class SteamDetector
    {
        /// <summary>
        /// Steam ライブラリをスキャンしてゲームフォルダのフルパスを返す。見つからなければ null。
        /// </summary>
        public static string? FindGamePath(string gameFolderName, string? exeName = null)
        {
            exeName ??= gameFolderName + ".exe";

            var libraryRoots = GetSteamLibraryRoots();
            foreach (var root in libraryRoots)
            {
                var gamePath = Path.Combine(root, "steamapps", "common", gameFolderName);
                if (Directory.Exists(gamePath) && File.Exists(Path.Combine(gamePath, exeName)))
                    return gamePath;
            }

            return null;
        }

        private static List<string> GetSteamLibraryRoots()
        {
            var roots = new List<string>();
            var steamPath = GetSteamInstallPath();

            if (steamPath != null)
                roots.Add(steamPath);

            // libraryfolders.vdf からライブラリフォルダを読み取り
            if (steamPath != null)
            {
                var vdfPath = Path.Combine(steamPath, "steamapps", "libraryfolders.vdf");
                if (File.Exists(vdfPath))
                {
                    try
                    {
                        var vdf = File.ReadAllText(vdfPath);
                        var matches = Regex.Matches(vdf, "\"path\"\\s+\"([^\"]+)\"");
                        foreach (Match m in matches)
                        {
                            var path = m.Groups[1].Value.Replace("\\\\", "\\");
                            if (!roots.Contains(path) && Directory.Exists(path))
                                roots.Add(path);
                        }
                    }
                    catch { /* VDF パース失敗は無視 */ }
                }
            }

            // フォールバック: よくあるパス
            var fallbacks = new[]
            {
                @"C:\Program Files (x86)\Steam",
                @"C:\Program Files\Steam",
                @"D:\Steam",
                @"D:\SteamLibrary",
                @"E:\SteamLibrary",
            };

            foreach (var fb in fallbacks)
            {
                if (!roots.Contains(fb) && Directory.Exists(fb))
                    roots.Add(fb);
            }

            return roots;
        }

        private static string? GetSteamInstallPath()
        {
            try
            {
                using (var key = Registry.CurrentUser.OpenSubKey(@"Software\Valve\Steam"))
                {
                    var val = key?.GetValue("SteamPath") as string;
                    if (val != null)
                        return val.Replace('/', '\\');
                }
            }
            catch { /* レジストリ読取失敗 */ }

            return null;
        }
    }
}
