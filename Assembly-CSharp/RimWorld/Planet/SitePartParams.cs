using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003DE8 RID: 15848
	public class SitePartParams : IExposable
	{
		// Token: 0x06015A0D RID: 88589 RVA: 0x006793B4 File Offset: 0x006775B4
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.randomValue, "randomValue", 0, false);
			Scribe_Values.Look<float>(ref this.threatPoints, "threatPoints", 0f, false);
			Scribe_Values.Look<float>(ref this.lootMarketValue, "lootMarketValue", 0f, false);
			Scribe_Values.Look<float>(ref this.points, "points", 0f, false);
			Scribe_References.Look<Precept_Relic>(ref this.relic, "relic", false);
			Scribe_Defs.Look<ThingDef>(ref this.preciousLumpResources, "preciousLumpResources");
			Scribe_Defs.Look<PawnKindDef>(ref this.animalKind, "animalKind");
			Scribe_Values.Look<int>(ref this.turretsCount, "turretsCount", 0, false);
			Scribe_Values.Look<int>(ref this.mortarsCount, "mortarsCount", 0, false);
			Scribe_Deep.Look<LayoutStructureSketch>(ref this.ancientLayoutStructureSketch, "ancientComplexSketch", Array.Empty<object>());
			Scribe_Defs.Look<ThingSetMakerDef>(ref this.ancientComplexRewardMaker, "ancientComplexRewardMaker");
			Scribe_Values.Look<string>(ref this.triggerSecuritySignal, "triggerSecuritySignal", null, false);
			Scribe_References.Look<Thing>(ref this.relicThing, "relicThing", false);
			Scribe_Values.Look<string>(ref this.relicLostSignal, "relicLostSignal", null, false);
			Scribe_Values.Look<float>(ref this.interiorThreatPoints, "interiorThreatPoints", 0f, false);
			Scribe_Values.Look<float>(ref this.exteriorThreatPoints, "exteriorThreatPoints", 0f, false);
			Scribe_Values.Look<int>(ref this.stabilizerCount, "stabilizerCount", 0, false);
		}

		// Token: 0x0400F04B RID: 61515
		public int randomValue;

		// Token: 0x0400F04C RID: 61516
		public float threatPoints;

		// Token: 0x0400F04D RID: 61517
		public float lootMarketValue;

		// Token: 0x0400F04E RID: 61518
		public float points;

		// Token: 0x0400F04F RID: 61519
		public Precept_Relic relic;

		// Token: 0x0400F050 RID: 61520
		public ThingDef preciousLumpResources;

		// Token: 0x0400F051 RID: 61521
		public PawnKindDef animalKind;

		// Token: 0x0400F052 RID: 61522
		public int turretsCount;

		// Token: 0x0400F053 RID: 61523
		public int mortarsCount;

		// Token: 0x0400F054 RID: 61524
		public LayoutStructureSketch ancientLayoutStructureSketch;

		// Token: 0x0400F055 RID: 61525
		public ThingSetMakerDef ancientComplexRewardMaker;

		// Token: 0x0400F056 RID: 61526
		public string triggerSecuritySignal;

		// Token: 0x0400F057 RID: 61527
		public string relicLostSignal;

		// Token: 0x0400F058 RID: 61528
		public Thing relicThing;

		// Token: 0x0400F059 RID: 61529
		public float interiorThreatPoints;

		// Token: 0x0400F05A RID: 61530
		public float exteriorThreatPoints;

		// Token: 0x0400F05B RID: 61531
		public int stabilizerCount;
	}
}
