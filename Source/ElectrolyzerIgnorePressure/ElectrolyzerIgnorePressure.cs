using Harmony;

namespace ElectrolyzerIgnorePressure
{
    [HarmonyPatch(typeof(Electrolyzer), "OverPressure")]
    internal static class ElectrolyzerIgnorePressure
    {
        private static void Postfix(Electrolyzer __instance, ref bool __result, ref int cell)
        {
            //ElectrolyzerComplete
            //MineralDeoxidizerComplete           
            if (__instance.name == "ElectrolyzerComplete")
            {
                __result = false;
            }
        }
    }
}
