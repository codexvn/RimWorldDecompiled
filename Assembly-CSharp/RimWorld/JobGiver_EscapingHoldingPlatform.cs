using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001528 RID: 5416
	public class JobGiver_EscapingHoldingPlatform : JobGiver_AIFightEnemies
	{
		// Token: 0x060080DE RID: 32990 RVA: 0x0026CFB1 File Offset: 0x0026B1B1
		public JobGiver_EscapingHoldingPlatform()
		{
			this.chaseTarget = true;
		}

		// Token: 0x060080DF RID: 32991 RVA: 0x0026CFC0 File Offset: 0x0026B1C0
		protected override Job MeleeAttackJob(Pawn pawn, Thing enemyTarget)
		{
			Job job = base.MeleeAttackJob(pawn, enemyTarget);
			job.attackDoorIfTargetLost = true;
			job.canBashDoors = true;
			return job;
		}

		// Token: 0x060080E0 RID: 32992 RVA: 0x0026CFD8 File Offset: 0x0026B1D8
		protected override bool ShouldLoseTarget(Pawn pawn)
		{
			Thing enemyTarget = pawn.mindState.enemyTarget;
			return enemyTarget.Destroyed || ((IAttackTarget)enemyTarget).ThreatDisabled(pawn);
		}
	}
}
