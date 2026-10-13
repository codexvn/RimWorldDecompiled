namespace Unity.Jobs;

public static class IJobForExtensions
{
	public static void EarlyJobInit<T0>() where T0 : struct, IJobFor
	{
	}

	public static void Run<T0>(this T0 jobData, int arrayLength) where T0 : struct, IJobFor
	{
		T0 val = jobData;
		for (int i = 0; i < arrayLength; i++)
		{
			val.Execute(i);
		}
	}

	public static JobHandle Schedule<T0>(this T0 jobData, int arrayLength, JobHandle dependency) where T0 : struct, IJobFor
	{
		if (arrayLength <= 0)
		{
			return dependency;
		}
		T0 scheduledJob = jobData;
		return UnityJobScheduling.Schedule(delegate
		{
			T0 val = scheduledJob;
			for (int i = 0; i < arrayLength; i++)
			{
				val.Execute(i);
			}
		}, dependency, arrayLength);
	}

	public static JobHandle ScheduleParallel<T0>(this T0 jobData, int arrayLength, int innerloopBatchCount, JobHandle dependency) where T0 : struct, IJobFor
	{
		return UnityJobScheduling.ScheduleParallel(arrayLength, innerloopBatchCount, dependency, delegate(int start, int end)
		{
			T0 chunkJob = jobData;
			return delegate
			{
				T0 val = chunkJob;
				for (int i = start; i < end; i++)
				{
					val.Execute(i);
				}
			};
		});
	}
}
