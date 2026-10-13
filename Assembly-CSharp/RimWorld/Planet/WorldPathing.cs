using System;
using System.Collections.Generic;
using LudeonTK;
using Unity.Collections;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003C16 RID: 15382
	public class WorldPathing : IDisposable
	{
		// Token: 0x170033A1 RID: 13217
		// (get) Token: 0x06014DDB RID: 85467 RVA: 0x0063F752 File Offset: 0x0063D952
		public PlanetLayer Layer { get; }

		// Token: 0x06014DDC RID: 85468 RVA: 0x0063F75C File Offset: 0x0063D95C
		public WorldPathing(PlanetLayer layer)
		{
			this.Layer = layer;
			this.calcGrid = new NativeArray<WorldPathing.PathFinderNodeFast>(layer.TilesCount, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			this.frontier = new NativePriorityQueue<int, int, IntMinComparer>(Mathf.Min(layer.TilesCount, 500000), default(IntMinComparer), Allocator.Persistent);
		}

		// Token: 0x06014DDD RID: 85469 RVA: 0x0063F7B0 File Offset: 0x0063D9B0
		public WorldPath FindPath(PlanetTile startTile, PlanetTile destTile, Caravan caravan, Func<float, bool> terminator = null)
		{
			if (!startTile.Valid)
			{
				Log.Error(string.Format("Tried to FindPath with invalid start tile {0}, caravan = {1}", startTile, caravan));
				return WorldPath.NotFound;
			}
			if (!destTile.Valid)
			{
				Log.Error(string.Format("Tried to FindPath with invalid dest tile {0}, caravan = {1}", destTile, caravan));
				return WorldPath.NotFound;
			}
			if (startTile.Layer != destTile.Layer)
			{
				Log.Error(string.Format("Tried to FindPath to a different layer {0} -> {1}, caravan = {2}", startTile, destTile, caravan));
				return WorldPath.NotFound;
			}
			if (caravan != null)
			{
				if (!caravan.CanReach(destTile))
				{
					return WorldPath.NotFound;
				}
			}
			else if (!Find.WorldReachability.CanReach(startTile, destTile))
			{
				return WorldPath.NotFound;
			}
			PlanetTile planetTile = startTile;
			World world = Find.World;
			WorldGrid grid = world.grid;
			NativeArray<int> unsafeTileIDToNeighbors_offsets = startTile.Layer.UnsafeTileIDToNeighbors_offsets;
			NativeArray<PlanetTile> unsafeTileIDToNeighbors_values = startTile.Layer.UnsafeTileIDToNeighbors_values;
			Vector3 normalized = grid.GetTileCenter(destTile).normalized;
			float[] array = world.pathGrid.layerMovementDifficulty[startTile.Layer];
			int num = 0;
			int num2 = (caravan != null) ? caravan.TicksPerMove : 3300;
			int num3 = this.CalculateHeuristicStrength(startTile, destTile);
			this.frontier.Clear();
			this.calcGrid.Clear<WorldPathing.PathFinderNodeFast>();
			this.InitalizeSearch(planetTile);
			while (this.frontier.Count > 0)
			{
				int num4;
				int num5;
				this.frontier.Dequeue(out num4, out num5);
				WorldPathing.PathFinderNodeFast pathFinderNodeFast = this.calcGrid[num4];
				if (num5 == pathFinderNodeFast.costNodeCost)
				{
					planetTile = new PlanetTile(num4, this.Layer);
					if (pathFinderNodeFast.status != WorldPathing.PathFinderNodeFast.Status.Closed)
					{
						if (planetTile == destTile)
						{
							return this.FinalizedPath(planetTile);
						}
						if (num > 500000)
						{
							Log.Warning(string.Format("{0} pathing from {1} to {2} hit search limit of {3} tiles.", new object[]
							{
								caravan,
								startTile,
								destTile,
								500000
							}));
							return WorldPath.NotFound;
						}
						ValueTuple<int, int> listIndexes = PackedListOfLists.GetListIndexes<PlanetTile>(unsafeTileIDToNeighbors_offsets, unsafeTileIDToNeighbors_values, planetTile.tileId);
						int item = listIndexes.Item1;
						int item2 = listIndexes.Item2;
						for (int i = item; i < item2; i++)
						{
							PlanetTile planetTile2 = unsafeTileIDToNeighbors_values[i];
							WorldPathing.PathFinderNodeFast pathFinderNodeFast2 = this.calcGrid[planetTile2.tileId];
							if (pathFinderNodeFast2.status != WorldPathing.PathFinderNodeFast.Status.Closed && !world.Impassable(planetTile2))
							{
								int num6 = (int)((float)num2 * array[planetTile2.tileId] * grid.GetRoadMovementDifficultyMultiplier(planetTile, planetTile2, null)) + pathFinderNodeFast.knownCost;
								WorldPathing.PathFinderNodeFast.Status status = this.calcGrid[planetTile2.tileId].status;
								if ((status != WorldPathing.PathFinderNodeFast.Status.Closed && status != WorldPathing.PathFinderNodeFast.Status.Open) || this.calcGrid[planetTile2.tileId].knownCost > num6)
								{
									Vector3 tileCenter = grid.GetTileCenter(planetTile2);
									if (status != WorldPathing.PathFinderNodeFast.Status.Closed && status != WorldPathing.PathFinderNodeFast.Status.Open)
									{
										float num7 = grid.ApproxDistanceInTiles(GenMath.SphericalDistance(tileCenter.normalized, normalized));
										pathFinderNodeFast2.heuristicCost = Mathf.RoundToInt((float)num2 * num7 * (float)num3 * 0.5f);
									}
									int num8 = num6 + this.calcGrid[planetTile2.tileId].heuristicCost;
									pathFinderNodeFast2.parentTile = planetTile;
									pathFinderNodeFast2.knownCost = num6;
									pathFinderNodeFast2.status = WorldPathing.PathFinderNodeFast.Status.Open;
									pathFinderNodeFast2.costNodeCost = num8;
									this.calcGrid[planetTile2.tileId] = pathFinderNodeFast2;
									this.frontier.Enqueue(planetTile2.tileId, num8);
								}
							}
						}
						num++;
						pathFinderNodeFast.status = WorldPathing.PathFinderNodeFast.Status.Closed;
						this.calcGrid[num4] = pathFinderNodeFast;
						if (terminator != null && terminator((float)pathFinderNodeFast.costNodeCost))
						{
							return WorldPath.NotFound;
						}
					}
				}
			}
			Log.Warning(string.Format("{0} pathing from {1} to {2} ran out of tiles to process.", caravan, startTile, destTile));
			return WorldPath.NotFound;
		}

		// Token: 0x06014DDE RID: 85470 RVA: 0x0063FB78 File Offset: 0x0063DD78
		private void InitalizeSearch(PlanetTile tile)
		{
			WorldPathing.PathFinderNodeFast value = this.calcGrid[tile.tileId];
			value.knownCost = 0;
			value.heuristicCost = 0;
			value.costNodeCost = 0;
			value.parentTile = tile;
			value.status = WorldPathing.PathFinderNodeFast.Status.Open;
			this.calcGrid[tile.tileId] = value;
			this.frontier.Clear();
			this.frontier.Enqueue(tile.tileId, 0);
		}

		// Token: 0x06014DDF RID: 85471 RVA: 0x0063FBF0 File Offset: 0x0063DDF0
		public void FloodPathsWithCost(List<PlanetTile> startTiles, Func<PlanetTile, PlanetTile, int> costFunc, Func<PlanetTile, bool> impassable = null, Func<PlanetTile, float, bool> terminator = null)
		{
			if (startTiles.Count < 1 || startTiles.Contains(PlanetTile.Invalid))
			{
				Log.Error("Tried to FindPath with invalid start tiles");
				return;
			}
			for (int i = 0; i < startTiles.Count; i++)
			{
				if (startTiles[i].Layer != this.Layer)
				{
					Log.Error(string.Format("Tried to FindPath with tiles on different layers, layer is {0} but tile at index {1} is on layer {2}", this.Layer, i, startTiles[i].Layer));
					return;
				}
			}
			NativeArray<int> unsafeTileIDToNeighbors_offsets = this.Layer.UnsafeTileIDToNeighbors_offsets;
			NativeArray<PlanetTile> unsafeTileIDToNeighbors_values = this.Layer.UnsafeTileIDToNeighbors_values;
			if (impassable == null)
			{
				impassable = ((PlanetTile tid) => Find.World.Impassable(tid));
			}
			this.calcGrid.Clear<WorldPathing.PathFinderNodeFast>();
			this.frontier.Clear();
			using (List<PlanetTile>.Enumerator enumerator = startTiles.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					PlanetTile planetTile = enumerator.Current;
					WorldPathing.PathFinderNodeFast value = this.calcGrid[planetTile.tileId];
					value.knownCost = 0;
					value.costNodeCost = 0;
					value.parentTile = planetTile;
					value.status = WorldPathing.PathFinderNodeFast.Status.Open;
					this.calcGrid[planetTile.tileId] = value;
					this.frontier.Enqueue(planetTile.tileId, 0);
				}
				goto IL_31B;
			}
			IL_151:
			int num;
			int num2;
			this.frontier.Dequeue(out num, out num2);
			WorldPathing.PathFinderNodeFast pathFinderNodeFast = this.calcGrid[num];
			if (num2 == pathFinderNodeFast.costNodeCost)
			{
				PlanetTile planetTile2 = new PlanetTile(num, this.Layer);
				if (this.calcGrid[planetTile2.tileId].status != WorldPathing.PathFinderNodeFast.Status.Closed)
				{
					ValueTuple<int, int> listIndexes = PackedListOfLists.GetListIndexes<PlanetTile>(unsafeTileIDToNeighbors_offsets, unsafeTileIDToNeighbors_values, planetTile2.tileId);
					int item = listIndexes.Item1;
					int item2 = listIndexes.Item2;
					for (int j = item; j < item2; j++)
					{
						PlanetTile planetTile3 = unsafeTileIDToNeighbors_values[j];
						WorldPathing.PathFinderNodeFast value2 = this.calcGrid[planetTile3.tileId];
						if (this.calcGrid[planetTile3.tileId].status != WorldPathing.PathFinderNodeFast.Status.Closed && !impassable(planetTile3))
						{
							int num3 = costFunc(planetTile2, planetTile3) + this.calcGrid[planetTile2.tileId].knownCost;
							WorldPathing.PathFinderNodeFast.Status status = this.calcGrid[planetTile3.tileId].status;
							if ((status != WorldPathing.PathFinderNodeFast.Status.Closed && status != WorldPathing.PathFinderNodeFast.Status.Open) || this.calcGrid[planetTile3.tileId].knownCost > num3)
							{
								int num4 = num3;
								value2.parentTile = planetTile2;
								value2.knownCost = num3;
								value2.status = WorldPathing.PathFinderNodeFast.Status.Open;
								value2.costNodeCost = num4;
								this.calcGrid[planetTile3.tileId] = value2;
								this.frontier.Enqueue(planetTile3.tileId, num4);
							}
						}
					}
					pathFinderNodeFast.status = WorldPathing.PathFinderNodeFast.Status.Closed;
					this.calcGrid[planetTile2.tileId] = pathFinderNodeFast;
					if (terminator != null && terminator(planetTile2, (float)this.calcGrid[planetTile2.tileId].costNodeCost))
					{
						return;
					}
				}
			}
			IL_31B:
			if (this.frontier.Count > 0)
			{
				goto IL_151;
			}
		}

		// Token: 0x06014DE0 RID: 85472 RVA: 0x0063FF3C File Offset: 0x0063E13C
		public List<PlanetTile>[] FloodPathsWithCostForTree(List<PlanetTile> startTiles, Func<PlanetTile, PlanetTile, int> costFunc, Func<PlanetTile, bool> impassable = null, Func<PlanetTile, float, bool> terminator = null)
		{
			this.FloodPathsWithCost(startTiles, costFunc, impassable, terminator);
			PlanetLayer layer = startTiles[0].Layer;
			List<PlanetTile>[] array = new List<PlanetTile>[layer.TilesCount];
			for (int i = 0; i < layer.TilesCount; i++)
			{
				if (this.calcGrid[i].status == WorldPathing.PathFinderNodeFast.Status.Closed)
				{
					PlanetTile planetTile = new PlanetTile(i, layer);
					PlanetTile parentTile = this.calcGrid[i].parentTile;
					if (parentTile != planetTile)
					{
						if (array[parentTile.tileId] == null)
						{
							array[parentTile.tileId] = new List<PlanetTile>();
						}
						array[parentTile.tileId].Add(planetTile);
					}
				}
			}
			return array;
		}

		// Token: 0x06014DE1 RID: 85473 RVA: 0x0063FFE8 File Offset: 0x0063E1E8
		private WorldPath FinalizedPath(PlanetTile lastTile)
		{
			WorldPath emptyWorldPath = Find.WorldPathPool.GetEmptyWorldPath();
			PlanetTile planetTile = lastTile;
			for (;;)
			{
				PlanetTile parentTile = this.calcGrid[planetTile.tileId].parentTile;
				PlanetTile planetTile2 = planetTile;
				emptyWorldPath.AddNodeAtStart(planetTile2);
				if (planetTile2 == parentTile)
				{
					break;
				}
				planetTile = parentTile;
			}
			emptyWorldPath.SetupFound((float)this.calcGrid[lastTile.tileId].knownCost, lastTile.Layer);
			return emptyWorldPath;
		}

		// Token: 0x06014DE2 RID: 85474 RVA: 0x00640054 File Offset: 0x0063E254
		private int CalculateHeuristicStrength(PlanetTile startTile, PlanetTile destTile)
		{
			float x = Find.WorldGrid.ApproxDistanceInTiles(startTile, destTile);
			return Mathf.RoundToInt(WorldPathing.HeuristicStrength_DistanceCurve.Evaluate(x));
		}

		// Token: 0x06014DE3 RID: 85475 RVA: 0x0064007E File Offset: 0x0063E27E
		public void Dispose()
		{
			ref this.calcGrid.EnsureDisposed<WorldPathing.PathFinderNodeFast>();
			ref this.frontier.EnsureDisposed<int, int, IntMinComparer>();
		}

		// Token: 0x0400E88B RID: 59531
		private NativePriorityQueue<int, int, IntMinComparer> frontier;

		// Token: 0x0400E88C RID: 59532
		private NativeArray<WorldPathing.PathFinderNodeFast> calcGrid;

		// Token: 0x0400E88D RID: 59533
		private const int SearchLimit = 500000;

		// Token: 0x0400E88E RID: 59534
		private static readonly SimpleCurve HeuristicStrength_DistanceCurve = new SimpleCurve
		{
			{
				new CurvePoint(30f, 1f),
				true
			},
			{
				new CurvePoint(40f, 1.3f),
				true
			},
			{
				new CurvePoint(130f, 2f),
				true
			}
		};

		// Token: 0x0400E88F RID: 59535
		private const float BestRoadDiscount = 0.5f;

		// Token: 0x02003C17 RID: 15383
		private struct PathFinderNodeFast
		{
			// Token: 0x0400E891 RID: 59537
			public int knownCost;

			// Token: 0x0400E892 RID: 59538
			public int heuristicCost;

			// Token: 0x0400E893 RID: 59539
			public PlanetTile parentTile;

			// Token: 0x0400E894 RID: 59540
			public int costNodeCost;

			// Token: 0x0400E895 RID: 59541
			public WorldPathing.PathFinderNodeFast.Status status;

			// Token: 0x02003C18 RID: 15384
			public enum Status : byte
			{
				// Token: 0x0400E897 RID: 59543
				None,
				// Token: 0x0400E898 RID: 59544
				Open,
				// Token: 0x0400E899 RID: 59545
				Closed
			}
		}
	}
}
