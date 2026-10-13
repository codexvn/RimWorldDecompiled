using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002D32 RID: 11570
	public class CompProperties_MechGestatorTank : CompProperties
	{
		// Token: 0x06010421 RID: 66593 RVA: 0x004B3515 File Offset: 0x004B1715
		public CompProperties_MechGestatorTank()
		{
			this.compClass = typeof(CompMechGestatorTank);
		}

		// Token: 0x0400A9D0 RID: 43472
		[MustTranslate]
		public string triggeredMessage;

		// Token: 0x0400A9D1 RID: 43473
		public FloatRange triggerRadiusRange = new FloatRange(6f, 12f);

		// Token: 0x0400A9D2 RID: 43474
		public GraphicData dormantGraphic;

		// Token: 0x0400A9D3 RID: 43475
		public GraphicData emptyGraphic;

		// Token: 0x0400A9D4 RID: 43476
		public SoundDef triggerSound;

		// Token: 0x0400A9D5 RID: 43477
		public List<PawnKindDefWeight> mechKindOptions = new List<PawnKindDefWeight>();
	}
}
