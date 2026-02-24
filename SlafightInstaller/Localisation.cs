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

        public static readonly Dictionary<string, string> En = new()
        {
            ["Welcome"]        = "Welcome to the Slafight Installer!",
            ["SelectGame"]     = "Please select target game:",
            ["GameName"]       = "Game Name: ",
            ["InvalidGame"]    = "Please select valid game!",

            ["EnterGamePath"]  = "[STRAFTAT]Please enter Game Path: ",
            ["InvalidGamePath"]= "[STRAFTAT]Please enter a valid Game Path!",
            ["UsableMods"]     = "[STRAFTAT]Usable Mods:",
            ["EnterModName"]   = "[STRAFTAT]Please enter mod name: ",
            ["InvalidModName"] = "[STRAFTAT]Please enter a valid mod name!",

            ["OverwriteAsk"]   = "[STRAFTAT]File already exists. Overwrite? (y/n): ",
            ["OverwriteSkip"]  = "[STRAFTAT]Skip installing because file already exists.",

            ["InstallFailed"]  = "[STRAFTAT]Install failed: ",
        };

        public static readonly Dictionary<string, string> Jp = new()
        {
            ["Welcome"]        = "Slafight Installer へようこそ！",
            ["SelectGame"]     = "対象ゲームを選択してください:",
            ["GameName"]       = "ゲーム名: ",
            ["InvalidGame"]    = "有効なゲームを選択してください！",

            ["EnterGamePath"]  = "[STRAFTAT]ゲームパスを入力してください: ",
            ["InvalidGamePath"]= "[STRAFTAT]正しいゲームパスを入力してください！",
            ["UsableMods"]     = "[STRAFTAT]使用可能なMOD:",
            ["EnterModName"]   = "[STRAFTAT]MOD名を入力してください: ",
            ["InvalidModName"] = "[STRAFTAT]正しいMOD名を入力してください！",

            ["OverwriteAsk"]   = "[STRAFTAT]ファイルが既に存在します。上書きしますか？ (y/n): ",
            ["OverwriteSkip"]  = "[STRAFTAT]ファイルが存在するため、このインストールをスキップしました。",

            ["InstallFailed"]  = "[STRAFTAT]インストールに失敗しました: ",
        };
    }
}
