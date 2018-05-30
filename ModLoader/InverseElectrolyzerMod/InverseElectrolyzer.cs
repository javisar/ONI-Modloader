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
		public class OnStates : State
		{
			public State waiting;

			public State working_pre;

			public State working;

			public State working_pst;
		}

		public State off;

		public OnStates on;

		public override void InitializeStates(out BaseState default_state)
		{
			default_state = this.off;
			this.off.PlayAnim("off").EventTransition(GameHashes.OperationalChanged, this.on, (StatesInstance smi) => smi.master.operational.IsOperational);
			this.on.PlayAnim("on").EventTransition(GameHashes.OperationalChanged, this.off, (StatesInstance smi) => !smi.master.operational.IsOperational).DefaultState(this.on.waiting);
			this.on.waiting.EventTransition(GameHashes.OnStorageChange, this.on.working_pre, (StatesInstance smi) => ((Component)smi.master).GetComponent<ElementConverter>().HasEnoughMassToStartConverting());
			this.on.working_pre.PlayAnim("working_pre").OnAnimQueueComplete(this.on.working);
			this.on.working.Enter(delegate (StatesInstance smi)
			{
				smi.master.operational.SetActive(true, false);
			}).QueueAnim("working_loop", true, null).EventTransition(GameHashes.OnStorageChange, this.on.working_pst, (StatesInstance smi) => !((Component)smi.master).GetComponent<ElementConverter>().CanConvertAtAll())
				.Exit(delegate (StatesInstance smi)
				{
					smi.master.operational.SetActive(false, false);
				});
			this.on.working_pst.PlayAnim("working_pst").OnAnimQueueComplete(this.on.waiting);
		}
	}

	[MyCmpGet]
	private Operational operational;

	//private ManualDeliveryKG[] deliveryComponents;

	protected override void OnSpawn()
	{
		base.OnSpawn();
		//this.deliveryComponents = base.GetComponents<ManualDeliveryKG>();
		this.OnConduitConnectionChanged(base.GetComponent<ConduitConsumer>().IsConnected);
		base.Subscribe(-2094018600, this.OnConduitConnectionChanged);
		base.smi.StartSM();
	}

	private void OnConduitConnectionChanged(object data)
	{
		/*
		bool pause = (bool)data;
		ManualDeliveryKG[] array = this.deliveryComponents;
		foreach (ManualDeliveryKG manualDeliveryKG in array)
		{
			Element element = ElementLoader.GetElement(manualDeliveryKG.requestedItemTag);
			if (element != null && element.IsLiquid)
			{
				manualDeliveryKG.Pause(pause, "pipe connected");
			}
		}
		*/
	}
}

