namespace Patches
{
    using Harmony;

    // Todo: add options ingamem, make optional
    [HarmonyPatch(typeof(LogicWireBridgeConfig), nameof(LogicWireBridgeConfig.CreateBuildingDef))]
    public static class LogicWireBridgeMod
    {
        public static void Postfix(LogicWireBridgeConfig __instance, BuildingDef __result)
        {
            __result.ObjectLayer = ObjectLayer.Backwall;
        }
    }
}