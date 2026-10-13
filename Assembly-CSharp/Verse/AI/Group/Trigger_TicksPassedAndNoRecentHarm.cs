using System;

namespace Verse.AI.Group
{
	// Token: 0x020010EC RID: 4332
	public class Trigger_TicksPassedAndNoRecentHarm : Trigger_TicksPassed
	{
		// Token: 0x0600695F RID: 26975 RVA: 0x00213349 File Offset: 0x00211549
		public Trigger_TicksPassedAndNoRecentHarm(int tickLimit) : base(tickLimit)
		{
		}

		// Token: 0x06006960 RID: 26976 RVA: 0x00213352 File Offset: 0x00211552
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return base.ActivateOn(lord, signal) && Find.TickManager.TicksGame - lord.lastPawnHarmTick >= 300;
		}

		// Token: 0x04004A87 RID: 19079
		private const int MinTicksSinceDamage = 300;
	}
}
