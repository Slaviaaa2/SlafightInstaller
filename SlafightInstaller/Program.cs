using System;
using System.Collections.Generic;
using SlafightInstaller.Games.STRAFTAT;

namespace SlafightInstaller
{
    internal class Program
    {
        public static List<string> games = new()
        {
            "STRAFTAT",
        };

        public static void Main(string[] args)
        {
            if (Environment.OSVersion.Platform != PlatformID.Win32NT)
            {
                Console.WriteLine("You need to run this program as a Windows NT.");
                return;
            }

            if (!Environment.Is64BitOperatingSystem)
            {
                Console.WriteLine("You need to run this program as a 64-bit operating system.");
                return;
            }

            Console.Write("Language / 言語 (en/jp): ");
            var langInput = Console.ReadLine()?.Trim().ToLower();
            Messages.Current = langInput == "jp" ? Lang.Jp : Lang.En;

            Console.WriteLine(Messages.Get("Welcome"));
            Console.WriteLine(Messages.Get("SelectGame"));
            Console.WriteLine(string.Join(",\n", games));

            Console.Write(Messages.Get("GameName"));
            var userInput = Console.ReadLine();

            if (!games.Contains(userInput))
            {
                Console.WriteLine(Messages.Get("InvalidGame"));
                return;
            }

            switch (userInput)
            {
                case "STRAFTAT":
                    STRAFTAT.Entry();
                    break;
            }
        }
    }
}