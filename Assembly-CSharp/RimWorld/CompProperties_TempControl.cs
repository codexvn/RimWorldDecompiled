using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001988 RID: 6536
	public class CompProperties_TempControl : CompProperties
	{
		// Token: 0x06009449 RID: 37961 RVA: 0x002B8FBC File Offset: 0x002B71BC
		public CompProperties_TempControl()
		{
			this.compClass = typeof(CompTempControl);
		}

		// Token: 0x04005FCE RID: 24526
		public float energyPerSecond = 12f;

		// Token: 0x04005FCF RID: 24527
		public float defaultTargetTemperature = 21f;

		// Token: 0x04005FD0 RID: 24528
		public float minTargetTemperature = -50f;

		// Token: 0x04005FD1 RID: 24529
		public float maxTargetTemperature = 50f;

		// Token: 0x04005FD2 RID: 24530
		public float lowPowerConsumptionFactor = 0.1f;

		// Token: 0x04005FD3 RID: 24531
		[MustTranslate]
		public string inspectString;
	}
}
