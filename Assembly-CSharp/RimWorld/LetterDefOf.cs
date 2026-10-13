using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020035D4 RID: 13780
	[DefOf]
	public static class LetterDefOf
	{
		// Token: 0x060133C2 RID: 78786 RVA: 0x005B5E6D File Offset: 0x005B406D
		static LetterDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(LetterDefOf));
		}

		// Token: 0x0400D32A RID: 54058
		public static LetterDef ThreatBig;

		// Token: 0x0400D32B RID: 54059
		public static LetterDef ThreatSmall;

		// Token: 0x0400D32C RID: 54060
		public static LetterDef NegativeEvent;

		// Token: 0x0400D32D RID: 54061
		public static LetterDef NeutralEvent;

		// Token: 0x0400D32E RID: 54062
		public static LetterDef PositiveEvent;

		// Token: 0x0400D32F RID: 54063
		public static LetterDef Death;

		// Token: 0x0400D330 RID: 54064
		public static LetterDef AcceptVisitors;

		// Token: 0x0400D331 RID: 54065
		public static LetterDef AcceptJoiner;

		// Token: 0x0400D332 RID: 54066
		public static LetterDef GameEnded;

		// Token: 0x0400D333 RID: 54067
		public static LetterDef ChoosePawn;

		// Token: 0x0400D334 RID: 54068
		public static LetterDef RitualOutcomeNegative;

		// Token: 0x0400D335 RID: 54069
		public static LetterDef RitualOutcomePositive;

		// Token: 0x0400D336 RID: 54070
		[MayRequireIdeology]
		public static LetterDef RelicHuntInstallationFound;

		// Token: 0x0400D337 RID: 54071
		[MayRequireBiotech]
		public static LetterDef BabyBirth;

		// Token: 0x0400D338 RID: 54072
		[MayRequireBiotech]
		public static LetterDef BabyToChild;

		// Token: 0x0400D339 RID: 54073
		[MayRequireBiotech]
		public static LetterDef ChildToAdult;

		// Token: 0x0400D33A RID: 54074
		[MayRequireBiotech]
		public static LetterDef ChildBirthday;

		// Token: 0x0400D33B RID: 54075
		[MayRequireBiotech]
		public static LetterDef Bossgroup;

		// Token: 0x0400D33C RID: 54076
		[MayRequireAnomaly]
		public static LetterDef AcceptCreepJoiner;

		// Token: 0x0400D33D RID: 54077
		[MayRequireAnomaly]
		public static LetterDef EntityDiscovered;

		// Token: 0x0400D33E RID: 54078
		public static LetterDef BundleLetter;
	}
}
