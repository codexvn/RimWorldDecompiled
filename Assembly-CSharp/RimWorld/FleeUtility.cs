using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001103 RID: 4355
	public static class FleeUtility
	{
		// Token: 0x06006A13 RID: 27155 RVA: 0x00216664 File Offset: 0x00214864
		public static Job FleeJob(Pawn pawn, Thing danger, int fleeDistance)
		{
			IntVec3 intVec;
			if (pawn.CurJob != null && pawn.CurJob.def == JobDefOf.Flee)
			{
				intVec = pawn.CurJob.targetA.Cell;
			}
			else
			{
				FleeUtility.tmpThings.Clear();
				FleeUtility.tmpThings.Add(danger);
				intVec = CellFinderLoose.GetFleeDest(pawn, FleeUtility.tmpThings, (float)fleeDistance);
				FleeUtility.tmpThings.Clear();
			}
			if (intVec != pawn.Position)
			{
				return JobMaker.MakeJob(JobDefOf.Flee, intVec, danger);
			}
			return null;
		}

		// Token: 0x06006A14 RID: 27156 RVA: 0x002166F4 File Offset: 0x002148F4
		public static Job FleeLargeFireJob(Pawn pawn, int minFiresNearbyToFlee, int distToFireToFlee, int fleeDistance)
		{
			if (pawn.Map.listerThings.ThingsInGroup(ThingRequestGroup.Fire).Count < minFiresNearbyToFlee)
			{
				return null;
			}
			TraverseParms tp = TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true);
			Fire closestFire = null;
			float closestDistSq = -1f;
			int firesCount = 0;
			RegionTraverser.BreadthFirstTraverse(pawn.Position, pawn.Map, (Region from, Region to) => to.Allows(tp, false), delegate(Region x)
			{
				List<Thing> list = x.ListerThings.ThingsInGroup(ThingRequestGroup.Fire);
				int firesCount;
				for (int i = 0; i < list.Count; i++)
				{
					float num = (float)pawn.Position.DistanceToSquared(list[i].Position);
					if (num <= (float)(FleeUtility.MinFiresNearbyRadius * FleeUtility.MinFiresNearbyRadius))
					{
						if (closestFire == null || num < closestDistSq)
						{
							closestDistSq = num;
							closestFire = (Fire)list[i];
						}
						firesCount = firesCount;
						firesCount++;
					}
				}
				return closestDistSq <= (float)(distToFireToFlee * distToFireToFlee) && firesCount >= minFiresNearbyToFlee;
			}, FleeUtility.MinFiresNearbyRegionsToScan, RegionType.Set_Passable);
			if (closestDistSq <= (float)(distToFireToFlee * distToFireToFlee) && firesCount >= minFiresNearbyToFlee)
			{
				Job job = FleeUtility.FleeJob(pawn, closestFire, fleeDistance);
				if (job != null)
				{
					return job;
				}
			}
			return null;
		}

		// Token: 0x06006A15 RID: 27157 RVA: 0x002167EC File Offset: 0x002149EC
		public static bool ShouldAnimalFleeDanger(Pawn pawn)
		{
			return pawn.IsAnimal && !pawn.InMentalState && !pawn.IsFighting() && !pawn.Downed && !pawn.Dead && !ThinkNode_ConditionalShouldFollowMaster.ShouldFollowMaster(pawn) && pawn.GetLord() == null && (pawn.Faction != Faction.OfPlayer || !pawn.Map.IsPlayerHome) && (pawn.Faction == null || pawn.Faction.def.animalsFleeDanger) && (pawn.CurJob == null || !pawn.CurJobDef.neverFleeFromEnemies) && (pawn.CurJob == null || pawn.jobs.curJob.def != JobDefOf.Flee || pawn.jobs.curJob.startTick != Find.TickManager.TicksGame);
		}

		// Token: 0x06006A16 RID: 27158 RVA: 0x002168D4 File Offset: 0x00214AD4
		public static bool ShouldFleeFrom(Thing t, Pawn pawn, bool checkDistance, bool checkLOS)
		{
			if (t == pawn || (checkDistance && !t.Position.InHorDistOf(pawn.Position, 8f)))
			{
				return false;
			}
			if (t.def.alwaysFlee)
			{
				return true;
			}
			if (!t.HostileTo(pawn))
			{
				return false;
			}
			IAttackTarget attackTarget = t as IAttackTarget;
			return attackTarget != null && !attackTarget.ThreatDisabled(pawn) && t is IAttackTargetSearcher && (!checkLOS || GenSight.LineOfSight(pawn.Position, t.Position, pawn.Map));
		}

		// Token: 0x04004AF0 RID: 19184
		private static int MinFiresNearbyRadius = 20;

		// Token: 0x04004AF1 RID: 19185
		private static int MinFiresNearbyRegionsToScan = 18;

		// Token: 0x04004AF2 RID: 19186
		public const float FleeWhenDistToHostileLessThan = 8f;

		// Token: 0x04004AF3 RID: 19187
		private static List<Thing> tmpThings = new List<Thing>();
	}
}
