using System;

namespace RimWorld
{
	// Token: 0x02003597 RID: 13719
	[DefOf]
	public static class MainButtonDefOf
	{
		// Token: 0x06013385 RID: 78725 RVA: 0x005B5A60 File Offset: 0x005B3C60
		static MainButtonDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(MainButtonDefOf));
		}

		// Token: 0x0400CE54 RID: 52820
		public static MainButtonDef Inspect;

		// Token: 0x0400CE55 RID: 52821
		public static MainButtonDef Architect;

		// Token: 0x0400CE56 RID: 52822
		public static MainButtonDef Research;

		// Token: 0x0400CE57 RID: 52823
		public static MainButtonDef Menu;

		// Token: 0x0400CE58 RID: 52824
		public static MainButtonDef World;

		// Token: 0x0400CE59 RID: 52825
		public static MainButtonDef Quests;

		// Token: 0x0400CE5A RID: 52826
		public static MainButtonDef Factions;

		// Token: 0x0400CE5B RID: 52827
		[MayRequireIdeology]
		public static MainButtonDef Ideos;
	}
}
