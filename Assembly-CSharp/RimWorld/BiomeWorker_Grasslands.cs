using System;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x02002561 RID: 9569
	public class BiomeWorker_Grasslands : BiomeWorker
	{
		// Token: 0x0600D17D RID: 53629 RVA: 0x003D9BD7 File Offset: 0x003D7DD7
		public override float GetScore(BiomeDef biome, Tile tile, PlanetTile planetTile)
		{
			if (tile.WaterCovered)
			{
				return -100f;
			}
			if (!BiomeWorker_Grasslands.Allowed(tile))
			{
				return 0f;
			}
			return BiomeWorker_Grasslands.Score(tile);
		}

		// Token: 0x0600D17E RID: 53630 RVA: 0x003D9BFC File Offset: 0x003D7DFC
		public static bool Allowed(Tile tile)
		{
			return tile.temperature >= -10f && tile.temperature <= 55f && tile.rainfall >= 800f && tile.rainfall < 2500f && tile.hilliness != Hilliness.Mountainous && tile.hilliness != Hilliness.Impassable;
		}

		// Token: 0x0600D17F RID: 53631 RVA: 0x003D9C56 File Offset: 0x003D7E56
		public static float Score(Tile tile)
		{
			return 22.5f + (tile.temperature - 20f) * 2.4f + (tile.rainfall - 800f) / 100f;
		}
	}
}
