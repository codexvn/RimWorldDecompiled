using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002CAB RID: 11435
	public class CompProperties_GasOnDestroyed : CompProperties
	{
		// Token: 0x060100D3 RID: 65747 RVA: 0x004A59CC File Offset: 0x004A3BCC
		public CompProperties_GasOnDestroyed()
		{
			this.compClass = typeof(CompGasOnDestroyed);
		}

		// Token: 0x0400A7C9 RID: 42953
		public GasType gasType;

		// Token: 0x0400A7CA RID: 42954
		public int amount;
	}
}
