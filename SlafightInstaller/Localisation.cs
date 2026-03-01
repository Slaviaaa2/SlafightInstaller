using System;
using System.Collections.Generic;

namespace SlafightInstaller;

public enum Lang { En, Jp }

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

        // ===== Top-level help =====
        ["HelpTitle"]  = "Available Commands",
        ["HelpNormal"] =
        """
          .join cli                             Enter CLI mode.

          @game                                 Show @game subcommand list.
          @game sel      <Game> [Path]          Select game.
          @game -install <Mod|@all> [--y] [--no-backup]   Install mod(s).
          @game -remove  <Mod|@all> [--y]       Remove mod(s).
          @game -backup                         Backup game folder.

          @mod                                  Show @mod subcommand list.
          @mod list                             List custom mods.
          @mod add    <Name> <Ver> <Url> [...]  Register custom mod.
          @mod remove <Name>                    Delete custom mod entry.
          @mod update <Name> <field> <val>      Update a field.
          @mod reload                           Reload from JSON.

          help    Show this help.
          exit    Quit.
        """,
        ["HelpCli"] =
        """
          @game                                 Show @game subcommand list.
          @game sel      <Game> [Path]          Select game.
          @game -install <Mod|@all> [--y] [--no-backup]   Install mod(s).
          @game -remove  <Mod|@all> [--y]       Remove mod(s).
          @game -backup                         Backup game folder.

          @mod                                  Show @mod subcommand list.
          @mod list / add / remove / update / reload

          Chain: @game sel STRAFTAT C:\path && @game -install @all --y --no-backup

          help / exit
        """,

        // ===== @game help =====
        ["HelpGame"] =
        """
          @game — Game management

          Subcommands:
            sel      <GameName> [Path]
            -install <ModName|@all>    [--y] [--no-backup]
            -remove  <ModName|@all>    [--y]
            -backup

          Flags:
            --y          Skip all confirmation prompts.
            --no-backup  Skip backup (even when --y is set).

          Type '@game <subcommand>' with no args for detailed help.
          Example: @game -install
        """,
        ["HelpGameSel"] =
        """
          @game sel <GameName> [Path]
            Select a game. Optionally set the game path inline.
            Available games: STRAFTAT

          Examples:
            @game sel STRAFTAT
            @game sel STRAFTAT C:\Games\STRAFTAT
        """,
        ["HelpGameInstall"] =
        """
          @game -install <ModName|@all> [--y] [--no-backup]
            Install a mod and its dependencies.

          Args:
            ModName      Exact mod name.
            @all         Install every available mod.

          Flags:
            --y          Skip all confirmation prompts.
            --no-backup  Skip backup even with --y.

          Examples:
            @game -install BepInEx
            @game -install @all --y --no-backup
        """,
        ["HelpGameRemove"] =
        """
          @game -remove <ModName|@all> [--y]
            Remove installed mod(s).

          Args:
            ModName  Exact mod name.
            @all     Remove all currently installed mods.

          Flags:
            --y  Skip confirmation prompts.

          Examples:
            @game -remove Fancy
            @game -remove @all --y
        """,
        ["HelpGameBackup"] =
        """
          @game -backup
            Create a folder-copy backup of the game directory.
            Placed next to the game folder with a timestamp suffix.
            Requires game path to be set via '@game sel' first.

          Example:
            @game sel STRAFTAT C:\Games\STRAFTAT && @game -backup
        """,

        // ===== @mod help =====
        ["HelpMod"] =
        """
          @mod — Custom mod registry

          Subcommands:
            list                               List all registered custom mods.
            add    <Name> <Ver> <Url> [...]    Register or overwrite a custom mod.
            remove <Name>                      Delete a custom mod entry.
            update <Name> <field> <value>      Update a single field.
            reload                             Reload from JSON file.

          Type '@mod <subcommand>' with no args for detailed help.
          Example: @mod add
        """,
        ["HelpModAdd"] =
        """
          @mod add <Name> <Version> <Url> [InstallFileName] [InstallSubPath] [ExtractTargetSubPath]
            Register a new custom mod, or overwrite an existing entry.

          Args:
            Name                  Mod name (quote if it contains spaces).
            Version               Version string, e.g. 1.0.0
            Url                   Download URL.
            InstallFileName       (opt) Filename to save as.
            InstallSubPath        (opt) Path inside zip to extract from, e.g. BepInEx/plugins
            ExtractTargetSubPath  (opt) Subfolder under plugins/ to extract into, e.g. CosmeticBundles

          Example:
            @mod add "My Mod" 1.0.0 https://example.com/mod.zip mod.zip BepInEx/plugins
        """,
        ["HelpModRemove"] =
        """
          @mod remove <Name>
            Delete a custom mod entry from the registry.

          Example:
            @mod remove "My Mod"
        """,
        ["HelpModUpdate"] =
        """
          @mod update <Name> <field> <value>
            Update a single field. Use "null" to clear an optional field.

          Fields:
            version / url / installfilename / installsubpath / extracttargetsubpath / finalpath

          Examples:
            @mod update "My Mod" url https://new.example.com/mod.zip
            @mod update "My Mod" installsubpath null
        """,
        ["HelpModReload"] =
        """
          @mod reload
            Reload custom mods from the JSON file on disk.
            Useful after manually editing the file.

          File: %AppData%\SlafightInstaller\custom_mods.json
        """,

        // ==== Update Checker (EN) ==== //
        ["Update_Stable"]       = "A new version is available: {0} (current: {1})",
        ["Update_StableUrl"]    = "GitHub release page: {0}",
        ["Update_Pre"]          = "A new pre-release version is available: {0} (current: {1})",
        ["Update_PreWarn"]      = "This is a pre-release (beta) build. It may contain experimental changes.",
        ["Update_DownloadAsk"]  = "Do you want to download and update now? (y/n) > ",
        ["Update_Downloading"]  = "Downloading updater package...",
        ["Update_DownloadDone"] = "Download completed. Please run the new installer if it does not start automatically.",
        ["Update_DownloadFailed"]= "Download failed: {0}",
        ["Update_NoAssets"]     = "No downloadable assets were found for this release.",
        ["Update_Skip"]         = "Skip updating."
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

        // ===== トップレベルヘルプ =====
        ["HelpTitle"]  = "使用可能なコマンド",
        ["HelpNormal"] =
        """
          .join cli                              CLIモードに移行

          @game                                  @game サブコマンド一覧を表示
          @game sel      <ゲーム> [パス]          ゲームを選択
          @game -install <MOD|@all> [--y] [--no-backup]   MODをインストール
          @game -remove  <MOD|@all> [--y]        MODを削除
          @game -backup                           バックアップを作成

          @mod                                   @mod サブコマンド一覧を表示
          @mod list                              カスタムMOD一覧
          @mod add    <名> <Ver> <Url> [...]     カスタムMODを登録
          @mod remove <名>                       カスタムMODを削除
          @mod update <名> <フィールド> <値>      フィールドを更新
          @mod reload                            JSONから再読み込み

          help    このヘルプを表示
          exit    終了
        """,
        ["HelpCli"] =
        """
          @game                                  @game サブコマンド一覧を表示
          @game sel      <ゲーム> [パス]          ゲームを選択
          @game -install <MOD|@all> [--y] [--no-backup]   MODをインストール
          @game -remove  <MOD|@all> [--y]        MODを削除
          @game -backup                           バックアップを作成

          @mod                                   @mod サブコマンド一覧を表示
          @mod list / add / remove / update / reload

          連結: @game sel STRAFTAT C:\path && @game -install @all --y --no-backup

          help / exit
        """,

        // ===== @game ヘルプ =====
        ["HelpGame"] =
        """
          @game — ゲーム管理コマンド

          サブコマンド:
            sel      <ゲーム名> [パス]
            -install <MOD名|@all>   [--y] [--no-backup]
            -remove  <MOD名|@all>   [--y]
            -backup

          フラグ:
            --y          確認プロンプトをすべてスキップ
            --no-backup  バックアップをスキップ（--y 時も有効）

          詳細は '@game <サブコマンド>' を引数なしで入力。
          例: @game -install
        """,
        ["HelpGameSel"] =
        """
          @game sel <ゲーム名> [パス]
            ゲームを選択します。パスをインラインで指定可。
            利用可能なゲーム: STRAFTAT

          例:
            @game sel STRAFTAT
            @game sel STRAFTAT C:\Games\STRAFTAT
        """,
        ["HelpGameInstall"] =
        """
          @game -install <MOD名|@all> [--y] [--no-backup]
            MODとその依存関係をインストールします。

          引数:
            MOD名       正確なMOD名
            @all        利用可能な全MODをインストール

          フラグ:
            --y          確認プロンプトをすべてスキップ
            --no-backup  バックアップをスキップ

          例:
            @game -install BepInEx
            @game -install @all --y --no-backup
        """,
        ["HelpGameRemove"] =
        """
          @game -remove <MOD名|@all> [--y]
            インストール済みMODを削除します。

          引数:
            MOD名   正確なMOD名
            @all    現在インストールされている全MODを削除

          フラグ:
            --y  確認プロンプトをスキップ

          例:
            @game -remove Fancy
            @game -remove @all --y
        """,
        ["HelpGameBackup"] =
        """
          @game -backup
            ゲームフォルダのコピーバックアップを作成します。
            ゲームフォルダと同じ親ディレクトリにタイムスタンプ付きで保存。
            事前に '@game sel' でパスを設定する必要があります。

          例:
            @game sel STRAFTAT C:\Games\STRAFTAT && @game -backup
        """,

        // ===== @mod ヘルプ =====
        ["HelpMod"] =
        """
          @mod — カスタムMOD管理コマンド

          サブコマンド:
            list                               登録済みMODを一覧表示
            add    <名> <Ver> <Url> [...]      MODを登録または上書き
            remove <名>                         MODエントリを削除
            update <名> <フィールド> <値>        フィールドを更新
            reload                              JSONから再読み込み

          詳細は '@mod <サブコマンド>' を引数なしで入力。
          例: @mod add
        """,
        ["HelpModAdd"] =
        """
          @mod add <名前> <バージョン> <URL> [InstallFileName] [InstallSubPath] [ExtractTargetSubPath]
            カスタムMODを登録。同名エントリがあれば上書き。

          引数:
            名前                  MOD名（スペースを含む場合はクォートで囲む）
            バージョン            バージョン文字列（例: 1.0.0）
            URL                   ダウンロードURL
            InstallFileName       (省略可) 保存ファイル名
            InstallSubPath        (省略可) ZIP内の展開元パス（例: BepInEx/plugins）
            ExtractTargetSubPath  (省略可) plugins/ 以下の展開先（例: CosmeticBundles）

          例:
            @mod add "My Mod" 1.0.0 https://example.com/mod.zip mod.zip BepInEx/plugins
        """,
        ["HelpModRemove"] =
        """
          @mod remove <名前>
            カスタムMODエントリを削除します。

          例:
            @mod remove "My Mod"
        """,
        ["HelpModUpdate"] =
        """
          @mod update <名前> <フィールド> <値>
            フィールドを1つ更新。省略可能なフィールドは "null" で空にできます。

          フィールド:
            version / url / installfilename / installsubpath / extracttargetsubpath / finalpath

          例:
            @mod update "My Mod" url https://new.example.com/mod.zip
            @mod update "My Mod" installsubpath null
        """,
        ["HelpModReload"] =
        """
          @mod reload
            ディスク上のJSONファイルからカスタムMODを再読み込みします。
            手動編集後に使用してください。

          ファイル場所:
            %AppData%\SlafightInstaller\custom_mods.json
        """,

        // ==== Update Checker (JP) ==== //
        ["Update_Stable"]       = "新しいバージョンがあります: {0} (現在: {1})",
        ["Update_StableUrl"]    = "GitHub リリースページ: {0}",
        ["Update_Pre"]          = "新しいプレリリース版があります: {0} (現在: {1})",
        ["Update_PreWarn"]      = "これはプレリリース（ベータ）版です。実験的な変更が含まれている可能性があります。",
        ["Update_DownloadAsk"]  = "今すぐダウンロードして更新しますか？ (y/n) > ",
        ["Update_Downloading"]  = "アップデータをダウンロードしています...",
        ["Update_DownloadDone"] = "ダウンロードが完了しました。自動で起動しない場合は、新しいインストーラーを実行してください。",
        ["Update_DownloadFailed"]= "ダウンロードに失敗しました: {0}",
        ["Update_NoAssets"]     = "このリリースにはダウンロード可能なアセットが見つかりませんでした。",
        ["Update_Skip"]         = "更新をスキップしました。"
    };
}
