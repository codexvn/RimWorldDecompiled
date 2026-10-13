using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001422 RID: 5154
	public class JobGiver_PrepareCaravan_GatherItems : ThinkNode_JobGiver
	{
		// Token: 0x06007D53 RID: 32083 RVA: 0x0025BF3C File Offset: 0x0025A13C
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (!pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
			{
				return null;
			}
			Lord lord = pawn.GetLord();
			Thing thing = GatherItemsForCaravanUtility.FindThingToHaul(pawn, lord);
			if (thing == null)
			{
				return null;
			}
			Job job = JobMaker.MakeJob(JobDefOf.PrepareCaravan_GatherItems, thing);
			job.lord = lord;
			return job;
		}
	}
}
