using System.Collections.Generic;
using JetBrains.Annotations;
using JetBrains.ReSharper.Feature.Services.Cpp.ProjectModel.UE4;
using JetBrains.Util;

namespace ReSharperPlugin.RiderUprojectModelModsFix;

public class PatchTrampolines
{
    [UsedImplicitly]
    public static void FixupGetProjectRoots(UnrealProjectRoots SourceProjectRoots)
    {
        var RootDir = SourceProjectRoots.PluginsRoots[0].Parent;
        var ModsDir = RootDir / "Mods";
        SourceProjectRoots.PluginsRoots.Add(ModsDir);
    }

    [UsedImplicitly]
    public static void FixupFindProjectRoots(IEnumerator<VirtualFileSystemPath> ChildDirectoryEnumerator, UnrealProjectRoots ResultProjectRoots)
    {
        // Will be null before the first iteration due to the jump in the generated code
        if (ChildDirectoryEnumerator.Current != null)
        {
            ResultProjectRoots.PluginsRoots.Add(ChildDirectoryEnumerator.Current / "Mods");
        }
    }
}