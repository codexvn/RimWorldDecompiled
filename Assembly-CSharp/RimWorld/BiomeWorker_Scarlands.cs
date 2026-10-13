using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Noise;

namespace RimWorld
{
	// Token: 0x02002566 RID: 9574
	public class BiomeWorker_Scarlands : BiomeWorker
	{
		// Token: 0x0600D18C RID: 53644 RVA: 0x003D9F04 File Offset: 0x003D8104
		public override float GetScore(BiomeDef biome, Tile tile, PlanetTile planetTile)
		{
			if (tile.WaterCovered)
			{
				return -100f;
			}
			if (!BiomeWorker_TemperateForest.Allowed(tile) || !this.AllowedAt(planetTile))
			{
				return 0f;
			}
			return BiomeWorker_TemperateForest.Score(tile) + 20f;
		}

		// Token: 0x0600D18D RID: 53645 RVA: 0x003D9F38 File Offset: 0x003D8138
		private bool AllowedAt(PlanetTile tileID)
		{
			if (this.NoiseAt(tileID) < 0.9f)
			{
				return false;
			}
			Find.WorldGrid.GetTileNeighbors(tileID, BiomeWorker_Scarlands.SearchList);
			if (BiomeWorker_Scarlands.SearchList.Empty<PlanetTile>())
			{
				return false;
			}
			foreach (PlanetTile tileID2 in BiomeWorker_Scarlands.SearchList)
			{
				if (this.NoiseAt(tileID2) >= 0.9f)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600D18E RID: 53646 RVA: 0x003D9FC8 File Offset: 0x003D81C8
		private float NoiseAt(PlanetTile tileID)
		{
			if (this.perlin == null)
			{
				this.perlin = new Perlin(0.20000000298023224, 0.5, 0.10000000149011612, 4, true, false, Gen.HashCombineInt(Find.World.info.Seed, 417235213), QualityMode.Medium);
			}
			Vector3 tileCenter = Find.WorldGrid.GetTileCenter(tileID);
			return (float)this.perlin.GetValue((double)tileCenter.x, (double)tileCenter.y, (double)tileCenter.z);
		}

		// Token: 0x04008E3D RID: 36413
		private Perlin perlin;

		// Token: 0x04008E3E RID: 36414
		private static readonly List<PlanetTile> SearchList = new List<PlanetTile>();

		// Token: 0x04008E3F RID: 36415
		private const float NoiseThreshold = 0.9f;

		// Token: 0x04008E40 RID: 36416
		private const int SeedPart = 417235213;
	}
}
