using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001FBC RID: 8124
	public abstract class GenStep_LargeRuins : GenStep_BaseRuins
	{
		// Token: 0x17001B46 RID: 6982
		// (get) Token: 0x0600B328 RID: 45864
		protected abstract IntRange RuinsMinMaxRange { get; }

		// Token: 0x17001B47 RID: 6983
		// (get) Token: 0x0600B329 RID: 45865 RVA: 0x00335411 File Offset: 0x00333611
		protected virtual IntVec2 MinSize { get; } = new IntVec2(40, 40);

		// Token: 0x17001B48 RID: 6984
		// (get) Token: 0x0600B32A RID: 45866 RVA: 0x00335419 File Offset: 0x00333619
		protected virtual IntVec2 MaxSize { get; } = new IntVec2(60, 60);

		// Token: 0x0600B32B RID: 45867 RVA: 0x00335424 File Offset: 0x00333624
		protected override IEnumerable<CellRect> GetRectOrder(IEnumerable<CellRect> rects, Map map)
		{
			return (from r in rects
			orderby (float)r.Area - (map.Center - r.CenterCell).Magnitude / 2f descending
			select r).Take(this.RuinsMinMaxRange.TrueMax);
		}

		// Token: 0x0600B32C RID: 45868 RVA: 0x00335464 File Offset: 0x00333664
		protected override IEnumerable<CellRect> GetRects(CellRect area, Map map)
		{
			MapGenFloatGrid elevation = MapGenerator.Elevation;
			List<CellRect> largestClearRects = MapGenUtility.GetLargestClearRects(map, this.MinSize, this.MaxSize, this.UseUsedRects, -1f, 0.7f, this.MinAffordance, true);
			if (largestClearRects.Count < this.RuinsMinMaxRange.TrueMin)
			{
				largestClearRects = MapGenUtility.GetLargestClearRects(map, this.MinSize, this.MaxSize, this.UseUsedRects, -1f, 1f, null, true);
				if (largestClearRects.Count < this.RuinsMinMaxRange.TrueMin)
				{
					largestClearRects = MapGenUtility.GetLargestClearRects(map, this.MinSize, this.MaxSize, this.UseUsedRects, -1f, 1f, null, false);
				}
				while ((float)largestClearRects.Count > this.RuinsMinMaxRange.Average)
				{
					largestClearRects.RemoveAt(largestClearRects.Count - 1);
				}
			}
			return largestClearRects;
		}
	}
}
