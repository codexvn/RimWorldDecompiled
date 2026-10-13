using System;

namespace RimWorld
{
	// Token: 0x020027EF RID: 10223
	public class CompProperties_RitualEffectConstantCircle : CompProperties_RitualVisualEffect
	{
		// Token: 0x0600DFD2 RID: 57298 RVA: 0x0041BB2C File Offset: 0x00419D2C
		public CompProperties_RitualEffectConstantCircle()
		{
			this.compClass = typeof(CompRitualEffect_ConstantCircle);
		}

		// Token: 0x040096A3 RID: 38563
		public float radius = 5f;

		// Token: 0x040096A4 RID: 38564
		public int numCopies = 5;
	}
}
