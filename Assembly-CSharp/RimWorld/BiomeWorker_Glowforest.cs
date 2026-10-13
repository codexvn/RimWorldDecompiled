using System;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x02002560 RID: 9568
	public class BiomeWorker_Glowforest : BiomeWorker
	{
		// Token: 0x0600D17B RID: 53627 RVA: 0x003D9B44 File Offset: 0x003D7D44
		public override float GetScore(BiomeDef biome, Tile tile, PlanetTile planetTile)
		{
			if (tile.WaterCovered)
			{
				return -100f;
			}
			if (tile.temperature < 10f || tile.temperature > 55f)
			{
				return 0f;
			}
			if (tile.rainfall < 1000f)
			{
				return 0f;
			}
			if (tile.swampiness < 0.6f)
			{
				return 0f;
			}
			return 23f + (tile.temperature - 7f) + (tile.rainfall - 600f) / 220f + tile.swampiness * 3f;
		}
	}
}
