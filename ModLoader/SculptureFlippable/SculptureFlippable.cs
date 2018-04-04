using Harmony;

namespace SculptureFlippable
{
    [HarmonyPatch(typeof(SculptureConfig), "CreateBuildingDef")]
    public static class SculptureFlippable
    {
        public static void Postfix(SculptureConfig __instance, ref BuildingDef __result)
        {
            __result.PermittedRotations = PermittedRotations.FlipH;
        }
    }
}
