using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001465 RID: 5221
	public class JobGiver_ManTurretsNearPoint : JobGiver_ManTurrets
	{
		// Token: 0x06007E10 RID: 32272 RVA: 0x0025F563 File Offset: 0x0025D763
		protected override IntVec3 GetRoot(Pawn pawn)
		{
			return pawn.GetLord().CurLordToil.FlagLoc;
		}
	}
}
