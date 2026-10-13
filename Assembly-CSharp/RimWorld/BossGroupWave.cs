using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A0D RID: 6669
	public class BossGroupWave
	{
		// Token: 0x040062AE RID: 25262
		public int bossCount;

		// Token: 0x040062AF RID: 25263
		public List<ThingDef> bossApparel;

		// Token: 0x040062B0 RID: 25264
		public List<PawnKindDefCount> escorts = new List<PawnKindDefCount>();
	}
}
