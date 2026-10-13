using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200148E RID: 5262
	public class JobGiver_GetAgeReversal : ThinkNode_JobGiver
	{
		// Token: 0x06007E84 RID: 32388 RVA: 0x00261288 File Offset: 0x0025F488
		public CompBiosculpterPod GetBiosculpterPod(Pawn pawn)
		{
			List<CompBiosculpterPod> list = CompBiosculpterPod.BiotunedPods(pawn);
			if (list != null && CompBiosculpterPod.CanAgeReverse(pawn) && pawn.ageTracker.AgeReversalDemandedDeadlineTicks <= 0L)
			{
				foreach (CompBiosculpterPod compBiosculpterPod in list)
				{
					if (compBiosculpterPod.parent.Spawned && compBiosculpterPod.AutoAgeReversal && compBiosculpterPod.CanAcceptOnceCycleChosen(pawn) && compBiosculpterPod.PawnCanUseNow(pawn, compBiosculpterPod.GetCycle(compBiosculpterPod.AgeReversalCycleKey)))
					{
						return compBiosculpterPod;
					}
				}
			}
			return null;
		}

		// Token: 0x06007E85 RID: 32389 RVA: 0x0026132C File Offset: 0x0025F52C
		public override float GetPriority(Pawn pawn)
		{
			if (!ModsConfig.IdeologyActive)
			{
				return 0f;
			}
			if (this.GetBiosculpterPod(pawn) != null)
			{
				return 7.4f;
			}
			return 0f;
		}

		// Token: 0x06007E86 RID: 32390 RVA: 0x00261350 File Offset: 0x0025F550
		protected override Job TryGiveJob(Pawn pawn)
		{
			CompBiosculpterPod biosculpterPod = this.GetBiosculpterPod(pawn);
			if (biosculpterPod == null || !pawn.CanReserve(biosculpterPod.parent, 1, -1, null, false))
			{
				return null;
			}
			Job job = biosculpterPod.EnterBiosculpterJob();
			biosculpterPod.ConfigureJobForCycle(job, biosculpterPod.GetCycle(biosculpterPod.AgeReversalCycleKey), null);
			return job;
		}
	}
}
