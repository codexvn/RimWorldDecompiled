using System;
using Unity.Collections;
using Waylith.Jobs;

namespace Unity.Jobs;

public static class IJobParallelForDeferExtensions
{
	private struct DeferredJob<T0> : IWaylithParallelForJob where T0 : struct, IJobParallelForDefer
	{
		public T0 Job;

		public void Execute(int index)
		{
			Job.Execute(index);
		}
	}

	public static void EarlyJobInit<T0>() where T0 : struct, IJobParallelForDefer
	{
	}

	public static JobHandle Schedule<T0, T1>(this T0 jobData, NativeList<T1> list, int innerloopBatchCount, JobHandle dependsOn = default(JobHandle)) where T0 : struct, IJobParallelForDefer where T1 : unmanaged
	{
		int batchSize = Math.Max(1, innerloopBatchCount);
		return UnityJobScheduling.Schedule(delegate
		{
			int length = list.Length;
			if (length > 0)
			{
				DeferredJob<T0> job = default(DeferredJob<T0>);
				job.Job = jobData;
				WaylithJobRuntime.Complete(WaylithJobRuntime.ScheduleFor(job, length, batchSize));
			}
		}, dependsOn);
	}
}
