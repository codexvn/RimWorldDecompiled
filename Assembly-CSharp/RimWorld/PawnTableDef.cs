using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001AAF RID: 6831
	public class PawnTableDef : Def
	{
		// Token: 0x040067E2 RID: 26594
		public List<PawnColumnDef> columns;

		// Token: 0x040067E3 RID: 26595
		public Type workerClass = typeof(PawnTable);

		// Token: 0x040067E4 RID: 26596
		public int minWidth = 998;
	}
}
