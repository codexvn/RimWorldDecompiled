using System;
using Waylith.Jobs;

namespace Unity.Jobs;

internal static class UnityJobScheduling
{
	internal static JobHandle Schedule(Action action, JobHandle dependsOn, int timeCostEstimate = 1)
	{
		Span<int> span = stackalloc int[1];
		int length = 0;
		if (dependsOn.Id != 0)
		{
			span[length++] = dependsOn.Id;
		}
		return new JobHandle(WaylithJobRuntime.Schedule(action, span.Slice(0, length), timeCostEstimate));
	}

	internal static JobHandle ScheduleParallel(int iterationCount, int innerloopBatchCount, JobHandle dependsOn, Func<int, int, Action> createAction)
	{
		if (iterationCount <= 0)
		{
			return dependsOn;
		}
		int num = Math.Max(1, innerloopBatchCount);
		int num2 = (iterationCount + num - 1) / num;
		if (num2 == 1)
		{
			return Schedule(createAction(0, iterationCount), dependsOn, iterationCount);
		}
		Action[] array = new Action[num2];
		int num3 = 0;
		for (int i = 0; i < iterationCount; i += num)
		{
			int arg = Math.Min(iterationCount, i + num);
			array[num3++] = createAction(i, arg);
		}
		Action[] actions = array;
		Span<int> span = stackalloc int[1] { dependsOn.Id };
		return new JobHandle(WaylithJobRuntime.ScheduleParallelBatch(actions, span, num));
	}
}
