using HarmonyLib;
using JetBrains.Application;
using JetBrains.Application.Components;
using JetBrains.Application.Parts;
using JetBrains.Util;
using JetBrains.Util.Logging;

namespace ReSharperPlugin.RiderUprojectModelModsFix;

[ShellComponent(Instantiation.ContainerAsyncPrimaryThread)]
public class RiderPluginPatcherActivity : IStartupActivity
{
    internal static readonly ILogger logger = Logger.GetLogger(typeof(RiderPluginPatcherActivity));
    
    public RiderPluginPatcherActivity()
    {
        var harmony = new Harmony("resharper_plugin.rider_project_model_mods_fix");
        harmony.PatchAll();
    }
}