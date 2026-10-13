using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002E73 RID: 11891
	public class CompProperties_TargetingBeam : CompProperties
	{
		// Token: 0x06010BF1 RID: 68593 RVA: 0x004D3823 File Offset: 0x004D1A23
		public CompProperties_TargetingBeam()
		{
			this.compClass = typeof(CompTargetingBeam);
		}

		// Token: 0x0400AE7D RID: 44669
		public int delayTicks;

		// Token: 0x0400AE7E RID: 44670
		public DamageDef damage;
	}
}
