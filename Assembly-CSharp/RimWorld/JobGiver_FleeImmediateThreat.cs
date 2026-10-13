using System;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001489 RID: 5257
	public class JobGiver_FleeImmediateThreat : ThinkNode_JobGiver
	{
		// Token: 0x06007E79 RID: 32377 RVA: 0x00261004 File Offset: 0x0025F204
		protected override Job TryGiveJob(Pawn pawn)
		{
			Job job = FleeUtility.FleeLargeFireJob(pawn, 1, this.distToFireToFlee, Mathf.CeilToInt(this.fleeDistRange.RandomInRange));
			if (job != null)
			{
				return job;
			}
			Thing danger;
			if (!GenAI.EnemyIsNear(pawn, (float)this.distToDangerToFlee, out danger, false, false))
			{
				return null;
			}
			return FleeUtility.FleeJob(pawn, danger, Mathf.CeilToInt(this.fleeDistRange.RandomInRange));
		}

		// Token: 0x06007E7A RID: 32378 RVA: 0x00261060 File Offset: 0x0025F260
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_FleeImmediateThreat jobGiver_FleeImmediateThreat = (JobGiver_FleeImmediateThreat)base.DeepCopy(resolve);
			jobGiver_FleeImmediateThreat.fleeDistRange = this.fleeDistRange;
			jobGiver_FleeImmediateThreat.distToFireToFlee = this.distToFireToFlee;
			jobGiver_FleeImmediateThreat.distToDangerToFlee = this.distToDangerToFlee;
			return jobGiver_FleeImmediateThreat;
		}

		// Token: 0x040054D1 RID: 21713
		private FloatRange fleeDistRange = new FloatRange(50f, 75f);

		// Token: 0x040054D2 RID: 21714
		private int distToFireToFlee = 4;

		// Token: 0x040054D3 RID: 21715
		private int distToDangerToFlee = 40;
	}
}
