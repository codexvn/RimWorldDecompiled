using System;

namespace Verse.AI.Group
{
	// Token: 0x020010CD RID: 4301
	public class Trigger_DormancyWakeup : Trigger
	{
		// Token: 0x06006918 RID: 26904 RVA: 0x0021246B File Offset: 0x0021066B
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == TriggerSignalType.DormancyWakeup;
		}
	}
}
