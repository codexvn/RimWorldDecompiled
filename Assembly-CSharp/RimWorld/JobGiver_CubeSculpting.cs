using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001525 RID: 5413
	public class JobGiver_CubeSculpting : ThinkNode_JobGiver
	{
		// Token: 0x060080D7 RID: 32983 RVA: 0x0026CDE8 File Offset: 0x0026AFE8
		protected override Job TryGiveJob(Pawn pawn)
		{
			IntVec3 c2;
			if (RCellFinder.TryFindNearbyEmptyCell(pawn, out c2, (IntVec3 c, Map map) => JobGiver_CubeSculpting.ValidateCell(c, map, pawn), 20))
			{
				return JobMaker.MakeJob(JobDefOf.BuildCubeSculpture, c2);
			}
			return null;
		}

		// Token: 0x060080D8 RID: 32984 RVA: 0x0026CE34 File Offset: 0x0026B034
		private static bool ValidateCell(IntVec3 cell, Map map, Pawn pawn)
		{
			foreach (IntVec3 c in GenAdjFast.AdjacentCellsCardinal(cell))
			{
				if (c.InBounds(map) && (c.GetEdifice(map) != null || cell.GetFirstThing(map) != null))
				{
					return false;
				}
			}
			TerrainDef terrainDef = map.terrainGrid.TerrainAt(cell);
			TerrainAffordanceDef terrainAffordanceNeed = ThingDefOf.DirtCubeSculpture.GetTerrainAffordanceNeed(null);
			if (terrainDef.IsRoad || !terrainDef.affordances.Contains(terrainAffordanceNeed))
			{
				return false;
			}
			Plant plant = cell.GetPlant(map);
			return (plant == null || !plant.def.plant.IsTree) && pawn.CanReserveAndReach(cell, PathEndMode.OnCell, Danger.None, 1, -1, null, false) && cell.GetEdifice(map) == null && cell.GetFirstThing(map) == null;
		}
	}
}
