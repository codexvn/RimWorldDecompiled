using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020013CA RID: 5066
	public class JobGiver_HiveDefense : JobGiver_AIFightEnemies
	{
		// Token: 0x06007C11 RID: 31761 RVA: 0x00255DD0 File Offset: 0x00253FD0
		protected override IntVec3 GetFlagPosition(Pawn pawn)
		{
			Hive hive = pawn.mindState.duty.focus.Thing as Hive;
			if (hive != null && hive.Spawned)
			{
				return hive.Position;
			}
			return pawn.Position;
		}

		// Token: 0x06007C12 RID: 31762 RVA: 0x00255E10 File Offset: 0x00254010
		protected override float GetFlagRadius(Pawn pawn)
		{
			return pawn.mindState.duty.radius;
		}

		// Token: 0x06007C13 RID: 31763 RVA: 0x00255E22 File Offset: 0x00254022
		protected override Job MeleeAttackJob(Pawn pawn, Thing enemyTarget)
		{
			Job job = base.MeleeAttackJob(pawn, enemyTarget);
			job.attackDoorIfTargetLost = true;
			return job;
		}
	}
}
