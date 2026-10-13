using System;
using System.Runtime.CompilerServices;

namespace Unity.Collections
{
	// Token: 0x02000065 RID: 101
	internal struct HeapData<[IsUnmanaged] T, U> where T : struct, ValueType
	{
		// Token: 0x0400010E RID: 270
		public int Count;

		// Token: 0x0400010F RID: 271
		public int Capacity;

		// Token: 0x04000110 RID: 272
		public unsafe HeapNode<T>* Heap;

		// Token: 0x04000111 RID: 273
		public unsafe TableValue* Table;

		// Token: 0x04000112 RID: 274
		public U Comparator;
	}
}
