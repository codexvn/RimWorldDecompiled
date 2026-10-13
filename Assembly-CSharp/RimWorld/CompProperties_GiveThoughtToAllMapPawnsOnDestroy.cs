using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002CB8 RID: 11448
	public class CompProperties_GiveThoughtToAllMapPawnsOnDestroy : CompProperties
	{
		// Token: 0x06010118 RID: 65816 RVA: 0x004A658D File Offset: 0x004A478D
		public CompProperties_GiveThoughtToAllMapPawnsOnDestroy()
		{
			this.compClass = typeof(CompGiveThoughtToAllMapPawnsOnDestroy);
		}

		// Token: 0x0400A7E6 RID: 42982
		public ThoughtDef thought;

		// Token: 0x0400A7E7 RID: 42983
		[MustTranslate]
		public string message;

		// Token: 0x0400A7E8 RID: 42984
		public bool onlyWhenKilled;

		// Token: 0x0400A7E9 RID: 42985
		public bool ignoreOnVanish = true;
	}
}
