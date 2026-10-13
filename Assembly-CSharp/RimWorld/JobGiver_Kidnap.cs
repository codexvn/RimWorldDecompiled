using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200145C RID: 5212
	public class JobGiver_Kidnap : ThinkNode_JobGiver
	{
		// Token: 0x06007DFB RID: 32251 RVA: 0x0025F0DC File Offset: 0x0025D2DC
		protected override Job TryGiveJob(Pawn pawn)
		{
			IntVec3 c;
			if (!RCellFinder.TryFindBestExitSpot(pawn, out c, TraverseMode.ByPawn, true))
			{
				return null;
			}
			Pawn t;
			if (KidnapAIUtility.TryFindGoodKidnapVictim(pawn, 18f, out t, null) && !GenAI.InDangerousCombat(pawn))
			{
				Job job = JobMaker.MakeJob(JobDefOf.Kidnap);
				job.targetA = t;
				job.targetB = c;
				job.count = 1;
				return job;
			}
			return null;
		}

		// Token: 0x04005498 RID: 21656
		public const float VictimSearchRadiusInitial = 8f;

		// Token: 0x04005499 RID: 21657
		private const float VictimSearchRadiusOngoing = 18f;
	}
}
