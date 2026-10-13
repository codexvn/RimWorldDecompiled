using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200158D RID: 5517
	public class WorkGiver_Warden_ExecuteSlave : WorkGiver_Warden
	{
		// Token: 0x17001402 RID: 5122
		// (get) Token: 0x060082B5 RID: 33461 RVA: 0x000E96BB File Offset: 0x000E78BB
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x060082B6 RID: 33462 RVA: 0x00274315 File Offset: 0x00272515
		public static void ResetStaticData()
		{
			WorkGiver_Warden_ExecuteSlave.IncapableOfViolenceLowerTrans = "IncapableOfViolenceLower".Translate();
		}

		// Token: 0x060082B7 RID: 33463 RVA: 0x0027432C File Offset: 0x0027252C
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (!base.ShouldTakeCareOfSlave(pawn, t, forced))
			{
				return null;
			}
			Pawn pawn2 = (Pawn)t;
			if (pawn2.guest.slaveInteractionMode != SlaveInteractionModeDefOf.Execute || !pawn.CanReserve(t, 1, -1, null, false))
			{
				return null;
			}
			if (pawn.WorkTagIsDisabled(WorkTags.Violent))
			{
				JobFailReason.Is(WorkGiver_Warden_ExecuteSlave.IncapableOfViolenceLowerTrans, null);
				return null;
			}
			if (!base.IsExecutionIdeoAllowed(pawn, pawn2))
			{
				return null;
			}
			return JobMaker.MakeJob(JobDefOf.SlaveExecution, t);
		}

		// Token: 0x040056A4 RID: 22180
		private static string IncapableOfViolenceLowerTrans;
	}
}
