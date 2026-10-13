using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001591 RID: 5521
	public class WorkGiver_Warden_InterrogateIdentity : WorkGiver_Warden
	{
		// Token: 0x060082BF RID: 33471 RVA: 0x002745D0 File Offset: 0x002727D0
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (!ModsConfig.AnomalyActive)
			{
				return false;
			}
			if (!base.ShouldTakeCareOfPrisoner(pawn, t, false))
			{
				return false;
			}
			Pawn pawn2 = (Pawn)t;
			if (pawn2.InMentalState)
			{
				JobFailReason.Is("PawnIsInMentalState".Translate(pawn2), null);
				return false;
			}
			if (pawn2.guest.IsInteractionEnabled(PrisonerInteractionModeDefOf.Interrogate) && !pawn2.guest.ScheduledForInteraction)
			{
				JobFailReason.Is("PrisonerInteractedTooRecently".Translate(), null);
				return false;
			}
			return base.HasJobOnThing(pawn, t, forced);
		}

		// Token: 0x060082C0 RID: 33472 RVA: 0x00274660 File Offset: 0x00272860
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (!ModsConfig.AnomalyActive)
			{
				return null;
			}
			if (!base.ShouldTakeCareOfPrisoner(pawn, t, false))
			{
				return null;
			}
			Pawn pawn2 = (Pawn)t;
			if (pawn2.guest.IsInteractionEnabled(PrisonerInteractionModeDefOf.Interrogate) && pawn2.guest.ScheduledForInteraction && pawn2.guest.IsPrisoner && (!pawn2.Downed || pawn2.InBed()) && pawn.health.capacities.CapableOf(PawnCapacityDefOf.Talking) && pawn.CanReserve(t, 1, -1, null, false) && pawn2.Awake())
			{
				return JobMaker.MakeJob(JobDefOf.PrisonerInterrogateIdentity, t);
			}
			return null;
		}
	}
}
