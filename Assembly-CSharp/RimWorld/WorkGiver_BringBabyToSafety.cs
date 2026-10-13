using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200159D RID: 5533
	public class WorkGiver_BringBabyToSafety : WorkGiver
	{
		// Token: 0x060082E5 RID: 33509 RVA: 0x00274FD8 File Offset: 0x002731D8
		public override Job NonScanJob(Pawn pawn)
		{
			Pawn pawn2 = ChildcareUtility.FindUnsafeBaby(pawn, AutofeedMode.Childcare);
			if (pawn2 == null)
			{
				return null;
			}
			Job job = JobMaker.MakeJob(JobDefOf.BringBabyToSafetyUnforced, pawn2);
			job.count = 1;
			return job;
		}
	}
}
