using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020035E5 RID: 13797
	[DefOf]
	public static class LogEntryDefOf
	{
		// Token: 0x060133D3 RID: 78803 RVA: 0x005B5F8E File Offset: 0x005B418E
		static LogEntryDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(LogEntryDefOf));
		}

		// Token: 0x0400D3A8 RID: 54184
		public static LogEntryDef MeleeAttack;
	}
}
