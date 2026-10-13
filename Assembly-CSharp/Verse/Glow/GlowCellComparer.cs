using System;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;

namespace Verse.Glow
{
	// Token: 0x02000DF3 RID: 3571
	public struct GlowCellComparer : IComparer<IntVec3>
	{
		// Token: 0x06005ADA RID: 23258 RVA: 0x001D259E File Offset: 0x001D079E
		public GlowCellComparer(UnsafeList<GlowCell> area, in GlowLight light)
		{
			this.area = area;
			this.light = light;
		}

		// Token: 0x06005ADB RID: 23259 RVA: 0x001D25B4 File Offset: 0x001D07B4
		public int Compare(IntVec3 a, IntVec3 b)
		{
			int index = this.light.DeltaToLocalIndex(a);
			int index2 = this.light.DeltaToLocalIndex(b);
			return this.area[index].intDist.CompareTo(this.area[index2].intDist);
		}

		// Token: 0x040041C5 RID: 16837
		private UnsafeList<GlowCell> area;

		// Token: 0x040041C6 RID: 16838
		private GlowLight light;
	}
}
