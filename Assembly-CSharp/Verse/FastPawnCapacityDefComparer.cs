using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020002B3 RID: 691
	public class FastPawnCapacityDefComparer : IEqualityComparer<PawnCapacityDef>
	{
		// Token: 0x060010F5 RID: 4341 RVA: 0x0004FA93 File Offset: 0x0004DC93
		public bool Equals(PawnCapacityDef x, PawnCapacityDef y)
		{
			return x == y;
		}

		// Token: 0x060010F6 RID: 4342 RVA: 0x0004FA99 File Offset: 0x0004DC99
		public int GetHashCode(PawnCapacityDef obj)
		{
			return obj.GetHashCode();
		}

		// Token: 0x04000E69 RID: 3689
		public static readonly FastPawnCapacityDefComparer Instance = new FastPawnCapacityDefComparer();
	}
}
