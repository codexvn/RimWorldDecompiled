using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002E0D RID: 11789
	public class CompProperties_ShipLandingBeacon : CompProperties
	{
		// Token: 0x06010989 RID: 67977 RVA: 0x004C840B File Offset: 0x004C660B
		public CompProperties_ShipLandingBeacon()
		{
			this.compClass = typeof(CompShipLandingBeacon);
		}

		// Token: 0x0400ACEF RID: 44271
		public FloatRange edgeLengthRange;

		// Token: 0x0400ACF0 RID: 44272
		public TerrainAffordanceDef landingAreaTerrainSupport;
	}
}
