using System;

namespace RimWorld
{
	// Token: 0x02003610 RID: 13840
	[DefOf]
	public static class PsychicRitualRoleDefOf
	{
		// Token: 0x060133FB RID: 78843 RVA: 0x005B6236 File Offset: 0x005B4436
		static PsychicRitualRoleDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(PsychicRitualRoleDefOf));
		}

		// Token: 0x0400D529 RID: 54569
		[MayRequireAnomaly]
		public static PsychicRitualRoleDef Invoker;

		// Token: 0x0400D52A RID: 54570
		[MayRequireAnomaly]
		public static PsychicRitualRoleDef Chanter;

		// Token: 0x0400D52B RID: 54571
		[MayRequireAnomaly]
		public static PsychicRitualRoleDef ChanterAdvanced;

		// Token: 0x0400D52C RID: 54572
		[MayRequireAnomaly]
		public static PsychicRitualRoleDef Defender;
	}
}
