using System;
using RimWorld.Planet;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200153F RID: 5439
	public class JobGiver_WanderControlGroupTarget : JobGiver_Wander
	{
		// Token: 0x0600811C RID: 33052 RVA: 0x001FF5F5 File Offset: 0x001FD7F5
		public JobGiver_WanderControlGroupTarget()
		{
			this.wanderRadius = 7f;
			this.ticksBetweenWandersRange = new IntRange(125, 200);
		}

		// Token: 0x0600811D RID: 33053 RVA: 0x0026DD91 File Offset: 0x0026BF91
		private GlobalTargetInfo Target(Pawn pawn)
		{
			return pawn.relations.GetFirstDirectRelationPawn(PawnRelationDefOf.Overseer, null).mechanitor.GetControlGroup(pawn).Target;
		}

		// Token: 0x0600811E RID: 33054 RVA: 0x0026DDB4 File Offset: 0x0026BFB4
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (this.Target(pawn).Map != pawn.Map)
			{
				return null;
			}
			return base.TryGiveJob(pawn);
		}

		// Token: 0x0600811F RID: 33055 RVA: 0x0026DDE4 File Offset: 0x0026BFE4
		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			return this.Target(pawn).Cell;
		}

		// Token: 0x06008120 RID: 33056 RVA: 0x0026DE00 File Offset: 0x0026C000
		protected override void DecorateGotoJob(Job job)
		{
			job.expiryInterval = 120;
			job.expireRequiresEnemiesNearby = true;
		}
	}
}
