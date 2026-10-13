using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003C31 RID: 15409
	public class FeatureWorker_OuterOcean : FeatureWorker
	{
		// Token: 0x06014E7D RID: 85629 RVA: 0x00642E10 File Offset: 0x00641010
		public override void GenerateWhereAppropriate(PlanetLayer layer)
		{
			this.edgeTiles.Clear();
			for (int i = 0; i < layer.TilesCount; i++)
			{
				PlanetTile planetTile = new PlanetTile(i, layer);
				if (this.IsRoot(planetTile))
				{
					this.edgeTiles.Add(planetTile);
				}
			}
			if (!this.edgeTiles.Any<PlanetTile>())
			{
				return;
			}
			this.group.Clear();
			layer.Filler.FloodFill(PlanetTile.Invalid, new Predicate<PlanetTile>(this.CanTraverse), delegate(PlanetTile tile, int traversalDist)
			{
				this.group.Add(tile);
				return false;
			}, int.MaxValue, this.edgeTiles);
			this.group.RemoveAll((PlanetTile x) => layer[x].feature != null);
			if (this.group.Count < this.def.minSize || this.group.Count > this.def.maxSize)
			{
				return;
			}
			base.AddFeature(layer, this.group, this.group);
		}

		// Token: 0x06014E7E RID: 85630 RVA: 0x00642F27 File Offset: 0x00641127
		private bool IsRoot(PlanetTile tile)
		{
			return Find.WorldGrid.IsOnEdge(tile) && this.CanTraverse(tile) && Find.WorldGrid[tile].feature == null;
		}

		// Token: 0x06014E7F RID: 85631 RVA: 0x00642F54 File Offset: 0x00641154
		private bool CanTraverse(PlanetTile tile)
		{
			BiomeDef primaryBiome = Find.WorldGrid[tile].PrimaryBiome;
			return primaryBiome == BiomeDefOf.Ocean || primaryBiome == BiomeDefOf.Lake;
		}

		// Token: 0x0400E912 RID: 59666
		private readonly List<PlanetTile> group = new List<PlanetTile>();

		// Token: 0x0400E913 RID: 59667
		private readonly List<PlanetTile> edgeTiles = new List<PlanetTile>();
	}
}
