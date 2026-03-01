using System;
using System.Collections.Generic;

namespace SlafightInstaller
{
    public static class CommandParser
    {
        public class ParsedCommand
        {
            public string Namespace { get; set; }
            public string Action { get; set; }
            public List<string> Args { get; set; } = new List<string>();
            public HashSet<string> Flags { get; set; } = new HashSet<string>();
        }

        public static List<ParsedCommand> Parse(string input)
        {
            var result = new List<ParsedCommand>();
            var parts = input.Split(["&&"], StringSplitOptions.RemoveEmptyEntries);

            foreach (var part in parts)
            {
                var tokens = part.Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (tokens.Length == 0) continue;
                if (!tokens[0].StartsWith("@")) continue;

                var cmd = new ParsedCommand
                {
                    Namespace = tokens[0].TrimStart('@'),
                    Action    = tokens.Length >= 2 ? tokens[1] : string.Empty,
                };

                for (var i = 2; i < tokens.Length; i++)
                {
                    if (tokens[i].StartsWith("--"))
                        cmd.Flags.Add(tokens[i].TrimStart('-').ToLower());
                    else
                        cmd.Args.Add(tokens[i]);
                }

                result.Add(cmd);
            }

            return result;
        }

        public static bool IsCommand(string? input)
        {
            return input?.TrimStart().StartsWith("@") == true;
        }
    }
}