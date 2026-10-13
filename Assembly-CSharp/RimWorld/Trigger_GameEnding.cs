using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001762 RID: 5986
	public class Trigger_GameEnding : Trigger
	{
		// Token: 0x06008C7F RID: 35967 RVA: 0x0029C87E File Offset: 0x0029AA7E
		public Trigger_GameEnding(int triggerAfterTicks = 2500)
		{
			this.triggerAfterTicks = triggerAfterTicks;
		}

		// Token: 0x06008C80 RID: 35968 RVA: 0x0029C89C File Offset: 0x0029AA9C
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			if (signal.type == TriggerSignalType.Tick)
			{
				if (this.requireActivePawns && !lord.AnyActivePawn)
				{
					return false;
				}
				if (Find.GameEnder.gameEnding)
				{
					if (this.gameEndingSinceTick == -1)
					{
						this.gameEndingSinceTick = Find.TickManager.TicksGame;
					}
				}
				else
				{
					this.gameEndingSinceTick = -1;
				}
				if (this.gameEndingSinceTick > 0 && Find.TickManager.TicksGame - this.gameEndingSinceTick > this.triggerAfterTicks)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04005B4F RID: 23375
		private int triggerAfterTicks;

		// Token: 0x04005B50 RID: 23376
		private bool requireActivePawns = true;

		// Token: 0x04005B51 RID: 23377
		private int gameEndingSinceTick = -1;
	}
}
