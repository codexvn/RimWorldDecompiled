using System;

namespace Verse.AI.Group
{
	// Token: 0x020010CB RID: 4299
	public class Trigger_ChanceOnTickInterval : Trigger
	{
		// Token: 0x06006913 RID: 26899 RVA: 0x002123B4 File Offset: 0x002105B4
		public Trigger_ChanceOnTickInterval(int interval, float chancePerInterval)
		{
			this.chancePerInterval = chancePerInterval;
			this.interval = interval;
		}

		// Token: 0x06006914 RID: 26900 RVA: 0x002123CA File Offset: 0x002105CA
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == TriggerSignalType.Tick && Find.TickManager.TicksGame % this.interval == 0 && Rand.Value < this.chancePerInterval;
		}

		// Token: 0x04004A66 RID: 19046
		private float chancePerInterval;

		// Token: 0x04004A67 RID: 19047
		private int interval;
	}
}
