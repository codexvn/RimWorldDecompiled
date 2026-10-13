using System;

namespace RimWorld
{
	// Token: 0x020035CC RID: 13772
	[DefOf]
	public static class PrisonerInteractionModeDefOf
	{
		// Token: 0x060133BA RID: 78778 RVA: 0x005B5DE5 File Offset: 0x005B3FE5
		static PrisonerInteractionModeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(PrisonerInteractionModeDefOf));
		}

		// Token: 0x0400D306 RID: 54022
		public static PrisonerInteractionModeDef MaintainOnly;

		// Token: 0x0400D307 RID: 54023
		public static PrisonerInteractionModeDef AttemptRecruit;

		// Token: 0x0400D308 RID: 54024
		public static PrisonerInteractionModeDef ReduceResistance;

		// Token: 0x0400D309 RID: 54025
		public static PrisonerInteractionModeDef Release;

		// Token: 0x0400D30A RID: 54026
		public static PrisonerInteractionModeDef Execution;

		// Token: 0x0400D30B RID: 54027
		[MayRequireIdeology]
		public static PrisonerInteractionModeDef Enslave;

		// Token: 0x0400D30C RID: 54028
		[MayRequireIdeology]
		public static PrisonerInteractionModeDef ReduceWill;

		// Token: 0x0400D30D RID: 54029
		[MayRequireIdeology]
		public static PrisonerInteractionModeDef Convert;

		// Token: 0x0400D30E RID: 54030
		[MayRequireBiotech]
		public static PrisonerInteractionModeDef Bloodfeed;

		// Token: 0x0400D30F RID: 54031
		[MayRequireBiotech]
		public static PrisonerInteractionModeDef HemogenFarm;

		// Token: 0x0400D310 RID: 54032
		[MayRequireAnomaly]
		public static PrisonerInteractionModeDef Study;

		// Token: 0x0400D311 RID: 54033
		[MayRequireAnomaly]
		public static PrisonerInteractionModeDef Interrogate;
	}
}
