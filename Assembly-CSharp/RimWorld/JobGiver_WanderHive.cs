using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020013CD RID: 5069
	public class JobGiver_WanderHive : JobGiver_Wander
	{
		// Token: 0x06007C1B RID: 31771 RVA: 0x00256023 File Offset: 0x00254223
		public JobGiver_WanderHive()
		{
			this.wanderRadius = 7.5f;
			this.ticksBetweenWandersRange = new IntRange(125, 200);
		}

		// Token: 0x06007C1C RID: 31772 RVA: 0x00256048 File Offset: 0x00254248
		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			Hive hive = pawn.mindState.duty.focus.Thing as Hive;
			if (hive == null || !hive.Spawned)
			{
				return pawn.Position;
			}
			return hive.Position;
		}
	}
}
