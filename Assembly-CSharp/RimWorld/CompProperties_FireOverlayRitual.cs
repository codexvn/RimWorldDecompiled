using System;

namespace RimWorld
{
	// Token: 0x0200196F RID: 6511
	public class CompProperties_FireOverlayRitual : CompProperties_FireOverlay
	{
		// Token: 0x060093D6 RID: 37846 RVA: 0x002B79BB File Offset: 0x002B5BBB
		public CompProperties_FireOverlayRitual()
		{
			this.compClass = typeof(CompRitualFireOverlay);
		}

		// Token: 0x04005F5E RID: 24414
		public float minRitualProgress;
	}
}
