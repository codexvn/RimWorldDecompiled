using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003D00 RID: 15616
	public class WorldGenStep_Roads : WorldGenStep
	{
		// Token: 0x170034F1 RID: 13553
		// (get) Token: 0x060153EE RID: 87022 RVA: 0x0065AF7D File Offset: 0x0065917D
		public override int SeedPart
		{
			get
			{
				return 1538475135;
			}
		}

		// Token: 0x060153EF RID: 87023 RVA: 0x0065AF84 File Offset: 0x00659184
		public override void GenerateFresh(string seed, PlanetLayer layer)
		{
			this.GenerateRoadEndpoints(layer);
			Rand.PushState();
			Rand.Seed = GenText.StableStringHash(seed);
			this.GenerateRoadNetwork(layer);
			Rand.PopState();
		}

		// Token: 0x060153F0 RID: 87024 RVA: 0x0065AFA9 File Offset: 0x006591A9
		public override void GenerateWithoutWorldData(string seed, PlanetLayer layer)
		{
			Rand.PushState();
			Rand.Seed = GenText.StableStringHash(seed);
			this.GenerateRoadNetwork(layer);
			Rand.PopState();
		}

		// Token: 0x060153F1 RID: 87025 RVA: 0x0065AFC8 File Offset: 0x006591C8
		private void GenerateRoadEndpoints(PlanetLayer layer)
		{
			List<PlanetTile> list = (from wo in Find.WorldObjects.AllWorldObjects
			where Rand.Value > 0.05f && wo.Tile.Layer == layer
			select wo.Tile).ToList<PlanetTile>();
			int num = GenMath.RoundRandom((float)Find.WorldGrid.TilesCount / 100000f * WorldGenStep_Roads.ExtraRoadNodesPer100kTiles.RandomInRange);
			for (int i = 0; i < num; i++)
			{
				list.Add(TileFinder.RandomSettlementTileFor(layer, null, false, null));
			}
			List<PlanetTile> list2 = new List<PlanetTile>();
			for (int j = 0; j < list.Count; j++)
			{
				int num2 = Mathf.Max(0, WorldGenStep_Roads.RoadDistanceFromSettlement.RandomInRange);
				PlanetTile planetTile = list[j];
				for (int k = 0; k < num2; k++)
				{
					Find.WorldGrid.GetTileNeighbors(planetTile, list2);
					planetTile = list2.RandomElement<PlanetTile>();
				}
				if (Find.WorldReachability.CanReach(list[j], planetTile))
				{
					list[j] = planetTile;
				}
			}
			list = list.Distinct<PlanetTile>().ToList<PlanetTile>();
			Find.World.genData.roadNodes[layer] = list;
		}

		// Token: 0x060153F2 RID: 87026 RVA: 0x0065B11C File Offset: 0x0065931C
		private void GenerateRoadNetwork(PlanetLayer layer)
		{
			Find.WorldPathGrid.RecalculateLayerPerceivedPathCosts(layer, new int?(0));
			List<WorldGenStep_Roads.Link> linkProspective = this.GenerateProspectiveLinks(Find.World.genData.roadNodes[layer], layer);
			List<WorldGenStep_Roads.Link> linkFinal = this.GenerateFinalLinks(linkProspective, Find.World.genData.roadNodes[layer].Count);
			this.DrawLinksOnWorld(layer, linkFinal, Find.World.genData.roadNodes[layer]);
		}

		// Token: 0x060153F3 RID: 87027 RVA: 0x0065B198 File Offset: 0x00659398
		private List<WorldGenStep_Roads.Link> GenerateProspectiveLinks(List<PlanetTile> indexToTile, PlanetLayer layer)
		{
			Dictionary<PlanetTile, PlanetTile> tileToIndexLookup = new Dictionary<PlanetTile, PlanetTile>();
			for (int i = 0; i < indexToTile.Count; i++)
			{
				tileToIndexLookup[indexToTile[i]] = new PlanetTile(i, layer);
			}
			List<WorldGenStep_Roads.Link> linkProspective = new List<WorldGenStep_Roads.Link>();
			List<PlanetTile> list = new List<PlanetTile>();
			for (int j = 0; j < indexToTile.Count; j++)
			{
				int srcLocal = j;
				PlanetTile srcTile = indexToTile[j];
				list.Clear();
				list.Add(srcTile);
				int found = 0;
				layer.Pather.FloodPathsWithCost(list, (PlanetTile src, PlanetTile dst) => Caravan_PathFollower.CostToMove(3300, src, dst, null, true, null, null, false), null, delegate(PlanetTile tile, float distance)
				{
					PlanetTile planetTile;
					int found;
					if (tile != srcTile && tileToIndexLookup.TryGetValue(tile, out planetTile))
					{
						found++;
						found = found;
						linkProspective.Add(new WorldGenStep_Roads.Link
						{
							distance = distance,
							indexA = srcLocal,
							indexB = planetTile.tileId
						});
					}
					return found >= 8;
				});
			}
			linkProspective.Sort((WorldGenStep_Roads.Link lhs, WorldGenStep_Roads.Link rhs) => lhs.distance.CompareTo(rhs.distance));
			return linkProspective;
		}

		// Token: 0x060153F4 RID: 87028 RVA: 0x0065B2B0 File Offset: 0x006594B0
		private List<WorldGenStep_Roads.Link> GenerateFinalLinks(List<WorldGenStep_Roads.Link> linkProspective, int endpointCount)
		{
			List<WorldGenStep_Roads.Connectedness> list = new List<WorldGenStep_Roads.Connectedness>();
			for (int i = 0; i < endpointCount; i++)
			{
				list.Add(new WorldGenStep_Roads.Connectedness());
			}
			List<WorldGenStep_Roads.Link> list2 = new List<WorldGenStep_Roads.Link>();
			for (int j = 0; j < linkProspective.Count; j++)
			{
				WorldGenStep_Roads.Link prospective = linkProspective[j];
				if (list[prospective.indexA].Group() != list[prospective.indexB].Group() || (Rand.Value <= 0.015f && !list2.Any((WorldGenStep_Roads.Link link) => link.indexB == prospective.indexA && link.indexA == prospective.indexB)))
				{
					if (Rand.Value > 0.1f)
					{
						list2.Add(prospective);
					}
					if (list[prospective.indexA].Group() != list[prospective.indexB].Group())
					{
						WorldGenStep_Roads.Connectedness parent = new WorldGenStep_Roads.Connectedness();
						list[prospective.indexA].Group().parent = parent;
						list[prospective.indexB].Group().parent = parent;
					}
				}
			}
			return list2;
		}

		// Token: 0x060153F5 RID: 87029 RVA: 0x0065B3F0 File Offset: 0x006595F0
		private void DrawLinksOnWorld(PlanetLayer layer, List<WorldGenStep_Roads.Link> linkFinal, List<PlanetTile> indexToTile)
		{
			foreach (WorldGenStep_Roads.Link link in linkFinal)
			{
				WorldPath worldPath = layer.Pather.FindPath(indexToTile[link.indexA], indexToTile[link.indexB], null, null);
				List<PlanetTile> nodesReversed = worldPath.NodesReversed;
				RoadDef roadDef = (from rd in DefDatabase<RoadDef>.AllDefsListForReading
				where !rd.ancientOnly
				select rd).RandomElementWithFallback(null);
				for (int i = 0; i < nodesReversed.Count - 1; i++)
				{
					Find.WorldGrid.OverlayRoad(nodesReversed[i], nodesReversed[i + 1], roadDef);
				}
				worldPath.ReleaseToPool();
			}
		}

		// Token: 0x0400ECDF RID: 60639
		private static readonly FloatRange ExtraRoadNodesPer100kTiles = new FloatRange(30f, 50f);

		// Token: 0x0400ECE0 RID: 60640
		private static readonly IntRange RoadDistanceFromSettlement = new IntRange(-4, 4);

		// Token: 0x0400ECE1 RID: 60641
		private const float ChanceExtraNonSpanningTreeLink = 0.015f;

		// Token: 0x0400ECE2 RID: 60642
		private const float ChanceHideSpanningTreeLink = 0.1f;

		// Token: 0x0400ECE3 RID: 60643
		private const float ChanceWorldObjectReclusive = 0.05f;

		// Token: 0x0400ECE4 RID: 60644
		private const int PotentialSpanningTreeLinksPerSettlement = 8;

		// Token: 0x02003D01 RID: 15617
		private struct Link
		{
			// Token: 0x0400ECE5 RID: 60645
			public float distance;

			// Token: 0x0400ECE6 RID: 60646
			public int indexA;

			// Token: 0x0400ECE7 RID: 60647
			public int indexB;
		}

		// Token: 0x02003D02 RID: 15618
		private class Connectedness
		{
			// Token: 0x060153F8 RID: 87032 RVA: 0x0065B4FB File Offset: 0x006596FB
			public WorldGenStep_Roads.Connectedness Group()
			{
				if (this.parent == null)
				{
					return this;
				}
				return this.parent.Group();
			}

			// Token: 0x0400ECE8 RID: 60648
			public WorldGenStep_Roads.Connectedness parent;
		}
	}
}
