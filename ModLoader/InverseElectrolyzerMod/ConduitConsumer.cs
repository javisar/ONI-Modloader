// ConduitConsumer
using STRINGS;
using System;
using UnityEngine;

[SkipSaveFileSerialization]
public class ConduitConsumer : KMonoBehaviour
{
	public enum WrongElementResult
	{
		Destroy,
		Dump,
		Store
	}

	[SerializeField]
	public ConduitType conduitType;

	[SerializeField]
	public bool ignoreMinMassCheck;

	[SerializeField]
	public Tag capacityTag = GameTags.Any;

	[SerializeField]
	public float capacityKG = float.PositiveInfinity;

	[SerializeField]
	public bool forceAlwaysSatisfied;

	[SerializeField]
	public bool alwaysConsume;

	[NonSerialized]
	public bool isConsuming = true;

	[MyCmpReq]
	private Operational operational;

	[MyCmpReq]
	private Building building;

	[MyCmpGet]
	public Storage storage;

	private int utilityCell = -1;

	public float consumptionRate = float.PositiveInfinity;

	public static readonly Operational.Flag elementRequirementFlag = new Operational.Flag("elementRequired", Operational.Flag.Type.Requirement);

	private GameScenePartitionerEntry partitionerEntry;

	private bool satisfied;

	public WrongElementResult wrongElementResult;

	public bool IsConnected
	{
		get
		{
			GameObject gameObject = Grid.Objects[this.utilityCell, (this.conduitType != ConduitType.Gas) ? 16 : 12];
			return (UnityEngine.Object)gameObject != (UnityEngine.Object)null && (UnityEngine.Object)gameObject.GetComponent<BuildingComplete>() != (UnityEngine.Object)null;
		}
	}

	public bool CanConsume
	{
		get
		{
			bool result = false;
			if (this.IsConnected)
			{
				ConduitFlow conduitManager = this.GetConduitManager();
				ConduitFlow.ConduitContents contents = conduitManager.GetContents(this.utilityCell);
				result = (contents.mass > 0f);
			}
			return result;
		}
	}

	public ConduitType TypeOfConduit
	{
		get
		{
			return this.conduitType;
		}
	}

	public bool IsAlmostEmpty
	{
		get
		{
			return !this.ignoreMinMassCheck && this.MassAvailable < this.ConsumptionRate * 30f;
		}
	}

	public bool IsEmpty
	{
		get
		{
			return !this.ignoreMinMassCheck && (this.MassAvailable == 0f || this.MassAvailable < this.ConsumptionRate);
		}
	}

	public float ConsumptionRate
	{
		get
		{
			return this.consumptionRate;
		}
	}

	public bool IsSatisfied
	{
		get
		{
			return this.satisfied || !this.isConsuming;
		}
		set
		{
			this.satisfied = (value || this.forceAlwaysSatisfied);
		}
	}

	public float MassAvailable
	{
		get
		{
			int utilityInputCell = this.building.GetUtilityInputCell();
			ConduitFlow conduitManager = this.GetConduitManager();
			ConduitFlow.ConduitContents contents = conduitManager.GetContents(utilityInputCell);
			return contents.mass;
		}
	}

	public void SetConduitData(ConduitType type)
	{
		this.conduitType = type;
	}

	private ConduitFlow GetConduitManager()
	{
		switch (this.conduitType)
		{
		case ConduitType.Gas:
			return Game.Instance.gasConduitFlow;
		case ConduitType.Liquid:
			return Game.Instance.liquidConduitFlow;
		default:
			return null;
		}
	}

	protected override void OnSpawn()
	{
		Debug.LogWarning("ConduitConsumer - OnSpawn");
		base.OnSpawn();
		this.utilityCell = this.building.GetUtilityInputCell();
		ScenePartitionerLayer layer = GameScenePartitioner.Instance.objectLayers[(this.conduitType != ConduitType.Gas) ? 16 : 12];
		this.partitionerEntry = GameScenePartitioner.Instance.Add("ConduitConsumer.OnSpawn", base.gameObject, this.utilityCell, layer, this.OnConduitConnectionChanged);
		this.GetConduitManager().AddConduitUpdater(this.ConduitUpdate, ConduitFlowPriority.Default);
		this.OnConduitConnectionChanged(null);
	}

	protected override void OnCleanUp()
	{
		this.GetConduitManager().RemoveConduitUpdater(this.ConduitUpdate);
		if (this.partitionerEntry != null)
		{
			this.partitionerEntry.Release();
		}
		base.OnCleanUp();
	}

	private void OnConduitConnectionChanged(object data)
	{
		base.Trigger(-2094018600, this.IsConnected);
	}

	private void ConduitUpdate(float dt)
	{
		if (this.isConsuming)
		{
			ConduitFlow conduitManager = this.GetConduitManager();
			this.Consume(dt, conduitManager);
		}
	}

	private void Consume(float dt, ConduitFlow conduit_mgr)
	{
		if (this.IsConnected)
		{
			ConduitFlow.ConduitContents contents = conduit_mgr.GetContents(this.utilityCell);
			if (contents.mass > 0f)
			{
				this.IsSatisfied = true;
				if (!this.alwaysConsume && !this.operational.IsOperational)
				{
					return;
				}
				float num = (!(this.capacityTag != GameTags.Any)) ? this.storage.MassStored() : this.storage.GetMassAvailable(this.capacityTag);
				float b = Mathf.Min(this.storage.RemainingCapacity(), this.capacityKG - num);
				float a = this.ConsumptionRate * dt;
				a = Mathf.Min(a, b);
				float num2 = 0f;
				if (a > 0f)
				{
					ConduitFlow.ConduitContents conduitContents = conduit_mgr.RemoveElement(this.utilityCell, a);
					num2 = conduitContents.mass;
				}
				Element element = ElementLoader.FindElementByHash(contents.element);
				bool flag = element.HasTag(this.capacityTag);
				if (num2 > 0f && this.capacityTag != GameTags.Any && !flag)
				{
					base.Trigger(-794517298, new BuildingHP.DamageSourceInfo
					{
						damage = 1,
						source = BUILDINGS.DAMAGESOURCES.BAD_INPUT_ELEMENT,
						popString = UI.GAMEOBJECTEFFECTS.DAMAGE_POPS.WRONG_ELEMENT
					});
				}
				if (flag || this.wrongElementResult == WrongElementResult.Store || contents.element == SimHashes.Vacuum || this.capacityTag == GameTags.Any)
				{
					if (num2 > 0f)
					{
						int disease_count = (int)((float)contents.diseaseCount * (num2 / contents.mass));
						Element element2 = ElementLoader.FindElementByHash(contents.element);
						switch (this.conduitType)
						{
						case ConduitType.Liquid:
							if (element2.IsLiquid)
							{
								this.storage.AddLiquid(contents.element, num2, contents.temperature, contents.diseaseIdx, disease_count, true, false);
							}
							else
							{
								Debug.LogWarning("Liquid conduit consumer consuming non liquid: " + element2.id.ToString(), null);
							}
							break;
						case ConduitType.Gas:
							if (element2.IsGas)
							{
								this.storage.AddGasChunk(contents.element, num2, contents.temperature, contents.diseaseIdx, disease_count, true, false);
							}
							else
							{
								Debug.LogWarning("Gas conduit consumer consuming non gas: " + element2.id.ToString(), null);
							}
							break;
						}
					}
				}
				else if (num2 > 0f && this.wrongElementResult == WrongElementResult.Dump)
				{
					int disease_count2 = (int)((float)contents.diseaseCount * (num2 / contents.mass));
					int gameCell = Grid.PosToCell(base.transform.GetPosition());
					SimMessages.AddRemoveSubstance(gameCell, contents.element, CellEventLogger.Instance.ConduitConsumerWrongElement, num2, contents.temperature, contents.diseaseIdx, disease_count2, -1);
				}
			}
			else
			{
				this.IsSatisfied = false;
			}
		}
		else
		{
			this.IsSatisfied = false;
		}
	}
}
