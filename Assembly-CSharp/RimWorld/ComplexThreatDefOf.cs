using System;

namespace RimWorld
{
	// Token: 0x02003601 RID: 13825
	[DefOf]
	public static class ComplexThreatDefOf
	{
		// Token: 0x060133EE RID: 78830 RVA: 0x005B6159 File Offset: 0x005B4359
		static ComplexThreatDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(ComplexThreatDefOf));
		}

		// Token: 0x0400D502 RID: 54530
		[MayRequireIdeology]
		public static ComplexThreatDef SleepingInsects;

		// Token: 0x0400D503 RID: 54531
		[MayRequireIdeology]
		public static ComplexThreatDef Infestation;

		// Token: 0x0400D504 RID: 54532
		[MayRequireIdeology]
		public static ComplexThreatDef SleepingMechanoids;

		// Token: 0x0400D505 RID: 54533
		[MayRequireIdeology]
		public static ComplexThreatDef CryptosleepPods;
	}
}
