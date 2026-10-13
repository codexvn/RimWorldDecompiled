using System;

namespace Verse.AI.Group
{
	// Token: 0x020010C8 RID: 4296
	public class Trigger_ChanceOnSignal : Trigger
	{
		// Token: 0x0600690D RID: 26893 RVA: 0x00212321 File Offset: 0x00210521
		public Trigger_ChanceOnSignal(TriggerSignalType signalType, float chance)
		{
			this.signalType = signalType;
			this.chance = chance;
		}

		// Token: 0x0600690E RID: 26894 RVA: 0x00212337 File Offset: 0x00210537
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == this.signalType && Rand.Value < this.chance;
		}

		// Token: 0x04004A62 RID: 19042
		private TriggerSignalType signalType;

		// Token: 0x04004A63 RID: 19043
		private float chance;
	}
}
