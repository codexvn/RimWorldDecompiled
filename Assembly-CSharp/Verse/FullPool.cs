using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000167 RID: 359
	public static class FullPool<T> where T : IFullPoolable, new()
	{
		// Token: 0x06000A92 RID: 2706 RVA: 0x00031EDC File Offset: 0x000300DC
		public static T Get()
		{
			T result;
			if (!FullPool<T>.freeItems.TryDequeue(out result))
			{
				result = Activator.CreateInstance<T>();
			}
			return result;
		}

		// Token: 0x06000A93 RID: 2707 RVA: 0x00031EFE File Offset: 0x000300FE
		public static void Return(T item)
		{
			item.Reset();
			FullPool<T>.freeItems.Enqueue(item);
		}

		// Token: 0x0400058E RID: 1422
		private static readonly Queue<T> freeItems = new Queue<T>();
	}
}
