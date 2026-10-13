using System;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x0200255E RID: 9566
	public class BiomeWorker_ExtremeDesert : BiomeWorker
	{
		// Token: 0x0600D175 RID: 53621 RVA: 0x003D99C4 File Offset: 0x003D7BC4
		public override float GetScore(BiomeDef biome, Tile tile, PlanetTile planetTile)
		{
			if (tile.WaterCovered)
			{
				return -100f;
			}
			if (tile.rainfall >= 340f)
			{
				return 0f;
			}
			return tile.temperature * 2.7f - 13f - tile.rainfall * 0.14f;
		}
	}
}
