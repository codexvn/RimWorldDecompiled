using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001419 RID: 5145
	public class JobGiver_AIWaitAmbush : ThinkNode_JobGiver
	{
		// Token: 0x06007D37 RID: 32055 RVA: 0x0025B4A8 File Offset: 0x002596A8
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_AIWaitAmbush jobGiver_AIWaitAmbush = (JobGiver_AIWaitAmbush)base.DeepCopy(resolve);
			jobGiver_AIWaitAmbush.ignoreNonCombatants = this.ignoreNonCombatants;
			jobGiver_AIWaitAmbush.humanlikesOnly = this.humanlikesOnly;
			return jobGiver_AIWaitAmbush;
		}

		// Token: 0x06007D38 RID: 32056 RVA: 0x0025B4D0 File Offset: 0x002596D0
		protected override Job TryGiveJob(Pawn pawn)
		{
			Thing thing = this.FindBestTarget(pawn);
			if (thing == null)
			{
				return null;
			}
			Job result;
			using (PawnPath pawnPath = pawn.Map.pathFinder.FindPathNow(pawn.Position, thing.Position, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.PassDoors, false, false, false, true), null, PathEndMode.OnCell, null))
			{
				if (!pawnPath.Found)
				{
					result = null;
				}
				else
				{
					IntVec3 loc;
					if (!pawnPath.TryFindLastCellBeforeBlockingDoor(pawn, out loc))
					{
						if (thing.Position.GetDoor(thing.Map) == null)
						{
							Log.Error(((pawn != null) ? pawn.ToString() : null) + " did TryFindLastCellBeforeDoor but found none when it should have been one. Target: " + thing.LabelCap);
							return null;
						}
						loc = pawnPath.NodesReversed[1];
					}
					IntVec3 randomCell = CellFinder.RandomRegionNear(loc.GetRegion(pawn.Map, RegionType.Set_Passable), 4, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), null, null, RegionType.Set_Passable).RandomCell;
					Job job;
					if (randomCell == pawn.Position)
					{
						job = JobMaker.MakeJob(JobDefOf.Wait, 30, false);
					}
					pawn.mindState.nextMoveOrderIsWait = !pawn.mindState.nextMoveOrderIsWait;
					if (pawn.mindState.nextMoveOrderIsWait)
					{
						job = JobMaker.MakeJob(JobDefOf.Wait, 300, false);
					}
					else
					{
						job = JobMaker.MakeJob(JobDefOf.Goto, randomCell);
					}
					if (this.expireOnNearbyEnemy)
					{
						job.expiryInterval = 30;
						job.checkOverrideOnExpire = true;
					}
					result = job;
				}
			}
			return result;
		}

		// Token: 0x06007D39 RID: 32057 RVA: 0x0025B668 File Offset: 0x00259868
		private Thing FindBestTarget(Pawn pawn)
		{
			float num = float.MaxValue;
			Thing result = null;
			List<IAttackTarget> potentialTargetsFor = pawn.Map.attackTargetsCache.GetPotentialTargetsFor(pawn);
			for (int i = 0; i < potentialTargetsFor.Count; i++)
			{
				IAttackTarget attackTarget = potentialTargetsFor[i];
				if (!attackTarget.ThreatDisabled(pawn) && AttackTargetFinder.IsAutoTargetable(attackTarget))
				{
					if (this.humanlikesOnly)
					{
						Pawn pawn2 = attackTarget as Pawn;
						if (pawn2 != null && !pawn2.RaceProps.Humanlike)
						{
							goto IL_DE;
						}
					}
					Pawn pawn3 = attackTarget.Thing as Pawn;
					if (pawn3 == null || pawn3.IsCombatant() || (!this.ignoreNonCombatants && GenSight.LineOfSightToThing(pawn.Position, pawn3, pawn.Map, false, null)))
					{
						Thing thing = (Thing)attackTarget;
						int num2 = thing.Position.DistanceToSquared(pawn.Position);
						if ((float)num2 < num && pawn.CanReach(thing, PathEndMode.OnCell, Danger.Deadly, false, false, TraverseMode.PassDoors))
						{
							num = (float)num2;
							result = thing;
						}
					}
				}
				IL_DE:;
			}
			return result;
		}

		// Token: 0x0400543C RID: 21564
		private const int WanderOutsideDoorRegions = 4;

		// Token: 0x0400543D RID: 21565
		private const int WaitTicks = 300;

		// Token: 0x0400543E RID: 21566
		private const int EnemyScanCheckTicks = 30;

		// Token: 0x0400543F RID: 21567
		private bool ignoreNonCombatants;

		// Token: 0x04005440 RID: 21568
		private bool humanlikesOnly;

		// Token: 0x04005441 RID: 21569
		protected bool expireOnNearbyEnemy;
	}
}
