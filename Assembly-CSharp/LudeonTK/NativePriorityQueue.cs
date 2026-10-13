using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;

namespace LudeonTK
{
	// Token: 0x020000B5 RID: 181
	[NativeContainer]
	[DebuggerDisplay("Count = {Count}, Capacity = {Capacity}, IsCreated = {IsCreated}")]
	public struct NativePriorityQueue<[IsUnmanaged] TElement, [IsUnmanaged] TPriority, [IsUnmanaged] TComparer> : IDisposable where TElement : struct, ValueType where TPriority : struct, ValueType where TComparer : struct, ValueType, IComparer<TPriority>
	{
		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x060003E7 RID: 999 RVA: 0x00016BD3 File Offset: 0x00014DD3
		public bool IsCreated
		{
			get
			{
				return this.m_ElementBuffer != null;
			}
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x060003E8 RID: 1000 RVA: 0x00016BE2 File Offset: 0x00014DE2
		public int Capacity
		{
			get
			{
				return this.m_Capacity;
			}
		}

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x060003E9 RID: 1001 RVA: 0x00016BEA File Offset: 0x00014DEA
		public unsafe int Count
		{
			get
			{
				if (this.m_Data == null)
				{
					return 0;
				}
				return this.m_Data->m_Count;
			}
		}

		// Token: 0x060003EA RID: 1002 RVA: 0x00016C04 File Offset: 0x00014E04
		public unsafe NativePriorityQueue(int capacity, TComparer comparer, Allocator allocator)
		{
			long size = (long)UnsafeUtility.SizeOf<TElement>() * (long)capacity;
			this.m_ElementBuffer = UnsafeUtility.Malloc(size, UnsafeUtility.AlignOf<TElement>(), allocator);
			long size2 = (long)UnsafeUtility.SizeOf<TPriority>() * (long)capacity;
			this.m_PriorityBuffer = UnsafeUtility.Malloc(size2, UnsafeUtility.AlignOf<TPriority>(), allocator);
			this.m_Data = (NativePriorityQueue<TElement, TPriority, TComparer>.Data*)UnsafeUtility.Malloc((long)UnsafeUtility.SizeOf<NativePriorityQueue<TElement, TPriority, TComparer>.Data>(), UnsafeUtility.AlignOf<NativePriorityQueue<TElement, TPriority, TComparer>.Data>(), allocator);
			this.m_Data->m_Count = 0;
			this.m_Capacity = capacity;
			this.m_Comparer = comparer;
			this.m_AllocatorLabel = allocator;
		}

		// Token: 0x060003EB RID: 1003 RVA: 0x00016C81 File Offset: 0x00014E81
		[WriteAccessRequired]
		public unsafe void Clear()
		{
			this.m_Data->m_Count = 0;
		}

		// Token: 0x060003EC RID: 1004 RVA: 0x00016C90 File Offset: 0x00014E90
		[WriteAccessRequired]
		public unsafe void Enqueue(TElement element, TPriority priority)
		{
			int i = this.m_Data->m_Count;
			this.m_Data->m_Count = i + 1;
			TComparer comparer = this.m_Comparer;
			while (i > 0)
			{
				int num = i - 1 >> 2;
				TPriority tpriority = UnsafeUtility.ReadArrayElement<TPriority>(this.m_PriorityBuffer, num);
				if (comparer.Compare(priority, tpriority) >= 0)
				{
					break;
				}
				TElement value = UnsafeUtility.ReadArrayElement<TElement>(this.m_ElementBuffer, num);
				UnsafeUtility.WriteArrayElement<TElement>(this.m_ElementBuffer, i, value);
				UnsafeUtility.WriteArrayElement<TPriority>(this.m_PriorityBuffer, i, tpriority);
				i = num;
			}
			UnsafeUtility.WriteArrayElement<TElement>(this.m_ElementBuffer, i, element);
			UnsafeUtility.WriteArrayElement<TPriority>(this.m_PriorityBuffer, i, priority);
		}

		// Token: 0x060003ED RID: 1005 RVA: 0x00016D2E File Offset: 0x00014F2E
		[WriteAccessRequired]
		public void Dequeue(out TElement element, out TPriority priority)
		{
			element = UnsafeUtility.ReadArrayElement<TElement>(this.m_ElementBuffer, 0);
			priority = UnsafeUtility.ReadArrayElement<TPriority>(this.m_PriorityBuffer, 0);
			this.RemoveRootNode();
		}

		// Token: 0x060003EE RID: 1006 RVA: 0x00016D5C File Offset: 0x00014F5C
		private unsafe void RemoveRootNode()
		{
			int num = this.m_Data->m_Count - 1;
			this.m_Data->m_Count = num;
			if (num > 0)
			{
				TElement element = UnsafeUtility.ReadArrayElement<TElement>(this.m_ElementBuffer, num);
				TPriority priority = UnsafeUtility.ReadArrayElement<TPriority>(this.m_PriorityBuffer, num);
				this.MoveDown(element, priority, 0);
			}
		}

		// Token: 0x060003EF RID: 1007 RVA: 0x00016DAC File Offset: 0x00014FAC
		private void MoveDown(TElement element, TPriority priority, int nodeIndex)
		{
			TComparer comparer = this.m_Comparer;
			int count = this.Count;
			int num;
			while ((num = (nodeIndex << 2) + 1) < count)
			{
				TElement value = UnsafeUtility.ReadArrayElement<TElement>(this.m_ElementBuffer, num);
				TPriority tpriority = UnsafeUtility.ReadArrayElement<TPriority>(this.m_PriorityBuffer, num);
				int num2 = num;
				int num3 = math.min(num + 4, count);
				while (++num < num3)
				{
					TPriority tpriority2 = UnsafeUtility.ReadArrayElement<TPriority>(this.m_PriorityBuffer, num);
					if (comparer.Compare(tpriority2, tpriority) < 0)
					{
						value = UnsafeUtility.ReadArrayElement<TElement>(this.m_ElementBuffer, num);
						tpriority = tpriority2;
						num2 = num;
					}
				}
				if (comparer.Compare(priority, tpriority) <= 0)
				{
					break;
				}
				UnsafeUtility.WriteArrayElement<TElement>(this.m_ElementBuffer, nodeIndex, value);
				UnsafeUtility.WriteArrayElement<TPriority>(this.m_PriorityBuffer, nodeIndex, tpriority);
				nodeIndex = num2;
			}
			UnsafeUtility.WriteArrayElement<TElement>(this.m_ElementBuffer, nodeIndex, element);
			UnsafeUtility.WriteArrayElement<TPriority>(this.m_PriorityBuffer, nodeIndex, priority);
		}

		// Token: 0x060003F0 RID: 1008 RVA: 0x00016E90 File Offset: 0x00015090
		[WriteAccessRequired]
		public unsafe void Dispose()
		{
			if (this.m_ElementBuffer == null)
			{
				throw new ObjectDisposedException("The collection is already disposed.");
			}
			if (this.m_AllocatorLabel == Allocator.Invalid)
			{
				throw new InvalidOperationException("The collection can not be Disposed because it was not allocated with a valid allocator.");
			}
			if (this.m_AllocatorLabel > Allocator.None)
			{
				UnsafeUtility.Free(this.m_ElementBuffer, this.m_AllocatorLabel);
				UnsafeUtility.Free(this.m_PriorityBuffer, this.m_AllocatorLabel);
				UnsafeUtility.Free((void*)this.m_Data, this.m_AllocatorLabel);
				this.m_AllocatorLabel = Allocator.Invalid;
			}
			this.m_ElementBuffer = null;
			this.m_PriorityBuffer = null;
		}

		// Token: 0x060003F1 RID: 1009 RVA: 0x00016F18 File Offset: 0x00015118
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		internal static void CheckAllocateArguments(int capacity, Allocator allocator)
		{
			if (capacity < 0)
			{
				throw new ArgumentOutOfRangeException("capacity", "Capacity must be >= 0");
			}
			if (allocator <= Allocator.None || allocator > Allocator.Persistent)
			{
				throw new ArgumentException("Allocator must be Temp, TempJob or Persistent", "allocator");
			}
		}

		// Token: 0x060003F2 RID: 1010 RVA: 0x00016F46 File Offset: 0x00015146
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		internal static void CheckTotalSize(long totalSize)
		{
			if (totalSize > 2147483647L)
			{
				throw new ArgumentOutOfRangeException("totalSize", string.Format("Allocation total size cannot exceed {0} bytes", int.MaxValue));
			}
		}

		// Token: 0x060003F3 RID: 1011 RVA: 0x00016F70 File Offset: 0x00015170
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		internal static void CheckNotEmpty(int count)
		{
			if (count == 0)
			{
				throw new InvalidOperationException("The container is empty");
			}
		}

		// Token: 0x060003F4 RID: 1012 RVA: 0x00016F80 File Offset: 0x00015180
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		internal static void CheckSufficientCapacity(int capacity, int index)
		{
			if (index >= capacity)
			{
				throw new InvalidOperationException("The container has a fixed capacity and is already full.");
			}
		}

		// Token: 0x04000265 RID: 613
		[NativeDisableUnsafePtrRestriction]
		internal unsafe void* m_ElementBuffer;

		// Token: 0x04000266 RID: 614
		[NativeDisableUnsafePtrRestriction]
		internal unsafe void* m_PriorityBuffer;

		// Token: 0x04000267 RID: 615
		[NativeDisableUnsafePtrRestriction]
		internal unsafe NativePriorityQueue<TElement, TPriority, TComparer>.Data* m_Data;

		// Token: 0x04000268 RID: 616
		internal int m_Capacity;

		// Token: 0x04000269 RID: 617
		internal TComparer m_Comparer;

		// Token: 0x0400026A RID: 618
		internal Allocator m_AllocatorLabel;

		// Token: 0x0400026B RID: 619
		private const int Arity = 4;

		// Token: 0x0400026C RID: 620
		private const int Log2Arity = 2;

		// Token: 0x020000B6 RID: 182
		internal struct Data
		{
			// Token: 0x0400026D RID: 621
			public int m_Count;
		}
	}
}
