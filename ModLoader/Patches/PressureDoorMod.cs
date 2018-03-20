namespace Patches
{
    using Harmony;

    [HarmonyPatch(typeof(PressureDoorConfig), "CreateBuildingDef")]
    public static class PressureDoorMod
    {
        public static void Postfix(PressureDoorConfig __instance, BuildingDef __result)
        {
            Debug.Log(" === PressureDoorMod INI === ");
            __result.RequiresPowerInput          = false;
            __result.EnergyConsumptionWhenActive = 0f;

            // Traverse.Create<CameraController>().Property("maxOrthographicSize").SetValue(100.0);
            // Traverse.Create<CameraController>().Property("maxOrthographicSizeDebug").SetValue(200.0);
            Debug.Log(" === PressureDoorMod END === ");
        }
    }
}