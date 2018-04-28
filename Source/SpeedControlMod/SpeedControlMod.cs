namespace SpeedControlMod
{
    using Harmony;
    using System;
    using UnityEngine;
    using Debug = Debug;

    [HarmonyPatch(typeof(SpeedControlScreen), "OnChanged", new Type[0])]
    internal static class SpeedControlMod
    {
        private static bool Prefix(SpeedControlScreen __instance)
        {
            Debug.Log(" === SpeedControlMod INI === ");

            if (__instance.IsPaused)
            {
                Time.timeScale = 0f;
            }
            else if (__instance.GetSpeed() == 0)
            {
                Time.timeScale = __instance.normalSpeed;
            }
            else if (__instance.GetSpeed() == 1)
            {
                Time.timeScale = __instance.fastSpeed;
            }
            else if (__instance.GetSpeed() == 2)
            {
                Time.timeScale = 10f;
            }

            __instance.OnGameSpeedChanged?.Invoke();

            Debug.Log(" === SpeedControlMod END === ");

            return false;
        }
    }
}