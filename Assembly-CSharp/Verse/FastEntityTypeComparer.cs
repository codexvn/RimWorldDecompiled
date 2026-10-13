using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000446 RID: 1094
	public class FastEntityTypeComparer : IEqualityComparer<ThingCategory>
	{
		// Token: 0x06001C5C RID: 7260 RVA: 0x0004FA93 File Offset: 0x0004DC93
		public bool Equals(ThingCategory x, ThingCategory y)
		{
			return x == y;
		}

		// Token: 0x06001C5D RID: 7261 RVA: 0x00009C2B File Offset: 0x00007E2B
		public int GetHashCode(ThingCategory obj)
		{
			return (int)obj;
		}

		// Token: 0x0400184C RID: 6220
		public static readonly FastEntityTypeComparer Instance = new FastEntityTypeComparer();
	}
}
