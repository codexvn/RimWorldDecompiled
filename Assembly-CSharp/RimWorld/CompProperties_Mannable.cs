using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001972 RID: 6514
	public class CompProperties_Mannable : CompProperties
	{
		// Token: 0x060093E3 RID: 37859 RVA: 0x002B7C13 File Offset: 0x002B5E13
		public CompProperties_Mannable()
		{
			this.compClass = typeof(CompMannable);
		}

		// Token: 0x04005F6C RID: 24428
		public WorkTags manWorkType;

		// Token: 0x04005F6D RID: 24429
		public List<PlanetLayerDef> planetLayerWhitelist;
	}
}
