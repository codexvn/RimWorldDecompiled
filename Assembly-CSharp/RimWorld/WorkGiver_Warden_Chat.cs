using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001585 RID: 5509
	public class WorkGiver_Warden_Chat : WorkGiver_Warden
	{
		// Token: 0x0600829E RID: 33438 RVA: 0x00273888 File Offset: 0x00271A88
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (!base.ShouldTakeCareOfPrisoner(pawn, t, false))
			{
				return false;
			}
			Pawn pawn2 = (Pawn)t;
			PrisonerInteractionModeDef exclusiveInteractionMode = pawn2.guest.ExclusiveInteractionMode;
			if (pawn2.InMentalState)
			{
				JobFailReason.Is("PawnIsInMentalState".Translate(pawn2), null);
				return false;
			}
			if ((exclusiveInteractionMode == PrisonerInteractionModeDefOf.AttemptRecruit || exclusiveInteractionMode == PrisonerInteractionModeDefOf.ReduceResistance) && !pawn2.guest.ScheduledForInteraction)
			{
				JobFailReason.Is("PrisonerInteractedTooRecently".Translate(), null);
				return false;
			}
			return base.HasJobOnThing(pawn, t, forced);
		}

		// Token: 0x0600829F RID: 33439 RVA: 0x00273918 File Offset: 0x00271B18
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (!base.ShouldTakeCareOfPrisoner(pawn, t, false))
			{
				return null;
			}
			Pawn pawn2 = (Pawn)t;
			PrisonerInteractionModeDef exclusiveInteractionMode = pawn2.guest.ExclusiveInteractionMode;
			if ((exclusiveInteractionMode != PrisonerInteractionModeDefOf.AttemptRecruit && exclusiveInteractionMode != PrisonerInteractionModeDefOf.ReduceResistance) || !pawn2.guest.ScheduledForInteraction || !pawn.health.capacities.CapableOf(PawnCapacityDefOf.Talking) || (pawn2.Downed && !pawn2.InBed()) || !pawn.CanReserve(t, 1, -1, null, false) || !pawn2.Awake())
			{
				return null;
			}
			if (exclusiveInteractionMode == PrisonerInteractionModeDefOf.ReduceResistance && pawn2.guest.Resistance <= 0f)
			{
				return null;
			}
			return JobMaker.MakeJob(JobDefOf.PrisonerAttemptRecruit, t);
		}
	}
}
