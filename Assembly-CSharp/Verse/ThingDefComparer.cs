using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000447 RID: 1095
	public class ThingDefComparer : IEqualityComparer<ThingDef>
	{
		// Token: 0x06001C60 RID: 7264 RVA: 0x0008E71F File Offset: 0x0008C91F
		public bool Equals(ThingDef x, ThingDef y)
		{
			return (x == null && y == null) || (x != null && y != null && x.shortHash == y.shortHash);
		}

		// Token: 0x06001C61 RID: 7265 RVA: 0x0004FA99 File Offset: 0x0004DC99
		public int GetHashCode(ThingDef obj)
		{
			return obj.GetHashCode();
		}

		// Token: 0x0400184D RID: 6221
		public static readonly ThingDefComparer Instance = new ThingDefComparer();
	}
}
