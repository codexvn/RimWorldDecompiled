using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x0200210B RID: 8459
	public class TileMutatorWorker_Patches : TileMutatorWorker
	{
		// Token: 0x0600B895 RID: 47253 RVA: 0x0034CD7F File Offset: 0x0034AF7F
		public TileMutatorWorker_Patches(TileMutatorDef def) : base(def)
		{
		}

		// Token: 0x0600B896 RID: 47254 RVA: 0x00353D60 File Offset: 0x00351F60
		public override void GeneratePostTerrain(Map map)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			bool flag = map.TileInfo.Mutators.Any((TileMutatorDef m) => m.preventsPondGeneration);
			if (map.TileInfo.Mutators.Any((TileMutatorDef m) => m.preventPatches))
			{
				return;
			}
			foreach (IntVec3 intVec in map.AllCells)
			{
				TerrainDef terrain = intVec.GetTerrain(map);
				if (terrain.categoryType != TerrainDef.TerrainCategoryType.Stone && !terrain.IsWater && terrain != MapGenUtility.BeachTerrainAt(intVec, map) && terrain != MapGenUtility.LakeshoreTerrainAt(intVec, map))
				{
					foreach (TerrainPatchMaker terrainPatchMaker in this.def.terrainPatchMakers)
					{
						if (!terrainPatchMaker.isPond || !flag)
						{
							TerrainDef terrainDef = terrainPatchMaker.TerrainAt(intVec, map, intVec.GetFertility(map));
							if (terrainDef != null)
							{
								map.terrainGrid.SetTerrain(intVec, terrainDef);
							}
						}
					}
				}
			}
			foreach (TerrainPatchMaker terrainPatchMaker2 in this.def.terrainPatchMakers)
			{
				terrainPatchMaker2.Cleanup();
			}
		}
	}
}
