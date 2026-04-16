using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace SlafightInstaller.Updater
{
    internal static class Program
    {
        // 新方式: args[0] = installDir (インストール先ディレクトリ), args[1] = extractDir (展開フォルダ)
        // 旧方式: args[0] = targetExePath (上書き対象 exe),           args[1] = newExePath (新 exe)
        private static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Usage:");
                Console.WriteLine("  New: SlafightInstaller.Updater <installDir> <extractDir>");
                Console.WriteLine("  Old: SlafightInstaller.Updater <targetExePath> <newExePath>");
                return;
            }

            var arg0 = args[0];
            var arg1 = args[1];

            // モード判定: arg1 がディレクトリならフォルダモード、.exe ファイルなら旧モード
            if (Directory.Exists(arg1))
                RunFolderMode(arg0, arg1);
            else if (File.Exists(arg1) && arg1.EndsWith(".exe", StringComparison.OrdinalIgnoreCase))
                RunLegacyMode(arg0, arg1);
            else
                Console.WriteLine($"Error: '{arg1}' is neither a directory nor an exe file.");
        }

        /// <summary>
        /// 新方式: 展開フォルダの全ファイルをインストールディレクトリにコピー
        /// </summary>
        private static void RunFolderMode(string installDir, string extractDir)
        {
            var mainExe = Path.Combine(installDir, "SlafightInstaller.exe");
            WaitForProcessExit("SlafightInstaller", timeoutMs: 30000);

            try
            {
                var updaterName = Path.GetFileName(
                    System.Reflection.Assembly.GetExecutingAssembly().Location);

                foreach (var srcFile in Directory.GetFiles(extractDir))
                {
                    var fileName = Path.GetFileName(srcFile);

                    // 自身（Updater）は実行中なのでスキップ
                    if (fileName.Equals(updaterName, StringComparison.OrdinalIgnoreCase))
                        continue;

                    var destFile = Path.Combine(installDir, fileName);
                    try
                    {
                        File.Copy(srcFile, destFile, overwrite: true);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Warning: Could not copy {fileName}: {ex.Message}");
                    }
                }

                Console.WriteLine("Update completed successfully.");

                // メイン exe を再起動
                if (File.Exists(mainExe))
                {
                    try
                    {
                        Process.Start(new ProcessStartInfo
                        {
                            FileName = mainExe,
                            UseShellExecute = true
                        });
                    }
                    catch { }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Update failed: " + ex.Message);
            }
            finally
            {
                // 展開フォルダを掃除
                try
                {
                    if (Directory.Exists(extractDir))
                        Directory.Delete(extractDir, true);
                }
                catch { }
            }
        }

        /// <summary>
        /// 旧方式: 単一 exe を上書き（v2.1.x 以前との互換性）
        /// </summary>
        private static void RunLegacyMode(string targetExePath, string newExePath)
        {
            WaitForProcessExit("SlafightInstaller", timeoutMs: 30000);

            try
            {
                // バックアップ
                var backupPath = targetExePath + ".bak";
                try
                {
                    if (File.Exists(backupPath))
                        File.Delete(backupPath);
                    if (File.Exists(targetExePath))
                        File.Move(targetExePath, backupPath);
                }
                catch { }

                File.Copy(newExePath, targetExePath, overwrite: true);

                try
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = targetExePath,
                        UseShellExecute = true
                    });
                }
                catch { }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Update failed: " + ex.Message);
            }
            finally
            {
                try
                {
                    if (File.Exists(newExePath))
                        File.Delete(newExePath);
                }
                catch { }
            }
        }

        private static void WaitForProcessExit(string processName, int timeoutMs)
        {
            var sw = Stopwatch.StartNew();
            while (sw.ElapsedMilliseconds < timeoutMs)
            {
                var procs = Process.GetProcessesByName(processName);
                if (procs.Length == 0)
                    break;
                Thread.Sleep(500);
            }
        }
    }
}
