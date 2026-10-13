using System;

namespace Verse
{
	// Token: 0x02000B63 RID: 2915
	public static class ModToolUtility
	{
		// Token: 0x06004AC6 RID: 19142 RVA: 0x0017C43B File Offset: 0x0017A63B
		public static bool IsValueEditable(this Type type)
		{
			return type.IsValueType || type == typeof(string);
		}
	}
}
