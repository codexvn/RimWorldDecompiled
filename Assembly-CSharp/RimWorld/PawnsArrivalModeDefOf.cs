using System;

namespace RimWorld
{
	// Token: 0x020035DF RID: 13791
	[DefOf]
	public static class PawnsArrivalModeDefOf
	{
		// Token: 0x060133CD RID: 78797 RVA: 0x005B5F28 File Offset: 0x005B4128
		static PawnsArrivalModeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(PawnsArrivalModeDefOf));
		}

		// Token: 0x0400D382 RID: 54146
		public static PawnsArrivalModeDef EdgeWalkIn;

		// Token: 0x0400D383 RID: 54147
		public static PawnsArrivalModeDef EdgeWalkInGroups;

		// Token: 0x0400D384 RID: 54148
		public static PawnsArrivalModeDef EdgeWalkInDistributed;

		// Token: 0x0400D385 RID: 54149
		public static PawnsArrivalModeDef CenterDrop;

		// Token: 0x0400D386 RID: 54150
		public static PawnsArrivalModeDef EdgeDrop;

		// Token: 0x0400D387 RID: 54151
		public static PawnsArrivalModeDef SpecificDropDebug;

		// Token: 0x0400D388 RID: 54152
		public static PawnsArrivalModeDef EmergeFromWater;

		// Token: 0x0400D389 RID: 54153
		public static PawnsArrivalModeDef RandomDrop;

		// Token: 0x0400D38A RID: 54154
		[MayRequireAnomaly]
		public static PawnsArrivalModeDef EdgeWalkInHateChanters;

		// Token: 0x0400D38B RID: 54155
		[MayRequireAnomaly]
		public static PawnsArrivalModeDef EdgeWalkInDistributedGroups;

		// Token: 0x0400D38C RID: 54156
		[MayRequireAnomaly]
		public static PawnsArrivalModeDef EdgeWalkInDarkness;
	}
}
