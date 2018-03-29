using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Harmony;
using Klei.CustomSettings;

namespace CustomWorldMod
{
    public static class HarmonyPatches
    {
        [HarmonyPatch(typeof(CustomGameSettings), nameof(CustomGameSettings.AddSettingConfig))]
        public static class MyClass
        {
            public static SettingConfig UseCustomWorld = new ToggleSettingConfig(UseCustomWorldSize,
                                                                                    "Use custom world size",
                                                                               "Default 256x384 tiles; If enabled, the world will be generated with the values provided",
                                                                               new SettingLevel("Disabled",
                                                                                                "Name01",
                                                                                                "Tooltip01"),
                                                                               new SettingLevel("Active",
                                                                                                "Name02",
                                                                                                "Tooltip02"),
                                                                               "Disabled");

            public static readonly SettingConfig WorldgenSeedX = new ListSettingConfig(WorldsizeX, "Custom World Width ", "Use a custom size.",
                                                                              new List<SettingLevel>
                                                                              {
                                                                              new SettingLevel("256",  "256",  "256"),
                                                                              new SettingLevel("320", "320", "320"),
                                                                              new SettingLevel("384", "384", "384"),
                                                                              new SettingLevel("448", "448", "448"),
                                                                              new SettingLevel("512", "512", "512")
                                                                              }, "256");

            public static readonly SettingConfig WorldgenSeedY = new ListSettingConfig(WorldsizeY, "Custom World Height", "Use a custom size.",
                                                                              new List<SettingLevel>
                                                                              {
                                                                              new SettingLevel("384", "384", "384"),
                                                                              new SettingLevel("512", "512", "512"),
                                                                              new SettingLevel("640", "640", "640"),
                                                                              new SettingLevel("768", "768", "768")
                                                                              }, "384");
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

        public const string UseCustomWorldSize = "UseCustomWorldSize";
        public const string WorldsizeX = "WorldSizeX";
        public const string WorldsizeY = "WorldSizeY";

        [HarmonyPatch(typeof(GridSettings), nameof(GridSettings.Reset))]
        public static class GridSettings_Reset
        {
            public const string ModName = "CustomWorldSize";

            public static void Prefix(ref int width, ref int height)
            {
                // 256x512 default

                Debug.Log("CWS: Using custom world size ...");
                if (!CustomGameSettings.Get().is_custom_game)
                {
                    Debug.Log("CWS: Nah, no custom game ...");
                    return;
                }
                
                SettingConfig settingConfig = CustomGameSettings.Get().QualitySettings[UseCustomWorldSize];
                SettingLevel currentQualitySetting =
                CustomGameSettings.Get().GetCurrentQualitySetting(UseCustomWorldSize);

                bool allowCustomSize = !settingConfig.IsDefaultLevel(currentQualitySetting.id);

                if (!allowCustomSize)
                {
                    Debug.Log("CWS: No custom size allowed ...");
                    return;
                }

                SettingLevel currentQualitySettingX = CustomGameSettings.Get().GetCurrentQualitySetting(WorldsizeX);
                SettingLevel currentQualitySettingY = CustomGameSettings.Get().GetCurrentQualitySetting(WorldsizeY);
                Int32.TryParse(currentQualitySettingX.id, out width);
                Int32.TryParse(currentQualitySettingY.id, out height);

                Debug.Log("CWS: Using " + width + "/" + height + " as new world size");

                //  if (Config.Enabled && Config.CustomWorldSize)
                //{
                //    width  = Config.width;
                //    height = Config.height;
                //}
            }
        }

        public class ModConfig
        {
            private string configName;

            public ModConfig(string configName)
            {
                this.configName = configName;

                this.TryLoadConfigFromFile(configName);
            }

            private void TryLoadConfigFromFile(string s)
            {
                //  var json =
            }
        }
    }
}