using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020014D0 RID: 5328
	public class JobGiver_SightstealerAttack : ThinkNode_JobGiver
	{
		// Token: 0x06007F6A RID: 32618 RVA: 0x00265B69 File Offset: 0x00263D69
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_SightstealerAttack jobGiver_SightstealerAttack = (JobGiver_SightstealerAttack)base.DeepCopy(resolve);
			jobGiver_SightstealerAttack.targetAcquireRadius = this.targetAcquireRadius;
			return jobGiver_SightstealerAttack;
		}

		// Token: 0x06007F6B RID: 32619 RVA: 0x00265B84 File Offset: 0x00263D84
		protected override Job TryGiveJob(Pawn pawn)
		{
			Pawn pawn2 = this.FindPawnTarget(pawn);
			if (pawn2 != null)
			{
				return this.MeleeAttackJob(pawn2, false);
			}
			return null;
		}

		// Token: 0x06007F6C RID: 32620 RVA: 0x00265BA6 File Offset: 0x00263DA6
		private Job MeleeAttackJob(Thing target, bool canBashFences)
		{
			Job job = JobMaker.MakeJob(JobDefOf.AttackMelee, target);
			job.maxNumMeleeAttacks = 1;
			job.expiryInterval = Rand.Range(420, 900);
			job.attackDoorIfTargetLost = true;
			job.canBashFences = canBashFences;
			return job;
		}

		// Token: 0x06007F6D RID: 32621 RVA: 0x00265BE4 File Offset: 0x00263DE4
		private Pawn FindPawnTarget(Pawn pawn)
		{
			return (Pawn)AttackTargetFinder.BestAttackTarget(pawn, TargetScanFlags.NeedThreat | TargetScanFlags.NeedAutoTargetable, (Thing thing) => !thing.Isnt<Pawn>() && thing.def.race.intelligence >= Intelligence.ToolUser && (this.targetAcquireRadius <= 0 || pawn.Position.DistanceTo(thing.PositionHeld) <= (float)this.targetAcquireRadius), 0f, 9999f, default(IntVec3), float.MaxValue, false, true, false, false);
		}

		// Token: 0x04005544 RID: 21828
		private const int MinMeleeChaseTicks = 420;

		// Token: 0x04005545 RID: 21829
		private const int MaxMeleeChaseTicks = 900;

		// Token: 0x04005546 RID: 21830
		private int targetAcquireRadius = -1;
	}
}
