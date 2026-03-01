Slafight Installer
==================

概要
----

Slafight Installer は、対応ゲーム向けの MOD をまとめて管理・インストールするためのツールです。
現在は「STRAFTAT」に対応しており、BepInEx や Thunderstore / GitHub 上のいくつかの MOD を
自動でダウンロード・展開できます。

主な機能
--------

- 対応ゲームごとの MOD 一覧表示
- 依存関係を考慮した MOD のインストール / アンインストール
- ゲームフォルダのバックアップ作成（任意）
- CLI コマンドからの一括インストール / アンインストール
- ユーザー定義のカスタム MOD 登録（JSON 保存）
- GitHub Release を使った自己アップデート（Stable / Pre-release 判定付き）

動作環境
--------

- OS: Windows 10 以降 (64bit)
- .NET Framework 4.8 以降（またはそれ相当のランタイム）
- 対応ゲームがあらかじめインストールされていること

起動方法
--------

1. SlafightInstaller.exe を実行します。
2. 最初に言語選択プロンプトが表示されます。

   Language / 言語 (en/jp) > 

   - `en` を入力すると英語モード
   - `jp` を入力すると日本語モード

3. 起動時に GitHub の Release を確認し、ツール本体に新しいバージョンがあれば通知されます。
   - 安定版リリース（Stable）の場合は、その旨が表示されます。
   - プレリリース（Pre-release）の場合は、警告文付きで表示されます。
   - `y` を入力すると最新バージョンの実行ファイルを自動ダウンロードし、
     別のアップデータ（SlafightInstaller.Updater.exe）経由で exe を置き換えます。

   ※ アップデータは現在の SlafightInstaller.exe の終了を待機し、exe を上書きしてから
      新しい SlafightInstaller.exe を起動します。

通常モードの使い方
-------------------

起動後、まず対象ゲームを選択します。

- 利用可能なゲームが一覧表示されます（例: STRAFTAT）。
- プロンプト:

  ゲーム名 > 

- ゲーム名を入力すると、そのゲーム専用の MOD 管理メニューに移動します。
- `exit` と入力するとプログラムを終了します。
- `.join cli` と入力すると CLI モードに移行します（後述）。

ゲームを選択すると、次のようなモードを選べます。

- install  : MOD のインストール
- uninstall: MOD のアンインストール

### MOD のインストール

1. `install` を入力。
2. 対象ゲームの「使用可能な MOD」が一覧表示されます。
   - すでにインストールされている MOD にはチェックマーク（✓）が付きます。
3. プロンプト:

   MOD 名 > 

4. インストールしたい MOD 名を入力すると、その MOD と依存関係を自動的にインストールします。
5. 特殊入力:
   - `@all` を入力すると、利用可能な全ての MOD をまとめてインストールします。
   - `exit` を入力するとモード選択に戻ります。

インストール時、必要に応じて以下の確認が行われます。

- ゲームフォルダのバックアップを取るかどうか（初回）
- ファイルの上書きを行うかどうか
- 依存関係や競合がある場合の続行確認

### MOD のアンインストール

1. `uninstall` を入力。
2. MOD 一覧が表示されます。
3. プロンプト:

   MOD 名 > 

4. 削除したい MOD 名を入力すると、対応するファイルやフォルダを削除します。
5. 特殊入力:
   - `@all` を入力すると、インストール済みの MOD をまとめて削除します（BepInEx なども含む）。
   - `exit` を入力するとモード選択に戻ります。

CLI モードの使い方
------------------

`.join cli` を入力すると CLI モードに入ります。

CLI モードでは、`@game` / `@mod` などのコマンドをチェーンして実行できます。

代表的なコマンド:

- ゲーム選択:

  @game sel STRAFTAT C:\Games\STRAFTAT

- すべての MOD を一括インストール (確認付き):

  @game -install @all

- すべての MOD をバックアップ無し・確認無しで一括インストール:

  @game -install @all --y --no-backup

- すべての MOD を一括削除:

  @game -remove @all --y

- ゲームフォルダのバックアップのみ:

  @game sel STRAFTAT C:\Games\STRAFTAT && @game -backup

CLI モードを終了したい場合は `exit` を入力します。

カスタム MOD の登録 (@mod)
---------------------------

Slafight Installer には、ユーザーが自由に MOD 情報を追加できる「カスタム MOD レジストリ」があります。
これらは `%AppData%\SlafightInstaller\custom_mods.json` に保存されます。

代表的なコマンド:

- 登録済みカスタム MOD の一覧表示:

  @mod list

- カスタム MOD の追加:

  @mod add "My Mod" 1.0.0 https://example.com/mod.zip mod.zip BepInEx/plugins

  引数:
  - Name                  MOD 名（スペースを含む場合はクォートで囲む）
  - Version               バージョン文字列 (例: 1.0.0)
  - Url                   ダウンロード URL
  - InstallFileName       ZIP / DLL の保存ファイル名（省略可）
  - InstallSubPath        ZIP 内の展開元パス（省略可）
  - ExtractTargetSubPath  plugins/ 以下の展開先（省略可）
  - FinalPath             最終的なファイルの設置位置（省略可）

- フィールドの更新:

  @mod update "My Mod" url https://new.example.com/mod.zip
  @mod update "My Mod" installsubpath null

- カスタム MOD の削除:

  @mod remove "My Mod"

- JSON から再読み込み:

  @mod reload

バックアップ仕様
----------------

- `@game -backup` もしくはインストール時の確認で「Yes」を選ぶと、
  ゲームフォルダのコピーが作成されます。
- バックアップはゲームフォルダと同じ親ディレクトリに、
  タイムスタンプ付きのフォルダ名で保存されます。
- 例: `STRAFTAT_backup_20240101-123000` など。

更新（アップデート）仕様
------------------------

- 起動時に GitHub の最新 Release を確認します。
- バージョン比較は数値（Major.Minor[.Build]）で行われます。
  - 最新バージョンが現在より古い場合は何もしません。
  - 同じ数値でも、Pre-release → Release に変わった場合は再度通知されます。
- 新しいバージョンが見つかった場合:
  - 安定版: 通常の案内メッセージを表示。
  - プレリリース版: ベータ版であることを示す警告メッセージを表示。
  - ダウンロード確認: `今すぐダウンロードして更新しますか？ (y/n) >`
- `y` を選択した場合:
  - GitHub Release の最初のアセット（通常は exe）を一時フォルダにダウンロード。
  - SlafightInstaller.Updater.exe を起動し、現在の exe を安全に置き換えます。
  - 新しい SlafightInstaller.exe が自動的に起動します。

よくある質問
------------

Q. このツールはどこに何を書き込みますか？  
A. 初期状態ではゲームフォルダ内（BepInEx/plugins 等）のみを書き換えます。
   カスタム MOD 情報は `%AppData%\SlafightInstaller\custom_mods.json` に保存されます。

Q. アップデート時に .bak ファイルができますが、これは何ですか？  
A. 自動更新時に旧バージョンの exe を `*.bak` としてバックアップしているファイルです。
   問題なければ手動で削除して構いません。

Q. 管理者権限は必要ですか？  
A. ゲームフォルダが Program Files 配下など書き込み制限のある場所にある場合は、
   管理者権限が必要になる可能性があります。

ライセンス / 注意
-----------------
- 本ツールはUnlicenseのライセンスで公開・配布されます。詳しい事はLICENSE.txtを参照してください。
- MOD の利用は各 MOD の配布元の利用規約に従ってください。
- 本ツールの利用によって生じた損害について、作者は一切の責任を負いません。
- 不具合や要望があれば、GitHub の Issue から報告してください。
