using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B0A RID: 6922
	public struct ThingSetMakerParams
	{
		// Token: 0x04006A8A RID: 27274
		public TechLevel? techLevel;

		// Token: 0x04006A8B RID: 27275
		public IntRange? countRange;

		// Token: 0x04006A8C RID: 27276
		public ThingFilter filter;

		// Token: 0x04006A8D RID: 27277
		public Predicate<ThingDef> validator;

		// Token: 0x04006A8E RID: 27278
		public QualityGenerator? qualityGenerator;

		// Token: 0x04006A8F RID: 27279
		public float? maxTotalMass;

		// Token: 0x04006A90 RID: 27280
		public float? maxThingMarketValue;

		// Token: 0x04006A91 RID: 27281
		public bool? allowNonStackableDuplicates;

		// Token: 0x04006A92 RID: 27282
		public float? minSingleItemMarketValuePct;

		// Token: 0x04006A93 RID: 27283
		public FloatRange? totalMarketValueRange;

		// Token: 0x04006A94 RID: 27284
		public FloatRange? totalNutritionRange;

		// Token: 0x04006A95 RID: 27285
		public PodContentsType? podContentsType;

		// Token: 0x04006A96 RID: 27286
		public Faction makingFaction;

		// Token: 0x04006A97 RID: 27287
		public TraderKindDef traderDef;

		// Token: 0x04006A98 RID: 27288
		public PlanetTile? tile;

		// Token: 0x04006A99 RID: 27289
		public Dictionary<string, object> custom;
	}
}
