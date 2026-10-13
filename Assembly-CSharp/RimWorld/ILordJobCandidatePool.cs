using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020027A0 RID: 10144
	public interface ILordJobCandidatePool
	{
		// Token: 0x170021F1 RID: 8689
		// (get) Token: 0x0600DE28 RID: 56872
		List<Pawn> AllCandidatePawns { get; }

		// Token: 0x170021F2 RID: 8690
		// (get) Token: 0x0600DE29 RID: 56873
		List<Pawn> NonAssignablePawns { get; }
	}
}
