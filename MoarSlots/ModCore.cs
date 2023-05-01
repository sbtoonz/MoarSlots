using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using ServerSync;

namespace MoarSlots
{
    [BepInPlugin(ModGUID, ModName, ModVersion)]
    public class MoarSlotsMod : BaseUnityPlugin
    {
        private const string ModName = "MoarSlotsMod";
        private const string ModVersion = "1.0";
        private const string ModGUID = "MoarSlotsMod";
        private static Harmony harmony = null!;
        ConfigSync configSync = new(ModGUID) 
            { DisplayName = ModName, CurrentVersion = ModVersion, MinimumRequiredVersion = ModVersion};
        internal static ConfigEntry<bool> ServerConfigLocked = null!;
        ConfigEntry<T> config<T>(string group, string name, T value, ConfigDescription description, bool synchronizedSetting = true)
        {
            ConfigEntry<T> configEntry = Config.Bind(group, name, value, description);

            SyncedConfigEntry<T> syncedConfigEntry = configSync.AddConfigEntry(configEntry);
            syncedConfigEntry.SynchronizedConfig = synchronizedSetting;

            return configEntry;
        }
        ConfigEntry<T> config<T>(string group, string name, T value, string description, bool synchronizedSetting = true) => config(group, name, value, new ConfigDescription(description), synchronizedSetting);

        internal static ConfigEntry<int>? Slots;

        public void Awake()
        {
            Slots = config("1 - General", "Number of rows for player to have", 4,
                new ConfigDescription("Set this to the number of rows you want your inventory to have",
                    new AcceptableValueRange<int>(4, 16)));
            Assembly assembly = Assembly.GetExecutingAssembly();
            harmony = new(ModGUID);
            harmony.PatchAll(assembly);
            ServerConfigLocked = config("1 - General", "Lock Configuration", true, "If on, the configuration is locked and can be changed by server admins only.");
            configSync.AddLockingConfigEntry(ServerConfigLocked);
            
        }
    }
}
