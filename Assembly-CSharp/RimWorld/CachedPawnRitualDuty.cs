using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001740 RID: 5952
	public struct CachedPawnRitualDuty
	{
		// Token: 0x04005AE5 RID: 23269
		public DutyDef duty;

		// Token: 0x04005AE6 RID: 23270
		public IntVec3 spot;

		// Token: 0x04005AE7 RID: 23271
		public Thing usedThing;

		// Token: 0x04005AE8 RID: 23272
		public Rot4 overrideFacing;

		// Token: 0x04005AE9 RID: 23273
		public LocalTargetInfo secondFocus;
	}
}
