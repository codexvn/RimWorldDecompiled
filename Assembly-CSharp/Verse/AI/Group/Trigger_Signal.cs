using System;

namespace Verse.AI.Group
{
	// Token: 0x020010C9 RID: 4297
	public class Trigger_Signal : Trigger
	{
		// Token: 0x0600690F RID: 26895 RVA: 0x00212356 File Offset: 0x00210556
		public Trigger_Signal(string signal)
		{
			this.signal = signal;
		}

		// Token: 0x06006910 RID: 26896 RVA: 0x00212365 File Offset: 0x00210565
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == TriggerSignalType.Signal && signal.signal.tag == this.signal;
		}

		// Token: 0x04004A64 RID: 19044
		private string signal;
	}
}
