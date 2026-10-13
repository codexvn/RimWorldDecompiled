using System;
using LudeonTK;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace Verse.Glow
{
	// Token: 0x02000DF6 RID: 3574
	public struct GlowUniqueState : IDisposable
	{
		// Token: 0x06005AE8 RID: 23272 RVA: 0x001D2DEC File Offset: 0x001D0FEC
		public static GlowUniqueState AllocateNew()
		{
			GlowUniqueState result = new GlowUniqueState
			{
				queue = new UnsafeHeap<IntVec3, GlowCellComparer>(Allocator.Persistent, 324, default(GlowCellComparer)),
				area = new UnsafeList<GlowCell>(6561, Allocator.Persistent, NativeArrayOptions.UninitializedMemory),
				blockers = new UnsafeBitArray(8, Allocator.Persistent, NativeArrayOptions.ClearMemory)
			};
			result.area.Resize(6561, NativeArrayOptions.ClearMemory);
			return result;
		}

		// Token: 0x06005AE9 RID: 23273 RVA: 0x001D2E5D File Offset: 0x001D105D
		public void PrepareComparer(ref GlowLight light)
		{
			this.queue.Comparator = new GlowCellComparer(this.area, ref light);
		}

		// Token: 0x06005AEA RID: 23274 RVA: 0x001D2E76 File Offset: 0x001D1076
		public void Clear()
		{
			NativeArrayUtility.MemClear<GlowCell>(this.area);
			this.queue.Clear();
			this.blockers.Clear();
		}

		// Token: 0x06005AEB RID: 23275 RVA: 0x001D2E99 File Offset: 0x001D1099
		public void Dispose()
		{
			ref this.area.EnsureDisposed<GlowCell>();
			ref this.queue.EnsureDisposed<IntVec3, GlowCellComparer>();
			ref this.blockers.EnsureDisposed();
		}

		// Token: 0x040041DD RID: 16861
		[NativeDisableParallelForRestriction]
		public UnsafeHeap<IntVec3, GlowCellComparer> queue;

		// Token: 0x040041DE RID: 16862
		[NativeDisableParallelForRestriction]
		public UnsafeList<GlowCell> area;

		// Token: 0x040041DF RID: 16863
		[NativeDisableParallelForRestriction]
		public UnsafeBitArray blockers;
	}
}
