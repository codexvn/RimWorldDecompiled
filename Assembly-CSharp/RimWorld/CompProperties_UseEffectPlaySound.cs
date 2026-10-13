using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002F6A RID: 12138
	public class CompProperties_UseEffectPlaySound : CompProperties_UseEffect
	{
		// Token: 0x060111A0 RID: 70048 RVA: 0x004E8FD3 File Offset: 0x004E71D3
		public CompProperties_UseEffectPlaySound()
		{
			this.compClass = typeof(CompUseEffect_PlaySound);
		}

		// Token: 0x0400B183 RID: 45443
		public SoundDef soundOnUsed;
	}
}
