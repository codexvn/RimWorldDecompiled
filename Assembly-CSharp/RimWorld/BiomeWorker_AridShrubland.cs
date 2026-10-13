using System;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x0200255A RID: 9562
	public class BiomeWorker_AridShrubland : BiomeWorker
	{
		// Token: 0x0600D169 RID: 53609 RVA: 0x003D985F File Offset: 0x003D7A5F
		public override float GetScore(BiomeDef biome, Tile tile, PlanetTile planetTile)
		{
			if (tile.WaterCovered)
			{
				return -100f;
			}
			if (!BiomeWorker_AridShrubland.Allowed(tile))
			{
				return 0f;
			}
			return BiomeWorker_AridShrubland.Score(tile);
		}

		// Token: 0x0600D16A RID: 53610 RVA: 0x003D9883 File Offset: 0x003D7A83
		public static bool Allowed(Tile tile)
		{
			return tile.temperature >= -10f && tile.rainfall >= 600f && tile.rainfall < 2000f;
		}

		// Token: 0x0600D16B RID: 53611 RVA: 0x003D98B1 File Offset: 0x003D7AB1
		public static float Score(Tile tile)
		{
			return 22.5f + (tile.temperature - 20f) * 2.2f + (tile.rainfall - 600f) / 100f;
		}
	}
}
