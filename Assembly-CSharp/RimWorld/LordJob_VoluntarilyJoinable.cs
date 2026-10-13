using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020016D4 RID: 5844
	public abstract class LordJob_VoluntarilyJoinable : LordJob
	{
		// Token: 0x17001524 RID: 5412
		// (get) Token: 0x06008A06 RID: 35334 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool ShouldExistWithoutPawns
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06008A07 RID: 35335 RVA: 0x00006AAC File Offset: 0x00004CAC
		public virtual float VoluntaryJoinPriorityFor(Pawn p)
		{
			return 0f;
		}

		// Token: 0x17001525 RID: 5413
		// (get) Token: 0x06008A08 RID: 35336 RVA: 0x00002501 File Offset: 0x00000701
		public override bool AddFleeToil
		{
			get
			{
				return false;
			}
		}
	}
}
