using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020019F7 RID: 6647
	public class TerrainThreshold
	{
		// Token: 0x06009610 RID: 38416 RVA: 0x002BEED0 File Offset: 0x002BD0D0
		public static TerrainDef TerrainAtValue(List<TerrainThreshold> threshes, float val)
		{
			for (int i = 0; i < threshes.Count; i++)
			{
				if (threshes[i].min <= val && threshes[i].max >= val)
				{
					return threshes[i].terrain;
				}
			}
			return null;
		}

		// Token: 0x04006219 RID: 25113
		public TerrainDef terrain;

		// Token: 0x0400621A RID: 25114
		public float min = -1000f;

		// Token: 0x0400621B RID: 25115
		public float max = 1000f;
	}
}
