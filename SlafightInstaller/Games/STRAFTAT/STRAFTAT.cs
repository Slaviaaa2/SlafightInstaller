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
                ExtractTargetSubPath = null
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
                ExtractTargetSubPath = null
            },
            new ModBase
            {
                ModName = "moreStrafts",
                ModVersion = "0.0.4",
                ModDependencies = new List<ModDependency> { new ModDependency("BepInEx") },
                ConflictsWith = new List<string>(),
                // GitHubはDLL直リンクなのでそのまま
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
                SourceUrl = "https://thunderstore.io/package/download/Yeastmans/MoreStrafts_UISpawnAddon/1.0.5/",
                InstallFileName = "MoreStrafts_UISpawnAddon.zip",
                InstallSubPath = null,
                ExtractTargetSubPath = null
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
                ExtractTargetSubPath = null
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
                // ZIP内: BlacktieStraftat/BepInEx/plugins/CosmeticBundles/...
                InstallSubPath = "BlacktieStraftat/BepInEx/plugins/CosmeticBundles",
                // 展開先: BepInEx/plugins/CosmeticBundles/...
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
                // ZIP内: Straftat_Cosmetics_Bundle_IC/plugins/CosmeticBundle_IC/...
                InstallSubPath = "plugins/CosmeticBundle_IC",
                ExtractTargetSubPath = "CosmeticBundles"
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
                ExtractTargetSubPath = null
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
                ExtractTargetSubPath = null
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
                ExtractTargetSubPath = null
            },
            new ModBase
            {
                ModName = "FreecamSpectate",
                ModVersion = "1.0.0",
                ModDependencies = new List<ModDependency> { new ModDependency("BepInEx") },
                ConflictsWith = new List<string>(),
                SourceUrl = "https://thunderstore.io/package/download/LeodisTaylor/FreecamSpectate/1.0.0/",
                InstallFileName = "FreecamSpectate.zip",
                // README: FreecamSpectate フォルダごと plugins に入れろと書いてある
                // ZIP内: FreecamSpectate/FreecamSpectate.dll → フォルダごとplugins配下に置く
                InstallSubPath = null,                // ルートから全部
                ExtractTargetSubPath = null           // plugins/FreecamSpectate/...
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
                if (mod.ModName == "BepInEx")
                    continue;

                bool installed = false;

                var originalName = mod.InstallFileName ?? Path.GetFileName(mod.SourceUrl);
                var isZip = !string.IsNullOrEmpty(originalName) &&
                            originalName.EndsWith(".zip", StringComparison.OrdinalIgnoreCase);

                if (isZip)
                {
                    if (!string.IsNullOrEmpty(mod.ExtractTargetSubPath))
                    {
                        // ZIP＋ExtractTargetSubPathあり → そのディレクトリがあればインストール済み扱い
                        var targetDir = Path.Combine(pluginsDir, mod.ExtractTargetSubPath);
                        installed = Directory.Exists(targetDir);
                    }
                    else
                    {
                        // ZIP＋SubPathなし → Mod名でDLL検索
                        var dllPattern = mod.ModName + "*.dll";
                        installed = Directory.GetFiles(pluginsDir, dllPattern, SearchOption.AllDirectories).Length > 0;
                    }
                }
                else
                {
                    // 生DLL: InstallModのDLL名ロジックを再現
                    var baseName = string.IsNullOrEmpty(originalName)
                        ? mod.ModName
                        : Path.GetFileNameWithoutExtension(originalName);
                    var dllName = baseName + ".dll";

                    var filePath = Path.Combine(pluginsDir, dllName);
                    installed = File.Exists(filePath);
                }

                if (installed)
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

            // 元の名前（InstallFileName優先、なければURL末尾）
            var originalName = mod.InstallFileName ?? Path.GetFileName(mod.SourceUrl);

            // zip判定は originalName でだけ行う
            var isZip = !string.IsNullOrEmpty(originalName) &&
                        originalName.EndsWith(".zip", StringComparison.OrdinalIgnoreCase);

            // 一時ファイル名（なければ拡張子付きで補完）
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

                if (isZip)
                {
                    // ZIP: ExtractTargetSubPath があればそこ、なければ plugins 直下
                    var extractTarget = string.IsNullOrEmpty(mod.ExtractTargetSubPath)
                        ? pluginsDir
                        : Path.Combine(pluginsDir, mod.ExtractTargetSubPath);

                    Directory.CreateDirectory(extractTarget);
                    ConsoleUI.Info($"Extracting {mod.ModName} to {extractTarget} ...");
                    BasicUtils.ExtractOverwrite(tmpFile, extractTarget, subPath: mod.InstallSubPath);
                }
                else
                {
                    // DLL: DLL名を整えて plugins 直下へ
                    var dllName = Path.GetFileNameWithoutExtension(tmpName);
                    if (string.IsNullOrEmpty(dllName))
                        dllName = mod.ModName;
                    dllName += ".dll";

                    var finalDst = Path.Combine(pluginsDir, dllName);

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

                    ConsoleUI.Info($"Copying {mod.ModName} to {finalDst}");
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

                var originalName = mod.InstallFileName ?? Path.GetFileName(mod.SourceUrl);
                var isZip = !string.IsNullOrEmpty(originalName) &&
                            originalName.EndsWith(".zip", StringComparison.OrdinalIgnoreCase);

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
                    else
                    {
                        // SubPathなしZIP → Mod名DLLを削除
                        var dllPattern = mod.ModName + "*.dll";
                        var files = Directory.GetFiles(pluginsDir, dllPattern, SearchOption.AllDirectories);
                        foreach (var f in files)
                        {
                            File.Delete(f);
                            removed = true;
                        }
                    }
                }
                else
                {
                    var baseName = string.IsNullOrEmpty(originalName)
                        ? mod.ModName
                        : Path.GetFileNameWithoutExtension(originalName);
                    var dllName = baseName + ".dll";
                    var filePath = Path.Combine(pluginsDir, dllName);
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                        removed = true;
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