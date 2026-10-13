using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001437 RID: 5175
	public class JobGiver_Duel : JobGiver_AIFightEnemies
	{
		// Token: 0x1700139C RID: 5020
		// (get) Token: 0x06007D95 RID: 32149 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool DisableAbilityVerbs
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06007D96 RID: 32150 RVA: 0x0025D15C File Offset: 0x0025B35C
		protected override Job TryGiveJob(Pawn pawn)
		{
			Lord lord = pawn.GetLord();
			LordJob_Ritual_Duel lordJob_Ritual_Duel = ((lord != null) ? lord.LordJob : null) as LordJob_Ritual_Duel;
			if (lordJob_Ritual_Duel == null)
			{
				return null;
			}
			lordJob_Ritual_Duel.StartDuelIfNotStartedYet();
			if (lordJob_Ritual_Duel.CurrentDuelStage == DuelBehaviorStage.Attack)
			{
				return base.TryGiveJob(pawn);
			}
			Job job = JobMaker.MakeJob(JobDefOf.Goto, this.GetMoveTarget(pawn, lordJob_Ritual_Duel), lordJob_Ritual_Duel.Opponent(pawn));
			job.checkOverrideOnExpire = true;
			job.expiryInterval = 40;
			job.collideWithPawns = true;
			job.locomotionUrgency = LocomotionUrgency.Sprint;
			return job;
		}

		// Token: 0x06007D97 RID: 32151 RVA: 0x0025D1D8 File Offset: 0x0025B3D8
		private LocalTargetInfo GetMoveTarget(Pawn pawn, LordJob_Ritual_Duel duel)
		{
			Pawn opponent = duel.Opponent(pawn);
			return RCellFinder.RandomWanderDestFor(pawn, duel.selectedTarget.Cell, 3.1f, delegate(Pawn p, IntVec3 c, IntVec3 r)
			{
				if (c == pawn.Position || !c.Standable(p.Map) || !p.CanReserveAndReach(c, PathEndMode.OnCell, Danger.Deadly, 1, -1, null, false) || c.DistanceTo(duel.selectedTarget.Cell) > 3.1f)
				{
					return false;
				}
				Job curJob = opponent.CurJob;
				IntVec3 a = (((curJob != null) ? curJob.def : null) == JobDefOf.Goto) ? opponent.CurJob.targetA.Cell : IntVec3.Invalid;
				if (c.DistanceTo(opponent.Position) < 1.9f || (a != IntVec3.Invalid && a.DistanceTo(c) < 1.9f))
				{
					return false;
				}
				PawnPath pawnPath = pawn.Map.pathFinder.FindPathNow(pawn.Position, c, pawn, null, PathEndMode.OnCell);
				try
				{
					foreach (IntVec3 a2 in pawnPath.NodesReversed)
					{
						if (a2.DistanceTo(opponent.Position) < 1.9f || a2.DistanceTo(duel.selectedTarget.Cell) > 3.1f)
						{
							return false;
						}
					}
					if (a != IntVec3.Invalid && opponent.pather.curPath != null)
					{
						foreach (IntVec3 a3 in opponent.pather.curPath.NodesReversed)
						{
							if (a3.DistanceTo(pawn.Position) < 1.9f || a3.DistanceTo(duel.selectedTarget.Cell) > 3.1f)
							{
								return false;
							}
							foreach (IntVec3 b in pawnPath.NodesReversed)
							{
								if (a3.DistanceTo(b) < 1.9f)
								{
									return false;
								}
							}
						}
					}
				}
				finally
				{
					pawnPath.ReleaseToPool();
				}
				return true;
			}, Danger.Deadly, false);
		}

		// Token: 0x06007D98 RID: 32152 RVA: 0x0025D244 File Offset: 0x0025B444
		protected override void UpdateEnemyTarget(Pawn pawn)
		{
			Pawn pawn2 = ((LordJob_Ritual_Duel)pawn.GetLord().LordJob).Opponent(pawn);
			if (pawn2 == null || pawn2.Dead)
			{
				pawn.mindState.enemyTarget = null;
				return;
			}
			pawn.mindState.enemyTarget = pawn2;
		}

		// Token: 0x06007D99 RID: 32153 RVA: 0x0025D28C File Offset: 0x0025B48C
		protected override Job MeleeAttackJob(Pawn pawn, Thing enemyTarget)
		{
			Job job = base.MeleeAttackJob(pawn, enemyTarget);
			job.killIncappedTarget = true;
			return job;
		}

		// Token: 0x0400545E RID: 21598
		public const float MinDistOpponentWhenMoving = 1.9f;

		// Token: 0x0400545F RID: 21599
		public const float MaxFightMoveDist = 3.1f;
	}
}
