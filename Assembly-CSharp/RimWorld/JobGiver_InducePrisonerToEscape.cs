using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200152C RID: 5420
	public class JobGiver_InducePrisonerToEscape : ThinkNode_JobGiver
	{
		// Token: 0x060080EA RID: 33002 RVA: 0x0026D2A8 File Offset: 0x0026B4A8
		protected override Job TryGiveJob(Pawn pawn)
		{
			Pawn pawn2 = JailbreakerMentalStateUtility.FindPrisoner(pawn);
			if (pawn2 == null || !pawn.CanReach(pawn2, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				return null;
			}
			Job job = JobMaker.MakeJob(JobDefOf.InducePrisonerToEscape, pawn2);
			job.interaction = InteractionDefOf.SparkJailbreak;
			return job;
		}
	}
}
