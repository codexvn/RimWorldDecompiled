using System;

namespace Verse
{
	// Token: 0x02000B58 RID: 2904
	[AttributeUsage(AttributeTargets.Field)]
	public class UnsavedAttribute : Attribute
	{
		// Token: 0x06004AB9 RID: 19129 RVA: 0x0017C332 File Offset: 0x0017A532
		public UnsavedAttribute(bool allowLoading = false)
		{
			this.allowLoading = allowLoading;
		}

		// Token: 0x0400350C RID: 13580
		public bool allowLoading;
	}
}
