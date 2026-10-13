using System;

namespace RimWorld
{
	// Token: 0x020035A5 RID: 13733
	[DefOf]
	public static class DifficultyDefOf
	{
		// Token: 0x06013393 RID: 78739 RVA: 0x005B5B4E File Offset: 0x005B3D4E
		static DifficultyDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(DifficultyDefOf));
		}

		// Token: 0x0400CF02 RID: 52994
		public static DifficultyDef Easy;

		// Token: 0x0400CF03 RID: 52995
		public static DifficultyDef Rough;
	}
}
