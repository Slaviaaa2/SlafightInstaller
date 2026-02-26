using System.Collections.Generic;

namespace SlafightInstaller
{
    public struct ModBase
    {
        public string ModName;
        public string ModVersion;
        public List<ModDependency> ModDependencies;
        public List<string> ConflictsWith;
        public string? SourceUrl;
        public string? InstallSubPath;
        public string? ExtractTargetSubPath;
        public string? InstallFileName;

        public override string ToString()
        {
            var deps = (ModDependencies != null && ModDependencies.Count > 0)
                ? $" (Deps: {string.Join(", ", ModDependencies)})"
                : "";
            return $"{ModName} {ModVersion}{deps}";
        }
    }
    public struct ModDependency
    {
        public string ModName;
        public string? RequiredVersion;

        public ModDependency(string modName, string? requiredVersion = null)
        {
            ModName = modName;
            RequiredVersion = requiredVersion;
        }

        public override string ToString()
        {
            return RequiredVersion != null ? $"{ModName}@{RequiredVersion}" : ModName;
        }
    }
}