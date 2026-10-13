using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Unity.Collections
{
	// Token: 0x02000063 RID: 99
	internal class NativeHeapDebugView<[IsUnmanaged] T, [IsUnmanaged] U> where T : struct, ValueType where U : struct, ValueType, IComparer<T>
	{
		// Token: 0x06000224 RID: 548 RVA: 0x0000C667 File Offset: 0x0000A867
		public NativeHeapDebugView(UnsafeHeap<T, U> heap)
		{
			this._heap = heap;
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x06000225 RID: 549 RVA: 0x0000C676 File Offset: 0x0000A876
		public int Count
		{
			get
			{
				return this._heap.Count;
			}
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x06000226 RID: 550 RVA: 0x0000C683 File Offset: 0x0000A883
		public int Capacity
		{
			get
			{
				return this._heap.Capacity;
			}
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x06000227 RID: 551 RVA: 0x0000C690 File Offset: 0x0000A890
		public U Comparator
		{
			get
			{
				return this._heap.Comparator;
			}
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x06000228 RID: 552 RVA: 0x0000C6A0 File Offset: 0x0000A8A0
		public unsafe T[] Items
		{
			get
			{
				T[] array = new T[this._heap.Count];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = this._heap.Data->Heap[(IntPtr)i * (IntPtr)sizeof(HeapNode<T>) / (IntPtr)sizeof(HeapNode<T>)].Item;
				}
				return array;
			}
		}

		// Token: 0x0400010C RID: 268
		private UnsafeHeap<T, U> _heap;
	}
}
