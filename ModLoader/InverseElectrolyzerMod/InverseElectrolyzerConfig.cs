using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using STRINGS;
using TUNING;
using UnityEngine;


public class InverseElectrolyzerConfig : IBuildingConfig
{
	public const string ID = "InverseElectrolyzer";
	public static Tag gasTag = TagManager.Create("H&O2");
		
	private static readonly LogicPorts.Port[] INPUT_PORTS = new LogicPorts.Port[1]
	{
		LogicPorts.Port.InputPort(LogicOperationalController.PORT_ID, new CellOffset(1, 1), UI.LOGIC_PORTS.CONTROL_OPERATIONAL, false)			
	};

	public override BuildingDef CreateBuildingDef()
	{
		string id = "InverseElectrolyzer";
		int width = 2;
		int height = 2;
		string anim = "electrolyzer_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		float[] tIER = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
		string[] aLL_METALS = MATERIALS.ALL_METALS;
		float melting_point = 800f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tIER2 = NOISE_POLLUTION.NOISY.TIER3;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tIER, aLL_METALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.PENALTY.TIER1, tIER2, 0.2f);
		buildingDef.RequiresPowerInput = true;
		buildingDef.PowerInputOffset = new CellOffset(1, 0);
		buildingDef.EnergyConsumptionWhenActive = 120f;
		buildingDef.ExhaustKilowattsWhenActive = 0.25f;
		buildingDef.SelfHeatKilowattsWhenActive = 1f;
		buildingDef.ViewMode = SimViewMode.OxygenMap;
		buildingDef.MaterialCategory = MATERIALS.ALL_METALS;
		buildingDef.AudioCategory = "HollowMetal";
		buildingDef.InputConduitType = ConduitType.Gas;
		buildingDef.OutputConduitType = ConduitType.Liquid;
		buildingDef.UtilityInputOffset = new CellOffset(0, 0);
		buildingDef.UtilityOutputOffset = new CellOffset(1, 1);
		return buildingDef;
	}

	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddPrefabTag(RoomConstraints.ConstraintTags.IndustrialMachinery);
		InverseElectrolyzer electrolyzer = go.AddOrGet<InverseElectrolyzer>();
		electrolyzer.maxMass = 10f;
		electrolyzer.hasMeter = true;

		ConduitConsumer conduitConsumer1 = go.AddOrGet<ConduitConsumer>();
		conduitConsumer1.conduitType = ConduitType.Gas;
		conduitConsumer1.consumptionRate = 1f;
		conduitConsumer1.capacityTag = ElementLoader.FindElementByHash(SimHashes.Oxygen).tag;
		//conduitConsumer1.capacityTag = GameTags.Any;
		//conduitConsumer1.capacityTag = gasTag;
		conduitConsumer1.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;
		/*
		ConduitConsumer conduitConsumer2 = go.AddOrGet<ConduitConsumer>();
		conduitConsumer2.conduitType = ConduitType.Gas;
		conduitConsumer2.consumptionRate = 1f;
		conduitConsumer2.capacityTag = ElementLoader.FindElementByHash(SimHashes.Hydrogen).tag;
		conduitConsumer2.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;
		*/
		Storage storage = go.AddOrGet<Storage>();
		storage.capacityKg = 2f;
		storage.showInUI = true;

		ElementConverter elementConverter = go.AddOrGet<ElementConverter>();
		elementConverter.consumedElements = new ElementConverter.ConsumedElement[2]
		{
			//new ElementConverter.ConsumedElement(new Tag("Water"), 1f)
			new ElementConverter.ConsumedElement(new Tag("Oxygen"), 0.888f),
			new ElementConverter.ConsumedElement(new Tag("Hydrogen"), 0.111999989f)
		};
		elementConverter.outputElements = new ElementConverter.OutputElement[1]
		{
			//new ElementConverter.OutputElement(0.888f, SimHashes.Oxygen, 343.15f, false, 0f, 1f, false, 1f, 255, 0),
			//new ElementConverter.OutputElement(0.111999989f, SimHashes.Hydrogen, 343.15f, false, 0f, 1f, false, 1f, 255, 0)
			new ElementConverter.OutputElement(1f, SimHashes.Water, 343.15f, true, 0f, 0.5f, false, 0.75f, 255, 0)
		};

		
		ConduitDispenser conduitDispenser = go.AddOrGet<ConduitDispenser>();
		conduitDispenser.conduitType = ConduitType.Liquid;
		conduitDispenser.invertElementFilter = true;
		/*
		conduitDispenser.elementFilter = new SimHashes[1]
		{
			SimHashes.DirtyWater
		};
		*/

		Prioritizable.AddRef(go);
	}

	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
		GeneratedBuildings.RegisterLogicPorts(go, InverseElectrolyzerConfig.INPUT_PORTS);
	}

	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		GeneratedBuildings.RegisterLogicPorts(go, InverseElectrolyzerConfig.INPUT_PORTS);
	}

	public override void DoPostConfigureComplete(GameObject go)
	{
		BuildingTemplates.DoPostConfigure(go);
		GeneratedBuildings.RegisterLogicPorts(go, InverseElectrolyzerConfig.INPUT_PORTS);
		go.AddOrGet<LogicOperationalController>();
		go.GetComponent<KPrefabID>().prefabInitFn += delegate (GameObject game_object)
		{
			PoweredActiveController.Instance instance = new PoweredActiveController.Instance(game_object.GetComponent<KPrefabID>());
			instance.StartSM();
		};
	}
}

