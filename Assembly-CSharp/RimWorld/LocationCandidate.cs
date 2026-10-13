using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200353D RID: 13629
	public struct LocationCandidate
	{
		// Token: 0x060131EE RID: 78318 RVA: 0x005ACBE5 File Offset: 0x005AADE5
		public LocationCandidate(IntVec3 cell, float score)
		{
			this.cell = cell;
			this.score = score;
		}

		// Token: 0x0400C661 RID: 50785
		public IntVec3 cell;

		// Token: 0x0400C662 RID: 50786
		public float score;
	}
}
