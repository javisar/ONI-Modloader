using Harmony;
using System;
using System.Collections.Generic;


namespace InsulatedDoorsMod
{    
    
    [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
    internal class InsulatedPressureDoorMod
    {
        private static void Prefix()
        {
            Debug.Log(" === GeneratedBuildings Prefix === "+ InsulatedPressureDoorConfig.ID);
            Strings.Add("STRINGS.BUILDINGS.PREFABS.INSULATEDPRESSUREDOOR.NAME", "Insulated Mechanized Airlock");
            Strings.Add("STRINGS.BUILDINGS.PREFABS.INSULATEDPRESSUREDOOR.DESC", "Insulated Mechanized airlocks have the same function as other doors, but open and close more quickly.");
            Strings.Add("STRINGS.BUILDINGS.PREFABS.INSULATEDPRESSUREDOOR.EFFECT", "Blocks <style=\"liquid\">Liquid</style> and <style=\"gas\">Gas</style> flow, maintaining pressure between areas.\n\nSets Duplicant Access Permissions for area restriction.\n\nFunctions as a Manual Airlock when no <style=\"power\">Power</style> is available.");

            List<string> ls = new List<string>((string[])TUNING.BUILDINGS.PLANORDER[0].data);
            ls.Add("InsulatedPressureDoor");            
            TUNING.BUILDINGS.PLANORDER[0].data = (string[]) ls.ToArray();

            TUNING.BUILDINGS.COMPONENT_DESCRIPTION_ORDER.Add("InsulatedPressureDoor");

        }
        private static void Postfix()
        {
            
            Debug.Log(" === GeneratedBuildings Postfix === " + InsulatedPressureDoorConfig.ID);
            object obj = Activator.CreateInstance(typeof(InsulatedPressureDoorConfig));
            BuildingConfigManager.Instance.RegisterBuilding(obj as IBuildingConfig);
        }
    }

	[HarmonyPatch(typeof(Db), "Initialize")]
	internal class InsulatedPressureDoorTechMod
	{
		private static void Prefix(Db __instance)
		{
			Debug.Log(" === Database.Techs loaded === " + InsulatedPressureDoorConfig.ID);
			List<string> ls = new List<string>((string[])Database.Techs.TECH_GROUPING["TemperatureModulation"]);
			ls.Add("InsulatedPressureDoor");
			Database.Techs.TECH_GROUPING["TemperatureModulation"] = (string[])ls.ToArray();

			//Database.Techs.TECH_GROUPING["TemperatureModulation"].Add("InsulatedPressureDoor");
		}
	}

}
