using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200158B RID: 5515
	public class WorkGiver_Warden_EmancipateSlave : WorkGiver_Warden
	{
		// Token: 0x060082B1 RID: 33457 RVA: 0x00274148 File Offset: 0x00272348
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (!ModLister.CheckIdeology("Slave imprisonment"))
			{
				return null;
			}
			Pawn pawn2 = t as Pawn;
			if (pawn2 == null)
			{
				return null;
			}
			if (!base.ShouldTakeCareOfSlave(pawn, pawn2, false))
			{
				return null;
			}
			if (!pawn.MapHeld.CanEverExit)
			{
				JobFailReason.Is("CannotExitMap".Translate(), null);
				return null;
			}
			if (pawn2.guest.slaveInteractionMode != SlaveInteractionModeDefOf.Emancipate || pawn2.Downed || !pawn2.Awake())
			{
				return null;
			}
			Job job = JobMaker.MakeJob(JobDefOf.SlaveEmancipation, pawn2);
			job.count = 1;
			return job;
		}
	}
}
