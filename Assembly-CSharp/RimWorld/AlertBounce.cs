using System;
using UnityEngine;

namespace RimWorld
{
	// Token: 0x02002FC1 RID: 12225
	internal class AlertBounce
	{
		// Token: 0x060113AD RID: 70573 RVA: 0x004EFE32 File Offset: 0x004EE032
		public void DoAlertStartEffect()
		{
			this.position = 300f;
			this.velocity = -200f;
			this.lastTime = Time.time;
			this.idle = false;
		}

		// Token: 0x060113AE RID: 70574 RVA: 0x004EFE5C File Offset: 0x004EE05C
		public float CalculateHorizontalOffset()
		{
			if (this.idle)
			{
				return this.position;
			}
			float num = Mathf.Min(Time.time - this.lastTime, 0.05f);
			this.lastTime = Time.time;
			this.velocity -= 1200f * num;
			this.position += this.velocity * num;
			if (this.position < 0f)
			{
				this.position = 0f;
				this.velocity = Mathf.Max(-this.velocity / 3f - 1f, 0f);
			}
			if (Mathf.Abs(this.velocity) < 0.0001f && this.position < 1f)
			{
				this.velocity = 0f;
				this.position = 0f;
				this.idle = true;
			}
			return this.position;
		}

		// Token: 0x0400B2D8 RID: 45784
		private float position;

		// Token: 0x0400B2D9 RID: 45785
		private float velocity;

		// Token: 0x0400B2DA RID: 45786
		private float lastTime = Time.time;

		// Token: 0x0400B2DB RID: 45787
		private bool idle;

		// Token: 0x0400B2DC RID: 45788
		private const float StartPosition = 300f;

		// Token: 0x0400B2DD RID: 45789
		private const float StartVelocity = -200f;

		// Token: 0x0400B2DE RID: 45790
		private const float Acceleration = 1200f;

		// Token: 0x0400B2DF RID: 45791
		private const float DampingRatio = 3f;

		// Token: 0x0400B2E0 RID: 45792
		private const float DampingConstant = 1f;

		// Token: 0x0400B2E1 RID: 45793
		private const float MaxDelta = 0.05f;
	}
}
