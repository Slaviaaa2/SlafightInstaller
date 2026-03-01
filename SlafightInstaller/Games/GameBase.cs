using System;
using System.Collections.Generic;
using System.Linq;
using SlafightInstaller.Utils;

namespace SlafightInstaller.Games
{
    public abstract class GameBase
    {
        public abstract string GameId { get; }          // 例: "STRAFTAT"
        public abstract string DisplayName { get; }     // 例: "STRAFTAT"
        public abstract string ExecutableName { get; }  // 例: "STRAFTAT.exe"

        protected string? GamePath { get; private set; }
        protected bool BackupDone { get; private set; }
        protected bool NoBackup { get; private set; }

        // 各ゲーム固有の ModList
        public abstract List<ModBase> ModsList { get; }

        // インストール済み管理（共通）
        protected readonly Dictionary<string, (string Version, string? FolderName)>
            InstalledMods = new Dictionary<string, (string, string?)>();

        public IEnumerable<string> GetInstalledModNames() => InstalledMods.Keys;

        public void Entry(
            string? preselectedGamePath = null,
            List<string>? commandQueue = null,
            List<string>? removeQueue = null,
            bool autoYes = false,
            bool noBackup = false,
            bool isCli = false
        )
        {
            BackupDone = false;
            NoBackup = noBackup;
            GamePath = preselectedGamePath ?? Program.CurrentGamePath;

            if (!isCli)
                ConsoleUI.Header($"{DisplayName} Mod Installer");

            // カスタムMODマージ（ゲーム固有ロジックに任せてもよい）
            MergeCustomMods();

            if (!isCli && string.IsNullOrEmpty(GamePath))
            {
                ConsoleUI.Prompt(Messages.Get("EnterGamePath"));
                GamePath = Console.ReadLine()?.Trim();
            }

            if (!IsValidGamePath(GamePath))
            {
                if (!isCli) ConsoleUI.Error(Messages.Get("InvalidGamePath"));
                return;
            }

            if (!isCli)
                ConsoleUI.Success($"Game found: {GamePath}");

            ScanInstalledModsInternal();

            if (isCli)
            {
                if (removeQueue != null && removeQueue.Count > 0)
                {
                    foreach (var modName in removeQueue.Distinct())
                    {
                        var mod = ModsList.Find(m => m.ModName == modName);
                        if (mod.ModName == null) continue;
                        RemoveModInternal(mod, autoYes);
                    }
                }
                else if (commandQueue != null && commandQueue.Count > 0)
                {
                    foreach (var cmd in commandQueue)
                        ProcessModInputInternal(cmd, autoYes);
                }
                return;
            }

            if (removeQueue != null && removeQueue.Count > 0)
            {
                foreach (var modName in removeQueue)
                {
                    var mod = ModsList.Find(m => m.ModName == modName);
                    if (mod.ModName == null)
                    {
                        ConsoleUI.Error(string.Format(Messages.Get("RemoveNotFound"), modName));
                        continue;
                    }
                    RemoveModInternal(mod, autoYes);
                }
                if (commandQueue == null) return;
            }

            if (commandQueue != null && commandQueue.Count > 0)
            {
                EnsureBackupInternal(autoYes);
                foreach (var cmd in commandQueue)
                    ProcessModInputInternal(cmd, autoYes);
                return;
            }

            RunInteractiveModeInternal();
        }

        protected virtual void MergeCustomMods()
        {
            foreach (var custom in CustomModRegistry.CustomMods)
            {
                var idx = ModsList.FindIndex(m => m.ModName == custom.ModName);
                if (idx >= 0)
                    ModsList[idx] = custom;
                else
                    ModsList.Add(custom);
            }
        }

        protected virtual bool IsValidGamePath(string? path)
        {
            if (!BasicUtils.IsValidPath(path)) return false;
            var exe = System.IO.Path.Combine(path!, ExecutableName);
            return System.IO.File.Exists(exe);
        }

        protected void EnsureBackupInternal(bool autoYes)
        {
            if (BackupDone || NoBackup) return;
            BackupUtils.TryBackup(GamePath!, autoYes: autoYes);
            BackupDone = true;
        }

        protected void RunInteractiveModeInternal()
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
                    case "install":   RunInstallLoopInternal();   break;
                    case "uninstall": RunUninstallLoopInternal(); break;
                    default: ConsoleUI.Error(Messages.Get("InvalidMode")); break;
                }
            }
        }

        protected void RunInstallLoopInternal()
        {
            ConsoleUI.Header("Install Mode");
            while (true)
            {
                PrintModListInternal();
                ConsoleUI.Info(Messages.Get("InstallExitHint"));
                ConsoleUI.Prompt(Messages.Get("EnterModName"));
                var userInput = Console.ReadLine()?.Trim();
                if (userInput?.ToLower() == "exit") return;
                ProcessModInputInternal(userInput, autoYes: false);
            }
        }

        protected void RunUninstallLoopInternal()
        {
            ConsoleUI.Header("Uninstall Mode");
            while (true)
            {
                PrintModListInternal();
                ConsoleUI.Info(Messages.Get("UninstallExitHint"));
                ConsoleUI.Prompt(Messages.Get("EnterModName"));
                var userInput = Console.ReadLine()?.Trim();
                if (userInput?.ToLower() == "exit") return;
                ProcessRemoveInputInternal(userInput, autoYes: false);
            }
        }

        protected void PrintModListInternal()
        {
            ScanInstalledModsInternal();
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

        protected void ProcessModInputInternal(string? userInput, bool autoYes)
        {
            if (string.IsNullOrEmpty(userInput)) return;

            if (userInput.ToLower() == "@all")
            {
                ConsoleUI.Info("Installing all mods...");
                EnsureBackupInternal(autoYes);
                foreach (var mod in ModsList)
                    TryInstallModInternal(mod, autoYes);
                return;
            }

            var selectedMod = ModsList.Find(m => m.ModName == userInput);
            if (selectedMod.ModName == null)
            {
                ConsoleUI.Error(Messages.Get("InvalidModName"));
                return;
            }

            EnsureBackupInternal(autoYes);
            TryInstallModInternal(selectedMod, autoYes);
        }

        protected void ProcessRemoveInputInternal(string? userInput, bool autoYes)
        {
            if (string.IsNullOrEmpty(userInput)) return;

            if (userInput.ToLower() == "@all")
            {
                ConsoleUI.Info("Removing all installed mods...");
                var installedNames = new List<string>(InstalledMods.Keys);
                if (installedNames.Count == 0)
                {
                    ConsoleUI.Warn("No mods are currently installed.");
                    return;
                }
                foreach (var name in installedNames)
                {
                    var mod = ModsList.Find(m => m.ModName == name);
                    if (mod.ModName == null)
                    {
                        ConsoleUI.Warn($"{name} is marked as installed but not found in ModsList. Skipped.");
                        continue;
                    }
                    RemoveModInternal(mod, autoYes);
                }
                return;
            }

            var selectedMod = ModsList.Find(m => m.ModName == userInput);
            if (selectedMod.ModName == null)
            {
                ConsoleUI.Error(string.Format(Messages.Get("RemoveNotFound"), userInput));
                return;
            }
            RemoveModInternal(selectedMod, autoYes);
        }

        // ===== ゲームごとに必要な差分ポイント =====

        protected abstract void ScanInstalledModsInternal();

        protected abstract void InstallModInternal(ModBase mod);

        protected abstract void RemoveModInternal(ModBase mod, bool autoYes);

        protected abstract bool IsBepInExMod(ModBase mod);

        protected abstract bool InstallBepInEx(string gamePath, string version);

        protected abstract bool UninstallBepInEx(string gamePath);

        // ===== 共通の依存関係・競合判定・InstallWithDependencies =====

        protected void TryInstallModInternal(ModBase selectedMod, bool autoYes)
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
                InstallWithDependenciesInternal(selectedMod);
                ConsoleUI.Success($"✓ {selectedMod.ModName} and its dependencies installed successfully.");
            }
            catch (Exception ex)
            {
                ConsoleUI.Error(Messages.Get("InstallFailed") + ex.Message);
            }
        }

        protected List<string> CheckExclusions(ModBase mod)
        {
            var result = new List<string>();
            if (mod.ConflictsWith == null) return result;
            foreach (var conflictName in mod.ConflictsWith)
                if (InstalledMods.ContainsKey(conflictName))
                    result.Add($"{mod.ModName} conflicts with already installed mod '{conflictName}'.");
            return result;
        }

        protected List<string> CheckConflicts(ModBase mod, List<string> visited)
        {
            var conflicts = new List<string>();
            if (visited.Contains(mod.ModName)) return conflicts;
            visited.Add(mod.ModName);
            if (mod.ModDependencies == null) return conflicts;

            foreach (var dep in mod.ModDependencies)
            {
                var depMod = ModsList.Find(m => m.ModName == dep.ModName);
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

        protected void InstallWithDependenciesInternal(ModBase mod)
        {
            if (InstalledMods.ContainsKey(mod.ModName)) return;

            if (mod.ModDependencies != null)
            {
                foreach (var dep in mod.ModDependencies)
                {
                    var depMod = ModsList.Find(m => m.ModName == dep.ModName);
                    if (depMod.ModName == null)
                        throw new InvalidOperationException($"Dependency '{dep.ModName}' not found for mod '{mod.ModName}'.");
                    InstallWithDependenciesInternal(depMod);
                }
            }

            InstallModInternal(mod);
        }

        // BepInEx のインストール／アンインストール用ヘルパー（必要なら派生側で使用）
        protected void InstallBepInExInternal(ModBase bepMod)
        {
            if (GamePath == null) throw new InvalidOperationException("GamePath is not set.");
            var ok = InstallBepInEx(GamePath, bepMod.ModVersion);
            if (!ok) { ConsoleUI.Error("Failed to install BepInEx."); throw new Exception("BepInEx install failed."); }
            InstalledMods[bepMod.ModName] = (bepMod.ModVersion, null);
            ConsoleUI.Success($"✓ Installed {bepMod.ModName}.");
        }

        protected void UninstallBepInExInternal(ModBase bepMod, bool autoYes)
        {
            if (GamePath == null) throw new InvalidOperationException("GamePath is not set.");

            if (!autoYes)
            {
                ConsoleUI.Prompt(string.Format(Messages.Get("RemoveAsk"), bepMod.ModName));
                if (Console.ReadLine()?.Trim().ToLower() != "y")
                {
                    ConsoleUI.Warn(Messages.Get("RemoveSkip"));
                    return;
                }
            }

            var ok = UninstallBepInEx(GamePath);
            if (ok)
            {
                InstalledMods.Remove(bepMod.ModName);
                ConsoleUI.Success(string.Format(Messages.Get("RemoveSuccess"), bepMod.ModName));
            }
            else
            {
                ConsoleUI.Error(string.Format(Messages.Get("RemoveFailed"), bepMod.ModName) + "See above for details.");
            }
        }

        protected string GetPluginsDir()
        {
            if (GamePath == null) throw new InvalidOperationException("GamePath is not set.");
            var pluginsDir = System.IO.Path.Combine(GamePath, "BepInEx", "plugins");
            System.IO.Directory.CreateDirectory(pluginsDir);
            return pluginsDir;
        }
    }
}
