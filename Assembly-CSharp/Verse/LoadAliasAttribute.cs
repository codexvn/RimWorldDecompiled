using System;

namespace Verse
{
	// Token: 0x02000B5F RID: 2911
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
	public class LoadAliasAttribute : Attribute
	{
		// Token: 0x06004AC2 RID: 19138 RVA: 0x0017C40E File Offset: 0x0017A60E
		public LoadAliasAttribute(string alias)
		{
			this.alias = alias;
		}

		// Token: 0x0400350E RID: 13582
		public string alias;
	}
}
