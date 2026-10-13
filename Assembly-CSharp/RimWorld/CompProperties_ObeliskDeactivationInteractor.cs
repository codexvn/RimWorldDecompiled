using System;

namespace RimWorld
{
	// Token: 0x02002D7B RID: 11643
	public class CompProperties_ObeliskDeactivationInteractor : CompProperties_Interactable
	{
		// Token: 0x060105DA RID: 67034 RVA: 0x004B9187 File Offset: 0x004B7387
		public CompProperties_ObeliskDeactivationInteractor()
		{
			this.compClass = typeof(CompObeliskDeactivationInteractor);
		}

		// Token: 0x0400AAC1 RID: 43713
		public int shardsRequired = 2;
	}
}
