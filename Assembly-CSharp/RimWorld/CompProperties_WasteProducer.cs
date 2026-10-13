using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002ED1 RID: 11985
	public class CompProperties_WasteProducer : CompProperties
	{
		// Token: 0x06010EC1 RID: 69313 RVA: 0x004DEF22 File Offset: 0x004DD122
		public CompProperties_WasteProducer()
		{
			this.compClass = typeof(CompWasteProducer);
		}

		// Token: 0x0400B00C RID: 45068
		public bool showContentsInInspectPane = true;
	}
}
