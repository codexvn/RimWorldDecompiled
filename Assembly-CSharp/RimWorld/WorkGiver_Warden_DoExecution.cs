using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200158A RID: 5514
	public class WorkGiver_Warden_DoExecution : WorkGiver_Warden
	{
		// Token: 0x17001401 RID: 5121
		// (get) Token: 0x060082AD RID: 33453 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.OnCell;
			}
		}

		// Token: 0x060082AE RID: 33454 RVA: 0x002740B6 File Offset: 0x002722B6
		public static void ResetStaticData()
		{
			WorkGiver_Warden_DoExecution.IncapableOfViolenceLowerTrans = "IncapableOfViolenceLower".Translate();
		}

		// Token: 0x060082AF RID: 33455 RVA: 0x002740CC File Offset: 0x002722CC
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (!base.ShouldTakeCareOfPrisoner(pawn, t, false))
			{
				return null;
			}
			Pawn pawn2 = (Pawn)t;
			if (pawn2.guest.IsInteractionDisabled(PrisonerInteractionModeDefOf.Execution) || !pawn.CanReserve(t, 1, -1, null, false))
			{
				return null;
			}
			if (pawn.WorkTagIsDisabled(WorkTags.Violent))
			{
				JobFailReason.Is(WorkGiver_Warden_DoExecution.IncapableOfViolenceLowerTrans, null);
				return null;
			}
			if (!base.IsExecutionIdeoAllowed(pawn, pawn2))
			{
				return null;
			}
			return JobMaker.MakeJob(JobDefOf.PrisonerExecution, t);
		}

		// Token: 0x040056A3 RID: 22179
		private static string IncapableOfViolenceLowerTrans;
	}
}
