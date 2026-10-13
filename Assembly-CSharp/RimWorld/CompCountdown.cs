using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002C48 RID: 11336
	public class CompCountdown : ThingComp
	{
		// Token: 0x17002805 RID: 10245
		// (get) Token: 0x0600FE3A RID: 65082 RVA: 0x0049B97F File Offset: 0x00499B7F
		public CompProperties_Countdown Props
		{
			get
			{
				return (CompProperties_Countdown)this.props;
			}
		}

		// Token: 0x0600FE3B RID: 65083 RVA: 0x0049B98C File Offset: 0x00499B8C
		public override string CompInspectStringExtra()
		{
			int num = this.endTick - GenTicks.TicksGame;
			if (num <= 0)
			{
				return null;
			}
			return this.Props.label.Formatted(num.ToStringTicksToPeriod(true, false, true, true, false).Colorize(ColoredText.DateTimeColor).Named("DURATION")).Resolve();
		}

		// Token: 0x0600FE3C RID: 65084 RVA: 0x0049B9E3 File Offset: 0x00499BE3
		public override void PostExposeData()
		{
			Scribe_Values.Look<int>(ref this.endTick, "endTick", 0, false);
		}

		// Token: 0x0400A69F RID: 42655
		public int endTick;
	}
}
