using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020004A4 RID: 1188
	public class ScatteredPrefabs
	{
		// Token: 0x04001ABD RID: 6845
		public List<GenStep_ScatterGroupPrefabs.ScatterGroup> prefabs = new List<GenStep_ScatterGroupPrefabs.ScatterGroup>();

		// Token: 0x04001ABE RID: 6846
		public FloatRange countPer10kCellsRange = FloatRange.Zero;

		// Token: 0x04001ABF RID: 6847
		public int minSpacing;

		// Token: 0x04001AC0 RID: 6848
		public float maxDistFromStructure;
	}
}
