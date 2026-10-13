using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020006C0 RID: 1728
	public class HediffCompProperties_GiveHediffsInRange : HediffCompProperties
	{
		// Token: 0x06002B1A RID: 11034 RVA: 0x000E1C14 File Offset: 0x000DFE14
		public HediffCompProperties_GiveHediffsInRange()
		{
			this.compClass = typeof(HediffComp_GiveHediffsInRange);
		}

		// Token: 0x040022C8 RID: 8904
		public float range;

		// Token: 0x040022C9 RID: 8905
		public TargetingParameters targetingParameters;

		// Token: 0x040022CA RID: 8906
		public HediffDef hediff;

		// Token: 0x040022CB RID: 8907
		public ThingDef mote;

		// Token: 0x040022CC RID: 8908
		public bool hideMoteWhenNotDrafted;

		// Token: 0x040022CD RID: 8909
		public float initialSeverity = 1f;

		// Token: 0x040022CE RID: 8910
		public bool onlyPawnsInSameFaction = true;
	}
}
