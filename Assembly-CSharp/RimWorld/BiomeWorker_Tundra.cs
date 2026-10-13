using System;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x0200256C RID: 9580
	public class BiomeWorker_Tundra : BiomeWorker
	{
		// Token: 0x0600D1A0 RID: 53664 RVA: 0x003DA261 File Offset: 0x003D8461
		public override float GetScore(BiomeDef biome, Tile tile, PlanetTile planetTile)
		{
			if (tile.WaterCovered)
			{
				return -100f;
			}
			return -tile.temperature;
		}
	}
}
