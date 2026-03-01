using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using SlafightInstaller.Utils;

namespace SlafightInstaller.Games
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
                SourceUrl = null,
                InstallSubPath = null,
                ExtractTargetSubPath = null,
                InstallFileName = null,
                FinalPath = null
            },
            // ========= Thunderstore DLL系 (plugins直下) =========
            new ModBase
            {
                ModName = "ModMenu",
                ModVersion = "1.1.3",
                ModDependencies = new List<ModDependency> { new ModDependency("BepInEx") },
                ConflictsWith = new List<string>(),
                SourceUrl = "https://thunderstore.io/package/download/kestrel/Mod_Menu/1.1.3/",
                InstallFileName = "ModMenu.zip",
                InstallSubPath = "BepInEx/plugins",
                ExtractTargetSubPath = null,
                FinalPath = null
            },
            new ModBase
            {
                ModName = "Chat Commands",
                ModVersion = "1.2.7",
                ModDependencies = new List<ModDependency> { new ModDependency("BepInEx") },
                ConflictsWith = new List<string>(),
                SourceUrl = "https://thunderstore.io/package/download/kestrel/Chat_Commands/1.2.7/",
                InstallFileName = "Chat_Commands.zip",
                InstallSubPath = "BepInEx/plugins",
                ExtractTargetSubPath = null,
                FinalPath = null
            },
            new ModBase
            {
                ModName = "moreStrafts",
                ModVersion = "0.0.4",
                ModDependencies = new List<ModDependency> { new ModDependency("BepInEx") },
                ConflictsWith = new List<string>(),
                SourceUrl = "https://github.com/ALBINALSHAIKH/moreStrafts/releases/download/v0.0.4/moreStrafts.dll",
                InstallSubPath = null,
                ExtractTargetSubPath = null,
                InstallFileName = null,
                FinalPath = null
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
                SourceUrl = "https://thunderstore.io/package/download/Yeastmans/MoreStrafts_UISpawnAddon/1.0.5/",
                InstallFileName = "MoreStrafts_UISpawnAddon.zip",
                InstallSubPath = null,
                ExtractTargetSubPath = null,
                FinalPath = null
            },
            new ModBase
            {
                ModName = "Fancy",
                ModVersion = "1.0.1",
                ModDependencies = new List<ModDependency> { new ModDependency("BepInEx") },
                ConflictsWith = new List<string>(),
                SourceUrl = "https://thunderstore.io/package/download/kestrel/Fancy/1.0.1/",
                InstallFileName = "Fancy.zip",
                InstallSubPath = null,
                ExtractTargetSubPath = null,
                FinalPath = null
            },
            // ========= Cosmetic Bundles (CosmeticBundlesフォルダ) =========
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
                ExtractTargetSubPath = "CosmeticBundles",
                FinalPath = "CosmeticBundles/JetRhino.BlackTieStraftat.cbundle"
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
                ExtractTargetSubPath = "CosmeticBundles",
                FinalPath = "CosmeticBundles/ic.ChickensStraftatBundle.cbundle"
            },
            // ========= その他DLL系 =========
            new ModBase
            {
                ModName = "Custom Levels",
                ModVersion = "1.0.0",
                ModDependencies = new List<ModDependency> { new ModDependency("BepInEx") },
                ConflictsWith = new List<string>(),
                SourceUrl = "https://thunderstore.io/package/download/bro/Custom_Levels/1.0.0/",
                InstallFileName = "Custom_Levels.zip",
                InstallSubPath = "BepInEx/plugins",
                ExtractTargetSubPath = null,
                FinalPath = null
            },
            new ModBase
            {
                ModName = "Aboubi Acrobatic",
                ModVersion = "1.1.0",
                ModDependencies = new List<ModDependency> { new ModDependency("BepInEx") },
                ConflictsWith = new List<string>(),
                SourceUrl = "https://thunderstore.io/package/download/kestrel/Aboubi_Acrobatics/1.1.0/",
                InstallFileName = "Aboubi_Acrobatics.zip",
                InstallSubPath = "BepInEx/plugins",
                ExtractTargetSubPath = null,
                FinalPath = null
            },
            new ModBase
            {
                ModName = "Aboubi Unbound",
                ModVersion = "1.0.0",
                ModDependencies = new List<ModDependency> { new ModDependency("BepInEx") },
                ConflictsWith = new List<string>(),
                SourceUrl = "https://thunderstore.io/package/download/kestrel/Aboubi_Unbound/1.0.0/",
                InstallFileName = "Aboubi_Unbound.zip",
                InstallSubPath = "BepInEx/plugins",
                ExtractTargetSubPath = null,
                FinalPath = null
            },
            new ModBase
            {
                ModName = "FreecamSpectate",
                ModVersion = "1.0.0",
                ModDependencies = new List<ModDependency> { new ModDependency("BepInEx") },
                ConflictsWith = new List<string>(),
                SourceUrl = "https://thunderstore.io/package/download/LeodisTaylor/FreecamSpectate/1.0.0/",
                InstallFileName = "FreecamSpectate.zip",
                InstallSubPath       = null,
                ExtractTargetSubPath = null,
                FinalPath = "FreecamSpectate/FreecamSpectate.dll"
            },
        };

        // ModName → (Version, InstalledFolderName or null)
        private static readonly Dictionary<string, (string Version, string? FolderName)> InstalledMods
            = new Dictionary<string, (string, string?)>();

        private static string? _gamePath;
        private static bool _backupDone = false;
        private static bool _noBackup = false;

        public static IEnumerable<string> GetInstalledModNames()
            => InstalledMods.Keys;

        public static void Entry(
            string? preselectedGamePath = null,
            List<string>? commandQueue = null,
            List<string>? removeQueue = null,
            bool autoYes = false,
            bool noBackup = false,
            bool isCli = false
        )
        {
            _backupDone = false;
            _noBackup = noBackup;
            _gamePath = preselectedGamePath ?? Program.CurrentGamePath;

            if (!isCli)
                ConsoleUI.Header("STRAFTAT Mod Installer");

            // カスタムMODをModsListにマージ（既存MODは上書き）
            foreach (var custom in CustomModRegistry.CustomMods)
            {
                var idx = ModsList.FindIndex(m => m.ModName == custom.ModName);
                if (idx >= 0)
                    ModsList[idx] = custom;
                else
                    ModsList.Add(custom);
            }

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
                ConsoleUI.Success($"Game found: {_gamePath}");

            ScanInstalledMods(_gamePath!);

            if (isCli)
            {
                if (removeQueue != null && removeQueue.Count > 0)
                {
                    foreach (var modName in removeQueue.Distinct())
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

            if (commandQueue != null && commandQueue.Count > 0)
            {
                EnsureBackup(autoYes);
                foreach (var cmd in commandQueue)
                    ProcessModInput(cmd, autoYes: autoYes);
                return;
            }

            RunInteractiveMode();
        }

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

                if (mode == "exit") return;

                switch (mode)
                {
                    case "install":   RunInstallLoop();   break;
                    case "uninstall": RunUninstallLoop(); break;
                    default: ConsoleUI.Error(Messages.Get("InvalidMode")); break;
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
                if (userInput?.ToLower() == "exit") return;
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
                if (userInput?.ToLower() == "exit") return;
                ProcessRemoveInput(userInput, autoYes: false);
            }
        }

        private static void EnsureBackup(bool autoYes)
        {
            if (_backupDone || _noBackup) return;
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
                    ? string.Join(", ", mod.ModDependencies) : "";
                var conflicts = mod.ConflictsWith != null && mod.ConflictsWith.Count > 0
                    ? $"  [conflicts: {string.Join(", ", mod.ConflictsWith)}]" : "";
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
                EnsureBackup(autoYes);
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

            EnsureBackup(autoYes);
            TryInstallMod(selectedMod, autoYes);
        }

        private static void ProcessRemoveInput(string? userInput, bool autoYes)
        {
            if (string.IsNullOrEmpty(userInput)) return;

            if (userInput.ToLower() == "@all")
            {
                ConsoleUI.Info("Removing all installed mods...");
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
            // Clear前にインストール時記録済みのFolderNameを退避
            var knownFolders = InstalledMods
                .Where(kv => kv.Value.FolderName != null)
                .ToDictionary(kv => kv.Key, kv => kv.Value.FolderName!);

            InstalledMods.Clear();
            var pluginsDir = Path.Combine(gamePath, "BepInEx", "plugins");
            if (!Directory.Exists(pluginsDir)) return;

            foreach (var mod in ModsList)
            {
                if (mod.ModName == "BepInEx") continue;

                bool installed = false;
                string? foundFolder = null;

                // FinalPath が設定されている場合はそれを最優先で判定
                if (!string.IsNullOrEmpty(mod.FinalPath))
                {
                    var finalFull = Path.Combine(pluginsDir, mod.FinalPath);
                    if (File.Exists(finalFull) || Directory.Exists(finalFull))
                    {
                        installed = true;

                        var dir = Path.GetDirectoryName(finalFull);
                        if (!string.IsNullOrEmpty(dir))
                        {
                            var dirName = Path.GetFileName(dir);
                            if (!string.Equals(dirName, "plugins", StringComparison.OrdinalIgnoreCase))
                                foundFolder = dirName;
                        }
                    }
                }
                else
                {
                    var originalName = mod.InstallFileName ?? Path.GetFileName(mod.SourceUrl);
                    var isZip = !string.IsNullOrEmpty(originalName) &&
                                originalName.EndsWith(".zip", StringComparison.OrdinalIgnoreCase);

                    if (isZip)
                    {
                        if (!string.IsNullOrEmpty(mod.ExtractTargetSubPath))
                        {
                            // CosmeticBundles等: そのディレクトリがあればOK
                            installed = Directory.Exists(Path.Combine(pluginsDir, mod.ExtractTargetSubPath));
                        }
                        else if (mod.InstallSubPath == null)
                        {
                            // InstallSubPath=null → ルートから展開 → plugins直下にDLL直置き
                            // 例: Fancy.dll, MoreStrafts_UISpawnAddon.dll
                            var baseName = Path.GetFileNameWithoutExtension(mod.InstallFileName ?? mod.ModName);
                            installed = File.Exists(Path.Combine(pluginsDir, baseName + ".dll"));
                        }
                        else
                        {
                            // InstallSubPath指定あり → plugins直下にフォルダが展開される
                            // インストール時に記録済みのフォルダ名を優先
                            if (knownFolders.TryGetValue(mod.ModName, out var known) &&
                                Directory.Exists(Path.Combine(pluginsDir, known)))
                            {
                                installed = true;
                                foundFolder = known;
                            }
                            else
                            {
                                // 未記録の場合はフォルダ名・DLL名で検索
                                var modKey = mod.ModName.Replace(" ", "");
                                foreach (var dir in Directory.GetDirectories(pluginsDir))
                                {
                                    var dirName = Path.GetFileName(dir);
                                    if (dirName.IndexOf(modKey, StringComparison.OrdinalIgnoreCase) >= 0 ||
                                        Directory.GetFiles(dir, "*.dll").Any(f =>
                                            Path.GetFileNameWithoutExtension(f)
                                                .IndexOf(modKey, StringComparison.OrdinalIgnoreCase) >= 0))
                                    {
                                        installed = true;
                                        foundFolder = dirName;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        // 生DLL (moreStrafts等)
                        var originalName2 = mod.InstallFileName ?? Path.GetFileName(mod.SourceUrl);
                        var baseName = string.IsNullOrEmpty(originalName2)
                            ? mod.ModName
                            : Path.GetFileNameWithoutExtension(originalName2);
                        installed = File.Exists(Path.Combine(pluginsDir, baseName + ".dll"));
                    }
                }

                if (installed)
                    InstalledMods[mod.ModName] = (mod.ModVersion, foundFolder);
            }

            if (File.Exists(Path.Combine(gamePath, ".doorstop_version")))
                InstalledMods["BepInEx"] = (ModsList.First(m => m.ModName == "BepInEx").ModVersion, null);
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
                foreach (var e in exclusions) ConsoleUI.Error($"  x {e}");
                if (autoYes) { ConsoleUI.Warn("Skipping due to exclusion conflict."); return; }
                ConsoleUI.Prompt("Continue anyway? (y/n) > ");
                if (Console.ReadLine()?.Trim().ToLower() != "y") { ConsoleUI.Warn("Installation cancelled."); return; }
            }

            var conflicts = CheckConflicts(selectedMod, new List<string>());
            if (conflicts.Count > 0)
            {
                ConsoleUI.Warn("Version conflicts detected:");
                foreach (var c in conflicts) ConsoleUI.Warn($"  ! {c}");
                if (!autoYes)
                {
                    ConsoleUI.Prompt("Continue anyway? (y/n) > ");
                    if (Console.ReadLine()?.Trim().ToLower() != "y") { ConsoleUI.Warn("Installation cancelled."); return; }
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
                if (InstalledMods.ContainsKey(conflictName))
                    result.Add($"{mod.ModName} conflicts with already installed mod '{conflictName}'.");
            return result;
        }

        private static List<string> CheckConflicts(ModBase mod, List<string> visited)
        {
            var conflicts = new List<string>();
            if (visited.Contains(mod.ModName)) return conflicts;
            visited.Add(mod.ModName);
            if (mod.ModDependencies == null) return conflicts;

            foreach (var dep in mod.ModDependencies)
            {
                var depMod = ModsList.FirstOrDefault(m => m.ModName == dep.ModName);
                if (depMod.ModName == null) continue;

                if (dep.RequiredVersion != null
                    && InstalledMods.TryGetValue(dep.ModName, out var installedInfo)
                    && installedInfo.Version != dep.RequiredVersion)
                    conflicts.Add($"{mod.ModName} requires {dep.ModName}@{dep.RequiredVersion} but {installedInfo.Version} is already installed.");

                if (dep.RequiredVersion != null && depMod.ModVersion != dep.RequiredVersion)
                    conflicts.Add($"{mod.ModName} requires {dep.ModName}@{dep.RequiredVersion} but only {depMod.ModVersion} is available.");

                conflicts.AddRange(CheckConflicts(depMod, visited));
            }
            return conflicts;
        }

        private static void InstallWithDependencies(ModBase mod, string gamePath)
        {
            if (InstalledMods.ContainsKey(mod.ModName)) return;

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
        }

        private static void InstallMod(ModBase mod, string gamePath)
        {
            ConsoleUI.Info($"Installing {mod.ModName}@{mod.ModVersion} ...");

            if (mod.ModName == "BepInEx")
            {
                var ok = BepInExInstallUtils.Install(gamePath, mod.ModVersion);
                if (!ok) { ConsoleUI.Error("Failed to install BepInEx."); throw new Exception("BepInEx install failed."); }
                InstalledMods[mod.ModName] = (mod.ModVersion, null);
                ConsoleUI.Success($"✓ Installed {mod.ModName}.");
                return;
            }

            if (string.IsNullOrEmpty(mod.SourceUrl))
                throw new Exception($"SourceURL is missing for mod '{mod.ModName}'.");

            var pluginsDir = Path.Combine(gamePath, "BepInEx", "plugins");
            Directory.CreateDirectory(pluginsDir);

            var originalName = mod.InstallFileName ?? Path.GetFileName(mod.SourceUrl);
            var isZip = !string.IsNullOrEmpty(originalName) &&
                        originalName.EndsWith(".zip", StringComparison.OrdinalIgnoreCase);
            var tmpName = string.IsNullOrEmpty(originalName)
                ? (isZip ? mod.ModName + ".zip" : mod.ModName + ".dll")
                : originalName;
            var tmpFile = Path.Combine(Path.GetTempPath(), $"{mod.ModName}_{tmpName}");

            try
            {
                using var client = new WebClient();
                client.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64)");
                ConsoleUI.Info($"Downloading {mod.ModName} from {mod.SourceUrl} ...");
                client.DownloadFile(mod.SourceUrl, tmpFile);

                string? newFolder = null;

                if (isZip)
                {
                    var extractTarget = string.IsNullOrEmpty(mod.ExtractTargetSubPath)
                        ? pluginsDir
                        : Path.Combine(pluginsDir, mod.ExtractTargetSubPath);
                    Directory.CreateDirectory(extractTarget);

                    var beforeDirs = Directory.GetDirectories(extractTarget)
                        .Select(Path.GetFileName).ToHashSet();

                    ConsoleUI.Info($"Extracting {mod.ModName} to {extractTarget} ...");
                    BasicUtils.ExtractOverwrite(tmpFile, extractTarget, subPath: mod.InstallSubPath);

                    newFolder = Directory.GetDirectories(extractTarget)
                        .Select(Path.GetFileName)
                        .FirstOrDefault(d => !beforeDirs.Contains(d));
                }
                else
                {
                    var dllName = Path.GetFileNameWithoutExtension(tmpName);
                    if (string.IsNullOrEmpty(dllName)) dllName = mod.ModName;
                    dllName += ".dll";
                    var finalDst = Path.Combine(pluginsDir, dllName);

                    if (File.Exists(finalDst))
                    {
                        ConsoleUI.Prompt(Messages.Get("OverwriteAsk"));
                        if (Console.ReadLine()?.Trim().ToLower() != "y")
                        {
                            ConsoleUI.Warn(Messages.Get("OverwriteSkip"));
                            return;
                        }
                    }

                    ConsoleUI.Info($"Copying {mod.ModName} to {finalDst}");
                    File.Copy(tmpFile, finalDst, overwrite: true);
                }

                // FinalPath が設定されている場合は、ここで存在チェック
                if (!string.IsNullOrEmpty(mod.FinalPath))
                {
                    var finalFull = Path.Combine(pluginsDir, mod.FinalPath);
                    if (!File.Exists(finalFull) && !Directory.Exists(finalFull))
                        throw new Exception($"FinalPath not found after install: {mod.FinalPath}");
                }

                InstalledMods[mod.ModName] = (mod.ModVersion, newFolder);
            }
            catch (Exception ex)
            {
                ConsoleUI.Error($"{mod.ModName} installation failed:");
                Console.WriteLine(ex);
                throw;
            }
            finally
            {
                if (File.Exists(tmpFile)) File.Delete(tmpFile);
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
                    if (Console.ReadLine()?.Trim().ToLower() != "y") { ConsoleUI.Warn(Messages.Get("RemoveSkip")); return; }
                }
                var ok = BepInExInstallUtils.Uninstall(gamePath);
                if (ok) { InstalledMods.Remove(mod.ModName); ConsoleUI.Success(string.Format(Messages.Get("RemoveSuccess"), "BepInEx")); }
                else ConsoleUI.Error(string.Format(Messages.Get("RemoveFailed"), "BepInEx") + "See above for details.");
                return;
            }

            if (!autoYes)
            {
                ConsoleUI.Prompt(string.Format(Messages.Get("RemoveAsk"), mod.ModName));
                if (Console.ReadLine()?.Trim().ToLower() != "y") { ConsoleUI.Warn(Messages.Get("RemoveSkip")); return; }
            }

            try
            {
                var pluginsDir = Path.Combine(gamePath, "BepInEx", "plugins");
                var removed = false;

                var originalName = mod.InstallFileName ?? Path.GetFileName(mod.SourceUrl);
                var isZip = !string.IsNullOrEmpty(originalName) &&
                            originalName.EndsWith(".zip", StringComparison.OrdinalIgnoreCase);

                if (isZip)
                {
                    if (!string.IsNullOrEmpty(mod.ExtractTargetSubPath))
                    {
                        var targetDir = Path.Combine(pluginsDir, mod.ExtractTargetSubPath);
                        if (Directory.Exists(targetDir)) { Directory.Delete(targetDir, recursive: true); removed = true; }
                    }
                    else if (mod.InstallSubPath == null)
                    {
                        // DLL直置き系 (Fancy, MoreStrafts UISpawnAddon等)
                        var baseName = Path.GetFileNameWithoutExtension(mod.InstallFileName ?? mod.ModName);
                        var filePath = Path.Combine(pluginsDir, baseName + ".dll");
                        if (File.Exists(filePath)) { File.Delete(filePath); removed = true; }
                    }
                    else
                    {
                        // フォルダ系: 記録済みフォルダ名を優先、なければ名前検索
                        string? folderName = null;
                        if (InstalledMods.TryGetValue(mod.ModName, out var info))
                            folderName = info.FolderName;

                        if (!string.IsNullOrEmpty(folderName))
                        {
                            var folderPath = Path.Combine(pluginsDir, folderName);
                            if (Directory.Exists(folderPath)) { Directory.Delete(folderPath, recursive: true); removed = true; }
                        }
                        else
                        {
                            var modKey = mod.ModName.Replace(" ", "");
                            foreach (var dir in Directory.GetDirectories(pluginsDir))
                            {
                                var dirName = Path.GetFileName(dir);
                                if (dirName.IndexOf(modKey, StringComparison.OrdinalIgnoreCase) >= 0 ||
                                    Directory.GetFiles(dir, "*.dll").Any(f =>
                                        Path.GetFileNameWithoutExtension(f)
                                            .IndexOf(modKey, StringComparison.OrdinalIgnoreCase) >= 0))
                                {
                                    Directory.Delete(dir, recursive: true);
                                    removed = true;
                                    break;
                                }
                            }
                        }
                    }
                }
                else
                {
                    // 生DLL
                    var originalName2 = mod.InstallFileName ?? Path.GetFileName(mod.SourceUrl);
                    var baseName = string.IsNullOrEmpty(originalName2)
                        ? mod.ModName
                        : Path.GetFileNameWithoutExtension(originalName2);
                    var filePath = Path.Combine(pluginsDir, baseName + ".dll");
                    if (File.Exists(filePath)) { File.Delete(filePath); removed = true; }
                }

                if (removed)
                {
                    InstalledMods.Remove(mod.ModName);
                    ConsoleUI.Success(string.Format(Messages.Get("RemoveSuccess"), mod.ModName));
                }
                else
                    ConsoleUI.Warn(string.Format(Messages.Get("RemoveNotFound"), mod.ModName));
            }
            catch (Exception ex)
            {
                ConsoleUI.Error(string.Format(Messages.Get("RemoveFailed"), mod.ModName) + ex.Message);
            }
        }
    }
}