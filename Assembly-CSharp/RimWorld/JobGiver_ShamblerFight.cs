using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020014CD RID: 5325
	public class JobGiver_ShamblerFight : JobGiver_AIFightEnemy
	{
		// Token: 0x170013A5 RID: 5029
		// (get) Token: 0x06007F58 RID: 32600 RVA: 0x002657E2 File Offset: 0x002639E2
		protected override IntRange ExpiryInterval_Melee
		{
			get
			{
				return JobGiver_ShamblerFight.CheckForBetterTargetInterval;
			}
		}

		// Token: 0x170013A6 RID: 5030
		// (get) Token: 0x06007F59 RID: 32601 RVA: 0x002657E9 File Offset: 0x002639E9
		protected override int TicksSinceEngageToLoseTarget
		{
			get
			{
				return 4200;
			}
		}

		// Token: 0x170013A7 RID: 5031
		// (get) Token: 0x06007F5A RID: 32602 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool DisableAbilityVerbs
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06007F5B RID: 32603 RVA: 0x002657F0 File Offset: 0x002639F0
		public JobGiver_ShamblerFight()
		{
			this.targetAcquireRadius = 20f;
			this.targetKeepRadius = 40f;
		}

		// Token: 0x06007F5C RID: 32604 RVA: 0x0026580E File Offset: 0x00263A0E
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (pawn.mindState.enemyTarget == null)
			{
				return null;
			}
			return base.TryGiveJob(pawn);
		}

		// Token: 0x06007F5D RID: 32605 RVA: 0x00265828 File Offset: 0x00263A28
		protected override void UpdateEnemyTarget(Pawn pawn)
		{
			Thing enemyTarget = pawn.mindState.enemyTarget;
			if (enemyTarget == null)
			{
				return;
			}
			Thing thing = base.FindAttackTargetIfPossible(pawn);
			if (thing != null && pawn.Position.DistanceTo(thing.Position) < pawn.Position.DistanceTo(enemyTarget.Position) / 2f)
			{
				pawn.mindState.enemyTarget = thing;
				pawn.mindState.Notify_EngagedTarget();
			}
			if (this.ShouldLoseTarget(pawn))
			{
				pawn.mindState.enemyTarget = thing;
			}
			Pawn pawn2 = thing as Pawn;
			if (pawn2 != null && thing.Faction == Faction.OfPlayer && pawn.Position.InHorDistOf(thing.Position, 40f) && !pawn2.IsShambler && !pawn.IsPsychologicallyInvisible())
			{
				Find.TickManager.slower.SignalForceNormalSpeed();
			}
		}

		// Token: 0x06007F5E RID: 32606 RVA: 0x002658F7 File Offset: 0x00263AF7
		protected override Thing FindAttackTarget(Pawn pawn)
		{
			return MutantUtility.FindShamblerTarget(pawn);
		}

		// Token: 0x06007F5F RID: 32607 RVA: 0x00265900 File Offset: 0x00263B00
		protected override Job MeleeAttackJob(Pawn pawn, Thing enemyTarget)
		{
			if (pawn.Faction == Faction.OfPlayer)
			{
				return base.MeleeAttackJob(pawn, enemyTarget);
			}
			Job result;
			using (PawnPath pawnPath = pawn.Map.pathFinder.FindPathNow(pawn.Position, enemyTarget.Position, TraverseParms.For(TraverseMode.PassAllDestroyablePlayerOwnedThings, Danger.Deadly, false, false, false, true, false), new PathFinderCostTuning?(JobGiver_ShamblerFight.tuning), PathEndMode.OnCell, null))
			{
				if (!pawnPath.Found)
				{
					Job job = JobMaker.MakeJob(JobDefOf.Wait);
					job.expiryInterval = JobGiver_ShamblerFight.CheckForBetterTargetInterval.RandomInRange;
					result = job;
				}
				else
				{
					IntVec3 intVec;
					Thing thing = pawnPath.FirstBlockingBuilding(out intVec, pawn);
					result = base.MeleeAttackJob(pawn, thing ?? enemyTarget);
				}
			}
			return result;
		}

		// Token: 0x06007F60 RID: 32608 RVA: 0x002659BC File Offset: 0x00263BBC
		protected override bool ShouldLoseTarget(Pawn pawn)
		{
			Thing enemyTarget = pawn.mindState.enemyTarget;
			return enemyTarget == null || enemyTarget.Destroyed || !enemyTarget.Spawned || enemyTarget.Map != pawn.Map || ((IAttackTarget)enemyTarget).ThreatDisabled(pawn) || (Find.TickManager.TicksGame - pawn.mindState.lastEngageTargetTick > this.TicksSinceEngageToLoseTarget && !pawn.CanReach(enemyTarget, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.PassAllDestroyablePlayerOwnedThings));
		}

		// Token: 0x06007F61 RID: 32609 RVA: 0x00262D8D File Offset: 0x00260F8D
		protected override bool TryFindShootingPosition(Pawn pawn, out IntVec3 dest, Verb verbToUse = null)
		{
			dest = IntVec3.Invalid;
			return false;
		}

		// Token: 0x04005540 RID: 21824
		private static readonly IntRange CheckForBetterTargetInterval = new IntRange(240, 540);

		// Token: 0x04005541 RID: 21825
		private static readonly PathFinderCostTuning tuning = new PathFinderCostTuning
		{
			costBlockedDoor = 75,
			costBlockedWallBase = 75,
			costBlockedWallExtraForNaturalWalls = 0,
			costBlockedDoorPerHitPoint = 0f,
			costBlockedWallExtraPerHitPoint = 0f
		};
	}
}
