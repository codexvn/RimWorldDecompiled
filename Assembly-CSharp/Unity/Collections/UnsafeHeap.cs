using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Collections
{
	// Token: 0x02000062 RID: 98
	[DebuggerDisplay("Count = {Count}")]
	[DebuggerTypeProxy(typeof(NativeHeapDebugView<, >))]
	public struct UnsafeHeap<[IsUnmanaged] T, [IsUnmanaged] U> : IDisposable where T : struct, ValueType where U : struct, ValueType, IComparer<T>
	{
		// Token: 0x1700005B RID: 91
		// (get) Token: 0x06000212 RID: 530 RVA: 0x0000BE64 File Offset: 0x0000A064
		// (set) Token: 0x06000213 RID: 531 RVA: 0x0000BE74 File Offset: 0x0000A074
		public unsafe int Capacity
		{
			get
			{
				return this.Data->Capacity;
			}
			set
			{
				TableValue* ptr = (TableValue*)UnsafeUtility.Malloc((long)(UnsafeUtility.SizeOf<TableValue>() * value), UnsafeUtility.AlignOf<TableValue>(), this.Allocator);
				HeapNode<T>* ptr2 = (HeapNode<T>*)UnsafeUtility.Malloc((long)(UnsafeUtility.SizeOf<HeapNode<T>>() * value), UnsafeUtility.AlignOf<HeapNode<T>>(), this.Allocator);
				int num = (this.Data->Capacity < value) ? this.Data->Capacity : value;
				UnsafeUtility.MemCpy((void*)ptr, (void*)this.Data->Table, (long)(num * UnsafeUtility.SizeOf<TableValue>()));
				UnsafeUtility.MemCpy((void*)ptr2, (void*)this.Data->Heap, (long)(num * UnsafeUtility.SizeOf<HeapNode<T>>()));
				for (int i = 0; i < value - this.Data->Capacity; i++)
				{
					ptr2[(IntPtr)(i + this.Data->Capacity) * (IntPtr)sizeof(HeapNode<T>) / (IntPtr)sizeof(HeapNode<T>)] = new HeapNode<T>
					{
						TableIndex = i + this.Data->Capacity
					};
				}
				UnsafeUtility.Free((void*)this.Data->Table, this.Allocator);
				UnsafeUtility.Free((void*)this.Data->Heap, this.Allocator);
				this.Data->Table = ptr;
				this.Data->Heap = ptr2;
				this.Data->Capacity = value;
			}
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x06000214 RID: 532 RVA: 0x0000BFA4 File Offset: 0x0000A1A4
		public unsafe int Count
		{
			get
			{
				return this.Data->Count;
			}
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x06000215 RID: 533 RVA: 0x0000BFB1 File Offset: 0x0000A1B1
		// (set) Token: 0x06000216 RID: 534 RVA: 0x0000BFBE File Offset: 0x0000A1BE
		public unsafe U Comparator
		{
			get
			{
				return this.Data->Comparator;
			}
			set
			{
				this.Data->Comparator = value;
			}
		}

		// Token: 0x06000217 RID: 535 RVA: 0x0000BFCC File Offset: 0x0000A1CC
		public UnsafeHeap(Allocator allocator, int initialCapacity = 128, U comparator = default(U))
		{
			this = new UnsafeHeap<T, U>(initialCapacity, comparator, allocator);
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x06000218 RID: 536 RVA: 0x0000BFD7 File Offset: 0x0000A1D7
		public bool IsCreated
		{
			get
			{
				return this.Data != null;
			}
		}

		// Token: 0x06000219 RID: 537 RVA: 0x0000BFE8 File Offset: 0x0000A1E8
		public unsafe void Dispose()
		{
			this.Data->Count = 0;
			this.Data->Capacity = 0;
			UnsafeUtility.Free((void*)this.Data->Heap, this.Allocator);
			UnsafeUtility.Free((void*)this.Data->Table, this.Allocator);
			UnsafeUtility.Free((void*)this.Data, this.Allocator);
			this.Data = null;
		}

		// Token: 0x0600021A RID: 538 RVA: 0x0000C052 File Offset: 0x0000A252
		public unsafe void Clear()
		{
			this.Data->Count = 0;
		}

		// Token: 0x0600021B RID: 539 RVA: 0x0000C060 File Offset: 0x0000A260
		public T Peek()
		{
			T result;
			if (!this.TryPeek(out result))
			{
				throw new InvalidOperationException("Cannot Peek NativeHeap when the count is zero.");
			}
			return result;
		}

		// Token: 0x0600021C RID: 540 RVA: 0x0000C083 File Offset: 0x0000A283
		public unsafe bool TryPeek(out T t)
		{
			if (this.Data->Count == 0)
			{
				t = default(T);
				return false;
			}
			t = this.Data->Heap->Item;
			return true;
		}

		// Token: 0x0600021D RID: 541 RVA: 0x0000C0B4 File Offset: 0x0000A2B4
		public T Pop()
		{
			T result;
			if (!this.TryPop(out result))
			{
				throw new InvalidOperationException("Cannot Pop NativeHeap when the count is zero.");
			}
			return result;
		}

		// Token: 0x0600021E RID: 542 RVA: 0x0000C0D8 File Offset: 0x0000A2D8
		public unsafe bool TryPop(out T t)
		{
			if (this.Data->Count == 0)
			{
				t = default(T);
				return false;
			}
			HeapNode<T> heapNode = *this.Data->Heap;
			HeapData<T, U>* data = this.Data;
			int num = data->Count - 1;
			data->Count = num;
			int num2 = num;
			HeapNode<T> node = this.Data->Heap[(IntPtr)num2 * (IntPtr)sizeof(HeapNode<T>) / (IntPtr)sizeof(HeapNode<T>)];
			this.Data->Heap[(IntPtr)num2 * (IntPtr)sizeof(HeapNode<T>) / (IntPtr)sizeof(HeapNode<T>)] = heapNode;
			this.InsertAndBubbleDown(node, 0);
			t = heapNode.Item;
			return true;
		}

		// Token: 0x0600021F RID: 543 RVA: 0x0000C16C File Offset: 0x0000A36C
		public unsafe NativeHeapIndex Insert(in T t)
		{
			if (this.Data->Count == this.Data->Capacity)
			{
				this.Capacity *= 2;
			}
			HeapNode<T> heapNode = this.Data->Heap[(IntPtr)this.Data->Count * (IntPtr)sizeof(HeapNode<T>) / (IntPtr)sizeof(HeapNode<T>)];
			heapNode.Item = t;
			HeapData<T, U>* data = this.Data;
			int count = data->Count;
			data->Count = count + 1;
			int insertIndex = count;
			this.InsertAndBubbleUp(heapNode, insertIndex);
			return new NativeHeapIndex
			{
				TableIndex = heapNode.TableIndex
			};
		}

		// Token: 0x06000220 RID: 544 RVA: 0x0000C204 File Offset: 0x0000A404
		public unsafe T Remove(NativeHeapIndex index)
		{
			int heapIndex = this.Data->Table[index.TableIndex].HeapIndex;
			HeapNode<T> heapNode = this.Data->Heap[(IntPtr)heapIndex * (IntPtr)sizeof(HeapNode<T>) / (IntPtr)sizeof(HeapNode<T>)];
			IntPtr heap = this.Data->Heap;
			HeapData<T, U>* data = this.Data;
			int num = data->Count - 1;
			data->Count = num;
			HeapNode<T> heapNode2 = *(heap + (IntPtr)num * (IntPtr)sizeof(HeapNode<T>));
			UnsafeUtility.WriteArrayElement<HeapNode<T>>((void*)this.Data->Heap, this.Data->Count, heapNode);
			if (heapIndex != 0)
			{
				int num2 = (heapIndex - 1) / 2;
				HeapNode<T> heapNode3 = this.Data->Heap[(IntPtr)num2 * (IntPtr)sizeof(HeapNode<T>) / (IntPtr)sizeof(HeapNode<T>)];
				if (this.Data->Comparator.Compare(heapNode2.Item, heapNode3.Item) < 0)
				{
					this.InsertAndBubbleUp(heapNode2, heapIndex);
					return heapNode.Item;
				}
			}
			this.InsertAndBubbleDown(heapNode2, heapIndex);
			return heapNode.Item;
		}

		// Token: 0x06000221 RID: 545 RVA: 0x0000C300 File Offset: 0x0000A500
		internal unsafe UnsafeHeap(int initialCapacity, U comparator, Allocator allocator)
		{
			this.Data = (HeapData<T, U>*)UnsafeUtility.Malloc((long)UnsafeUtility.SizeOf<HeapData<T, U>>(), UnsafeUtility.AlignOf<HeapData<T, U>>(), allocator);
			this.Data->Heap = (HeapNode<T>*)UnsafeUtility.Malloc((long)(UnsafeUtility.SizeOf<HeapNode<T>>() * initialCapacity), UnsafeUtility.AlignOf<HeapNode<T>>(), allocator);
			this.Data->Table = (TableValue*)UnsafeUtility.Malloc((long)(UnsafeUtility.SizeOf<TableValue>() * initialCapacity), UnsafeUtility.AlignOf<TableValue>(), allocator);
			this.Allocator = allocator;
			for (int i = 0; i < initialCapacity; i++)
			{
				this.Data->Heap[(IntPtr)i * (IntPtr)sizeof(HeapNode<T>) / (IntPtr)sizeof(HeapNode<T>)] = new HeapNode<T>
				{
					TableIndex = i
				};
			}
			this.Data->Count = 0;
			this.Data->Capacity = initialCapacity;
			this.Data->Comparator = comparator;
		}

		// Token: 0x06000222 RID: 546 RVA: 0x0000C3C4 File Offset: 0x0000A5C4
		internal unsafe void InsertAndBubbleDown(HeapNode<T> node, int insertIndex)
		{
			for (;;)
			{
				int num = insertIndex * 2 + 1;
				int num2 = insertIndex * 2 + 2;
				if (num >= this.Data->Count)
				{
					break;
				}
				if (num2 >= this.Data->Count || this.Data->Comparator.Compare(this.Data->Heap[(IntPtr)num * (IntPtr)sizeof(HeapNode<T>) / (IntPtr)sizeof(HeapNode<T>)].Item, this.Data->Heap[(IntPtr)num2 * (IntPtr)sizeof(HeapNode<T>) / (IntPtr)sizeof(HeapNode<T>)].Item) <= 0)
				{
					HeapNode<T> heapNode = this.Data->Heap[(IntPtr)num * (IntPtr)sizeof(HeapNode<T>) / (IntPtr)sizeof(HeapNode<T>)];
					if (this.Data->Comparator.Compare(node.Item, heapNode.Item) <= 0)
					{
						break;
					}
					this.Data->Heap[(IntPtr)insertIndex * (IntPtr)sizeof(HeapNode<T>) / (IntPtr)sizeof(HeapNode<T>)] = heapNode;
					this.Data->Table[heapNode.TableIndex].HeapIndex = insertIndex;
					insertIndex = num;
				}
				else
				{
					HeapNode<T> heapNode2 = this.Data->Heap[(IntPtr)num2 * (IntPtr)sizeof(HeapNode<T>) / (IntPtr)sizeof(HeapNode<T>)];
					if (this.Data->Comparator.Compare(node.Item, heapNode2.Item) <= 0)
					{
						break;
					}
					this.Data->Heap[(IntPtr)insertIndex * (IntPtr)sizeof(HeapNode<T>) / (IntPtr)sizeof(HeapNode<T>)] = heapNode2;
					this.Data->Table[heapNode2.TableIndex].HeapIndex = insertIndex;
					insertIndex = num2;
				}
			}
			this.Data->Heap[(IntPtr)insertIndex * (IntPtr)sizeof(HeapNode<T>) / (IntPtr)sizeof(HeapNode<T>)] = node;
			this.Data->Table[node.TableIndex].HeapIndex = insertIndex;
		}

		// Token: 0x06000223 RID: 547 RVA: 0x0000C590 File Offset: 0x0000A790
		internal unsafe void InsertAndBubbleUp(HeapNode<T> node, int insertIndex)
		{
			while (insertIndex != 0)
			{
				int num = (insertIndex - 1) / 2;
				HeapNode<T> heapNode = this.Data->Heap[(IntPtr)num * (IntPtr)sizeof(HeapNode<T>) / (IntPtr)sizeof(HeapNode<T>)];
				if (this.Data->Comparator.Compare(heapNode.Item, node.Item) <= 0)
				{
					break;
				}
				this.Data->Heap[(IntPtr)insertIndex * (IntPtr)sizeof(HeapNode<T>) / (IntPtr)sizeof(HeapNode<T>)] = heapNode;
				this.Data->Table[heapNode.TableIndex].HeapIndex = insertIndex;
				insertIndex = num;
			}
			this.Data->Heap[(IntPtr)insertIndex * (IntPtr)sizeof(HeapNode<T>) / (IntPtr)sizeof(HeapNode<T>)] = node;
			this.Data->Table[node.TableIndex].HeapIndex = insertIndex;
		}

		// Token: 0x04000106 RID: 262
		public const int DEFAULT_CAPACITY = 128;

		// Token: 0x04000107 RID: 263
		internal const int VALIDATION_ERROR_WRONG_INSTANCE = 1;

		// Token: 0x04000108 RID: 264
		internal const int VALIDATION_ERROR_INVALID = 2;

		// Token: 0x04000109 RID: 265
		internal const int VALIDATION_ERROR_REMOVED = 3;

		// Token: 0x0400010A RID: 266
		[NativeDisableUnsafePtrRestriction]
		internal unsafe HeapData<T, U>* Data;

		// Token: 0x0400010B RID: 267
		internal Allocator Allocator;
	}
}
