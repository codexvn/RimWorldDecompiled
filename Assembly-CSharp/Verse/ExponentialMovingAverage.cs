using System;

namespace Verse
{
	// Token: 0x02000CC7 RID: 3271
	public class ExponentialMovingAverage
	{
		// Token: 0x0600538C RID: 21388 RVA: 0x001AECFE File Offset: 0x001ACEFE
		public ExponentialMovingAverage(float alpha)
		{
			this.alpha = alpha;
		}

		// Token: 0x0600538D RID: 21389 RVA: 0x001AED0D File Offset: 0x001ACF0D
		public void AddValue(float value)
		{
			if (!this.initialized)
			{
				this.average = value;
				this.initialized = true;
				return;
			}
			this.average = this.alpha * value + (1f - this.alpha) * this.average;
		}

		// Token: 0x0600538E RID: 21390 RVA: 0x001AED48 File Offset: 0x001ACF48
		public float GetAverage()
		{
			return this.average;
		}

		// Token: 0x0600538F RID: 21391 RVA: 0x001AED50 File Offset: 0x001ACF50
		public void Reset()
		{
			this.initialized = false;
			this.average = 0f;
		}

		// Token: 0x04003C48 RID: 15432
		private readonly float alpha;

		// Token: 0x04003C49 RID: 15433
		private float average;

		// Token: 0x04003C4A RID: 15434
		private bool initialized;
	}
}
