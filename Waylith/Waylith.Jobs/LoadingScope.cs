using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace Waylith.Jobs;

public sealed class LoadingScope : IDisposable
{
	public readonly record struct Snapshot(string Label, float Fraction, string? Detail);

	private struct Milestone
	{
		public int Handle;

		public string Label;

		public int Weight;
	}

	private readonly Lock _lock = new Lock();

	private readonly List<Milestone> _milestones = new List<Milestone>();

	private readonly string _fallbackLabel;

	private readonly long _startTimestamp = Stopwatch.GetTimestamp();

	private int _totalWeight;

	private ProgressCounter? _activeCounter;

	private bool _disposed;

	public TimeSpan Elapsed => Stopwatch.GetElapsedTime(_startTimestamp);

	internal LoadingScope(string label)
	{
		_fallbackLabel = label ?? "";
	}

	public int Track(int handle, string? label, int weight = 1)
	{
		if (weight < 1)
		{
			weight = 1;
		}
		using (_lock.EnterScope())
		{
			_milestones.Add(new Milestone
			{
				Handle = handle,
				Label = (label ?? _fallbackLabel),
				Weight = weight
			});
			_totalWeight += weight;
		}
		WaylithJobRuntime.PinHandle(handle);
		return handle;
	}

	public ProgressCounter Counter(int total)
	{
		ProgressCounter progressCounter = new ProgressCounter(total);
		Volatile.Write(ref _activeCounter, progressCounter);
		return progressCounter;
	}

	public void ClearCounter()
	{
		Volatile.Write(ref _activeCounter, null);
	}

	public Snapshot GetSnapshot()
	{
		using (_lock.EnterScope())
		{
			if (_milestones.Count == 0)
			{
				return new Snapshot(_fallbackLabel, 0f, null);
			}
			int num = 0;
			int num2 = 0;
			List<Milestone> milestones = _milestones;
			string label = milestones[milestones.Count - 1].Label;
			bool flag = false;
			foreach (Milestone milestone in _milestones)
			{
				if (WaylithJobRuntime.IsCompleted(milestone.Handle))
				{
					num += milestone.Weight;
				}
				else if (!flag)
				{
					flag = true;
					label = milestone.Label;
					num2 = milestone.Weight;
				}
			}
			string detail = null;
			ProgressCounter progressCounter = Volatile.Read(in _activeCounter);
			float value;
			if (flag && progressCounter != null && progressCounter.Total > 0)
			{
				float num3 = System.Math.Clamp((float)progressCounter.Done / (float)progressCounter.Total, 0f, 1f);
				value = ((float)num + (float)num2 * num3) / (float)_totalWeight;
				detail = $"{progressCounter.Done} / {progressCounter.Total}";
			}
			else
			{
				value = ((_totalWeight > 0) ? ((float)num / (float)_totalWeight) : 0f);
			}
			return new Snapshot(label, System.Math.Clamp(value, 0f, 1f), detail);
		}
	}

	public void Dispose()
	{
		using (_lock.EnterScope())
		{
			if (_disposed)
			{
				return;
			}
			_disposed = true;
			foreach (Milestone milestone in _milestones)
			{
				WaylithJobRuntime.UnpinHandle(milestone.Handle);
			}
		}
	}
}
