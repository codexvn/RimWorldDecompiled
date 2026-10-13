using System;
using Unity.Collections;
using Waylith.Jobs;

namespace Unity.Jobs;

public readonly struct JobHandle : IEquatable<JobHandle>
{
	private readonly int _id;

	internal int Id => _id;

	public bool IsCompleted => WaylithJobRuntime.IsCompleted(_id);

	internal JobHandle(int id)
	{
		_id = id;
	}

	public bool Equals(JobHandle other)
	{
		return _id == other._id;
	}

	public override bool Equals(object obj)
	{
		if (obj is JobHandle other)
		{
			return Equals(other);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return _id;
	}

	public static bool operator ==(JobHandle left, JobHandle right)
	{
		return left.Equals(right);
	}

	public static bool operator !=(JobHandle left, JobHandle right)
	{
		return !left.Equals(right);
	}

	public static JobHandle CombineDependencies(NativeArray<JobHandle> jobs)
	{
		if (jobs.Length == 0)
		{
			return default(JobHandle);
		}
		int[] array = new int[jobs.Length];
		int num = 0;
		for (int i = 0; i < jobs.Length; i++)
		{
			int id = jobs[i]._id;
			if (id != 0)
			{
				array[num++] = id;
			}
		}
		if (num != 0)
		{
			return new JobHandle(WaylithJobRuntime.CombineDependencies(array.AsSpan(0, num)));
		}
		return default(JobHandle);
	}

	public static JobHandle CombineDependencies(JobHandle job0, JobHandle job1)
	{
		Span<int> span = stackalloc int[2];
		int num = 0;
		if (job0._id != 0)
		{
			span[num++] = job0._id;
		}
		if (job1._id != 0)
		{
			span[num++] = job1._id;
		}
		if (num != 0)
		{
			return new JobHandle(WaylithJobRuntime.CombineDependencies(span.Slice(0, num)));
		}
		return default(JobHandle);
	}

	public void Complete()
	{
		WaylithJobRuntime.Complete(_id);
	}

	public static void CompleteAll(ref JobHandle job0, ref JobHandle job1)
	{
		job0.Complete();
		job1.Complete();
		job0 = default(JobHandle);
		job1 = default(JobHandle);
	}

	public static void CompleteAll(NativeArray<JobHandle> jobs)
	{
		for (int i = 0; i < jobs.Length; i++)
		{
			jobs[i].Complete();
		}
	}

	public static void ScheduleBatchedJobs()
	{
	}
}
