using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003CF3 RID: 15603
	public class WorldGenStep_Lakes : WorldGenStep
	{
		// Token: 0x170034EC RID: 13548
		// (get) Token: 0x060153B7 RID: 86967 RVA: 0x00659E78 File Offset: 0x00658078
		public override int SeedPart
		{
			get
			{
				return 401463656;
			}
		}

		// Token: 0x060153B8 RID: 86968 RVA: 0x00659E7F File Offset: 0x0065807F
		public override void GenerateFresh(string seed, PlanetLayer layer)
		{
			this.GenerateLakes(layer);
		}

		// Token: 0x060153B9 RID: 86969 RVA: 0x00659E88 File Offset: 0x00658088
		private void GenerateLakes(PlanetLayer layer)
		{
			bool[] touched = new bool[layer.TilesCount];
			List<int> oceanChunk = new List<int>();
			Predicate<PlanetTile> <>9__0;
			Action<PlanetTile> <>9__1;
			foreach (Tile tile3 in layer.Tiles)
			{
				PlanetTile tile2 = tile3.tile;
				if (!touched[tile2.tileId] && layer[tile2.tileId].PrimaryBiome == BiomeDefOf.Ocean)
				{
					WorldFloodFiller filler = layer.Filler;
					PlanetTile rootTile = tile2;
					Predicate<PlanetTile> passCheck;
					if ((passCheck = <>9__0) == null)
					{
						passCheck = (<>9__0 = ((PlanetTile tid) => layer[tid].PrimaryBiome == BiomeDefOf.Ocean));
					}
					Action<PlanetTile> processor;
					if ((processor = <>9__1) == null)
					{
						processor = (<>9__1 = delegate(PlanetTile tile)
						{
							oceanChunk.Add(tile.tileId);
							touched[tile.tileId] = true;
						});
					}
					filler.FloodFill(rootTile, passCheck, processor, int.MaxValue, null);
					if (oceanChunk.Count <= 15)
					{
						for (int i = 0; i < oceanChunk.Count; i++)
						{
							layer[oceanChunk[i]].PrimaryBiome = BiomeDefOf.Lake;
						}
					}
					oceanChunk.Clear();
				}
			}
		}

		// Token: 0x0400ECB6 RID: 60598
		private const int LakeMaxSize = 15;
	}
}
