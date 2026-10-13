using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001AD3 RID: 6867
	public class ResearchTabDef : Def
	{
		// Token: 0x04006929 RID: 26921
		[MustTranslate]
		public string generalTitle = "";

		// Token: 0x0400692A RID: 26922
		[MustTranslate]
		public string generalDescription = "";

		// Token: 0x0400692B RID: 26923
		public bool visibleByDefault = true;

		// Token: 0x0400692C RID: 26924
		public int minMonolithLevelVisible = -1;

		// Token: 0x0400692D RID: 26925
		public string tutorTag;
	}
}
