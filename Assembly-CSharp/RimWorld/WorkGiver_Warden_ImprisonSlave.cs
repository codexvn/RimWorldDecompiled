using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001590 RID: 5520
	public class WorkGiver_Warden_ImprisonSlave : WorkGiver_Warden
	{
		// Token: 0x060082BD RID: 33469 RVA: 0x00274558 File Offset: 0x00272758
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (!ModLister.CheckIdeology("Slave imprisonment"))
			{
				return null;
			}
			Pawn pawn2 = t as Pawn;
			if (!base.ShouldTakeCareOfSlave(pawn, pawn2, false))
			{
				return null;
			}
			if (pawn2.guest.slaveInteractionMode != SlaveInteractionModeDefOf.Imprison)
			{
				return null;
			}
			Building_Bed building_Bed = RestUtility.FindBedFor(pawn2, pawn, false, false, new GuestStatus?(GuestStatus.Prisoner));
			if (building_Bed == null)
			{
				return null;
			}
			Job job = JobMaker.MakeJob(JobDefOf.Arrest, pawn2, building_Bed);
			job.count = 1;
			return job;
		}
	}
}
