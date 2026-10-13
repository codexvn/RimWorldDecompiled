using System;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x02002568 RID: 9576
	public class BiomeWorker_TemperateForest : BiomeWorker
	{
		// Token: 0x0600D194 RID: 53652 RVA: 0x003DA068 File Offset: 0x003D8268
		public override float GetScore(BiomeDef biome, Tile tile, PlanetTile planetTile)
		{
			if (tile.WaterCovered)
			{
				return -100f;
			}
			if (!BiomeWorker_TemperateForest.Allowed(tile))
			{
				return 0f;
			}
			return BiomeWorker_TemperateForest.Score(tile);
		}

		// Token: 0x0600D195 RID: 53653 RVA: 0x003DA08C File Offset: 0x003D828C
		public static bool Allowed(Tile tile)
		{
			return tile.temperature >= -10f && tile.rainfall >= 600f;
		}

		// Token: 0x0600D196 RID: 53654 RVA: 0x003DA0AD File Offset: 0x003D82AD
		public static float Score(Tile tile)
		{
			return 15f + (tile.temperature - 7f) + (tile.rainfall - 600f) / 180f;
		}
	}
}
