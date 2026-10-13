using System;
using System.Collections.Generic;
using System.Text;

namespace Waylith.Graphics;

public static class GpuMemoryHistory
{
	public const string TotalLabel = "(total tracked)";

	public const string DriverLabel = "(driver)";

	public const string OverflowLabel = "(other)";

	private const int Capacity = 512;

	private const int MaxLabels = 1024;

	private const float InitialIntervalSeconds = 2f;

	private static readonly object Gate = new object();

	private static readonly Dictionary<string, GpuMemoryTracker.LabelStat> aggregateScratch = new Dictionary<string, GpuMemoryTracker.LabelStat>();

	private static readonly Dictionary<string, long[]> seriesByLabel = new Dictionary<string, long[]>();

	private static readonly List<string> pruneScratch = new List<string>();

	private static bool started;

	private static int count;

	private static float intervalSeconds = 2f;

	private static float nextSampleTime;

	public static float IntervalSeconds
	{
		get
		{
			lock (Gate)
			{
				return intervalSeconds;
			}
		}
	}

	public static int SampleCount
	{
		get
		{
			lock (Gate)
			{
				return count;
			}
		}
	}

	public static void StartCounting()
	{
		started = true;
	}

	public static void Update(float elapsedTime)
	{
		if (!started || elapsedTime < nextSampleTime)
		{
			return;
		}
		nextSampleTime = elapsedTime + intervalSeconds;
		GpuMemoryTracker.AggregateByLabel(aggregateScratch);
		GpuMemoryReport.TryGetDeviceLocalDriverUsage(out var usageBytes);
		lock (Gate)
		{
			if (count == 512)
			{
				Decimate();
			}
			foreach (var (label, labelStat2) in aggregateScratch)
			{
				SeriesFor(label)[count] += labelStat2.Bytes;
			}
			SeriesFor("(total tracked)")[count] = GpuMemoryTracker.TotalBytes();
			SeriesFor("(driver)")[count] = usageBytes;
			count++;
		}
	}

	private static long[] SeriesFor(string label)
	{
		if (seriesByLabel.TryGetValue(label, out long[] value))
		{
			return value;
		}
		if (seriesByLabel.Count >= 1024 && label != "(other)")
		{
			return SeriesFor("(other)");
		}
		value = new long[512];
		seriesByLabel.Add(label, value);
		return value;
	}

	private static void Decimate()
	{
		int num = count / 2;
		pruneScratch.Clear();
		foreach (KeyValuePair<string, long[]> item2 in seriesByLabel)
		{
			item2.Deconstruct(out var key, out var value);
			string item = key;
			long[] array = value;
			long num2 = 0L;
			for (int i = 0; i < num; i++)
			{
				array[i] = System.Math.Max(array[2 * i], array[2 * i + 1]);
				num2 = System.Math.Max(num2, array[i]);
			}
			Array.Clear(array, num, count - num);
			if (num2 == 0L)
			{
				pruneScratch.Add(item);
			}
		}
		foreach (string item3 in pruneScratch)
		{
			seriesByLabel.Remove(item3);
		}
		pruneScratch.Clear();
		count = num;
		intervalSeconds *= 2f;
	}

	public static void CollectSeries(string label, List<long> into)
	{
		ArgumentNullException.ThrowIfNull(into, "into");
		into.Clear();
		lock (Gate)
		{
			if (seriesByLabel.TryGetValue(label, out long[] value))
			{
				for (int i = 0; i < count; i++)
				{
					into.Add(value[i]);
				}
			}
		}
	}

	public static void AppendGrowthSummary(StringBuilder sb, int topN = 12, long minGrowthBytes = 1048576L)
	{
		ArgumentNullException.ThrowIfNull(sb, "sb");
		lock (Gate)
		{
			if (count < 2)
			{
				sb.AppendLine("VRAM history: no samples recorded yet.");
				return;
			}
			float num = (float)count * intervalSeconds / 60f;
			sb.Append("VRAM history (window ").Append(num.ToString("0.0")).Append(" min, sample interval ")
				.Append(intervalSeconds.ToString("0"))
				.Append(" s), top growth:")
				.AppendLine();
			List<(string, long[], long)> list = new List<(string, long[], long)>();
			foreach (KeyValuePair<string, long[]> item5 in seriesByLabel)
			{
				item5.Deconstruct(out var key, out var value);
				string item = key;
				long[] array = value;
				long num2 = array[count - 1] - array[0];
				if (num2 >= minGrowthBytes)
				{
					list.Add((item, array, num2));
				}
			}
			list.Sort(((string Label, long[] Series, long Growth) a, (string Label, long[] Series, long Growth) b) => b.Growth.CompareTo(a.Growth));
			int num3 = System.Math.Min(list.Count, topN);
			int num4 = System.Math.Max(2, count / 4);
			for (int i = 0; i < num3; i++)
			{
				(string, long[], long) tuple = list[i];
				string item2 = tuple.Item1;
				long[] item3 = tuple.Item2;
				long item4 = tuple.Item3;
				long num5 = item3[count - 1] - item3[count - num4];
				float val = (float)(num4 - 1) * intervalSeconds / 60f;
				sb.Append("  ").Append(item2).Append(": ")
					.Append(GpuMemoryReport.FormatBytes(item3[0]))
					.Append(" -> ")
					.Append(GpuMemoryReport.FormatBytes(item3[count - 1]))
					.Append(" (+")
					.Append(GpuMemoryReport.FormatBytes(item4))
					.Append(", recent ")
					.Append(((float)num5 / 1048576f / System.Math.Max(0.01f, val)).ToString("+0.0;-0.0;0"))
					.Append(" MiB/min)")
					.AppendLine();
			}
			if (num3 == 0)
			{
				sb.AppendLine("  (no label grew by 1 MiB or more across the window)");
			}
		}
	}
}
