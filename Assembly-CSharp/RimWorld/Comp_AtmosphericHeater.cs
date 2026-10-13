using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02002ED4 RID: 11988
	public class Comp_AtmosphericHeater : CompTempControl, IThingGlower
	{
		// Token: 0x17002BA5 RID: 11173
		// (get) Token: 0x06010ECC RID: 69324 RVA: 0x004DF13C File Offset: 0x004DD33C
		private GameCondition_UnnaturalHeat Condition
		{
			get
			{
				GameCondition_UnnaturalHeat gameCondition_UnnaturalHeat = this.parent.Map.gameConditionManager.GetActiveCondition(GameConditionDefOf.UnnaturalHeat) as GameCondition_UnnaturalHeat;
				if (gameCondition_UnnaturalHeat == null)
				{
					gameCondition_UnnaturalHeat = (GameCondition_UnnaturalHeat)GameConditionMaker.MakeCondition(GameConditionDefOf.UnnaturalHeat, -1);
					this.parent.Map.GameConditionManager.RegisterCondition(gameCondition_UnnaturalHeat);
					gameCondition_UnnaturalHeat.Permanent = true;
				}
				return gameCondition_UnnaturalHeat;
			}
		}

		// Token: 0x17002BA6 RID: 11174
		// (get) Token: 0x06010ECD RID: 69325 RVA: 0x004DF19B File Offset: 0x004DD39B
		// (set) Token: 0x06010ECE RID: 69326 RVA: 0x004DF1A8 File Offset: 0x004DD3A8
		public override float TargetTemperature
		{
			get
			{
				return this.Condition.heaterTargetTemp;
			}
			set
			{
				this.Condition.heaterTargetTemp = value;
			}
		}

		// Token: 0x17002BA7 RID: 11175
		// (get) Token: 0x06010ECF RID: 69327 RVA: 0x004DF1B6 File Offset: 0x004DD3B6
		private bool Powered
		{
			get
			{
				return this.powerTraderComp.PowerOn;
			}
		}

		// Token: 0x17002BA8 RID: 11176
		// (get) Token: 0x06010ED0 RID: 69328 RVA: 0x004DF1C3 File Offset: 0x004DD3C3
		public int TempOffset
		{
			get
			{
				if (!this.Powered || !this.refuelableComp.HasFuel || this.TargetTemperature <= this.parent.Map.mapTemperature.OutdoorTemp)
				{
					return 0;
				}
				return 10;
			}
		}

		// Token: 0x17002BA9 RID: 11177
		// (get) Token: 0x06010ED1 RID: 69329 RVA: 0x004DF1FB File Offset: 0x004DD3FB
		private bool Working
		{
			get
			{
				return this.Powered && this.refuelableComp.HasFuel && this.TargetTemperature > this.parent.Map.mapTemperature.OutdoorTemp - 1f;
			}
		}

		// Token: 0x06010ED2 RID: 69330 RVA: 0x004DF237 File Offset: 0x004DD437
		public bool ShouldBeLitNow()
		{
			return this.Working;
		}

		// Token: 0x06010ED3 RID: 69331 RVA: 0x004DF240 File Offset: 0x004DD440
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			if (!ModLister.CheckAnomaly("Atmospheric heater"))
			{
				this.parent.Destroy(DestroyMode.Vanish);
				return;
			}
			base.PostSpawnSetup(respawningAfterLoad);
			this.powerTraderComp = this.parent.GetComp<CompPowerTrader>();
			this.heatPusherComp = this.parent.GetComp<CompHeatPusher>();
			this.refuelableComp = this.parent.GetComp<CompRefuelable>();
		}

		// Token: 0x06010ED4 RID: 69332 RVA: 0x004DF2A0 File Offset: 0x004DD4A0
		public override void CompTick()
		{
			if (this.Working)
			{
				this.powerTraderComp.PowerOutput = -this.powerTraderComp.Props.PowerConsumption;
				this.refuelableComp.Notify_UsedThisTick();
				this.heatPusherComp.enabled = true;
			}
			else
			{
				this.powerTraderComp.PowerOutput = -this.powerTraderComp.Props.PowerConsumption * base.Props.lowPowerConsumptionFactor;
				this.heatPusherComp.enabled = false;
			}
			int num = this.Working ? 1 : 0;
			int? overrideGraphicIndex = this.parent.overrideGraphicIndex;
			int num2 = num;
			if (!(overrideGraphicIndex.GetValueOrDefault() == num2 & overrideGraphicIndex != null))
			{
				this.parent.overrideGraphicIndex = new int?(num);
				this.parent.DirtyMapMesh(this.parent.Map);
				CompGlower compGlower = this.parent.TryGetComp<CompGlower>();
				if (compGlower != null)
				{
					compGlower.UpdateLit(this.parent.Map);
				}
			}
			this.operatingAtHighPower = this.Working;
		}

		// Token: 0x06010ED5 RID: 69333 RVA: 0x004DF3A1 File Offset: 0x004DD5A1
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			float num = base.RoundedToCurrentTempModeOffset(-10f);
			yield return new Comp_AtmosphericHeater.Command_GroupedTempChange(this, num)
			{
				defaultLabel = num.ToStringTemperatureOffset("F0"),
				defaultDesc = "CommandLowerTempDesc".Translate(),
				hotKey = KeyBindingDefOf.Misc5,
				icon = ContentFinder<Texture2D>.Get("UI/Commands/TempLower", true)
			};
			float num2 = base.RoundedToCurrentTempModeOffset(-1f);
			yield return new Comp_AtmosphericHeater.Command_GroupedTempChange(this, num2)
			{
				defaultLabel = num2.ToStringTemperatureOffset("F0"),
				defaultDesc = "CommandLowerTempDesc".Translate(),
				hotKey = KeyBindingDefOf.Misc4,
				icon = ContentFinder<Texture2D>.Get("UI/Commands/TempLower", true)
			};
			yield return new Command_Action
			{
				action = delegate()
				{
					this.TargetTemperature = 21f;
					SoundDefOf.Tick_Tiny.PlayOneShotOnCamera(null);
					base.ThrowCurrentTemperatureText();
				},
				defaultLabel = "CommandResetTemp".Translate(),
				defaultDesc = "CommandResetTempDesc".Translate(),
				hotKey = KeyBindingDefOf.Misc1,
				icon = ContentFinder<Texture2D>.Get("UI/Commands/TempReset", true)
			};
			float num3 = base.RoundedToCurrentTempModeOffset(1f);
			yield return new Comp_AtmosphericHeater.Command_GroupedTempChange(this, num3)
			{
				defaultLabel = "+" + num3.ToStringTemperatureOffset("F0"),
				defaultDesc = "CommandRaiseTempDesc".Translate(),
				hotKey = KeyBindingDefOf.Misc2,
				icon = ContentFinder<Texture2D>.Get("UI/Commands/TempRaise", true)
			};
			float num4 = base.RoundedToCurrentTempModeOffset(10f);
			yield return new Comp_AtmosphericHeater.Command_GroupedTempChange(this, num4)
			{
				defaultLabel = "+" + num4.ToStringTemperatureOffset("F0"),
				defaultDesc = "CommandRaiseTempDesc".Translate(),
				hotKey = KeyBindingDefOf.Misc3,
				icon = ContentFinder<Texture2D>.Get("UI/Commands/TempRaise", true)
			};
			yield break;
		}

		// Token: 0x0400B00F RID: 45071
		private const int DisabledTextureIndex = 0;

		// Token: 0x0400B010 RID: 45072
		private const int EnabledTextureIndex = 1;

		// Token: 0x0400B011 RID: 45073
		private const int MaxOffset = 10;

		// Token: 0x0400B012 RID: 45074
		private CompPowerTrader powerTraderComp;

		// Token: 0x0400B013 RID: 45075
		private CompHeatPusher heatPusherComp;

		// Token: 0x0400B014 RID: 45076
		private CompRefuelable refuelableComp;

		// Token: 0x02002ED5 RID: 11989
		private class Command_GroupedTempChange : Command_Action
		{
			// Token: 0x06010ED8 RID: 69336 RVA: 0x004DF3B9 File Offset: 0x004DD5B9
			public Command_GroupedTempChange(Comp_AtmosphericHeater heater, float offset)
			{
				this.heater = heater;
				this.offset = offset;
			}

			// Token: 0x06010ED9 RID: 69337 RVA: 0x000026EA File Offset: 0x000008EA
			public override void ProcessInput(Event ev)
			{
			}

			// Token: 0x06010EDA RID: 69338 RVA: 0x004DF3CF File Offset: 0x004DD5CF
			public override void ProcessGroupInput(Event ev, List<Gizmo> group)
			{
				this.heater.InterfaceChangeTargetTemperature(this.offset);
			}

			// Token: 0x0400B015 RID: 45077
			private Comp_AtmosphericHeater heater;

			// Token: 0x0400B016 RID: 45078
			private float offset;
		}
	}
}
