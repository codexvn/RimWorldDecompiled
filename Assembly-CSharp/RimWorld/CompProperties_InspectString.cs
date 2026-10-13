using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002CF0 RID: 11504
	public class CompProperties_InspectString : CompProperties
	{
		// Token: 0x0601029C RID: 66204 RVA: 0x004AD37B File Offset: 0x004AB57B
		public CompProperties_InspectString()
		{
			this.compClass = typeof(CompInspectString);
		}

		// Token: 0x0400A8D3 RID: 43219
		[MustTranslate]
		public string inspectString;
	}
}
