using System;
using System.Collections.Generic;
using RimWorld;
using Verse.AI;

namespace Verse
{
	// Token: 0x02000524 RID: 1316
	public static class RoofUtility
	{
		// Token: 0x06002250 RID: 8784 RVA: 0x000B2B98 File Offset: 0x000B0D98
		public static Thing FirstBlockingThing(IntVec3 pos, Map map)
		{
			List<Thing> list = map.thingGrid.ThingsListAt(pos);
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].def.plant != null && list[i].def.plant.interferesWithRoof)
				{
					return list[i];
				}
			}
			return null;
		}

		// Token: 0x06002251 RID: 8785 RVA: 0x000B2BF8 File Offset: 0x000B0DF8
		public static bool IsAnyCellUnderRoof(Thing thing)
		{
			CellRect cellRect = thing.OccupiedRect();
			bool result = false;
			RoofGrid roofGrid = thing.Map.roofGrid;
			foreach (IntVec3 c in cellRect)
			{
				if (roofGrid.Roofed(c))
				{
					result = true;
					break;
				}
			}
			return result;
		}

		// Token: 0x06002252 RID: 8786 RVA: 0x000B2C68 File Offset: 0x000B0E68
		public static bool CanHandleBlockingThing(Thing blocker, Pawn worker, bool forced = false)
		{
			if (blocker == null)
			{
				return true;
			}
			if (blocker.def.category == ThingCategory.Plant)
			{
				if (!PlantUtility.PawnWillingToCutPlant_Job(blocker, worker))
				{
					return false;
				}
				if (worker.CanReserveAndReach(blocker, PathEndMode.ClosestTouch, worker.NormalMaxDanger(), 1, -1, null, forced))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002253 RID: 8787 RVA: 0x000B2CA4 File Offset: 0x000B0EA4
		public static Job HandleBlockingThingJob(Thing blocker, Pawn worker, bool forced = false)
		{
			if (blocker == null)
			{
				return null;
			}
			if (blocker.def.category != ThingCategory.Plant || !worker.CanReserveAndReach(blocker, PathEndMode.ClosestTouch, worker.NormalMaxDanger(), 1, -1, null, forced))
			{
				return null;
			}
			if (!PlantUtility.PawnWillingToCutPlant_Job(blocker, worker))
			{
				return null;
			}
			return JobMaker.MakeJob(JobDefOf.CutPlant, blocker);
		}
	}
}
