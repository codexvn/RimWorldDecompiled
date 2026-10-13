using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003BDB RID: 15323
	public class QuestNode_TradeRequest_GetRequestedThing : QuestNode
	{
		// Token: 0x06014CD4 RID: 85204 RVA: 0x0063B464 File Offset: 0x00639664
		private static int RandomRequestCount(ThingDef thingDef, Map map)
		{
			Rand.PushState(Find.TickManager.TicksGame ^ thingDef.GetHashCode() ^ 876093659);
			float num = (float)QuestNode_TradeRequest_GetRequestedThing.BaseValueWantedRange.RandomInRange;
			Rand.PopState();
			num *= QuestNode_TradeRequest_GetRequestedThing.ValueWantedFactorFromWealthCurve.Evaluate(map.wealthWatcher.WealthTotal);
			return ThingUtility.RoundedResourceStackCount(Mathf.Max(1, Mathf.RoundToInt(num / thingDef.BaseMarketValue)));
		}

		// Token: 0x06014CD5 RID: 85205 RVA: 0x0063B4D4 File Offset: 0x006396D4
		private static bool TryFindRandomRequestedThingDef(Map map, out ThingDef thingDef, out int count, List<ThingDef> dontRequest)
		{
			QuestNode_TradeRequest_GetRequestedThing.<>c__DisplayClass4_0 CS$<>8__locals1 = new QuestNode_TradeRequest_GetRequestedThing.<>c__DisplayClass4_0();
			CS$<>8__locals1.map = map;
			CS$<>8__locals1.dontRequest = dontRequest;
			QuestNode_TradeRequest_GetRequestedThing.requestCountDict.Clear();
			if (ThingSetMakerUtility.allGeneratableItems.Where(new Func<ThingDef, bool>(CS$<>8__locals1.<TryFindRandomRequestedThingDef>g__GlobalValidator|0)).TryRandomElement(out thingDef))
			{
				count = QuestNode_TradeRequest_GetRequestedThing.requestCountDict[thingDef];
				return true;
			}
			count = 0;
			return false;
		}

		// Token: 0x06014CD6 RID: 85206 RVA: 0x0063B534 File Offset: 0x00639734
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			ThingDef thingDef;
			int num;
			if (QuestNode_TradeRequest_GetRequestedThing.TryFindRandomRequestedThingDef(slate.Get<Map>("map", null, false), out thingDef, out num, this.dontRequest.GetValue(slate)))
			{
				slate.Set<ThingDef>(this.storeThingAs.GetValue(slate), thingDef, false);
				slate.Set<int>(this.storeThingCountAs.GetValue(slate), num, false);
				slate.Set<float>(this.storeMarketValueAs.GetValue(slate), thingDef.GetStatValueAbstract(StatDefOf.MarketValue, null) * (float)num, false);
				slate.Set<bool>(this.storeHasQualityAs.GetValue(slate), thingDef.HasComp(typeof(CompQuality)), false);
			}
		}

		// Token: 0x06014CD7 RID: 85207 RVA: 0x0063B5D8 File Offset: 0x006397D8
		protected override bool TestRunInt(Slate slate)
		{
			ThingDef thingDef;
			int num;
			if (QuestNode_TradeRequest_GetRequestedThing.TryFindRandomRequestedThingDef(slate.Get<Map>("map", null, false), out thingDef, out num, this.dontRequest.GetValue(slate)))
			{
				slate.Set<ThingDef>(this.storeThingAs.GetValue(slate), thingDef, false);
				slate.Set<int>(this.storeThingCountAs.GetValue(slate), num, false);
				slate.Set<float>(this.storeMarketValueAs.GetValue(slate), thingDef.GetStatValueAbstract(StatDefOf.MarketValue, null) * (float)num, false);
				return true;
			}
			return false;
		}

		// Token: 0x0400E78B RID: 59275
		private static readonly IntRange BaseValueWantedRange = new IntRange(500, 2500);

		// Token: 0x0400E78C RID: 59276
		private static readonly SimpleCurve ValueWantedFactorFromWealthCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0.3f),
				true
			},
			{
				new CurvePoint(50000f, 1f),
				true
			},
			{
				new CurvePoint(300000f, 2f),
				true
			}
		};

		// Token: 0x0400E78D RID: 59277
		private static Dictionary<ThingDef, int> requestCountDict = new Dictionary<ThingDef, int>();

		// Token: 0x0400E78E RID: 59278
		[NoTranslate]
		public SlateRef<string> storeThingAs;

		// Token: 0x0400E78F RID: 59279
		[NoTranslate]
		public SlateRef<string> storeThingCountAs;

		// Token: 0x0400E790 RID: 59280
		[NoTranslate]
		public SlateRef<string> storeMarketValueAs;

		// Token: 0x0400E791 RID: 59281
		[NoTranslate]
		public SlateRef<string> storeHasQualityAs;

		// Token: 0x0400E792 RID: 59282
		public SlateRef<List<ThingDef>> dontRequest;
	}
}
