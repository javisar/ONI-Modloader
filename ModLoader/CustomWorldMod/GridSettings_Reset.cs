using System;
using Harmony;
using Klei.CustomSettings;

namespace CustomWorldMod
{
    public static partial class HarmonyPatches
    {
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
    }
}