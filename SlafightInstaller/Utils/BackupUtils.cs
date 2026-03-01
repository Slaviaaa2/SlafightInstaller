using System;
using System.IO;

namespace SlafightInstaller.Utils
{
    public static class BackupUtils
    {
        public static bool TryBackup(string gamePath, bool autoYes = false)
        {
            if (!autoYes)
            {
                ConsoleUI.Prompt(Messages.Get("BackupAsk"));
                var ans = Console.ReadLine()?.Trim().ToLower();
                if (ans != "y")
                {
                    ConsoleUI.Warn(Messages.Get("BackupSkip"));
                    return true;
                }
            }

            return Backup(gamePath);
        }

        public static bool Backup(string gamePath)
        {
            try
            {
                var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                var parentDir = Path.GetDirectoryName(gamePath)!;
                var folderName = Path.GetFileName(gamePath);
                var backupPath = Path.Combine(parentDir, $"{folderName}_backup_{timestamp}");

                ConsoleUI.Info(string.Format(Messages.Get("BackupCreating"), backupPath));
                CopyDirectory(gamePath, backupPath);
                ConsoleUI.Success(string.Format(Messages.Get("BackupSuccess"), backupPath));
                return true;
            }
            catch (Exception ex)
            {
                ConsoleUI.Error(string.Format(Messages.Get("BackupFailed"), ex.Message));
                return false;
            }
        }

        private static void CopyDirectory(string sourceDir, string destDir)
        {
            Directory.CreateDirectory(destDir);

            foreach (var file in Directory.GetFiles(sourceDir))
            {
                var destFile = Path.Combine(destDir, Path.GetFileName(file));
                File.Copy(file, destFile, overwrite: true);
            }

            foreach (var subDir in Directory.GetDirectories(sourceDir))
            {
                var destSubDir = Path.Combine(destDir, Path.GetFileName(subDir));
                CopyDirectory(subDir, destSubDir);
            }
        }
    }
}