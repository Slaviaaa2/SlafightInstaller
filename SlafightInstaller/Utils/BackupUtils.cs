using System;
using System.IO;
using System.IO.Compression;

namespace SlafightInstaller
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
                var backupDir = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                    "SlafightInstaller", "Backups"
                );
                Directory.CreateDirectory(backupDir);

                var backupPath = Path.Combine(backupDir, $"STRAFTAT_backup_{timestamp}.zip");

                ConsoleUI.Info(string.Format(Messages.Get("BackupCreating"), backupPath));
                ZipFile.CreateFromDirectory(gamePath, backupPath, CompressionLevel.Fastest, false);
                ConsoleUI.Success(string.Format(Messages.Get("BackupSuccess"), backupPath));
                return true;
            }
            catch (Exception ex)
            {
                ConsoleUI.Error(string.Format(Messages.Get("BackupFailed"), ex.Message));
                return false;
            }
        }
    }
}