using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002C3A RID: 11322
	public class CompProperties_CascadeOnDestroyed : CompProperties
	{
		// Token: 0x0600FDE3 RID: 64995 RVA: 0x0049A377 File Offset: 0x00498577
		public CompProperties_CascadeOnDestroyed()
		{
			this.compClass = typeof(CompCascadeOnDestroyed);
		}

		// Token: 0x0400A668 RID: 42600
		public IntRange cascadeCountRange = new IntRange(4, 8);

		// Token: 0x0400A669 RID: 42601
		public List<ThingDef> cascadeThingDefs;
	}
}
