using System;

namespace LudeonTK
{
	// Token: 0x020000A9 RID: 169
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
	public class TweakValue : Attribute
	{
		// Token: 0x060003AA RID: 938 RVA: 0x00015AE2 File Offset: 0x00013CE2
		public TweakValue(string category, float min = 0f, float max = 100f)
		{
			this.category = category;
			this.min = min;
			this.max = max;
		}

		// Token: 0x0400024C RID: 588
		public string category;

		// Token: 0x0400024D RID: 589
		public float min;

		// Token: 0x0400024E RID: 590
		public float max;
	}
}
