using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.BaseGen;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002B7F RID: 11135
	public class ThingSetMaker_RandomGeneralGoods : ThingSetMaker
	{
		// Token: 0x0600F907 RID: 63751 RVA: 0x00486644 File Offset: 0x00484844
		protected override void Generate(ThingSetMakerParams parms, List<Thing> outThings)
		{
			IntRange intRange = parms.countRange ?? new IntRange(10, 20);
			TechLevel valueOrDefault = parms.techLevel.GetValueOrDefault();
			int num = Mathf.Max(intRange.RandomInRange, 1);
			for (int i = 0; i < num; i++)
			{
				outThings.Add(this.GenerateSingle(valueOrDefault));
			}
		}

		// Token: 0x0600F908 RID: 63752 RVA: 0x004866AC File Offset: 0x004848AC
		private Thing GenerateSingle(TechLevel techLevel)
		{
			Thing thing = null;
			int num = 0;
			while (thing == null && num < 50)
			{
				IEnumerable<Pair<ThingSetMaker_RandomGeneralGoods.GoodsType, float>> goodsWeights = ThingSetMaker_RandomGeneralGoods.GoodsWeights;
				Func<Pair<ThingSetMaker_RandomGeneralGoods.GoodsType, float>, float> weightSelector = (Pair<ThingSetMaker_RandomGeneralGoods.GoodsType, float> x) => x.Second;
				switch (goodsWeights.RandomElementByWeight(weightSelector).First)
				{
				case ThingSetMaker_RandomGeneralGoods.GoodsType.Meals:
					thing = this.RandomMeals(techLevel);
					break;
				case ThingSetMaker_RandomGeneralGoods.GoodsType.RawFood:
					thing = this.RandomRawFood(techLevel);
					break;
				case ThingSetMaker_RandomGeneralGoods.GoodsType.Medicine:
					thing = this.RandomMedicine(techLevel);
					break;
				case ThingSetMaker_RandomGeneralGoods.GoodsType.Drugs:
					thing = this.RandomDrugs(techLevel);
					break;
				case ThingSetMaker_RandomGeneralGoods.GoodsType.Resources:
					thing = this.RandomResources(techLevel);
					break;
				default:
					throw new Exception();
				}
				num++;
			}
			return thing;
		}

		// Token: 0x0600F909 RID: 63753 RVA: 0x0048675C File Offset: 0x0048495C
		private Thing RandomMeals(TechLevel techLevel)
		{
			ThingDef thingDef;
			if (techLevel.IsNeolithicOrWorse())
			{
				thingDef = ThingDefOf.Pemmican;
			}
			else
			{
				float value = Rand.Value;
				if (value < 0.5f)
				{
					thingDef = ThingDefOf.MealSimple;
				}
				else if ((double)value < 0.75)
				{
					thingDef = ThingDefOf.MealFine;
				}
				else
				{
					thingDef = ThingDefOf.MealSurvivalPack;
				}
			}
			Thing thing = ThingMaker.MakeThing(thingDef, null);
			int num = Mathf.Min(thingDef.stackLimit, 10);
			thing.stackCount = Rand.RangeInclusive(num / 2, num);
			return thing;
		}

		// Token: 0x0600F90A RID: 63754 RVA: 0x004867D0 File Offset: 0x004849D0
		private Thing RandomRawFood(TechLevel techLevel)
		{
			ThingDef thingDef;
			if (!this.PossibleRawFood(techLevel).TryRandomElement(out thingDef))
			{
				return null;
			}
			Thing thing = ThingMaker.MakeThing(thingDef, null);
			int maxInclusive = Mathf.Min(thingDef.stackLimit, 75);
			thing.stackCount = Rand.RangeInclusive(1, maxInclusive);
			return thing;
		}

		// Token: 0x0600F90B RID: 63755 RVA: 0x00486814 File Offset: 0x00484A14
		private IEnumerable<ThingDef> PossibleRawFood(TechLevel techLevel)
		{
			return from x in ThingSetMakerUtility.allGeneratableItems
			where x.IsNutritionGivingIngestible && !x.IsCorpse && x.ingestible.HumanEdible && !x.HasComp(typeof(CompHatcher)) && x.techLevel <= techLevel && x.ingestible.preferability < FoodPreferability.MealAwful
			select x;
		}

		// Token: 0x0600F90C RID: 63756 RVA: 0x00486844 File Offset: 0x00484A44
		private Thing RandomMedicine(TechLevel techLevel)
		{
			ThingDef thingDef;
			if (Rand.Value < 0.75f && techLevel >= ThingDefOf.MedicineHerbal.techLevel)
			{
				thingDef = (from x in ThingSetMakerUtility.allGeneratableItems
				where x.IsMedicine && x.techLevel <= techLevel
				select x).MaxBy((ThingDef x) => x.GetStatValueAbstract(StatDefOf.MedicalPotency, null));
			}
			else if (!(from x in ThingSetMakerUtility.allGeneratableItems
			where x.IsMedicine
			select x).TryRandomElement(out thingDef))
			{
				return null;
			}
			if (techLevel.IsNeolithicOrWorse())
			{
				thingDef = ThingDefOf.MedicineHerbal;
			}
			Thing thing = ThingMaker.MakeThing(thingDef, null);
			int maxInclusive = Mathf.Min(thingDef.stackLimit, 20);
			thing.stackCount = Rand.RangeInclusive(1, maxInclusive);
			return thing;
		}

		// Token: 0x0600F90D RID: 63757 RVA: 0x0048692C File Offset: 0x00484B2C
		private Thing RandomDrugs(TechLevel techLevel)
		{
			ThingDef thingDef;
			if (!(from x in ThingSetMakerUtility.allGeneratableItems
			where x.IsDrug && x.techLevel <= techLevel
			select x).TryRandomElement(out thingDef))
			{
				return null;
			}
			Thing thing = ThingMaker.MakeThing(thingDef, null);
			int maxInclusive = Mathf.Min(thingDef.stackLimit, 25);
			thing.stackCount = Rand.RangeInclusive(1, maxInclusive);
			return thing;
		}

		// Token: 0x0600F90E RID: 63758 RVA: 0x0048698C File Offset: 0x00484B8C
		private Thing RandomResources(TechLevel techLevel)
		{
			ThingDef thingDef = BaseGenUtility.RandomCheapWallStuff(techLevel, false);
			Thing thing = ThingMaker.MakeThing(thingDef, null);
			int num = Mathf.Min(thingDef.stackLimit, 75);
			thing.stackCount = Rand.RangeInclusive(num / 2, num);
			return thing;
		}

		// Token: 0x0600F90F RID: 63759 RVA: 0x004869C5 File Offset: 0x00484BC5
		protected override IEnumerable<ThingDef> AllGeneratableThingsDebugSub(ThingSetMakerParams parms)
		{
			TechLevel techLevel = parms.techLevel.GetValueOrDefault();
			if (techLevel.IsNeolithicOrWorse())
			{
				yield return ThingDefOf.Pemmican;
			}
			else
			{
				yield return ThingDefOf.MealSimple;
				yield return ThingDefOf.MealFine;
				yield return ThingDefOf.MealSurvivalPack;
			}
			foreach (ThingDef thingDef in this.PossibleRawFood(techLevel))
			{
				yield return thingDef;
			}
			IEnumerator<ThingDef> enumerator = null;
			foreach (ThingDef thingDef2 in from x in ThingSetMakerUtility.allGeneratableItems
			where x.IsMedicine
			select x)
			{
				yield return thingDef2;
			}
			enumerator = null;
			IEnumerable<ThingDef> allGeneratableItems = ThingSetMakerUtility.allGeneratableItems;
			Func<ThingDef, bool> <>9__1;
			Func<ThingDef, bool> predicate;
			if ((predicate = <>9__1) == null)
			{
				predicate = (<>9__1 = ((ThingDef x) => x.IsDrug && x.techLevel <= techLevel));
			}
			foreach (ThingDef thingDef3 in allGeneratableItems.Where(predicate))
			{
				yield return thingDef3;
			}
			enumerator = null;
			if (techLevel.IsNeolithicOrWorse())
			{
				yield return ThingDefOf.WoodLog;
			}
			else
			{
				foreach (ThingDef thingDef4 in from d in GenStuff.AllowedStuffsFor(ThingDefOf.Wall, techLevel, false)
				where d.BaseMarketValue / d.VolumePerUnit < 5f
				select d)
				{
					yield return thingDef4;
				}
				enumerator = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x0400A3BF RID: 41919
		private static Pair<ThingSetMaker_RandomGeneralGoods.GoodsType, float>[] GoodsWeights = new Pair<ThingSetMaker_RandomGeneralGoods.GoodsType, float>[]
		{
			new Pair<ThingSetMaker_RandomGeneralGoods.GoodsType, float>(ThingSetMaker_RandomGeneralGoods.GoodsType.Meals, 1f),
			new Pair<ThingSetMaker_RandomGeneralGoods.GoodsType, float>(ThingSetMaker_RandomGeneralGoods.GoodsType.RawFood, 0.75f),
			new Pair<ThingSetMaker_RandomGeneralGoods.GoodsType, float>(ThingSetMaker_RandomGeneralGoods.GoodsType.Medicine, 0.234f),
			new Pair<ThingSetMaker_RandomGeneralGoods.GoodsType, float>(ThingSetMaker_RandomGeneralGoods.GoodsType.Drugs, 0.234f),
			new Pair<ThingSetMaker_RandomGeneralGoods.GoodsType, float>(ThingSetMaker_RandomGeneralGoods.GoodsType.Resources, 0.234f)
		};

		// Token: 0x02002B80 RID: 11136
		private enum GoodsType
		{
			// Token: 0x0400A3C1 RID: 41921
			None,
			// Token: 0x0400A3C2 RID: 41922
			Meals,
			// Token: 0x0400A3C3 RID: 41923
			RawFood,
			// Token: 0x0400A3C4 RID: 41924
			Medicine,
			// Token: 0x0400A3C5 RID: 41925
			Drugs,
			// Token: 0x0400A3C6 RID: 41926
			Resources
		}
	}
}
