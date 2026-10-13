using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200158C RID: 5516
	public class WorkGiver_Warden_Enslave : WorkGiver_Warden
	{
		// Token: 0x060082B3 RID: 33459 RVA: 0x002741DC File Offset: 0x002723DC
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (!ModLister.CheckIdeology("WorkGiver_Warden_Enslave"))
			{
				return null;
			}
			if (!base.ShouldTakeCareOfPrisoner(pawn, t, false))
			{
				return null;
			}
			Pawn pawn2 = (Pawn)t;
			if (pawn2.InMentalState)
			{
				JobFailReason.Is("PawnIsInMentalState".Translate(pawn2), null);
				return null;
			}
			bool flag = pawn2.guest.IsInteractionEnabled(PrisonerInteractionModeDefOf.Enslave);
			bool flag2 = pawn2.guest.IsInteractionEnabled(PrisonerInteractionModeDefOf.ReduceWill);
			if ((!flag && !flag2) || !pawn2.guest.ScheduledForInteraction || !pawn2.guest.IsPrisoner || (flag2 && pawn2.guest.will <= 0f) || (pawn2.Downed && !pawn2.InBed()) || !pawn.health.capacities.CapableOf(PawnCapacityDefOf.Talking) || !pawn.CanReserve(t, 1, -1, null, false) || !pawn2.Awake() || !new HistoryEvent(HistoryEventDefOf.EnslavedPrisoner, pawn.Named(HistoryEventArgsNames.Doer)).Notify_PawnAboutToDo_Job())
			{
				return null;
			}
			if (flag)
			{
				return JobMaker.MakeJob(JobDefOf.PrisonerEnslave, t);
			}
			return JobMaker.MakeJob(JobDefOf.PrisonerReduceWill, t);
		}
	}
}
