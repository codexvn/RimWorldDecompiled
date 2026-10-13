using System;

namespace Verse.AI.Group
{
	// Token: 0x020010C6 RID: 4294
	public class Trigger_FractionPawnsLost : Trigger
	{
		// Token: 0x06006909 RID: 26889 RVA: 0x002122AC File Offset: 0x002104AC
		public Trigger_FractionPawnsLost(float fraction)
		{
			this.fraction = fraction;
		}

		// Token: 0x0600690A RID: 26890 RVA: 0x002122C6 File Offset: 0x002104C6
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == TriggerSignalType.PawnLost && (float)lord.numPawnsLostViolently >= (float)lord.numPawnsEverGained * this.fraction;
		}

		// Token: 0x04004A60 RID: 19040
		private float fraction = 0.5f;
	}
}
