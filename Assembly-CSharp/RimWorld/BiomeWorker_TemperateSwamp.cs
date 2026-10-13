using System;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x02002569 RID: 9577
	public class BiomeWorker_TemperateSwamp : BiomeWorker
	{
		// Token: 0x0600D198 RID: 53656 RVA: 0x003DA0D4 File Offset: 0x003D82D4
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
			if (tile.swampiness < 0.5f)
			{
				return 0f;
			}
			return 15f + (tile.temperature - 7f) + (tile.rainfall - 600f) / 180f + tile.swampiness * 3f;
		}
	}
}
