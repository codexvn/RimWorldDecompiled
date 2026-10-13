namespace Unity.Jobs;

public static class IJobParallelForExtensions
{
	public static void EarlyJobInit<T0>() where T0 : struct, IJobParallelFor
	{
	}

	public static void Run<T0>(this T0 jobData, int arrayLength) where T0 : struct, IJobParallelFor
	{
		T0 val = ThreadIndexField<T0>.Inject(jobData);
		for (int i = 0; i < arrayLength; i++)
		{
			val.Execute(i);
		}
	}

	public static JobHandle Schedule<T0>(this T0 jobData, int arrayLength, int innerloopBatchCount, JobHandle dependsOn = default(JobHandle)) where T0 : struct, IJobParallelFor
	{
		return UnityJobScheduling.ScheduleParallel(arrayLength, innerloopBatchCount, dependsOn, delegate(int start, int end)
		{
			T0 chunkJob = jobData;
			return delegate
			{
				T0 val = ThreadIndexField<T0>.Inject(chunkJob);
				for (int i = start; i < end; i++)
				{
					val.Execute(i);
				}
			};
		});
	}
}
