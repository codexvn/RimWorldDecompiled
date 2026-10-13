using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200203B RID: 8251
	public class RoadDefGenStep_DryWithFallback : RoadDefGenStep
	{
		// Token: 0x0600B54B RID: 46411 RVA: 0x003421B0 File Offset: 0x003403B0
		public override void Place(Map map, IntVec3 position, TerrainDef rockDef, IntVec3 origin, GenStep_Roads.DistanceElement[,] distance)
		{
			RoadDefGenStep_DryWithFallback.PlaceWorker(map, position, this.fallback);
		}

		// Token: 0x0600B54C RID: 46412 RVA: 0x003421C0 File Offset: 0x003403C0
		public static void PlaceWorker(Map map, IntVec3 position, TerrainDef fallback)
		{
			while (map.terrainGrid.TerrainAt(position).driesTo != null)
			{
				map.terrainGrid.SetTerrain(position, map.terrainGrid.TerrainAt(position).driesTo);
			}
			TerrainDef terrainDef = map.terrainGrid.TerrainAt(position);
			if (terrainDef.passability == Traversability.Impassable || terrainDef.IsRiver)
			{
				map.terrainGrid.SetTerrain(position, fallback);
			}
		}

		// Token: 0x04007C83 RID: 31875
		public TerrainDef fallback;
	}
}
