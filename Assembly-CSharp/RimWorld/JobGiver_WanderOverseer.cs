using System;
using RimWorld.Planet;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001540 RID: 5440
	public class JobGiver_WanderOverseer : JobGiver_Wander
	{
		// Token: 0x06008121 RID: 33057 RVA: 0x0026DE11 File Offset: 0x0026C011
		public JobGiver_WanderOverseer()
		{
			this.wanderRadius = 3f;
			this.ticksBetweenWandersRange = new IntRange(125, 200);
		}

		// Token: 0x06008122 RID: 33058 RVA: 0x0026DE36 File Offset: 0x0026C036
		private GlobalTargetInfo Target(Pawn pawn)
		{
			return pawn.GetOverseer();
		}

		// Token: 0x06008123 RID: 33059 RVA: 0x0026DE44 File Offset: 0x0026C044
		protected override Job TryGiveJob(Pawn pawn)
		{
			GlobalTargetInfo globalTargetInfo = this.Target(pawn);
			if (globalTargetInfo.Map != pawn.Map)
			{
				return null;
			}
			Job job = base.TryGiveJob(pawn);
			job.reportStringOverride = "Escorting".Translate(globalTargetInfo.Thing.Named("TARGET"));
			return job;
		}

		// Token: 0x06008124 RID: 33060 RVA: 0x0026DE98 File Offset: 0x0026C098
		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			return this.Target(pawn).Cell;
		}

		// Token: 0x06008125 RID: 33061 RVA: 0x0026DE00 File Offset: 0x0026C000
		protected override void DecorateGotoJob(Job job)
		{
			job.expiryInterval = 120;
			job.expireRequiresEnemiesNearby = true;
		}
	}
}
