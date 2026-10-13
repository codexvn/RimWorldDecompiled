using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020015FD RID: 5629
	public class WorkGiver_PatientGoToBedRecuperate : WorkGiver
	{
		// Token: 0x060084D9 RID: 34009 RVA: 0x0027C9D8 File Offset: 0x0027ABD8
		public override Job NonScanJob(Pawn pawn)
		{
			ThinkResult thinkResult = WorkGiver_PatientGoToBedRecuperate.jgp.TryIssueJobPackage(pawn, default(JobIssueParams));
			if (thinkResult.IsValid)
			{
				return thinkResult.Job;
			}
			return null;
		}

		// Token: 0x04005748 RID: 22344
		private static JobGiver_PatientGoToBed jgp = new JobGiver_PatientGoToBed
		{
			respectTimetable = false
		};
	}
}
