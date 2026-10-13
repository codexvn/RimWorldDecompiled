using System;

namespace Unity.IO.LowLevel.Unsafe;

public static class AsyncReadManagerMetrics
{
	[Flags]
	public enum Flags
	{
		None = 0,
		ClearOnRead = 1
	}

	public static bool IsEnabled()
	{
		return false;
	}

	public static void StartCollectingMetrics()
	{
	}

	public static void StopCollectingMetrics()
	{
	}

	public static void ClearCompletedMetrics()
	{
	}
}
