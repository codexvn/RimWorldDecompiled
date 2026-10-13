using System;
using System.Collections.Generic;
using Verse;

// Token: 0x02000044 RID: 68
public static class FleckUtility
{
	// Token: 0x06000163 RID: 355 RVA: 0x000097C3 File Offset: 0x000079C3
	public static FleckParallelizationInfo GetParallelizationInfo()
	{
		if (FleckUtility.parallelizationInfosPool.Count == 0)
		{
			return new FleckParallelizationInfo();
		}
		return FleckUtility.parallelizationInfosPool.Pop<FleckParallelizationInfo>();
	}

	// Token: 0x06000164 RID: 356 RVA: 0x000097E1 File Offset: 0x000079E1
	public static void ReturnParallelizationInfo(FleckParallelizationInfo info)
	{
		info.doneEvent.Reset();
		info.drawBatch.Flush(false);
		FleckUtility.parallelizationInfosPool.Add(info);
	}

	// Token: 0x040000AF RID: 175
	private static List<FleckParallelizationInfo> parallelizationInfosPool = new List<FleckParallelizationInfo>();
}
