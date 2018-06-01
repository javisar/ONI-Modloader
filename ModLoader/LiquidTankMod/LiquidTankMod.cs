using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harmony;

namespace LiquidTankMod
{

	[HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
	internal class InverseElectrolyzerMod
	{
		private static void Prefix()
		{
			Debug.Log(" === GeneratedBuildings Prefix === " + LiquidTankConfig.ID);
			Strings.Add("STRINGS.BUILDINGS.PREFABS.LIQUIDTANK.NAME", "Liquid Tank");
			Strings.Add("STRINGS.BUILDINGS.PREFABS.LIQUIDTANK.DESC", "");
			Strings.Add("STRINGS.BUILDINGS.PREFABS.LIQUIDTANK.EFFECT", "");

			List<string> ls = new List<string>((string[])TUNING.BUILDINGS.PLANORDER[10].data);
			ls.Add(LiquidTankConfig.ID);
			TUNING.BUILDINGS.PLANORDER[10].data = (string[])ls.ToArray();

			TUNING.BUILDINGS.COMPONENT_DESCRIPTION_ORDER.Add(LiquidTankConfig.ID);


		}
		private static void Postfix()
		{

			Debug.Log(" === GeneratedBuildings Postfix === " + LiquidTankConfig.ID);
			object obj = Activator.CreateInstance(typeof(LiquidTankConfig));
			BuildingConfigManager.Instance.RegisterBuilding(obj as IBuildingConfig);
		}
	}

	[HarmonyPatch(typeof(Db), "Initialize")]
	internal class InverseElectrolyzerTechMod
	{
		private static void Prefix(Db __instance)
		{
			Debug.Log(" === Database.Techs loaded === " + LiquidTankConfig.ID);
			List<string> ls = new List<string>((string[])Database.Techs.TECH_GROUPING["SmartStorage"]);
			ls.Add(LiquidTankConfig.ID);
			Database.Techs.TECH_GROUPING["SmartStorage"] = (string[])ls.ToArray();

			//Database.Techs.TECH_GROUPING["TemperatureModulation"].Add("InsulatedPressureDoor");
		}
	}
	
}
