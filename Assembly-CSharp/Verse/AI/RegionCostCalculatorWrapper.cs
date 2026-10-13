using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse.AI
{
	// Token: 0x02000FC2 RID: 4034
	public class RegionCostCalculatorWrapper
	{
		// Token: 0x06006349 RID: 25417 RVA: 0x001F59CB File Offset: 0x001F3BCB
		public RegionCostCalculatorWrapper(Map map)
		{
			this.map = map;
			this.regionCostCalculator = new RegionCostCalculator(map);
		}

		// Token: 0x0600634A RID: 25418 RVA: 0x001F59F4 File Offset: 0x001F3BF4
		public void Init(CellRect end, TraverseParms traverseParms, float moveTicksCardinal, float moveTicksDiagonal, ByteGrid avoidGrid, Area allowedArea, bool drafted, List<int> disallowedCorners)
		{
			this.moveTicksCardinal = moveTicksCardinal;
			this.moveTicksDiagonal = moveTicksDiagonal;
			this.endCell = end.CenterCell;
			this.cachedRegion = null;
			this.cachedBestLink = null;
			this.cachedSecondBestLink = null;
			this.cachedBestLinkCost = 0;
			this.cachedSecondBestLinkCost = 0;
			this.cachedRegionIsDestination = false;
			this.regionGrid = this.map.regionGrid.DirectGrid;
			this.destRegions.Clear();
			if (end.Width == 1 && end.Height == 1)
			{
				Region region = this.endCell.GetRegion(this.map, RegionType.Set_Passable);
				if (region != null)
				{
					this.destRegions.Add(region);
				}
			}
			else
			{
				foreach (IntVec3 intVec in end)
				{
					if (intVec.InBounds(this.map) && !disallowedCorners.Contains(this.map.cellIndices.CellToIndex(intVec)))
					{
						Region region2 = intVec.GetRegion(this.map, RegionType.Set_Passable);
						if (region2 != null && region2.Allows(traverseParms, true))
						{
							this.destRegions.Add(region2);
						}
					}
				}
			}
			if (this.destRegions.Count == 0)
			{
				Log.Error("Couldn't find any destination regions. This shouldn't ever happen because we've checked reachability.");
			}
			this.regionCostCalculator.Init(end, this.destRegions, traverseParms, moveTicksCardinal, moveTicksDiagonal, avoidGrid, allowedArea, drafted);
		}

		// Token: 0x0600634B RID: 25419 RVA: 0x001F5B64 File Offset: 0x001F3D64
		public int GetPathCostFromDestToRegion(int cellIndex)
		{
			Region region = this.regionGrid[cellIndex];
			IntVec3 cell = this.map.cellIndices.IndexToCell(cellIndex);
			if (region != this.cachedRegion)
			{
				this.cachedRegionIsDestination = this.destRegions.Contains(region);
				if (this.cachedRegionIsDestination)
				{
					return this.OctileDistanceToEnd(cell);
				}
				this.cachedBestLinkCost = this.regionCostCalculator.GetRegionBestDistances(region, out this.cachedBestLink, out this.cachedSecondBestLink, out this.cachedSecondBestLinkCost);
				this.cachedRegion = region;
			}
			else if (this.cachedRegionIsDestination)
			{
				return this.OctileDistanceToEnd(cell);
			}
			if (this.cachedBestLink != null)
			{
				int num = this.regionCostCalculator.RegionLinkDistance(cell, this.cachedBestLink, 1);
				int num3;
				if (this.cachedSecondBestLink != null)
				{
					int num2 = this.regionCostCalculator.RegionLinkDistance(cell, this.cachedSecondBestLink, 1);
					num3 = Mathf.Min(this.cachedSecondBestLinkCost + num2, this.cachedBestLinkCost + num);
				}
				else
				{
					num3 = this.cachedBestLinkCost + num;
				}
				return num3 + this.OctileDistanceToEndEps(cell);
			}
			return 10000;
		}

		// Token: 0x0600634C RID: 25420 RVA: 0x001F5C60 File Offset: 0x001F3E60
		private int OctileDistanceToEnd(IntVec3 cell)
		{
			int dx = Mathf.Abs(cell.x - this.endCell.x);
			int dz = Mathf.Abs(cell.z - this.endCell.z);
			return GenMath.OctileDistance(dx, dz, Mathf.RoundToInt(this.moveTicksCardinal), Mathf.RoundToInt(this.moveTicksDiagonal));
		}

		// Token: 0x0600634D RID: 25421 RVA: 0x001F5CB8 File Offset: 0x001F3EB8
		private int OctileDistanceToEndEps(IntVec3 cell)
		{
			int dx = Mathf.Abs(cell.x - this.endCell.x);
			int dz = Mathf.Abs(cell.z - this.endCell.z);
			return GenMath.OctileDistance(dx, dz, 2, 3);
		}

		// Token: 0x040046F4 RID: 18164
		private Map map;

		// Token: 0x040046F5 RID: 18165
		private IntVec3 endCell;

		// Token: 0x040046F6 RID: 18166
		private HashSet<Region> destRegions = new HashSet<Region>();

		// Token: 0x040046F7 RID: 18167
		private float moveTicksCardinal;

		// Token: 0x040046F8 RID: 18168
		private float moveTicksDiagonal;

		// Token: 0x040046F9 RID: 18169
		private RegionCostCalculator regionCostCalculator;

		// Token: 0x040046FA RID: 18170
		private Region cachedRegion;

		// Token: 0x040046FB RID: 18171
		private RegionLink cachedBestLink;

		// Token: 0x040046FC RID: 18172
		private RegionLink cachedSecondBestLink;

		// Token: 0x040046FD RID: 18173
		private int cachedBestLinkCost;

		// Token: 0x040046FE RID: 18174
		private int cachedSecondBestLinkCost;

		// Token: 0x040046FF RID: 18175
		private bool cachedRegionIsDestination;

		// Token: 0x04004700 RID: 18176
		private Region[] regionGrid;
	}
}
