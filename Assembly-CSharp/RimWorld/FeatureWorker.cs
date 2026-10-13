using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A2E RID: 6702
	public abstract class FeatureWorker
	{
		// Token: 0x06009713 RID: 38675
		public abstract void GenerateWhereAppropriate(PlanetLayer layer);

		// Token: 0x06009714 RID: 38676 RVA: 0x002C279C File Offset: 0x002C099C
		protected void AddFeature(PlanetLayer layer, List<PlanetTile> members, List<PlanetTile> tilesForTextDrawPosCalculation)
		{
			WorldFeature worldFeature = new WorldFeature(this.def, layer);
			worldFeature.name = NameGenerator.GenerateName(this.def.nameMaker, from x in Find.WorldFeatures.features
			select x.name, false, "r_name");
			WorldGrid worldGrid = Find.WorldGrid;
			for (int i = 0; i < members.Count; i++)
			{
				worldGrid[members[i]].feature = worldFeature;
			}
			this.AssignBestDrawPos(layer, worldFeature, tilesForTextDrawPosCalculation);
			Find.WorldFeatures.features.Add(worldFeature);
		}

		// Token: 0x06009715 RID: 38677 RVA: 0x002C2844 File Offset: 0x002C0A44
		private void AssignBestDrawPos(PlanetLayer layer, WorldFeature newFeature, List<PlanetTile> tilesForTextDrawPosCalculation)
		{
			FeatureWorker.<>c__DisplayClass10_0 CS$<>8__locals1 = new FeatureWorker.<>c__DisplayClass10_0();
			WorldGrid worldGrid = Find.WorldGrid;
			FeatureWorker.tmpEdgeTiles.Clear();
			FeatureWorker.tmpTilesForTextDrawPosCalculationSet.Clear();
			FeatureWorker.tmpTilesForTextDrawPosCalculationSet.AddRange(tilesForTextDrawPosCalculation);
			Vector3 vector = Vector3.zero;
			for (int i = 0; i < tilesForTextDrawPosCalculation.Count; i++)
			{
				PlanetTile planetTile = tilesForTextDrawPosCalculation[i];
				vector += worldGrid.GetTileCenter(planetTile);
				bool flag = worldGrid.IsOnEdge(planetTile);
				if (!flag)
				{
					worldGrid.GetTileNeighbors(planetTile, FeatureWorker.tmpNeighbors);
					for (int j = 0; j < FeatureWorker.tmpNeighbors.Count; j++)
					{
						if (!FeatureWorker.tmpTilesForTextDrawPosCalculationSet.Contains(FeatureWorker.tmpNeighbors[j]))
						{
							flag = true;
							break;
						}
					}
				}
				if (flag)
				{
					FeatureWorker.tmpEdgeTiles.Add(planetTile);
				}
			}
			vector /= (float)tilesForTextDrawPosCalculation.Count;
			if (!FeatureWorker.tmpEdgeTiles.Any<PlanetTile>())
			{
				FeatureWorker.tmpEdgeTiles.Add(tilesForTextDrawPosCalculation.RandomElement<PlanetTile>());
			}
			CS$<>8__locals1.bestTileDist = 0;
			FeatureWorker.tmpTraversedTiles.Clear();
			layer.Filler.FloodFill(PlanetTile.Invalid, (PlanetTile tile) => FeatureWorker.tmpTilesForTextDrawPosCalculationSet.Contains(tile), new Predicate<PlanetTile, int>(CS$<>8__locals1.<AssignBestDrawPos>g__Process|0), int.MaxValue, FeatureWorker.tmpEdgeTiles);
			PlanetTile tile2 = PlanetTile.Invalid;
			float num = -1f;
			for (int k = 0; k < FeatureWorker.tmpTraversedTiles.Count; k++)
			{
				if (FeatureWorker.tmpTraversedTiles[k].Item2 == CS$<>8__locals1.bestTileDist)
				{
					float sqrMagnitude = (worldGrid.GetTileCenter(FeatureWorker.tmpTraversedTiles[k].Item1) - vector).sqrMagnitude;
					if (!tile2.Valid || sqrMagnitude < num)
					{
						tile2 = FeatureWorker.tmpTraversedTiles[k].Item1;
						num = sqrMagnitude;
					}
				}
			}
			float maxDrawSizeInTiles = (float)CS$<>8__locals1.bestTileDist * 2f * 1.2f;
			newFeature.drawCenter = worldGrid.GetTileCenter(tile2);
			newFeature.maxDrawSizeInTiles = maxDrawSizeInTiles;
		}

		// Token: 0x06009716 RID: 38678 RVA: 0x002C2A4D File Offset: 0x002C0C4D
		protected static void ClearVisited(PlanetLayer layer)
		{
			FeatureWorker.ClearOrCreate<bool>(layer, ref FeatureWorker.visited);
		}

		// Token: 0x06009717 RID: 38679 RVA: 0x002C2A5A File Offset: 0x002C0C5A
		protected static void ClearGroupSizes(PlanetLayer layer)
		{
			FeatureWorker.ClearOrCreate<int>(layer, ref FeatureWorker.groupSize);
		}

		// Token: 0x06009718 RID: 38680 RVA: 0x002C2A67 File Offset: 0x002C0C67
		protected static void ClearGroupIDs(PlanetLayer layer)
		{
			FeatureWorker.ClearOrCreate<int>(layer, ref FeatureWorker.groupID);
		}

		// Token: 0x06009719 RID: 38681 RVA: 0x002C2A74 File Offset: 0x002C0C74
		private static void ClearOrCreate<T>(PlanetLayer layer, ref T[] array)
		{
			int tilesCount = layer.TilesCount;
			if (array == null || array.Length != tilesCount)
			{
				array = new T[tilesCount];
				return;
			}
			Array.Clear(array, 0, array.Length);
		}

		// Token: 0x04006408 RID: 25608
		public FeatureDef def;

		// Token: 0x04006409 RID: 25609
		protected static bool[] visited;

		// Token: 0x0400640A RID: 25610
		protected static int[] groupSize;

		// Token: 0x0400640B RID: 25611
		protected static int[] groupID;

		// Token: 0x0400640C RID: 25612
		private static readonly List<PlanetTile> tmpNeighbors = new List<PlanetTile>();

		// Token: 0x0400640D RID: 25613
		private static readonly HashSet<PlanetTile> tmpTilesForTextDrawPosCalculationSet = new HashSet<PlanetTile>();

		// Token: 0x0400640E RID: 25614
		private static readonly List<PlanetTile> tmpEdgeTiles = new List<PlanetTile>();

		// Token: 0x0400640F RID: 25615
		[TupleElementNames(new string[]
		{
			"tile",
			"traversalDistance"
		})]
		private static readonly List<ValueTuple<PlanetTile, int>> tmpTraversedTiles = new List<ValueTuple<PlanetTile, int>>();
	}
}
