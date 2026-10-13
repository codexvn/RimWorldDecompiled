using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Noise;

namespace RimWorld.Planet
{
	// Token: 0x02003CF9 RID: 15609
	public class WorldGenStep_Pollution : WorldGenStep
	{
		// Token: 0x170034EF RID: 13551
		// (get) Token: 0x060153CE RID: 86990 RVA: 0x0065A67F File Offset: 0x0065887F
		public override int SeedPart
		{
			get
			{
				return 759372056;
			}
		}

		// Token: 0x060153CF RID: 86991 RVA: 0x0065A688 File Offset: 0x00658888
		public override void GenerateFresh(string seed, PlanetLayer layer)
		{
			float pollution = Find.World.info.pollution;
			if (pollution <= 0f)
			{
				for (int i = 0; i < layer.TilesCount; i++)
				{
					Tile tile = layer[i];
					float pollutionOffset = tile.PrimaryBiome.pollutionOffset;
					tile.pollution = Mathf.Clamp01(pollutionOffset);
				}
				return;
			}
			Perlin perlin = new Perlin(0.10000000149011612, 2.0, 0.5, 6, seed.GetHashCode(), QualityMode.Medium);
			this.tmpTiles.Clear();
			this.tmpTileNoise.Clear();
			foreach (Tile tile2 in layer.Tiles)
			{
				if (tile2.PrimaryBiome.allowPollution)
				{
					this.tmpTileNoise.Add(tile2.tile, perlin.GetValue(layer.GetTileCenter(tile2.tile)));
					this.tmpTiles.Add(tile2.tile);
				}
			}
			this.tmpTiles.SortByDescending((PlanetTile t) => this.tmpTileNoise[t]);
			int num = Mathf.RoundToInt((float)this.tmpTiles.Count * pollution);
			for (int j = 0; j < num; j++)
			{
				Tile tile3 = layer[this.tmpTiles[j]];
				float value = Mathf.Lerp(0.25f, 1f, this.tmpTileNoise[this.tmpTiles[j]]);
				tile3.pollution = Mathf.Clamp01(value);
			}
			this.tmpTiles.Clear();
			this.tmpTileNoise.Clear();
		}

		// Token: 0x0400ECC8 RID: 60616
		private const float MinPollution = 0.25f;

		// Token: 0x0400ECC9 RID: 60617
		private const float MaxPollution = 1f;

		// Token: 0x0400ECCA RID: 60618
		private const float PerlinFrequency = 0.1f;

		// Token: 0x0400ECCB RID: 60619
		private readonly List<PlanetTile> tmpTiles = new List<PlanetTile>();

		// Token: 0x0400ECCC RID: 60620
		private readonly Dictionary<PlanetTile, float> tmpTileNoise = new Dictionary<PlanetTile, float>();
	}
}
