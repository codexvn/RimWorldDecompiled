using System;

namespace Verse
{
	// Token: 0x020001E6 RID: 486
	internal class ExtendedShakeRequest : IExposable
	{
		// Token: 0x17000207 RID: 519
		// (get) Token: 0x06000D14 RID: 3348 RVA: 0x0003E277 File Offset: 0x0003C477
		public float Mag
		{
			get
			{
				return this.mag;
			}
		}

		// Token: 0x17000208 RID: 520
		// (get) Token: 0x06000D15 RID: 3349 RVA: 0x0003E27F File Offset: 0x0003C47F
		public int Duration
		{
			get
			{
				return this.duration;
			}
		}

		// Token: 0x17000209 RID: 521
		// (get) Token: 0x06000D16 RID: 3350 RVA: 0x0003E287 File Offset: 0x0003C487
		public int StartTick
		{
			get
			{
				return this.startTick;
			}
		}

		// Token: 0x06000D17 RID: 3351 RVA: 0x000024DF File Offset: 0x000006DF
		public ExtendedShakeRequest()
		{
		}

		// Token: 0x06000D18 RID: 3352 RVA: 0x0003E28F File Offset: 0x0003C48F
		public ExtendedShakeRequest(float mag, int duration)
		{
			this.mag = mag;
			this.duration = duration;
			this.startTick = Find.TickManager.TicksGame;
		}

		// Token: 0x06000D19 RID: 3353 RVA: 0x0003E2B5 File Offset: 0x0003C4B5
		public void ExposeData()
		{
			Scribe_Values.Look<float>(ref this.mag, "mag", 0f, false);
			Scribe_Values.Look<int>(ref this.duration, "duration", 0, false);
			Scribe_Values.Look<int>(ref this.startTick, "startTick", 0, false);
		}

		// Token: 0x04000797 RID: 1943
		private float mag;

		// Token: 0x04000798 RID: 1944
		private int duration;

		// Token: 0x04000799 RID: 1945
		private int startTick;
	}
}
