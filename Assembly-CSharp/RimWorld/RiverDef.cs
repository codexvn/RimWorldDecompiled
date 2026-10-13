using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001AD4 RID: 6868
	public class RiverDef : Def
	{
		// Token: 0x0400692E RID: 26926
		public int spawnFlowThreshold = -1;

		// Token: 0x0400692F RID: 26927
		public float spawnChance = 1f;

		// Token: 0x04006930 RID: 26928
		public int degradeThreshold;

		// Token: 0x04006931 RID: 26929
		public RiverDef degradeChild;

		// Token: 0x04006932 RID: 26930
		public List<RiverDef.Branch> branches;

		// Token: 0x04006933 RID: 26931
		public float widthOnWorld = 0.5f;

		// Token: 0x04006934 RID: 26932
		public float widthOnMap = 10f;

		// Token: 0x04006935 RID: 26933
		public float debugOpacity;

		// Token: 0x02001AD5 RID: 6869
		public class Branch
		{
			// Token: 0x04006936 RID: 26934
			public int minFlow;

			// Token: 0x04006937 RID: 26935
			public RiverDef child;

			// Token: 0x04006938 RID: 26936
			public float chance = 1f;
		}
	}
}
