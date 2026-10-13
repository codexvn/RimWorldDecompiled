using System;

namespace RimWorld
{
	// Token: 0x0200361A RID: 13850
	[DefOf]
	public static class AnomalyPlaystyleDefOf
	{
		// Token: 0x06013405 RID: 78853 RVA: 0x005B62E0 File Offset: 0x005B44E0
		static AnomalyPlaystyleDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(AnomalyPlaystyleDefOf));
		}

		// Token: 0x0400D55F RID: 54623
		[MayRequireAnomaly]
		public static AnomalyPlaystyleDef Standard;

		// Token: 0x0400D560 RID: 54624
		[MayRequireAnomaly]
		public static AnomalyPlaystyleDef AmbientHorror;
	}
}
