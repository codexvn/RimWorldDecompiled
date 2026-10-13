using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020037BA RID: 14266
	public class RoyalAid
	{
		// Token: 0x0400DADE RID: 56030
		public int favorCost;

		// Token: 0x0400DADF RID: 56031
		public int points;

		// Token: 0x0400DAE0 RID: 56032
		public FloatRange? overrideAcceptableTemperatureRange;

		// Token: 0x0400DAE1 RID: 56033
		public int pawnCount;

		// Token: 0x0400DAE2 RID: 56034
		public PawnKindDef pawnKindDef;

		// Token: 0x0400DAE3 RID: 56035
		public float targetingRange;

		// Token: 0x0400DAE4 RID: 56036
		public bool targetingRequireLOS = true;

		// Token: 0x0400DAE5 RID: 56037
		public float aidDurationDays;

		// Token: 0x0400DAE6 RID: 56038
		public float radius;

		// Token: 0x0400DAE7 RID: 56039
		public int intervalTicks;

		// Token: 0x0400DAE8 RID: 56040
		public int explosionCount;

		// Token: 0x0400DAE9 RID: 56041
		public int warmupTicks;

		// Token: 0x0400DAEA RID: 56042
		public FloatRange explosionRadiusRange;

		// Token: 0x0400DAEB RID: 56043
		public List<ThingDefCountClass> itemsToDrop;
	}
}
