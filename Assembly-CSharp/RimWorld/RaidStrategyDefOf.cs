using System;

namespace RimWorld
{
	// Token: 0x020035A8 RID: 13736
	[DefOf]
	public static class RaidStrategyDefOf
	{
		// Token: 0x06013396 RID: 78742 RVA: 0x005B5B81 File Offset: 0x005B3D81
		static RaidStrategyDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(RaidStrategyDefOf));
		}

		// Token: 0x0400CF08 RID: 53000
		public static RaidStrategyDef ImmediateAttack;

		// Token: 0x0400CF09 RID: 53001
		public static RaidStrategyDef ImmediateAttackFriendly;

		// Token: 0x0400CF0A RID: 53002
		[MayRequireAnomaly]
		public static RaidStrategyDef PsychicRitualSiege;

		// Token: 0x0400CF0B RID: 53003
		[MayRequireAnomaly]
		public static RaidStrategyDef ShamblerAssault;
	}
}
