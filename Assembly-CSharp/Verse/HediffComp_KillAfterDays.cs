using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020006D7 RID: 1751
	public class HediffComp_KillAfterDays : HediffComp
	{
		// Token: 0x170007A1 RID: 1953
		// (get) Token: 0x06002B71 RID: 11121 RVA: 0x000E323D File Offset: 0x000E143D
		public HediffCompProperties_KillAfterDays Props
		{
			get
			{
				return (HediffCompProperties_KillAfterDays)this.props;
			}
		}

		// Token: 0x170007A2 RID: 1954
		// (get) Token: 0x06002B72 RID: 11122 RVA: 0x000E324C File Offset: 0x000E144C
		public override string CompTipStringExtra
		{
			get
			{
				if (this.ticksLeft <= 0)
				{
					return null;
				}
				return "DeathIn".Translate(this.ticksLeft.ToStringTicksToPeriod(true, false, true, true, false).Colorize(ColoredText.DateTimeColor)).Resolve().CapitalizeFirst();
			}
		}

		// Token: 0x06002B73 RID: 11123 RVA: 0x000E329A File Offset: 0x000E149A
		public override void CompPostPostAdd(DamageInfo? dinfo)
		{
			this.ticksLeft = 60000 * this.Props.days;
		}

		// Token: 0x06002B74 RID: 11124 RVA: 0x000E32B4 File Offset: 0x000E14B4
		public override void CompPostTickInterval(ref float severityAdjustment, int delta)
		{
			this.ticksLeft -= delta;
			if (this.ticksLeft <= 0)
			{
				base.Pawn.Kill(null, this.parent);
			}
		}

		// Token: 0x06002B75 RID: 11125 RVA: 0x000E32F2 File Offset: 0x000E14F2
		public override void CompExposeData()
		{
			Scribe_Values.Look<int>(ref this.ticksLeft, "ticksLeft", 0, false);
		}

		// Token: 0x040022FF RID: 8959
		private int ticksLeft;
	}
}
