using System;

namespace Verse
{
	// Token: 0x02000746 RID: 1862
	public class Hediff_FrenzyField : Hediff
	{
		// Token: 0x17000829 RID: 2089
		// (get) Token: 0x06002D71 RID: 11633 RVA: 0x000EA48A File Offset: 0x000E868A
		public bool InRangeOfInducer
		{
			get
			{
				return GenTicks.TicksGame <= this.lastTickInRangeOfInducer + 60;
			}
		}

		// Token: 0x06002D72 RID: 11634 RVA: 0x000EA49F File Offset: 0x000E869F
		public override void PostAdd(DamageInfo? dinfo)
		{
			if (!ModLister.CheckAnomaly("Frenzy field"))
			{
				this.pawn.health.RemoveHediff(this);
				return;
			}
			base.PostAdd(dinfo);
		}

		// Token: 0x06002D73 RID: 11635 RVA: 0x000EA4C6 File Offset: 0x000E86C6
		public override void TickInterval(int delta)
		{
			base.TickInterval(delta);
			if (this.InRangeOfInducer)
			{
				this.Severity += 0.000132f * (float)delta;
				return;
			}
			this.Severity -= 2.2400001E-05f * (float)delta;
		}

		// Token: 0x1700082A RID: 2090
		// (get) Token: 0x06002D74 RID: 11636 RVA: 0x000EA502 File Offset: 0x000E8702
		public override bool ShouldRemove
		{
			get
			{
				return !this.InRangeOfInducer && base.ShouldRemove;
			}
		}

		// Token: 0x06002D75 RID: 11637 RVA: 0x000EA514 File Offset: 0x000E8714
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.lastTickInRangeOfInducer, "lastTickInRangeOfInducer", 0, false);
		}

		// Token: 0x04002404 RID: 9220
		public int lastTickInRangeOfInducer;

		// Token: 0x04002405 RID: 9221
		private const float SeverityRaisePerTick = 0.000132f;

		// Token: 0x04002406 RID: 9222
		private const float SeverityFallPerTick = 2.2400001E-05f;

		// Token: 0x04002407 RID: 9223
		private const int InducerBufferTicks = 60;
	}
}
