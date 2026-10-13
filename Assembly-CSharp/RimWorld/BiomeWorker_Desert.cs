using System;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x0200255D RID: 9565
	public class BiomeWorker_Desert : BiomeWorker
	{
		// Token: 0x0600D171 RID: 53617 RVA: 0x003D997F File Offset: 0x003D7B7F
		public override float GetScore(BiomeDef biome, Tile tile, PlanetTile planetTile)
		{
			if (tile.WaterCovered)
			{
				return -100f;
			}
			if (!BiomeWorker_Desert.Allowed(tile))
			{
				return 0f;
			}
			return BiomeWorker_Desert.Score(tile);
		}

		// Token: 0x0600D172 RID: 53618 RVA: 0x003D99A3 File Offset: 0x003D7BA3
		public static bool Allowed(Tile tile)
		{
			return tile.rainfall < 600f;
		}

		// Token: 0x0600D173 RID: 53619 RVA: 0x003D99B5 File Offset: 0x003D7BB5
		public static float Score(Tile tile)
		{
			return tile.temperature + 0.0001f;
		}
	}
}
