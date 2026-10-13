using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001592 RID: 5522
	public class WorkGiver_Warden_ReleasePrisoner : WorkGiver_Warden
	{
		// Token: 0x060082C2 RID: 33474 RVA: 0x0027470C File Offset: 0x0027290C
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (!base.ShouldTakeCareOfPrisoner(pawn, t, forced))
			{
				return null;
			}
			Pawn pawn2 = (Pawn)t;
			if (pawn2.InMentalState)
			{
				JobFailReason.Is("PawnIsInMentalState".Translate(pawn2), null);
				return null;
			}
			if (!pawn2.guest.IsInteractionEnabled(PrisonerInteractionModeDefOf.Release) || pawn2.Downed || pawn2.guest.Released)
			{
				return null;
			}
			if (pawn.Faction != pawn2.Faction && !pawn.MapHeld.CanEverExit)
			{
				JobFailReason.Is("CannotExitMap".Translate(), null);
				return null;
			}
			IntVec3 c;
			if (!RCellFinder.TryFindPrisonerReleaseCell(pawn2, pawn, out c))
			{
				return null;
			}
			Job job = JobMaker.MakeJob(JobDefOf.ReleasePrisoner, pawn2, c);
			job.count = 1;
			return job;
		}
	}
}
