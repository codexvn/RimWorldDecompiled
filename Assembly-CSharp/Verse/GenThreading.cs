using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000D0B RID: 3339
	public static class GenThreading
	{
		// Token: 0x17000D54 RID: 3412
		// (get) Token: 0x06005577 RID: 21879 RVA: 0x001B90F3 File Offset: 0x001B72F3
		public static int ProcessorCount
		{
			get
			{
				return Environment.ProcessorCount;
			}
		}

		// Token: 0x06005578 RID: 21880 RVA: 0x001B90FA File Offset: 0x001B72FA
		private static void GetMaxDegreeOfParallelism(ref int maxDegreeOfParallelism)
		{
			if (maxDegreeOfParallelism == -1)
			{
				maxDegreeOfParallelism = GenThreading.ProcessorCount;
			}
		}

		// Token: 0x06005579 RID: 21881 RVA: 0x001B9108 File Offset: 0x001B7308
		public static void SliceWorkNoAlloc(int fromInclusive, int toExclusive, int maxBatches, List<GenThreading.Slice> batches)
		{
			int num = toExclusive - fromInclusive;
			if (num <= 0)
			{
				return;
			}
			int num2 = num % maxBatches;
			int num3 = Mathf.FloorToInt((float)num / (float)maxBatches);
			if (num3 > 0)
			{
				int num4 = 0;
				for (int i = 0; i < maxBatches; i++)
				{
					int num5 = num3;
					if (num2 > 0)
					{
						num5++;
						num2--;
					}
					batches.Add(new GenThreading.Slice(num4, num4 + num5));
					num4 += num5;
				}
				return;
			}
			for (int j = 0; j < num2; j++)
			{
				batches.Add(new GenThreading.Slice(j, j + 1));
			}
		}

		// Token: 0x0600557A RID: 21882 RVA: 0x001B918C File Offset: 0x001B738C
		public static List<GenThreading.Slice> SliceWork(int fromInclusive, int toExclusive, int maxBatches)
		{
			List<GenThreading.Slice> list = new List<GenThreading.Slice>(maxBatches);
			GenThreading.SliceWorkNoAlloc(fromInclusive, toExclusive, maxBatches, list);
			return list;
		}

		// Token: 0x0600557B RID: 21883 RVA: 0x001B91AC File Offset: 0x001B73AC
		public static List<List<T>> SliceWork<T>(List<T> list, int maxBatches)
		{
			List<List<T>> list2 = new List<List<T>>(maxBatches);
			foreach (GenThreading.Slice slice in GenThreading.SliceWork(0, list.Count, maxBatches))
			{
				List<T> list3 = new List<T>(slice.toExclusive - slice.fromInclusive);
				for (int i = slice.fromInclusive; i < slice.toExclusive; i++)
				{
					list3.Add(list[i]);
				}
				list2.Add(list3);
			}
			return list2;
		}

		// Token: 0x0600557C RID: 21884 RVA: 0x001B924C File Offset: 0x001B744C
		public static void ParallelForEach<T>(List<T> list, Action<T> callback, int maxDegreeOfParallelism = -1)
		{
			GenThreading.GetMaxDegreeOfParallelism(ref maxDegreeOfParallelism);
			int count = list.Count;
			long tasksDone = 0L;
			AutoResetEvent taskDoneEvent = new AutoResetEvent(false);
			using (List<List<T>>.Enumerator enumerator = GenThreading.SliceWork<T>(list, maxDegreeOfParallelism).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					List<T> localBatch2 = enumerator.Current;
					List<T> localBatch = localBatch2;
					ThreadPool.QueueUserWorkItem(delegate(object _)
					{
						foreach (T obj in localBatch)
						{
							try
							{
								callback(obj);
							}
							catch (Exception ex)
							{
								string str = "Error in ParallelForEach(): ";
								Exception ex2 = ex;
								Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
							}
						}
						Interlocked.Add(ref tasksDone, (long)localBatch.Count);
						taskDoneEvent.Set();
					});
				}
				goto IL_8F;
			}
			IL_83:
			taskDoneEvent.WaitOne();
			IL_8F:
			if (Interlocked.Read(ref tasksDone) >= (long)count)
			{
				return;
			}
			goto IL_83;
		}

		// Token: 0x0600557D RID: 21885 RVA: 0x001B9308 File Offset: 0x001B7508
		public static void ParallelFor(int fromInclusive, int toExclusive, Action<int> callback, int maxDegreeOfParallelism = -1)
		{
			GenThreading.GetMaxDegreeOfParallelism(ref maxDegreeOfParallelism);
			int num = toExclusive - fromInclusive;
			long tasksDone = 0L;
			AutoResetEvent taskDoneEvent = new AutoResetEvent(false);
			using (List<GenThreading.Slice>.Enumerator enumerator = GenThreading.SliceWork(fromInclusive, toExclusive, maxDegreeOfParallelism).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					GenThreading.Slice localBatch2 = enumerator.Current;
					GenThreading.Slice localBatch = localBatch2;
					ThreadPool.QueueUserWorkItem(delegate(object _)
					{
						for (int i = localBatch.fromInclusive; i < localBatch.toExclusive; i++)
						{
							try
							{
								callback(i);
							}
							catch (Exception ex)
							{
								string str = "Error in ParallelFor(): ";
								Exception ex2 = ex;
								Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
							}
						}
						Interlocked.Add(ref tasksDone, (long)(localBatch.toExclusive - localBatch.fromInclusive));
						taskDoneEvent.Set();
					});
				}
				goto IL_8D;
			}
			IL_81:
			taskDoneEvent.WaitOne();
			IL_8D:
			if (Interlocked.Read(ref tasksDone) >= (long)num)
			{
				return;
			}
			goto IL_81;
		}

		// Token: 0x02000D0C RID: 3340
		public struct Slice
		{
			// Token: 0x0600557E RID: 21886 RVA: 0x001B93C4 File Offset: 0x001B75C4
			public Slice(int fromInclusive, int toExclusive)
			{
				this.fromInclusive = fromInclusive;
				this.toExclusive = toExclusive;
			}

			// Token: 0x04003DCB RID: 15819
			public readonly int fromInclusive;

			// Token: 0x04003DCC RID: 15820
			public readonly int toExclusive;
		}
	}
}
