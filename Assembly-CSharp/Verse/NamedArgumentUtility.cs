using System;

namespace Verse
{
	// Token: 0x02000366 RID: 870
	public static class NamedArgumentUtility
	{
		// Token: 0x0600166B RID: 5739 RVA: 0x0006D6DD File Offset: 0x0006B8DD
		public static NamedArgument Named(this object arg, string label)
		{
			return new NamedArgument(arg, label);
		}
	}
}
