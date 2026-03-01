using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SlafightInstaller
{
    /// <summary>
    /// ユーザー定義のカスタムModBaseをJSONで管理する
    /// 保存先: %AppData%/SlafightInstaller/custom_mods.json
    /// </summary>
    public static class CustomModRegistry
    {
        private static readonly string SavePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "SlafightInstaller", "custom_mods.json"
        );

        private static List<ModBase> _customMods = new List<ModBase>();
        public static IReadOnlyList<ModBase> CustomMods => _customMods;

        /// <summary>
        /// 起動時に自動ロード
        /// </summary>
        public static void Load()
        {
            if (!File.Exists(SavePath))
            {
                _customMods = new List<ModBase>();
                return;
            }

            try
            {
                var json = File.ReadAllText(SavePath, Encoding.UTF8);
                _customMods = SimpleJson.DeserializeList(json);
                ConsoleUI.Info($"Custom mods loaded: {_customMods.Count} mod(s) from {SavePath}");
            }
            catch (Exception ex)
            {
                ConsoleUI.Warn($"Failed to load custom mods: {ex.Message}");
                _customMods = new List<ModBase>();
            }
        }

        /// <summary>
        /// 保存
        /// </summary>
        public static void Save()
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(SavePath)!);
                var json = SimpleJson.SerializeList(_customMods);
                File.WriteAllText(SavePath, json, Encoding.UTF8);
                ConsoleUI.Success($"Custom mods saved: {SavePath}");
            }
            catch (Exception ex)
            {
                ConsoleUI.Error($"Failed to save custom mods: {ex.Message}");
            }
        }

        /// <summary>
        /// MODを追加または上書き
        /// </summary>
        public static void AddOrUpdate(ModBase mod)
        {
            var idx = _customMods.FindIndex(m => m.ModName == mod.ModName);
            if (idx >= 0)
                _customMods[idx] = mod;
            else
                _customMods.Add(mod);
            Save();
        }

        /// <summary>
        /// MODを削除
        /// </summary>
        public static bool Remove(string modName)
        {
            var idx = _customMods.FindIndex(m => m.ModName == modName);
            if (idx < 0) return false;
            _customMods.RemoveAt(idx);
            Save();
            return true;
        }

        /// <summary>
        /// 一覧表示
        /// </summary>
        public static void PrintList()
        {
            if (_customMods.Count == 0)
            {
                ConsoleUI.Info("No custom mods registered.");
                return;
            }
            ConsoleUI.Divider();
            ConsoleUI.Info("Custom Mods:");
            foreach (var m in _customMods)
            {
                var deps = m.ModDependencies != null && m.ModDependencies.Count > 0
                    ? string.Join(", ", m.ModDependencies) : "";
                ConsoleUI.ModEntry(m.ModName, m.ModVersion, deps);
            }
            ConsoleUI.Divider();
        }
    }

    /// <summary>
    /// System.Text.Json非依存の最小JSONシリアライザ (NET48対応)
    /// ModBaseのリストのみ対象
    /// </summary>
    internal static class SimpleJson
    {
        public static string SerializeList(List<ModBase> mods)
        {
            var sb = new StringBuilder();
            sb.AppendLine("[");
            for (int i = 0; i < mods.Count; i++)
            {
                var m = mods[i];
                sb.AppendLine("  {");
                sb.AppendLine($"    \"ModName\": {Quote(m.ModName)},");
                sb.AppendLine($"    \"ModVersion\": {Quote(m.ModVersion)},");
                sb.AppendLine($"    \"SourceUrl\": {Quote(m.SourceUrl)},");
                sb.AppendLine($"    \"InstallFileName\": {Quote(m.InstallFileName)},");
                sb.AppendLine($"    \"InstallSubPath\": {Quote(m.InstallSubPath)},");
                sb.AppendLine($"    \"ExtractTargetSubPath\": {Quote(m.ExtractTargetSubPath)},");

                // ModDependencies
                sb.Append("    \"ModDependencies\": [");
                if (m.ModDependencies != null && m.ModDependencies.Count > 0)
                {
                    var depParts = new List<string>();
                    foreach (var d in m.ModDependencies)
                        depParts.Add($"{{\"ModName\":{Quote(d.ModName)},\"RequiredVersion\":{Quote(d.RequiredVersion)}}}");
                    sb.Append(string.Join(", ", depParts));
                }
                sb.AppendLine("],");

                // ConflictsWith
                sb.Append("    \"ConflictsWith\": [");
                if (m.ConflictsWith != null && m.ConflictsWith.Count > 0)
                    sb.Append(string.Join(", ", m.ConflictsWith.ConvertAll(Quote)));
                sb.AppendLine("]");

                sb.Append(i < mods.Count - 1 ? "  }," : "  }");
                sb.AppendLine();
            }
            sb.AppendLine("]");
            return sb.ToString();
        }

        public static List<ModBase> DeserializeList(string json)
        {
            var result = new List<ModBase>();
            // 各オブジェクトブロック { ... } を抽出
            int pos = 0;
            while (true)
            {
                var start = json.IndexOf('{', pos);
                if (start < 0) break;
                var end = FindMatchingBrace(json, start);
                if (end < 0) break;

                var block = json.Substring(start, end - start + 1);
                var mod = ParseModBase(block);
                if (mod.ModName != null)
                    result.Add(mod);

                pos = end + 1;
            }
            return result;
        }

        private static ModBase ParseModBase(string block)
        {
            var mod = new ModBase
            {
                ModName      = ReadString(block, "ModName"),
                ModVersion   = ReadString(block, "ModVersion") ?? "0.0.0",
                SourceUrl    = ReadString(block, "SourceUrl"),
                InstallFileName      = ReadString(block, "InstallFileName"),
                InstallSubPath       = ReadString(block, "InstallSubPath"),
                ExtractTargetSubPath = ReadString(block, "ExtractTargetSubPath"),
                ModDependencies = new List<ModDependency>(),
                ConflictsWith   = new List<string>()
            };

            // ModDependencies: [{"ModName":"...","RequiredVersion":"..."},...]
            var depsRaw = ReadArray(block, "ModDependencies");
            foreach (var item in depsRaw)
            {
                var name = ReadString(item, "ModName");
                var ver  = ReadString(item, "RequiredVersion");
                if (name != null)
                    mod.ModDependencies.Add(new ModDependency(name, ver));
            }

            // ConflictsWith: ["...",...]
            var conflictsRaw = ReadStringArray(block, "ConflictsWith");
            mod.ConflictsWith.AddRange(conflictsRaw);

            return mod;
        }

        // ---- helpers ----

        private static string Quote(string? s)
            => s == null ? "null" : $"\"{s.Replace("\\", "\\\\").Replace("\"", "\\\"")}\"";

        private static string? ReadString(string json, string key)
        {
            var search = $"\"{key}\"";
            var idx = json.IndexOf(search);
            if (idx < 0) return null;

            idx += search.Length;
            // skip whitespace and colon
            while (idx < json.Length && (json[idx] == ' ' || json[idx] == ':' || json[idx] == '\t')) idx++;
            if (idx >= json.Length) return null;

            if (json[idx] == 'n') return null; // null
            if (json[idx] != '"') return null;

            idx++; // skip opening quote
            var sb = new StringBuilder();
            while (idx < json.Length && json[idx] != '"')
            {
                if (json[idx] == '\\' && idx + 1 < json.Length)
                {
                    idx++;
                    sb.Append(json[idx] == 'n' ? '\n' : json[idx]);
                }
                else
                    sb.Append(json[idx]);
                idx++;
            }
            return sb.ToString();
        }

        private static List<string> ReadStringArray(string json, string key)
        {
            var result = new List<string>();
            var search = $"\"{key}\"";
            var idx = json.IndexOf(search);
            if (idx < 0) return result;

            idx += search.Length;
            while (idx < json.Length && json[idx] != '[') idx++;
            if (idx >= json.Length) return result;

            var end = json.IndexOf(']', idx);
            if (end < 0) return result;

            var inner = json.Substring(idx + 1, end - idx - 1);
            int p = 0;
            while (p < inner.Length)
            {
                var qs = inner.IndexOf('"', p);
                if (qs < 0) break;
                var qe = inner.IndexOf('"', qs + 1);
                if (qe < 0) break;
                result.Add(inner.Substring(qs + 1, qe - qs - 1));
                p = qe + 1;
            }
            return result;
        }

        private static List<string> ReadArray(string json, string key)
        {
            var result = new List<string>();
            var search = $"\"{key}\"";
            var idx = json.IndexOf(search);
            if (idx < 0) return result;

            idx += search.Length;
            while (idx < json.Length && json[idx] != '[') idx++;
            if (idx >= json.Length) return result;

            // find matching ]
            int depth = 0;
            int start = idx;
            int end = idx;
            for (int i = idx; i < json.Length; i++)
            {
                if (json[i] == '[') depth++;
                else if (json[i] == ']') { depth--; if (depth == 0) { end = i; break; } }
            }

            var inner = json.Substring(start + 1, end - start - 1).Trim();
            if (string.IsNullOrWhiteSpace(inner)) return result;

            // split by top-level { }
            int p = 0;
            while (p < inner.Length)
            {
                var s = inner.IndexOf('{', p);
                if (s < 0) break;
                var e = FindMatchingBrace(inner, s);
                if (e < 0) break;
                result.Add(inner.Substring(s, e - s + 1));
                p = e + 1;
            }
            return result;
        }

        private static int FindMatchingBrace(string s, int open)
        {
            int depth = 0;
            bool inString = false;
            for (int i = open; i < s.Length; i++)
            {
                if (s[i] == '"' && (i == 0 || s[i - 1] != '\\')) inString = !inString;
                if (inString) continue;
                if (s[i] == '{') depth++;
                else if (s[i] == '}') { depth--; if (depth == 0) return i; }
            }
            return -1;
        }
    }
}