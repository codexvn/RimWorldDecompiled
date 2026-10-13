using System;

namespace Verse.AI.Group
{
	// Token: 0x020010C7 RID: 4295
	public class Trigger_PawnsLost : Trigger
	{
		// Token: 0x0600690B RID: 26891 RVA: 0x002122ED File Offset: 0x002104ED
		public Trigger_PawnsLost(int count)
		{
			this.count = count;
		}

		// Token: 0x0600690C RID: 26892 RVA: 0x00212303 File Offset: 0x00210503
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == TriggerSignalType.PawnLost && lord.numPawnsLostViolently >= this.count;
		}

		// Token: 0x04004A61 RID: 19041
		private int count = 1;
	}
}
