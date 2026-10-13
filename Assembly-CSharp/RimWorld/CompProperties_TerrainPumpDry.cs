using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002E89 RID: 11913
	public class CompProperties_TerrainPumpDry : CompProperties_TerrainPump
	{
		// Token: 0x06010C69 RID: 68713 RVA: 0x004D500B File Offset: 0x004D320B
		public CompProperties_TerrainPumpDry()
		{
			this.compClass = typeof(CompTerrainPumpDry);
		}

		// Token: 0x0400AEB3 RID: 44723
		public SoundDef soundWorking;
	}
}
