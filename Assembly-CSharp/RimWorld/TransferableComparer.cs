using System;
using System.Collections.Generic;

namespace RimWorld
{
	// Token: 0x02001B22 RID: 6946
	public abstract class TransferableComparer : IComparer<Transferable>
	{
		// Token: 0x06009B28 RID: 39720
		public abstract int Compare(Transferable lhs, Transferable rhs);
	}
}
