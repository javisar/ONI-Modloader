using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Harmony;
using Klei.CustomSettings;

namespace CustomWorldMod
{
    
        [HarmonyPatch(typeof(CustomGameSettings), nameof(CustomGameSettings.AddSettingConfig))]
        public static class CustomWorldMod
        {
            public static SettingConfig UseCustomWorld = new ToggleSettingConfig(UseCustomWorldSize,
                                                                                    "Use custom world size",
                                                                               "Default 256x384 tiles; If enabled, the world will be generated with the values provided",
                                                                               new SettingLevel("Disabled",
                                                                                                "Name01",
                                                                                                "Default 256"),
                                                                               new SettingLevel("Active",
                                                                                                "Name02",
                                                                                                "Default 384"),
                                                                               "Disabled");
            public const string UseCustomWorldSize = "UseCustomWorldSize";
            public const string WorldsizeX         = "WorldSizeX";
            public const string WorldsizeY         = "WorldSizeY";

        public static SettingConfig WorldgenSeedX;

            public static SettingConfig WorldgenSeedY;
            /// <summary>
            /// Adds the settings before the immunity
            /// </summary>
            /// <param name="__instance"></param>
            /// <param name="config"></param>
            public static void Prefix(CustomGameSettings __instance, SettingConfig config)
            {
                if (config != CustomGameSettingConfigs.ImmuneSystem)
                {
                    return;
                }

                var worldListX = new List<SettingLevel>();
                for (int i = 256; i <= 1024; i+=32)
                {
                    worldListX.Add(new SettingLevel(i.ToString(), i.ToString(), "Default: 256"));
                }
                var worldListY = new List<SettingLevel>();
                for (int i = 384; i <= 1024; i += 32)
                {
                    worldListY.Add(new SettingLevel(i.ToString(), i.ToString(), "Default: 384"));
                }

                WorldgenSeedX =  new ListSettingConfig(WorldsizeX, "Custom World Width ", "Use a custom size.",
                                                        worldListX, "256");

                WorldgenSeedY = new ListSettingConfig(WorldsizeY, "Custom World Height", "Use a custom size.",
                                                      worldListY, "384");

                List<SettingConfig> settings = new List<SettingConfig> { UseCustomWorld, WorldgenSeedX, WorldgenSeedY };
                foreach (SettingConfig settingConfig in settings)
                {
                    __instance.QualitySettings.Add(settingConfig.id, settingConfig);
                    if (!__instance.CurrentQualityLevelsBySetting.ContainsKey(settingConfig.id) || string.IsNullOrEmpty(__instance.CurrentQualityLevelsBySetting[settingConfig.id]))
                    {
                        __instance.CurrentQualityLevelsBySetting[settingConfig.id] = settingConfig.default_level_id;
                    }
                }
            }
        }

        // [HarmonyPatch(typeof(NewGameSettingsScreen),"OnSpawn")]
        // public static class NewGameSettingsScreen_
        // {
        //     public static void Postfix() { }
        // }

    
}