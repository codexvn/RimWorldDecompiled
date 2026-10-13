using System;

namespace Verse
{
	// Token: 0x020004DF RID: 1247
	public struct FloodFillRange
	{
		// Token: 0x06002040 RID: 8256 RVA: 0x000A898B File Offset: 0x000A6B8B
		public FloodFillRange(int minX, int maxX, int y)
		{
			this.minX = minX;
			this.maxX = maxX;
			this.z = y;
		}

		// Token: 0x04001B8C RID: 7052
		public int minX;

		// Token: 0x04001B8D RID: 7053
		public int maxX;

		// Token: 0x04001B8E RID: 7054
		public int z;
	}
}
