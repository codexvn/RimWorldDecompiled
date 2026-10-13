using System;

namespace Verse
{
	// Token: 0x0200076E RID: 1902
	public class Hediff_SleepSuppression : Hediff
	{
		// Token: 0x17000879 RID: 2169
		// (get) Token: 0x06002EAF RID: 11951 RVA: 0x000F03A5 File Offset: 0x000EE5A5
		public bool InRangeOfSuppressor
		{
			get
			{
				return GenTicks.TicksGame <= this.lastTickInRangeOfSuppressor + 60;
			}
		}

		// Token: 0x06002EB0 RID: 11952 RVA: 0x000F03BA File Offset: 0x000EE5BA
		public override void PostAdd(DamageInfo? dinfo)
		{
			if (!ModLister.CheckAnomaly("Sleep suppression"))
			{
				this.pawn.health.RemoveHediff(this);
				return;
			}
			base.PostAdd(dinfo);
		}

		// Token: 0x06002EB1 RID: 11953 RVA: 0x000F03E1 File Offset: 0x000EE5E1
		public override void TickInterval(int delta)
		{
			base.TickInterval(delta);
			if (!this.InRangeOfSuppressor)
			{
				this.Severity -= 0.000132f * (float)delta;
				return;
			}
			this.Severity += 6.4E-05f * (float)delta;
		}

		// Token: 0x1700087A RID: 2170
		// (get) Token: 0x06002EB2 RID: 11954 RVA: 0x000F041D File Offset: 0x000EE61D
		public override bool ShouldRemove
		{
			get
			{
				return !this.InRangeOfSuppressor && base.ShouldRemove;
			}
		}

		// Token: 0x06002EB3 RID: 11955 RVA: 0x000F042F File Offset: 0x000EE62F
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.lastTickInRangeOfSuppressor, "lastTickInRangeOfSuppressor", 0, false);
		}

		// Token: 0x04002484 RID: 9348
		public int lastTickInRangeOfSuppressor;

		// Token: 0x04002485 RID: 9349
		private const float FallRatePerHour = 0.33f;

		// Token: 0x04002486 RID: 9350
		private const float GainRatePerHour = 0.16f;

		// Token: 0x04002487 RID: 9351
		private const int SuppressorBufferTicks = 60;
	}
}
