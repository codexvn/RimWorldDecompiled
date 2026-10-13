using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200144E RID: 5198
	public class JobGiver_Scarify : ThinkNode_JobGiver
	{
		// Token: 0x06007DD3 RID: 32211 RVA: 0x0025E4EC File Offset: 0x0025C6EC
		protected override Job TryGiveJob(Pawn pawn)
		{
			Lord lord = pawn.GetLord();
			if (lord != null)
			{
				LordJob_Ritual_Mutilation lordJob_Ritual_Mutilation = lord.LordJob as LordJob_Ritual_Mutilation;
				if (lordJob_Ritual_Mutilation != null)
				{
					Pawn pawn2 = pawn.mindState.duty.focusSecond.Pawn;
					if (lordJob_Ritual_Mutilation.mutilatedPawns.Contains(pawn2) || !pawn.CanReserveAndReach(pawn2, PathEndMode.ClosestTouch, Danger.None, 1, -1, null, false) || !JobDriver_Scarify.AvailableOnNow(pawn2, null))
					{
						return null;
					}
					return JobMaker.MakeJob(JobDefOf.Scarify, pawn2, pawn.mindState.duty.focus);
				}
			}
			return null;
		}
	}
}
