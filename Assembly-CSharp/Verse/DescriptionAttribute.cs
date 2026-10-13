using System;

namespace Verse
{
	// Token: 0x02000B60 RID: 2912
	[AttributeUsage(AttributeTargets.Field)]
	public class DescriptionAttribute : Attribute
	{
		// Token: 0x06004AC3 RID: 19139 RVA: 0x0017C41D File Offset: 0x0017A61D
		public DescriptionAttribute(string description)
		{
			this.description = description;
		}

		// Token: 0x0400350F RID: 13583
		public string description;
	}
}
