using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld;

[StaticConstructorOnStartup]
public class CompRefuelable : ThingComp_VacuumAware, IThingGlower
{
	[CompilerGenerated]
	private sealed class _003CCompGetGizmosExtra_003Ed__51 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompRefuelable _003C_003E4__this;

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
		public _003CCompGetGizmosExtra_003Ed__51(int _003C_003E1__state)
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
			CompRefuelable CS_0024_003C_003E8__locals21 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (CS_0024_003C_003E8__locals21.Props.fuelIsMortarBarrel && Find.Storyteller.difficulty.classicMortars)
				{
					return false;
				}
				if (!CS_0024_003C_003E8__locals21.Props.hideGizmosIfNotPlayerFaction || CS_0024_003C_003E8__locals21.parent.Faction == Faction.OfPlayer)
				{
					if (Find.Selector.SelectedObjects.Count == 1)
					{
						_003C_003E2__current = new Gizmo_SetFuelLevel(CS_0024_003C_003E8__locals21);
						_003C_003E1__state = 1;
						return true;
					}
					if (CS_0024_003C_003E8__locals21.Props.targetFuelLevelConfigurable)
					{
						_003C_003E2__current = new Command_SetTargetFuelLevel
						{
							refuelable = CS_0024_003C_003E8__locals21,
							defaultLabel = "CommandSetTargetFuelLevel".Translate(),
							defaultDesc = "CommandSetTargetFuelLevelDesc".Translate(),
							icon = SetTargetFuelLevelCommand
						};
						_003C_003E1__state = 2;
						return true;
					}
					goto IL_0118;
				}
				goto IL_0200;
			case 1:
				_003C_003E1__state = -1;
				goto IL_0200;
			case 2:
				_003C_003E1__state = -1;
				goto IL_0118;
			case 3:
				_003C_003E1__state = -1;
				goto IL_0200;
			case 4:
				_003C_003E1__state = -1;
				_003C_003E2__current = new Command_Action
				{
					defaultLabel = "DEV: Set fuel to 0.1",
					action = delegate
					{
						CS_0024_003C_003E8__locals21.fuel = 0.1f;
						CS_0024_003C_003E8__locals21.parent.BroadcastCompSignal("Refueled");
					}
				};
				_003C_003E1__state = 5;
				return true;
			case 5:
				_003C_003E1__state = -1;
				_003C_003E2__current = new Command_Action
				{
					defaultLabel = "DEV: Fuel -20%",
					action = delegate
					{
						CS_0024_003C_003E8__locals21.ConsumeFuel(CS_0024_003C_003E8__locals21.Props.fuelCapacity * 0.2f);
					}
				};
				_003C_003E1__state = 6;
				return true;
			case 6:
				_003C_003E1__state = -1;
				_003C_003E2__current = new Command_Action
				{
					defaultLabel = "DEV: Set fuel to max",
					action = delegate
					{
						CS_0024_003C_003E8__locals21.fuel = CS_0024_003C_003E8__locals21.Props.fuelCapacity;
						CS_0024_003C_003E8__locals21.parent.BroadcastCompSignal("Refueled");
					}
				};
				_003C_003E1__state = 7;
				return true;
			case 7:
				{
					_003C_003E1__state = -1;
					break;
				}
				IL_0118:
				if (CS_0024_003C_003E8__locals21.Props.showAllowAutoRefuelToggle)
				{
					string str = (CS_0024_003C_003E8__locals21.allowAutoRefuel ? "On".Translate() : "Off".Translate());
					_003C_003E2__current = new Command_Toggle
					{
						isActive = () => CS_0024_003C_003E8__locals21.allowAutoRefuel,
						toggleAction = delegate
						{
							CS_0024_003C_003E8__locals21.allowAutoRefuel = !CS_0024_003C_003E8__locals21.allowAutoRefuel;
						},
						defaultLabel = "CommandToggleAllowAutoRefuel".Translate(),
						defaultDesc = "CommandToggleAllowAutoRefuelDescMult".Translate(str.UncapitalizeFirst().Named("ONOFF")),
						icon = (CS_0024_003C_003E8__locals21.allowAutoRefuel ? TexCommand.ForbidOn : TexCommand.ForbidOff),
						Order = 20f,
						hotKey = KeyBindingDefOf.Command_ItemForbid
					};
					_003C_003E1__state = 3;
					return true;
				}
				goto IL_0200;
				IL_0200:
				if (DebugSettings.ShowDevGizmos)
				{
					_003C_003E2__current = new Command_Action
					{
						defaultLabel = "DEV: Set fuel to 0",
						action = delegate
						{
							CS_0024_003C_003E8__locals21.fuel = 0f;
							CS_0024_003C_003E8__locals21.parent.BroadcastCompSignal("Refueled");
						}
					};
					_003C_003E1__state = 4;
					return true;
				}
				break;
			}
			return false;
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
			_003CCompGetGizmosExtra_003Ed__51 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CCompGetGizmosExtra_003Ed__51(0)
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

	[CompilerGenerated]
	private sealed class _003CSpecialDisplayStats_003Ed__41 : IEnumerable<StatDrawEntry>, IEnumerable, IEnumerator<StatDrawEntry>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private StatDrawEntry _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompRefuelable _003C_003E4__this;

		StatDrawEntry IEnumerator<StatDrawEntry>.Current
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
		public _003CSpecialDisplayStats_003Ed__41(int _003C_003E1__state)
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
			CompRefuelable compRefuelable = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (compRefuelable.parent.def.building != null && compRefuelable.parent.def.building.IsTurret)
				{
					TaggedString taggedString = "RearmCostExplanation".Translate();
					if (compRefuelable.Props.factorByDifficulty)
					{
						taggedString += " (" + "RearmCostExplanationDifficulty".Translate() + ")";
					}
					taggedString += ".";
					_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.Building, "RearmCost".Translate(), GenLabel.ThingLabel(compRefuelable.Props.fuelFilter.AnyAllowedDef, null, compRefuelable.GetFuelCountToFullyRefuel()).CapitalizeFirst(), taggedString, 3171);
					_003C_003E1__state = 1;
					return true;
				}
				break;
			case 1:
				_003C_003E1__state = -1;
				break;
			}
			return false;
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
		IEnumerator<StatDrawEntry> IEnumerable<StatDrawEntry>.GetEnumerator()
		{
			_003CSpecialDisplayStats_003Ed__41 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CSpecialDisplayStats_003Ed__41(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<StatDrawEntry>)this).GetEnumerator();
		}
	}

	private float fuel;

	private float configuredTargetFuelLevel = -1f;

	public bool allowAutoRefuel = true;

	private CompFlickable flickComp;

	private CompExplosive explosiveComp;

	public const string RefueledSignal = "Refueled";

	public const string RanOutOfFuelSignal = "RanOutOfFuel";

	private static readonly Texture2D SetTargetFuelLevelCommand = ContentFinder<Texture2D>.Get("UI/Commands/SetTargetFuelLevel");

	private static readonly Vector2 FuelBarSize = new Vector2(1f, 0.2f);

	private static readonly Material FuelBarFilledMat = SolidColorMaterials.SimpleSolidColorMaterial(new Color(0.6f, 0.56f, 0.13f));

	private static readonly Material FuelBarUnfilledMat = SolidColorMaterials.SimpleSolidColorMaterial(new Color(0.3f, 0.3f, 0.3f));

	protected override bool FunctionsInVacuum => Props.functionsInVacuum;

	public float TargetFuelLevel
	{
		get
		{
			if (configuredTargetFuelLevel >= 0f)
			{
				return configuredTargetFuelLevel;
			}
			if (Props.targetFuelLevelConfigurable)
			{
				return Props.initialConfigurableTargetFuelLevel;
			}
			return Props.fuelCapacity;
		}
		set
		{
			configuredTargetFuelLevel = Mathf.Clamp(value, 0f, Props.fuelCapacity);
		}
	}

	public CompProperties_Refuelable Props => (CompProperties_Refuelable)props;

	public float Fuel => fuel;

	public float FuelPercentOfTarget => fuel / TargetFuelLevel;

	public float FuelPercentOfMax => fuel / Props.fuelCapacity;

	public bool IsFull => TargetFuelLevel - fuel < 1f * Props.FuelMultiplierCurrentDifficulty;

	public bool HasFuel
	{
		get
		{
			if (fuel > 0f && fuel >= Props.minimumFueledThreshold)
			{
				if (!FunctionsInVacuum)
				{
					return !base.InVacuum;
				}
				return true;
			}
			return false;
		}
	}

	private float ConsumptionRatePerTick => Props.fuelConsumptionRate / 60000f;

	public bool ShouldAutoRefuelNow
	{
		get
		{
			if (FuelPercentOfTarget <= Props.autoRefuelPercent && !IsFull && TargetFuelLevel > 0f)
			{
				return ShouldAutoRefuelNowIgnoringFuelPct;
			}
			return false;
		}
	}

	public bool ShouldAutoRefuelNowIgnoringFuelPct
	{
		get
		{
			if (!parent.IsBurning() && (flickComp == null || flickComp.SwitchIsOn) && parent.Map.designationManager.DesignationOn(parent, DesignationDefOf.Flick) == null)
			{
				return parent.Map.designationManager.DesignationOn(parent, DesignationDefOf.Deconstruct) == null;
			}
			return false;
		}
	}

	public bool ShouldBeLitNow()
	{
		return HasFuel;
	}

	public override void Initialize(CompProperties props)
	{
		base.Initialize(props);
		allowAutoRefuel = Props.initialAllowAutoRefuel;
		fuel = Props.fuelCapacity * Props.initialFuelPercent;
	}

	public override void PostSpawnSetup(bool respawningAfterLoad)
	{
		flickComp = parent.GetComp<CompFlickable>();
		explosiveComp = parent.GetComp<CompExplosive>();
	}

	public override void PostExposeData()
	{
		base.PostExposeData();
		Scribe_Values.Look(ref fuel, "fuel", 0f);
		Scribe_Values.Look(ref configuredTargetFuelLevel, "configuredTargetFuelLevel", -1f);
		Scribe_Values.Look(ref allowAutoRefuel, "allowAutoRefuel", defaultValue: false);
		if (Scribe.mode == LoadSaveMode.PostLoadInit && !Props.showAllowAutoRefuelToggle)
		{
			allowAutoRefuel = Props.initialAllowAutoRefuel;
		}
	}

	public override void PostDraw()
	{
		base.PostDraw();
		if (!allowAutoRefuel)
		{
			parent.Map.overlayDrawer.DrawOverlay(parent, OverlayTypes.ForbiddenRefuel);
		}
		else if (!HasFuel && Props.drawOutOfFuelOverlay)
		{
			parent.Map.overlayDrawer.DrawOverlay(parent, OverlayTypes.OutOfFuel);
		}
		if (Props.drawFuelGaugeInMap)
		{
			GenDraw.FillableBarRequest r = default(GenDraw.FillableBarRequest);
			r.center = parent.DrawPos + Vector3.up * 0.1f;
			r.size = FuelBarSize;
			r.fillPercent = FuelPercentOfMax;
			r.filledMat = FuelBarFilledMat;
			r.unfilledMat = FuelBarUnfilledMat;
			r.margin = 0.15f;
			Rot4 rotation = parent.Rotation;
			rotation.Rotate(RotationDirection.Clockwise);
			r.rotation = rotation;
			GenDraw.DrawFillableBar(r);
		}
	}

	public override void PostDestroy(DestroyMode mode, Map previousMap)
	{
		base.PostDestroy(mode, previousMap);
		if ((!Props.fuelIsMortarBarrel || !Find.Storyteller.difficulty.classicMortars) && mode != 0 && previousMap != null && Props.fuelFilter.AllowedDefCount == 1 && Props.initialFuelPercent == 0f)
		{
			ThingDef thingDef = Props.fuelFilter.AllowedThingDefs.First();
			int num = Mathf.FloorToInt(1f * fuel);
			while (num > 0)
			{
				Thing thing = ThingMaker.MakeThing(thingDef);
				thing.stackCount = Mathf.Min(num, thingDef.stackLimit);
				num -= thing.stackCount;
				GenPlace.TryPlaceThing(thing, parent.Position, previousMap, ThingPlaceMode.Near, null, null, null);
			}
		}
	}

	public override string CompInspectStringExtra()
	{
		if (Props.fuelIsMortarBarrel && Find.Storyteller.difficulty.classicMortars)
		{
			return string.Empty;
		}
		string text = base.CompInspectStringExtra();
		text = ((text != null) ? (text + "\n") : string.Empty);
		text = text + Props.FuelLabel + ": " + fuel.ToStringDecimalIfSmall() + " / " + Props.fuelCapacity.ToStringDecimalIfSmall();
		if (!Props.consumeFuelOnlyWhenUsed && HasFuel)
		{
			int numTicks = (int)(fuel / Props.fuelConsumptionRate * 60000f);
			text = text + " (" + numTicks.ToStringTicksToPeriod() + ")";
		}
		if (!HasFuel && !Props.outOfFuelMessage.NullOrEmpty())
		{
			string value = ((parent.def.building != null && parent.def.building.IsTurret) ? ("CannotShoot".Translate() + ": " + Props.outOfFuelMessage).Resolve() : Props.outOfFuelMessage);
			text += $"\n{value} ({GetFuelCountToFullyRefuel()}x {Props.fuelFilter.AnyAllowedDef.label})";
		}
		if (Props.targetFuelLevelConfigurable)
		{
			text += "\n" + "ConfiguredTargetFuelLevel".Translate(TargetFuelLevel.ToStringDecimalIfSmall());
		}
		return text;
	}

	[IteratorStateMachine(typeof(_003CSpecialDisplayStats_003Ed__41))]
	public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CSpecialDisplayStats_003Ed__41(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override void CompTick()
	{
		base.CompTick();
		CompPowerTrader comp = parent.GetComp<CompPowerTrader>();
		if (!Props.consumeFuelOnlyWhenUsed && (flickComp == null || flickComp.SwitchIsOn) && (!Props.consumeFuelOnlyWhenPowered || (comp != null && comp.PowerOn)) && !Props.externalTicking)
		{
			ConsumeFuel(ConsumptionRatePerTick);
		}
		if (Props.fuelConsumptionPerTickInRain > 0f && parent.Spawned && parent.Map.weatherManager.RainRate > 0.4f && !parent.Map.roofGrid.Roofed(parent.Position) && !Props.externalTicking)
		{
			ConsumeFuel(Props.fuelConsumptionPerTickInRain);
		}
	}

	public void ConsumeFuel(float amount)
	{
		if ((!Props.fuelIsMortarBarrel || !Find.Storyteller.difficulty.classicMortars) && !(fuel <= 0f))
		{
			fuel -= amount;
			if (fuel <= 0f)
			{
				fuel = 0f;
				Notify_RanOutOfFuel();
			}
		}
	}

	private void Notify_RanOutOfFuel()
	{
		if (Props.destroyOnNoFuel)
		{
			parent.Destroy();
		}
		parent.BroadcastCompSignal("RanOutOfFuel");
	}

	public void Refuel(List<Thing> fuelThings)
	{
		if (Props.atomicFueling && fuelThings.Sum((Thing t) => t.stackCount) < GetFuelCountToFullyRefuel())
		{
			Log.ErrorOnce("Error refueling; not enough fuel available for proper atomic refuel", 19586442);
			return;
		}
		int num = GetFuelCountToFullyRefuel();
		while (num > 0 && fuelThings.Count > 0)
		{
			Thing thing = fuelThings.Pop();
			int num2 = Mathf.Min(num, thing.stackCount);
			Refuel(num2);
			thing.SplitOff(num2).Destroy();
			num -= num2;
		}
	}

	public void Refuel(float amount)
	{
		fuel += amount * Props.FuelMultiplierCurrentDifficulty;
		if (fuel > Props.fuelCapacity)
		{
			fuel = Props.fuelCapacity;
		}
		parent.BroadcastCompSignal("Refueled");
	}

	public AcceptanceReport CanEjectFuel()
	{
		if (!Props.canEjectFuel)
		{
			return false;
		}
		CompExplosive compExplosive = explosiveComp;
		if (compExplosive != null && compExplosive.wickStarted)
		{
			return "AboutToExplode".Translate();
		}
		if (Fuel == 0f)
		{
			return "RefuelableNoFuelToEject".Translate();
		}
		return true;
	}

	public void EjectFuel()
	{
		ThingDef thingDef = Props.fuelFilter.AllowedThingDefs.First();
		int num = Mathf.FloorToInt(fuel);
		while (num > 0)
		{
			Thing thing = ThingMaker.MakeThing(thingDef);
			thing.stackCount = Mathf.Min(num, thingDef.stackLimit);
			num -= thing.stackCount;
			GenPlace.TryPlaceThing(thing, parent.Position, parent.Map, ThingPlaceMode.Near, null, null, null);
			thing.SetForbidden(value: true);
		}
		fuel = 0f;
		Notify_RanOutOfFuel();
	}

	public void Notify_UsedThisTick()
	{
		ConsumeFuel(ConsumptionRatePerTick);
	}

	public int GetFuelCountToFullyRefuel()
	{
		if (Props.atomicFueling)
		{
			return Mathf.CeilToInt(Props.fuelCapacity / Props.FuelMultiplierCurrentDifficulty);
		}
		return Mathf.Max(Mathf.CeilToInt((TargetFuelLevel - fuel) / Props.FuelMultiplierCurrentDifficulty), 1);
	}

	[IteratorStateMachine(typeof(_003CCompGetGizmosExtra_003Ed__51))]
	public override IEnumerable<Gizmo> CompGetGizmosExtra()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CCompGetGizmosExtra_003Ed__51(-2)
		{
			_003C_003E4__this = this
		};
	}
}
