using System;

namespace RimWorld
{
	// Token: 0x020035AB RID: 13739
	[DefOf]
	public static class StatCategoryDefOf
	{
		// Token: 0x06013399 RID: 78745 RVA: 0x005B5BB4 File Offset: 0x005B3DB4
		static StatCategoryDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(StatCategoryDefOf));
		}

		// Token: 0x0400CF12 RID: 53010
		public static StatCategoryDef Basics;

		// Token: 0x0400CF13 RID: 53011
		public static StatCategoryDef BasicsImportant;

		// Token: 0x0400CF14 RID: 53012
		public static StatCategoryDef BasicsPawnImportant;

		// Token: 0x0400CF15 RID: 53013
		public static StatCategoryDef BasicsNonPawnImportant;

		// Token: 0x0400CF16 RID: 53014
		public static StatCategoryDef BasicsNonPawn;

		// Token: 0x0400CF17 RID: 53015
		public static StatCategoryDef BasicsPawn;

		// Token: 0x0400CF18 RID: 53016
		public static StatCategoryDef Animals;

		// Token: 0x0400CF19 RID: 53017
		public static StatCategoryDef AnimalProductivity;

		// Token: 0x0400CF1A RID: 53018
		public static StatCategoryDef Source;

		// Token: 0x0400CF1B RID: 53019
		[MayRequireBiotech]
		public static StatCategoryDef Mechanoid;

		// Token: 0x0400CF1C RID: 53020
		[MayRequireAnomaly]
		public static StatCategoryDef PsychicRituals;

		// Token: 0x0400CF1D RID: 53021
		[MayRequireAnomaly]
		public static StatCategoryDef Containment;

		// Token: 0x0400CF1E RID: 53022
		[MayRequireAnomaly]
		public static StatCategoryDef Serum;

		// Token: 0x0400CF1F RID: 53023
		public static StatCategoryDef Apparel;

		// Token: 0x0400CF20 RID: 53024
		public static StatCategoryDef Implant;

		// Token: 0x0400CF21 RID: 53025
		public static StatCategoryDef Weapon;

		// Token: 0x0400CF22 RID: 53026
		public static StatCategoryDef Weapon_Ranged;

		// Token: 0x0400CF23 RID: 53027
		public static StatCategoryDef Weapon_Melee;

		// Token: 0x0400CF24 RID: 53028
		public static StatCategoryDef Ability;

		// Token: 0x0400CF25 RID: 53029
		public static StatCategoryDef Building;

		// Token: 0x0400CF26 RID: 53030
		public static StatCategoryDef Terrain;

		// Token: 0x0400CF27 RID: 53031
		public static StatCategoryDef PawnResistances;

		// Token: 0x0400CF28 RID: 53032
		public static StatCategoryDef PawnPsyfocus;

		// Token: 0x0400CF29 RID: 53033
		public static StatCategoryDef PawnHealth;

		// Token: 0x0400CF2A RID: 53034
		public static StatCategoryDef PawnFood;

		// Token: 0x0400CF2B RID: 53035
		public static StatCategoryDef PawnWork;

		// Token: 0x0400CF2C RID: 53036
		public static StatCategoryDef PawnCombat;

		// Token: 0x0400CF2D RID: 53037
		public static StatCategoryDef PawnSocial;

		// Token: 0x0400CF2E RID: 53038
		public static StatCategoryDef PawnMisc;

		// Token: 0x0400CF2F RID: 53039
		public static StatCategoryDef EquippedStatOffsets;

		// Token: 0x0400CF30 RID: 53040
		public static StatCategoryDef StuffStatFactors;

		// Token: 0x0400CF31 RID: 53041
		public static StatCategoryDef StuffStatOffsets;

		// Token: 0x0400CF32 RID: 53042
		public static StatCategoryDef Surgery;

		// Token: 0x0400CF33 RID: 53043
		public static StatCategoryDef CapacityEffects;

		// Token: 0x0400CF34 RID: 53044
		public static StatCategoryDef Meditation;

		// Token: 0x0400CF35 RID: 53045
		public static StatCategoryDef Drug;

		// Token: 0x0400CF36 RID: 53046
		public static StatCategoryDef DrugAddiction;

		// Token: 0x0400CF37 RID: 53047
		public static StatCategoryDef Genetics;
	}
}
