using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200143B RID: 5179
	public class JobGiver_EatAtCannibalPlatter : ThinkNode_JobGiver
	{
		// Token: 0x06007DA3 RID: 32163 RVA: 0x0025D75C File Offset: 0x0025B95C
		protected override Job TryGiveJob(Pawn pawn)
		{
			JobGiver_EatAtCannibalPlatter.<>c__DisplayClass0_0 CS$<>8__locals1 = new JobGiver_EatAtCannibalPlatter.<>c__DisplayClass0_0();
			CS$<>8__locals1.pawn = pawn;
			LordJob_Ritual lordJob_Ritual = CS$<>8__locals1.pawn.GetLord().LordJob as LordJob_Ritual;
			IntVec3 c;
			if (!GatheringsUtility.TryFindRandomCellAroundTarget(CS$<>8__locals1.pawn, lordJob_Ritual.selectedTarget.Thing, out c) && !GatheringsUtility.TryFindRandomCellInGatheringArea(CS$<>8__locals1.pawn, new Predicate<IntVec3>(CS$<>8__locals1.<TryGiveJob>g__CellValid|0), out c))
			{
				return null;
			}
			Job job = JobMaker.MakeJob(JobDefOf.EatAtCannibalPlatter, lordJob_Ritual.selectedTarget.Thing, c);
			job.doUntilGatheringEnded = true;
			job.expiryInterval = lordJob_Ritual.DurationTicks;
			return job;
		}
	}
}
