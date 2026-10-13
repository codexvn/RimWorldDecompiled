using System;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x0200256B RID: 9579
	public class BiomeWorker_TropicalSwamp : BiomeWorker
	{
		// Token: 0x0600D19C RID: 53660 RVA: 0x003DA1C8 File Offset: 0x003D83C8
		public override float GetScore(BiomeDef biome, Tile tile, PlanetTile planetTile)
		{
			if (tile.WaterCovered)
			{
				return -100f;
			}
			if (!BiomeWorker_TropicalSwamp.Allowed(tile))
			{
				return 0f;
			}
			return BiomeWorker_TropicalSwamp.Score(tile);
		}

		// Token: 0x0600D19D RID: 53661 RVA: 0x003DA1EC File Offset: 0x003D83EC
		public static bool Allowed(Tile tile)
		{
			return tile.temperature >= 15f && tile.temperature <= 55f && tile.rainfall >= 2000f && tile.swampiness >= 0.5f;
		}

		// Token: 0x0600D19E RID: 53662 RVA: 0x003DA227 File Offset: 0x003D8427
		public static float Score(Tile tile)
		{
			return 28f + (tile.temperature - 20f) * 1.5f + (tile.rainfall - 600f) / 165f + tile.swampiness * 3f;
		}
	}
}
