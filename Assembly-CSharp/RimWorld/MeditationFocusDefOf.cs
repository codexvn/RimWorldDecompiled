using System;

namespace RimWorld
{
	// Token: 0x020035EE RID: 13806
	[DefOf]
	public static class MeditationFocusDefOf
	{
		// Token: 0x060133DC RID: 78812 RVA: 0x005B6027 File Offset: 0x005B4227
		static MeditationFocusDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(MeditationFocusDefOf));
		}

		// Token: 0x0400D3E3 RID: 54243
		[MayRequireRoyalty]
		public static MeditationFocusDef Natural;
	}
}
