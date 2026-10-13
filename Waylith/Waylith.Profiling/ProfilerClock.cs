using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics.X86;
using System.Threading;

namespace Waylith.Profiling;

public static class ProfilerClock
{
	public static readonly bool UsesTimeStampCounter = HasInvariantTimeStampCounter();

	private static readonly bool FencedTimeStampCounter = Environment.GetEnvironmentVariable("WAYLITH_PROFILER_FENCED_TSC") == "1";

	private static long frequency;

	private static long resolution;

	public static long Frequency
	{
		get
		{
			long num = Volatile.Read(in frequency);
			if (num == 0L)
			{
				return EnsureCalibrated();
			}
			return num;
		}
	}

	public static long Resolution
	{
		get
		{
			long num = Volatile.Read(in resolution);
			if (num != 0L)
			{
				return num;
			}
			EnsureCalibrated();
			return Volatile.Read(in resolution);
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static long GetTimestamp()
	{
		if (UsesTimeStampCounter)
		{
			if (FencedTimeStampCounter)
			{
				Sse2.LoadFence();
			}
			(uint CounterLower, uint CounterUpper) tuple = X86Base.ReadTimeStampCounter();
			var (num, _) = tuple;
			return (long)(((ulong)tuple.CounterUpper << 32) | num);
		}
		return Stopwatch.GetTimestamp();
	}

	public static long EnsureCalibrated()
	{
		if (Volatile.Read(in resolution) == 0L)
		{
			Interlocked.CompareExchange(ref resolution, MeasureResolution(), 0L);
		}
		long num = Volatile.Read(in frequency);
		if (num != 0L)
		{
			return num;
		}
		long num2 = (UsesTimeStampCounter ? Measure() : Stopwatch.Frequency);
		long num3 = Interlocked.CompareExchange(ref frequency, num2, 0L);
		if (num3 == 0L)
		{
			return num2;
		}
		return num3;
	}

	private static long MeasureResolution()
	{
		long num = long.MaxValue;
		for (int i = 0; i < 4096; i++)
		{
			long timestamp = GetTimestamp();
			long num2 = GetTimestamp() - timestamp;
			if (num2 > 0 && num2 < num)
			{
				num = num2;
			}
		}
		if (num != long.MaxValue)
		{
			return num;
		}
		return 1L;
	}

	private static long Measure()
	{
		(long, long) tuple = SampleStopwatchPairing();
		Thread.Sleep(10);
		(long Counter, long Reference) tuple2 = SampleStopwatchPairing();
		long num = tuple2.Reference - tuple.Item2;
		long num2 = tuple2.Counter - tuple.Item1;
		if (num <= 0 || num2 <= 0)
		{
			return Stopwatch.Frequency;
		}
		return (long)((double)num2 * (double)Stopwatch.Frequency / (double)num);
	}

	internal static (long Counter, long Reference) SampleStopwatchPairing()
	{
		long num = long.MaxValue;
		(long, long) result = default((long, long));
		for (int i = 0; i < 8; i++)
		{
			long timestamp = Stopwatch.GetTimestamp();
			long item = ReadFencedTimestamp();
			long num2 = Stopwatch.GetTimestamp() - timestamp;
			if (num2 < num)
			{
				num = num2;
				result = (item, timestamp + num2 / 2);
			}
		}
		return result;
	}

	internal static long FromStopwatchTimestamp(long stopwatchTimestamp, (long Counter, long Reference) pairing)
	{
		if (stopwatchTimestamp == 0L || !UsesTimeStampCounter)
		{
			return stopwatchTimestamp;
		}
		return pairing.Counter - (long)((double)(pairing.Reference - stopwatchTimestamp) * ((double)Frequency / (double)Stopwatch.Frequency));
	}

	private static long ReadFencedTimestamp()
	{
		if (UsesTimeStampCounter)
		{
			Sse2.LoadFence();
			long timestamp = GetTimestamp();
			Sse2.LoadFence();
			return timestamp;
		}
		return GetTimestamp();
	}

	private static bool HasInvariantTimeStampCounter()
	{
		if (Environment.GetEnvironmentVariable("WAYLITH_PROFILER_NO_TSC") == "1")
		{
			return false;
		}
		if (!X86Base.IsSupported || !Sse2.IsSupported)
		{
			return false;
		}
		if ((X86Base.CpuId(1, 0).Edx & 0x10) == 0)
		{
			return false;
		}
		if ((uint)X86Base.CpuId(int.MinValue, 0).Eax < 2147483655u)
		{
			return false;
		}
		return (X86Base.CpuId(-2147483641, 0).Edx & 0x100) != 0;
	}
}
