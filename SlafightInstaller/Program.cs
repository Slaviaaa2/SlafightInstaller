using System;
using System.Collections.Generic;
using SlafightInstaller.Games;
using SlafightInstaller.Games.STRAFTAT;

namespace SlafightInstaller
{
    internal class Program
    {
        public static List<string> games = new List<string>()
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
            string userInput;
            Console.WriteLine("Welcome to the Slafight Installer!");
            Console.WriteLine($"Please select target game:\n{String.Join(",\n", games)}");
            Console.Write("Game Name: ");
            userInput = Console.ReadLine();
            if (!games.Contains(userInput))
            {
                Console.WriteLine("Please select valid game!");
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