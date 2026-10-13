using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003CC6 RID: 15558
	public static class GenPlanetMorphology
	{
		// Token: 0x0601526F RID: 86639 RVA: 0x00654D30 File Offset: 0x00652F30
		public static void Erode(PlanetLayer layer, List<PlanetTile> tiles, int count, Predicate<PlanetTile> extraPredicate = null)
		{
			if (count <= 0)
			{
				return;
			}
			WorldGrid worldGrid = Find.WorldGrid;
			GenPlanetMorphology.tilesSet.Clear();
			GenPlanetMorphology.tilesSet.AddRange(tiles);
			GenPlanetMorphology.tmpEdgeTiles.Clear();
			for (int i = 0; i < tiles.Count; i++)
			{
				worldGrid.GetTileNeighbors(tiles[i], GenPlanetMorphology.tmpNeighbors);
				for (int j = 0; j < GenPlanetMorphology.tmpNeighbors.Count; j++)
				{
					if (!GenPlanetMorphology.tilesSet.Contains(GenPlanetMorphology.tmpNeighbors[j]))
					{
						GenPlanetMorphology.tmpEdgeTiles.Add(tiles[i]);
						break;
					}
				}
			}
			if (!GenPlanetMorphology.tmpEdgeTiles.Any<PlanetTile>())
			{
				return;
			}
			GenPlanetMorphology.tmpOutput.Clear();
			Predicate<PlanetTile> passCheck;
			if (extraPredicate != null)
			{
				passCheck = ((PlanetTile x) => GenPlanetMorphology.tilesSet.Contains(x) && extraPredicate(x));
			}
			else
			{
				passCheck = ((PlanetTile x) => GenPlanetMorphology.tilesSet.Contains(x));
			}
			layer.Filler.FloodFill(PlanetTile.Invalid, passCheck, delegate(PlanetTile tile, int traversalDist)
			{
				if (traversalDist >= count)
				{
					GenPlanetMorphology.tmpOutput.Add(tile);
				}
				return false;
			}, int.MaxValue, GenPlanetMorphology.tmpEdgeTiles);
			tiles.Clear();
			tiles.AddRange(GenPlanetMorphology.tmpOutput);
		}

		// Token: 0x06015270 RID: 86640 RVA: 0x00654E70 File Offset: 0x00653070
		public static void Dilate(PlanetLayer layer, List<PlanetTile> tiles, int count, Predicate<PlanetTile> extraPredicate = null)
		{
			if (count <= 0)
			{
				return;
			}
			WorldFloodFiller filler = layer.Filler;
			PlanetTile invalid = PlanetTile.Invalid;
			Predicate<PlanetTile> passCheck = extraPredicate;
			if (extraPredicate == null && (passCheck = GenPlanetMorphology.<>c.<>9__5_0) == null)
			{
				passCheck = (GenPlanetMorphology.<>c.<>9__5_0 = ((PlanetTile x) => true));
			}
			filler.FloodFill(invalid, passCheck, delegate(PlanetTile tile, int traversalDist)
			{
				if (traversalDist > count)
				{
					return true;
				}
				if (traversalDist != 0)
				{
					tiles.Add(tile);
				}
				return false;
			}, int.MaxValue, tiles);
		}

		// Token: 0x06015271 RID: 86641 RVA: 0x00654EE6 File Offset: 0x006530E6
		public static void Open(PlanetLayer layer, List<PlanetTile> tiles, int count)
		{
			GenPlanetMorphology.Erode(layer, tiles, count, null);
			GenPlanetMorphology.Dilate(layer, tiles, count, null);
		}

		// Token: 0x06015272 RID: 86642 RVA: 0x00654EFA File Offset: 0x006530FA
		public static void Close(PlanetLayer layer, List<PlanetTile> tiles, int count)
		{
			GenPlanetMorphology.Dilate(layer, tiles, count, null);
			GenPlanetMorphology.Erode(layer, tiles, count, null);
		}

		// Token: 0x0400EBFF RID: 60415
		private static readonly HashSet<PlanetTile> tmpOutput = new HashSet<PlanetTile>();

		// Token: 0x0400EC00 RID: 60416
		private static readonly HashSet<PlanetTile> tilesSet = new HashSet<PlanetTile>();

		// Token: 0x0400EC01 RID: 60417
		private static readonly List<PlanetTile> tmpEdgeTiles = new List<PlanetTile>();

		// Token: 0x0400EC02 RID: 60418
		private static readonly List<PlanetTile> tmpNeighbors = new List<PlanetTile>();
	}
}
