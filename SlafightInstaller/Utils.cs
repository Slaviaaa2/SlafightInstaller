using System.IO;
using System.IO.Compression;
using System.Linq;

namespace SlafightInstaller
{
    public static class Utils
    {
        public static bool IsValidPath(string? path)
        {
            if (string.IsNullOrWhiteSpace(path)) return false;
            return !path.Any(c => Path.GetInvalidPathChars().Contains(c));
        }
        public static void ExtractOverwrite(string zipPath, string destPath)
        {
            using var archive = ZipFile.OpenRead(zipPath);
            foreach (var entry in archive.Entries)
            {
                var fullPath = Path.Combine(destPath, entry.FullName);
                var dir = Path.GetDirectoryName(fullPath);
                if (!string.IsNullOrEmpty(dir))
                    Directory.CreateDirectory(dir);

                // ディレクトリエントリはスキップ
                if (string.IsNullOrEmpty(entry.Name))
                    continue;

                // 上書き true
                entry.ExtractToFile(fullPath, overwrite: true);
            }
        }
    }
}