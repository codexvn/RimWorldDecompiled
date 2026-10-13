using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D82 RID: 7554
	public class IncidentWorker_BoomshroomSprout : IncidentWorker
	{
		// Token: 0x0600A92B RID: 43307 RVA: 0x00306A64 File Offset: 0x00304C64
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			if (!base.CanFireNowSub(parms))
			{
				return false;
			}
			Map map = (Map)parms.target;
			IntVec3 intVec;
			return PlantUtility.GrowthSeasonNow(map, ThingDefOf.Boomshroom) && this.TryFindRootCell(map, out intVec);
		}

		// Token: 0x0600A92C RID: 43308 RVA: 0x00306AA0 File Offset: 0x00304CA0
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			IntVec3 intVec;
			if (!this.TryFindRootCell(map, out intVec))
			{
				return false;
			}
			Thing thing = null;
			int randomInRange = IncidentWorker_BoomshroomSprout.CountRange.RandomInRange;
			Predicate<IntVec3> <>9__0;
			for (int i = 0; i < randomInRange; i++)
			{
				IntVec3 root = intVec;
				Map map2 = map;
				int radius = 4;
				Predicate<IntVec3> extraValidator;
				if ((extraValidator = <>9__0) == null)
				{
					extraValidator = (<>9__0 = ((IntVec3 x) => this.CanSpawnAt(x, map)));
				}
				IntVec3 intVec2;
				if (!CellFinder.TryRandomClosewalkCellNear(root, map2, radius, out intVec2, extraValidator))
				{
					break;
				}
				Plant plant = intVec2.GetPlant(map);
				if (plant != null)
				{
					plant.Destroy(DestroyMode.Vanish);
				}
				Thing thing2 = GenSpawn.Spawn(ThingDefOf.Boomshroom, intVec2, map, WipeMode.Vanish);
				if (thing == null)
				{
					thing = thing2;
				}
			}
			if (thing == null)
			{
				return false;
			}
			base.SendStandardLetter(parms, thing, Array.Empty<NamedArgument>());
			return true;
		}

		// Token: 0x0600A92D RID: 43309 RVA: 0x00306B80 File Offset: 0x00304D80
		private bool TryFindRootCell(Map map, out IntVec3 cell)
		{
			return CellFinderLoose.TryFindRandomNotEdgeCellWith(10, (IntVec3 x) => this.CanSpawnAt(x, map) && x.GetRoom(map).CellCount >= 64, map, out cell);
		}

		// Token: 0x0600A92E RID: 43310 RVA: 0x00306BBC File Offset: 0x00304DBC
		private bool CanSpawnAt(IntVec3 c, Map map)
		{
			if (!c.Standable(map) || c.Fogged(map) || map.fertilityGrid.FertilityAt(c) < ThingDefOf.Boomshroom.plant.fertilityMin || !c.GetRoom(map).PsychologicallyOutdoors || c.GetEdifice(map) != null || !PlantUtility.GrowthSeasonNow(c, map, ThingDefOf.Boomshroom))
			{
				return false;
			}
			Thing thing;
			if (!ThingDefOf.Boomshroom.CanEverPlantAt(c, map, out thing, false, true, true))
			{
				return false;
			}
			Plant plant = c.GetPlant(map);
			if (plant != null && plant.def.plant.growDays > 10f)
			{
				return false;
			}
			List<Thing> thingList = c.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				if (thingList[i].def == ThingDefOf.Boomshroom)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x04007510 RID: 29968
		private static readonly IntRange CountRange = new IntRange(6, 10);

		// Token: 0x04007511 RID: 29969
		private const int MinRoomCells = 64;

		// Token: 0x04007512 RID: 29970
		private const int SpawnRadius = 4;
	}
}
