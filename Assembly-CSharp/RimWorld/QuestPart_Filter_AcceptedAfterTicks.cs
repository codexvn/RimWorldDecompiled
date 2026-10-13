using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B96 RID: 7062
	public class QuestPart_Filter_AcceptedAfterTicks : QuestPart_Filter
	{
		// Token: 0x06009D9A RID: 40346 RVA: 0x002DAC4D File Offset: 0x002D8E4D
		protected override bool Pass(SignalArgs args)
		{
			return this.quest.TicksSinceAccepted > this.timeTicks;
		}

		// Token: 0x06009D9B RID: 40347 RVA: 0x002DAC62 File Offset: 0x002D8E62
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.timeTicks, "timeTicks", 0, false);
		}

		// Token: 0x04006D36 RID: 27958
		public int timeTicks;
	}
}
