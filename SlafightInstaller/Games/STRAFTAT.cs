using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using SlafightInstaller.Utils;

namespace SlafightInstaller.Games
{
    public class STRAFTAT : GameBase
    {
        public override string GameId => "STRAFTAT";
        public override string DisplayName => "STRAFTAT";
        public override string ExecutableName => "STRAFTAT.exe";

        private readonly List<ModBase> _modsList = new List<ModBase>
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

        public override List<ModBase> ModsList => _modsList;

        // ===== 抽象メソッド実装 =====

        protected override void ScanInstalledModsInternal()
        {
            // Clear前にインストール時記録済みのFolderNameを退避
            var knownFolders = InstalledMods
                .Where(kv => kv.Value.FolderName != null)
                .ToDictionary(kv => kv.Key, kv => kv.Value.FolderName!);

            InstalledMods.Clear();
            var pluginsDir = GetPluginsDir();
            var gamePath = Program.CurrentGamePath ?? GamePathFromPlugins(pluginsDir);
            if (gamePath == null) return;

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

            if (gamePath != null && File.Exists(Path.Combine(gamePath, ".doorstop_version")))
            {
                var bepMod = ModsList.First(m => m.ModName == "BepInEx");
                InstalledMods["BepInEx"] = (bepMod.ModVersion, null);
            }
        }

        private string? GamePathFromPlugins(string pluginsDir)
        {
            // pluginsDir = <gamePath>\BepInEx\plugins 想定
            var dirInfo = Directory.GetParent(pluginsDir); // BepInEx
            if (dirInfo == null) return null;
            dirInfo = dirInfo.Parent; // gamePath
            return dirInfo?.FullName;
        }

        protected override void InstallModInternal(ModBase mod)
        {
            if (Program.CurrentGamePath == null)
                throw new InvalidOperationException("GamePath is not set.");
            var gamePath = Program.CurrentGamePath;

            ConsoleUI.Info($"Installing {mod.ModName}@{mod.ModVersion} ...");

            if (mod.ModName == "BepInEx")
            {
                InstallBepInExInternal(mod);
                return;
            }

            if (string.IsNullOrEmpty(mod.SourceUrl))
                throw new Exception($"SourceURL is missing for mod '{mod.ModName}'.");

            var pluginsDir = GetPluginsDir();

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

        protected override void RemoveModInternal(ModBase mod, bool autoYes)
        {
            if (Program.CurrentGamePath == null)
                throw new InvalidOperationException("GamePath is not set.");
            var gamePath = Program.CurrentGamePath;

            if (mod.ModName == "BepInEx")
            {
                UninstallBepInExInternal(mod, autoYes);
                return;
            }

            if (!autoYes)
            {
                ConsoleUI.Prompt(string.Format(Messages.Get("RemoveAsk"), mod.ModName));
                if (Console.ReadLine()?.Trim().ToLower() != "y")
                {
                    ConsoleUI.Warn(Messages.Get("RemoveSkip"));
                    return;
                }
            }

            try
            {
                var pluginsDir = GetPluginsDir();
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

        protected override bool IsBepInExMod(ModBase mod)
        {
            return mod.ModName == "BepInEx";
        }

        protected override bool InstallBepInEx(string gamePath, string version)
        {
            return BepInExInstallUtils.Install(gamePath, version);
        }

        protected override bool UninstallBepInEx(string gamePath)
        {
            return BepInExInstallUtils.Uninstall(gamePath);
        }
    }
}
