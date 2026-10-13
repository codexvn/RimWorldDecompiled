using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020035C7 RID: 13767
	[DefOf]
	public static class ResearchProjectDefOf
	{
		// Token: 0x060133B5 RID: 78773 RVA: 0x005B5D90 File Offset: 0x005B3F90
		static ResearchProjectDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(ResearchProjectDefOf));
		}

		// Token: 0x0400D2D7 RID: 53975
		public static ResearchProjectDef CarpetMaking;

		// Token: 0x0400D2D8 RID: 53976
		public static ResearchProjectDef MicroelectronicsBasics;

		// Token: 0x0400D2D9 RID: 53977
		public static ResearchProjectDef Batteries;

		// Token: 0x0400D2DA RID: 53978
		public static ResearchProjectDef ColoredLights;

		// Token: 0x0400D2DB RID: 53979
		[MayRequireBiotech]
		public static ResearchProjectDef BasicMechtech;

		// Token: 0x0400D2DC RID: 53980
		[MayRequireBiotech]
		public static ResearchProjectDef Archogenetics;

		// Token: 0x0400D2DD RID: 53981
		[MayRequireAnomaly]
		public static ResearchProjectDef BasicPsychicRituals;

		// Token: 0x0400D2DE RID: 53982
		[MayRequireAnomaly]
		public static ResearchProjectDef AdvancedPsychicRituals;

		// Token: 0x0400D2DF RID: 53983
		[MayRequireAnomaly]
		public static ResearchProjectDef BioferriteExtraction;

		// Token: 0x0400D2E0 RID: 53984
		[MayRequireOdyssey]
		public static ResearchProjectDef Fishing;

		// Token: 0x0400D2E1 RID: 53985
		[MayRequireOdyssey]
		public static ResearchProjectDef BasicGravtech;
	}
}
