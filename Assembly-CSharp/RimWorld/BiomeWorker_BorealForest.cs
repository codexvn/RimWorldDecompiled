using System;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x0200255B RID: 9563
	public class BiomeWorker_BorealForest : BiomeWorker
	{
		// Token: 0x0600D16D RID: 53613 RVA: 0x003D98E6 File Offset: 0x003D7AE6
		public override float GetScore(BiomeDef biome, Tile tile, PlanetTile planetTile)
		{
			if (tile.WaterCovered)
			{
				return -100f;
			}
			if (tile.temperature < -10f)
			{
				return 0f;
			}
			if (tile.rainfall < 600f)
			{
				return 0f;
			}
			return 15f;
		}
	}
}
