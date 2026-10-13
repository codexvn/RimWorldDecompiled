using System;

namespace Verse.AI.Group
{
	// Token: 0x020010CE RID: 4302
	public class Trigger_DormancyWakeupOrClamor : Trigger
	{
		// Token: 0x0600691A RID: 26906 RVA: 0x00212477 File Offset: 0x00210677
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == TriggerSignalType.DormancyWakeup || signal.type == TriggerSignalType.Clamor;
		}
	}
}
