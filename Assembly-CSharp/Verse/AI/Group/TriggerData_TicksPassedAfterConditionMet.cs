using System;

namespace Verse.AI.Group
{
	// Token: 0x020010F0 RID: 4336
	public class TriggerData_TicksPassedAfterConditionMet : TriggerData_TicksPassed
	{
		// Token: 0x06006968 RID: 26984 RVA: 0x0021346F File Offset: 0x0021166F
		public override void ExposeData()
		{
			Scribe_Values.Look<bool>(ref this.conditionMet, "conditionMet", false, false);
		}

		// Token: 0x04004A8B RID: 19083
		public bool conditionMet;
	}
}
