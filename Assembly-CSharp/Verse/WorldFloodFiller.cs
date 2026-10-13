using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using Unity.Collections;

namespace Verse
{
	// Token: 0x02000197 RID: 407
	public class WorldFloodFiller
	{
		// Token: 0x06000B6F RID: 2927 RVA: 0x0003634F File Offset: 0x0003454F
		public WorldFloodFiller(PlanetLayer layer)
		{
			this.layer = layer;
		}

		// Token: 0x06000B70 RID: 2928 RVA: 0x00036380 File Offset: 0x00034580
		public void FloodFill(PlanetTile rootTile, Predicate<PlanetTile> passCheck, Action<PlanetTile> processor, int maxTilesToProcess = 2147483647, IEnumerable<PlanetTile> extraRootTiles = null)
		{
			this.FloodFill(rootTile, passCheck, delegate(PlanetTile _, PlanetTile tile, int _)
			{
				processor(tile);
				return false;
			}, maxTilesToProcess, extraRootTiles);
		}

		// Token: 0x06000B71 RID: 2929 RVA: 0x000363B4 File Offset: 0x000345B4
		public void FloodFill(PlanetTile rootTile, Predicate<PlanetTile> passCheck, Action<PlanetTile, int> processor, int maxTilesToProcess = 2147483647, IEnumerable<PlanetTile> extraRootTiles = null)
		{
			this.FloodFill(rootTile, passCheck, delegate(PlanetTile _, PlanetTile tile, int dist)
			{
				processor(tile, dist);
				return false;
			}, maxTilesToProcess, extraRootTiles);
		}

		// Token: 0x06000B72 RID: 2930 RVA: 0x000363E8 File Offset: 0x000345E8
		public void FloodFill(PlanetTile rootTile, Predicate<PlanetTile> passCheck, Action<PlanetTile, PlanetTile, int> processor, int maxTilesToProcess = 2147483647, IEnumerable<PlanetTile> extraRootTiles = null)
		{
			this.FloodFill(rootTile, passCheck, delegate(PlanetTile from, PlanetTile tile, int dist)
			{
				processor(from, tile, dist);
				return false;
			}, maxTilesToProcess, extraRootTiles);
		}

		// Token: 0x06000B73 RID: 2931 RVA: 0x0003641C File Offset: 0x0003461C
		public void FloodFill(PlanetTile rootTile, Predicate<PlanetTile> passCheck, Predicate<PlanetTile> processor, int maxTilesToProcess = 2147483647, IEnumerable<PlanetTile> extraRootTiles = null)
		{
			this.FloodFill(rootTile, passCheck, (PlanetTile _, PlanetTile tile, int _) => processor(tile), maxTilesToProcess, extraRootTiles);
		}

		// Token: 0x06000B74 RID: 2932 RVA: 0x00036450 File Offset: 0x00034650
		public void FloodFill(PlanetTile rootTile, Predicate<PlanetTile> passCheck, Predicate<PlanetTile, int> processor, int maxTilesToProcess = 2147483647, IEnumerable<PlanetTile> extraRootTiles = null)
		{
			this.FloodFill(rootTile, passCheck, (PlanetTile _, PlanetTile tile, int dist) => processor(tile, dist), maxTilesToProcess, extraRootTiles);
		}

		// Token: 0x06000B75 RID: 2933 RVA: 0x00036484 File Offset: 0x00034684
		public void FloodFill(PlanetTile rootTile, Predicate<PlanetTile> passCheck, WorldFloodFiller.WorldFillPredicate processor, int maxTilesToProcess = 2147483647, IEnumerable<PlanetTile> extraRootTiles = null)
		{
			if (this.working)
			{
				Log.Error("Nested FloodFill calls are not allowed. This will cause bugs.");
			}
			using (ProfilerBlock.Scope("WorldFloodFill"))
			{
				this.working = true;
				this.ClearVisited();
				if (rootTile.Valid && extraRootTiles == null && !passCheck(rootTile))
				{
					this.working = false;
				}
				else
				{
					int tilesCount = rootTile.Layer.TilesCount;
					int num = tilesCount;
					if (this.traversalDistance.Count < tilesCount)
					{
						this.traversalDistance.Capacity = tilesCount;
						for (int i = this.traversalDistance.Count; i < tilesCount; i++)
						{
							this.traversalDistance.Add(-1);
						}
					}
					NativeArray<int> unsafeTileIDToNeighbors_offsets = this.layer.UnsafeTileIDToNeighbors_offsets;
					NativeArray<PlanetTile> unsafeTileIDToNeighbors_values = this.layer.UnsafeTileIDToNeighbors_values;
					int num2 = 0;
					if (unsafeTileIDToNeighbors_offsets.IsCreated)
					{
						this.openSet.Clear();
						if (rootTile.Valid)
						{
							this.visited.Add(rootTile);
							this.traversalDistance[rootTile.tileId] = 0;
							this.openSet.Enqueue(new ValueTuple<PlanetTile, PlanetTile>(PlanetTile.Invalid, rootTile));
						}
						if (extraRootTiles == null)
						{
							goto IL_30C;
						}
						foreach (PlanetTile item in extraRootTiles)
						{
							this.visited.Add(item);
						}
						IList<PlanetTile> list = extraRootTiles as IList<PlanetTile>;
						if (list != null)
						{
							for (int j = 0; j < list.Count; j++)
							{
								PlanetTile planetTile = list[j];
								this.traversalDistance[planetTile.tileId] = 0;
								this.openSet.Enqueue(new ValueTuple<PlanetTile, PlanetTile>(PlanetTile.Invalid, planetTile));
							}
							goto IL_30C;
						}
						using (IEnumerator<PlanetTile> enumerator = extraRootTiles.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								PlanetTile planetTile2 = enumerator.Current;
								this.traversalDistance[planetTile2.tileId] = 0;
								this.openSet.Enqueue(new ValueTuple<PlanetTile, PlanetTile>(PlanetTile.Invalid, planetTile2));
							}
							goto IL_30C;
						}
						IL_201:
						ValueTuple<PlanetTile, PlanetTile> valueTuple = this.openSet.Dequeue();
						PlanetTile item2 = valueTuple.Item1;
						PlanetTile item3 = valueTuple.Item2;
						int num3 = this.traversalDistance[item3.tileId];
						if (processor(item2, item3, num3))
						{
							goto IL_31D;
						}
						num2++;
						if (num2 == maxTilesToProcess)
						{
							goto IL_31D;
						}
						ValueTuple<int, int> listIndexes = PackedListOfLists.GetListIndexes<PlanetTile>(unsafeTileIDToNeighbors_offsets, unsafeTileIDToNeighbors_values, item3);
						int item4 = listIndexes.Item1;
						int item5 = listIndexes.Item2;
						for (int k = item4; k < item5; k++)
						{
							PlanetTile planetTile3 = unsafeTileIDToNeighbors_values[k];
							if (this.traversalDistance[planetTile3.tileId] == -1 && passCheck(planetTile3))
							{
								this.visited.Add(planetTile3);
								this.openSet.Enqueue(new ValueTuple<PlanetTile, PlanetTile>(item3, planetTile3));
								this.traversalDistance[planetTile3.tileId] = num3 + 1;
							}
						}
						if (this.openSet.Count > num)
						{
							Log.Error(string.Format("Overflow on world flood fill (>{0} cells). Make sure we're not flooding over the same area after we check it.", num));
							this.working = false;
							return;
						}
						IL_30C:
						if (this.openSet.Count > 0)
						{
							goto IL_201;
						}
						IL_31D:
						this.working = false;
					}
				}
			}
		}

		// Token: 0x06000B76 RID: 2934 RVA: 0x00036814 File Offset: 0x00034A14
		private void ClearVisited()
		{
			int i = 0;
			int count = this.visited.Count;
			while (i < count)
			{
				this.traversalDistance[this.visited[i].tileId] = -1;
				i++;
			}
			this.visited.Clear();
			this.openSet.Clear();
		}

		// Token: 0x04000692 RID: 1682
		private bool working;

		// Token: 0x04000693 RID: 1683
		private PlanetLayer layer;

		// Token: 0x04000694 RID: 1684
		[TupleElementNames(new string[]
		{
			"from",
			"to"
		})]
		private readonly Queue<ValueTuple<PlanetTile, PlanetTile>> openSet = new Queue<ValueTuple<PlanetTile, PlanetTile>>();

		// Token: 0x04000695 RID: 1685
		private readonly List<int> traversalDistance = new List<int>();

		// Token: 0x04000696 RID: 1686
		private readonly List<PlanetTile> visited = new List<PlanetTile>();

		// Token: 0x02000198 RID: 408
		// (Invoke) Token: 0x06000B78 RID: 2936
		public delegate bool WorldFillPredicate(PlanetTile from, PlanetTile to, int distance);
	}
}
