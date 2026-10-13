using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200200E RID: 8206
	public class GenStep_Terrain : GenStep
	{
		// Token: 0x17001B87 RID: 7047
		// (get) Token: 0x0600B4B7 RID: 46263 RVA: 0x0033D9E2 File Offset: 0x0033BBE2
		public override int SeedPart
		{
			get
			{
				return 262606459;
			}
		}

		// Token: 0x0600B4B8 RID: 46264 RVA: 0x0033D9EC File Offset: 0x0033BBEC
		public override void Generate(Map map, GenStepParams parms)
		{
			TerrainGrid terrainGrid = map.terrainGrid;
			List<IntVec3> list = new List<IntVec3>();
			using (map.pathing.DisableIncrementalScope())
			{
				foreach (IntVec3 intVec in map.AllCells)
				{
					Building edifice = intVec.GetEdifice(map);
					TerrainDef naturalTerrainAt = MapGenUtility.GetNaturalTerrainAt(intVec, map);
					if (!naturalTerrainAt.supportsRock && edifice != null)
					{
						list.Add(edifice.Position);
						edifice.Destroy(DestroyMode.Vanish);
					}
					terrainGrid.SetTerrain(intVec, naturalTerrainAt);
				}
				RoofCollapseCellsFinder.RemoveBulkCollapsingRoofs(list, map);
				foreach (TerrainPatchMaker terrainPatchMaker in map.Biome.terrainPatchMakers)
				{
					terrainPatchMaker.Cleanup();
				}
			}
		}
	}
}
