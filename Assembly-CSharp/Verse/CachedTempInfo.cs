using System;

namespace Verse
{
	// Token: 0x02000539 RID: 1337
	public struct CachedTempInfo
	{
		// Token: 0x060022BF RID: 8895 RVA: 0x000B4D04 File Offset: 0x000B2F04
		public static CachedTempInfo NewCachedTempInfo()
		{
			CachedTempInfo result = default(CachedTempInfo);
			result.Reset();
			return result;
		}

		// Token: 0x060022C0 RID: 8896 RVA: 0x000B4D21 File Offset: 0x000B2F21
		public void Reset()
		{
			this.roomID = -1;
			this.numCells = 0;
			this.temperature = 0f;
			this.vacuum = 0f;
		}

		// Token: 0x060022C1 RID: 8897 RVA: 0x000B4D47 File Offset: 0x000B2F47
		public CachedTempInfo(int roomID, int numCells, float temperature, float vacuum)
		{
			this.roomID = roomID;
			this.numCells = numCells;
			this.temperature = temperature;
			this.vacuum = vacuum;
		}

		// Token: 0x04001D18 RID: 7448
		public int roomID;

		// Token: 0x04001D19 RID: 7449
		public int numCells;

		// Token: 0x04001D1A RID: 7450
		public float temperature;

		// Token: 0x04001D1B RID: 7451
		public float vacuum;
	}
}
