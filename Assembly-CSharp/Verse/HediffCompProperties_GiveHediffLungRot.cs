using System;

namespace Verse
{
	// Token: 0x020006BE RID: 1726
	public class HediffCompProperties_GiveHediffLungRot : HediffCompProperties_GiveHediff
	{
		// Token: 0x06002B15 RID: 11029 RVA: 0x000E1A02 File Offset: 0x000DFC02
		public HediffCompProperties_GiveHediffLungRot()
		{
			this.compClass = typeof(HediffComp_GiveHediffLungRot);
		}

		// Token: 0x040022C5 RID: 8901
		public SimpleCurve mtbOverRotGasExposureCurve;

		// Token: 0x040022C6 RID: 8902
		public float minSeverity = 0.5f;

		// Token: 0x040022C7 RID: 8903
		public int mtbCheckDuration = 60;
	}
}
