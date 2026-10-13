using System;

namespace RimWorld
{
	// Token: 0x02002F67 RID: 12135
	public class CompProperties_UseEffectOffsetDeathrestCapacity : CompProperties_UseEffect
	{
		// Token: 0x06011192 RID: 70034 RVA: 0x004E8E39 File Offset: 0x004E7039
		public CompProperties_UseEffectOffsetDeathrestCapacity()
		{
			this.compClass = typeof(CompUseEffect_OffsetDeathrestCapacity);
		}

		// Token: 0x0400B17E RID: 45438
		public int offset;
	}
}
