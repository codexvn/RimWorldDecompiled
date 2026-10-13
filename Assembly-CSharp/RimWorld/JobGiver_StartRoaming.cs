using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020013D9 RID: 5081
	public class JobGiver_StartRoaming : ThinkNode_JobGiver
	{
		// Token: 0x06007C35 RID: 31797 RVA: 0x00256744 File Offset: 0x00254944
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (!MentalStateWorker_Roaming.CanRoamNow(pawn))
			{
				return null;
			}
			if (!pawn.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Roaming, null, false, false, false, null, false, false, false))
			{
				return null;
			}
			return JobMaker.MakeJob(JobDefOf.Wait, 10, false);
		}
	}
}
