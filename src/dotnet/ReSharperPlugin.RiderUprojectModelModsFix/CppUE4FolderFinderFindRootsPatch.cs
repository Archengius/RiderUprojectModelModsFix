using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using JetBrains.Annotations;
using JetBrains.ReSharper.Feature.Services.Cpp.ProjectModel.UE4;
using JetBrains.Util;
using JetBrains.Util.Logging;

namespace ReSharperPlugin.RiderUprojectModelModsFix;

[HarmonyPatch(typeof(CppUE4FolderFinder), "FindSourceAndPluginRootsIn")]
public class CppUE4FolderFinderFindRootsPatch
{
    private static MethodInfo enumeratorMoveNextMethod = SymbolExtensions.GetMethodInfo<IEnumerator, bool>(enumerator => enumerator.MoveNext());
    private static MethodInfo fixupFindProjectRootsMethod = SymbolExtensions.GetMethodInfo(() => PatchTrampolines.FixupFindProjectRoots(null, null));
    
    [UsedImplicitly]
    static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var foundInjectionSite = false;
        foreach (var instruction in instructions)
        {
            if (instruction.opcode == OpCodes.Callvirt && instruction.operand.Equals(enumeratorMoveNextMethod) && !foundInjectionSite)
            {
                yield return new CodeInstruction(OpCodes.Dup);
                yield return new CodeInstruction(OpCodes.Ldarg_0);
                yield return new CodeInstruction(OpCodes.Call, fixupFindProjectRootsMethod);
                RiderPluginPatcherActivity.logger.Info("Found CppUE4FolderFinder.FindSourceAndPluginRootsIn injection site successfully");
                foundInjectionSite = true;
            }
            yield return instruction;
        }

        if (!foundInjectionSite)
        {
            RiderPluginPatcherActivity.logger.Error("Failed to find IEnumarator.Next call in CppUE4FolderFinder.FindSourceAndPluginRootsIn");
        }
    }
}