using System;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace SlafightInstaller
{
    public static class BasicUtils
    {
        public static bool IsValidPath(string? path)
        {
            if (string.IsNullOrWhiteSpace(path)) return false;
            return !path.Any(c => Path.GetInvalidPathChars().Contains(c));
        }

        public static void ExtractOverwrite(string zipPath, string destPath, string? subPath = null,
            string[]? skipFileNames = null)
        {
            skipFileNames ??= new[] { "README.md", "manifest.json", "icon.png", "CHANGELOG.md" };

            var prefix = string.IsNullOrEmpty(subPath)
                ? null
                : subPath.Replace('\\', '/').TrimEnd('/') + '/';

            using var archive = ZipFile.OpenRead(zipPath);
            foreach (var entry in archive.Entries)
            {
                if (skipFileNames.Contains(entry.Name, StringComparer.OrdinalIgnoreCase))
                    continue;

                var entryPath = entry.FullName.Replace('\\', '/');

                string relativePath;
                if (prefix != null)
                {
                    if (!entryPath.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                        continue;
                    relativePath = entryPath.Substring(prefix.Length);
                }
                else
                {
                    relativePath = entryPath;
                }

                if (string.IsNullOrEmpty(relativePath))
                    continue;

                var fullPath = Path.Combine(destPath, relativePath.Replace('/', Path.DirectorySeparatorChar));
                var dir = Path.GetDirectoryName(fullPath);
                if (!string.IsNullOrEmpty(dir))
                    Directory.CreateDirectory(dir);

                if (string.IsNullOrEmpty(entry.Name))
                    continue;

                entry.ExtractToFile(fullPath, overwrite: true);
            }
        }

        public static void EndScreen()
        {
            Console.WriteLine("Press any key to exit... / 何かキーを押して閉じる...");
            Console.ReadKey();
        }
    }
}