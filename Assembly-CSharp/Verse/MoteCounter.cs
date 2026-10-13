using System;

namespace Verse
{
	// Token: 0x020001AE RID: 430
	public class MoteCounter
	{
		// Token: 0x170001DC RID: 476
		// (get) Token: 0x06000C15 RID: 3093 RVA: 0x00039A8C File Offset: 0x00037C8C
		public int MoteCount
		{
			get
			{
				return this.moteCount;
			}
		}

		// Token: 0x170001DD RID: 477
		// (get) Token: 0x06000C16 RID: 3094 RVA: 0x00039A94 File Offset: 0x00037C94
		public float Saturation
		{
			get
			{
				return (float)this.moteCount / 250f;
			}
		}

		// Token: 0x170001DE RID: 478
		// (get) Token: 0x06000C17 RID: 3095 RVA: 0x00039AA3 File Offset: 0x00037CA3
		public bool Saturated
		{
			get
			{
				return this.Saturation > 1f;
			}
		}

		// Token: 0x170001DF RID: 479
		// (get) Token: 0x06000C18 RID: 3096 RVA: 0x00039AB2 File Offset: 0x00037CB2
		public bool SaturatedLowPriority
		{
			get
			{
				return this.Saturation > 0.8f;
			}
		}

		// Token: 0x06000C19 RID: 3097 RVA: 0x00039AC1 File Offset: 0x00037CC1
		public void Notify_MoteSpawned()
		{
			this.moteCount++;
		}

		// Token: 0x06000C1A RID: 3098 RVA: 0x00039AD1 File Offset: 0x00037CD1
		public void Notify_MoteDespawned()
		{
			this.moteCount--;
		}

		// Token: 0x040006FA RID: 1786
		private int moteCount;

		// Token: 0x040006FB RID: 1787
		private const int SaturatedCount = 250;
	}
}
