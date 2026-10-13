using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002DDD RID: 11741
	[StaticConstructorOnStartup]
	public class CompRefuelable : ThingComp_VacuumAware, IThingGlower
	{
		// Token: 0x17002A2D RID: 10797
		// (get) Token: 0x0601083C RID: 67644 RVA: 0x004C2F57 File Offset: 0x004C1157
		protected override bool FunctionsInVacuum
		{
			get
			{
				return this.Props.functionsInVacuum;
			}
		}

		// Token: 0x17002A2E RID: 10798
		// (get) Token: 0x0601083D RID: 67645 RVA: 0x004C2F64 File Offset: 0x004C1164
		// (set) Token: 0x0601083E RID: 67646 RVA: 0x004C2F9E File Offset: 0x004C119E
		public float TargetFuelLevel
		{
			get
			{
				if (this.configuredTargetFuelLevel >= 0f)
				{
					return this.configuredTargetFuelLevel;
				}
				if (this.Props.targetFuelLevelConfigurable)
				{
					return this.Props.initialConfigurableTargetFuelLevel;
				}
				return this.Props.fuelCapacity;
			}
			set
			{
				this.configuredTargetFuelLevel = Mathf.Clamp(value, 0f, this.Props.fuelCapacity);
			}
		}

		// Token: 0x17002A2F RID: 10799
		// (get) Token: 0x0601083F RID: 67647 RVA: 0x004C2FBC File Offset: 0x004C11BC
		public CompProperties_Refuelable Props
		{
			get
			{
				return (CompProperties_Refuelable)this.props;
			}
		}

		// Token: 0x17002A30 RID: 10800
		// (get) Token: 0x06010840 RID: 67648 RVA: 0x004C2FC9 File Offset: 0x004C11C9
		public float Fuel
		{
			get
			{
				return this.fuel;
			}
		}

		// Token: 0x17002A31 RID: 10801
		// (get) Token: 0x06010841 RID: 67649 RVA: 0x004C2FD1 File Offset: 0x004C11D1
		public float FuelPercentOfTarget
		{
			get
			{
				return this.fuel / this.TargetFuelLevel;
			}
		}

		// Token: 0x17002A32 RID: 10802
		// (get) Token: 0x06010842 RID: 67650 RVA: 0x004C2FE0 File Offset: 0x004C11E0
		public float FuelPercentOfMax
		{
			get
			{
				return this.fuel / this.Props.fuelCapacity;
			}
		}

		// Token: 0x17002A33 RID: 10803
		// (get) Token: 0x06010843 RID: 67651 RVA: 0x004C2FF4 File Offset: 0x004C11F4
		public bool IsFull
		{
			get
			{
				return this.TargetFuelLevel - this.fuel < 1f * this.Props.FuelMultiplierCurrentDifficulty;
			}
		}

		// Token: 0x17002A34 RID: 10804
		// (get) Token: 0x06010844 RID: 67652 RVA: 0x004C3016 File Offset: 0x004C1216
		public bool HasFuel
		{
			get
			{
				return this.fuel > 0f && this.fuel >= this.Props.minimumFueledThreshold && (this.FunctionsInVacuum || !base.InVacuum);
			}
		}

		// Token: 0x17002A35 RID: 10805
		// (get) Token: 0x06010845 RID: 67653 RVA: 0x004C304D File Offset: 0x004C124D
		private float ConsumptionRatePerTick
		{
			get
			{
				return this.Props.fuelConsumptionRate / 60000f;
			}
		}

		// Token: 0x17002A36 RID: 10806
		// (get) Token: 0x06010846 RID: 67654 RVA: 0x004C3060 File Offset: 0x004C1260
		public bool ShouldAutoRefuelNow
		{
			get
			{
				return this.FuelPercentOfTarget <= this.Props.autoRefuelPercent && !this.IsFull && this.TargetFuelLevel > 0f && this.ShouldAutoRefuelNowIgnoringFuelPct;
			}
		}

		// Token: 0x17002A37 RID: 10807
		// (get) Token: 0x06010847 RID: 67655 RVA: 0x004C3094 File Offset: 0x004C1294
		public bool ShouldAutoRefuelNowIgnoringFuelPct
		{
			get
			{
				return !this.parent.IsBurning() && (this.flickComp == null || this.flickComp.SwitchIsOn) && this.parent.Map.designationManager.DesignationOn(this.parent, DesignationDefOf.Flick) == null && this.parent.Map.designationManager.DesignationOn(this.parent, DesignationDefOf.Deconstruct) == null;
			}
		}

		// Token: 0x06010848 RID: 67656 RVA: 0x004C310A File Offset: 0x004C130A
		public bool ShouldBeLitNow()
		{
			return this.HasFuel;
		}

		// Token: 0x06010849 RID: 67657 RVA: 0x004C3112 File Offset: 0x004C1312
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			this.allowAutoRefuel = this.Props.initialAllowAutoRefuel;
			this.fuel = this.Props.fuelCapacity * this.Props.initialFuelPercent;
		}

		// Token: 0x0601084A RID: 67658 RVA: 0x004C3149 File Offset: 0x004C1349
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			this.flickComp = this.parent.GetComp<CompFlickable>();
			this.explosiveComp = this.parent.GetComp<CompExplosive>();
		}

		// Token: 0x0601084B RID: 67659 RVA: 0x004C3170 File Offset: 0x004C1370
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<float>(ref this.fuel, "fuel", 0f, false);
			Scribe_Values.Look<float>(ref this.configuredTargetFuelLevel, "configuredTargetFuelLevel", -1f, false);
			Scribe_Values.Look<bool>(ref this.allowAutoRefuel, "allowAutoRefuel", false, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit && !this.Props.showAllowAutoRefuelToggle)
			{
				this.allowAutoRefuel = this.Props.initialAllowAutoRefuel;
			}
		}

		// Token: 0x0601084C RID: 67660 RVA: 0x004C31E8 File Offset: 0x004C13E8
		public override void PostDraw()
		{
			base.PostDraw();
			if (!this.allowAutoRefuel)
			{
				this.parent.Map.overlayDrawer.DrawOverlay(this.parent, OverlayTypes.ForbiddenRefuel);
			}
			else if (!this.HasFuel && this.Props.drawOutOfFuelOverlay)
			{
				this.parent.Map.overlayDrawer.DrawOverlay(this.parent, OverlayTypes.OutOfFuel);
			}
			if (this.Props.drawFuelGaugeInMap)
			{
				GenDraw.FillableBarRequest r = default(GenDraw.FillableBarRequest);
				r.center = this.parent.DrawPos + Vector3.up * 0.1f;
				r.size = CompRefuelable.FuelBarSize;
				r.fillPercent = this.FuelPercentOfMax;
				r.filledMat = CompRefuelable.FuelBarFilledMat;
				r.unfilledMat = CompRefuelable.FuelBarUnfilledMat;
				r.margin = 0.15f;
				Rot4 rotation = this.parent.Rotation;
				rotation.Rotate(RotationDirection.Clockwise);
				r.rotation = rotation;
				GenDraw.DrawFillableBar(r);
			}
		}

		// Token: 0x0601084D RID: 67661 RVA: 0x004C32F8 File Offset: 0x004C14F8
		public override void PostDestroy(DestroyMode mode, Map previousMap)
		{
			base.PostDestroy(mode, previousMap);
			if (this.Props.fuelIsMortarBarrel && Find.Storyteller.difficulty.classicMortars)
			{
				return;
			}
			if (mode != DestroyMode.Vanish && previousMap != null && this.Props.fuelFilter.AllowedDefCount == 1 && this.Props.initialFuelPercent == 0f)
			{
				ThingDef thingDef = this.Props.fuelFilter.AllowedThingDefs.First<ThingDef>();
				int i = Mathf.FloorToInt(1f * this.fuel);
				while (i > 0)
				{
					Thing thing = ThingMaker.MakeThing(thingDef, null);
					thing.stackCount = Mathf.Min(i, thingDef.stackLimit);
					i -= thing.stackCount;
					GenPlace.TryPlaceThing(thing, this.parent.Position, previousMap, ThingPlaceMode.Near, null, null, null, 1);
				}
			}
		}

		// Token: 0x0601084E RID: 67662 RVA: 0x004C33D4 File Offset: 0x004C15D4
		public override string CompInspectStringExtra()
		{
			if (this.Props.fuelIsMortarBarrel && Find.Storyteller.difficulty.classicMortars)
			{
				return string.Empty;
			}
			string text = base.CompInspectStringExtra();
			if (text == null)
			{
				text = string.Empty;
			}
			else
			{
				text += "\n";
			}
			text = string.Concat(new string[]
			{
				text,
				this.Props.FuelLabel,
				": ",
				this.fuel.ToStringDecimalIfSmall(),
				" / ",
				this.Props.fuelCapacity.ToStringDecimalIfSmall()
			});
			if (!this.Props.consumeFuelOnlyWhenUsed && this.HasFuel)
			{
				int numTicks = (int)(this.fuel / this.Props.fuelConsumptionRate * 60000f);
				text = text + " (" + numTicks.ToStringTicksToPeriod(true, false, true, true, false) + ")";
			}
			if (!this.HasFuel && !this.Props.outOfFuelMessage.NullOrEmpty())
			{
				string arg = (this.parent.def.building != null && this.parent.def.building.IsTurret) ? ("CannotShoot".Translate() + ": " + this.Props.outOfFuelMessage).Resolve() : this.Props.outOfFuelMessage;
				text += string.Format("\n{0} ({1}x {2})", arg, this.GetFuelCountToFullyRefuel(), this.Props.fuelFilter.AnyAllowedDef.label);
			}
			if (this.Props.targetFuelLevelConfigurable)
			{
				text += "\n" + "ConfiguredTargetFuelLevel".Translate(this.TargetFuelLevel.ToStringDecimalIfSmall());
			}
			return text;
		}

		// Token: 0x0601084F RID: 67663 RVA: 0x004C35B0 File Offset: 0x004C17B0
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			if (this.parent.def.building != null && this.parent.def.building.IsTurret)
			{
				TaggedString taggedString = "RearmCostExplanation".Translate();
				if (this.Props.factorByDifficulty)
				{
					taggedString += " (" + "RearmCostExplanationDifficulty".Translate() + ")";
				}
				taggedString += ".";
				yield return new StatDrawEntry(StatCategoryDefOf.Building, "RearmCost".Translate(), GenLabel.ThingLabel(this.Props.fuelFilter.AnyAllowedDef, null, this.GetFuelCountToFullyRefuel()).CapitalizeFirst(), taggedString, 3171, null, null, false, false);
			}
			yield break;
		}

		// Token: 0x06010850 RID: 67664 RVA: 0x004C35C0 File Offset: 0x004C17C0
		public override void CompTick()
		{
			base.CompTick();
			CompPowerTrader comp = this.parent.GetComp<CompPowerTrader>();
			if (!this.Props.consumeFuelOnlyWhenUsed && (this.flickComp == null || this.flickComp.SwitchIsOn) && (!this.Props.consumeFuelOnlyWhenPowered || (comp != null && comp.PowerOn)) && !this.Props.externalTicking)
			{
				this.ConsumeFuel(this.ConsumptionRatePerTick);
			}
			if (this.Props.fuelConsumptionPerTickInRain > 0f && this.parent.Spawned && this.parent.Map.weatherManager.RainRate > 0.4f && !this.parent.Map.roofGrid.Roofed(this.parent.Position) && !this.Props.externalTicking)
			{
				this.ConsumeFuel(this.Props.fuelConsumptionPerTickInRain);
			}
		}

		// Token: 0x06010851 RID: 67665 RVA: 0x004C36B0 File Offset: 0x004C18B0
		public void ConsumeFuel(float amount)
		{
			if (this.Props.fuelIsMortarBarrel && Find.Storyteller.difficulty.classicMortars)
			{
				return;
			}
			if (this.fuel <= 0f)
			{
				return;
			}
			this.fuel -= amount;
			if (this.fuel <= 0f)
			{
				this.fuel = 0f;
				this.Notify_RanOutOfFuel();
			}
		}

		// Token: 0x06010852 RID: 67666 RVA: 0x004C3716 File Offset: 0x004C1916
		private void Notify_RanOutOfFuel()
		{
			if (this.Props.destroyOnNoFuel)
			{
				this.parent.Destroy(DestroyMode.Vanish);
			}
			this.parent.BroadcastCompSignal("RanOutOfFuel");
		}

		// Token: 0x06010853 RID: 67667 RVA: 0x004C3744 File Offset: 0x004C1944
		public void Refuel(List<Thing> fuelThings)
		{
			if (this.Props.atomicFueling)
			{
				if (fuelThings.Sum((Thing t) => t.stackCount) < this.GetFuelCountToFullyRefuel())
				{
					Log.ErrorOnce("Error refueling; not enough fuel available for proper atomic refuel", 19586442);
					return;
				}
			}
			int num = this.GetFuelCountToFullyRefuel();
			while (num > 0 && fuelThings.Count > 0)
			{
				Thing thing = fuelThings.Pop<Thing>();
				int num2 = Mathf.Min(num, thing.stackCount);
				this.Refuel((float)num2);
				thing.SplitOff(num2).Destroy(DestroyMode.Vanish);
				num -= num2;
			}
		}

		// Token: 0x06010854 RID: 67668 RVA: 0x004C37E0 File Offset: 0x004C19E0
		public void Refuel(float amount)
		{
			this.fuel += amount * this.Props.FuelMultiplierCurrentDifficulty;
			if (this.fuel > this.Props.fuelCapacity)
			{
				this.fuel = this.Props.fuelCapacity;
			}
			this.parent.BroadcastCompSignal("Refueled");
		}

		// Token: 0x06010855 RID: 67669 RVA: 0x004C383C File Offset: 0x004C1A3C
		public AcceptanceReport CanEjectFuel()
		{
			if (!this.Props.canEjectFuel)
			{
				return false;
			}
			CompExplosive compExplosive = this.explosiveComp;
			if (compExplosive != null && compExplosive.wickStarted)
			{
				return "AboutToExplode".Translate();
			}
			if (this.Fuel == 0f)
			{
				return "RefuelableNoFuelToEject".Translate();
			}
			return true;
		}

		// Token: 0x06010856 RID: 67670 RVA: 0x004C38A4 File Offset: 0x004C1AA4
		public void EjectFuel()
		{
			ThingDef thingDef = this.Props.fuelFilter.AllowedThingDefs.First<ThingDef>();
			int i = Mathf.FloorToInt(this.fuel);
			while (i > 0)
			{
				Thing thing = ThingMaker.MakeThing(thingDef, null);
				thing.stackCount = Mathf.Min(i, thingDef.stackLimit);
				i -= thing.stackCount;
				GenPlace.TryPlaceThing(thing, this.parent.Position, this.parent.Map, ThingPlaceMode.Near, null, null, null, 1);
				thing.SetForbidden(true, true);
			}
			this.fuel = 0f;
			this.Notify_RanOutOfFuel();
		}

		// Token: 0x06010857 RID: 67671 RVA: 0x004C393F File Offset: 0x004C1B3F
		public void Notify_UsedThisTick()
		{
			this.ConsumeFuel(this.ConsumptionRatePerTick);
		}

		// Token: 0x06010858 RID: 67672 RVA: 0x004C3950 File Offset: 0x004C1B50
		public int GetFuelCountToFullyRefuel()
		{
			if (this.Props.atomicFueling)
			{
				return Mathf.CeilToInt(this.Props.fuelCapacity / this.Props.FuelMultiplierCurrentDifficulty);
			}
			return Mathf.Max(Mathf.CeilToInt((this.TargetFuelLevel - this.fuel) / this.Props.FuelMultiplierCurrentDifficulty), 1);
		}

		// Token: 0x06010859 RID: 67673 RVA: 0x004C39AB File Offset: 0x004C1BAB
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (this.Props.fuelIsMortarBarrel && Find.Storyteller.difficulty.classicMortars)
			{
				yield break;
			}
			if (!this.Props.hideGizmosIfNotPlayerFaction || this.parent.Faction == Faction.OfPlayer)
			{
				if (Find.Selector.SelectedObjects.Count == 1)
				{
					yield return new Gizmo_SetFuelLevel(this);
				}
				else
				{
					if (this.Props.targetFuelLevelConfigurable)
					{
						yield return new Command_SetTargetFuelLevel
						{
							refuelable = this,
							defaultLabel = "CommandSetTargetFuelLevel".Translate(),
							defaultDesc = "CommandSetTargetFuelLevelDesc".Translate(),
							icon = CompRefuelable.SetTargetFuelLevelCommand
						};
					}
					if (this.Props.showAllowAutoRefuelToggle)
					{
						string str = this.allowAutoRefuel ? "On".Translate() : "Off".Translate();
						yield return new Command_Toggle
						{
							isActive = (() => this.allowAutoRefuel),
							toggleAction = delegate()
							{
								this.allowAutoRefuel = !this.allowAutoRefuel;
							},
							defaultLabel = "CommandToggleAllowAutoRefuel".Translate(),
							defaultDesc = "CommandToggleAllowAutoRefuelDescMult".Translate(str.UncapitalizeFirst().Named("ONOFF")),
							icon = (this.allowAutoRefuel ? TexCommand.ForbidOn : TexCommand.ForbidOff),
							Order = 20f,
							hotKey = KeyBindingDefOf.Command_ItemForbid
						};
					}
				}
			}
			if (DebugSettings.ShowDevGizmos)
			{
				yield return new Command_Action
				{
					defaultLabel = "DEV: Set fuel to 0",
					action = delegate()
					{
						this.fuel = 0f;
						this.parent.BroadcastCompSignal("Refueled");
					}
				};
				yield return new Command_Action
				{
					defaultLabel = "DEV: Set fuel to 0.1",
					action = delegate()
					{
						this.fuel = 0.1f;
						this.parent.BroadcastCompSignal("Refueled");
					}
				};
				yield return new Command_Action
				{
					defaultLabel = "DEV: Fuel -20%",
					action = delegate()
					{
						this.ConsumeFuel(this.Props.fuelCapacity * 0.2f);
					}
				};
				yield return new Command_Action
				{
					defaultLabel = "DEV: Set fuel to max",
					action = delegate()
					{
						this.fuel = this.Props.fuelCapacity;
						this.parent.BroadcastCompSignal("Refueled");
					}
				};
			}
			yield break;
		}

		// Token: 0x0400AC52 RID: 44114
		private float fuel;

		// Token: 0x0400AC53 RID: 44115
		private float configuredTargetFuelLevel = -1f;

		// Token: 0x0400AC54 RID: 44116
		public bool allowAutoRefuel = true;

		// Token: 0x0400AC55 RID: 44117
		private CompFlickable flickComp;

		// Token: 0x0400AC56 RID: 44118
		private CompExplosive explosiveComp;

		// Token: 0x0400AC57 RID: 44119
		public const string RefueledSignal = "Refueled";

		// Token: 0x0400AC58 RID: 44120
		public const string RanOutOfFuelSignal = "RanOutOfFuel";

		// Token: 0x0400AC59 RID: 44121
		private static readonly Texture2D SetTargetFuelLevelCommand = ContentFinder<Texture2D>.Get("UI/Commands/SetTargetFuelLevel", true);

		// Token: 0x0400AC5A RID: 44122
		private static readonly Vector2 FuelBarSize = new Vector2(1f, 0.2f);

		// Token: 0x0400AC5B RID: 44123
		private static readonly Material FuelBarFilledMat = SolidColorMaterials.SimpleSolidColorMaterial(new Color(0.6f, 0.56f, 0.13f), false);

		// Token: 0x0400AC5C RID: 44124
		private static readonly Material FuelBarUnfilledMat = SolidColorMaterials.SimpleSolidColorMaterial(new Color(0.3f, 0.3f, 0.3f), false);
	}
}
