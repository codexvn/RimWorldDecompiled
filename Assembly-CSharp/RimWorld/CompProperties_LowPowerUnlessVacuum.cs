using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002132 RID: 8498
	public class CompProperties_LowPowerUnlessVacuum : CompProperties
	{
		// Token: 0x0600B96C RID: 47468 RVA: 0x00358178 File Offset: 0x00356378
		public CompProperties_LowPowerUnlessVacuum()
		{
			this.compClass = typeof(CompLowPowerInSpace);
		}

		// Token: 0x04007FDD RID: 32733
		public float lowPowerConsumptionFactor = 0.1f;

		// Token: 0x04007FDE RID: 32734
		public bool checkRoomVacuum = true;
	}
}
