using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020035D2 RID: 13778
	[DefOf]
	public static class TrainabilityDefOf
	{
		// Token: 0x060133C0 RID: 78784 RVA: 0x005B5E4B File Offset: 0x005B404B
		static TrainabilityDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(TrainabilityDefOf));
		}

		// Token: 0x0400D321 RID: 54049
		public static TrainabilityDef None;

		// Token: 0x0400D322 RID: 54050
		public static TrainabilityDef Intermediate;

		// Token: 0x0400D323 RID: 54051
		public static TrainabilityDef Advanced;
	}
}
