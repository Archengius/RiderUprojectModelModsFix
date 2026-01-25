using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using JetBrains.Annotations;
using JetBrains.ReSharper.Feature.Services.Cpp.ProjectModel.UE4;
using JetBrains.Util;
using JetBrains.Util.Logging;

namespace ReSharperPlugin.RiderUprojectModelModsFix;

[HarmonyPatch(typeof(CppUE4FolderFinder), "GetSourceAndPluginsRoots")]
public class CppUE4FolderFinderGetRootsPatch
{
    private static MethodInfo fixupGetProjectRootsMethod = SymbolExtensions.GetMethodInfo(() => PatchTrampolines.FixupGetProjectRoots(null));
    
    [UsedImplicitly]
    static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var foundInjectionSite = false;
        foreach (var instruction in instructions)
        {
            yield return instruction;
            if (instruction.opcode == OpCodes.Newobj && 
                ((MethodBase) instruction.operand).DeclaringType == typeof(UnrealProjectRoots) &&
                !foundInjectionSite)
            {
                yield return new CodeInstruction(OpCodes.Dup);
                yield return new CodeInstruction(OpCodes.Call, fixupGetProjectRootsMethod);
                RiderPluginPatcherActivity.logger.Info("Found CppUE4FolderFinder.GetSourceAndPluginsRoots injection site successfully");
                foundInjectionSite = true;
            }
        }

        if (!foundInjectionSite)
        {
            RiderPluginPatcherActivity.logger.Error("Failed to find UnrealProjectRoots instantiation in CppUE4FolderFinder.GetSourceAndPluginsRoots");
        }
    }
}