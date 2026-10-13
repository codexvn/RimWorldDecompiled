using System;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x0200255C RID: 9564
	public class BiomeWorker_ColdBog : BiomeWorker
	{
		// Token: 0x0600D16F RID: 53615 RVA: 0x003D9924 File Offset: 0x003D7B24
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
			if (tile.swampiness < 0.5f)
			{
				return 0f;
			}
			return -tile.temperature + 13f + tile.swampiness * 8f;
		}
	}
}
