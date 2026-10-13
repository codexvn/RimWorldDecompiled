using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020014CC RID: 5324
	public class JobGiver_SelfTend : ThinkNode_JobGiver
	{
		// Token: 0x06007F56 RID: 32598 RVA: 0x0026574C File Offset: 0x0026394C
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (!pawn.RaceProps.Humanlike || !pawn.health.HasHediffsNeedingTend(false) || !pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation) || pawn.InAggroMentalState)
			{
				return null;
			}
			if (pawn.IsColonist && pawn.WorkTypeIsDisabled(WorkTypeDefOf.Doctor))
			{
				return null;
			}
			if (!pawn.Map.reservationManager.CanReserve(pawn, pawn, 1, -1, null, false))
			{
				return null;
			}
			Job job = JobMaker.MakeJob(JobDefOf.TendPatient, pawn);
			job.endAfterTendedOnce = true;
			return job;
		}
	}
}
