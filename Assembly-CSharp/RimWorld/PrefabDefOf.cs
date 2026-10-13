using System;

namespace RimWorld
{
	// Token: 0x02003621 RID: 13857
	[DefOf]
	public static class PrefabDefOf
	{
		// Token: 0x0601340C RID: 78860 RVA: 0x005B6357 File Offset: 0x005B4557
		static PrefabDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(PrefabDefOf));
		}

		// Token: 0x0400D58E RID: 54670
		[MayRequireOdyssey]
		public static PrefabDef AncientUplink;

		// Token: 0x0400D58F RID: 54671
		[MayRequireOdyssey]
		public static PrefabDef SurveyScanner;

		// Token: 0x0400D590 RID: 54672
		[MayRequireOdyssey]
		public static PrefabDef CerebrexCore;

		// Token: 0x0400D591 RID: 54673
		[MayRequireOdyssey]
		public static PrefabDef CorridorBarricade;
	}
}
