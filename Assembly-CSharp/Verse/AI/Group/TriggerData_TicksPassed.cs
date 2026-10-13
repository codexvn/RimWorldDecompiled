using System;

namespace Verse.AI.Group
{
	// Token: 0x020010E9 RID: 4329
	public class TriggerData_TicksPassed : TriggerData
	{
		// Token: 0x06006957 RID: 26967 RVA: 0x00213274 File Offset: 0x00211474
		public override void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.ticksPassed, "ticksPassed", 0, false);
		}

		// Token: 0x04004A83 RID: 19075
		public int ticksPassed;
	}
}
