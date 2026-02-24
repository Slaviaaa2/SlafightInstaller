using System;
using System.IO;
using System.IO.Compression;
using System.Net;

namespace SlafightInstaller.InstallUtils;

public static class InstallBepInEx
{
    public static bool Install(string path, string version)
    {
        if (!Utils.IsValidPath(path)) return false;

        var resourceUrl =
            $"https://github.com/BepInEx/BepInEx/releases/download/v{version}/BepInEx_win_x64_{version}.zip";

        var filePath = Path.Combine(path, $"BepInEx_win_x64_{version}.zip");

        try
        {
            using var client = new WebClient();
            client.DownloadFile(resourceUrl, filePath);

            if (!File.Exists(filePath)) return false;

            Utils.ExtractOverwrite(filePath, path);
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
}
