using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld;

public class Comp_AtmosphericHeater : CompTempControl, IThingGlower
{
	private class Command_GroupedTempChange : Command_Action
	{
		private Comp_AtmosphericHeater heater;

		private float offset;

		public Command_GroupedTempChange(Comp_AtmosphericHeater heater, float offset)
		{
			this.heater = heater;
			this.offset = offset;
		}

		public override void ProcessInput(Event ev)
		{
		}

		public override void ProcessGroupInput(Event ev, List<Gizmo> group)
		{
			heater.InterfaceChangeTargetTemperature(offset);
		}
	}

	[CompilerGenerated]
	private sealed class _003CCompGetGizmosExtra_003Ed__20 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Comp_AtmosphericHeater _003C_003E4__this;

		Gizmo IEnumerator<Gizmo>.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		[DebuggerHidden]
		public _003CCompGetGizmosExtra_003Ed__20(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			Comp_AtmosphericHeater CS_0024_003C_003E8__locals10 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				float num5 = CS_0024_003C_003E8__locals10.RoundedToCurrentTempModeOffset(-10f);
				_003C_003E2__current = new Command_GroupedTempChange(CS_0024_003C_003E8__locals10, num5)
				{
					defaultLabel = num5.ToStringTemperatureOffset("F0"),
					defaultDesc = "CommandLowerTempDesc".Translate(),
					hotKey = KeyBindingDefOf.Misc5,
					icon = ContentFinder<Texture2D>.Get("UI/Commands/TempLower")
				};
				_003C_003E1__state = 1;
				return true;
			}
			case 1:
			{
				_003C_003E1__state = -1;
				float num4 = CS_0024_003C_003E8__locals10.RoundedToCurrentTempModeOffset(-1f);
				_003C_003E2__current = new Command_GroupedTempChange(CS_0024_003C_003E8__locals10, num4)
				{
					defaultLabel = num4.ToStringTemperatureOffset("F0"),
					defaultDesc = "CommandLowerTempDesc".Translate(),
					hotKey = KeyBindingDefOf.Misc4,
					icon = ContentFinder<Texture2D>.Get("UI/Commands/TempLower")
				};
				_003C_003E1__state = 2;
				return true;
			}
			case 2:
				_003C_003E1__state = -1;
				_003C_003E2__current = new Command_Action
				{
					action = delegate
					{
						CS_0024_003C_003E8__locals10.TargetTemperature = 21f;
						SoundDefOf.Tick_Tiny.PlayOneShotOnCamera();
						CS_0024_003C_003E8__locals10.ThrowCurrentTemperatureText();
					},
					defaultLabel = "CommandResetTemp".Translate(),
					defaultDesc = "CommandResetTempDesc".Translate(),
					hotKey = KeyBindingDefOf.Misc1,
					icon = ContentFinder<Texture2D>.Get("UI/Commands/TempReset")
				};
				_003C_003E1__state = 3;
				return true;
			case 3:
			{
				_003C_003E1__state = -1;
				float num3 = CS_0024_003C_003E8__locals10.RoundedToCurrentTempModeOffset(1f);
				_003C_003E2__current = new Command_GroupedTempChange(CS_0024_003C_003E8__locals10, num3)
				{
					defaultLabel = "+" + num3.ToStringTemperatureOffset("F0"),
					defaultDesc = "CommandRaiseTempDesc".Translate(),
					hotKey = KeyBindingDefOf.Misc2,
					icon = ContentFinder<Texture2D>.Get("UI/Commands/TempRaise")
				};
				_003C_003E1__state = 4;
				return true;
			}
			case 4:
			{
				_003C_003E1__state = -1;
				float num2 = CS_0024_003C_003E8__locals10.RoundedToCurrentTempModeOffset(10f);
				_003C_003E2__current = new Command_GroupedTempChange(CS_0024_003C_003E8__locals10, num2)
				{
					defaultLabel = "+" + num2.ToStringTemperatureOffset("F0"),
					defaultDesc = "CommandRaiseTempDesc".Translate(),
					hotKey = KeyBindingDefOf.Misc3,
					icon = ContentFinder<Texture2D>.Get("UI/Commands/TempRaise")
				};
				_003C_003E1__state = 5;
				return true;
			}
			case 5:
				_003C_003E1__state = -1;
				return false;
			}
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<Gizmo> IEnumerable<Gizmo>.GetEnumerator()
		{
			_003CCompGetGizmosExtra_003Ed__20 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CCompGetGizmosExtra_003Ed__20(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Gizmo>)this).GetEnumerator();
		}
	}

	private const int DisabledTextureIndex = 0;

	private const int EnabledTextureIndex = 1;

	private const int MaxOffset = 10;

	private CompPowerTrader powerTraderComp;

	private CompHeatPusher heatPusherComp;

	private CompRefuelable refuelableComp;

	private GameCondition_UnnaturalHeat Condition
	{
		get
		{
			GameCondition_UnnaturalHeat gameCondition_UnnaturalHeat = parent.Map.gameConditionManager.GetActiveCondition(GameConditionDefOf.UnnaturalHeat) as GameCondition_UnnaturalHeat;
			if (gameCondition_UnnaturalHeat == null)
			{
				gameCondition_UnnaturalHeat = (GameCondition_UnnaturalHeat)GameConditionMaker.MakeCondition(GameConditionDefOf.UnnaturalHeat);
				parent.Map.GameConditionManager.RegisterCondition(gameCondition_UnnaturalHeat);
				gameCondition_UnnaturalHeat.Permanent = true;
			}
			return gameCondition_UnnaturalHeat;
		}
	}

	public override float TargetTemperature
	{
		get
		{
			return Condition.heaterTargetTemp;
		}
		set
		{
			Condition.heaterTargetTemp = value;
		}
	}

	private bool Powered => powerTraderComp.PowerOn;

	public int TempOffset
	{
		get
		{
			if (!Powered || !refuelableComp.HasFuel || !(TargetTemperature > parent.Map.mapTemperature.OutdoorTemp))
			{
				return 0;
			}
			return 10;
		}
	}

	private bool Working
	{
		get
		{
			if (Powered && refuelableComp.HasFuel)
			{
				return TargetTemperature > parent.Map.mapTemperature.OutdoorTemp - 1f;
			}
			return false;
		}
	}

	public bool ShouldBeLitNow()
	{
		return Working;
	}

	public override void PostSpawnSetup(bool respawningAfterLoad)
	{
		if (!ModLister.CheckAnomaly("Atmospheric heater"))
		{
			parent.Destroy();
			return;
		}
		base.PostSpawnSetup(respawningAfterLoad);
		powerTraderComp = parent.GetComp<CompPowerTrader>();
		heatPusherComp = parent.GetComp<CompHeatPusher>();
		refuelableComp = parent.GetComp<CompRefuelable>();
	}

	public override void CompTick()
	{
		if (Working)
		{
			powerTraderComp.PowerOutput = 0f - powerTraderComp.Props.PowerConsumption;
			refuelableComp.Notify_UsedThisTick();
			heatPusherComp.enabled = true;
		}
		else
		{
			powerTraderComp.PowerOutput = (0f - powerTraderComp.Props.PowerConsumption) * base.Props.lowPowerConsumptionFactor;
			heatPusherComp.enabled = false;
		}
		int num = (Working ? 1 : 0);
		if (parent.overrideGraphicIndex != num)
		{
			parent.overrideGraphicIndex = num;
			parent.DirtyMapMesh(parent.Map);
			parent.TryGetComp<CompGlower>()?.UpdateLit(parent.Map);
		}
		operatingAtHighPower = Working;
	}

	[IteratorStateMachine(typeof(_003CCompGetGizmosExtra_003Ed__20))]
	public override IEnumerable<Gizmo> CompGetGizmosExtra()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CCompGetGizmosExtra_003Ed__20(-2)
		{
			_003C_003E4__this = this
		};
	}
}
