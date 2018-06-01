
using Klei;
using KSerialization;
using STRINGS;
using System.Collections.Generic;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
public class LiquidTank : StateMachineComponent<LiquidTank.StatesInstance>, IEffectDescriptor
{
	public class StatesInstance : GameStateMachine<States, StatesInstance, LiquidTank, object>.GameInstance
	{
		public StatesInstance(LiquidTank smi)
			: base(smi)
		{
		}
		/*
		public bool IsWorkable()
		{
			
			bool result = false;
			if (base.master.operational.IsOperational && this.EnvironmentNeedsCooling() && base.smi.master.HasMaterial() && base.smi.EnvironmentHighEnoughPressure())
			{
				result = true;
			}
			return result;
			
			return true;
		}
		*/
		/*
		public bool EnvironmentNeedsCooling()
		{
			bool result = false;
			int cell = Grid.PosToCell(base.transform.GetPosition());
			for (int i = base.master.minCoolingRange.y; i < base.master.maxCoolingRange.y; i++)
			{
				for (int j = base.master.minCoolingRange.x; j < base.master.maxCoolingRange.x; j++)
				{
					CellOffset offset = new CellOffset(j, i);
					int i2 = Grid.OffsetCell(cell, offset);
					if (Grid.Temperature[i2] > base.master.minCooledTemperature)
					{
						result = true;
						break;
					}
				}
			}
			return result;
		}

		public bool EnvironmentHighEnoughPressure()
		{
			int cell = Grid.PosToCell(base.transform.GetPosition());
			for (int i = base.master.minCoolingRange.y; i < base.master.maxCoolingRange.y; i++)
			{
				for (int j = base.master.minCoolingRange.x; j < base.master.maxCoolingRange.x; j++)
				{
					CellOffset offset = new CellOffset(j, i);
					int i2 = Grid.OffsetCell(cell, offset);
					if (Grid.Mass[i2] >= base.master.minEnvironmentMass)
					{
						return true;
					}
				}
			}
			return false;
		}
		*/
	}

	public class States : GameStateMachine<States, StatesInstance, LiquidTank>
	{
		public class Workable : State
		{
			public State waiting;

			public State consuming;

			public State emitting;
		}

		public Workable workable;

		public State unworkable;

		public State work_pst;

		public override void InitializeStates(out BaseState default_state)
		{
			default_state = this.unworkable;
			/*
			base.root.Enter(delegate (StatesInstance smi)
			{
				smi.master.workable.SetWorkTime(float.PositiveInfinity);
			});
			this.workable.ToggleChore(this.CreateUseChore, this.work_pst).EventTransition(GameHashes.ActiveChanged, this.workable.consuming, (StatesInstance smi) => (Object)smi.master.workable.worker != (Object)null).EventTransition(GameHashes.OperationalChanged, this.workable.consuming, (StatesInstance smi) => (Object)smi.master.workable.worker != (Object)null)
				.Transition(this.unworkable, (StatesInstance smi) => !smi.IsWorkable(), UpdateRate.SIM_200ms);
			this.work_pst.Update("LiquidFanEmitCooledContents", delegate (StatesInstance smi, float dt)
			{
				smi.master.EmitContents();
			}, UpdateRate.SIM_200ms, false).ScheduleGoTo(2f, this.unworkable);
			
			this.unworkable.Update("LiquidFanEmitCooledContents", delegate (StatesInstance smi, float dt)
			{
				smi.master.EmitContents();
			}, UpdateRate.SIM_200ms, false).Update("LiquidFanUnworkableStatusItems", delegate (StatesInstance smi, float dt)
			{
				smi.master.UpdateUnworkableStatusItems();
			}, UpdateRate.SIM_200ms, false).Transition(this.workable.waiting, (StatesInstance smi) => smi.IsWorkable(), UpdateRate.SIM_200ms)
				.Enter(delegate (StatesInstance smi)
				{
					smi.master.UpdateUnworkableStatusItems();
				})
				.Exit(delegate (StatesInstance smi)
				{
					smi.master.UpdateUnworkableStatusItems();
				});
			this.workable.consuming.EventTransition(GameHashes.OperationalChanged, this.unworkable, (StatesInstance smi) => (Object)smi.master.workable.worker == (Object)null).EventHandler(GameHashes.ActiveChanged, delegate (StatesInstance smi)
			{
				smi.master.CheckWorking();
			}).Enter(delegate (StatesInstance smi)
			{
				if (!smi.EnvironmentNeedsCooling() || !smi.master.HasMaterial() || !smi.EnvironmentHighEnoughPressure())
				{
					smi.GoTo(this.unworkable);
				}
				ElementConsumer component2 = ((Component)smi.master).GetComponent<ElementConsumer>();
				component2.consumptionRate = smi.master.flowRate;
				component2.RefreshConsumptionRate();
			})
				.Update(delegate (StatesInstance smi, float dt)
				{
					smi.master.CoolContents(dt);
				}, UpdateRate.SIM_200ms, false)
				.ScheduleGoTo(12f, this.workable.emitting)
				.Exit(delegate (StatesInstance smi)
				{
					ElementConsumer component = ((Component)smi.master).GetComponent<ElementConsumer>();
					component.consumptionRate = 0f;
					component.RefreshConsumptionRate();
				});
			this.workable.emitting.EventTransition(GameHashes.ActiveChanged, this.unworkable, (StatesInstance smi) => (Object)smi.master.workable.worker == (Object)null).EventTransition(GameHashes.OperationalChanged, this.unworkable, (StatesInstance smi) => (Object)smi.master.workable.worker == (Object)null).ScheduleGoTo(3f, this.workable.consuming)
				.Update("LiquidFanEmitCooledContents", delegate (StatesInstance smi, float dt)
				{
					smi.master.EmitContents();
				}, UpdateRate.SIM_200ms, false);
			this.workable.emitting.EventTransition(GameHashes.ActiveChanged, this.unworkable, (StatesInstance smi) => (Object)smi.master.workable.worker == (Object)null).EventTransition(GameHashes.OperationalChanged, this.unworkable, (StatesInstance smi) => (Object)smi.master.workable.worker == (Object)null).ScheduleGoTo(3f, this.workable.consuming)
				.Update(delegate (StatesInstance smi, float dt)
				{
					smi.master.CoolContents(dt);
				}, UpdateRate.SIM_200ms, false)
				.Update("LiquidFanEmitCooledContents", delegate (StatesInstance smi, float dt)
				{
					smi.master.EmitContents();
				}, UpdateRate.SIM_200ms, false);
				*/
		}
		/*
		private Chore CreateUseChore(StatesInstance smi)
		{
			return new WorkChore<LiquidCooledFanWorkable>(Db.Get().ChoreTypes.LiquidCooledFan, smi.master.workable, null, null, true, null, null, null, true, null, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 0, false);
		}
		*/
	}

	//[SerializeField]
	//public float coolingKilowatts;

	//[SerializeField]
	//public float minCooledTemperature;

	//[SerializeField]
	//public float minEnvironmentMass;

	//[SerializeField]
	//public float waterKGConsumedPerKJ;

	//[SerializeField]
	//public Vector2I minCoolingRange;

	//[SerializeField]
	//public Vector2I maxCoolingRange;

	//private float flowRate = 0.3f;

	//[SerializeField]
	//public Storage gasStorage;

	[SerializeField]
	public Storage liquidStorage;

	/*
	[MyCmpAdd]
	private LiquidCooledFanWorkable workable;
	*/

	[MyCmpGet]
	private Operational operational;

	//private HandleVector<int>.Handle waterConsumptionAccumulator = HandleVector<int>.InvalidHandle;

	private MeterController meter;

	public bool HasMaterial()
	{
		/*
		List<GameObject> list = base.smi.master.gasStorage.Find(GameTags.Water);
		if (list != null && list.Count > 0)
		{
			Debug.LogWarning("Liquid Cooled fan Gas storage contains water - A duplicant probably delivered to the wrong storage - moving it to liquid storage.", null);
			foreach (GameObject item in list)
			{
				base.smi.master.gasStorage.Transfer(item, base.smi.master.liquidStorage, false, false);
			}
		}
		*/
		this.UpdateMeter();
		return this.liquidStorage.MassStored() > 0f;
	}
	
	/*
	public void CheckWorking()
	{
		if ((Object)base.smi.master.workable.worker == (Object)null)
		{
			base.smi.GoTo(base.smi.sm.unworkable);
		}
	}
	*/
	/*
	private void UpdateUnworkableStatusItems()
	{
		KSelectable component = base.GetComponent<KSelectable>();
		if (!base.smi.EnvironmentNeedsCooling())
		{
			if (!component.HasStatusItem(Db.Get().BuildingStatusItems.CannotCoolFurther))
			{
				component.AddStatusItem(Db.Get().BuildingStatusItems.CannotCoolFurther, null);
			}
		}
		else if (component.HasStatusItem(Db.Get().BuildingStatusItems.CannotCoolFurther))
		{
			component.RemoveStatusItem(Db.Get().BuildingStatusItems.CannotCoolFurther, false);
		}
		if (!base.smi.EnvironmentHighEnoughPressure())
		{
			if (!component.HasStatusItem(Db.Get().BuildingStatusItems.UnderPressure))
			{
				component.AddStatusItem(Db.Get().BuildingStatusItems.UnderPressure, null);
			}
		}
		else if (component.HasStatusItem(Db.Get().BuildingStatusItems.UnderPressure))
		{
			component.RemoveStatusItem(Db.Get().BuildingStatusItems.UnderPressure, false);
		}
	}
	*/

	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.meter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Behind, "meter_target", "meter_waterbody", "meter_waterlevel");
		base.GetComponent<ElementConsumer>().EnableConsumption(true);
		//base.smi.StartSM();
		//base.smi.master.waterConsumptionAccumulator = Game.Instance.accumulators.Add("waterConsumptionAccumulator", this);
		//base.GetComponent<ElementConsumer>().storage = this.gasStorage;
		base.GetComponent<ElementConsumer>().storage = this.liquidStorage;
		//base.GetComponent<ManualDeliveryKG>().SetStorage(this.liquidStorage);
	}

	private void UpdateMeter()
	{
		this.meter.SetPositionPercent(Mathf.Clamp01(this.liquidStorage.MassStored() / this.liquidStorage.capacityKg));
	}
	/*
	private void EmitContents()
	{
		if (this.gasStorage.items.Count != 0)
		{
			float num = 0.1f;
			float num2 = num;
			PrimaryElement primaryElement = null;
			for (int i = 0; i < this.gasStorage.items.Count; i++)
			{
				PrimaryElement component = this.gasStorage.items[i].GetComponent<PrimaryElement>();
				if (component.Mass > num2 && component.Element.IsGas)
				{
					primaryElement = component;
					num2 = primaryElement.Mass;
				}
			}
			if ((Object)primaryElement != (Object)null)
			{
				SimMessages.AddRemoveSubstance(Grid.CellRight(Grid.CellAbove(Grid.PosToCell(base.gameObject))), ElementLoader.GetElementIndex(primaryElement.ElementID), CellEventLogger.Instance.ExhaustSimUpdate, primaryElement.Mass, primaryElement.Temperature, primaryElement.DiseaseIdx, primaryElement.DiseaseCount, -1);
				this.gasStorage.ConsumeIgnoringDisease(primaryElement.gameObject);
			}
		}
	}

	private void CoolContents(float dt)
	{
		if (this.gasStorage.items.Count != 0)
		{
			float num = float.PositiveInfinity;
			float num2 = 0f;
			float num3 = 0f;
			foreach (GameObject item in this.gasStorage)
			{
				PrimaryElement component = item.GetComponent<PrimaryElement>();
				if (!((Object)component == (Object)null) && !(component.Mass < 0.1f) && !(component.Temperature < this.minCooledTemperature))
				{
					num2 = GameUtil.GetThermalEnergy(component);
					if (num > num2)
					{
						num = num2;
					}
				}
			}
			foreach (GameObject item2 in this.gasStorage)
			{
				PrimaryElement component = item2.GetComponent<PrimaryElement>();
				if (!((Object)component == (Object)null) && !(component.Mass < 0.1f) && !(component.Temperature < this.minCooledTemperature))
				{
					float num4 = Mathf.Min(num, 10f);
					GameUtil.DeltaThermalEnergy(component, 0f - num4);
					num3 += num4;
				}
			}
			float num5 = Mathf.Abs(num3 * this.waterKGConsumedPerKJ);
			Game.Instance.accumulators.Accumulate(base.smi.master.waterConsumptionAccumulator, num5);
			if (num5 != 0f)
			{
				SimUtil.DiseaseInfo diseaseInfo = default(SimUtil.DiseaseInfo);
				float num6 = default(float);
				this.liquidStorage.ConsumeAndGetDisease(GameTags.Water, num5, out diseaseInfo, out num6);
				SimMessages.ModifyDiseaseOnCell(Grid.PosToCell(base.gameObject), diseaseInfo.idx, diseaseInfo.count);
				this.UpdateMeter();
			}
		}
	}
	*/
	public List<Descriptor> GetDescriptors(BuildingDef def)
	{
		List<Descriptor> list = new List<Descriptor>();
		/*
		Descriptor item = default(Descriptor);
		item.SetupDescriptor(string.Format(UI.BUILDINGEFFECTS.HEATCONSUMED, GameUtil.GetFormattedWattage(this.coolingKilowatts, GameUtil.WattageFormatterUnit.Automatic)), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.HEATCONSUMED, GameUtil.GetFormattedJoules(this.coolingKilowatts, "F1", GameUtil.TimeSlice.None)), Descriptor.DescriptorType.Effect);
		list.Add(item);
		*/
		return list;
	}
}
