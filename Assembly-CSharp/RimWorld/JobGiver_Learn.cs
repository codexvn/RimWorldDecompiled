using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001516 RID: 5398
	public class JobGiver_Learn : ThinkNode_JobGiver
	{
		// Token: 0x0600809E RID: 32926 RVA: 0x0026C1D8 File Offset: 0x0026A3D8
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (!ModsConfig.BiotechActive)
			{
				return null;
			}
			if (pawn.needs.learning.CurLevelPercentage >= 0.95f)
			{
				return null;
			}
			List<LearningDesireDef> activeLearningDesires = pawn.learning.ActiveLearningDesires;
			if (activeLearningDesires.NullOrEmpty<LearningDesireDef>())
			{
				return null;
			}
			JobGiver_Learn.tmpRandomLearningDesires.Clear();
			JobGiver_Learn.tmpRandomLearningDesires.AddRange(activeLearningDesires.InRandomOrder(null));
			for (int i = 0; i < JobGiver_Learn.tmpRandomLearningDesires.Count; i++)
			{
				if (JobGiver_Learn.tmpRandomLearningDesires[i].Worker.CanDo(pawn))
				{
					Job job = JobGiver_Learn.tmpRandomLearningDesires[i].Worker.TryGiveJob(pawn);
					if (job != null)
					{
						JobGiver_Learn.tmpRandomLearningDesires.Clear();
						job.isLearningDesire = true;
						return job;
					}
				}
			}
			JobGiver_Learn.tmpRandomLearningDesires.Clear();
			return null;
		}

		// Token: 0x040055F9 RID: 22009
		private const float MaxLearning = 0.95f;

		// Token: 0x040055FA RID: 22010
		private static List<LearningDesireDef> tmpRandomLearningDesires = new List<LearningDesireDef>();
	}
}
