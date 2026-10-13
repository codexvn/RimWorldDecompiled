using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002CDA RID: 11482
	public class CompProperties_HasSources : CompProperties
	{
		// Token: 0x06010214 RID: 66068 RVA: 0x004AABD3 File Offset: 0x004A8DD3
		public CompProperties_HasSources()
		{
			this.compClass = typeof(CompHasSources);
		}

		// Token: 0x0400A886 RID: 43142
		public bool affectLabel = true;

		// Token: 0x0400A887 RID: 43143
		[MustTranslate]
		public string inspectStringLabel;
	}
}
