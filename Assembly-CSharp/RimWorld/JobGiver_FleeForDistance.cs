using System;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001482 RID: 5250
	public class JobGiver_FleeForDistance : ThinkNode_JobGiver
	{
		// Token: 0x06007E67 RID: 32359 RVA: 0x00260B6C File Offset: 0x0025ED6C
		protected override Job TryGiveJob(Pawn pawn)
		{
			Thing danger;
			if (GenAI.EnemyIsNear(pawn, this.enemyDistToFleeRange.min, out danger, true, true))
			{
				return null;
			}
			if (GenAI.EnemyIsNear(pawn, this.enemyDistToFleeRange.max, out danger, true, true))
			{
				return FleeUtility.FleeJob(pawn, danger, Mathf.CeilToInt(this.fleeDistRange.RandomInRange));
			}
			return null;
		}

		// Token: 0x06007E68 RID: 32360 RVA: 0x00260BC2 File Offset: 0x0025EDC2
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_FleeForDistance jobGiver_FleeForDistance = (JobGiver_FleeForDistance)base.DeepCopy(resolve);
			jobGiver_FleeForDistance.enemyDistToFleeRange = this.enemyDistToFleeRange;
			jobGiver_FleeForDistance.fleeDistRange = this.fleeDistRange;
			return jobGiver_FleeForDistance;
		}

		// Token: 0x040054C2 RID: 21698
		protected FloatRange enemyDistToFleeRange = new FloatRange(2.9f, 7.9f);

		// Token: 0x040054C3 RID: 21699
		protected FloatRange fleeDistRange = new FloatRange(13.5f, 20f);
	}
}
