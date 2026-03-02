using System;
using System.Linq;

namespace SlafightInstaller
{
    public static class ConsoleUI
    {
        public static int DisplayWidth(string s)
        {
            return s.Sum(c => IsFullWidth(c) ? 2 : 1);
        }

        private static bool IsFullWidth(char c)
        {
            return (c >= 0x1100 && c <= 0x115F)
                || (c >= 0x2E80 && c <= 0x303E)
                || (c >= 0x3041 && c <= 0x33FF)
                || (c >= 0x3400 && c <= 0x4DBF)
                || (c >= 0x4E00 && c <= 0xA4CF)
                || (c >= 0xA960 && c <= 0xA97F)
                || (c >= 0xAC00 && c <= 0xD7FF)
                || (c >= 0xF900 && c <= 0xFAFF)
                || (c >= 0xFE10 && c <= 0xFE1F)
                || (c >= 0xFE30 && c <= 0xFE6F)
                || (c >= 0xFF01 && c <= 0xFF60)
                || (c >= 0xFFE0 && c <= 0xFFE6);
        }

        private static string PadRightDisplay(string s, int totalWidth)
        {
            var pad = totalWidth - DisplayWidth(s);
            return pad > 0 ? s + new string(' ', pad) : s;
        }

        public static void Info(string message)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public static void Success(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public static void Warn(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public static void Error(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public static void Prompt(string message)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(message);
            Console.ResetColor();
        }
        
        public static void Debug(string message, bool forceShow = false)
        {
            if (forceShow)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray; 
                Console.WriteLine(message);
                Console.ResetColor();
            }
            else
            {
                #if DEBUG
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine(message);
                    Console.ResetColor();
                #endif
            }
        }

        public static void Header(string message)
        {
            var innerWidth = DisplayWidth(message) + 2;
            var border = new string('═', innerWidth);
            var padded = PadRightDisplay(" " + message, innerWidth);

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"╔{border}╗");
            Console.WriteLine($"║{padded}║");
            Console.WriteLine($"╚{border}╝");
            Console.ResetColor();
        }

        public static void Divider()
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(new string('─', 44));
            Console.ResetColor();
        }

        public static void ModEntry(string name, string version, string deps)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"  * {name}");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write($"@{version}");
            if (!string.IsNullOrEmpty(deps))
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write($"  (deps: {deps})");
            }
            Console.WriteLine();
            Console.ResetColor();
        }
    }
}