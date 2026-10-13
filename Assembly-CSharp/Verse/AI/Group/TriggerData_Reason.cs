using System;

namespace Verse.AI.Group
{
	// Token: 0x020010C4 RID: 4292
	public class TriggerData_Reason : TriggerData
	{
		// Token: 0x06006904 RID: 26884 RVA: 0x0021223B File Offset: 0x0021043B
		public override void ExposeData()
		{
			Scribe_Values.Look<string>(ref this.reason, "reason", null, false);
		}

		// Token: 0x04004A5D RID: 19037
		public string reason;
	}
}
