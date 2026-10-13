using System;

namespace RimWorld
{
	// Token: 0x0200357B RID: 13691
	[AttributeUsage(AttributeTargets.Field)]
	public class MayRequireAttribute : Attribute
	{
		// Token: 0x06013367 RID: 78695 RVA: 0x005B5610 File Offset: 0x005B3810
		public MayRequireAttribute(string modId)
		{
			this.modId = modId;
		}

		// Token: 0x0400C73F RID: 51007
		public string modId;
	}
}
