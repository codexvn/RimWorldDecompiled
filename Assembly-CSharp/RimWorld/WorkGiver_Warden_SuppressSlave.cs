using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001595 RID: 5525
	public class WorkGiver_Warden_SuppressSlave : WorkGiver_Warden
	{
		// Token: 0x060082CB RID: 33483 RVA: 0x002749D0 File Offset: 0x00272BD0
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (!ModLister.CheckIdeology("Slave suppression"))
			{
				return null;
			}
			Pawn pawn2 = t as Pawn;
			if (!base.ShouldTakeCareOfSlave(pawn, pawn2, false))
			{
				return null;
			}
			if (pawn2.guest.slaveInteractionMode != SlaveInteractionModeDefOf.Suppress || pawn2.Downed || !pawn2.Awake() || !pawn.CanReserve(t, 1, -1, null, false))
			{
				return null;
			}
			Need_Suppression need_Suppression;
			pawn2.needs.TryGetNeed<Need_Suppression>(out need_Suppression);
			if (need_Suppression == null || !need_Suppression.CanBeSuppressedNow || !pawn2.guest.ScheduledForSlaveSuppression)
			{
				return null;
			}
			return JobMaker.MakeJob(JobDefOf.SlaveSuppress, t);
		}
	}
}
