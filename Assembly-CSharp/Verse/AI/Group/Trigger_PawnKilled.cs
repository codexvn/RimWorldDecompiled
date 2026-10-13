using System;
using System.Collections.Generic;

namespace Verse.AI.Group
{
	// Token: 0x020010D7 RID: 4311
	public class Trigger_PawnKilled : Trigger
	{
		// Token: 0x0600692F RID: 26927 RVA: 0x00212463 File Offset: 0x00210663
		public Trigger_PawnKilled()
		{
		}

		// Token: 0x06006930 RID: 26928 RVA: 0x002129D4 File Offset: 0x00210BD4
		public Trigger_PawnKilled(List<Pawn> exceptions)
		{
			this.exceptions = exceptions;
		}

		// Token: 0x06006931 RID: 26929 RVA: 0x002129E4 File Offset: 0x00210BE4
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == TriggerSignalType.PawnLost && ((signal.condition == PawnLostCondition.Killed || signal.condition == PawnLostCondition.Incapped) && signal.Pawn.Dead) && (this.exceptions.NullOrEmpty<Pawn>() || !this.exceptions.Contains(signal.Pawn));
		}

		// Token: 0x04004A75 RID: 19061
		private List<Pawn> exceptions;
	}
}
