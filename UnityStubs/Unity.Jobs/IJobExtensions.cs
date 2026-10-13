namespace Unity.Jobs;

public static class IJobExtensions
{
	public static void EarlyJobInit<T0>() where T0 : struct, IJob
	{
	}

	public static void Run<T0>(this T0 jobData) where T0 : struct, IJob
	{
		T0 val = jobData;
		val.Execute();
	}

	public static JobHandle Schedule<T0>(this T0 jobData, JobHandle dependsOn = default(JobHandle)) where T0 : struct, IJob
	{
		return UnityJobScheduling.Schedule(delegate
		{
			T0 val = jobData;
			val.Execute();
		}, dependsOn);
	}
}
