using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002F41 RID: 12097
	public class CompProperties_Initiatable : CompProperties
	{
		// Token: 0x060110F7 RID: 69879 RVA: 0x004E6E5A File Offset: 0x004E505A
		public CompProperties_Initiatable()
		{
			this.compClass = typeof(CompInitiatable);
		}

		// Token: 0x0400B131 RID: 45361
		public int initiationDelayTicks;
	}
}
