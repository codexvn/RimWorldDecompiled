using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200148F RID: 5263
	public class JobGiver_GetDeathrest : ThinkNode_JobGiver
	{
		// Token: 0x06007E88 RID: 32392 RVA: 0x002613A0 File Offset: 0x0025F5A0
		public override float GetPriority(Pawn pawn)
		{
			if (!ModsConfig.BiotechActive)
			{
				return 0f;
			}
			Lord lord = pawn.GetLord();
			if (lord != null && !lord.CurLordToil.AllowSatisfyLongNeeds)
			{
				return 0f;
			}
			Need_Deathrest need_Deathrest;
			if (pawn.needs == null || !pawn.needs.TryGetNeed<Need_Deathrest>(out need_Deathrest) || need_Deathrest.CurLevelPercentage > this.maxNeedPercent)
			{
				return 0f;
			}
			return 7.75f;
		}

		// Token: 0x06007E89 RID: 32393 RVA: 0x00261408 File Offset: 0x0025F608
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (!ModsConfig.BiotechActive)
			{
				return null;
			}
			Need_Deathrest need_Deathrest;
			if (pawn.needs == null || !pawn.needs.TryGetNeed<Need_Deathrest>(out need_Deathrest))
			{
				return null;
			}
			if (need_Deathrest.CurLevelPercentage > this.maxNeedPercent)
			{
				return null;
			}
			Lord lord = pawn.GetLord();
			Building_Bed building_Bed;
			if ((lord == null || lord.CurLordToil == null || lord.CurLordToil.AllowRestingInBed) && !pawn.IsWildMan() && (!pawn.InMentalState || pawn.MentalState.AllowRestingInBed))
			{
				Pawn_RopeTracker roping = pawn.roping;
				if (roping == null || !roping.IsRoped)
				{
					building_Bed = RestUtility.FindBedFor(pawn);
					goto IL_8D;
				}
			}
			building_Bed = null;
			IL_8D:
			if (building_Bed != null)
			{
				return JobMaker.MakeJob(JobDefOf.Deathrest, building_Bed);
			}
			return JobMaker.MakeJob(JobDefOf.Deathrest, this.FindGroundSleepSpotFor(pawn));
		}

		// Token: 0x06007E8A RID: 32394 RVA: 0x002614CC File Offset: 0x0025F6CC
		private IntVec3 FindGroundSleepSpotFor(Pawn pawn)
		{
			Map map = pawn.Map;
			IntVec3 position = pawn.Position;
			Predicate<IntVec3> <>9__0;
			for (int i = 0; i < 2; i++)
			{
				int num = (i == 0) ? 4 : 12;
				IntVec3 root = position;
				Map map2 = map;
				int radius = num;
				Predicate<IntVec3> extraValidator;
				if ((extraValidator = <>9__0) == null)
				{
					extraValidator = (<>9__0 = ((IntVec3 x) => !x.IsForbidden(pawn) && !x.GetTerrain(map).avoidWander));
				}
				IntVec3 result;
				if (CellFinder.TryRandomClosewalkCellNear(root, map2, radius, out result, extraValidator))
				{
					return result;
				}
			}
			return CellFinder.RandomClosewalkCellNearNotForbidden(pawn, 4, null);
		}

		// Token: 0x06007E8B RID: 32395 RVA: 0x0026155A File Offset: 0x0025F75A
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_GetDeathrest jobGiver_GetDeathrest = (JobGiver_GetDeathrest)base.DeepCopy(resolve);
			jobGiver_GetDeathrest.maxNeedPercent = this.maxNeedPercent;
			return jobGiver_GetDeathrest;
		}

		// Token: 0x040054D9 RID: 21721
		public float maxNeedPercent = 0.05f;
	}
}
