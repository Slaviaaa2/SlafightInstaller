using System;
using System.Collections.Generic;

namespace SlafightInstaller
{
    public enum Lang
    {
        En,
        Jp
    }

    public static class Messages
    {
        public static Lang Current = Lang.En;

        public static string Get(string key)
        {
            var dict = Current == Lang.Jp ? Jp : En;
            return dict.TryGetValue(key, out var v) ? v : key;
        }

        public static readonly Dictionary<string, string> En = new Dictionary<string, string>
        {
            ["Welcome"]           = "Welcome to the Slafight Installer!",
            ["SelectGame"]        = "Please select target game (or type 'exit' to quit):",
            ["GameName"]          = "Game Name > ",
            ["InvalidGame"]       = "Invalid game name. Please try again.",
            ["CommandHint"]       = "(or type '.join cli' to enter CLI mode)",

            ["EnterGamePath"]     = "Game Path > ",
            ["InvalidGamePath"]   = "Invalid game path. Please check and try again.",
            ["UsableMods"]        = "Available Mods:",
            ["EnterModName"]      = "Mod Name > ",
            ["InvalidModName"]    = "Invalid mod name. Please try again.",
            ["ExitHint"]          = "(type 'exit' to return to game selection)",

            ["OverwriteAsk"]      = "File already exists. Overwrite? (y/n) > ",
            ["OverwriteSkip"]     = "Skipped (file already exists).",

            ["InstallFailed"]     = "Install failed: ",

            ["RemoveAsk"]         = "Are you sure you want to remove {0}? (y/n) > ",
            ["RemoveSuccess"]     = "Successfully removed {0}.",
            ["RemoveFailed"]      = "Failed to remove {0}: ",
            ["RemoveNotFound"]    = "{0} is not installed or files not found.",
            ["RemoveSkip"]        = "Removal cancelled.",

            ["BackupAsk"]         = "Backup game directory before installing? (y/n) > ",
            ["BackupSkip"]        = "Skipping backup.",
            ["BackupSuccess"]     = "Backup created: {0}",
            ["BackupFailed"]      = "Backup failed: {0}",
            ["BackupCreating"]    = "Creating backup at {0} ...",

            ["SelectMode"]        = "Select mode:",
            ["ModePrompt"]        = "Mode > ",
            ["InvalidMode"]       = "Invalid mode. Please enter 'install' or 'uninstall'.",
            ["InstallExitHint"]   = "(type '@all' to install all, 'exit' to return to mode selection)",
            ["UninstallExitHint"] = "(type 'exit' to return to mode selection)",

            ["HelpTitle"]         = "Available Commands",
            ["HelpNormal"]        =
              """
                .join cli
                  Enter CLI mode.

                @game sel <Game> [Path]
                  Select game (and optionally set path).
                  Example: @game sel STRAFTAT C:\Games\STRAFTAT

                @game -install <Mod|@all> [--y]
                  Install mod(s) and their dependencies.
                  @all installs every available mod.
                  --y skips all confirmation prompts.
                  Example: @game -install BepInEx --y
                           @game -install @all --y

                @game -remove <Mod|@all> [--y]
                  Remove installed mod(s).
                  @all removes all currently installed mods.
                  Example: @game -remove Fancy
                           @game -remove @all --y

                @game -backup
                  Create a zip backup of the game directory.
                  Example: @game sel STRAFTAT C:\Games\STRAFTAT && @game -backup

                exit
                  Return to previous screen.
              """,
            ["HelpCli"]          =
              """
                @game sel <Game> [Path]
                  Select a game. Optionally provide the game path inline.
                  Example: @game sel STRAFTAT C:\Games\STRAFTAT

                @game -install <ModName|@all> [--y]
                  Install a mod and its dependencies.
                  Use @all to install every available mod.
                  --y skips all confirmation prompts.
                  Example: @game -install BepInEx --y
                           @game -install @all --y

                @game -remove <ModName|@all> [--y]
                  Remove installed mod(s).
                  @all removes all currently installed mods.
                  Example: @game -remove Fancy
                           @game -remove @all --y

                @game -backup
                  Create a zip backup of the game directory.
                  Example: @game sel STRAFTAT C:\Games\STRAFTAT && @game -backup

                Chaining commands with &&:
                  @game sel STRAFTAT C:\Games\STRAFTAT && @game -install BepInEx --y
                  @game sel STRAFTAT C:\Games\STRAFTAT && @game -install @all --y

                exit
                  Exit CLI mode and return to normal mode.
              """,
        };

        public static readonly Dictionary<string, string> Jp = new Dictionary<string, string>
        {
            ["Welcome"]           = "Slafight Installer へようこそ！",
            ["SelectGame"]        = "対象ゲームを選択してください ('exit' で終了):",
            ["GameName"]          = "ゲーム名 > ",
            ["InvalidGame"]       = "無効なゲーム名です。もう一度お試しください。",
            ["CommandHint"]       = "('.join cli' でCLIモードに移行出来ます)",

            ["EnterGamePath"]     = "ゲームパス > ",
            ["InvalidGamePath"]   = "無効なゲームパスです。確認してもう一度お試しください。",
            ["UsableMods"]        = "使用可能な MOD:",
            ["EnterModName"]      = "MOD 名 > ",
            ["InvalidModName"]    = "無効な MOD 名です。もう一度お試しください。",
            ["ExitHint"]          = "('exit' でゲーム選択に戻る)",

            ["OverwriteAsk"]      = "ファイルが既に存在します。上書きしますか？ (y/n) > ",
            ["OverwriteSkip"]     = "スキップしました（ファイルが既に存在します）。",

            ["InstallFailed"]     = "インストールに失敗しました: ",

            ["RemoveAsk"]         = "{0} を削除してもよいですか？ (y/n) > ",
            ["RemoveSuccess"]     = "{0} を正常に削除しました。",
            ["RemoveFailed"]      = "{0} の削除に失敗しました: ",
            ["RemoveNotFound"]    = "{0} はインストールされていないか、ファイルが見つかりません。",
            ["RemoveSkip"]        = "削除をキャンセルしました。",

            ["BackupAsk"]         = "インストール前にゲームディレクトリをバックアップしますか？ (y/n) > ",
            ["BackupSkip"]        = "バックアップをスキップしました。",
            ["BackupSuccess"]     = "バックアップを作成しました: {0}",
            ["BackupFailed"]      = "バックアップに失敗しました: {0}",
            ["BackupCreating"]    = "{0} にバックアップを作成中...",

            ["SelectMode"]        = "モードを選択してください:",
            ["ModePrompt"]        = "モード > ",
            ["InvalidMode"]       = "無効なモードです。'install' または 'uninstall' を入力してください。",
            ["InstallExitHint"]   = "('@all' で全MODインストール、'exit' でモード選択に戻る)",
            ["UninstallExitHint"] = "('@all' で全MODアンインストール、'exit' でモード選択に戻る)",

            ["HelpTitle"]         = "使用可能なコマンド",
            ["HelpNormal"]        =
              """
                .join cli
                  CLIモードに移行

                @game sel <ゲーム> [パス]
                  ゲームを選択します（パスを直接指定可）。
                  例: @game sel STRAFTAT C:\Games\STRAFTAT

                @game -install <MOD|@all> [--y]
                  MODとその依存関係をインストールします。
                  @all ですべての利用可能なMODをインストール。
                  --y で確認プロンプトを省略。
                  例: @game -install BepInEx --y
                       @game -install @all --y

                @game -remove <MOD|@all> [--y]
                  インストール済みMODを削除します。
                  @all ですべての「現在インストールされている」MODを削除。
                  例: @game -remove Fancy
                       @game -remove @all --y

                @game -backup
                  ゲームディレクトリのzipバックアップを作成します。
                  例: @game sel STRAFTAT C:\Games\STRAFTAT && @game -backup

                exit
                  前の画面に戻ります。
              """,
            ["HelpCli"]          =
              """
                @game sel <ゲーム名> [パス]
                  ゲームを選択します。パスをインラインで指定することもできます。
                  例: @game sel STRAFTAT C:\Games\STRAFTAT

                @game -install <MOD名|@all> [--y]
                  MODとその依存関係をインストールします。
                  @all で利用可能な全MODをインストール。
                  --y で確認プロンプトをすべてスキップ。
                  例: @game -install BepInEx --y
                      @game -install @all --y

                @game -remove <MOD名|@all> [--y]
                  インストール済みMODを削除します。
                  @all ですべての「現在インストールされている」MODを削除。
                  例: @game -remove Fancy
                      @game -remove @all --y

                @game -backup
                  ゲームディレクトリのzipバックアップを作成します。
                  例: @game sel STRAFTAT C:\Games\STRAFTAT && @game -backup

                && でコマンドを連結:
                  @game sel STRAFTAT C:\Games\STRAFTAT && @game -install BepInEx --y
                  @game sel STRAFTAT C:\Games\STRAFTAT && @game -install @all --y

                exit
                  CLIモードを終了して通常モードに戻ります。
              """,
        };
    }
}
