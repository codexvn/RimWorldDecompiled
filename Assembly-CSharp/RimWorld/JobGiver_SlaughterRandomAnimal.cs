using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001533 RID: 5427
	public class JobGiver_SlaughterRandomAnimal : ThinkNode_JobGiver
	{
		// Token: 0x060080FD RID: 33021 RVA: 0x0026D768 File Offset: 0x0026B968
		protected override Job TryGiveJob(Pawn pawn)
		{
			MentalState_SlaughterThing mentalState_SlaughterThing = pawn.MentalState as MentalState_SlaughterThing;
			if (mentalState_SlaughterThing != null && mentalState_SlaughterThing.SlaughteredRecently)
			{
				return null;
			}
			Pawn pawn2 = SlaughtererMentalStateUtility.FindAnimal(pawn);
			if (pawn2 == null || !pawn.CanReserveAndReach(pawn2, PathEndMode.Touch, Danger.Deadly, 1, -1, null, false))
			{
				return null;
			}
			Job job = JobMaker.MakeJob(JobDefOf.Slaughter, pawn2);
			job.ignoreDesignations = true;
			return job;
		}
	}
}
