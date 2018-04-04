using Harmony;
using System.Collections.Generic;

namespace DefaultDeconstruct
{
    [HarmonyPatch(typeof(DeconstructTool), "GetDefaultFilters")] //Method inherited from FilteredDragTool
    internal static class DefaultDeconstruct
    {
        private static void Postfix(DeconstructTool __instance, ref Dictionary<string, ToolParameterMenu.ToggleState> filters)
        {
            //Debug.Log("=== DefaultDeconstruct INI ===");
            //filters.Add(ToolParameterMenu.FILTERLAYERS.ALL, ToolParameterMenu.ToggleState.On);
            //filters.Add(ToolParameterMenu.FILTERLAYERS.BUILDINGS, ToolParameterMenu.ToggleState.Off);

            //DeconstructTool (DeconstructTool) - TODO Need to get method execution only when DeconstructTool calls it
            if (__instance.ToString() == "DeconstructTool (DeconstructTool)")
            {
                if (filters.ContainsKey(ToolParameterMenu.FILTERLAYERS.BUILDINGS) && filters.ContainsKey(ToolParameterMenu.FILTERLAYERS.ALL))
                {
                    filters[ToolParameterMenu.FILTERLAYERS.ALL] = ToolParameterMenu.ToggleState.Off;
                    filters[ToolParameterMenu.FILTERLAYERS.BUILDINGS] = ToolParameterMenu.ToggleState.On;
                    Debug.Log("=== DEFAULT DECONSTRUCT Tool set to BUILDINGS ===");
                }
                else
                {
                    Debug.Log("=== DEFAULT DECONSTRUCT - Could not find Keys");
                };
            };
            //Debug.Log("=== DefaultDeconstruct CHANGED ===");
        }
    }
}