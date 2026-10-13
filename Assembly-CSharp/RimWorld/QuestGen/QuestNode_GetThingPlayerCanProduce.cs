using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003A87 RID: 14983
	public class QuestNode_GetThingPlayerCanProduce : QuestNode
	{
		// Token: 0x06014775 RID: 83829 RVA: 0x0061CBF0 File Offset: 0x0061ADF0
		public static void ResetStaticData()
		{
			QuestNode_GetThingPlayerCanProduce.allWorkTables.Clear();
			List<ThingDef> allDefsListForReading = DefDatabase<ThingDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				if (allDefsListForReading[i].IsWorkTable)
				{
					QuestNode_GetThingPlayerCanProduce.allWorkTables.Add(allDefsListForReading[i]);
				}
			}
		}

		// Token: 0x06014776 RID: 83830 RVA: 0x0061CC3D File Offset: 0x0061AE3D
		protected override bool TestRunInt(Slate slate)
		{
			return this.DoWork(slate);
		}

		// Token: 0x06014777 RID: 83831 RVA: 0x0061CC46 File Offset: 0x0061AE46
		protected override void RunInt()
		{
			this.DoWork(QuestGen.slate);
		}

		// Token: 0x06014778 RID: 83832 RVA: 0x0061CC54 File Offset: 0x0061AE54
		private bool DoWork(Slate slate)
		{
			Map map = slate.Get<Map>("map", null, false);
			if (map == null)
			{
				return false;
			}
			float x2 = slate.Get<float>("points", 0f, false);
			SimpleCurve value = this.pointsToMaxItemMarketValueCurve.GetValue(slate);
			float num = this.maxMarketValueFactor.GetValue(slate) ?? 1f;
			float maxMarketValue = value.Evaluate(x2) * num;
			SimpleCurve value2 = this.pointsToRequiredWorkCurve.GetValue(slate);
			float randomInRange = (this.workAmountRandomFactorRange.GetValue(slate) ?? FloatRange.One).RandomInRange;
			float num2 = value2.Evaluate(x2) * randomInRange;
			QuestNode_GetThingPlayerCanProduce.tmpCandidates.Clear();
			for (int i = 0; i < QuestNode_GetThingPlayerCanProduce.allWorkTables.Count; i++)
			{
				if (BuildCopyCommandUtility.FindAllowedDesignator(QuestNode_GetThingPlayerCanProduce.allWorkTables[i], true) != null)
				{
					List<RecipeDef> recipes = QuestNode_GetThingPlayerCanProduce.allWorkTables[i].AllRecipes;
					int j2;
					int j;
					for (j = 0; j < recipes.Count; j = j2 + 1)
					{
						if (recipes[j].AvailableNow && recipes[j].products.Any<ThingDefCountClass>() && !recipes[j].PotentiallyMissingIngredients(null, map).Any<ThingDef>())
						{
							using (IEnumerator<ThingDef> enumerator = (recipes[j].products[0].thingDef.MadeFromStuff ? GenStuff.AllowedStuffsFor(recipes[j].products[0].thingDef, TechLevel.Undefined, false) : Gen.YieldSingle<ThingDef>(null)).GetEnumerator())
							{
								while (enumerator.MoveNext())
								{
									ThingDef stuff = enumerator.Current;
									if (stuff == null || (map.listerThings.ThingsOfDef(stuff).Any<Thing>() && stuff.stuffProps.commonality >= this.minStuffCommonality.GetValue(slate)))
									{
										int num3 = 0;
										if (stuff != null)
										{
											List<Thing> list = map.listerThings.ThingsOfDef(stuff);
											for (int k = 0; k < list.Count; k++)
											{
												num3 += list[k].stackCount;
											}
										}
										float num4 = recipes[j].WorkAmountForStuff(stuff);
										if (num4 > 0f)
										{
											int num5 = Mathf.FloorToInt(num2 / num4);
											if (stuff != null)
											{
												IngredientCount ingredientCount = (from x in recipes[j].ingredients
												where x.filter.Allows(stuff)
												select x).MaxByWithFallback((IngredientCount x) => x.CountRequiredOfFor(stuff, recipes[j], null), null);
												num5 = Mathf.Min(num5, Mathf.FloorToInt((float)num3 / (float)ingredientCount.CountRequiredOfFor(stuff, recipes[j], null)));
											}
											if (num5 > 0)
											{
												QuestNode_GetThingPlayerCanProduce.tmpCandidates.Add(new Pair<ThingStuffPair, int>(new ThingStuffPair(recipes[j].products[0].thingDef, stuff, 1f), recipes[j].products[0].count * num5));
											}
										}
									}
								}
							}
						}
						j2 = j;
					}
				}
			}
			QuestNode_GetThingPlayerCanProduce.tmpCandidates.RemoveAll((Pair<ThingStuffPair, int> x) => x.Second <= 0);
			QuestNode_GetThingPlayerCanProduce.tmpCandidates.RemoveAll((Pair<ThingStuffPair, int> x) => StatDefOf.MarketValue.Worker.GetValueAbstract(x.First.thing, x.First.stuff) > maxMarketValue);
			if (!QuestNode_GetThingPlayerCanProduce.tmpCandidates.Any<Pair<ThingStuffPair, int>>())
			{
				return false;
			}
			Pair<ThingStuffPair, int> pair = QuestNode_GetThingPlayerCanProduce.tmpCandidates.RandomElement<Pair<ThingStuffPair, int>>();
			int num6 = Mathf.Min(Mathf.RoundToInt(maxMarketValue / StatDefOf.MarketValue.Worker.GetValueAbstract(pair.First.thing, pair.First.stuff)), pair.Second);
			float randomInRange2 = (this.productionItemCountRandomFactorRange.GetValue(slate) ?? FloatRange.One).RandomInRange;
			num6 = Mathf.RoundToInt((float)num6 * randomInRange2);
			num6 = Mathf.Max(num6, 1);
			slate.Set<ThingDef>(this.storeProductionItemDefAs.GetValue(slate), pair.First.thing, false);
			slate.Set<ThingDef>(this.storeProductionItemStuffAs.GetValue(slate), pair.First.stuff, false);
			slate.Set<int>(this.storeProductionItemCountAs.GetValue(slate), num6, false);
			string value3 = this.storeProductionItemLabelAs.GetValue(slate);
			if (!string.IsNullOrEmpty(value3))
			{
				slate.Set<string>(value3, GenLabel.ThingLabel(pair.First.thing, pair.First.stuff, num6), false);
			}
			return true;
		}

		// Token: 0x0400E28B RID: 57995
		[NoTranslate]
		public SlateRef<string> storeProductionItemDefAs;

		// Token: 0x0400E28C RID: 57996
		[NoTranslate]
		public SlateRef<string> storeProductionItemStuffAs;

		// Token: 0x0400E28D RID: 57997
		[NoTranslate]
		public SlateRef<string> storeProductionItemCountAs;

		// Token: 0x0400E28E RID: 57998
		[NoTranslate]
		public SlateRef<string> storeProductionItemLabelAs;

		// Token: 0x0400E28F RID: 57999
		public SlateRef<SimpleCurve> pointsToRequiredWorkCurve;

		// Token: 0x0400E290 RID: 58000
		public SlateRef<SimpleCurve> pointsToMaxItemMarketValueCurve;

		// Token: 0x0400E291 RID: 58001
		public SlateRef<float?> maxMarketValueFactor;

		// Token: 0x0400E292 RID: 58002
		public SlateRef<float> minStuffCommonality;

		// Token: 0x0400E293 RID: 58003
		public SlateRef<FloatRange?> workAmountRandomFactorRange;

		// Token: 0x0400E294 RID: 58004
		public SlateRef<FloatRange?> productionItemCountRandomFactorRange;

		// Token: 0x0400E295 RID: 58005
		private static List<ThingDef> allWorkTables = new List<ThingDef>();

		// Token: 0x0400E296 RID: 58006
		private static List<Pair<ThingStuffPair, int>> tmpCandidates = new List<Pair<ThingStuffPair, int>>();
	}
}
