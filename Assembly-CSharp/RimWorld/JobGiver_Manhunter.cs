using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200152F RID: 5423
	public class JobGiver_Manhunter : ThinkNode_JobGiver
	{
		// Token: 0x060080F0 RID: 33008 RVA: 0x0026D3A4 File Offset: 0x0026B5A4
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (pawn.TryGetAttackVerb(null, false, false) == null)
			{
				return null;
			}
			bool fenceBlocked = pawn.def.race.FenceBlocked;
			Pawn pawn2 = this.FindPawnTarget(pawn, fenceBlocked, this.canBashDoors);
			if (pawn2 != null && pawn.CanReach(pawn2, PathEndMode.Touch, Danger.Deadly, false, fenceBlocked, TraverseMode.ByPawn))
			{
				Job abilityJob = JobGiver_AIFightEnemy.GetAbilityJob(pawn, pawn2);
				if (abilityJob != null)
				{
					return abilityJob;
				}
				return this.MeleeAttackJob(pawn2, fenceBlocked, this.canBashDoors);
			}
			else
			{
				Building building = this.FindTurretTarget(pawn, fenceBlocked);
				if (building != null)
				{
					return this.MeleeAttackJob(building, fenceBlocked, this.canBashDoors);
				}
				if (pawn2 != null)
				{
					using (PawnPath pawnPath = pawn.Map.pathFinder.FindPathNow(pawn.Position, pawn2.Position, TraverseParms.For(pawn, Danger.Deadly, this.canBashDoors ? TraverseMode.PassDoors : TraverseMode.NoPassClosedDoors, false, false, false, true), null, PathEndMode.OnCell, null))
					{
						if (!pawnPath.Found)
						{
							return null;
						}
						IntVec3 loc;
						if (!pawnPath.TryFindLastCellBeforeBlockingDoor(pawn, out loc))
						{
							if (pawn2.Position.GetDoor(pawn2.Map) == null)
							{
								Log.Error(((pawn != null) ? pawn.ToString() : null) + " did TryFindLastCellBeforeDoor but found none when it should have been one. Target: " + pawn2.LabelCap);
								return null;
							}
							loc = pawnPath.NodesReversed[1];
						}
						IntVec3 randomCell = CellFinder.RandomRegionNear(loc.GetRegion(pawn.Map, RegionType.Set_Passable), 9, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), null, null, RegionType.Set_Passable).RandomCell;
						if (randomCell == pawn.Position)
						{
							return JobMaker.MakeJob(JobDefOf.Wait, 30, false);
						}
						return JobMaker.MakeJob(JobDefOf.Goto, randomCell);
					}
				}
				return null;
			}
		}

		// Token: 0x060080F1 RID: 33009 RVA: 0x0026D564 File Offset: 0x0026B764
		private Job MeleeAttackJob(Thing target, bool canBashFences, bool canBashDoors)
		{
			Job job = JobMaker.MakeJob(JobDefOf.AttackMelee, target);
			job.maxNumMeleeAttacks = 1;
			job.expiryInterval = Rand.Range(420, 900);
			job.attackDoorIfTargetLost = canBashDoors;
			job.canBashFences = canBashFences;
			return job;
		}

		// Token: 0x060080F2 RID: 33010 RVA: 0x0026D5A0 File Offset: 0x0026B7A0
		private Pawn FindPawnTarget(Pawn pawn, bool canBashFences, bool canBashDoors)
		{
			return (Pawn)AttackTargetFinder.BestAttackTarget(pawn, TargetScanFlags.NeedThreat | TargetScanFlags.NeedAutoTargetable, (Thing x) => x is Pawn && x.def.race.intelligence >= Intelligence.ToolUser, 0f, 9999f, default(IntVec3), float.MaxValue, canBashDoors, true, canBashFences, false);
		}

		// Token: 0x060080F3 RID: 33011 RVA: 0x0026D5FC File Offset: 0x0026B7FC
		private Building FindTurretTarget(Pawn pawn, bool canBashFences)
		{
			return (Building)AttackTargetFinder.BestAttackTarget(pawn, TargetScanFlags.NeedLOSToPawns | TargetScanFlags.NeedLOSToNonPawns | TargetScanFlags.NeedReachable | TargetScanFlags.NeedThreat | TargetScanFlags.NeedAutoTargetable, (Thing t) => t is Building, 0f, 70f, default(IntVec3), float.MaxValue, false, true, canBashFences, false);
		}

		// Token: 0x04005616 RID: 22038
		public bool canBashDoors = true;

		// Token: 0x04005617 RID: 22039
		private const float WaitChance = 0.75f;

		// Token: 0x04005618 RID: 22040
		private const int WaitTicks = 90;

		// Token: 0x04005619 RID: 22041
		private const int MinMeleeChaseTicks = 420;

		// Token: 0x0400561A RID: 22042
		private const int MaxMeleeChaseTicks = 900;

		// Token: 0x0400561B RID: 22043
		private const int WanderOutsideDoorRegions = 9;
	}
}
