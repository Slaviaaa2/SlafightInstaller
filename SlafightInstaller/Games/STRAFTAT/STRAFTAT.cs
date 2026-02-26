using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using SlafightInstaller.InstallUtils;

namespace SlafightInstaller.Games.STRAFTAT
{
    public static class STRAFTAT
    {
        public static List<ModBase> ModsList = new List<ModBase>
        {
            new ModBase
            {
                ModName = "BepInEx",
                ModVersion = "5.4.23.4",
                ModDependencies = new List<ModDependency>(),
                ConflictsWith = new List<string>(),
                SourceUrl = null
            },
            new ModBase
            {
                ModName = "ModMenu",
                ModVersion = "1.1.3",
                ModDependencies = new List<ModDependency> { new ModDependency("BepInEx") },
                ConflictsWith = new List<string>(),
                SourceUrl = "https://gcdn.thunderstore.io/live/blob-storage/sha256/9cfe22b0c239e2ec1befd824dcffc1c9f1fd719158d65e49b2062fd251b27a0d.sh_X9iHTbc.blob",
                InstallFileName = "ModMenu.dll"
            },
            new ModBase
            {
                ModName = "Chat Commands",
                ModVersion = "1.2.7",
                ModDependencies = new List<ModDependency> { new ModDependency("BepInEx") },
                ConflictsWith = new List<string>(),
                SourceUrl = "https://gcdn.thunderstore.io/live/blob-storage/sha256/df196fad52ebf98ea5fdbfe7ee0118e67a5e650d3ff01f1b9b8a50528e8176bc.sh_oBz6Gj2.blob",
                InstallFileName = "Chat_Commands.dll"
            },
            new ModBase
            {
                ModName = "moreStrafts",
                ModVersion = "0.0.4",
                ModDependencies = new List<ModDependency> { new ModDependency("BepInEx") },
                ConflictsWith = new List<string>(),
                SourceUrl = "https://github.com/ALBINALSHAIKH/moreStrafts/releases/download/v0.0.4/moreStrafts.dll"
            },
            new ModBase
            {
                ModName = "MoreStrafts UISpawnAddon",
                ModVersion = "1.0.5",
                ModDependencies = new List<ModDependency>
                {
                    new ModDependency("BepInEx"),
                    new ModDependency("moreStrafts", "0.0.4")
                },
                ConflictsWith = new List<string>(),
                SourceUrl = "https://gcdn.thunderstore.io/live/blob-storage/sha256/65a2ea98ae889e7152f969dfb73c62a406a24df738999992c6622a97655e7359.sh_uvDsSU1.blob",
                InstallFileName = "MoreStrafts_UISpawnAddon.dll"
            },
            new ModBase
            {
                ModName = "Fancy",
                ModVersion = "1.0.1",
                ModDependencies = new List<ModDependency> { new ModDependency("BepInEx") },
                ConflictsWith = new List<string>(),
                SourceUrl = "https://gcdn.thunderstore.io/live/blob-storage/sha256/9dc9b1effec14a8c2cbc211ebf259d477f5b734bc50e92e6621d28bcf9571991.sh_nNiakzJ.blob",
                InstallFileName = "Fancy.dll"
            },
            new ModBase
            {
                ModName = "Blacktie Straftat",
                ModVersion = "1.1.0",
                ModDependencies = new List<ModDependency>
                {
                    new ModDependency("BepInEx"),
                    new ModDependency("Fancy", "1.0.1")
                },
                ConflictsWith = new List<string>(),
                SourceUrl = "https://thunderstore.io/package/download/BlacktieTeam/Blacktie_Straftat/1.1.0/",
                InstallFileName = "Blacktie_Straftat.zip",
                InstallSubPath = "BepInEx/plugins/CosmeticBundles",
                ExtractTargetSubPath = "CosmeticBundles"
            },
            new ModBase
            {
                ModName = "Straftat Cosmetics Bundle IC",
                ModVersion = "1.0.2",
                ModDependencies = new List<ModDependency>
                {
                    new ModDependency("BepInEx"),
                    new ModDependency("Fancy", "1.0.1")
                },
                ConflictsWith = new List<string>(),
                SourceUrl = "https://thunderstore.io/package/download/ImmortalChickens/Straftat_Cosmetics_Bundle_IC/1.0.2/",
                InstallFileName = "Straftat_Cosmetics_Bundle_IC.zip",
                InstallSubPath = "plugins/CosmeticBundle_IC",
                ExtractTargetSubPath = "CosmeticBundles"
            },
            new ModBase
            {
                ModName = "Custom Levels",
                ModVersion = "1.0.0",
                ModDependencies = new List<ModDependency> { new ModDependency("BepInEx") },
                ConflictsWith = new List<string>(),
                SourceUrl = "https://gcdn.thunderstore.io/live/blob-storage/sha256/c94bf71e32b9afc5743a998fbea7dec938987e1faa74f6684c69eea68b9dd808.sh_YUmU4UE.blob",
                InstallFileName = "Custom_Levels.dll"
            },
            new ModBase
            {
                ModName = "Aboubi Acrobatic",
                ModVersion = "1.1.0",
                ModDependencies = new List<ModDependency> { new ModDependency("BepInEx") },
                ConflictsWith = new List<string>(),
                SourceUrl = "https://gcdn.thunderstore.io/live/blob-storage/sha256/31f32f6ca725a9a68875099a387951136cd16ad1d256603e2bad253e7e1245ec.sh_8Yqb5fw.blob",
                InstallFileName = "Aboubi_Acrobatic.dll"
            },
            new ModBase
            {
                ModName = "FreecamSpectate",
                ModVersion = "1.0.0",
                ModDependencies = new List<ModDependency> { new ModDependency("BepInEx") },
                ConflictsWith = new List<string>(),
                SourceUrl = "https://gcdn.thunderstore.io/live/blob-storage/sha256/9455b3865ca8986a3af3618e3c4ef938e45bec978c237d3e17fbd32d3bc84539.sh_IqTHhn8.blob",
                InstallFileName = "FreecamSpectate.dll"
            }
        };

        private static readonly Dictionary<string, string> InstalledMods = new Dictionary<string, string>();
        private static string? _gamePath;
        private static bool _backupDone = false;
        public static IEnumerable<string> GetInstalledModNames()
            => InstalledMods.Keys;

        public static void Entry(
            string? preselectedGamePath = null,
            List<string>? commandQueue = null,
            List<string>? removeQueue = null,
            bool autoYes = false,
            bool isCli = false  // CLIフラグ追加
        )
        {
            _backupDone = false;
            _gamePath = preselectedGamePath ?? Program.CurrentGamePath;

            if (!isCli)
            {
                ConsoleUI.Header("STRAFTAT Mod Installer");  // インタラクティブ時のみヘッダー
            }

            // CLI時はpath入力スキップ（preselectedGamePath or CurrentGamePath使用）
            if (!isCli && string.IsNullOrEmpty(_gamePath))
            {
                ConsoleUI.Prompt(Messages.Get("EnterGamePath"));
                _gamePath = Console.ReadLine()?.Trim();
            }

            if (!BasicUtils.IsValidPath(_gamePath) || !File.Exists(Path.Combine(_gamePath!, "STRAFTAT.exe")))
            {
                if (!isCli) ConsoleUI.Error(Messages.Get("InvalidGamePath"));
                return;
            }

            if (!isCli)
            {
                ConsoleUI.Success($"Game found: {_gamePath}");
            }
    
            ScanInstalledMods(_gamePath!);

            // CLI時はバックアップ/ヘッダー/プロンプト全スキップ → 直接処理
            if (isCli)
            {
                if (removeQueue != null && removeQueue.Count > 0)
                {
                    foreach (var modName in removeQueue.Distinct())  // 重複除去
                    {
                        var mod = ModsList.FirstOrDefault(m => m.ModName == modName);
                        if (mod.ModName == null) continue;
                        RemoveMod(mod, _gamePath!, autoYes);
                    }
                }
                else if (commandQueue != null && commandQueue.Count > 0)
                {
                    foreach (var cmd in commandQueue)
                        ProcessModInput(cmd, autoYes);
                }
                return;
            }
            
            if (removeQueue != null && removeQueue.Count > 0)
            {
                foreach (var modName in removeQueue)
                {
                    var mod = ModsList.FirstOrDefault(m => m.ModName == modName);
                    if (mod.ModName == null)
                    {
                        ConsoleUI.Error(string.Format(Messages.Get("RemoveNotFound"), modName));
                        continue;
                    }
                    RemoveMod(mod, _gamePath!, autoYes);
                }
                if (commandQueue == null) return;
            }

            // commandQueueの処理
            if (commandQueue != null && commandQueue.Count > 0)
            {
                EnsureBackup(autoYes);
                foreach (var cmd in commandQueue)
                    ProcessModInput(cmd, autoYes: autoYes);
                return;
            }

            // 対話モード: Install or Uninstall を選択
            RunInteractiveMode();
        }

        /// <summary>
        /// Install / Uninstall モード選択 → 各モードのループ → exitで戻る
        /// </summary>
        private static void RunInteractiveMode()
        {
            while (true)
            {
                ConsoleUI.Divider();
                ConsoleUI.Info(Messages.Get("SelectMode"));
                ConsoleUI.Info("  · install");
                ConsoleUI.Info("  · uninstall");
                ConsoleUI.Divider();
                ConsoleUI.Prompt(Messages.Get("ModePrompt"));
                var mode = Console.ReadLine()?.Trim().ToLower();

                if (mode == "exit")
                    return;

                switch (mode)
                {
                    case "install":
                        RunInstallLoop();
                        break;
                    case "uninstall":
                        RunUninstallLoop();
                        break;
                    default:
                        ConsoleUI.Error(Messages.Get("InvalidMode"));
                        break;
                }
            }
        }

        private static void RunInstallLoop()
        {
            ConsoleUI.Header("Install Mode");
            while (true)
            {
                PrintModList();
                ConsoleUI.Info(Messages.Get("InstallExitHint"));
                ConsoleUI.Prompt(Messages.Get("EnterModName"));
                var userInput = Console.ReadLine()?.Trim();

                if (userInput?.ToLower() == "exit")
                    return;

                ProcessModInput(userInput, autoYes: false);
            }
        }

        private static void RunUninstallLoop()
        {
            ConsoleUI.Header("Uninstall Mode");
            while (true)
            {
                PrintModList();
                ConsoleUI.Info(Messages.Get("UninstallExitHint"));
                ConsoleUI.Prompt(Messages.Get("EnterModName"));
                var userInput = Console.ReadLine()?.Trim();

                if (userInput?.ToLower() == "exit")
                    return;

                ProcessRemoveInput(userInput, autoYes: false);
            }
        }

        /// <summary>
        /// バックアップを一度だけ実行する
        /// </summary>
        private static void EnsureBackup(bool autoYes)
        {
            if (_backupDone) return;
            BackupUtils.TryBackup(_gamePath!, autoYes: autoYes);
            _backupDone = true;
        }

        private static void PrintModList()
        {
            ScanInstalledMods(_gamePath!);
            ConsoleUI.Divider();
            ConsoleUI.Info(Messages.Get("UsableMods"));
            foreach (var mod in ModsList)
            {
                var deps = mod.ModDependencies != null && mod.ModDependencies.Count > 0
                    ? string.Join(", ", mod.ModDependencies)
                    : "";
                var conflicts = mod.ConflictsWith != null && mod.ConflictsWith.Count > 0
                    ? $"  [conflicts: {string.Join(", ", mod.ConflictsWith)}]"
                    : "";
                var installedMark = InstalledMods.ContainsKey(mod.ModName) ? " ✓" : "";
                ConsoleUI.ModEntry(mod.ModName + installedMark, mod.ModVersion, deps + conflicts);
            }
            ConsoleUI.Divider();
        }

        private static void ProcessModInput(string? userInput, bool autoYes)
        {
            if (string.IsNullOrEmpty(userInput)) return;

            if (userInput.ToLower() == "@all")
            {
                ConsoleUI.Info("Installing all mods...");
                foreach (var mod in ModsList)
                    TryInstallMod(mod, autoYes);
                return;
            }

            var selectedMod = ModsList.FirstOrDefault(m => m.ModName == userInput);
            if (selectedMod.ModName == null)
            {
                ConsoleUI.Error(Messages.Get("InvalidModName"));
                return;
            }

            TryInstallMod(selectedMod, autoYes);
        }
        
        private static void ProcessRemoveInput(string? userInput, bool autoYes)
        {
            if (string.IsNullOrEmpty(userInput)) return;

            if (userInput.ToLower() == "@all")
            {
                ConsoleUI.Info("Removing all installed mods...");

                // 今入っているものだけ対象にする
                var installedNames = InstalledMods.Keys.ToList();
                if (installedNames.Count == 0)
                {
                    ConsoleUI.Warn("No mods are currently installed.");
                    return;
                }

                foreach (var name in installedNames)
                {
                    var mod = ModsList.FirstOrDefault(m => m.ModName == name);
                    if (mod.ModName == null)
                    {
                        // ModsListに無いけどInstalledModsに残っているゴミの場合
                        ConsoleUI.Warn($"{name} is marked as installed but not found in ModsList. Skipped.");
                        continue;
                    }

                    RemoveMod(mod, _gamePath!, autoYes);
                }

                return;
            }

            var selectedMod = ModsList.FirstOrDefault(m => m.ModName == userInput);
            if (selectedMod.ModName == null)
            {
                ConsoleUI.Error(string.Format(Messages.Get("RemoveNotFound"), userInput));
                return;
            }

            RemoveMod(selectedMod, _gamePath!, autoYes);
        }
        
        private static void ScanInstalledMods(string gamePath)
        {
            InstalledMods.Clear();
            var pluginsDir = Path.Combine(gamePath, "BepInEx", "plugins");
            if (!Directory.Exists(pluginsDir)) return;

            foreach (var mod in ModsList)
            {
                var fileName = mod.InstallFileName ?? Path.GetFileName(mod.SourceUrl);
                var isZip = fileName?.EndsWith(".zip", StringComparison.OrdinalIgnoreCase) == true;

                bool installed = false;
                if (isZip)
                {
                    if (!string.IsNullOrEmpty(mod.ExtractTargetSubPath))
                    {
                        var targetDir = Path.Combine(pluginsDir, mod.ExtractTargetSubPath);
                        installed = Directory.Exists(targetDir);
                    }
                }
                else if (!string.IsNullOrEmpty(fileName))
                {
                    var filePath = Path.Combine(pluginsDir, fileName);
                    installed = File.Exists(filePath);
                }

                if (installed && mod.ModName != "BepInEx")  // BepInExは別途判定
                    InstalledMods[mod.ModName] = mod.ModVersion;
            }

            // BepInExはdoorstop_versionで判定
            if (File.Exists(Path.Combine(gamePath, ".doorstop_version")))
                InstalledMods["BepInEx"] = ModsList.First(m => m.ModName == "BepInEx").ModVersion;
        }

        private static void TryInstallMod(ModBase selectedMod, bool autoYes)
        {
            if (InstalledMods.ContainsKey(selectedMod.ModName))
            {
                ConsoleUI.Info($"{selectedMod.ModName} is already installed. Skipped.");
                return;
            }
            var exclusions = CheckExclusions(selectedMod);
            if (exclusions.Count > 0)
            {
                ConsoleUI.Error("Exclusion conflicts detected:");
                foreach (var e in exclusions)
                    ConsoleUI.Error($"  x {e}");

                if (autoYes)
                {
                    ConsoleUI.Warn("Skipping due to exclusion conflict.");
                    return;
                }

                ConsoleUI.Prompt("Continue anyway? (y/n) > ");
                var ans = Console.ReadLine()?.Trim().ToLower();
                if (ans != "y")
                {
                    ConsoleUI.Warn("Installation cancelled.");
                    return;
                }
            }

            var conflicts = CheckConflicts(selectedMod, new List<string>());
            if (conflicts.Count > 0)
            {
                ConsoleUI.Warn("Version conflicts detected:");
                foreach (var c in conflicts)
                    ConsoleUI.Warn($"  ! {c}");

                if (!autoYes)
                {
                    ConsoleUI.Prompt("Continue anyway? (y/n) > ");
                    var ans = Console.ReadLine()?.Trim().ToLower();
                    if (ans != "y")
                    {
                        ConsoleUI.Warn("Installation cancelled.");
                        return;
                    }
                }
            }

            try
            {
                InstallWithDependencies(selectedMod, _gamePath!);
                ConsoleUI.Success($"✓ {selectedMod.ModName} and its dependencies installed successfully.");
            }
            catch (Exception ex)
            {
                ConsoleUI.Error(Messages.Get("InstallFailed") + ex.Message);
            }
        }

        private static List<string> CheckExclusions(ModBase mod)
        {
            var result = new List<string>();
            if (mod.ConflictsWith == null) return result;

            foreach (var conflictName in mod.ConflictsWith)
            {
                if (InstalledMods.ContainsKey(conflictName))
                    result.Add($"{mod.ModName} conflicts with already installed mod '{conflictName}'.");
            }
            return result;
        }

        private static List<string> CheckConflicts(ModBase mod, List<string> visited)
        {
            var conflicts = new List<string>();

            if (visited.Contains(mod.ModName))
                return conflicts;
            visited.Add(mod.ModName);

            if (mod.ModDependencies == null)
                return conflicts;

            foreach (var dep in mod.ModDependencies)
            {
                var depMod = ModsList.FirstOrDefault(m => m.ModName == dep.ModName);
                if (depMod.ModName == null)
                    continue;

                if (dep.RequiredVersion != null
                    && InstalledMods.TryGetValue(dep.ModName, out var installedVer)
                    && installedVer != dep.RequiredVersion)
                {
                    conflicts.Add($"{mod.ModName} requires {dep.ModName}@{dep.RequiredVersion} but {installedVer} is already installed.");
                }

                if (dep.RequiredVersion != null && depMod.ModVersion != dep.RequiredVersion)
                {
                    conflicts.Add($"{mod.ModName} requires {dep.ModName}@{dep.RequiredVersion} but only {depMod.ModVersion} is available.");
                }

                conflicts.AddRange(CheckConflicts(depMod, visited));
            }

            return conflicts;
        }

        private static void InstallWithDependencies(ModBase mod, string gamePath)
        {
            if (InstalledMods.ContainsKey(mod.ModName))
                return;

            if (mod.ModDependencies != null)
            {
                foreach (var dep in mod.ModDependencies)
                {
                    var depMod = ModsList.FirstOrDefault(m => m.ModName == dep.ModName);
                    if (depMod.ModName == null)
                        throw new InvalidOperationException($"Dependency '{dep.ModName}' not found for mod '{mod.ModName}'.");

                    InstallWithDependencies(depMod, gamePath);
                }
            }

            InstallMod(mod, gamePath);
            InstalledMods[mod.ModName] = mod.ModVersion;
        }

        private static void InstallMod(ModBase mod, string gamePath)
        {
            ConsoleUI.Info($"Installing {mod.ModName}@{mod.ModVersion} ...");

            if (mod.ModName == "BepInEx")
            {
                var ok = BepInExInstallUtils.Install(gamePath, mod.ModVersion);
                if (!ok)
                {
                    ConsoleUI.Error("Failed to install BepInEx.");
                    throw new Exception("BepInEx install failed.");
                }
                ConsoleUI.Success($"✓ Installed {mod.ModName}.");
                return;
            }

            if (string.IsNullOrEmpty(mod.SourceUrl))
            {
                ConsoleUI.Error($"No SourceURL defined for {mod.ModName}.");
                throw new Exception($"SourceURL is missing for mod '{mod.ModName}'.");
            }

            var pluginsDir = Path.Combine(gamePath, "BepInEx", "plugins");
            Directory.CreateDirectory(pluginsDir);

            var fileName = mod.InstallFileName ?? Path.GetFileName(mod.SourceUrl);
            var isZip = fileName.EndsWith(".zip", StringComparison.OrdinalIgnoreCase);
            var tmpFile = Path.Combine(Path.GetTempPath(), $"{mod.ModName}_{fileName}");

            if (!isZip)
            {
                var finalDst = Path.Combine(pluginsDir, fileName);
                if (File.Exists(finalDst))
                {
                    ConsoleUI.Prompt(Messages.Get("OverwriteAsk"));
                    var ans = Console.ReadLine()?.Trim().ToLower();
                    if (ans != "y")
                    {
                        ConsoleUI.Warn(Messages.Get("OverwriteSkip"));
                        return;
                    }
                }
            }

            try
            {
                using var client = new WebClient();
                ConsoleUI.Info($"Downloading {mod.ModName} from {mod.SourceUrl} ...");
                client.DownloadFile(mod.SourceUrl, tmpFile);

                if (isZip)
                {
                    var extractTarget = string.IsNullOrEmpty(mod.ExtractTargetSubPath)
                        ? pluginsDir
                        : Path.Combine(pluginsDir, mod.ExtractTargetSubPath);

                    Directory.CreateDirectory(extractTarget);
                    ConsoleUI.Info($"Extracting {mod.ModName} to {extractTarget} ...");
                    BasicUtils.ExtractOverwrite(tmpFile, extractTarget, subPath: mod.InstallSubPath);
                }
                else
                {
                    var finalDst = Path.Combine(pluginsDir, fileName);
                    File.Copy(tmpFile, finalDst, overwrite: true);
                }
            }
            catch (Exception ex)
            {
                ConsoleUI.Error($"{mod.ModName} installation failed:");
                Console.WriteLine(ex);
                throw;
            }
            finally
            {
                if (File.Exists(tmpFile))
                    File.Delete(tmpFile);
            }

            ConsoleUI.Success($"✓ Installed {mod.ModName}.");
        }

        private static void RemoveMod(ModBase mod, string gamePath, bool autoYes)
        {
            if (mod.ModName == "BepInEx")
            {
                if (!autoYes)
                {
                    ConsoleUI.Prompt(string.Format(Messages.Get("RemoveAsk"), "BepInEx"));
                    var ans = Console.ReadLine()?.Trim().ToLower();
                    if (ans != "y")
                    {
                        ConsoleUI.Warn(Messages.Get("RemoveSkip"));
                        return;
                    }
                }

                var ok = BepInExInstallUtils.Uninstall(gamePath);
                if (ok)
                {
                    InstalledMods.Remove(mod.ModName);
                    ConsoleUI.Success(string.Format(Messages.Get("RemoveSuccess"), "BepInEx"));
                }
                else
                {
                    ConsoleUI.Error(string.Format(Messages.Get("RemoveFailed"), "BepInEx") + "See above for details.");
                }
                return;
            }

            if (!autoYes)
            {
                ConsoleUI.Prompt(string.Format(Messages.Get("RemoveAsk"), mod.ModName));
                var ans = Console.ReadLine()?.Trim().ToLower();
                if (ans != "y")
                {
                    ConsoleUI.Warn(Messages.Get("RemoveSkip"));
                    return;
                }
            }

            try
            {
                var pluginsDir = Path.Combine(gamePath, "BepInEx", "plugins");
                var removed = false;

                var fileName = mod.InstallFileName ?? Path.GetFileName(mod.SourceUrl);
                var isZip = fileName?.EndsWith(".zip", StringComparison.OrdinalIgnoreCase) == true;

                if (isZip)
                {
                    if (!string.IsNullOrEmpty(mod.ExtractTargetSubPath))
                    {
                        var targetDir = Path.Combine(pluginsDir, mod.ExtractTargetSubPath);
                        if (Directory.Exists(targetDir))
                        {
                            Directory.Delete(targetDir, recursive: true);
                            removed = true;
                        }
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(fileName))
                    {
                        var filePath = Path.Combine(pluginsDir, fileName);
                        if (File.Exists(filePath))
                        {
                            File.Delete(filePath);
                            removed = true;
                        }
                    }
                }

                if (removed)
                {
                    InstalledMods.Remove(mod.ModName);
                    ConsoleUI.Success(string.Format(Messages.Get("RemoveSuccess"), mod.ModName));
                }
                else
                {
                    ConsoleUI.Warn(string.Format(Messages.Get("RemoveNotFound"), mod.ModName));
                }
            }
            catch (Exception ex)
            {
                ConsoleUI.Error(string.Format(Messages.Get("RemoveFailed"), mod.ModName) + ex.Message);
            }
        }
    }
}