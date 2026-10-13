using System;

namespace Verse
{
	// Token: 0x02000741 RID: 1857
	public class TickTimer : IExposable
	{
		// Token: 0x17000821 RID: 2081
		// (get) Token: 0x06002D59 RID: 11609 RVA: 0x000EA04B File Offset: 0x000E824B
		public bool Finished
		{
			get
			{
				return this.elapsed >= this.duration;
			}
		}

		// Token: 0x06002D5A RID: 11610 RVA: 0x000EA060 File Offset: 0x000E8260
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.elapsed, "elapsed", 0, false);
			Scribe_Values.Look<int>(ref this.duration, "duration", 0, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit && !this.Finished)
			{
				this.start = GenTicks.TicksGame;
			}
		}

		// Token: 0x06002D5B RID: 11611 RVA: 0x000EA0AC File Offset: 0x000E82AC
		public void Start(int startTicks, int durationTicks, Action finishAction)
		{
			this.start = startTicks;
			this.duration = durationTicks;
			this.OnFinish = finishAction;
			this.elapsed = 0;
		}

		// Token: 0x06002D5C RID: 11612 RVA: 0x000EA0CA File Offset: 0x000E82CA
		public void TickIntervalDelta()
		{
			this.elapsed = GenTicks.TicksGame - this.start;
			if (this.Finished && this.OnFinish != null)
			{
				this.OnFinish();
				this.OnFinish = null;
			}
		}

		// Token: 0x040023F9 RID: 9209
		private int start;

		// Token: 0x040023FA RID: 9210
		private int elapsed;

		// Token: 0x040023FB RID: 9211
		private int duration;

		// Token: 0x040023FC RID: 9212
		public Action OnFinish;
	}
}
