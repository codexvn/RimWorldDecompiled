using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002C87 RID: 11399
	public class CompProperties_FireBurst : CompProperties
	{
		// Token: 0x0600FFDF RID: 65503 RVA: 0x004A20D5 File Offset: 0x004A02D5
		public CompProperties_FireBurst()
		{
			this.compClass = typeof(CompFireBurst);
		}

		// Token: 0x0400A754 RID: 42836
		public float radius = 6f;

		// Token: 0x0400A755 RID: 42837
		public int ticksAwayFromDetonate = 17;
	}
}
