using System;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x0200256A RID: 9578
	public class BiomeWorker_TropicalRainforest : BiomeWorker
	{
		// Token: 0x0600D19A RID: 53658 RVA: 0x003DA15C File Offset: 0x003D835C
		public override float GetScore(BiomeDef biome, Tile tile, PlanetTile planetTile)
		{
			if (tile.WaterCovered)
			{
				return -100f;
			}
			if (tile.temperature < 15f)
			{
				return 0f;
			}
			if (tile.rainfall < 2000f)
			{
				return 0f;
			}
			return 28f + (tile.temperature - 20f) * 1.5f + (tile.rainfall - 600f) / 165f;
		}
	}
}
