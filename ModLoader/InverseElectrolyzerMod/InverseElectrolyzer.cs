using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSerialization;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
public class InverseElectrolyzer : StateMachineComponent<InverseElectrolyzer.StatesInstance>
{
	public class StatesInstance : GameStateMachine<States, StatesInstance, InverseElectrolyzer, object>.GameInstance
	{
		public StatesInstance(InverseElectrolyzer smi)
			: base(smi)
		{
		}
	}

	public class States : GameStateMachine<States, StatesInstance, InverseElectrolyzer>
	{
		public State disabled;

		public State waiting;

		public State converting;

		public State overpressure;

		public override void InitializeStates(out BaseState default_state)
		{
			default_state = this.disabled;
			base.root.EventTransition(GameHashes.OperationalChanged, this.disabled, (StatesInstance smi) => !smi.master.operational.IsOperational).EventHandler(GameHashes.OnStorageChange, delegate (StatesInstance smi)
			{
				smi.master.UpdateMeter();
			});
			this.disabled.EventTransition(GameHashes.OperationalChanged, this.waiting, (StatesInstance smi) => smi.master.operational.IsOperational);
			this.waiting.Enter("Waiting", delegate (StatesInstance smi)
			{
				smi.master.operational.SetActive(false, false);
			}).EventTransition(GameHashes.OnStorageChange, this.converting, (StatesInstance smi) => ((Component)smi.master).GetComponent<ElementConverter>().HasEnoughMassToStartConverting());
			this.converting.Enter("Ready", delegate (StatesInstance smi)
			{
				smi.master.operational.SetActive(true, false);
			}).Transition(this.waiting, (StatesInstance smi) => !((Component)smi.master).GetComponent<ElementConverter>().CanConvertAtAll(), UpdateRate.SIM_200ms).Transition(this.overpressure, (StatesInstance smi) => !smi.master.RoomForPressure, UpdateRate.SIM_200ms);
			this.overpressure.Enter("OverPressure", delegate (StatesInstance smi)
			{
				smi.master.operational.SetActive(false, false);
			}).ToggleStatusItem(Db.Get().BuildingStatusItems.PressureOk, (object)null).Transition(this.converting, (StatesInstance smi) => smi.master.RoomForPressure, UpdateRate.SIM_200ms);
		}
	}

	[SerializeField]
	public float maxMass = 10f;

	[SerializeField]
	public bool hasMeter = true;

	[MyCmpAdd]
	private Storage storage;

	[MyCmpGet]
	private ElementConverter emitter;

	[MyCmpReq]
	private Operational operational;

	private MeterController meter;

	private bool RoomForPressure
	{
		get
		{
			int cell = Grid.PosToCell(base.transform.GetPosition());
			cell = Grid.CellAbove(cell);
			return !GameUtil.FloodFillCheck(this.OverPressure, cell, 3, true, true);
		}
	}

	protected override void OnSpawn()
	{
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		if (this.hasMeter)
		{
			this.meter = new MeterController(component, "U2H_meter_target", "meter", Meter.Offset.Behind, new Vector3(-0.4f, 0.5f, -0.1f), "U2H_meter_target", "U2H_meter_tank", "U2H_meter_waterbody", "U2H_meter_level");
		}
		base.smi.StartSM();
		this.UpdateMeter();
	}

	public void UpdateMeter()
	{
		if (this.hasMeter)
		{
			float positionPercent = Mathf.Clamp01(this.storage.MassStored() / this.storage.capacityKg);
			this.meter.SetPositionPercent(positionPercent);
		}
	}

	public bool OverPressure(int cell)
	{
		return Grid.Mass[cell] > this.maxMass;
	}
}

