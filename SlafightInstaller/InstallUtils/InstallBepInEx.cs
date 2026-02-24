using System;
using System.IO;
using System.IO.Compression;
using System.Net;

namespace SlafightInstaller.InstallUtils
{
    public static class InstallBepInEx
    {
        public static bool Install(string path, string version)
        {
            if (!Utils.IsValidPath(path)) return false;

            var bepinExDir = Path.Combine(path, "BepInEx");
            if (Directory.Exists(bepinExDir))
            {
                Console.Write(Messages.Get("OverwriteAsk"));
                var ans = Console.ReadLine()?.Trim().ToLower();
                if (ans != "y")
                {
                    Console.WriteLine(Messages.Get("OverwriteSkip"));
                    return true; // スキップだが致命的エラーではない扱い
                }
            }

            var resourceUrl =
                $"https://github.com/BepInEx/BepInEx/releases/download/v{version}/BepInEx_win_x64_{version}.zip";

            var filePath = Path.Combine(path, $"BepInEx_win_x64_{version}.zip");

            try
            {
                using var client = new WebClient();
                client.DownloadFile(resourceUrl, filePath);

                if (!File.Exists(filePath))
                    return false;

#if NET6_0_OR_GREATER
                ZipFile.ExtractToDirectory(filePath, path, overwriteFiles: true);
#else
                ExtractOverwrite(filePath, path);
#endif

                File.Delete(filePath);
                Directory.CreateDirectory(Path.Combine(path, "BepInEx", "plugins"));

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

#if !NET6_0_OR_GREATER
        // overwriteFiles オーバーロードがない場合用
        private static void ExtractOverwrite(string zipPath, string destPath)
        {
            using var archive = ZipFile.OpenRead(zipPath);
            foreach (var entry in archive.Entries)
            {
                var fullPath = Path.Combine(destPath, entry.FullName);
                var dir = Path.GetDirectoryName(fullPath);
                if (!string.IsNullOrEmpty(dir))
                    Directory.CreateDirectory(dir);

                if (string.IsNullOrEmpty(entry.Name))
                    continue;

                entry.ExtractToFile(fullPath, overwrite: true);
            }
        }
#endif
    }
}
