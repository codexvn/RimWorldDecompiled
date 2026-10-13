using System;

namespace Verse.AI.Group
{
	// Token: 0x020010ED RID: 4333
	public class Trigger_TicksPassedWithoutHarm : Trigger_TicksPassed
	{
		// Token: 0x06006961 RID: 26977 RVA: 0x00213349 File Offset: 0x00211549
		public Trigger_TicksPassedWithoutHarm(int tickLimit) : base(tickLimit)
		{
		}

		// Token: 0x06006962 RID: 26978 RVA: 0x00213379 File Offset: 0x00211579
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			if (Trigger_PawnHarmed.SignalIsHarm(signal))
			{
				base.Data.ticksPassed = 0;
			}
			return base.ActivateOn(lord, signal);
		}
	}
}
