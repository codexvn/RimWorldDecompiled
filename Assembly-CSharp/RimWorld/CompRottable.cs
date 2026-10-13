using System;
using System.Text;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002DF1 RID: 11761
	public class CompRottable : ThingComp
	{
		// Token: 0x17002A4C RID: 10828
		// (get) Token: 0x060108D1 RID: 67793 RVA: 0x004C58E2 File Offset: 0x004C3AE2
		public CompProperties_Rottable PropsRot
		{
			get
			{
				return (CompProperties_Rottable)this.props;
			}
		}

		// Token: 0x17002A4D RID: 10829
		// (get) Token: 0x060108D2 RID: 67794 RVA: 0x004C58EF File Offset: 0x004C3AEF
		public float RotProgressPct
		{
			get
			{
				return this.RotProgress / (float)this.PropsRot.TicksToRotStart;
			}
		}

		// Token: 0x17002A4E RID: 10830
		// (get) Token: 0x060108D3 RID: 67795 RVA: 0x004C5904 File Offset: 0x004C3B04
		// (set) Token: 0x060108D4 RID: 67796 RVA: 0x004C590C File Offset: 0x004C3B0C
		public float RotProgress
		{
			get
			{
				return this.rotProgressInt;
			}
			set
			{
				RotStage stage = this.Stage;
				this.rotProgressInt = value;
				if (stage != this.Stage)
				{
					this.StageChanged();
				}
			}
		}

		// Token: 0x17002A4F RID: 10831
		// (get) Token: 0x060108D5 RID: 67797 RVA: 0x004C5929 File Offset: 0x004C3B29
		public RotStage Stage
		{
			get
			{
				if (this.RotProgress < (float)this.PropsRot.TicksToRotStart)
				{
					return RotStage.Fresh;
				}
				if (this.RotProgress < (float)this.PropsRot.TicksToDessicated)
				{
					return RotStage.Rotting;
				}
				return RotStage.Dessicated;
			}
		}

		// Token: 0x17002A50 RID: 10832
		// (get) Token: 0x060108D6 RID: 67798 RVA: 0x004C5958 File Offset: 0x004C3B58
		public int TicksUntilRotAtCurrentTemp
		{
			get
			{
				float num = this.parent.AmbientTemperature;
				num = (float)Mathf.RoundToInt(num);
				return this.TicksUntilRotAtTemp(num);
			}
		}

		// Token: 0x17002A51 RID: 10833
		// (get) Token: 0x060108D7 RID: 67799 RVA: 0x004C5980 File Offset: 0x004C3B80
		public bool Active
		{
			get
			{
				if (this.PropsRot.disableIfHatcher)
				{
					CompHatcher compHatcher = this.parent.TryGetComp<CompHatcher>();
					if (compHatcher != null && !compHatcher.TemperatureDamaged)
					{
						return false;
					}
				}
				return !this.disabled;
			}
		}

		// Token: 0x060108D8 RID: 67800 RVA: 0x004C59BC File Offset: 0x004C3BBC
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<float>(ref this.rotProgressInt, "rotProg", 0f, false);
			Scribe_Values.Look<bool>(ref this.disabled, "disabled", false, false);
		}

		// Token: 0x060108D9 RID: 67801 RVA: 0x004C59EC File Offset: 0x004C3BEC
		public override void CompTickInterval(int delta)
		{
			this.TickInterval(delta);
		}

		// Token: 0x060108DA RID: 67802 RVA: 0x004C59F5 File Offset: 0x004C3BF5
		public override void CompTickRare()
		{
			this.TickInterval(250);
		}

		// Token: 0x060108DB RID: 67803 RVA: 0x004C5A04 File Offset: 0x004C3C04
		private void TickInterval(int delta)
		{
			if (!this.Active)
			{
				return;
			}
			float rotProgress = this.RotProgress;
			float num = GenTemperature.RotRateAtTemperature(this.parent.AmbientTemperature);
			this.RotProgress += num * (float)delta;
			if (this.Stage == RotStage.Rotting && this.PropsRot.rotDestroys)
			{
				if (this.parent.IsInAnyStorage() && this.parent.SpawnedOrAnyParentSpawned)
				{
					Messages.Message("MessageRottedAwayInStorage".Translate(this.parent.Label, this.parent).CapitalizeFirst(), new TargetInfo(this.parent.PositionHeld, this.parent.MapHeld, false), MessageTypeDefOf.NegativeEvent, true);
					LessonAutoActivator.TeachOpportunity(ConceptDefOf.SpoilageAndFreezers, OpportunityType.GoodToKnow);
				}
				if (this.parent.Spawned && this.parent.def.thingCategories.NotNullAndContains(ThingCategoryDefOf.MeatRaw))
				{
					int amount = 2 * this.parent.stackCount;
					GasUtility.AddGas(this.parent.Position, this.parent.Map, GasType.RotStink, amount);
				}
				this.parent.Destroy(DestroyMode.Vanish);
				return;
			}
			if (Mathf.FloorToInt(rotProgress / 60000f) != Mathf.FloorToInt(this.RotProgress / 60000f) && this.ShouldTakeRotDamage())
			{
				if (this.Stage == RotStage.Rotting && this.PropsRot.rotDamagePerDay > 0f)
				{
					this.parent.TakeDamage(new DamageInfo(DamageDefOf.Rotting, (float)GenMath.RoundRandom(this.PropsRot.rotDamagePerDay), 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true, QualityCategory.Normal, true, false));
					return;
				}
				if (this.Stage == RotStage.Dessicated && this.PropsRot.dessicatedDamagePerDay > 0f)
				{
					this.parent.TakeDamage(new DamageInfo(DamageDefOf.Rotting, (float)GenMath.RoundRandom(this.PropsRot.dessicatedDamagePerDay), 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true, QualityCategory.Normal, true, false));
				}
			}
		}

		// Token: 0x060108DC RID: 67804 RVA: 0x004C5C24 File Offset: 0x004C3E24
		private bool ShouldTakeRotDamage()
		{
			Thing thing = this.parent.ParentHolder as Thing;
			return thing == null || thing.def.category != ThingCategory.Building || !thing.def.building.preventDeteriorationInside;
		}

		// Token: 0x060108DD RID: 67805 RVA: 0x004C5C68 File Offset: 0x004C3E68
		public override void PreAbsorbStack(Thing otherStack, int count)
		{
			float t = (float)count / (float)(this.parent.stackCount + count);
			float rotProgress = ((ThingWithComps)otherStack).GetComp<CompRottable>().RotProgress;
			this.RotProgress = Mathf.Lerp(this.RotProgress, rotProgress, t);
		}

		// Token: 0x060108DE RID: 67806 RVA: 0x004C5CAB File Offset: 0x004C3EAB
		public override void PostSplitOff(Thing piece)
		{
			((ThingWithComps)piece).GetComp<CompRottable>().RotProgress = this.RotProgress;
		}

		// Token: 0x060108DF RID: 67807 RVA: 0x004C5CC3 File Offset: 0x004C3EC3
		public override void PostIngested(Pawn ingester)
		{
			if (this.Stage != RotStage.Fresh && FoodUtility.GetFoodPoisonChanceFactor(ingester) > 1E-45f)
			{
				FoodUtility.AddFoodPoisoningHediff(ingester, this.parent, FoodPoisonCause.Rotten);
			}
		}

		// Token: 0x060108E0 RID: 67808 RVA: 0x004C5CE8 File Offset: 0x004C3EE8
		public override string CompInspectStringExtra()
		{
			if (!this.Active)
			{
				return null;
			}
			StringBuilder stringBuilder = new StringBuilder();
			switch (this.Stage)
			{
			case RotStage.Fresh:
				stringBuilder.Append("RotStateFresh".Translate());
				break;
			case RotStage.Rotting:
				stringBuilder.Append("RotStateRotting".Translate());
				break;
			case RotStage.Dessicated:
				stringBuilder.Append("RotStateDessicated".Translate());
				break;
			}
			if ((float)this.PropsRot.TicksToRotStart - this.RotProgress > 0f)
			{
				float num = GenTemperature.RotRateAtTemperature((float)Mathf.RoundToInt(this.parent.AmbientTemperature));
				int ticksUntilRotAtCurrentTemp = this.TicksUntilRotAtCurrentTemp;
				if (num < 0.001f)
				{
					stringBuilder.Append(string.Format(" ({0})", "CurrentlyFrozen".Translate()));
				}
				else if (num < 0.999f)
				{
					stringBuilder.AppendTagged(string.Format(" ({0})", "CurrentlyRefrigerated".Translate(ticksUntilRotAtCurrentTemp.ToStringTicksToPeriod(true, false, true, true, false))));
				}
				else
				{
					stringBuilder.AppendTagged(string.Format(" ({0})", "NotRefrigerated".Translate(ticksUntilRotAtCurrentTemp.ToStringTicksToPeriod(true, false, true, true, false))));
				}
			}
			else
			{
				stringBuilder.Append(".");
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060108E1 RID: 67809 RVA: 0x004C5E58 File Offset: 0x004C4058
		public int ApproxTicksUntilRotWhenAtTempOfTile(PlanetTile tile, int ticksAbs)
		{
			float temperatureFromSeasonAtTile = GenTemperature.GetTemperatureFromSeasonAtTile(ticksAbs, tile);
			return this.TicksUntilRotAtTemp(temperatureFromSeasonAtTile);
		}

		// Token: 0x060108E2 RID: 67810 RVA: 0x004C5E74 File Offset: 0x004C4074
		public int TicksUntilRotAtTemp(float temp)
		{
			if (!this.Active)
			{
				return 72000000;
			}
			float num = GenTemperature.RotRateAtTemperature(temp);
			if (num <= 0f)
			{
				return 72000000;
			}
			float num2 = (float)this.PropsRot.TicksToRotStart - this.RotProgress;
			if (num2 <= 0f)
			{
				return 0;
			}
			return Mathf.RoundToInt(num2 / num);
		}

		// Token: 0x060108E3 RID: 67811 RVA: 0x004C5ECC File Offset: 0x004C40CC
		private void StageChanged()
		{
			Corpse corpse = this.parent as Corpse;
			if (corpse != null)
			{
				corpse.RotStageChanged();
			}
		}

		// Token: 0x060108E4 RID: 67812 RVA: 0x004C5EF0 File Offset: 0x004C40F0
		public void RotImmediately(RotStage stage = RotStage.Rotting)
		{
			if (stage == RotStage.Rotting && this.RotProgress < (float)this.PropsRot.TicksToRotStart)
			{
				this.RotProgress = (float)this.PropsRot.TicksToRotStart;
				return;
			}
			if (stage == RotStage.Dessicated && this.RotProgress < (float)this.PropsRot.TicksToDessicated)
			{
				this.RotProgress = (float)this.PropsRot.TicksToDessicated;
			}
		}

		// Token: 0x0400AC9A RID: 44186
		private float rotProgressInt;

		// Token: 0x0400AC9B RID: 44187
		public bool disabled;
	}
}
