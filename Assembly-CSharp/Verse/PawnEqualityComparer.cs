using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020007A1 RID: 1953
	public class PawnEqualityComparer : IEqualityComparer<Pawn>
	{
		// Token: 0x06002FF7 RID: 12279 RVA: 0x000F6017 File Offset: 0x000F4217
		public bool Equals(Pawn x, Pawn y)
		{
			return x.Equals(y);
		}

		// Token: 0x06002FF8 RID: 12280 RVA: 0x0004FA99 File Offset: 0x0004DC99
		public int GetHashCode(Pawn obj)
		{
			return obj.GetHashCode();
		}
	}
}
