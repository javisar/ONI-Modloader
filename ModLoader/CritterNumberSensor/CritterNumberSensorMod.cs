using Harmony;
using STRINGS;
using System;
using System.Collections.Generic;


namespace CritterNumberSensorMod
{    
    
    [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
    internal class CritterNumberSensorMod
	{
        private static void Prefix()
        {
            Debug.Log(" === GeneratedBuildings Prefix === "+ CritterNumberSensorConfig.ID);			
			Strings.Add("STRINGS.BUILDINGS.PREFABS.CRITTERNUMBERSENSOR.NAME", "Critter Number Sensor");
            Strings.Add("STRINGS.BUILDINGS.PREFABS.CRITTERNUMBERSENSOR.DESC", "Critter Number Sensor can detect how many critters in the room.");
            Strings.Add("STRINGS.BUILDINGS.PREFABS.CRITTERNUMBERSENSOR.EFFECT", "Becomes " + UI.FormatAsLink("Active", "LOGIC") + " or on " + UI.FormatAsLink("Standby", "LOGIC") + " when room " + UI.FormatAsLink("Critter Number", "Critters") + " enters the chosen range.");

            List<string> ls = new List<string>((string[])TUNING.BUILDINGS.PLANORDER[11].data);
            ls.Add("CritterNumberSensor");            
            TUNING.BUILDINGS.PLANORDER[11].data = (string[]) ls.ToArray();

            TUNING.BUILDINGS.COMPONENT_DESCRIPTION_ORDER.Add("CritterNumberSensor");



        }
        private static void Postfix()
        {
            
            Debug.Log(" === GeneratedBuildings Postfix === ");
            object obj = Activator.CreateInstance(typeof(CritterNumberSensorConfig));
            BuildingConfigManager.Instance.RegisterBuilding(obj as IBuildingConfig);
        }
    }
	
    
	
}
