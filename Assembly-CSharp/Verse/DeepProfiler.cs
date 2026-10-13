using System;
using System.Collections.Generic;
using System.Threading;

namespace Verse
{
	// Token: 0x02000CA3 RID: 3235
	public static class DeepProfiler
	{
		// Token: 0x060052FB RID: 21243 RVA: 0x001AC0E4 File Offset: 0x001AA2E4
		public static ThreadLocalDeepProfiler Get()
		{
			object deepProfilersLock = DeepProfiler.DeepProfilersLock;
			ThreadLocalDeepProfiler result;
			lock (deepProfilersLock)
			{
				int managedThreadId = Thread.CurrentThread.ManagedThreadId;
				ThreadLocalDeepProfiler threadLocalDeepProfiler;
				if (!DeepProfiler.deepProfilers.TryGetValue(managedThreadId, out threadLocalDeepProfiler))
				{
					threadLocalDeepProfiler = new ThreadLocalDeepProfiler();
					DeepProfiler.deepProfilers.Add(managedThreadId, threadLocalDeepProfiler);
					result = threadLocalDeepProfiler;
				}
				else
				{
					result = threadLocalDeepProfiler;
				}
			}
			return result;
		}

		// Token: 0x060052FC RID: 21244 RVA: 0x001AC154 File Offset: 0x001AA354
		public static void Start(string label = null)
		{
			if (!DeepProfiler.enabled || !Prefs.LogVerbose)
			{
				return;
			}
			DeepProfiler.Get().Start(label);
		}

		// Token: 0x060052FD RID: 21245 RVA: 0x001AC172 File Offset: 0x001AA372
		public static void End()
		{
			if (!DeepProfiler.enabled || !Prefs.LogVerbose)
			{
				return;
			}
			DeepProfiler.Get().End();
		}

		// Token: 0x04003BF4 RID: 15348
		public static volatile bool enabled = true;

		// Token: 0x04003BF5 RID: 15349
		private static Dictionary<int, ThreadLocalDeepProfiler> deepProfilers = new Dictionary<int, ThreadLocalDeepProfiler>();

		// Token: 0x04003BF6 RID: 15350
		private static readonly object DeepProfilersLock = new object();
	}
}
