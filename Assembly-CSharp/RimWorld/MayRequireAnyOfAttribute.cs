using System;

namespace RimWorld
{
	// Token: 0x02003581 RID: 13697
	[AttributeUsage(AttributeTargets.Field)]
	public class MayRequireAnyOfAttribute : Attribute
	{
		// Token: 0x0601336D RID: 78701 RVA: 0x005B5660 File Offset: 0x005B3860
		public MayRequireAnyOfAttribute(string modId)
		{
			this.modIds = modId.Split(new char[]
			{
				','
			}, StringSplitOptions.RemoveEmptyEntries);
			for (int i = 0; i < this.modIds.Length; i++)
			{
				this.modIds[i] = this.modIds[i].Trim();
			}
		}

		// Token: 0x0400C740 RID: 51008
		public string[] modIds;
	}
}
