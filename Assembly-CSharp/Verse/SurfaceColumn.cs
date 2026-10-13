using System;

namespace Verse
{
	// Token: 0x02000C85 RID: 3205
	public struct SurfaceColumn
	{
		// Token: 0x06005240 RID: 21056 RVA: 0x001A9E1F File Offset: 0x001A801F
		public SurfaceColumn(float x, SimpleCurve y)
		{
			this.x = x;
			this.y = y;
		}

		// Token: 0x04003AF7 RID: 15095
		public float x;

		// Token: 0x04003AF8 RID: 15096
		public SimpleCurve y;
	}
}
