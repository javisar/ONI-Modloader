using Harmony;
using System;
using System.Collections.Generic;


namespace InsulatedDoorsMod
{    
    
    [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
    internal static class InsulatedPressureDoorMod
    {
        private static void Prefix()
        {
            Debug.Log(" === GeneratedBuildings Prefix === "+ InsulatedPressureDoorConfig.ID);
            Strings.Add("STRINGS.BUILDINGS.PREFABS.INSULATEDPRESSUREDOOR.NAME", "Insulated Mechanized Airlock");
            Strings.Add("STRINGS.BUILDINGS.PREFABS.INSULATEDPRESSUREDOOR.DESC", "Insulated Mechanized airlocks have the same function as other doors, but open and close more quickly.");
            Strings.Add("STRINGS.BUILDINGS.PREFABS.INSULATEDPRESSUREDOOR.EFFECT", "Blocks <style=\"liquid\">Liquid</style> and <style=\"gas\">Gas</style> flow, maintaining pressure between areas.\n\nSets Duplicant Access Permissions for area restriction.\n\nFunctions as a Manual Airlock when no <style=\"power\">Power</style> is available.");

	        List<string> ls = new List<string>((string[]) TUNING.BUILDINGS.PLANORDER[0].data) {"InsulatedPressureDoor"};

	        TUNING.BUILDINGS.PLANORDER[0].data = (string[]) ls.ToArray();

            TUNING.BUILDINGS.COMPONENT_DESCRIPTION_ORDER.Add("InsulatedPressureDoor");

        }
        private static void Postfix()
        {
            
            Debug.Log(" === GeneratedBuildings Postfix === ");
            object obj = Activator.CreateInstance(typeof(InsulatedPressureDoorConfig));
            BuildingConfigManager.Instance.RegisterBuilding(obj as IBuildingConfig);
        }
    }
	

	[HarmonyPatch(typeof(DoorConfig), "CreateBuildingDef")]
    internal static class InsulatedDoorsMod
    {

        private static void Postfix(DoorConfig __instance, ref BuildingDef __result)
        {          
            __result.MaterialCategory = TUNING.MATERIALS.ANY_BUILDABLE;
        }
    }


    [HarmonyPatch(typeof(ManualPressureDoorConfig), "CreateBuildingDef")]
    internal static class ManualPressureDoorConfigMod
    {

        private static void Postfix(ManualPressureDoorConfig __instance, ref BuildingDef __result)
        {
            __result.MaterialCategory = TUNING.MATERIALS.ANY_BUILDABLE;
        }
    }


    [HarmonyPatch(typeof(PressureDoorConfig), "CreateBuildingDef")]
    internal static class PressureDoorConfigMod
    {

        private static void Postfix(PressureDoorConfig __instance, ref BuildingDef __result)
        {
            __result.MaterialCategory = TUNING.MATERIALS.ANY_BUILDABLE;
        }
    }

	/*
    [HarmonyPatch(typeof(Database.Techs))]
    internal class TestNewElement
    {
        private static void Postfix()
        {
            Debug.Log(" === Database.Techs loaded === ");
            List<string> ls = new List<string>((string[])Database.Techs.TECH_GROUPING["TemperatureModulation"]);
            ls.Add("InsulatedPressureDoor");
            Database.Techs.TECH_GROUPING["TemperatureModulation"] = (string[])ls.ToArray();

            //Database.Techs.TECH_GROUPING["TemperatureModulation"].Add("InsulatedPressureDoor");
        }
    }
	*/
	/*
    [HarmonyPatch(typeof(ProductInfoScreen), "SetDescription", new Type[] { typeof(BuildingDef) })]
    internal class TestNewElement4
    {
        private static bool Prefix(ProductInfoScreen __instance, ref BuildingDef def)
        {
            Debug.Log(" === ProductInfoScreen.Desc loaded === "+ def.PrefabID.ToUpper());
            
            //Database.Techs.TECH_GROUPING["TemperatureModulation"].Add("InsulatedPressureDoor");
            if (def.PrefabID.ToUpper().Equals("INSULATEDPRESSUREDOOR"))
            {
                Strings.Add("STRINGS.BUILDINGS.PREFABS.INSULATEDPRESSUREDOOR.DESC", "Insulated Mechanized airlocks have the same function as other doors, but open and close more quickly.");

                //Traverse.Create(def).Property("Desc").SetValue("Insulated Mechanized airlocks have the same function as other doors, but open and close more quickly.");
                Debug.Log(" === ProductInfoScreen.Desc loaded === " + def.Desc);
                
            }
          
            return true;
        }
    }
    */
	/*
    [HarmonyPatch(typeof(BuildingDef))]
    [HarmonyPatch("Desc", PropertyMethod.Getter)]
    internal class TestNewElement4
    {
        private static bool Prefix(BuildingDef __instance, ref string __result)
        {
            Debug.Log(" === BuildingDef.Desc loaded === ");
            //Database.Techs.TECH_GROUPING["TemperatureModulation"].Add("InsulatedPressureDoor");
            if (__instance.PrefabID.ToUpper().Equals("INSULATEDPRESSUREDOOR"))
            {
                __result = "Insulated Mechanized airlocks have the same function as other doors, but open and close more quickly.";
                return false;
            }
            return true;
        }
    }
    */

	/*
    [HarmonyPatch(typeof(PlanScreen),"Refresh")]
    internal class TestNewElement2
    {
        private static void Prefix()
        {
            Debug.Log(" === TUNING.BUILDINGS loaded === ");

        }
    }
    */
	/*
    [HarmonyPatch(typeof(TUNING.BUILDINGS))]
    internal class TestNewElement2
    {
        private static void Postfix()
        {
            Debug.Log(" === TUNING.BUILDINGS loaded === ");
            ((string[])TUNING.BUILDINGS.PLANORDER[0].data).Add("InsulatedPressureDoor");
            TUNING.BUILDINGS.COMPONENT_DESCRIPTION_ORDER.Add("InsulatedPressureDoor");
        }
    }
    */


	/*
    [HarmonyPatch(typeof(ManualPressureDoorConfig), "CreateBuildingDef" )]
    internal class InsulatedDoorsMod
    {

        private static void Postfix(ManualPressureDoorConfig __instance, ref BuildingDef __result)
        {
            Debug.Log(" === InsulatedDoorsMod INI === ");
            __result.Insulation = 0.01f;
            Debug.Log(" === InsulatedDoorsMod END === ");
        }
    }
    */

	/*
	[HarmonyPatch(typeof(StructureTemperatureComponents), "Disable", new Type[] { typeof(HandleVector<int>.Handle) })]
	internal class TestNewElement5
	{
		private static bool Prefix(StructureTemperatureComponents __instance, HandleVector<int>.Handle handle)
		{
			Debug.Log(" === StructureTemperatureComponents Disable Prefix === " + handle.index);
			if (handle.index < 0)
			{
				return false;
			}
			//Traverse.Create(handle).Property("handles").
			//var foo = Traverse.Create(__instance).Property("base").GetValue<KCompactedVector<T>>();
			//HandleVector<int> handles = Traverse.Create(foo).Property("handles").GetValue<HandleVector<int>>();
			//HandleVector<int> handles = Traverse.Create(__instance).Property("base").Property("handles").GetValue<HandleVector<int>>();
			//Debug.Log(" === StructureTemperatureComponents Prefix === "+ handles.Items.Count);

			return true;
		}

	}

	[HarmonyPatch(typeof(StructureTemperatureComponents), "Enable", new Type[] { typeof(HandleVector<int>.Handle) })]
	internal class TestNewElement6
	{
		private static bool Prefix(StructureTemperatureComponents __instance, HandleVector<int>.Handle handle)
		{
			Debug.Log(" === StructureTemperatureComponents Enable Prefix === " + handle.index);
			if (handle.index < 0)
			{
				return false;
			}
			//Traverse.Create(handle).Property("handles").
			//var foo = Traverse.Create(__instance).Property("base").GetValue<KCompactedVector<T>>();
			//HandleVector<int> handles = Traverse.Create(foo).Property("handles").GetValue<HandleVector<int>>();
			//HandleVector<int> handles = Traverse.Create(__instance).Property("base").Property("handles").GetValue<HandleVector<int>>();
			//Debug.Log(" === StructureTemperatureComponents Prefix === "+ handles.Items.Count);

			return true;
		}

	}
	*/
}
