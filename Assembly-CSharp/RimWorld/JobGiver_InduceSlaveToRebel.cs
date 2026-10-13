using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200152D RID: 5421
	public class JobGiver_InduceSlaveToRebel : ThinkNode_JobGiver
	{
		// Token: 0x060080EC RID: 33004 RVA: 0x0026D2F0 File Offset: 0x0026B4F0
		protected override Job TryGiveJob(Pawn pawn)
		{
			Pawn pawn2 = SlaveRebellionUtility.FindSlaveForRebellion(pawn);
			if (pawn2 == null || !pawn.CanReach(pawn2, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				return null;
			}
			Job job = JobMaker.MakeJob(JobDefOf.InduceSlaveToRebel, pawn2);
			job.interaction = InteractionDefOf.SparkSlaveRebellion;
			return job;
		}
	}
}
