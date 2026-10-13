using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002CB5 RID: 11445
	public class CompProperties_GiveHediffSeverity : CompProperties
	{
		// Token: 0x06010111 RID: 65809 RVA: 0x004A62E7 File Offset: 0x004A44E7
		public CompProperties_GiveHediffSeverity()
		{
			this.compClass = typeof(CompGiveHediffSeverity);
		}

		// Token: 0x0400A7DD RID: 42973
		public HediffDef hediff;

		// Token: 0x0400A7DE RID: 42974
		public float range;

		// Token: 0x0400A7DF RID: 42975
		public float severityPerSecond;

		// Token: 0x0400A7E0 RID: 42976
		public bool drugExposure;

		// Token: 0x0400A7E1 RID: 42977
		public ChemicalDef chemical;

		// Token: 0x0400A7E2 RID: 42978
		public bool allowMechs = true;
	}
}
