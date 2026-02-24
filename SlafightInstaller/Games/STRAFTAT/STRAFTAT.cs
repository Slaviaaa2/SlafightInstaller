using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;

namespace SlafightInstaller.Games.STRAFTAT
{
    public static class STRAFTAT
    {
        public static List<ModBase> ModsList = new()
        {
            new ModBase
            {
                ModName = "BepInEx",
                ModVersion = "5.4.23.4",
                ModDependencies = new List<string>()
            },
            new ModBase
            {
                ModName = "moreStrafts",
                ModVersion = "0.0.4",
                ModDependencies = new List<string> { "BepInEx" }
            }
        };

        private static readonly HashSet<string> InstalledMods = new();

        public static void Entry()
        {
            Console.Write(Messages.Get("EnterGamePath"));
            var gamePath = Console.ReadLine();

            if (!Utils.IsValidPath(gamePath))
            {
                Console.WriteLine(Messages.Get("InvalidGamePath"));
                return;
            }

            Debug.Assert(gamePath != null, nameof(gamePath) + " != null");
            if (!File.Exists(Path.Combine(gamePath!, "STRAFTAT.exe")))
            {
                Console.WriteLine(Messages.Get("InvalidGamePath"));
                return;
            }

            Console.WriteLine(Messages.Get("UsableMods"));
            Console.WriteLine(string.Join(",\n", ModsList));

            Console.Write(Messages.Get("EnterModName"));
            var userInput = Console.ReadLine();

            var selectedMod = ModsList.FirstOrDefault(m => m.ModName == userInput);
            if (selectedMod.ModName == null)
            {
                Console.WriteLine(Messages.Get("InvalidModName"));
                return;
            }

            try
            {
                InstallWithDependencies(selectedMod, gamePath!);
                Console.WriteLine($"[STRAFTAT]{selectedMod.ModName} and its dependencies installed successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(Messages.Get("InstallFailed") + ex.Message);
            }
        }

        private static void InstallWithDependencies(ModBase mod, string gamePath)
        {
            if (InstalledMods.Contains(mod.ModName))
                return;

            if (mod.ModDependencies != null)
            {
                foreach (var depName in mod.ModDependencies)
                {
                    var depMod = ModsList.FirstOrDefault(m => m.ModName == depName);
                    if (depMod.ModName == null)
                        throw new InvalidOperationException($"Dependency '{depName}' not found for mod '{mod.ModName}'.");

                    InstallWithDependencies(depMod, gamePath);
                }
            }

            InstallMod(mod, gamePath);
            InstalledMods.Add(mod.ModName);
        }

        private static void InstallMod(ModBase mod, string gamePath)
        {
            Console.WriteLine($"[STRAFTAT]Installing {mod.ModName} {mod.ModVersion} ...");

            switch (mod.ModName)
            {
                case "BepInEx":
                {
                    var bep = ModsList.First(m => m.ModName == "BepInEx");
                    var ok = InstallUtils.InstallBepInEx.Install(gamePath, bep.ModVersion);
                    if (!ok)
                    {
                        Console.WriteLine("[STRAFTAT]Failed to install BepInEx.");
                        throw new Exception("BepInEx install failed.");
                    }
                    break;
                }

                case "moreStrafts":
                    InstallMoreStrafts(gamePath);
                    break;

                default:
                    Console.WriteLine("[STRAFTAT] Mod not found!");
                    throw new Exception("Unknown mod.");
            }

            Console.WriteLine($"[STRAFTAT]Installed {mod.ModName}.");
        }

        private static void InstallMoreStrafts(string gamePath)
        {
            try
            {
                var dstDir = Path.Combine(gamePath, "BepInEx", "plugins");
                Directory.CreateDirectory(dstDir);

                var dst = Path.Combine(dstDir, "moreStrafts.dll");
                if (File.Exists(dst))
                {
                    Console.Write(Messages.Get("OverwriteAsk"));
                    var ans = Console.ReadLine()?.Trim().ToLower();
                    if (ans != "y")
                    {
                        Console.WriteLine(Messages.Get("OverwriteSkip"));
                        return;
                    }
                }

                var url = "https://github.com/ALBINALSHAIKH/moreStrafts/releases/download/v0.0.4/moreStrafts.dll";

                using var client = new WebClient();
                Console.WriteLine($"[STRAFTAT]Downloading moreStrafts from {url} ...");
                client.DownloadFile(url, dst);
            }
            catch (Exception ex)
            {
                Console.WriteLine("[STRAFTAT]moreStrafts download failed:");
                Console.WriteLine(ex);
                throw;
            }
        }
    }
}
