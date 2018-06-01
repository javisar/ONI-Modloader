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

	private const float CO2_CONSUMPTION_RATE = 0.3f;

	private const float H2O_CONSUMPTION_RATE = 1f;

	private static readonly LogicPorts.Port[] INPUT_PORTS = new LogicPorts.Port[1]
	{
		LogicPorts.Port.InputPort(LogicOperationalController.PORT_ID, new CellOffset(-1, 0), UI.LOGIC_PORTS.CONTROL_OPERATIONAL, false)
	};

	public override BuildingDef CreateBuildingDef()
	{
		string id = "InverseElectrolyzer";
		int width = 4;
		int height = 3;
		string anim = "waterpurifier_kanim";
		int hitpoints = 100;
		float construction_time = 30f;
		float[] tIER = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
		string[] aLL_METALS = MATERIALS.ALL_METALS;
		float melting_point = 800f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tIER2 = NOISE_POLLUTION.NOISY.TIER3;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tIER, aLL_METALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.PENALTY.TIER2, tIER2, 0.2f);
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 120f;
		buildingDef.ExhaustKilowattsWhenActive = 0f;
		buildingDef.SelfHeatKilowattsWhenActive = 4f;
		buildingDef.InputConduitType = ConduitType.Gas;
		buildingDef.OutputConduitType = ConduitType.Gas;
		buildingDef.ViewMode = SimViewMode.GasVentMap;
		buildingDef.MaterialCategory = MATERIALS.ALL_METALS;
		buildingDef.AudioCategory = "HollowMetal";
		buildingDef.PowerInputOffset = new CellOffset(2, 0);
		buildingDef.UtilityInputOffset = new CellOffset(-1, 2);
		buildingDef.UtilityOutputOffset = new CellOffset(2, 2);
		buildingDef.PermittedRotations = PermittedRotations.FlipH;
		return buildingDef;
	}

	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<LoopingSounds>();
		//go.AddOrGet<InverseElectrolyzer>();
		go.GetComponent<KPrefabID>().AddPrefabTag(RoomConstraints.ConstraintTags.IndustrialMachinery);
		//go.AddOrGet<Pump>();
		//Storage storage = BuildingTemplates.CreateDefaultStorage(go, false);
		//storage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
		/*
		Storage storage = go.AddOrGet<Storage>();
		storage.capacityKg = 50f;
		storage.showInUI = true;
		*/
		Storage storage = BuildingTemplates.CreateDefaultStorage(go, false);
		storage.showInUI = true;
		storage.capacityKg = 30f;
		storage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);

		InverseElectrolyzer airFilter = go.AddOrGet<InverseElectrolyzer>();
		airFilter.filterTag = GameTagExtensions.Create(SimHashes.Hydrogen);


		//Prioritizable.AddRef(go);

		/*
		elementConverter.consumedElements = new ElementConverter.ConsumedElement[2]
		{
			new ElementConverter.ConsumedElement(new Tag("Filter"), 1f),
			new ElementConverter.ConsumedElement(new Tag("DirtyWater"), 5f)
		};
		elementConverter.outputElements = new ElementConverter.OutputElement[2]
		{
			new ElementConverter.OutputElement(5f, SimHashes.Water, 313.15f, true, 0f, 0.5f, false, 0.75f, 255, 0),
			new ElementConverter.OutputElement(0.2f, SimHashes.ToxicSand, 313.15f, true, 0f, 0.5f, false, 0.25f, 255, 0)
		};
		*/

		/*
		ElementConsumer elementConsumer = go.AddOrGet<ElementConsumer>();
		elementConsumer.configuration = ElementConsumer.Configuration.AllGas;
		elementConsumer.consumptionRate = 1f;
		elementConsumer.storeOnConsume = true;
		elementConsumer.showInStatusPanel = false;
		elementConsumer.consumptionRadius = 2;
		*/

		ElementConsumer elementConsumer = go.AddOrGet<PassiveElementConsumer>();
		elementConsumer.elementToConsume = SimHashes.Oxygen;
		elementConsumer.consumptionRate = 1f;
		elementConsumer.capacityKG = 1f;
		elementConsumer.consumptionRadius = 3;
		elementConsumer.showInStatusPanel = true;
		elementConsumer.sampleCellOffset = new Vector3(0f, 0f, 0f);
		elementConsumer.isRequired = false;
		elementConsumer.storeOnConsume = true;
		elementConsumer.showDescriptor = false;

		ElementConverter elementConverter = go.AddOrGet<ElementConverter>();
		elementConverter.consumedElements = new ElementConverter.ConsumedElement[2]
		{
			//new ElementConverter.ConsumedElement(new Tag("Oxygen"), 5*0.888f),
			//new ElementConverter.ConsumedElement(new Tag("Hydrogen"), 5*0.111999989f)
			new ElementConverter.ConsumedElement(GameTagExtensions.Create(SimHashes.Oxygen), 0.888f),
			new ElementConverter.ConsumedElement(GameTagExtensions.Create(SimHashes.Hydrogen), 0.111999989f)
		};		
		elementConverter.outputElements = new ElementConverter.OutputElement[1]
		{
			new ElementConverter.OutputElement(1f, SimHashes.Steam, 423.15f, true, 0f, 0.5f, false, 0.75f, 255, 0)
		};
		/*
		ElementDropper elementDropper = go.AddComponent<ElementDropper>();
		elementDropper.emitMass = 10f;
		elementDropper.emitTag = new Tag("ToxicSand");
		elementDropper.emitOffset = new Vector3(0f, 1f, 0f);

		ManualDeliveryKG manualDeliveryKG = go.AddComponent<ManualDeliveryKG>();
		manualDeliveryKG.SetStorage(storage);
		manualDeliveryKG.requestedItemTag = new Tag("Filter");
		manualDeliveryKG.capacity = 1200f;
		manualDeliveryKG.refillMass = 300f;
		manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.OperateFetch.IdHash;
		*/
		ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
		conduitConsumer.conduitType = ConduitType.Gas;
		conduitConsumer.consumptionRate = 2f;
		conduitConsumer.capacityKG = 5*0.111999989f;
		conduitConsumer.capacityTag = ElementLoader.FindElementByHash(SimHashes.Hydrogen).tag; //GameTagExtensions.Create(SimHashes.Hydrogen);//GameTags.Oxygen;
		conduitConsumer.forceAlwaysSatisfied = true;
		conduitConsumer.alwaysConsume = true;

		//conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Store;
		conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;
		/*
		ConduitConsumer conduitConsumer2 = go.AddOrGet<ConduitConsumer>();
		conduitConsumer2.conduitType = ConduitType.Gas;
		//conduitConsumer.consumptionRate = 1f;
		conduitConsumer2.capacityKG = 5*0.888f;
		conduitConsumer2.capacityTag = ElementLoader.FindElementByHash(SimHashes.Oxygen).tag; //GameTagExtensions.Create(SimHashes.Oxygen);//GameTags.Oxygen;
		conduitConsumer2.forceAlwaysSatisfied = true;
		//conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Store;
		conduitConsumer2.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;
		*/
		ConduitDispenser conduitDispenser = go.AddOrGet<ConduitDispenser>();
		conduitDispenser.conduitType = ConduitType.Gas;
		//conduitDispenser.invertElementFilter = true;
		
		conduitDispenser.elementFilter = new SimHashes[1]
		{
			SimHashes.Steam
		};
		
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

