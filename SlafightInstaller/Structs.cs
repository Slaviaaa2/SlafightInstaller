using System.Collections.Generic;

namespace SlafightInstaller;

public struct ModBase
{
    public string ModName;
    public string ModVersion;
    public List<string> ModDependencies;
    
    public override string ToString()
    {
        // 一覧表示用
        var deps = (ModDependencies != null && ModDependencies.Count > 0)
            ? $" (Deps: {string.Join(", ", ModDependencies)})"
            : "";
        return $"{ModName} {ModVersion}{deps}";
    }
}