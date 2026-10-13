using System;
using System.Runtime.CompilerServices;

namespace Unity.Collections
{
	// Token: 0x02000066 RID: 102
	internal struct HeapNode<[IsUnmanaged] T> where T : struct, ValueType
	{
		// Token: 0x04000113 RID: 275
		public T Item;

		// Token: 0x04000114 RID: 276
		public int TableIndex;
	}
}
