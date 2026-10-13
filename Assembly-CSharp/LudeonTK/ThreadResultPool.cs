using System;
using Unity.Collections;
using Verse;

namespace LudeonTK
{
	// Token: 0x020000B7 RID: 183
	public static class ThreadResultPool
	{
		// Token: 0x060003F5 RID: 1013 RVA: 0x00016F91 File Offset: 0x00015191
		public static void EnsureReady()
		{
			ThreadResultPool.EnsureAllocated();
			ThreadResultPool.Reset();
		}

		// Token: 0x060003F6 RID: 1014 RVA: 0x00016FA0 File Offset: 0x000151A0
		private static void EnsureAllocated()
		{
			if (ThreadResultPool.allocated)
			{
				return;
			}
			ThreadResultPool.allocated = true;
			UnityData.DisposeStatic += ThreadResultPool.Dispose;
			ThreadResultPool.closestResults = new NativeArray<int>(128, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			ThreadResultPool.closestSqrDists = new NativeArray<float>(128, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
		}

		// Token: 0x060003F7 RID: 1015 RVA: 0x00016FF0 File Offset: 0x000151F0
		private static void Reset()
		{
			if (!ThreadResultPool.closestResults.IsCreated)
			{
				return;
			}
			for (int i = 0; i < ThreadResultPool.closestResults.Length; i++)
			{
				ThreadResultPool.closestResults[i] = -1;
				ThreadResultPool.closestSqrDists[i] = float.MaxValue;
			}
		}

		// Token: 0x060003F8 RID: 1016 RVA: 0x0001703B File Offset: 0x0001523B
		private static void Dispose()
		{
			if (!ThreadResultPool.allocated)
			{
				return;
			}
			ThreadResultPool.closestResults.Dispose();
			ThreadResultPool.closestSqrDists.Dispose();
			ThreadResultPool.allocated = false;
		}

		// Token: 0x0400026E RID: 622
		public static NativeArray<int> closestResults;

		// Token: 0x0400026F RID: 623
		public static NativeArray<float> closestSqrDists;

		// Token: 0x04000270 RID: 624
		private static bool allocated;
	}
}
