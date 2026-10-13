using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200194E RID: 6478
	public class CompProperties_AffectedByFacilities : CompProperties
	{
		// Token: 0x0600935C RID: 37724 RVA: 0x002B583E File Offset: 0x002B3A3E
		public CompProperties_AffectedByFacilities()
		{
			this.compClass = typeof(CompAffectedByFacilities);
		}

		// Token: 0x04005EC0 RID: 24256
		public List<ThingDef> linkableFacilities;
	}
}
