using System;

namespace LudeonTK
{
	// Token: 0x0200007D RID: 125
	[AttributeUsage(AttributeTargets.Method)]
	public class DebugOutputAttribute : Attribute
	{
		// Token: 0x060002AE RID: 686 RVA: 0x0000E9CF File Offset: 0x0000CBCF
		public DebugOutputAttribute()
		{
		}

		// Token: 0x060002AF RID: 687 RVA: 0x0000E9E2 File Offset: 0x0000CBE2
		public DebugOutputAttribute(bool onlyWhenPlaying)
		{
			this.onlyWhenPlaying = onlyWhenPlaying;
		}

		// Token: 0x060002B0 RID: 688 RVA: 0x0000E9FC File Offset: 0x0000CBFC
		public DebugOutputAttribute(string category, bool onlyWhenPlaying = false) : this(onlyWhenPlaying)
		{
			this.category = category;
		}

		// Token: 0x04000181 RID: 385
		public string name;

		// Token: 0x04000182 RID: 386
		public string category = "General";

		// Token: 0x04000183 RID: 387
		public bool onlyWhenPlaying;
	}
}
