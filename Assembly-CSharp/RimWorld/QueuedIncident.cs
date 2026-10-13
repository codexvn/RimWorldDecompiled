using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D5C RID: 7516
	public class QueuedIncident : IExposable
	{
		// Token: 0x17001A0A RID: 6666
		// (get) Token: 0x0600A876 RID: 43126 RVA: 0x00303862 File Offset: 0x00301A62
		public int FireTick
		{
			get
			{
				return this.fireTick;
			}
		}

		// Token: 0x17001A0B RID: 6667
		// (get) Token: 0x0600A877 RID: 43127 RVA: 0x0030386A File Offset: 0x00301A6A
		public FiringIncident FiringIncident
		{
			get
			{
				return this.firingInc;
			}
		}

		// Token: 0x17001A0C RID: 6668
		// (get) Token: 0x0600A878 RID: 43128 RVA: 0x00303872 File Offset: 0x00301A72
		public int RetryDurationTicks
		{
			get
			{
				return this.retryDurationTicks;
			}
		}

		// Token: 0x17001A0D RID: 6669
		// (get) Token: 0x0600A879 RID: 43129 RVA: 0x0030387A File Offset: 0x00301A7A
		public bool TriedToFire
		{
			get
			{
				return this.triedToFire;
			}
		}

		// Token: 0x0600A87A RID: 43130 RVA: 0x00303882 File Offset: 0x00301A82
		public QueuedIncident()
		{
		}

		// Token: 0x0600A87B RID: 43131 RVA: 0x00303891 File Offset: 0x00301A91
		public QueuedIncident(FiringIncident firingInc, int fireTick, int retryDurationTicks = 0)
		{
			this.firingInc = firingInc;
			this.fireTick = fireTick;
			this.retryDurationTicks = retryDurationTicks;
		}

		// Token: 0x0600A87C RID: 43132 RVA: 0x003038B8 File Offset: 0x00301AB8
		public void ExposeData()
		{
			Scribe_Deep.Look<FiringIncident>(ref this.firingInc, "firingInc", Array.Empty<object>());
			Scribe_Values.Look<int>(ref this.fireTick, "fireTick", 0, false);
			Scribe_Values.Look<int>(ref this.retryDurationTicks, "retryDurationTicks", 0, false);
			Scribe_Values.Look<bool>(ref this.triedToFire, "triedToFire", false, false);
		}

		// Token: 0x0600A87D RID: 43133 RVA: 0x00303910 File Offset: 0x00301B10
		public void Notify_TriedToFire()
		{
			this.triedToFire = true;
		}

		// Token: 0x0600A87E RID: 43134 RVA: 0x00303919 File Offset: 0x00301B19
		public override string ToString()
		{
			return this.fireTick.ToString() + "->" + this.firingInc.ToString();
		}

		// Token: 0x040074AD RID: 29869
		private FiringIncident firingInc;

		// Token: 0x040074AE RID: 29870
		private int fireTick = -1;

		// Token: 0x040074AF RID: 29871
		private int retryDurationTicks;

		// Token: 0x040074B0 RID: 29872
		private bool triedToFire;

		// Token: 0x040074B1 RID: 29873
		public const int RetryIntervalTicks = 833;
	}
}
