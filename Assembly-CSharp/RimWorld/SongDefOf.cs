using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020035B4 RID: 13748
	[DefOf]
	public static class SongDefOf
	{
		// Token: 0x060133A2 RID: 78754 RVA: 0x005B5C4D File Offset: 0x005B3E4D
		static SongDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(SongDefOf));
		}

		// Token: 0x0400D11C RID: 53532
		public static SongDef EntrySong;

		// Token: 0x0400D11D RID: 53533
		public static SongDef EndCreditsSong;

		// Token: 0x0400D11E RID: 53534
		[MayRequireIdeology]
		public static SongDef ArchonexusVictorySong;

		// Token: 0x0400D11F RID: 53535
		[MayRequireAnomaly]
		public static SongDef AnomalyCreditsSong;

		// Token: 0x0400D120 RID: 53536
		[MayRequireOdyssey]
		public static SongDef OdysseyCreditsSong;
	}
}
