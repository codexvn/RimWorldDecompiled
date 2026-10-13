using System;

namespace Verse.AI
{
	// Token: 0x0200103C RID: 4156
	public struct CastPositionRequest
	{
		// Token: 0x04004873 RID: 18547
		public Pawn caster;

		// Token: 0x04004874 RID: 18548
		public Thing target;

		// Token: 0x04004875 RID: 18549
		public Verb verb;

		// Token: 0x04004876 RID: 18550
		public float maxRangeFromCaster;

		// Token: 0x04004877 RID: 18551
		public float maxRangeFromTarget;

		// Token: 0x04004878 RID: 18552
		public IntVec3 locus;

		// Token: 0x04004879 RID: 18553
		public float maxRangeFromLocus;

		// Token: 0x0400487A RID: 18554
		public bool wantCoverFromTarget;

		// Token: 0x0400487B RID: 18555
		public IntVec3? preferredCastPosition;

		// Token: 0x0400487C RID: 18556
		public Func<IntVec3, bool> validator;

		// Token: 0x0400487D RID: 18557
		public int maxRegions;
	}
}
