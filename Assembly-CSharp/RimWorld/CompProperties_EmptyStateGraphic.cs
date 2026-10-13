using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002C71 RID: 11377
	public class CompProperties_EmptyStateGraphic : CompProperties
	{
		// Token: 0x0600FF28 RID: 65320 RVA: 0x0049F47B File Offset: 0x0049D67B
		public CompProperties_EmptyStateGraphic()
		{
			this.compClass = typeof(CompEmptyStateGraphic);
		}

		// Token: 0x0400A70A RID: 42762
		public GraphicData graphicData;

		// Token: 0x0400A70B RID: 42763
		public bool alwaysDrawParent;
	}
}
