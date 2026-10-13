using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002D69 RID: 11625
	public class CompProperties_MoteEmitterProximityScan : CompProperties_MoteEmitter
	{
		// Token: 0x06010559 RID: 66905 RVA: 0x004B71C7 File Offset: 0x004B53C7
		public CompProperties_MoteEmitterProximityScan()
		{
			this.compClass = typeof(CompMoteEmitterProximityScan);
		}

		// Token: 0x0400AA65 RID: 43621
		public float warmupPulseFadeInTime;

		// Token: 0x0400AA66 RID: 43622
		public float warmupPulseSolidTime;

		// Token: 0x0400AA67 RID: 43623
		public float warmupPulseFadeOutTime;

		// Token: 0x0400AA68 RID: 43624
		public SoundDef soundEmitting;
	}
}
