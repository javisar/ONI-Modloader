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
		public State waiting;

		public State hasfilter;

		public State converting;

		public override void InitializeStates(out BaseState default_state)
		{
			default_state = this.waiting;
			this.waiting.EventTransition(GameHashes.OnStorageChange, this.hasfilter, (StatesInstance smi) => smi.master.HasFilter() && smi.master.operational.IsOperational).EventTransition(GameHashes.OperationalChanged, this.hasfilter, (StatesInstance smi) => smi.master.HasFilter() && smi.master.operational.IsOperational);
			this.hasfilter.EventTransition(GameHashes.OnStorageChange, this.converting, (StatesInstance smi) => smi.master.IsConvertable()).EventTransition(GameHashes.OperationalChanged, this.waiting, (StatesInstance smi) => !smi.master.operational.IsOperational).Enter("EnableConsumption", delegate (StatesInstance smi)
			{
				smi.master.elementConsumer.EnableConsumption(true);
			})
				.Exit("DisableConsumption", delegate (StatesInstance smi)
				{
					smi.master.elementConsumer.EnableConsumption(false);
				});
			this.converting.Enter("SetActive(true)", delegate (StatesInstance smi)
			{
				smi.master.operational.SetActive(true, false);
			}).Exit("SetActive(false)", delegate (StatesInstance smi)
			{
				smi.master.operational.SetActive(false, false);
			}).Enter("EnableConsumption", delegate (StatesInstance smi)
			{
				smi.master.elementConsumer.EnableConsumption(true);
			})
				.Exit("DisableConsumption", delegate (StatesInstance smi)
				{
					smi.master.elementConsumer.EnableConsumption(false);
				})
				.EventTransition(GameHashes.OnStorageChange, this.waiting, (StatesInstance smi) => !smi.master.IsConvertable())
				.EventTransition(GameHashes.OperationalChanged, this.waiting, (StatesInstance smi) => !smi.master.operational.IsOperational);
		}
	}

	[MyCmpGet]
	private Operational operational;

	[MyCmpGet]
	private Storage storage;

	[MyCmpGet]
	private ElementConverter elementConverter;

	[MyCmpGet]
	private ElementConsumer elementConsumer;

	public Tag filterTag;

	public bool HasFilter()
	{
		return this.elementConverter.HasEnoughMass(this.filterTag);
	}

	public bool IsConvertable()
	{
		return this.elementConverter.HasEnoughMassToStartConverting();
	}

	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		component.randomiseLoopedOffset = true;
	}

	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	public List<Descriptor> GetDescriptors(BuildingDef def)
	{
		return null;
	}
}

