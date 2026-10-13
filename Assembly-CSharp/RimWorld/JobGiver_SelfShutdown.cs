using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020014CB RID: 5323
	public class JobGiver_SelfShutdown : ThinkNode_JobGiver
	{
		// Token: 0x06007F54 RID: 32596 RVA: 0x0026570C File Offset: 0x0026390C
		protected override Job TryGiveJob(Pawn pawn)
		{
			IntVec3 c;
			if (RCellFinder.TryFindNearbyMechSelfShutdownSpot(pawn.Position, pawn, pawn.Map, out c, true))
			{
				Job job = JobMaker.MakeJob(JobDefOf.SelfShutdown, c);
				job.forceSleep = true;
				return job;
			}
			return null;
		}
	}
}
