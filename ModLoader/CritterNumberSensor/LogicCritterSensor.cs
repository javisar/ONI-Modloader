using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSerialization;
using STRINGS;
using UnityEngine;

public class LogicCritterSensor : Switch, ISaveLoadable, IThresholdSwitch, ISim200ms
{
		
	[Serialize]
	public float thresholdCritters = 0f;

	[Serialize]
	public bool activateOnAboveThan;

	public float minCritters = 0f;

	public float maxCritters = 50f;

	//private float[] temperatures = new float[8];

	private float numCritters;

	private bool wasOn;


	public float Threshold
	{
		get
		{
			return this.thresholdCritters;
		}
		set
		{
			this.thresholdCritters = value;
		}
	}

	public bool ActivateAboveThreshold
	{
		get
		{
			return this.activateOnAboveThan;
		}
		set
		{
			this.activateOnAboveThan = value;
		}
	}

	public float CurrentValue
	{
		get
		{
			return this.GetCritters();
		}
	}

	public float RangeMin
	{
		get
		{
			return this.minCritters;
		}
	}

	public float RangeMax
	{
		get
		{
			return this.maxCritters;
		}
	}

	public LocString Title
	{
		get
		{
			return "Critter Number Threshold";
		}
	}

	public LocString ThresholdValueName
	{
		get
		{
			return UI.CODEX.CATEGORYNAMES.CREATURES;
		}
	}

	public string AboveToolTip
	{
		get
		{
			return "Switch will be on if the critter number is above {0}";
		}
	}

	public string BelowToolTip
	{
		get
		{
			return "Switch will be on if the critter number is below {0}";
		}
	}

	protected override void OnSpawn()
	{
		base.OnSpawn();
		//this.structureTemperature = GameComps.StructureTemperatures.GetHandle(base.gameObject);
		base.OnToggle += this.OnSwitchToggled;
		this.UpdateVisualState(true);
		this.wasOn = base.switchedOn;
	}

	public void Sim200ms(float dt)
	{
		this.numCritters = (float)Game.Instance.roomProber.GetCavityForCell(Grid.PosToCell(this)).creatures.Count;

		if (this.activateOnAboveThan)
		{
			if (!(this.numCritters > this.thresholdCritters) || base.IsSwitchedOn)
			{
				if (!(this.numCritters < this.thresholdCritters))
				{
					return;
				}
				if (!base.IsSwitchedOn)
				{
					return;
				}
			}
			this.Toggle();
		}
		else
		{
			if (!(this.numCritters > this.thresholdCritters) || !base.IsSwitchedOn)
			{
				if (!(this.numCritters < this.thresholdCritters))
				{
					return;
				}
				if (base.IsSwitchedOn)
				{
					return;
				}
			}
			this.Toggle();
		}
		
	}

	public float GetCritters()
	{
		return this.numCritters;
	}

	private void OnSwitchToggled(bool toggled_on)
	{
		this.UpdateVisualState(false);
		base.GetComponent<LogicPorts>().SendSignal(LogicSwitch.PORT_ID, base.switchedOn ? 1 : 0);
	}

	private void UpdateVisualState(bool force = false)
	{
		if (this.wasOn == base.switchedOn && !force)
		{
			return;
		}
		this.wasOn = base.switchedOn;
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		component.Play((!base.switchedOn) ? "on_pst" : "on_pre", KAnim.PlayMode.Once, 1f, 0f);
		component.Queue((!base.switchedOn) ? "off" : "on", KAnim.PlayMode.Once, 1f, 0f);
	}

	public float GetRangeMinInputField()
	{
		//return GameUtil.GetConvertedTemperature(this.RangeMin);
		return this.RangeMin;
	}

	public float GetRangeMaxInputField()
	{
		//return GameUtil.GetConvertedTemperature(this.RangeMax);
		return this.RangeMax;
	}

	public string Format(float value, bool units)
	{
		//bool displayUnits = units;
		//return GameUtil.GetFormattedTemperature(value, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, displayUnits);
		return "" + Convert.ToInt32(value);
	}

	public float ProcessedSliderValue(float input)
	{
		return Mathf.Round(input);
	}

	public float ProcessedInputValue(float input)
	{
		//return GameUtil.GetTemperatureConvertedToKelvin(input);
		return input;
	}

	public LocString ThresholdValueUnits()
	{
		/*
		LocString result = null;
		switch (GameUtil.temperatureUnit)
		{
			case GameUtil.TemperatureUnit.Celsius:
				result = UI.UNITSUFFIXES.TEMPERATURE.CELSIUS;
				break;
			case GameUtil.TemperatureUnit.Fahrenheit:
				result = UI.UNITSUFFIXES.TEMPERATURE.FAHRENHEIT;
				break;
			case GameUtil.TemperatureUnit.Kelvin:
				result = UI.UNITSUFFIXES.TEMPERATURE.KELVIN;
				break;
		}
		return result;
		*/
		return "";
	}
}

