using Harmony;
using STRINGS;
using System;
using System.Collections.Generic;


namespace InverseElectrolyzerMod
{    
    
    [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
    internal class InverseElectrolyzerMod
	{
        private static void Prefix()
        {
            Debug.Log(" === GeneratedBuildings Prefix === "+ InverseElectrolyzerConfig.ID);			
			Strings.Add("STRINGS.BUILDINGS.PREFABS.INVERSEELECTROLYZER.NAME", "Combustioneer");
            Strings.Add("STRINGS.BUILDINGS.PREFABS.INVERSEELECTROLYZER.DESC", "");
            Strings.Add("STRINGS.BUILDINGS.PREFABS.INVERSEELECTROLYZER.EFFECT", "");

            List<string> ls = new List<string>((string[])TUNING.BUILDINGS.PLANORDER[10].data);
            ls.Add(InverseElectrolyzerConfig.ID);            
            TUNING.BUILDINGS.PLANORDER[10].data = (string[]) ls.ToArray();

            TUNING.BUILDINGS.COMPONENT_DESCRIPTION_ORDER.Add(InverseElectrolyzerConfig.ID);


        }
        private static void Postfix()
        {
            
            Debug.Log(" === GeneratedBuildings Postfix === " + InverseElectrolyzerConfig.ID);
            object obj = Activator.CreateInstance(typeof(InverseElectrolyzerConfig));
            BuildingConfigManager.Instance.RegisterBuilding(obj as IBuildingConfig);
        }
    }

	[HarmonyPatch(typeof(Db), "Initialize")]
	internal class InverseElectrolyzerTechMod
	{
		private static void Prefix(Db __instance)
		{
			Debug.Log(" === Database.Techs loaded === " + InverseElectrolyzerConfig.ID);
			List<string> ls = new List<string>((string[])Database.Techs.TECH_GROUPING["Combustion"]);
			ls.Add(InverseElectrolyzerConfig.ID);
			Database.Techs.TECH_GROUPING["Combustion"] = (string[])ls.ToArray();

			//Database.Techs.TECH_GROUPING["TemperatureModulation"].Add("InsulatedPressureDoor");
		}
	}
	/*
	[HarmonyPatch(typeof(ConduitConsumer), "Consume", new Type[] { typeof(float), typeof(ConduitFlow)})]
	internal class ConduitConsumerMod
	{
		private static void Prefix(ConduitConsumer __instance, float dt, ConduitFlow conduit_mgr)
		{
			
			if (__instance.IsConnected && __instance.capacityTag == InverseElectrolyzerConfig.gasTag)
			{
				ConduitFlow.ConduitContents contents = conduit_mgr.GetContents((int)AccessTools.Field(typeof(ConduitConsumer), "utilityCell").GetValue(__instance));
				Element element = ElementLoader.FindElementByHash(contents.element);
				if (element.tag == ElementLoader.FindElementByHash(SimHashes.Oxygen).tag)
				{
					__instance.capacityTag = ElementLoader.FindElementByHash(SimHashes.Oxygen).tag;
				}
				else if (element.tag == ElementLoader.FindElementByHash(SimHashes.Hydrogen).tag)
				{
					__instance.capacityTag = ElementLoader.FindElementByHash(SimHashes.Hydrogen).tag;
				}
				else
				{
					__instance.capacityTag = ElementLoader.FindElementByHash(SimHashes.Oxygen).tag;
				}
			}
		}

		private static void Postfix(ConduitConsumer __instance)
		{
			__instance.capacityTag = InverseElectrolyzerConfig.gasTag;
		}
	}
	*/
	
}
