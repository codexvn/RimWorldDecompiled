using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000B5B RID: 2907
	[AttributeUsage(AttributeTargets.Field)]
	public class DefaultEmptyListAttribute : DefaultValueAttribute
	{
		// Token: 0x06004ABD RID: 19133 RVA: 0x0017C389 File Offset: 0x0017A589
		public DefaultEmptyListAttribute(Type type) : base(type)
		{
		}

		// Token: 0x06004ABE RID: 19134 RVA: 0x0017C394 File Offset: 0x0017A594
		public override bool ObjIsDefault(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (obj.GetType().GetGenericTypeDefinition() != typeof(List<>))
			{
				return false;
			}
			Type[] genericArguments = obj.GetType().GetGenericArguments();
			return genericArguments.Length == 1 && !(genericArguments[0] != (Type)this.value) && (int)obj.GetType().GetProperty("Count").GetValue(obj, null) == 0;
		}
	}
}
