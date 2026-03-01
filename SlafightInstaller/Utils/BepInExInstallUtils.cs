using System;
using System.IO;
using System.Net;

namespace SlafightInstaller.Utils;

public static class BepInExInstallUtils
{
    public static bool Install(string path, string version)
    {
        if (!BasicUtils.IsValidPath(path)) return false;

        var bepinExDir = Path.Combine(path, "BepInEx");
        if (Directory.Exists(bepinExDir))
        {
            Console.Write(Messages.Get("OverwriteAsk"));
            var ans = Console.ReadLine()?.Trim().ToLower();
            if (ans != "y")
            {
                Console.WriteLine(Messages.Get("OverwriteSkip"));
                return true;
            }
        }

        var resourceUrl =
            $"https://github.com/BepInEx/BepInEx/releases/download/v{version}/BepInEx_win_x64_{version}.zip";

        var filePath = Path.Combine(path, $"BepInEx_win_x64_{version}.zip");

        try
        {
            using var client = new WebClient();
            client.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36");
            client.DownloadFile(resourceUrl, filePath);

            if (!File.Exists(filePath))
                return false;

            BasicUtils.ExtractOverwrite(filePath, path);
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

    public static bool Uninstall(string path)
    {
        if (!BasicUtils.IsValidPath(path)) return false;

        try
        {
            var bepInExDir = Path.Combine(path, "BepInEx");
            if (Directory.Exists(bepInExDir))
            {
                ConsoleUI.Warn("BepInEx folder contains all installed mods. They will also be removed.");
                Directory.Delete(bepInExDir, recursive: true);
                ConsoleUI.Info("Removed: BepInEx/");
            }
            else
            {
                ConsoleUI.Warn("BepInEx directory not found, skipping.");
            }

            var filesToRemove = new[]
            {
                "winhttp.dll",
                "doorstop_config.ini",
                "changelog.txt",
                ".doorstop_version"
            };

            foreach (var file in filesToRemove)
            {
                var filePath = Path.Combine(path, file);
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    ConsoleUI.Info($"Removed: {file}");
                }
                else
                {
                    ConsoleUI.Warn($"Not found, skipping: {file}");
                }
            }

            return true;
        }
        catch (Exception e)
        {
            ConsoleUI.Error($"Uninstall failed: {e.Message}");
            return false;
        }
    }
}