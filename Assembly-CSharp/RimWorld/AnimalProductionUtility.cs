using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020022C7 RID: 8903
	public static class AnimalProductionUtility
	{
		// Token: 0x0600C1A4 RID: 49572 RVA: 0x00381446 File Offset: 0x0037F646
		public static IEnumerable<StatDrawEntry> AnimalProductionStats(ThingDef d)
		{
			float num = AnimalProductionUtility.GestationDaysLitter(d);
			if (num > 0f)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.AnimalProductivity, "Stat_Animal_GestationTime".Translate(), "PeriodDays".Translate(num.ToString("#.##")), "Stat_Animal_GestationTimeDesc".Translate(), 10000, null, null, false, false);
			}
			IntRange lhs = AnimalProductionUtility.OffspringRange(d);
			if (lhs != IntRange.One)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.AnimalProductivity, "Stat_Animal_LitterSize".Translate(), lhs.ToString(), "Stat_Animal_LitterSizeDesc".Translate(), 9990, null, null, false, false);
			}
			float num2 = AnimalProductionUtility.DaysToAdulthood(d);
			if (num2 > 0f)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.AnimalProductivity, "Stat_Animal_GrowthTime".Translate(), "PeriodDays".Translate(num2), "Stat_Animal_GrowthTimeDesc".Translate(), 9980, null, null, false, false);
				yield return new StatDrawEntry(StatCategoryDefOf.AnimalProductivity, "Stat_Animal_MeatPerDayDuringGrowth".Translate(), AnimalProductionUtility.MeatPerDayDuringGrowth(d).ToString("#.##"), "Stat_Animal_MeatPerDayDuringGrowthDesc".Translate(), 9960, null, null, false, false);
			}
			yield return new StatDrawEntry(StatCategoryDefOf.AnimalProductivity, "Stat_Animal_AdultMeatAmount".Translate(), AnimalProductionUtility.AdultMeatAmount(d).ToString("F0"), "Stat_Animal_AdultMeatAmountDesc".Translate(), 9970, null, Gen.YieldSingle<Dialog_InfoCard.Hyperlink>(new Dialog_InfoCard.Hyperlink(d.race.meatDef, -1)), false, false);
			if (AnimalProductionUtility.Herbivore(d))
			{
				yield return new StatDrawEntry(StatCategoryDefOf.AnimalProductivity, "Stat_Animal_GrassToMaintain".Translate(), AnimalProductionUtility.GrassToMaintain(d).ToString("#.##"), "Stat_Animal_GrassToMaintainDesc".Translate(), 9950, null, null, false, false);
			}
			CompProperties_EggLayer compProperties = d.GetCompProperties<CompProperties_EggLayer>();
			if (compProperties != null)
			{
				ThingDef thingDef = compProperties.eggUnfertilizedDef ?? compProperties.eggFertilizedDef;
				if (thingDef != null)
				{
					yield return new StatDrawEntry(StatCategoryDefOf.AnimalProductivity, "Stat_Animal_EggType".Translate(), thingDef.LabelCap, "Stat_Animal_EggTypeDesc".Translate(), 9940, null, Gen.YieldSingle<Dialog_InfoCard.Hyperlink>(new Dialog_InfoCard.Hyperlink(thingDef, -1)), false, false);
				}
				yield return new StatDrawEntry(StatCategoryDefOf.AnimalProductivity, "Stat_Animal_EggsPerYear".Translate(), AnimalProductionUtility.EggsPerYear(d).ToStringByStyle(ToStringStyle.FloatMaxTwo, ToStringNumberSense.Absolute), "Stat_Animal_EggsPerYearDesc".Translate(), 9930, null, null, false, false);
				yield return new StatDrawEntry(StatCategoryDefOf.AnimalProductivity, "Stat_Animal_EggNutrition".Translate(), AnimalProductionUtility.EggNutrition(d).ToStringByStyle(ToStringStyle.FloatMaxTwo, ToStringNumberSense.Absolute), "Stat_Animal_EggNutritionDesc".Translate(), 9920, null, null, false, false);
				yield return new StatDrawEntry(StatCategoryDefOf.AnimalProductivity, "Stat_Animal_EggNutritionYearly".Translate(), AnimalProductionUtility.EggNutritionPerYear(d).ToStringByStyle(ToStringStyle.FloatMaxTwo, ToStringNumberSense.Absolute), "Stat_Animal_EggNutritionYearlyDesc".Translate(), 9910, null, null, false, false);
				yield return new StatDrawEntry(StatCategoryDefOf.AnimalProductivity, "Stat_Animal_EggMarketValue".Translate(), AnimalProductionUtility.EggMarketValue(d).ToStringMoney(null), "Stat_Animal_EggMarketValueDesc".Translate(), 9900, null, null, false, false);
				yield return new StatDrawEntry(StatCategoryDefOf.AnimalProductivity, "Stat_Animal_EggMarketValueYearly".Translate(), AnimalProductionUtility.EggMarketValuePerYear(d).ToStringMoney(null), "Stat_Animal_EggMarketValueYearlyDesc".Translate(), 9890, null, null, false, false);
			}
			CompProperties_Milkable milkable = d.GetCompProperties<CompProperties_Milkable>();
			if (milkable != null)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.AnimalProductivity, "Stat_Animal_MilkType".Translate(), milkable.milkDef.LabelCap, "Stat_Animal_MilkTypeDesc".Translate(), 9880, null, Gen.YieldSingle<Dialog_InfoCard.Hyperlink>(new Dialog_InfoCard.Hyperlink(milkable.milkDef, -1)), false, false);
				yield return new StatDrawEntry(StatCategoryDefOf.AnimalProductivity, "Stat_Animal_MilkAmount".Translate(), milkable.milkAmount.ToString(), "Stat_Animal_MilkAmountDesc".Translate(), 9870, null, null, false, false);
				yield return new StatDrawEntry(StatCategoryDefOf.AnimalProductivity, "Stat_Animal_MilkGrowthTime".Translate(), "PeriodDays".Translate(milkable.milkIntervalDays), "Stat_Animal_MilkGrowthTimeDesc".Translate(), 9860, null, null, false, false);
				yield return new StatDrawEntry(StatCategoryDefOf.AnimalProductivity, "Stat_Animal_MilkPerYear".Translate(), AnimalProductionUtility.MilkPerYear(d).ToString("F0"), "Stat_Animal_MilkPerYearDesc".Translate(), 9850, null, null, false, false);
				yield return new StatDrawEntry(StatCategoryDefOf.AnimalProductivity, "Stat_Animal_MilkValue".Translate(), AnimalProductionUtility.MilkMarketValue(d).ToStringMoney(null), "Stat_Animal_MilkValueDesc".Translate(), 9840, null, null, false, false);
				yield return new StatDrawEntry(StatCategoryDefOf.AnimalProductivity, "Stat_Animal_MilkValuePerYear".Translate(), AnimalProductionUtility.MilkMarketValuePerYear(d).ToStringMoney(null), "Stat_Animal_MilkValuePerYearDesc".Translate(), 9830, null, null, false, false);
			}
			CompProperties_Shearable shearable = d.GetCompProperties<CompProperties_Shearable>();
			if (shearable != null)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.AnimalProductivity, "Stat_Animal_WoolType".Translate(), shearable.woolDef.LabelCap, "Stat_Animal_WoolTypeDesc".Translate(), 9820, null, Gen.YieldSingle<Dialog_InfoCard.Hyperlink>(new Dialog_InfoCard.Hyperlink(shearable.woolDef, -1)), false, false);
				yield return new StatDrawEntry(StatCategoryDefOf.AnimalProductivity, "Stat_Animal_WoolAmount".Translate(), shearable.woolAmount.ToString(), "Stat_Animal_WoolAmountDesc".Translate(), 9810, null, null, false, false);
				yield return new StatDrawEntry(StatCategoryDefOf.AnimalProductivity, "Stat_Animal_WoolGrowthTime".Translate(), "PeriodDays".Translate(shearable.shearIntervalDays), "Stat_Animal_WoolGrowthTimeDesc".Translate(), 9800, null, null, false, false);
				yield return new StatDrawEntry(StatCategoryDefOf.AnimalProductivity, "Stat_Animal_WoolPerYear".Translate(), AnimalProductionUtility.WoolPerYear(d).ToString("F0"), "Stat_Animal_WoolPerYearDesc".Translate(), 9790, null, null, false, false);
				yield return new StatDrawEntry(StatCategoryDefOf.AnimalProductivity, "Stat_Animal_WoolValue".Translate(), AnimalProductionUtility.WoolMarketValue(d).ToStringMoney(null), "Stat_Animal_WoolValueDesc".Translate(), 9780, null, null, false, false);
				yield return new StatDrawEntry(StatCategoryDefOf.AnimalProductivity, "Stat_Animal_WoolValuePerYear".Translate(), AnimalProductionUtility.WoolMarketValuePerYear(d).ToStringMoney(null), "Stat_Animal_WoolValuePerYearDesc".Translate(), 9770, null, null, false, false);
			}
			yield break;
		}

		// Token: 0x0600C1A5 RID: 49573 RVA: 0x00381456 File Offset: 0x0037F656
		public static float AdultMeatAmount(ThingDef d)
		{
			return d.GetStatValueAbstract(StatDefOf.MeatAmount, null);
		}

		// Token: 0x0600C1A6 RID: 49574 RVA: 0x00381464 File Offset: 0x0037F664
		public static float AdultLeatherAmount(ThingDef d)
		{
			return d.GetStatValueAbstract(StatDefOf.LeatherAmount, null);
		}

		// Token: 0x0600C1A7 RID: 49575 RVA: 0x00381472 File Offset: 0x0037F672
		public static bool Herbivore(ThingDef d)
		{
			return (d.race.foodType & FoodTypeFlags.Plant) > FoodTypeFlags.None;
		}

		// Token: 0x0600C1A8 RID: 49576 RVA: 0x00381488 File Offset: 0x0037F688
		public static float NutritionToGestate(ThingDef d)
		{
			float num = 0f;
			LifeStageAge lifeStageAge = d.race.lifeStageAges[d.race.lifeStageAges.Count - 1];
			return num + AnimalProductionUtility.GestationDaysEach(d) * lifeStageAge.def.hungerRateFactor * d.race.baseHungerRate;
		}

		// Token: 0x0600C1A9 RID: 49577 RVA: 0x003814DC File Offset: 0x0037F6DC
		public static float NutritionToAdulthood(ThingDef d)
		{
			float num = 0f;
			num += AnimalProductionUtility.NutritionToGestate(d);
			for (int i = 1; i < d.race.lifeStageAges.Count; i++)
			{
				LifeStageAge lifeStageAge = d.race.lifeStageAges[i];
				float num2 = (lifeStageAge.minAge - d.race.lifeStageAges[i - 1].minAge) * 60f;
				num += num2 * lifeStageAge.def.hungerRateFactor * d.race.baseHungerRate;
			}
			return num;
		}

		// Token: 0x0600C1AA RID: 49578 RVA: 0x00381568 File Offset: 0x0037F768
		public static float GestationDaysEach(ThingDef d)
		{
			if (d.HasComp(typeof(CompEggLayer)))
			{
				CompProperties_EggLayer compProperties = d.GetCompProperties<CompProperties_EggLayer>();
				return compProperties.eggLayIntervalDays / compProperties.eggCountRange.Average;
			}
			return d.race.gestationPeriodDays / ((d.race.litterSizeCurve != null) ? Rand.ByCurveAverage(d.race.litterSizeCurve) : 1f);
		}

		// Token: 0x0600C1AB RID: 49579 RVA: 0x003815D1 File Offset: 0x0037F7D1
		public static float GestationDaysLitter(ThingDef d)
		{
			if (d.HasComp(typeof(CompEggLayer)))
			{
				return d.GetCompProperties<CompProperties_EggLayer>().eggLayIntervalDays;
			}
			return d.race.gestationPeriodDays;
		}

		// Token: 0x0600C1AC RID: 49580 RVA: 0x003815FC File Offset: 0x0037F7FC
		public static IntRange OffspringRange(ThingDef d)
		{
			CompProperties_EggLayer compProperties = d.GetCompProperties<CompProperties_EggLayer>();
			if (compProperties != null)
			{
				return compProperties.eggCountRange;
			}
			if (d.race.litterSizeCurve != null)
			{
				int min = Mathf.Max(Mathf.RoundToInt(d.race.litterSizeCurve.First<CurvePoint>().x), 1);
				int max = Mathf.Max(Mathf.RoundToInt(d.race.litterSizeCurve.Last<CurvePoint>().x), 1);
				return new IntRange(min, max);
			}
			return new IntRange(1, 1);
		}

		// Token: 0x0600C1AD RID: 49581 RVA: 0x0038167C File Offset: 0x0037F87C
		public static float GrassNutritionPerDay()
		{
			ThingDef plant_Grass = ThingDefOf.Plant_Grass;
			return plant_Grass.GetStatValueAbstract(StatDefOf.Nutrition, null) / (plant_Grass.plant.growDays / 0.5f);
		}

		// Token: 0x0600C1AE RID: 49582 RVA: 0x003816AD File Offset: 0x0037F8AD
		public static float GrassToMaintain(ThingDef d)
		{
			if (!AnimalProductionUtility.Herbivore(d))
			{
				return -1f;
			}
			return 2.6666667E-05f * d.race.baseHungerRate * 60000f / AnimalProductionUtility.GrassNutritionPerDay();
		}

		// Token: 0x0600C1AF RID: 49583 RVA: 0x003816DC File Offset: 0x0037F8DC
		public static float EggsPerYear(ThingDef d)
		{
			CompProperties_EggLayer compProperties = d.GetCompProperties<CompProperties_EggLayer>();
			if (compProperties == null)
			{
				return 0f;
			}
			return 60f / compProperties.eggLayIntervalDays * compProperties.eggCountRange.Average;
		}

		// Token: 0x0600C1B0 RID: 49584 RVA: 0x00381711 File Offset: 0x0037F911
		public static float EggNutritionPerYear(ThingDef d)
		{
			return AnimalProductionUtility.EggsPerYear(d) * AnimalProductionUtility.EggNutrition(d);
		}

		// Token: 0x0600C1B1 RID: 49585 RVA: 0x00381720 File Offset: 0x0037F920
		public static float EggNutrition(ThingDef d)
		{
			CompProperties_EggLayer compProperties = d.GetCompProperties<CompProperties_EggLayer>();
			if (compProperties == null)
			{
				return -1f;
			}
			return (compProperties.eggUnfertilizedDef ?? compProperties.eggFertilizedDef).GetStatValueAbstract(StatDefOf.Nutrition, null);
		}

		// Token: 0x0600C1B2 RID: 49586 RVA: 0x00381758 File Offset: 0x0037F958
		public static float EggMarketValue(ThingDef d)
		{
			CompProperties_EggLayer compProperties = d.GetCompProperties<CompProperties_EggLayer>();
			if (compProperties == null)
			{
				return -1f;
			}
			return (compProperties.eggUnfertilizedDef ?? compProperties.eggFertilizedDef).BaseMarketValue;
		}

		// Token: 0x0600C1B3 RID: 49587 RVA: 0x0038178A File Offset: 0x0037F98A
		public static float EggMarketValuePerYear(ThingDef d)
		{
			return AnimalProductionUtility.EggsPerYear(d) * AnimalProductionUtility.EggMarketValue(d);
		}

		// Token: 0x0600C1B4 RID: 49588 RVA: 0x00381799 File Offset: 0x0037F999
		public static float DaysToAdulthood(ThingDef d)
		{
			return d.race.lifeStageAges.Last<LifeStageAge>().minAge * 60f;
		}

		// Token: 0x0600C1B5 RID: 49589 RVA: 0x003817B8 File Offset: 0x0037F9B8
		public static float MeatPerDayDuringGrowth(ThingDef d)
		{
			float num = AnimalProductionUtility.AdultMeatAmount(d);
			float num2 = AnimalProductionUtility.DaysToAdulthood(d);
			return num / num2;
		}

		// Token: 0x0600C1B6 RID: 49590 RVA: 0x003817D4 File Offset: 0x0037F9D4
		public static float MilkPerYear(ThingDef d)
		{
			CompProperties_Milkable compProperties = d.GetCompProperties<CompProperties_Milkable>();
			if (compProperties == null)
			{
				return 0f;
			}
			return 60f / (float)compProperties.milkIntervalDays * (float)compProperties.milkAmount;
		}

		// Token: 0x0600C1B7 RID: 49591 RVA: 0x00381808 File Offset: 0x0037FA08
		public static float MilkMarketValue(ThingDef d)
		{
			CompProperties_Milkable compProperties = d.GetCompProperties<CompProperties_Milkable>();
			if (compProperties == null)
			{
				return -1f;
			}
			return compProperties.milkDef.BaseMarketValue;
		}

		// Token: 0x0600C1B8 RID: 49592 RVA: 0x00381830 File Offset: 0x0037FA30
		public static float MilkMarketValuePerYear(ThingDef d)
		{
			return AnimalProductionUtility.MilkPerYear(d) * AnimalProductionUtility.MilkMarketValue(d);
		}

		// Token: 0x0600C1B9 RID: 49593 RVA: 0x00381840 File Offset: 0x0037FA40
		public static float MilkNutrition(ThingDef d)
		{
			CompProperties_Milkable compProperties = d.GetCompProperties<CompProperties_Milkable>();
			if (compProperties == null)
			{
				return 0f;
			}
			if (!compProperties.milkDef.IsIngestible)
			{
				return 0f;
			}
			return compProperties.milkDef.ingestible.CachedNutrition;
		}

		// Token: 0x0600C1BA RID: 49594 RVA: 0x00381880 File Offset: 0x0037FA80
		public static float MilkNutritionPerYear(ThingDef d)
		{
			return AnimalProductionUtility.MilkPerYear(d) * AnimalProductionUtility.MilkNutrition(d);
		}

		// Token: 0x0600C1BB RID: 49595 RVA: 0x00381890 File Offset: 0x0037FA90
		public static float WoolPerYear(ThingDef d)
		{
			CompProperties_Shearable compProperties = d.GetCompProperties<CompProperties_Shearable>();
			if (compProperties == null)
			{
				return 0f;
			}
			return 60f / (float)compProperties.shearIntervalDays * (float)compProperties.woolAmount;
		}

		// Token: 0x0600C1BC RID: 49596 RVA: 0x003818C4 File Offset: 0x0037FAC4
		public static float WoolMarketValue(ThingDef d)
		{
			CompProperties_Shearable compProperties = d.GetCompProperties<CompProperties_Shearable>();
			if (compProperties == null)
			{
				return 0f;
			}
			return compProperties.woolDef.BaseMarketValue;
		}

		// Token: 0x0600C1BD RID: 49597 RVA: 0x003818EC File Offset: 0x0037FAEC
		public static float WoolMarketValuePerYear(ThingDef d)
		{
			return AnimalProductionUtility.WoolPerYear(d) * AnimalProductionUtility.WoolMarketValue(d);
		}
	}
}
