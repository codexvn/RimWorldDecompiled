using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002E13 RID: 11795
	public class CompProperties_Shuttle : CompProperties
	{
		// Token: 0x060109A9 RID: 68009 RVA: 0x004C93B4 File Offset: 0x004C75B4
		public CompProperties_Shuttle()
		{
			this.compClass = typeof(CompShuttle);
		}

		// Token: 0x0400ACFD RID: 44285
		public TransportShipDef shipDef;
	}
}
