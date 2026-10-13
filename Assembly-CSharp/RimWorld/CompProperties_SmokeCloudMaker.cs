using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002E1D RID: 11805
	public class CompProperties_SmokeCloudMaker : CompProperties
	{
		// Token: 0x06010A0A RID: 68106 RVA: 0x004CBA5F File Offset: 0x004C9C5F
		public CompProperties_SmokeCloudMaker()
		{
			this.compClass = typeof(CompSmokeCloudMaker);
		}

		// Token: 0x0400AD47 RID: 44359
		public EffecterDef sourceStreamEffect;

		// Token: 0x0400AD48 RID: 44360
		public float cloudRadius;

		// Token: 0x0400AD49 RID: 44361
		public FleckDef cloudFleck;

		// Token: 0x0400AD4A RID: 44362
		public float fleckScale = 1f;

		// Token: 0x0400AD4B RID: 44363
		public float fleckSpawnMTB;
	}
}
