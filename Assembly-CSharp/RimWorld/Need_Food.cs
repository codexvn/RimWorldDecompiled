using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200247C RID: 9340
	public class Need_Food : Need
	{
		// Token: 0x17001EB9 RID: 7865
		// (get) Token: 0x0600CA97 RID: 51863 RVA: 0x003B5745 File Offset: 0x003B3945
		public bool Starving
		{
			get
			{
				return this.CurCategory == HungerCategory.Starving;
			}
		}

		// Token: 0x17001EBA RID: 7866
		// (get) Token: 0x0600CA98 RID: 51864 RVA: 0x003B5750 File Offset: 0x003B3950
		public float PercentageThreshUrgentlyHungry
		{
			get
			{
				return this.pawn.RaceProps.FoodLevelPercentageWantEat * 0.4f;
			}
		}

		// Token: 0x17001EBB RID: 7867
		// (get) Token: 0x0600CA99 RID: 51865 RVA: 0x003B5768 File Offset: 0x003B3968
		public float PercentageThreshHungry
		{
			get
			{
				return this.pawn.RaceProps.FoodLevelPercentageWantEat * 0.8f;
			}
		}

		// Token: 0x17001EBC RID: 7868
		// (get) Token: 0x0600CA9A RID: 51866 RVA: 0x003B5780 File Offset: 0x003B3980
		public float NutritionBetweenHungryAndFed
		{
			get
			{
				return (1f - this.PercentageThreshHungry) * this.MaxLevel;
			}
		}

		// Token: 0x17001EBD RID: 7869
		// (get) Token: 0x0600CA9B RID: 51867 RVA: 0x003B5798 File Offset: 0x003B3998
		private CompHoldingPlatformTarget PlatformTarget
		{
			get
			{
				CompHoldingPlatformTarget result;
				if ((result = this.platformComp) == null)
				{
					result = (this.platformComp = this.pawn.TryGetComp<CompHoldingPlatformTarget>());
				}
				return result;
			}
		}

		// Token: 0x17001EBE RID: 7870
		// (get) Token: 0x0600CA9C RID: 51868 RVA: 0x003B57C3 File Offset: 0x003B39C3
		public HungerCategory CurCategory
		{
			get
			{
				if (base.CurLevelPercentage <= 0f)
				{
					return HungerCategory.Starving;
				}
				if (base.CurLevelPercentage < this.PercentageThreshUrgentlyHungry)
				{
					return HungerCategory.UrgentlyHungry;
				}
				if (base.CurLevelPercentage < this.PercentageThreshHungry)
				{
					return HungerCategory.Hungry;
				}
				return HungerCategory.Fed;
			}
		}

		// Token: 0x17001EBF RID: 7871
		// (get) Token: 0x0600CA9D RID: 51869 RVA: 0x003B57F5 File Offset: 0x003B39F5
		public float FoodFallPerTick
		{
			get
			{
				return this.FoodFallPerTickAssumingCategory(this.CurCategory, false);
			}
		}

		// Token: 0x17001EC0 RID: 7872
		// (get) Token: 0x0600CA9E RID: 51870 RVA: 0x003B5804 File Offset: 0x003B3A04
		public int TicksUntilHungryWhenFed
		{
			get
			{
				return Mathf.CeilToInt(this.NutritionBetweenHungryAndFed / this.FoodFallPerTickAssumingCategory(HungerCategory.Fed, false));
			}
		}

		// Token: 0x17001EC1 RID: 7873
		// (get) Token: 0x0600CA9F RID: 51871 RVA: 0x003B581A File Offset: 0x003B3A1A
		public int TicksUntilHungryWhenFedIgnoringMalnutrition
		{
			get
			{
				return Mathf.CeilToInt(this.NutritionBetweenHungryAndFed / this.FoodFallPerTickAssumingCategory(HungerCategory.Fed, true));
			}
		}

		// Token: 0x17001EC2 RID: 7874
		// (get) Token: 0x0600CAA0 RID: 51872 RVA: 0x003B5830 File Offset: 0x003B3A30
		public override int GUIChangeArrow
		{
			get
			{
				if (this.GainingFood())
				{
					return 1;
				}
				if (this.FoodFallPerTickAssumingCategory(HungerCategory.Hungry, false) <= 0f)
				{
					return 0;
				}
				return -1;
			}
		}

		// Token: 0x17001EC3 RID: 7875
		// (get) Token: 0x0600CAA1 RID: 51873 RVA: 0x003B584E File Offset: 0x003B3A4E
		public override float MaxLevel
		{
			get
			{
				if (Current.ProgramState != ProgramState.Playing)
				{
					return this.pawn.BodySize * this.pawn.ageTracker.CurLifeStage.foodMaxFactor;
				}
				return this.pawn.GetStatValue(StatDefOf.MaxNutrition, true, 15);
			}
		}

		// Token: 0x17001EC4 RID: 7876
		// (get) Token: 0x0600CAA2 RID: 51874 RVA: 0x003B588D File Offset: 0x003B3A8D
		public float NutritionWanted
		{
			get
			{
				return this.MaxLevel - this.CurLevel;
			}
		}

		// Token: 0x17001EC5 RID: 7877
		// (get) Token: 0x0600CAA3 RID: 51875 RVA: 0x003B589C File Offset: 0x003B3A9C
		public int TicksStarving
		{
			get
			{
				return Mathf.Max(0, Find.TickManager.TicksGame - this.lastNonStarvingTick);
			}
		}

		// Token: 0x17001EC6 RID: 7878
		// (get) Token: 0x0600CAA4 RID: 51876 RVA: 0x003B58B5 File Offset: 0x003B3AB5
		private float MalnutritionSeverityPerInterval
		{
			get
			{
				return 0.0011325f * Mathf.Lerp(0.8f, 1.2f, Rand.ValueSeeded(this.pawn.thingIDNumber ^ 2551674));
			}
		}

		// Token: 0x17001EC7 RID: 7879
		// (get) Token: 0x0600CAA5 RID: 51877 RVA: 0x003B58E2 File Offset: 0x003B3AE2
		protected override bool IsFrozen
		{
			get
			{
				if (!base.IsFrozen && !this.pawn.Deathresting)
				{
					CompHoldingPlatformTarget platformTarget = this.PlatformTarget;
					return platformTarget != null && platformTarget.CurrentlyHeldOnPlatform;
				}
				return true;
			}
		}

		// Token: 0x0600CAA6 RID: 51878 RVA: 0x003B590C File Offset: 0x003B3B0C
		public Need_Food(Pawn pawn) : base(pawn)
		{
		}

		// Token: 0x0600CAA7 RID: 51879 RVA: 0x003B5920 File Offset: 0x003B3B20
		public bool GainingFood()
		{
			Pawn_JobTracker jobs = this.pawn.jobs;
			IEatingDriver eatingDriver = ((jobs != null) ? jobs.curDriver : null) as IEatingDriver;
			if (eatingDriver != null && eatingDriver.GainingNutritionNow)
			{
				return true;
			}
			ChildcareUtility.BreastfeedFailReason? breastfeedFailReason;
			if (ModsConfig.BiotechActive && ChildcareUtility.CanSuckle(this.pawn, out breastfeedFailReason))
			{
				Pawn carriedBy = this.pawn.CarriedBy;
				JobDriver_FeedBaby jobDriver_FeedBaby = ((carriedBy != null) ? carriedBy.jobs.curDriver : null) as JobDriver_FeedBaby;
				if (jobDriver_FeedBaby != null && jobDriver_FeedBaby.Feeding)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600CAA8 RID: 51880 RVA: 0x003B59A0 File Offset: 0x003B3BA0
		public float FoodFallPerTickAssumingCategory(HungerCategory hunger, bool ignoreMalnutrition = false)
		{
			Building_Bed building_Bed = this.pawn.CurrentBed();
			float num = Need_Food.BaseHungerRate(this.pawn.ageTracker.CurLifeStage, this.pawn.def) * hunger.HungerMultiplier() * this.pawn.health.hediffSet.GetHungerRateFactor(ignoreMalnutrition ? HediffDefOf.Malnutrition : null);
			Pawn_StoryTracker story = this.pawn.story;
			float? num2;
			if (story == null)
			{
				num2 = null;
			}
			else
			{
				TraitSet traits = story.traits;
				num2 = ((traits != null) ? new float?(traits.HungerRateFactor) : null);
			}
			float num3 = num * (num2 ?? 1f) * ((building_Bed != null) ? building_Bed.GetStatValue(StatDefOf.BedHungerRateFactor, true, -1) : 1f);
			if (ModsConfig.BiotechActive)
			{
				Hediff firstHediffOfDef = this.pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Lactating, false);
				if (firstHediffOfDef != null)
				{
					HediffComp_Lactating hediffComp_Lactating = firstHediffOfDef.TryGetComp<HediffComp_Lactating>();
					if (hediffComp_Lactating != null)
					{
						num3 += hediffComp_Lactating.AddedNutritionPerDay() / 60000f;
					}
				}
			}
			if (ModsConfig.BiotechActive && this.pawn.genes != null)
			{
				int num4 = 0;
				foreach (Gene gene in this.pawn.genes.GenesListForReading)
				{
					if (!gene.Overridden)
					{
						num4 += gene.def.biostatMet;
					}
				}
				num3 *= GeneTuning.MetabolismToFoodConsumptionFactorCurve.Evaluate((float)num4);
			}
			if (ModsConfig.AnomalyActive)
			{
				CompHoldingPlatformTarget platformTarget = this.PlatformTarget;
				if (platformTarget != null && platformTarget.CurrentlyHeldOnPlatform)
				{
					num3 = 0f;
				}
			}
			return num3;
		}

		// Token: 0x0600CAA9 RID: 51881 RVA: 0x003B5B5C File Offset: 0x003B3D5C
		public override void NeedInterval()
		{
			if (!this.IsFrozen)
			{
				this.CurLevel -= this.FoodFallPerTick * 150f;
			}
			if (!this.Starving)
			{
				this.lastNonStarvingTick = Find.TickManager.TicksGame;
			}
			if (!this.IsFrozen || this.pawn.Deathresting)
			{
				if (this.Starving)
				{
					HealthUtility.AdjustSeverity(this.pawn, HediffDefOf.Malnutrition, this.MalnutritionSeverityPerInterval);
					return;
				}
				HealthUtility.AdjustSeverity(this.pawn, HediffDefOf.Malnutrition, -this.MalnutritionSeverityPerInterval);
			}
		}

		// Token: 0x0600CAAA RID: 51882 RVA: 0x003B5BF0 File Offset: 0x003B3DF0
		public override void SetInitialLevel()
		{
			StatDefOf.MaxNutrition.Worker.ClearCacheForThing(this.pawn);
			base.CurLevelPercentage = (this.pawn.RaceProps.Humanlike ? 0.8f : Rand.Range(0.5f, 0.9f));
			if (Current.ProgramState == ProgramState.Playing)
			{
				this.lastNonStarvingTick = Find.TickManager.TicksGame;
			}
		}

		// Token: 0x0600CAAB RID: 51883 RVA: 0x003B5C58 File Offset: 0x003B3E58
		public override void OnNeedRemoved()
		{
			Hediff hediff;
			if (this.pawn.health.hediffSet.TryGetHediff(HediffDefOf.Malnutrition, out hediff))
			{
				this.pawn.health.RemoveHediff(hediff);
			}
		}

		// Token: 0x0600CAAC RID: 51884 RVA: 0x003B5C94 File Offset: 0x003B3E94
		public override string GetTipString()
		{
			return string.Concat(new string[]
			{
				(base.LabelCap + ": " + base.CurLevelPercentage.ToStringPercent()).Colorize(ColoredText.TipSectionTitleColor),
				" (",
				this.CurLevel.ToString("0.##"),
				" / ",
				this.MaxLevel.ToString("0.##"),
				")\n",
				this.def.description
			});
		}

		// Token: 0x0600CAAD RID: 51885 RVA: 0x003B5D28 File Offset: 0x003B3F28
		public override void DrawOnGUI(Rect rect, int maxThresholdMarkers = 2147483647, float customMargin = -1f, bool drawArrows = true, bool doTooltip = true, Rect? rectForTooltip = null, bool drawLabel = true)
		{
			if (this.threshPercents == null)
			{
				this.threshPercents = new List<float>();
			}
			this.threshPercents.Clear();
			this.threshPercents.Add(this.PercentageThreshHungry);
			this.threshPercents.Add(this.PercentageThreshUrgentlyHungry);
			base.DrawOnGUI(rect, maxThresholdMarkers, customMargin, drawArrows, doTooltip, rectForTooltip, drawLabel);
		}

		// Token: 0x0600CAAE RID: 51886 RVA: 0x003B5D86 File Offset: 0x003B3F86
		public static float BaseHungerRate(LifeStageDef lifeStage, ThingDef pawnDef)
		{
			return lifeStage.hungerRateFactor * pawnDef.race.baseHungerRate * 2.6666667E-05f;
		}

		// Token: 0x0600CAAF RID: 51887 RVA: 0x003B5DA0 File Offset: 0x003B3FA0
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.lastNonStarvingTick, "lastNonStarvingTick", -99999, false);
		}

		// Token: 0x04008A01 RID: 35329
		public int lastNonStarvingTick = -99999;

		// Token: 0x04008A02 RID: 35330
		public const float BaseFoodFallPerTick = 2.6666667E-05f;

		// Token: 0x04008A03 RID: 35331
		private const float BaseMalnutritionSeverityPerDay = 0.453f;

		// Token: 0x04008A04 RID: 35332
		private const float BaseMalnutritionSeverityPerInterval = 0.0011325f;

		// Token: 0x04008A05 RID: 35333
		private CompHoldingPlatformTarget platformComp;
	}
}
