using System;

namespace RimWorld
{
	// Token: 0x020035C4 RID: 13764
	[DefOf]
	public static class PawnGroupKindDefOf
	{
		// Token: 0x060133B2 RID: 78770 RVA: 0x005B5D5D File Offset: 0x005B3F5D
		static PawnGroupKindDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(PawnGroupKindDefOf));
		}

		// Token: 0x0400D2AE RID: 53934
		public static PawnGroupKindDef Combat;

		// Token: 0x0400D2AF RID: 53935
		public static PawnGroupKindDef Trader;

		// Token: 0x0400D2B0 RID: 53936
		public static PawnGroupKindDef Peaceful;

		// Token: 0x0400D2B1 RID: 53937
		public static PawnGroupKindDef Settlement;

		// Token: 0x0400D2B2 RID: 53938
		public static PawnGroupKindDef Settlement_RangedOnly;

		// Token: 0x0400D2B3 RID: 53939
		[MayRequireIdeology]
		public static PawnGroupKindDef Miners;

		// Token: 0x0400D2B4 RID: 53940
		[MayRequireIdeology]
		public static PawnGroupKindDef Farmers;

		// Token: 0x0400D2B5 RID: 53941
		[MayRequireIdeology]
		public static PawnGroupKindDef Loggers;

		// Token: 0x0400D2B6 RID: 53942
		[MayRequireIdeology]
		public static PawnGroupKindDef Hunters;

		// Token: 0x0400D2B7 RID: 53943
		[MayRequireAnomaly]
		public static PawnGroupKindDef Shamblers;

		// Token: 0x0400D2B8 RID: 53944
		[MayRequireAnomaly]
		public static PawnGroupKindDef Fleshbeasts;

		// Token: 0x0400D2B9 RID: 53945
		[MayRequireAnomaly]
		public static PawnGroupKindDef FleshbeastsWithDreadmeld;

		// Token: 0x0400D2BA RID: 53946
		[MayRequireAnomaly]
		public static PawnGroupKindDef Sightstealers;

		// Token: 0x0400D2BB RID: 53947
		[MayRequireAnomaly]
		public static PawnGroupKindDef Metalhorrors;

		// Token: 0x0400D2BC RID: 53948
		[MayRequireAnomaly]
		public static PawnGroupKindDef PsychicRitualSiege;

		// Token: 0x0400D2BD RID: 53949
		[MayRequireAnomaly]
		public static PawnGroupKindDef Gorehulks;

		// Token: 0x0400D2BE RID: 53950
		[MayRequireAnomaly]
		public static PawnGroupKindDef Devourers;

		// Token: 0x0400D2BF RID: 53951
		[MayRequireAnomaly]
		public static PawnGroupKindDef Noctols;

		// Token: 0x0400D2C0 RID: 53952
		[MayRequireAnomaly]
		public static PawnGroupKindDef SightstealersNoctols;

		// Token: 0x0400D2C1 RID: 53953
		[MayRequireAnomaly]
		public static PawnGroupKindDef Chimeras;
	}
}
