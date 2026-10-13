using System;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x02002565 RID: 9573
	public class BiomeWorker_Ocean : BiomeWorker
	{
		// Token: 0x0600D18A RID: 53642 RVA: 0x003D9EEF File Offset: 0x003D80EF
		public override float GetScore(BiomeDef biome, Tile tile, PlanetTile planetTile)
		{
			if (!tile.WaterCovered)
			{
				return -100f;
			}
			return 0f;
		}
	}
}
