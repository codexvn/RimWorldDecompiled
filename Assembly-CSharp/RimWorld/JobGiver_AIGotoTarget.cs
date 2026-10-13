using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200140B RID: 5131
	public class JobGiver_AIGotoTarget : ThinkNode_JobGiver
	{
		// Token: 0x06007D04 RID: 32004 RVA: 0x0025A287 File Offset: 0x00258487
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_AIGotoTarget jobGiver_AIGotoTarget = (JobGiver_AIGotoTarget)base.DeepCopy(resolve);
			jobGiver_AIGotoTarget.ignoreNonCombatants = this.ignoreNonCombatants;
			jobGiver_AIGotoTarget.humanlikesOnly = this.humanlikesOnly;
			jobGiver_AIGotoTarget.overrideExpiryInterval = this.overrideExpiryInterval;
			return jobGiver_AIGotoTarget;
		}

		// Token: 0x06007D05 RID: 32005 RVA: 0x0025A2BC File Offset: 0x002584BC
		protected override Job TryGiveJob(Pawn pawn)
		{
			Pawn_MindState mindState = pawn.mindState;
			Thing thing = (mindState != null) ? mindState.enemyTarget : null;
			if (thing == null)
			{
				return null;
			}
			if (thing.PositionHeld == pawn.PositionHeld || pawn.CanReachImmediate(thing, PathEndMode.Touch))
			{
				return null;
			}
			if (!this.IsTargetStillValid(thing, pawn))
			{
				pawn.mindState.enemyTarget = null;
				return null;
			}
			Job job = JobMaker.MakeJob(JobDefOf.Goto, thing);
			if (this.overrideExpiryInterval > 0)
			{
				job.expiryInterval = this.overrideExpiryInterval;
			}
			else
			{
				job.intervalScalingTarget = TargetIndex.A;
			}
			job.checkOverrideOnExpire = true;
			job.collideWithPawns = true;
			return job;
		}

		// Token: 0x06007D06 RID: 32006 RVA: 0x0025A35C File Offset: 0x0025855C
		private bool IsTargetStillValid(Thing target, Pawn pawn)
		{
			Pawn pawn2 = target as Pawn;
			return target != null && (pawn2 == null || !pawn2.ThreatDisabled(pawn)) && pawn.CanReach(target, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn);
		}

		// Token: 0x04005423 RID: 21539
		private bool ignoreNonCombatants;

		// Token: 0x04005424 RID: 21540
		private bool humanlikesOnly;

		// Token: 0x04005425 RID: 21541
		private int overrideExpiryInterval = -1;
	}
}
