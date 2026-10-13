using System;

namespace RimWorld
{
	// Token: 0x02001953 RID: 6483
	public class CompProperties_Battery : CompProperties_Power
	{
		// Token: 0x0600936B RID: 37739 RVA: 0x002B5B08 File Offset: 0x002B3D08
		public CompProperties_Battery()
		{
			this.compClass = typeof(CompPowerBattery);
		}

		// Token: 0x04005ED7 RID: 24279
		public float storedEnergyMax = 1000f;

		// Token: 0x04005ED8 RID: 24280
		public float efficiency = 0.5f;
	}
}
