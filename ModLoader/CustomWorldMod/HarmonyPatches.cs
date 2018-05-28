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
                                                                               "Custom size always uses 12x16",
                                                                               //     "Use custom world size",
                                                                               "Default 8x12; If enabled, the world will be generated with the values provided",
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
                                                                              new SettingLevel("8",  "8",  "8"),
                                                                              new SettingLevel("10", "10", "10"),
                                                                              new SettingLevel("12", "12", "12"),
                                                                              new SettingLevel("14", "14", "14"),
                                                                              new SettingLevel("16", "16", "16")
                                                                              }, "8");

            public static readonly SettingConfig WorldgenSeedY = new ListSettingConfig(WorldsizeY, "Custom World Height", "Use a custom size.",
                                                                              new List<SettingLevel>
                                                                              {
                                                                              new SettingLevel("12", "12", "12"),
                                                                              new SettingLevel("16", "16", "16"),
                                                                              new SettingLevel("20", "20", "20"),
                                                                              new SettingLevel("24", "24", "24")
                                                                              }, "12");

            public static void Prefix(CustomGameSettings __instance, SettingConfig config)
            {
				Debug.Log(" === CustomGameSettingsMod INI === ");
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
				Debug.Log(" === GridSettings_ResetMod INI === ");

				// 8x12 default
				//    if ()
				{
                    width = 12 * 32;
                    height = 16 * 32;
                }
                return;
                //  ModConfig Config = new ModConfig(ModName);

                //   return;

                if (Game.Instance == null)
                {
                    return;
                }

                SettingConfig settingConfig = Game.Instance.customSettings.QualitySettings[UseCustomWorldSize];
                SettingLevel currentQualitySetting =
                Game.Instance.customSettings.GetCurrentQualitySetting(UseCustomWorldSize);

                bool allowCustomSize = settingConfig?.IsDefaultLevel(currentQualitySetting.id) == true;

                if (!allowCustomSize)
                {
                    return;
                }

                SettingLevel currentQualitySettingX = Game.Instance.customSettings.GetCurrentQualitySetting(WorldsizeX);
                SettingLevel currentQualitySettingY = Game.Instance.customSettings.GetCurrentQualitySetting(WorldsizeY);
                if (Int32.TryParse(currentQualitySettingX.id, out width))
                {
                    width *= 32;
                }
                if (Int32.TryParse(currentQualitySettingY.id, out height))
                {
                    height *= 32;
                }

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