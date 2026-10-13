using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse.AI
{
	// Token: 0x02000FBE RID: 4030
	public class RegionCostCalculator
	{
		// Token: 0x0600632B RID: 25387 RVA: 0x001F4C94 File Offset: 0x001F2E94
		public RegionCostCalculator(Map map)
		{
			this.map = map;
			this.preciseRegionLinkDistancesDistanceGetter = new Func<int, int, float>(this.PreciseRegionLinkDistancesDistanceGetter);
		}

		// Token: 0x0600632C RID: 25388 RVA: 0x001F4D08 File Offset: 0x001F2F08
		public void Init(CellRect destination, HashSet<Region> destRegions, TraverseParms parms, float moveTicksCardinal, float moveTicksDiagonal, ByteGrid avoidGrid, Area allowedArea, bool drafted)
		{
			this.regionGrid = this.map.regionGrid.DirectGrid;
			this.traverseParms = parms;
			this.pathingContext = this.map.pathing.For(parms);
			this.destinationCell = destination.CenterCell;
			this.moveTicksCardinal = moveTicksCardinal;
			this.moveTicksDiagonal = moveTicksDiagonal;
			this.avoidGrid = avoidGrid;
			this.allowedArea = allowedArea;
			this.drafted = drafted;
			this.regionMinLink.Clear();
			this.distances.Clear();
			this.linkTargetCells.Clear();
			this.queue.Clear();
			this.minPathCosts.Clear();
			foreach (Region region in destRegions)
			{
				int minPathCost = this.RegionMedianPathCost(region);
				for (int i = 0; i < region.links.Count; i++)
				{
					RegionLink regionLink = region.links[i];
					if (regionLink.GetOtherRegion(region).Allows(this.traverseParms, false))
					{
						int num = this.RegionLinkDistance(this.destinationCell, regionLink, minPathCost);
						int num2;
						if (this.distances.TryGetValue(regionLink, out num2))
						{
							if (num < num2)
							{
								this.linkTargetCells[regionLink] = this.GetLinkTargetCell(this.destinationCell, regionLink);
							}
							num = Math.Min(num2, num);
						}
						else
						{
							this.linkTargetCells[regionLink] = this.GetLinkTargetCell(this.destinationCell, regionLink);
						}
						this.distances[regionLink] = num;
					}
				}
				this.GetPreciseRegionLinkDistances(region, destination, this.preciseRegionLinkDistances);
				for (int j = 0; j < this.preciseRegionLinkDistances.Count; j++)
				{
					Pair<RegionLink, int> pair = this.preciseRegionLinkDistances[j];
					RegionLink first = pair.First;
					int num3 = this.distances[first];
					int num4;
					if (pair.Second > num3)
					{
						this.distances[first] = pair.Second;
						num4 = pair.Second;
					}
					else
					{
						num4 = num3;
					}
					this.queue.Push(new RegionCostCalculator.RegionLinkQueueEntry(region, first, num4, num4));
				}
			}
		}

		// Token: 0x0600632D RID: 25389 RVA: 0x001F4F5C File Offset: 0x001F315C
		public int GetRegionDistance(Region region, out RegionLink minLink)
		{
			if (this.regionMinLink.TryGetValue(region.id, out minLink))
			{
				return this.distances[minLink];
			}
			while (this.queue.Count != 0)
			{
				RegionCostCalculator.RegionLinkQueueEntry regionLinkQueueEntry = this.queue.Pop();
				int num = this.distances[regionLinkQueueEntry.Link];
				if (regionLinkQueueEntry.Cost == num)
				{
					Region otherRegion = regionLinkQueueEntry.Link.GetOtherRegion(regionLinkQueueEntry.From);
					if (otherRegion != null && otherRegion.valid)
					{
						int num2 = 0;
						if (otherRegion.door != null)
						{
							num2 = (int)PathUtility.GetBuildingCost(otherRegion.door, this.traverseParms, this.traverseParms.pawn, null);
							if (num2 == 65535)
							{
								continue;
							}
							num2 += this.OctileDistance(1, 0);
						}
						int minPathCost = this.RegionMedianPathCost(otherRegion);
						for (int i = 0; i < otherRegion.links.Count; i++)
						{
							RegionLink regionLink = otherRegion.links[i];
							if (regionLink != regionLinkQueueEntry.Link && regionLink.GetOtherRegion(otherRegion).Allows(this.traverseParms, false))
							{
								int num3 = (otherRegion.door != null) ? num2 : this.RegionLinkDistance(regionLinkQueueEntry.Link, regionLink, minPathCost);
								num3 = Math.Max(num3, 1);
								int num4 = num + num3;
								int estimatedPathCost = this.MinimumRegionLinkDistance(this.destinationCell, regionLink) + num4;
								int num5;
								if (this.distances.TryGetValue(regionLink, out num5))
								{
									if (num4 < num5)
									{
										this.distances[regionLink] = num4;
										this.queue.Push(new RegionCostCalculator.RegionLinkQueueEntry(otherRegion, regionLink, num4, estimatedPathCost));
									}
								}
								else
								{
									this.distances.Add(regionLink, num4);
									this.queue.Push(new RegionCostCalculator.RegionLinkQueueEntry(otherRegion, regionLink, num4, estimatedPathCost));
								}
							}
						}
						if (!this.regionMinLink.ContainsKey(otherRegion.id))
						{
							this.regionMinLink.Add(otherRegion.id, regionLinkQueueEntry.Link);
							if (otherRegion == region)
							{
								minLink = regionLinkQueueEntry.Link;
								return regionLinkQueueEntry.Cost;
							}
						}
					}
				}
			}
			return 10000;
		}

		// Token: 0x0600632E RID: 25390 RVA: 0x001F5180 File Offset: 0x001F3380
		public int GetRegionBestDistances(Region region, out RegionLink bestLink, out RegionLink secondBestLink, out int secondBestCost)
		{
			int regionDistance = this.GetRegionDistance(region, out bestLink);
			secondBestLink = null;
			secondBestCost = int.MaxValue;
			for (int i = 0; i < region.links.Count; i++)
			{
				RegionLink regionLink = region.links[i];
				int num;
				if (regionLink != bestLink && regionLink.GetOtherRegion(region).Allows(this.traverseParms, false) && this.distances.TryGetValue(regionLink, out num) && num < secondBestCost)
				{
					secondBestCost = num;
					secondBestLink = regionLink;
				}
			}
			return regionDistance;
		}

		// Token: 0x0600632F RID: 25391 RVA: 0x001F51FC File Offset: 0x001F33FC
		public int RegionMedianPathCost(Region region)
		{
			int result;
			if (this.minPathCosts.TryGetValue(region, out result))
			{
				return result;
			}
			bool ignoreAllowedAreaCost = this.allowedArea != null && region.OverlapWith(this.allowedArea) > AreaOverlap.None;
			CellIndices cellIndices = this.map.cellIndices;
			Rand.PushState();
			Rand.Seed = cellIndices.CellToIndex(region.extentsClose.CenterCell) * (region.links.Count + 1);
			for (int i = 0; i < 11; i++)
			{
				RegionCostCalculator.pathCostSamples[i] = this.GetCellCostFast(cellIndices.CellToIndex(region.RandomCell), ignoreAllowedAreaCost);
			}
			Rand.PopState();
			Array.Sort<int>(RegionCostCalculator.pathCostSamples);
			return this.minPathCosts[region] = RegionCostCalculator.pathCostSamples[4];
		}

		// Token: 0x06006330 RID: 25392 RVA: 0x001F52BC File Offset: 0x001F34BC
		private int GetCellCostFast(int index, bool ignoreAllowedAreaCost = false)
		{
			int num = this.pathingContext.pathGrid.Grid_Unsafe[index];
			if (this.avoidGrid != null)
			{
				num += (int)(this.avoidGrid[index] * 8);
			}
			if (this.allowedArea != null && !ignoreAllowedAreaCost && !this.allowedArea[index])
			{
				num += 600;
			}
			if (this.drafted)
			{
				num += this.map.terrainGrid.topGrid[index].extraDraftedPerceivedPathCost;
			}
			else
			{
				num += this.map.terrainGrid.topGrid[index].extraNonDraftedPerceivedPathCost;
			}
			return num;
		}

		// Token: 0x06006331 RID: 25393 RVA: 0x001F535C File Offset: 0x001F355C
		private int RegionLinkDistance(RegionLink a, RegionLink b, int minPathCost)
		{
			IntVec3 intVec;
			IntVec3 a2 = this.linkTargetCells.TryGetValue(a, out intVec) ? intVec : RegionCostCalculator.RegionLinkCenter(a);
			IntVec3 intVec2;
			IntVec3 b2 = this.linkTargetCells.TryGetValue(b, out intVec2) ? intVec2 : RegionCostCalculator.RegionLinkCenter(b);
			IntVec3 intVec3 = a2 - b2;
			int num = Math.Abs(intVec3.x);
			int num2 = Math.Abs(intVec3.z);
			return this.OctileDistance(num, num2) + minPathCost * Math.Max(num, num2) + minPathCost * Math.Min(num, num2);
		}

		// Token: 0x06006332 RID: 25394 RVA: 0x001F53DC File Offset: 0x001F35DC
		public int RegionLinkDistance(IntVec3 cell, RegionLink link, int minPathCost)
		{
			IntVec3 linkTargetCell = this.GetLinkTargetCell(cell, link);
			IntVec3 intVec = cell - linkTargetCell;
			int num = Math.Abs(intVec.x);
			int num2 = Math.Abs(intVec.z);
			return this.OctileDistance(num, num2) + minPathCost * Math.Max(num, num2) + minPathCost * Math.Min(num, num2);
		}

		// Token: 0x06006333 RID: 25395 RVA: 0x001F542C File Offset: 0x001F362C
		private static int SpanCenterX(EdgeSpan e)
		{
			return e.root.x + ((e.dir == SpanDirection.East) ? (e.length / 2) : 0);
		}

		// Token: 0x06006334 RID: 25396 RVA: 0x001F544E File Offset: 0x001F364E
		private static int SpanCenterZ(EdgeSpan e)
		{
			return e.root.z + ((e.dir == SpanDirection.North) ? (e.length / 2) : 0);
		}

		// Token: 0x06006335 RID: 25397 RVA: 0x001F546F File Offset: 0x001F366F
		private static IntVec3 RegionLinkCenter(RegionLink link)
		{
			return new IntVec3(RegionCostCalculator.SpanCenterX(link.span), 0, RegionCostCalculator.SpanCenterZ(link.span));
		}

		// Token: 0x06006336 RID: 25398 RVA: 0x001F5490 File Offset: 0x001F3690
		private int MinimumRegionLinkDistance(IntVec3 cell, RegionLink link)
		{
			IntVec3 intVec = cell - RegionCostCalculator.LinkClosestCell(cell, link);
			return this.OctileDistance(Math.Abs(intVec.x), Math.Abs(intVec.z));
		}

		// Token: 0x06006337 RID: 25399 RVA: 0x001F54C7 File Offset: 0x001F36C7
		private int OctileDistance(int dx, int dz)
		{
			return GenMath.OctileDistance(dx, dz, Mathf.RoundToInt(this.moveTicksCardinal), Mathf.RoundToInt(this.moveTicksDiagonal));
		}

		// Token: 0x06006338 RID: 25400 RVA: 0x001F54E6 File Offset: 0x001F36E6
		private IntVec3 GetLinkTargetCell(IntVec3 cell, RegionLink link)
		{
			return RegionCostCalculator.LinkClosestCell(cell, link);
		}

		// Token: 0x06006339 RID: 25401 RVA: 0x001F54F0 File Offset: 0x001F36F0
		private static IntVec3 LinkClosestCell(IntVec3 cell, RegionLink link)
		{
			EdgeSpan span = link.span;
			int num = 0;
			int num2 = 0;
			if (span.dir == SpanDirection.North)
			{
				num2 = span.length - 1;
			}
			else
			{
				num = span.length - 1;
			}
			IntVec3 root = span.root;
			return new IntVec3(Mathf.Clamp(cell.x, root.x, root.x + num), 0, Mathf.Clamp(cell.z, root.z, root.z + num2));
		}

		// Token: 0x0600633A RID: 25402 RVA: 0x001F5564 File Offset: 0x001F3764
		private void GetPreciseRegionLinkDistances(Region region, CellRect destination, List<Pair<RegionLink, int>> outDistances)
		{
			outDistances.Clear();
			RegionCostCalculator.tmpCellIndices.Clear();
			if (destination.Width == 1 && destination.Height == 1)
			{
				RegionCostCalculator.tmpCellIndices.Add(this.map.cellIndices.CellToIndex(destination.CenterCell));
			}
			else
			{
				foreach (IntVec3 c in destination)
				{
					if (c.InBounds(this.map))
					{
						RegionCostCalculator.tmpCellIndices.Add(this.map.cellIndices.CellToIndex(c));
					}
				}
			}
			Dijkstra<int>.Run(RegionCostCalculator.tmpCellIndices, (int x) => this.PreciseRegionLinkDistancesNeighborsGetter(x, region), this.preciseRegionLinkDistancesDistanceGetter, RegionCostCalculator.tmpDistances, null);
			for (int i = 0; i < region.links.Count; i++)
			{
				RegionLink regionLink = region.links[i];
				if (regionLink.GetOtherRegion(region).Allows(this.traverseParms, false))
				{
					float num;
					if (!RegionCostCalculator.tmpDistances.TryGetValue(this.map.cellIndices.CellToIndex(this.linkTargetCells[regionLink]), out num))
					{
						Log.ErrorOnce("Dijkstra couldn't reach one of the cells even though they are in the same region. There is most likely something wrong with the neighbor nodes getter.", 1938471531);
						num = 100f;
					}
					outDistances.Add(new Pair<RegionLink, int>(regionLink, (int)num));
				}
			}
		}

		// Token: 0x0600633B RID: 25403 RVA: 0x001F56F4 File Offset: 0x001F38F4
		private IEnumerable<int> PreciseRegionLinkDistancesNeighborsGetter(int node, Region region)
		{
			if (this.regionGrid[node] == null || this.regionGrid[node] != region)
			{
				return null;
			}
			return this.PathableNeighborIndices(node);
		}

		// Token: 0x0600633C RID: 25404 RVA: 0x001F5714 File Offset: 0x001F3914
		private float PreciseRegionLinkDistancesDistanceGetter(int a, int b)
		{
			return (float)this.GetCellCostFast(b, false) + (this.AreCellsDiagonal(a, b) ? this.moveTicksDiagonal : this.moveTicksCardinal);
		}

		// Token: 0x0600633D RID: 25405 RVA: 0x001F5738 File Offset: 0x001F3938
		private bool AreCellsDiagonal(int a, int b)
		{
			int x = this.map.Size.x;
			return a % x != b % x && a / x != b / x;
		}

		// Token: 0x0600633E RID: 25406 RVA: 0x001F576C File Offset: 0x001F396C
		private List<int> PathableNeighborIndices(int index)
		{
			RegionCostCalculator.tmpPathableNeighborIndices.Clear();
			PathGrid pathGrid = this.pathingContext.pathGrid;
			int x = this.map.Size.x;
			bool flag = index % x > 0;
			bool flag2 = index % x < x - 1;
			bool flag3 = index >= x;
			bool flag4 = index / x < this.map.Size.z - 1;
			if (flag3 && pathGrid.WalkableFast(index - x))
			{
				RegionCostCalculator.tmpPathableNeighborIndices.Add(index - x);
			}
			if (flag2 && pathGrid.WalkableFast(index + 1))
			{
				RegionCostCalculator.tmpPathableNeighborIndices.Add(index + 1);
			}
			if (flag && pathGrid.WalkableFast(index - 1))
			{
				RegionCostCalculator.tmpPathableNeighborIndices.Add(index - 1);
			}
			if (flag4 && pathGrid.WalkableFast(index + x))
			{
				RegionCostCalculator.tmpPathableNeighborIndices.Add(index + x);
			}
			bool canBashFences = this.traverseParms.canBashFences;
			bool flag5 = !flag || PathUtility.BlocksDiagonalMovement(index - 1, this.pathingContext, canBashFences);
			bool flag6 = !flag2 || PathUtility.BlocksDiagonalMovement(index + 1, this.pathingContext, canBashFences);
			if (flag3 && !PathUtility.BlocksDiagonalMovement(index - x, this.pathingContext, canBashFences))
			{
				if (!flag6 && pathGrid.WalkableFast(index - x + 1))
				{
					RegionCostCalculator.tmpPathableNeighborIndices.Add(index - x + 1);
				}
				if (!flag5 && pathGrid.WalkableFast(index - x - 1))
				{
					RegionCostCalculator.tmpPathableNeighborIndices.Add(index - x - 1);
				}
			}
			if (flag4 && !PathUtility.BlocksDiagonalMovement(index + x, this.pathingContext, canBashFences))
			{
				if (!flag6 && pathGrid.WalkableFast(index + x + 1))
				{
					RegionCostCalculator.tmpPathableNeighborIndices.Add(index + x + 1);
				}
				if (!flag5 && pathGrid.WalkableFast(index + x - 1))
				{
					RegionCostCalculator.tmpPathableNeighborIndices.Add(index + x - 1);
				}
			}
			return RegionCostCalculator.tmpPathableNeighborIndices;
		}

		// Token: 0x040046D8 RID: 18136
		private readonly Map map;

		// Token: 0x040046D9 RID: 18137
		private Region[] regionGrid;

		// Token: 0x040046DA RID: 18138
		private TraverseParms traverseParms;

		// Token: 0x040046DB RID: 18139
		private PathingContext pathingContext;

		// Token: 0x040046DC RID: 18140
		private IntVec3 destinationCell;

		// Token: 0x040046DD RID: 18141
		private float moveTicksCardinal;

		// Token: 0x040046DE RID: 18142
		private float moveTicksDiagonal;

		// Token: 0x040046DF RID: 18143
		private ByteGrid avoidGrid;

		// Token: 0x040046E0 RID: 18144
		private Area allowedArea;

		// Token: 0x040046E1 RID: 18145
		private bool drafted;

		// Token: 0x040046E2 RID: 18146
		private readonly Func<int, int, float> preciseRegionLinkDistancesDistanceGetter;

		// Token: 0x040046E3 RID: 18147
		private readonly Dictionary<int, RegionLink> regionMinLink = new Dictionary<int, RegionLink>();

		// Token: 0x040046E4 RID: 18148
		private readonly Dictionary<RegionLink, int> distances = new Dictionary<RegionLink, int>();

		// Token: 0x040046E5 RID: 18149
		private readonly FastPriorityQueue<RegionCostCalculator.RegionLinkQueueEntry> queue = new FastPriorityQueue<RegionCostCalculator.RegionLinkQueueEntry>(new RegionCostCalculator.DistanceComparer());

		// Token: 0x040046E6 RID: 18150
		private readonly Dictionary<Region, int> minPathCosts = new Dictionary<Region, int>();

		// Token: 0x040046E7 RID: 18151
		private readonly List<Pair<RegionLink, int>> preciseRegionLinkDistances = new List<Pair<RegionLink, int>>();

		// Token: 0x040046E8 RID: 18152
		private readonly Dictionary<RegionLink, IntVec3> linkTargetCells = new Dictionary<RegionLink, IntVec3>();

		// Token: 0x040046E9 RID: 18153
		private const int SampleCount = 11;

		// Token: 0x040046EA RID: 18154
		private static readonly int[] pathCostSamples = new int[11];

		// Token: 0x040046EB RID: 18155
		private static readonly List<int> tmpCellIndices = new List<int>();

		// Token: 0x040046EC RID: 18156
		private static readonly Dictionary<int, float> tmpDistances = new Dictionary<int, float>();

		// Token: 0x040046ED RID: 18157
		private static readonly List<int> tmpPathableNeighborIndices = new List<int>();

		// Token: 0x02000FBF RID: 4031
		private struct RegionLinkQueueEntry
		{
			// Token: 0x17000F9D RID: 3997
			// (get) Token: 0x06006340 RID: 25408 RVA: 0x001F5955 File Offset: 0x001F3B55
			public Region From
			{
				get
				{
					return this.from;
				}
			}

			// Token: 0x17000F9E RID: 3998
			// (get) Token: 0x06006341 RID: 25409 RVA: 0x001F595D File Offset: 0x001F3B5D
			public RegionLink Link
			{
				get
				{
					return this.link;
				}
			}

			// Token: 0x17000F9F RID: 3999
			// (get) Token: 0x06006342 RID: 25410 RVA: 0x001F5965 File Offset: 0x001F3B65
			public int Cost
			{
				get
				{
					return this.cost;
				}
			}

			// Token: 0x17000FA0 RID: 4000
			// (get) Token: 0x06006343 RID: 25411 RVA: 0x001F596D File Offset: 0x001F3B6D
			public int EstimatedPathCost
			{
				get
				{
					return this.estimatedPathCost;
				}
			}

			// Token: 0x06006344 RID: 25412 RVA: 0x001F5975 File Offset: 0x001F3B75
			public RegionLinkQueueEntry(Region from, RegionLink link, int cost, int estimatedPathCost)
			{
				this.from = from;
				this.link = link;
				this.cost = cost;
				this.estimatedPathCost = estimatedPathCost;
			}

			// Token: 0x040046EE RID: 18158
			private readonly Region from;

			// Token: 0x040046EF RID: 18159
			private readonly RegionLink link;

			// Token: 0x040046F0 RID: 18160
			private readonly int cost;

			// Token: 0x040046F1 RID: 18161
			private readonly int estimatedPathCost;
		}

		// Token: 0x02000FC0 RID: 4032
		private class DistanceComparer : IComparer<RegionCostCalculator.RegionLinkQueueEntry>
		{
			// Token: 0x06006345 RID: 25413 RVA: 0x001F5994 File Offset: 0x001F3B94
			public int Compare(RegionCostCalculator.RegionLinkQueueEntry a, RegionCostCalculator.RegionLinkQueueEntry b)
			{
				return a.EstimatedPathCost.CompareTo(b.EstimatedPathCost);
			}
		}
	}
}
