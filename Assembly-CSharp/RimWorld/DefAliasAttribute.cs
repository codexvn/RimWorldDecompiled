using System;

namespace RimWorld
{
	// Token: 0x0200357A RID: 13690
	[AttributeUsage(AttributeTargets.Field)]
	public class DefAliasAttribute : Attribute
	{
		// Token: 0x06013366 RID: 78694 RVA: 0x005B5601 File Offset: 0x005B3801
		public DefAliasAttribute(string defName)
		{
			this.defName = defName;
		}

		// Token: 0x0400C73E RID: 51006
		public string defName;
	}
}
