using System;

namespace Verse.AI.Group
{
	// Token: 0x020010D5 RID: 4309
	public class Trigger_PawnLost : Trigger
	{
		// Token: 0x0600692A RID: 26922 RVA: 0x002128DC File Offset: 0x00210ADC
		public Trigger_PawnLost(PawnLostCondition condition = PawnLostCondition.Undefined, Pawn pawn = null)
		{
			this.condition = condition;
			this.pawn = pawn;
		}

		// Token: 0x0600692B RID: 26923 RVA: 0x002128F2 File Offset: 0x00210AF2
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == TriggerSignalType.PawnLost && (this.condition == PawnLostCondition.Undefined || signal.condition == this.condition) && (this.pawn == null || this.pawn == signal.Pawn);
		}

		// Token: 0x04004A71 RID: 19057
		private Pawn pawn;

		// Token: 0x04004A72 RID: 19058
		private PawnLostCondition condition;
	}
}
