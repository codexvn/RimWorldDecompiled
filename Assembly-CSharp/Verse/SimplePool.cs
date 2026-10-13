using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000165 RID: 357
	public static class SimplePool<T> where T : new()
	{
		// Token: 0x17000198 RID: 408
		// (get) Token: 0x06000A8D RID: 2701 RVA: 0x00031E91 File Offset: 0x00030091
		public static int FreeItemsCount
		{
			get
			{
				return SimplePool<T>.freeItems.Count;
			}
		}

		// Token: 0x06000A8E RID: 2702 RVA: 0x00031EA0 File Offset: 0x000300A0
		public static T Get()
		{
			T result;
			if (!SimplePool<T>.freeItems.TryDequeue(out result))
			{
				result = Activator.CreateInstance<T>();
			}
			return result;
		}

		// Token: 0x06000A8F RID: 2703 RVA: 0x00031EC2 File Offset: 0x000300C2
		public static void Return(T item)
		{
			SimplePool<T>.freeItems.Enqueue(item);
		}

		// Token: 0x0400058D RID: 1421
		private static readonly Queue<T> freeItems = new Queue<T>();
	}
}
