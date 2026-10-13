using System;

namespace Verse.AI.Group
{
	// Token: 0x020010EB RID: 4331
	public class TriggerData_TicksPassedRitual : TriggerData
	{
		// Token: 0x0600695D RID: 26973 RVA: 0x00213331 File Offset: 0x00211531
		public override void ExposeData()
		{
			Scribe_Values.Look<float>(ref this.ticksPassed, "ticksPassed", 0f, false);
		}

		// Token: 0x04004A86 RID: 19078
		public float ticksPassed;
	}
}
