using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B82 RID: 7042
	public class QuestPart_DelayRandom : QuestPart_Delay
	{
		// Token: 0x06009D48 RID: 40264 RVA: 0x002D9927 File Offset: 0x002D7B27
		protected override void Enable(SignalArgs receivedArgs)
		{
			this.delayTicks = this.delayTicksRange.RandomInRange;
			base.Enable(receivedArgs);
		}

		// Token: 0x06009D49 RID: 40265 RVA: 0x002D9944 File Offset: 0x002D7B44
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<IntRange>(ref this.delayTicksRange, "delayTicksRange", default(IntRange), false);
		}

		// Token: 0x06009D4A RID: 40266 RVA: 0x002D9971 File Offset: 0x002D7B71
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.delayTicksRange = new IntRange(833, 2500);
		}

		// Token: 0x04006D0B RID: 27915
		public IntRange delayTicksRange;
	}
}
