using System;

namespace Unity.Jobs.LowLevel.Unsafe;

public static class JobsUtility
{
	public const int CacheLineSize = 64;

	public const int MaxJobThreadCount = 128;

	public static int JobWorkerCount => Environment.ProcessorCount - 2;

	public static int ThreadIndexCount => Environment.ProcessorCount;
}
