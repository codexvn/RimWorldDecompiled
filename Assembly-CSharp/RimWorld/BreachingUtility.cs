using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020013F5 RID: 5109
	public static class BreachingUtility
	{
		// Token: 0x06007C92 RID: 31890 RVA: 0x002584AC File Offset: 0x002566AC
		public static bool ShouldBreachBuilding(Thing thing)
		{
			Building building = thing as Building;
			return building != null && TrashUtility.ShouldTrashBuilding(building) && PathUtility.IsDestroyable(building) && building.def.IsEdifice() && !building.def.IsFrame && (building.def.mineable || ((building.Faction == null || building.Faction == Faction.OfPlayer) && BreachingUtility.BlocksBreaching(building.Map, building.Position)));
		}

		// Token: 0x06007C93 RID: 31891 RVA: 0x00258530 File Offset: 0x00256730
		public static bool IsWorthBreachingBuilding(BreachingGrid grid, Building b)
		{
			if (b.def.passability != Traversability.Impassable || b.def.IsDoor)
			{
				return true;
			}
			if (BreachingUtility.OnEdgeOfPath(b.Map, grid.BreachGrid, b.Position))
			{
				if (BreachingUtility.OnEdgeOfPathAndSufficientlyClear(grid, b.Position, IntVec3.North) || BreachingUtility.OnEdgeOfPathAndSufficientlyClear(grid, b.Position, IntVec3.East) || BreachingUtility.OnEdgeOfPathAndSufficientlyClear(grid, b.Position, IntVec3.South) || BreachingUtility.OnEdgeOfPathAndSufficientlyClear(grid, b.Position, IntVec3.West))
				{
					return false;
				}
			}
			else if (!BreachingUtility.AnyAdjacentImpassibleOnBreachPath(b.Map, grid.BreachGrid, b.Position))
			{
				return false;
			}
			return true;
		}

		// Token: 0x06007C94 RID: 31892 RVA: 0x002585E0 File Offset: 0x002567E0
		public static int CountReachableAdjacentCells(BreachingGrid grid, Building b)
		{
			int num = 0;
			for (int i = 0; i < 4; i++)
			{
				IntVec3 c = b.Position + GenAdj.CardinalDirections[i];
				if (c.InBounds(grid.Map) && grid.ReachableGrid[c])
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x06007C95 RID: 31893 RVA: 0x00258634 File Offset: 0x00256834
		private static bool OnEdgeOfPath(Map map, BoolGrid grid, IntVec3 p)
		{
			for (int i = 0; i < 4; i++)
			{
				IntVec3 c = p + GenAdj.CardinalDirections[i];
				if (c.InBounds(map) && !grid[c])
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06007C96 RID: 31894 RVA: 0x00258674 File Offset: 0x00256874
		private static bool OnEdgeOfPathAndSufficientlyClear(BreachingGrid breachingGrid, IntVec3 p, IntVec3 right)
		{
			Map map = breachingGrid.Map;
			BoolGrid breachGrid = breachingGrid.BreachGrid;
			int num = breachingGrid.BreachRadius * 2 + 1;
			IntVec3 c = p - right;
			if (!c.InBounds(map) || breachGrid[c])
			{
				return false;
			}
			IntVec3 intVec = p;
			int num2 = 0;
			while (intVec.InBounds(map) && breachGrid[intVec] && BreachingUtility.BlocksBreaching(map, intVec) && num2 <= num)
			{
				intVec += right;
				num2++;
			}
			int num3 = 0;
			while (intVec.InBounds(map) && breachGrid[intVec] && !BreachingUtility.BlocksBreaching(map, intVec) && num2 <= num)
			{
				intVec += right;
				num2++;
				num3++;
			}
			int num4 = Math.Max(2, num - 2);
			return num3 >= num4;
		}

		// Token: 0x06007C97 RID: 31895 RVA: 0x00258744 File Offset: 0x00256944
		public static bool BlocksBreaching(Map map, IntVec3 c)
		{
			Building edifice = c.GetEdifice(map);
			return (edifice == null || !edifice.def.building.ai_neverTrashThis) && (c.Impassable(map) || c.GetDoor(map) != null);
		}

		// Token: 0x06007C98 RID: 31896 RVA: 0x00258788 File Offset: 0x00256988
		private static bool AnyAdjacentImpassibleOnBreachPath(Map map, BoolGrid grid, IntVec3 position)
		{
			for (int i = 0; i < 8; i++)
			{
				IntVec3 c = position + GenAdj.AdjacentCellsAround[i];
				if (c.InBounds(map) && grid[c] && BreachingUtility.BlocksBreaching(map, c))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06007C99 RID: 31897 RVA: 0x002587D1 File Offset: 0x002569D1
		public static bool CanSoloAttackTargetBuilding(Thing thing)
		{
			return thing != null && thing.Faction == Faction.OfPlayer;
		}

		// Token: 0x06007C9A RID: 31898 RVA: 0x002587E8 File Offset: 0x002569E8
		public static bool IsSoloAttackVerb(Verb verb)
		{
			return verb.verbProps.ai_AvoidFriendlyFireRadius > 0f;
		}

		// Token: 0x06007C9B RID: 31899 RVA: 0x002587FC File Offset: 0x002569FC
		public static float EscortRadius(Pawn pawn)
		{
			float randomInRange;
			if (pawn.equipment != null && pawn.equipment.Primary != null && pawn.equipment.Primary.def.IsRangedWeapon)
			{
				randomInRange = BreachingUtility.EscortRadiusRanged.RandomInRange;
			}
			else
			{
				randomInRange = BreachingUtility.EscortRadiusMelee.RandomInRange;
			}
			return randomInRange;
		}

		// Token: 0x06007C9C RID: 31900 RVA: 0x00258854 File Offset: 0x00256A54
		private static bool UsableVerb(Verb verb)
		{
			return verb != null && verb.Available() && verb.HarmsHealth();
		}

		// Token: 0x06007C9D RID: 31901 RVA: 0x00258870 File Offset: 0x00256A70
		public static Verb FindVerbToUseForBreaching(Pawn pawn)
		{
			Pawn_EquipmentTracker equipment = pawn.equipment;
			CompEquippable compEquippable = (equipment != null) ? equipment.PrimaryEq : null;
			if (compEquippable == null)
			{
				return null;
			}
			Verb primaryVerb = compEquippable.PrimaryVerb;
			if (BreachingUtility.UsableVerb(primaryVerb) && primaryVerb.verbProps.ai_IsBuildingDestroyer)
			{
				return primaryVerb;
			}
			List<Verb> allVerbs = compEquippable.AllVerbs;
			for (int i = 0; i < allVerbs.Count; i++)
			{
				Verb verb = allVerbs[i];
				if (BreachingUtility.UsableVerb(verb) && verb.verbProps.ai_IsBuildingDestroyer)
				{
					return verb;
				}
			}
			if (BreachingUtility.UsableVerb(primaryVerb))
			{
				return primaryVerb;
			}
			return null;
		}

		// Token: 0x06007C9E RID: 31902 RVA: 0x002588FC File Offset: 0x00256AFC
		public static void FinalizeTrashJob(Job job)
		{
			job.expiryInterval = BreachingUtility.TrashJobCheckOverrideInterval.RandomInRange;
			job.checkOverrideOnExpire = true;
			job.expireRequiresEnemiesNearby = true;
		}

		// Token: 0x06007C9F RID: 31903 RVA: 0x0025892A File Offset: 0x00256B2A
		public static BreachingGrid BreachingGridFor(Pawn pawn)
		{
			return BreachingUtility.BreachingGridFor((pawn != null) ? pawn.GetLord() : null);
		}

		// Token: 0x06007CA0 RID: 31904 RVA: 0x0025893D File Offset: 0x00256B3D
		public static BreachingGrid BreachingGridFor(Lord lord)
		{
			LordToilData_AssaultColonyBreaching lordToilData_AssaultColonyBreaching = BreachingUtility.LordDataFor(lord);
			if (lordToilData_AssaultColonyBreaching == null)
			{
				return null;
			}
			return lordToilData_AssaultColonyBreaching.breachingGrid;
		}

		// Token: 0x06007CA1 RID: 31905 RVA: 0x00258950 File Offset: 0x00256B50
		public static LordToilData_AssaultColonyBreaching LordDataFor(Lord lord)
		{
			LordToil_AssaultColonyBreaching lordToil_AssaultColonyBreaching = ((lord != null) ? lord.CurLordToil : null) as LordToil_AssaultColonyBreaching;
			if (lordToil_AssaultColonyBreaching == null)
			{
				return null;
			}
			return lordToil_AssaultColonyBreaching.Data;
		}

		// Token: 0x06007CA2 RID: 31906 RVA: 0x0025896E File Offset: 0x00256B6E
		public static LordToil_AssaultColonyBreaching LordToilOf(Pawn pawn)
		{
			return ((pawn != null) ? pawn.GetLord().CurLordToil : null) as LordToil_AssaultColonyBreaching;
		}

		// Token: 0x06007CA3 RID: 31907 RVA: 0x00258986 File Offset: 0x00256B86
		public static bool TryFindCastPosition(Pawn pawn, Verb verb, Thing target, out IntVec3 result)
		{
			if (verb.IsMeleeAttack)
			{
				return BreachingUtility.TryFindMeleeCastPosition(pawn, verb, target, out result);
			}
			return BreachingUtility.cachedRangedCastPositionFinder.TryFindRangedCastPosition(pawn, verb, target, out result);
		}

		// Token: 0x06007CA4 RID: 31908 RVA: 0x002589A8 File Offset: 0x00256BA8
		private static bool TryFindMeleeCastPosition(Pawn pawn, Verb verb, Thing target, out IntVec3 result)
		{
			result = IntVec3.Invalid;
			BreachingGrid breachingGrid = BreachingUtility.BreachingGridFor(pawn);
			if (breachingGrid == null)
			{
				return false;
			}
			if (BreachingUtility.SafeUseableFiringPosition(breachingGrid, pawn.Position) && pawn.CanReachImmediate(target, PathEndMode.Touch))
			{
				result = pawn.Position;
				return true;
			}
			for (int i = 0; i < 8; i++)
			{
				IntVec3 intVec = GenAdj.AdjacentCells[i] + target.Position;
				if (intVec.InBounds(target.Map) && BreachingUtility.SafeUseableFiringPosition(breachingGrid, intVec) && pawn.CanReach(intVec, PathEndMode.OnCell, Danger.Deadly, false, false, TraverseMode.ByPawn))
				{
					result = intVec;
					return true;
				}
			}
			return false;
		}

		// Token: 0x06007CA5 RID: 31909 RVA: 0x00258A4E File Offset: 0x00256C4E
		private static bool SafeUseableFiringPosition(BreachingGrid grid, IntVec3 c)
		{
			return grid.ReachableGrid[c] && grid.MarkerGrid[c] != 180 && !c.ContainsStaticFire(grid.Map);
		}

		// Token: 0x06007CA6 RID: 31910 RVA: 0x00258A88 File Offset: 0x00256C88
		public static Pawn FindPawnToEscort(Pawn follower)
		{
			Lord lord = follower.GetLord();
			if (lord == null)
			{
				return null;
			}
			Pawn result = null;
			for (int i = 0; i < lord.ownedPawns.Count; i++)
			{
				Pawn pawn = lord.ownedPawns[i];
				if (pawn != follower)
				{
					PawnDuty duty = pawn.mindState.duty;
					if (((duty != null) ? duty.def : null) == DutyDefOf.Breaching)
					{
						BreachingTargetData breachingTarget = pawn.mindState.breachingTarget;
						if (breachingTarget != null && breachingTarget.firingPosition.IsValid)
						{
							return pawn;
						}
						result = pawn;
					}
				}
			}
			return result;
		}

		// Token: 0x06007CA7 RID: 31911 RVA: 0x00258B0C File Offset: 0x00256D0C
		public static bool CanDamageTarget(Verb verb, Thing target)
		{
			return verb.GetDamageDef() != DamageDefOf.Flame || target.FlammableNow;
		}

		// Token: 0x06007CA8 RID: 31912 RVA: 0x00258B28 File Offset: 0x00256D28
		public static bool IsGoodBreacher(Pawn pawn)
		{
			Verb verb = BreachingUtility.FindVerbToUseForBreaching(pawn);
			return verb != null && verb.verbProps.ai_IsBuildingDestroyer;
		}

		// Token: 0x040053FB RID: 21499
		public static readonly IntRange TrashJobCheckOverrideInterval = new IntRange(120, 300);

		// Token: 0x040053FC RID: 21500
		private static readonly FloatRange EscortRadiusRanged = new FloatRange(15f, 19f);

		// Token: 0x040053FD RID: 21501
		private static readonly FloatRange EscortRadiusMelee = new FloatRange(23f, 26f);

		// Token: 0x040053FE RID: 21502
		private static BreachingUtility.BreachRangedCastPositionFinder cachedRangedCastPositionFinder = new BreachingUtility.BreachRangedCastPositionFinder();

		// Token: 0x020013F6 RID: 5110
		private class BreachRangedCastPositionFinder
		{
			// Token: 0x06007CAA RID: 31914 RVA: 0x00258B9C File Offset: 0x00256D9C
			public BreachRangedCastPositionFinder()
			{
				this.visitDangerousCellFunc = new Action<IntVec3>(this.VisitDangerousCell);
				this.safeForRangedCastFunc = new Func<IntVec3, bool>(this.SafeForRangedCast);
			}

			// Token: 0x06007CAB RID: 31915 RVA: 0x00258BC8 File Offset: 0x00256DC8
			private void VisitDangerousCell(IntVec3 cell)
			{
				if (this.breachingGrid.MarkerGrid[cell] == 180)
				{
					this.wouldPutSomeoneElseInDanger = true;
				}
			}

			// Token: 0x06007CAC RID: 31916 RVA: 0x00258BEC File Offset: 0x00256DEC
			private bool SafeForRangedCast(IntVec3 c)
			{
				if (!BreachingUtility.SafeUseableFiringPosition(this.breachingGrid, c))
				{
					return false;
				}
				this.wouldPutSomeoneElseInDanger = false;
				this.breachingGrid.VisitDangerousCellsOfAttack(c, this.target.Position, this.verb, this.visitDangerousCellFunc);
				return !this.wouldPutSomeoneElseInDanger;
			}

			// Token: 0x06007CAD RID: 31917 RVA: 0x00258C3C File Offset: 0x00256E3C
			public bool TryFindRangedCastPosition(Pawn pawn, Verb verb, Thing target, out IntVec3 result)
			{
				bool result2;
				try
				{
					result = IntVec3.Invalid;
					LordToilData_AssaultColonyBreaching lordToilData_AssaultColonyBreaching = BreachingUtility.LordDataFor(pawn.GetLord());
					if (lordToilData_AssaultColonyBreaching == null)
					{
						result2 = false;
					}
					else
					{
						this.breachingGrid = lordToilData_AssaultColonyBreaching.breachingGrid;
						this.verb = verb;
						this.target = target;
						CastPositionRequest castPositionRequest = default(CastPositionRequest);
						castPositionRequest.caster = pawn;
						castPositionRequest.target = target;
						castPositionRequest.verb = verb;
						castPositionRequest.maxRangeFromTarget = verb.EffectiveRange;
						if (lordToilData_AssaultColonyBreaching.soloAttacker == null)
						{
							castPositionRequest.maxRangeFromTarget = Mathf.Min(lordToilData_AssaultColonyBreaching.maxRange, castPositionRequest.maxRangeFromTarget);
						}
						castPositionRequest.validator = this.safeForRangedCastFunc;
						IntVec3 intVec;
						if (CastPositionFinder.TryFindCastPosition(castPositionRequest, out intVec))
						{
							result = intVec;
							result2 = true;
						}
						else
						{
							result2 = false;
						}
					}
				}
				finally
				{
				}
				return result2;
			}

			// Token: 0x040053FF RID: 21503
			private BreachingGrid breachingGrid;

			// Token: 0x04005400 RID: 21504
			private Verb verb;

			// Token: 0x04005401 RID: 21505
			private Thing target;

			// Token: 0x04005402 RID: 21506
			private bool wouldPutSomeoneElseInDanger;

			// Token: 0x04005403 RID: 21507
			private Action<IntVec3> visitDangerousCellFunc;

			// Token: 0x04005404 RID: 21508
			private Func<IntVec3, bool> safeForRangedCastFunc;
		}
	}
}
