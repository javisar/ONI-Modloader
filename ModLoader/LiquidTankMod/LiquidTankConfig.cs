
using TUNING;
using UnityEngine;

public class LiquidTankConfig : IBuildingConfig
{
	public const string ID = "LiquidTank";

	public override BuildingDef CreateBuildingDef()
	{
		
		int width = 2;
		int height = 2;
		string anim = "fanliquid_kanim";
		int hitpoints = 30;
		float construction_time = 10f;
		float[] tIER = BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
		string[] aLL_METALS = MATERIALS.ALL_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tIER2 = NOISE_POLLUTION.NOISY.TIER2;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(ID, width, height, anim, hitpoints, construction_time, tIER, aLL_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, tIER2, 0.2f);
		buildingDef.EnergyConsumptionWhenActive = 120f;
		buildingDef.ExhaustKilowattsWhenActive = 0f;
		buildingDef.SelfHeatKilowattsWhenActive = 0f;
		buildingDef.Overheatable = false;
		buildingDef.ViewMode = SimViewMode.LiquidVentMap;
		buildingDef.MaterialCategory = MATERIALS.ALL_METALS;
		buildingDef.AudioCategory = "HollowMetal";
		buildingDef.PowerInputOffset = new CellOffset(0, 0);
		buildingDef.UtilityInputOffset = new CellOffset(0, 0);
		buildingDef.UtilityOutputOffset = new CellOffset(1, 0);
		buildingDef.InputConduitType = ConduitType.Liquid;
		buildingDef.OutputConduitType = ConduitType.Liquid;
		buildingDef.RequiresPowerInput = true;
		return buildingDef;
	}

	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		Storage storage = go.AddComponent<Storage>();
		Storage storage2 = go.AddComponent<Storage>();
		storage2.capacityKg = 20000f;
		go.AddOrGet<BuildingComplete>().isManuallyOperated = false;
		go.AddOrGet<LoopingSounds>();
		Prioritizable.AddRef(go);
		//float num = 2426.72f;
		//float num2 = 0.01f;

		LiquidTank liquidCooledFan = go.AddOrGet<LiquidTank>();
		//liquidCooledFan.gasStorage = storage;
		liquidCooledFan.liquidStorage = storage2;
		//liquidCooledFan.waterKGConsumedPerKJ = 1f / (num * num2);
		//liquidCooledFan.coolingKilowatts = 80f;
		//liquidCooledFan.minCooledTemperature = 290f;
		//liquidCooledFan.minEnvironmentMass = 0.25f;
		//liquidCooledFan.minCoolingRange = new Vector2I(-2, 0);
		//liquidCooledFan.maxCoolingRange = new Vector2I(2, 4);
		/*
		ManualDeliveryKG manualDeliveryKG = go.AddComponent<ManualDeliveryKG>();
		manualDeliveryKG.requestedItemTag = new Tag("Water");
		manualDeliveryKG.capacity = 500f;
		manualDeliveryKG.refillMass = 50f;
		manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.Fetch.IdHash;
		
		ElementConsumer elementConsumer = go.AddOrGet<ElementConsumer>();
		elementConsumer.storeOnConsume = true;
		elementConsumer.storage = storage;
		elementConsumer.configuration = ElementConsumer.Configuration.AllGas;
		elementConsumer.consumptionRadius = 8;
		elementConsumer.EnableConsumption(true);
		elementConsumer.sampleCellOffset = new Vector3(0f, 0f);
		elementConsumer.showDescriptor = false;
		*/
		ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
		conduitConsumer.conduitType = ConduitType.Liquid;
		conduitConsumer.consumptionRate = 10f;
		conduitConsumer.capacityKG = 20000f;
		conduitConsumer.capacityTag = GameTags.Liquid;
		conduitConsumer.forceAlwaysSatisfied = true;
		conduitConsumer.alwaysConsume = true;
		conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;

		ConduitDispenser conduitDispenser = go.AddOrGet<ConduitDispenser>();
		conduitDispenser.conduitType = ConduitType.Liquid;
		/*
		LiquidCooledFanWorkable liquidCooledFanWorkable = go.AddOrGet<LiquidCooledFanWorkable>();
		liquidCooledFanWorkable.SetWorkTime(20f);
		liquidCooledFanWorkable.overrideAnims = new KAnimFile[1]
		{
			Assets.GetAnim("anim_interacts_liquidfan_kanim")
		};
		*/
	}

	public override void DoPostConfigureComplete(GameObject go)
	{
		BuildingTemplates.DoPostConfigure(go);
		go.GetComponent<KPrefabID>().prefabInitFn += delegate (GameObject game_object)
		{
			PoweredActiveController.Instance instance = new PoweredActiveController.Instance(game_object.GetComponent<KPrefabID>());
			instance.StartSM();
		};
	}
}
