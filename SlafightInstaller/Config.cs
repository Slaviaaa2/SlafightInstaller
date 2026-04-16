using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SlafightInstaller
{
    public static class Config
    {
        private static readonly string SavePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "SlafightInstaller", "config.json"
        );

        public static string? Language { get; set; }
        public static string? LastSelectedGame { get; set; }

        private static Dictionary<string, string> _gamePaths = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        public static string? GetGamePath(string gameName)
            => _gamePaths.TryGetValue(gameName, out var p) ? p : null;

        public static void SetGamePath(string gameName, string path)
        {
            _gamePaths[gameName] = path;
            Save();
        }

        public static void Load()
        {
            if (!File.Exists(SavePath)) return;

            try
            {
                var json = File.ReadAllText(SavePath, Encoding.UTF8);
                Language = ReadString(json, "Language");
                LastSelectedGame = ReadString(json, "LastSelectedGame");

                // GamePaths: { "STRAFTAT": "C:\\..." }
                var gpBlock = ExtractObject(json, "GamePaths");
                if (gpBlock != null)
                {
                    foreach (var kvp in ReadAllStrings(gpBlock))
                        _gamePaths[kvp.Key] = kvp.Value;
                }
            }
            catch (Exception ex)
            {
                ConsoleUI.Warn($"Failed to load config: {ex.Message}");
            }
        }

        public static void Save()
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(SavePath)!);
                var sb = new StringBuilder();
                sb.AppendLine("{");
                sb.AppendLine($"  \"Language\": {Quote(Language)},");
                sb.AppendLine($"  \"LastSelectedGame\": {Quote(LastSelectedGame)},");
                sb.AppendLine("  \"GamePaths\": {");
                var pairs = _gamePaths.ToList();
                for (int i = 0; i < pairs.Count; i++)
                {
                    var comma = i < pairs.Count - 1 ? "," : "";
                    sb.AppendLine($"    {Quote(pairs[i].Key)}: {Quote(pairs[i].Value)}{comma}");
                }
                sb.AppendLine("  }");
                sb.AppendLine("}");
                File.WriteAllText(SavePath, sb.ToString(), Encoding.UTF8);
            }
            catch (Exception ex)
            {
                ConsoleUI.Warn($"Failed to save config: {ex.Message}");
            }
        }

        // --- JSON helpers ---

        private static string Quote(string? s)
            => s == null ? "null" : $"\"{s.Replace("\\", "\\\\").Replace("\"", "\\\"")}\"";

        private static string? ReadString(string json, string key)
        {
            var match = Regex.Match(json, $"\"{Regex.Escape(key)}\"\\s*:\\s*(?:\"((?:[^\"\\\\]|\\\\.)*)\"|(null))");
            if (!match.Success || match.Groups[2].Success) return null;
            return Unescape(match.Groups[1].Value);
        }

        private static string? ExtractObject(string json, string key)
        {
            var search = $"\"{key}\"";
            var idx = json.IndexOf(search, StringComparison.Ordinal);
            if (idx < 0) return null;
            idx += search.Length;
            while (idx < json.Length && json[idx] != '{') idx++;
            if (idx >= json.Length) return null;

            int depth = 0;
            for (int i = idx; i < json.Length; i++)
            {
                if (json[i] == '{') depth++;
                else if (json[i] == '}') { depth--; if (depth == 0) return json.Substring(idx, i - idx + 1); }
            }
            return null;
        }

        private static Dictionary<string, string> ReadAllStrings(string json)
        {
            var result = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            var matches = Regex.Matches(json, "\"((?:[^\"\\\\]|\\\\.)*)\"\\s*:\\s*\"((?:[^\"\\\\]|\\\\.)*)\"");
            foreach (Match m in matches)
                result[Unescape(m.Groups[1].Value)] = Unescape(m.Groups[2].Value);
            return result;
        }

        private static string Unescape(string s)
            => s.Replace("\\\"", "\"").Replace("\\\\", "\\").Replace("\\n", "\n").Replace("\\r", "\r");
    }
}
