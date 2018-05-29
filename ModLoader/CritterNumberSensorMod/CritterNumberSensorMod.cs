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
            ls.Add(CritterNumberSensorConfig.ID);            
            TUNING.BUILDINGS.PLANORDER[11].data = (string[]) ls.ToArray();

            TUNING.BUILDINGS.COMPONENT_DESCRIPTION_ORDER.Add(CritterNumberSensorConfig.ID);


        }
        private static void Postfix()
        {
            
            Debug.Log(" === GeneratedBuildings Postfix === " + CritterNumberSensorConfig.ID);
            object obj = Activator.CreateInstance(typeof(CritterNumberSensorConfig));
            BuildingConfigManager.Instance.RegisterBuilding(obj as IBuildingConfig);
        }
    }

	[HarmonyPatch(typeof(Db), "Initialize")]
	internal class CritterNumberSensorTechMod
	{
		private static void Prefix(Db __instance)
		{
			Debug.Log(" === Database.Techs loaded === " + CritterNumberSensorConfig.ID);
			List<string> ls = new List<string>((string[])Database.Techs.TECH_GROUPING["AnimalControl"]);
			ls.Add(CritterNumberSensorConfig.ID);
			Database.Techs.TECH_GROUPING["AnimalControl"] = (string[])ls.ToArray();

			//Database.Techs.TECH_GROUPING["TemperatureModulation"].Add("InsulatedPressureDoor");
		}
	}
			/*
			[HarmonyPatch(typeof(Database.Techs))]
			internal class CritterNumberSensorTechMod
			{
				private static void Postfix()
				{
					Debug.Log(" === Database.Techs CritterNumberSensor loaded === ");
					List<string> ls = new List<string>((string[])Database.Techs.TECH_GROUPING["AnimalControl"]);
					ls.Add("CritterNumberSensor");
					Database.Techs.TECH_GROUPING["AnimalControl"] = (string[])ls.ToArray();

					//Database.Techs.TECH_GROUPING["TemperatureModulation"].Add("InsulatedPressureDoor");
				}
			}
			*/
			/*
			[HarmonyPatch(typeof(Db), "Initialize")]
			internal class CritterNumberSensorTechMod
			{
				private static void Prefix(Db __instance)
				{
					Debug.Log(" === GeneratedBuildings Prefix === " + CritterNumberSensorConfig.ID);
					Strings.Add("STRINGS.BUILDINGS.PREFABS.CRITTERNUMBERSENSOR.NAME", "Critter Number Sensor");
					Strings.Add("STRINGS.BUILDINGS.PREFABS.CRITTERNUMBERSENSOR.DESC", "Critter Number Sensor can detect how many critters in the room.");
					Strings.Add("STRINGS.BUILDINGS.PREFABS.CRITTERNUMBERSENSOR.EFFECT", "Becomes " + UI.FormatAsLink("Active", "LOGIC") + " or on " + UI.FormatAsLink("Standby", "LOGIC") + " when room " + UI.FormatAsLink("Critter Number", "Critters") + " enters the chosen range.");

					List<string> ls = new List<string>((string[])TUNING.BUILDINGS.PLANORDER[11].data);
					ls.Add("CritterNumberSensor");
					TUNING.BUILDINGS.PLANORDER[11].data = (string[])ls.ToArray();

					TUNING.BUILDINGS.COMPONENT_DESCRIPTION_ORDER.Add("CritterNumberSensor");


				}
				private static void Postfix(Db __instance)
				{
					Tech tech = new Tech("", __instance, StringEntry.op_Implicit(Strings.Get("STRINGS.RESEARCH.TECHS." + item.Id.ToUpper() + ".NAME")), StringEntry.op_Implicit(Strings.Get("STRINGS.RESEARCH.TECHS." + item.Id.ToUpper() + ".DESC")), item);
					__instance.Techs.
					Debug.Log(" === GeneratedBuildings Postfix === ");
					object obj = Activator.CreateInstance(typeof(CritterNumberSensorConfig));
					BuildingConfigManager.Instance.RegisterBuilding(obj as IBuildingConfig);
				}
			}
			*/

		}
