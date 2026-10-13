using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001586 RID: 5510
	public class WorkGiver_Warden_Convert : WorkGiver_Warden
	{
		// Token: 0x060082A1 RID: 33441 RVA: 0x002739DC File Offset: 0x00271BDC
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (!ModLister.CheckIdeology("WorkGiver_Warden_Convert"))
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
			if (pawn2.guest.IsInteractionEnabled(PrisonerInteractionModeDefOf.Convert) && !pawn2.guest.ScheduledForInteraction)
			{
				JobFailReason.Is("PrisonerInteractedTooRecently".Translate(), null);
				return false;
			}
			return base.HasJobOnThing(pawn, t, forced);
		}

		// Token: 0x060082A2 RID: 33442 RVA: 0x00273A70 File Offset: 0x00271C70
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (!ModLister.CheckIdeology("WorkGiver_Warden_Convert"))
			{
				return null;
			}
			if (!base.ShouldTakeCareOfPrisoner(pawn, t, false))
			{
				return null;
			}
			Pawn pawn2 = (Pawn)t;
			if (pawn2.guest.IsInteractionEnabled(PrisonerInteractionModeDefOf.Convert) && pawn2.guest.ScheduledForInteraction && pawn2.guest.IsPrisoner && (!pawn2.Downed || pawn2.InBed()) && pawn2.Ideo != pawn.Ideo && pawn.Ideo == pawn2.guest.ideoForConversion && pawn.health.capacities.CapableOf(PawnCapacityDefOf.Talking) && pawn.CanReserve(t, 1, -1, null, false) && pawn2.Awake())
			{
				return JobMaker.MakeJob(JobDefOf.PrisonerConvert, t);
			}
			return null;
		}
	}
}
