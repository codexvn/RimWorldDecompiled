using System;

namespace Verse.AI.Group
{
	// Token: 0x020010DD RID: 4317
	public class Trigger_Memo : Trigger
	{
		// Token: 0x0600693C RID: 26940 RVA: 0x00212BC5 File Offset: 0x00210DC5
		public Trigger_Memo(string memo)
		{
			this.memo = memo;
		}

		// Token: 0x0600693D RID: 26941 RVA: 0x00212BD4 File Offset: 0x00210DD4
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == TriggerSignalType.Memo && signal.memo == this.memo;
		}

		// Token: 0x04004A76 RID: 19062
		private string memo;
	}
}
