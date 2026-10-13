using System;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Noise;

namespace RimWorld
{
	// Token: 0x02002564 RID: 9572
	public class BiomeWorker_LavaField : BiomeWorker
	{
		// Token: 0x0600D187 RID: 53639 RVA: 0x003D9DF0 File Offset: 0x003D7FF0
		public override float GetScore(BiomeDef biome, Tile tile, PlanetTile planetTile)
		{
			if (tile.WaterCovered)
			{
				return -100f;
			}
			if (!this.AllowedAt(planetTile))
			{
				return 0f;
			}
			float num;
			if (BiomeWorker_Desert.Allowed(tile))
			{
				num = BiomeWorker_Desert.Score(tile) - 0.0001f;
			}
			else
			{
				if (!BiomeWorker_AridShrubland.Allowed(tile))
				{
					return 0f;
				}
				num = BiomeWorker_AridShrubland.Score(tile) - 0.0001f;
			}
			if (tile.hilliness >= Hilliness.SmallHills)
			{
				num += 0.0002f;
			}
			return num;
		}

		// Token: 0x0600D188 RID: 53640 RVA: 0x003D9E60 File Offset: 0x003D8060
		private bool AllowedAt(PlanetTile tile)
		{
			if (this.perlin == null)
			{
				this.perlin = new Perlin(0.30000001192092896, 0.5, 0.5, 4, true, false, Gen.HashCombineInt(Find.World.info.Seed, 967525656), QualityMode.Medium);
			}
			Vector3 tileCenter = Find.WorldGrid.GetTileCenter(tile);
			return (float)this.perlin.GetValue((double)tileCenter.x, (double)tileCenter.y, (double)tileCenter.z) >= 0.9f;
		}

		// Token: 0x04008E3A RID: 36410
		private Perlin perlin;

		// Token: 0x04008E3B RID: 36411
		private const float NoiseThreshold = 0.9f;

		// Token: 0x04008E3C RID: 36412
		private const int SeedPart = 967525656;
	}
}
