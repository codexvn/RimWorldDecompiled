using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001625 RID: 5669
	public class WorkGiver_TendSelfEmergency : WorkGiver_TendSelf
	{
		// Token: 0x060085AC RID: 34220 RVA: 0x0027EB1C File Offset: 0x0027CD1C
		public override Job NonScanJob(Pawn pawn)
		{
			if (!this.HasJobOnThing(pawn, pawn, false) || !HealthAIUtility.ShouldBeTendedNowByPlayerUrgent(pawn))
			{
				return null;
			}
			ThinkResult thinkResult = WorkGiver_TendSelfEmergency.jgp.TryIssueJobPackage(pawn, default(JobIssueParams));
			if (thinkResult.IsValid)
			{
				return thinkResult.Job;
			}
			return null;
		}

		// Token: 0x04005768 RID: 22376
		private static JobGiver_SelfTend jgp = new JobGiver_SelfTend();
	}
}
