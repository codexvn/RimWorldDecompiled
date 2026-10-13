using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200142E RID: 5166
	public class JobGiver_Blind : ThinkNode_JobGiver
	{
		// Token: 0x06007D7F RID: 32127 RVA: 0x0025CAB0 File Offset: 0x0025ACB0
		protected override Job TryGiveJob(Pawn pawn)
		{
			Lord lord = pawn.GetLord();
			if (lord != null)
			{
				LordJob_Ritual_Mutilation lordJob_Ritual_Mutilation = lord.LordJob as LordJob_Ritual_Mutilation;
				if (lordJob_Ritual_Mutilation != null)
				{
					Pawn pawn2 = pawn.mindState.duty.focusSecond.Pawn;
					if (lordJob_Ritual_Mutilation.mutilatedPawns.Contains(pawn2) || !pawn.CanReserveAndReach(pawn2, PathEndMode.ClosestTouch, Danger.None, 1, -1, null, false))
					{
						return null;
					}
					return JobMaker.MakeJob(JobDefOf.Blind, pawn2, pawn.mindState.duty.focus);
				}
			}
			return null;
		}
	}
}
