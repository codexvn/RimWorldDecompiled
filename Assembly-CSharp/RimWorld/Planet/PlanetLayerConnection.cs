using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003CDB RID: 15579
	public class PlanetLayerConnection : IExposable
	{
		// Token: 0x060152E9 RID: 86761 RVA: 0x00656F91 File Offset: 0x00655191
		public void ExposeData()
		{
			Scribe_Values.Look<float>(ref this.fuelCost, "fuelCost", 0f, false);
			Scribe_References.Look<PlanetLayer>(ref this.origin, "origin", false);
			Scribe_References.Look<PlanetLayer>(ref this.target, "target", false);
		}

		// Token: 0x0400EC52 RID: 60498
		public PlanetLayer origin;

		// Token: 0x0400EC53 RID: 60499
		public PlanetLayer target;

		// Token: 0x0400EC54 RID: 60500
		public float fuelCost;
	}
}
