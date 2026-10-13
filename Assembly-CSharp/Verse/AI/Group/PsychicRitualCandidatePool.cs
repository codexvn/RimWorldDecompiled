using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x02001057 RID: 4183
	public class PsychicRitualCandidatePool : ILordJobCandidatePool
	{
		// Token: 0x17000FE8 RID: 4072
		// (get) Token: 0x06006612 RID: 26130 RVA: 0x00205570 File Offset: 0x00203770
		public List<Pawn> NonAssignablePawns
		{
			get
			{
				return this.nonAssignablePawns;
			}
		}

		// Token: 0x17000FE9 RID: 4073
		// (get) Token: 0x06006613 RID: 26131 RVA: 0x00205578 File Offset: 0x00203778
		public List<Pawn> AllCandidatePawns
		{
			get
			{
				return this.allCandidatePawns;
			}
		}

		// Token: 0x06006614 RID: 26132 RVA: 0x00205580 File Offset: 0x00203780
		public PsychicRitualCandidatePool(List<Pawn> allCandidatePawns, List<Pawn> nonAssignablePawns)
		{
			this.allCandidatePawns = allCandidatePawns;
			this.nonAssignablePawns = nonAssignablePawns;
		}

		// Token: 0x0400490A RID: 18698
		private List<Pawn> nonAssignablePawns;

		// Token: 0x0400490B RID: 18699
		private List<Pawn> allCandidatePawns;
	}
}
