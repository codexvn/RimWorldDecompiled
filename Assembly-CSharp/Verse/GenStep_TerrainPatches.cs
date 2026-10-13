using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200049A RID: 1178
	public class GenStep_TerrainPatches : GenStep
	{
		// Token: 0x1700058F RID: 1423
		// (get) Token: 0x06001EF0 RID: 7920 RVA: 0x000A1A03 File Offset: 0x0009FC03
		public override int SeedPart
		{
			get
			{
				return 1370184742;
			}
		}

		// Token: 0x06001EF1 RID: 7921 RVA: 0x000A1A0C File Offset: 0x0009FC0C
		public override void Generate(Map map, GenStepParams parms)
		{
			int num = Mathf.RoundToInt((float)map.Area / 10000f * this.patchesPer10kCellsRange.RandomInRange);
			using (map.pathing.DisableIncrementalScope())
			{
				for (int i = 0; i < num; i++)
				{
					float randomInRange = this.patchSizeRange.RandomInRange;
					IntVec3 a = CellFinder.RandomCell(map);
					foreach (IntVec3 b in GenRadial.RadialPatternInRadius(randomInRange / 2f))
					{
						IntVec3 c = a + b;
						if (c.InBounds(map))
						{
							map.terrainGrid.SetTerrain(c, this.terrainDef);
						}
					}
				}
			}
		}

		// Token: 0x04001A7A RID: 6778
		public TerrainDef terrainDef;

		// Token: 0x04001A7B RID: 6779
		public FloatRange patchesPer10kCellsRange;

		// Token: 0x04001A7C RID: 6780
		public FloatRange patchSizeRange;
	}
}
