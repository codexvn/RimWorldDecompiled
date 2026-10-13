using System;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Noise;

namespace RimWorld
{
	// Token: 0x02002563 RID: 9571
	public class BiomeWorker_SeaIce : BiomeWorker
	{
		// Token: 0x0600D184 RID: 53636 RVA: 0x003D9CAE File Offset: 0x003D7EAE
		public override float GetScore(BiomeDef biome, Tile tile, PlanetTile planetTile)
		{
			if (!tile.WaterCovered)
			{
				return -100f;
			}
			if (!this.AllowedAt(planetTile))
			{
				return -100f;
			}
			return BiomeWorker_IceSheet.PermaIceScore(tile) - 23f;
		}

		// Token: 0x0600D185 RID: 53637 RVA: 0x003D9CDC File Offset: 0x003D7EDC
		private bool AllowedAt(PlanetTile tile)
		{
			Vector3 tileCenter = Find.WorldGrid.GetTileCenter(tile);
			Vector3 surfaceViewCenter = Find.WorldGrid.SurfaceViewCenter;
			float value = Vector3.Angle(surfaceViewCenter, tileCenter);
			float surfaceViewAngle = Find.WorldGrid.SurfaceViewAngle;
			float num = Mathf.Min(7.5f, surfaceViewAngle * 0.12f);
			float num2 = Mathf.InverseLerp(surfaceViewAngle - num, surfaceViewAngle, value);
			if (num2 <= 0f)
			{
				return true;
			}
			if (this.cachedSeaIceAllowedNoise == null || this.cachedSeaIceAllowedNoiseForSeed != Find.World.info.Seed)
			{
				this.cachedSeaIceAllowedNoise = new Perlin(0.017000000923871994, 2.0, 0.5, 6, Find.World.info.Seed, QualityMode.Medium);
				this.cachedSeaIceAllowedNoiseForSeed = Find.World.info.Seed;
			}
			float headingFromTo = Find.WorldGrid.GetHeadingFromTo(surfaceViewCenter, tileCenter);
			float num3 = (float)this.cachedSeaIceAllowedNoise.GetValue((double)headingFromTo, 0.0, 0.0) * 0.5f + 0.5f;
			return num2 <= num3;
		}

		// Token: 0x04008E38 RID: 36408
		private ModuleBase cachedSeaIceAllowedNoise;

		// Token: 0x04008E39 RID: 36409
		private int cachedSeaIceAllowedNoiseForSeed;
	}
}
