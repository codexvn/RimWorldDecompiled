using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002F81 RID: 12161
	public class StockGenerator_MarketValue : StockGenerator_MiscItems
	{
		// Token: 0x06011210 RID: 70160 RVA: 0x004E9C50 File Offset: 0x004E7E50
		public override bool HandlesThingDef(ThingDef thingDef)
		{
			return base.HandlesThingDef(thingDef) && thingDef.tradeTags.NotNullAndContains(this.tradeTag) && (this.weaponTag.NullOrEmpty() || thingDef.weaponTags.NotNullAndContains(this.weaponTag)) && (this.apparelTag.NullOrEmpty() || (thingDef.apparel != null && thingDef.apparel.tags.NotNullAndContains(this.apparelTag)));
		}

		// Token: 0x06011211 RID: 70161 RVA: 0x004E9CCA File Offset: 0x004E7ECA
		protected override float SelectionWeight(ThingDef thingDef)
		{
			return StockGenerator_MarketValue.SelectionWeightMarketValueCurve.Evaluate(thingDef.BaseMarketValue);
		}

		// Token: 0x0400B1B4 RID: 45492
		private static readonly SimpleCurve SelectionWeightMarketValueCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 1f),
				true
			},
			{
				new CurvePoint(500f, 0.5f),
				true
			},
			{
				new CurvePoint(1500f, 0.2f),
				true
			},
			{
				new CurvePoint(5000f, 0.1f),
				true
			}
		};

		// Token: 0x0400B1B5 RID: 45493
		[NoTranslate]
		public string tradeTag;

		// Token: 0x0400B1B6 RID: 45494
		[NoTranslate]
		public string weaponTag;

		// Token: 0x0400B1B7 RID: 45495
		[NoTranslate]
		public string apparelTag;
	}
}
