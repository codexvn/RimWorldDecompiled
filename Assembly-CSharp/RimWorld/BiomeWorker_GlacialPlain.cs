using System;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Noise;

namespace RimWorld
{
	// Token: 0x0200255F RID: 9567
	public class BiomeWorker_GlacialPlain : BiomeWorker
	{
		// Token: 0x0600D177 RID: 53623 RVA: 0x003D9A14 File Offset: 0x003D7C14
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
			if (tile.rainfall <= 600f)
			{
				return 0f;
			}
			float num = 0f;
			if (tile.hilliness == Hilliness.SmallHills)
			{
				num = 10f;
			}
			else if (tile.hilliness == Hilliness.LargeHills)
			{
				num = 15f;
			}
			else if (tile.hilliness == Hilliness.Mountainous)
			{
				num = 18f;
			}
			else if (tile.hilliness == Hilliness.Impassable)
			{
				num = 18f;
			}
			return 15f + -tile.temperature + num;
		}

		// Token: 0x0600D178 RID: 53624 RVA: 0x003D9AA8 File Offset: 0x003D7CA8
		private bool AllowedAt(PlanetTile tileID)
		{
			return this.NoiseAt(tileID) >= 0.9f;
		}

		// Token: 0x0600D179 RID: 53625 RVA: 0x003D9ABC File Offset: 0x003D7CBC
		private float NoiseAt(PlanetTile tileID)
		{
			if (this.perlin == null)
			{
				this.perlin = new Perlin(0.30000001192092896, 0.4000000059604645, 0.5, 5, true, false, Gen.HashCombineInt(Find.World.info.Seed, 44319114), QualityMode.Medium);
			}
			Vector3 tileCenter = Find.WorldGrid.GetTileCenter(tileID);
			return (float)this.perlin.GetValue((double)tileCenter.x, (double)tileCenter.y, (double)tileCenter.z);
		}

		// Token: 0x04008E31 RID: 36401
		private Perlin perlin;

		// Token: 0x04008E32 RID: 36402
		private const float NoiseThreshold = 0.9f;

		// Token: 0x04008E33 RID: 36403
		private const int SeedPart = 44319114;

		// Token: 0x04008E34 RID: 36404
		private const float HillinessSmallHillsElevation = 10f;

		// Token: 0x04008E35 RID: 36405
		private const float HillinessLargeHillsElevation = 15f;

		// Token: 0x04008E36 RID: 36406
		private const float HillinessMountainousElevation = 18f;

		// Token: 0x04008E37 RID: 36407
		private const float HillinessImpassableElevation = 18f;
	}
}
