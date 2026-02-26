using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlafightInstaller.Games.STRAFTAT;

namespace SlafightInstaller
{
    public static class Program
    {
        public static List<string> games = new() { "STRAFTAT" };
        public static string? CurrentGame { get; set; }
        public static string? CurrentGamePath { get; set; }

        public static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;

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

            Console.Write("Language / 言語 (en/jp) > ");
            var langInput = Console.ReadLine()?.Trim().ToLower();
            Messages.Current = langInput == "jp" ? Lang.Jp : Lang.En;

            ConsoleUI.Header(Messages.Get("Welcome"));

            // 起動引数にコマンドがある場合はそのまま実行
            if (args.Length > 0)
            {
                var argLine = string.Join(" ", args);
                ExecuteCommandLine(argLine, autoYes: false);
                BasicUtils.EndScreen();
                return;
            }

            while (true)
            {
                ConsoleUI.Divider();
                ConsoleUI.Info(Messages.Get("SelectGame"));
                foreach (var g in games)
                    ConsoleUI.Info($"  · {g}");
                ConsoleUI.Divider();
                ConsoleUI.Info(Messages.Get("CommandHint"));
                ConsoleUI.Prompt(Messages.Get("GameName"));
                var userInput = Console.ReadLine()?.Trim();

                if (userInput?.ToLower() == "exit")
                    break;

                // CLIモード移行
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
                    ExecuteCommandLine(userInput!, autoYes: false);
                    continue;
                }

                if (!games.Contains(userInput))
                {
                    ConsoleUI.Error(Messages.Get("InvalidGame"));
                    continue;
                }

                switch (userInput)
                {
                    case "STRAFTAT":
                        STRAFTAT.Entry();
                        break;
                }
            }

            BasicUtils.EndScreen();
        }

        /// <summary>
        /// 完全コマンドモード。exitで通常モードに戻る。
        /// </summary>
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

                ExecuteCommandLine(input, autoYes: false);
            }
        }

        private static void ExecuteCommandLine(string input, bool autoYes)
        {
            var commands = CommandParser.Parse(input);
            if (commands.Count == 0)
            {
                ConsoleUI.Error("Invalid command.");
                return;
            }

            string? selectedGame = null;
            string? selectedGamePath = null;
            var installQueue = new List<string>();
            var removeQueue = new List<string>();

            foreach (var cmd in commands)
            {
                if (cmd.Namespace != "game")
                {
                    ConsoleUI.Error($"Unknown namespace: @{cmd.Namespace}");
                    return;
                }

                if (cmd.Flags.Contains("y"))
                    autoYes = true;

                switch (cmd.Action.ToLower())
                {
                    case "sel":
                        if (cmd.Args.Count < 1)
                        {
                            ConsoleUI.Error("Usage: @game sel <GameName> [GamePath]");
                            return;
                        }
                        selectedGame = cmd.Args[0];
                        if (!games.Contains(selectedGame))
                        {
                            ConsoleUI.Error($"Unknown game: {selectedGame}");
                            return;
                        }
                        CurrentGame = selectedGame;
                        if (cmd.Args.Count >= 2)
                        {
                            selectedGamePath = string.Join(" ", cmd.Args.Skip(1));
                            CurrentGamePath = selectedGamePath;
                        }
                        ConsoleUI.Success($"Selected {selectedGame} ({CurrentGamePath ?? "path pending"})");
                        break;

                    case "-install":
                        if (cmd.Args.Count < 1)
                        {
                            ConsoleUI.Error("Usage: @game -install <ModName|@all> [--y]");
                            return;
                        }
                        installQueue.AddRange(cmd.Args);
                        break;

                    case "-remove":
                        if (cmd.Args.Count < 1)
                        {
                            ConsoleUI.Error("Usage: @game -remove <ModName> [--y]");
                            return;
                        }
    
                        foreach (var arg in cmd.Args)
                        {
                            if (arg.ToLower() == "@all")
                            {
                                // @all 時は現在インストールされているものだけ
                                removeQueue.AddRange(
                                    STRAFTAT.GetInstalledModNames() // ← 後述のヘルパー or InstalledMods.Keysを直接
                                );
                            }
                            else
                            {
                                removeQueue.Add(arg);
                            }
                        }
                        break;

                    case "-backup":
                        if (selectedGamePath == null)
                        {
                            ConsoleUI.Error("Game path not set. Use @game sel first.");
                            return;
                        }
                        BackupUtils.Backup(selectedGamePath);
                        break;

                    default:
                        ConsoleUI.Error($"Unknown action: {cmd.Action}");
                        return;
                }
            }

            // gamePathが未設定なら対話入力
            if (selectedGame != null && selectedGamePath == null
                && (installQueue.Count > 0 || removeQueue.Count > 0))
            {
                ConsoleUI.Prompt(Messages.Get("EnterGamePath"));
                selectedGamePath = Console.ReadLine()?.Trim();
            }

            if (selectedGame == null)
            {
                // 直前の sel がこの行では無かった場合 → グローバル状態を使う
                selectedGame = CurrentGame;
                selectedGamePath ??= CurrentGamePath;
                if (selectedGame == null)
                {
                    ConsoleUI.Error("Game is not selected. Use '@game sel <GameName> [Path]' first.");
                    return;
                }
            }

            switch (selectedGame)
            {
                case "STRAFTAT":
                    STRAFTAT.Entry(
                        preselectedGamePath: selectedGamePath,
                        commandQueue: installQueue.Count > 0 ? installQueue : null,
                        removeQueue: removeQueue.Count > 0 ? removeQueue : null,
                        autoYes: autoYes,
                        isCli: true  // CLIフラグON
                    );
                    break;
            }
        }
    }
}