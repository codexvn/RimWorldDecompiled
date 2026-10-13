using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001414 RID: 5140
	public static class DigUtility
	{
		// Token: 0x06007D23 RID: 32035 RVA: 0x0025ABC4 File Offset: 0x00258DC4
		public static Job PassBlockerJob(Pawn pawn, Thing blocker, IntVec3 cellBeforeBlocker, bool canMineMineables, bool canMineNonMineables)
		{
			if (StatDefOf.MiningSpeed.Worker.IsDisabledFor(pawn))
			{
				canMineMineables = false;
				canMineNonMineables = false;
			}
			if (blocker.def.mineable)
			{
				if (canMineMineables)
				{
					return DigUtility.MineOrWaitJob(pawn, blocker, cellBeforeBlocker);
				}
				return DigUtility.MeleeOrWaitJob(pawn, blocker, cellBeforeBlocker);
			}
			else
			{
				if (pawn.equipment != null && pawn.equipment.Primary != null)
				{
					Verb primaryVerb = pawn.equipment.PrimaryEq.PrimaryVerb;
					if (primaryVerb.verbProps.ai_IsBuildingDestroyer && (!primaryVerb.IsIncendiary_Ranged() || blocker.FlammableNow))
					{
						Job job = JobMaker.MakeJob(JobDefOf.UseVerbOnThing);
						job.targetA = blocker;
						job.verbToUse = primaryVerb;
						job.expiryInterval = JobGiver_AIFightEnemy.ExpiryInterval_ShooterSucceeded.RandomInRange;
						return job;
					}
				}
				if (canMineNonMineables)
				{
					return DigUtility.MineOrWaitJob(pawn, blocker, cellBeforeBlocker);
				}
				return DigUtility.MeleeOrWaitJob(pawn, blocker, cellBeforeBlocker);
			}
		}

		// Token: 0x06007D24 RID: 32036 RVA: 0x0025AC94 File Offset: 0x00258E94
		private static Job MeleeOrWaitJob(Pawn pawn, Thing blocker, IntVec3 cellBeforeBlocker)
		{
			if (!pawn.CanReserve(blocker, 1, -1, null, false))
			{
				return DigUtility.WaitNearJob(pawn, cellBeforeBlocker);
			}
			Job job = JobMaker.MakeJob(JobDefOf.AttackMelee, blocker);
			job.ignoreDesignations = true;
			job.expiryInterval = JobGiver_AIFightEnemy.ExpiryInterval_ShooterSucceeded.RandomInRange;
			job.checkOverrideOnExpire = true;
			return job;
		}

		// Token: 0x06007D25 RID: 32037 RVA: 0x0025ACEC File Offset: 0x00258EEC
		private static Job MineOrWaitJob(Pawn pawn, Thing blocker, IntVec3 cellBeforeBlocker)
		{
			if (!pawn.CanReserve(blocker, 1, -1, null, false))
			{
				return DigUtility.WaitNearJob(pawn, cellBeforeBlocker);
			}
			Job job = JobMaker.MakeJob(JobDefOf.Mine, blocker);
			job.ignoreDesignations = true;
			job.expiryInterval = JobGiver_AIFightEnemy.ExpiryInterval_ShooterSucceeded.RandomInRange;
			job.checkOverrideOnExpire = true;
			return job;
		}

		// Token: 0x06007D26 RID: 32038 RVA: 0x0025AD44 File Offset: 0x00258F44
		private static Job WaitNearJob(Pawn pawn, IntVec3 cellBeforeBlocker)
		{
			IntVec3 intVec = CellFinder.RandomClosewalkCellNear(cellBeforeBlocker, pawn.Map, 10, null);
			if (intVec == pawn.Position)
			{
				return JobMaker.MakeJob(JobDefOf.Wait, 20, true);
			}
			return JobMaker.MakeJob(JobDefOf.Goto, intVec, 500, true);
		}

		// Token: 0x04005436 RID: 21558
		private const int CheckOverrideInterval = 500;
	}
}
