using System;

namespace Verse
{
	// Token: 0x02000B59 RID: 2905
	[AttributeUsage(AttributeTargets.Field)]
	public class DefaultValueAttribute : Attribute
	{
		// Token: 0x06004ABA RID: 19130 RVA: 0x0017C341 File Offset: 0x0017A541
		public DefaultValueAttribute(object value)
		{
			this.value = value;
		}

		// Token: 0x06004ABB RID: 19131 RVA: 0x0017C350 File Offset: 0x0017A550
		public virtual bool ObjIsDefault(object obj)
		{
			if (obj == null)
			{
				return this.value == null;
			}
			return this.value != null && this.value.Equals(obj);
		}

		// Token: 0x0400350D RID: 13581
		public object value;
	}
}
