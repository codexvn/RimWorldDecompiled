using System;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x02002562 RID: 9570
	public class BiomeWorker_IceSheet : BiomeWorker
	{
		// Token: 0x0600D181 RID: 53633 RVA: 0x003D9C83 File Offset: 0x003D7E83
		public override float GetScore(BiomeDef biome, Tile tile, PlanetTile planetTile)
		{
			if (tile.WaterCovered)
			{
				return -100f;
			}
			return BiomeWorker_IceSheet.PermaIceScore(tile);
		}

		// Token: 0x0600D182 RID: 53634 RVA: 0x003D9C99 File Offset: 0x003D7E99
		public static float PermaIceScore(Tile tile)
		{
			return -20f + -tile.temperature * 2f;
		}
	}
}
