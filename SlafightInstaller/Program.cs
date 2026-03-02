using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlafightInstaller.Games;
using SlafightInstaller.Utils;

namespace SlafightInstaller
{
    public static class Program
    {
        // 静的ゲームID一覧だけ持つ
        public static readonly List<string> Games = new()
        {
            "STRAFTAT"
        };

        public static string? CurrentGame { get; set; }
        public static string? CurrentGamePath { get; set; }

        public static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding  = Encoding.UTF8;

            ConsoleUI.Debug($"Launching System...\n" +
                            $"Author: Slaviaaa2, Version: {UpdateChecker.GetCurrentVersionDisplay()}, OperatingSystem: {Environment.OSVersion.Platform}, Is 64bit: {Environment.Is64BitOperatingSystem}\n",
                true);

            if (Environment.OSVersion.Platform != PlatformID.Win32NT)
            {
                ConsoleUI.Error("You need to run this program on Windows NT.");
                BasicUtils.EndScreen();
                return;
            }

            if (!Environment.Is64BitOperatingSystem)
            {
                ConsoleUI.Error("You need to run this program on a 64-bit operating system.");
                BasicUtils.EndScreen();
                return;
            }

            // カスタムMODを起動時に自動ロード
            CustomModRegistry.Load();

            Console.Write("Language / 言語 (en/jp) > ");
            var langInput    = Console.ReadLine()?.Trim().ToLower();
            Messages.Current = langInput == "jp" ? Lang.Jp : Lang.En;

            ConsoleUI.Header(Messages.Get("Welcome"));

            UpdateChecker.CheckForUpdates();

            // 起動引数にコマンドがある場合はそのまま実行
            if (args.Length > 0)
            {
                var argLine = string.Join(" ", args);
                ExecuteCommandLine(argLine, autoYes: false, noBackup: false);
                BasicUtils.EndScreen();
                return;
            }

            while (true)
            {
                ConsoleUI.Divider();
                ConsoleUI.Info(Messages.Get("SelectGame"));
                foreach (var g in Games)
                    ConsoleUI.Info($"  · {g}");
                ConsoleUI.Divider();
                ConsoleUI.Info(Messages.Get("CommandHint"));
                ConsoleUI.Prompt(Messages.Get("GameName"));
                var userInput = Console.ReadLine()?.Trim();

                if (userInput?.ToLower() == "exit") break;

                if (userInput?.ToLower() == ".join cli")
                {
                    RunCliMode();
                    continue;
                }

                if (userInput?.ToLower() == "help")
                {
                    ConsoleUI.Header(Messages.Get("HelpTitle"));
                    ConsoleUI.Info(Messages.Get("HelpNormal"));
                    continue;
                }

                if (CommandParser.IsCommand(userInput))
                {
                    ExecuteCommandLine(userInput!, autoYes: false, noBackup: false);
                    continue;
                }

                if (userInput == null || !Games.Contains(userInput))
                {
                    ConsoleUI.Error(Messages.Get("InvalidGame"));
                    continue;
                }

                CurrentGame     = userInput;
                CurrentGamePath = null; // 対話モードでは毎回入力させる

                // 静的 STRAFTAT を起動
                if (userInput == "STRAFTAT")
                    STRAFTAT.Entry(preselectedGamePath: null,
                                   commandQueue: null,
                                   removeQueue: null,
                                   autoYes: false,
                                   noBackup: false,
                                   isCli: false);
            }

            BasicUtils.EndScreen();
        }

        private static void RunCliMode()
        {
            ConsoleUI.Header("CLI Mode");
            ConsoleUI.Info("Entered CLI mode. Type 'help' for usage, 'exit' to return.");

            while (true)
            {
                ConsoleUI.Prompt("cli > ");
                var input = Console.ReadLine()?.Trim();

                if (input?.ToLower() == "exit")
                {
                    ConsoleUI.Info("Exiting CLI mode.");
                    return;
                }
                if (string.IsNullOrEmpty(input)) continue;

                if (input.ToLower() == "help")
                {
                    ConsoleUI.Header(Messages.Get("HelpTitle"));
                    ConsoleUI.Info(Messages.Get("HelpCli"));
                    continue;
                }

                if (!CommandParser.IsCommand(input))
                {
                    ConsoleUI.Error("CLI mode only accepts @ commands. Type 'help' for usage.");
                    continue;
                }

                ExecuteCommandLine(input, autoYes: false, noBackup: false);
            }
        }

        private static void ExecuteCommandLine(string input, bool autoYes, bool noBackup)
        {
            var commands = CommandParser.Parse(input);
            if (commands.Count == 0)
            {
                ConsoleUI.Error("Invalid command.");
                return;
            }

            string? selectedGame     = null;
            string? selectedGamePath = null;
            var installQueue         = new List<string>();
            var removeQueue          = new List<string>();

            foreach (var cmd in commands)
            {
                if (cmd.Flags.Contains("y"))         autoYes  = true;
                if (cmd.Flags.Contains("no-backup")) noBackup = true;

                if (cmd.Namespace == "game")
                {
                    if (!HandleGameCommand(cmd, ref selectedGame, ref selectedGamePath,
                            ref installQueue, ref removeQueue, autoYes, noBackup))
                        return;
                }
                else if (cmd.Namespace == "mod")
                {
                    if (!HandleModCommand(cmd)) return;
                }
                else
                {
                    ConsoleUI.Error($"Unknown namespace: @{cmd.Namespace}");
                    return;
                }
            }

            if (installQueue.Count == 0 && removeQueue.Count == 0) return;

            if (selectedGame == null)
            {
                selectedGame     = CurrentGame;
                selectedGamePath ??= CurrentGamePath;
                if (selectedGame == null)
                {
                    ConsoleUI.Error("Game is not selected. Use '@game sel <GameName> [Path]' first.");
                    return;
                }
            }

            if (!Games.Contains(selectedGame))
            {
                ConsoleUI.Error($"Unknown game: {selectedGame}");
                return;
            }

            if (selectedGamePath == null)
            {
                ConsoleUI.Prompt(Messages.Get("EnterGamePath"));
                selectedGamePath = Console.ReadLine()?.Trim();
            }

            CurrentGame     = selectedGame;
            CurrentGamePath = selectedGamePath;

            // 今は STRAFTAT だけ想定
            if (selectedGame == "STRAFTAT")
            {
                STRAFTAT.Entry(
                    preselectedGamePath: selectedGamePath,
                    commandQueue: installQueue.Count > 0 ? installQueue : null,
                    removeQueue:  removeQueue.Count  > 0 ? removeQueue  : null,
                    autoYes:   autoYes,
                    noBackup:  noBackup,
                    isCli:     true
                );
            }
        }

        // -----------------------------------------------------------------------
        // @game
        // -----------------------------------------------------------------------
        private static bool HandleGameCommand(
            CommandParser.ParsedCommand cmd,
            ref string? selectedGame,
            ref string? selectedGamePath,
            ref List<string> installQueue,
            ref List<string> removeQueue,
            bool autoYes, bool noBackup)
        {
            if (string.IsNullOrEmpty(cmd.Action))
            {
                ConsoleUI.Info(Messages.Get("HelpGame"));
                return true;
            }

            switch (cmd.Action.ToLower())
            {
                case "sel":
                    if (cmd.Args.Count < 1) { ConsoleUI.Info(Messages.Get("HelpGameSel")); return true; }
                    selectedGame = cmd.Args[0];
                    if (!Games.Contains(selectedGame))
                    {
                        ConsoleUI.Error($"Unknown game: {selectedGame}");
                        return false;
                    }
                    CurrentGame = selectedGame;
                    if (cmd.Args.Count >= 2)
                    {
                        selectedGamePath = string.Join(" ", cmd.Args.Skip(1));
                        CurrentGamePath  = selectedGamePath;
                    }
                    ConsoleUI.Success($"Selected {selectedGame} ({CurrentGamePath ?? "path pending"})");
                    break;

                case "-install":
                    if (cmd.Args.Count < 1) { ConsoleUI.Info(Messages.Get("HelpGameInstall")); return true; }
                    installQueue.AddRange(cmd.Args);
                    break;

                case "-remove":
                    if (cmd.Args.Count < 1) { ConsoleUI.Info(Messages.Get("HelpGameRemove")); return true; }
                    foreach (var arg in cmd.Args)
                    {
                        if (arg.ToLower() == "@all")
                        {
                            if (selectedGame != null && Games.Contains(selectedGame))
                            {
                                // 今は STRAFTAT だけ
                                if (selectedGame == "STRAFTAT")
                                    removeQueue.AddRange(STRAFTAT.GetInstalledModNames());
                            }
                            else
                            {
                                ConsoleUI.Warn("Game is not selected for '@game -remove @all'.");
                            }
                        }
                        else
                        {
                            removeQueue.Add(arg);
                        }
                    }
                    break;

                case "-backup":
                    var path = selectedGamePath ?? CurrentGamePath;
                    if (path == null) { ConsoleUI.Info(Messages.Get("HelpGameBackup")); return true; }
                    BackupUtils.Backup(path);
                    break;

                default:
                    ConsoleUI.Error($"Unknown @game action: '{cmd.Action}'");
                    ConsoleUI.Info(Messages.Get("HelpGame"));
                    return false;
            }
            return true;
        }

        // -----------------------------------------------------------------------
        // @mod
        // -----------------------------------------------------------------------
        private static bool HandleModCommand(CommandParser.ParsedCommand cmd)
        {
            if (string.IsNullOrEmpty(cmd.Action))
            {
                ConsoleUI.Info(Messages.Get("HelpMod"));
                return true;
            }

            switch (cmd.Action.ToLower())
            {
                case "list":
                    CustomModRegistry.PrintList();
                    break;

                case "reload":
                    CustomModRegistry.Load();
                    break;

                case "add":
                    if (cmd.Args.Count < 3) { ConsoleUI.Info(Messages.Get("HelpModAdd")); return true; }
                    var newMod = new ModBase
                    {
                        ModName              = cmd.Args[0],
                        ModVersion           = cmd.Args[1],
                        SourceUrl            = cmd.Args[2],
                        InstallFileName      = cmd.Args.Count > 3 ? cmd.Args[3] : null,
                        InstallSubPath       = cmd.Args.Count > 4 ? cmd.Args[4] : null,
                        ExtractTargetSubPath = cmd.Args.Count > 5 ? cmd.Args[5] : null,
                        ModDependencies      = new List<ModDependency>(),
                        ConflictsWith        = new List<string>(),
                        FinalPath            = null
                    };
                    CustomModRegistry.AddOrUpdate(newMod);
                    ConsoleUI.Success($"Custom mod '{newMod.ModName}' registered.");
                    break;

                case "remove":
                    if (cmd.Args.Count < 1) { ConsoleUI.Info(Messages.Get("HelpModRemove")); return true; }
                    if (CustomModRegistry.Remove(cmd.Args[0]))
                        ConsoleUI.Success($"Custom mod '{cmd.Args[0]}' removed.");
                    else
                        ConsoleUI.Warn($"Custom mod '{cmd.Args[0]}' not found.");
                    break;

                case "update":
                    if (cmd.Args.Count < 3) { ConsoleUI.Info(Messages.Get("HelpModUpdate")); return true; }
                    return UpdateCustomMod(cmd.Args[0], cmd.Args[1], string.Join(" ", cmd.Args.Skip(2)));

                default:
                    ConsoleUI.Error($"Unknown @mod action: '{cmd.Action}'");
                    ConsoleUI.Info(Messages.Get("HelpMod"));
                    return false;
            }
            return true;
        }

        private static bool UpdateCustomMod(string modName, string field, string value)
        {
            var mod = CustomModRegistry.CustomMods.FirstOrDefault(m => m.ModName == modName);
            if (mod.ModName == null)
            {
                ConsoleUI.Error($"Custom mod '{modName}' not found. Use '@mod add' first.");
                return false;
            }

            var updated = mod;
            switch (field.ToLower())
            {
                case "version":              updated.ModVersion           = value; break;
                case "url":                  updated.SourceUrl            = value; break;
                case "installfilename":      updated.InstallFileName      = value == "null" ? null : value; break;
                case "installsubpath":       updated.InstallSubPath       = value == "null" ? null : value; break;
                case "extracttargetsubpath": updated.ExtractTargetSubPath = value == "null" ? null : value; break;
                case "finalpath":            updated.FinalPath            = value == "null" ? null : value; break;
                default:
                    ConsoleUI.Error($"Unknown field: '{field}'");
                    ConsoleUI.Info(Messages.Get("HelpModUpdate"));
                    return false;
            }
            CustomModRegistry.AddOrUpdate(updated);
            ConsoleUI.Success($"Updated '{modName}'.{field} = {value}");
            return true;
        }
    }
}
