using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using MonoMod.RuntimeDetour.HookGen;
using Sprinterviews.Patches;

namespace Sprinterviews
{
    [ContentWarningPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_VERSION, false)]
    [BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
    public class SprinterviewsCore : BaseUnityPlugin
    {
        public static SprinterviewsCore Instance { get; private set; } = null!;
        internal new static ManualLogSource Logger { get; private set; } = null!;
        public static Config ConfigFile = new();

        private void Awake()
        {
            Logger = base.Logger;
            Instance = this;

            HookAll();

            Logger.LogInfo($"{MyPluginInfo.PLUGIN_GUID} v{MyPluginInfo.PLUGIN_VERSION} has loaded!");

            ConfigFile.LoadFile();
        }

        internal static void HookAll()
        {
            Logger.LogDebug("Hooking...");

            SprintPatch.Init();

            Logger.LogDebug("Finished Hooking!");
        }

        internal static void UnhookAll()
        {
            Logger.LogDebug("Unhooking...");

            HookEndpointManager.RemoveAllOwnedBy(Assembly.GetExecutingAssembly());

            Logger.LogDebug("Finished Unhooking!");
        }
    }
}
