using JetBrains.Annotations;
using TUNING;

namespace Patches
{
    using Harmony;

    [HarmonyPatch(typeof(PressureDoorConfig), nameof(PressureDoorConfig.CreateBuildingDef))]
    public static class PressureDoorMod
    {
        public static void Postfix([NotNull] BuildingDef __result)
        {
            Debug.Log(" === PressureDoorMod INI === ");
            __result.RequiresPowerInput          = false;
            __result.EnergyConsumptionWhenActive = 0f;
            __result.MaterialCategory = MATERIALS.ANY_BUILDABLE;
            __result.ViewMode = SimViewMode.None;

            Debug.Log(" === PressureDoorMod END === ");
        }
    }
}