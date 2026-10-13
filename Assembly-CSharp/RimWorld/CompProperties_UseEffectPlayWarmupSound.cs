using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002F6C RID: 12140
	public class CompProperties_UseEffectPlayWarmupSound : CompProperties_UseEffect
	{
		// Token: 0x060111A4 RID: 70052 RVA: 0x004E9030 File Offset: 0x004E7230
		public CompProperties_UseEffectPlayWarmupSound()
		{
			this.compClass = typeof(CompUseEffect_PlayWarmupSound);
		}

		// Token: 0x0400B184 RID: 45444
		public SoundDef warmupSound;
	}
}
