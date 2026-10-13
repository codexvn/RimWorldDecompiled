using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002CCC RID: 11468
	public class CompProperties_GrowsFleshmassLump : CompProperties
	{
		// Token: 0x060101A0 RID: 65952 RVA: 0x004A864F File Offset: 0x004A684F
		public CompProperties_GrowsFleshmassLump()
		{
			this.compClass = typeof(CompGrowsFleshmassLump);
		}

		// Token: 0x0400A838 RID: 43064
		public IntRange maxFleshmassRange;

		// Token: 0x0400A839 RID: 43065
		public IntRange ticksBetweenFleshmassRange;
	}
}
