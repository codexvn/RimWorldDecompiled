using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;

namespace Waylith.Rendering;

public static class TextureResourceManager
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public readonly struct RetirementHold : IDisposable
	{
		public void Dispose()
		{
			Interlocked.Decrement(ref _retirementHolds);
		}
	}

	private static readonly Lock Gate = new Lock();

	private static readonly List<DeferredGpuTextureDisposal> DeferredDisposals = new List<DeferredGpuTextureDisposal>();

	private static readonly List<WeakReference<GpuTextureAllocation>> LiveAllocations = new List<WeakReference<GpuTextureAllocation>>();

	private static int _addsSinceCompact;

	private static long _lastSubmittedFrameNumber;

	private static long _pendingBytes;

	private static int _pendingCount;

	private static long _pendingMovedAwayDeviceBytes;

	private static int _retirementHolds;

	public static long PendingDisposalBytes => Volatile.Read(in _pendingBytes);

	public static int PendingDisposalCount => Volatile.Read(in _pendingCount);

	internal static long PendingMovedAwayDeviceBytes => Volatile.Read(in _pendingMovedAwayDeviceBytes);

	public static RetirementHold HoldRetirement()
	{
		Interlocked.Increment(ref _retirementHolds);
		return default(RetirementHold);
	}

	internal static void Register(GpuTextureAllocation allocation)
	{
		using (Gate.EnterScope())
		{
			LiveAllocations.Add(new WeakReference<GpuTextureAllocation>(allocation));
			if (++_addsSinceCompact >= 1024)
			{
				CompactLiveLocked();
				_addsSinceCompact = 0;
			}
		}
	}

	public static void DeferDispose(GpuTextureAllocation allocation)
	{
		DeferDispose(allocation, movedAway: false, 0L);
	}

	internal static void DeferDispose(GpuTextureAllocation allocation, bool movedAway, long minForcedRetireFrame)
	{
		using (Gate.EnterScope())
		{
			long allocatedBytes = allocation.AllocatedBytes;
			DeferredDisposals.Add(new DeferredGpuTextureDisposal(allocation, _lastSubmittedFrameNumber + 4, allocatedBytes, movedAway, minForcedRetireFrame));
			Volatile.Write(ref _pendingBytes, _pendingBytes + allocatedBytes);
			Volatile.Write(ref _pendingCount, _pendingCount + 1);
			if (movedAway && !allocation.IsHostResident)
			{
				Volatile.Write(ref _pendingMovedAwayDeviceBytes, _pendingMovedAwayDeviceBytes + allocatedBytes);
			}
		}
	}

	public static void ProcessDeferredDisposals(long currentFrameNumber)
	{
		_lastSubmittedFrameNumber = currentFrameNumber;
		using (Gate.EnterScope())
		{
			if (Volatile.Read(in _retirementHolds) > 0)
			{
				return;
			}
			for (int num = DeferredDisposals.Count - 1; num >= 0; num--)
			{
				if (currentFrameNumber >= DeferredDisposals[num].RetireFrame)
				{
					RetireLocked(num);
				}
			}
		}
	}

	private static void RetireLocked(int index)
	{
		DeferredGpuTextureDisposal deferredGpuTextureDisposal = DeferredDisposals[index];
		if (deferredGpuTextureDisposal.Allocation.TryDetachForDisposal(out var disposal))
		{
			disposal.DisposeNow();
		}
		DeferredDisposals.RemoveAt(index);
		Volatile.Write(ref _pendingBytes, _pendingBytes - deferredGpuTextureDisposal.Bytes);
		Volatile.Write(ref _pendingCount, _pendingCount - 1);
		if (deferredGpuTextureDisposal.MovedAway && !deferredGpuTextureDisposal.Allocation.IsHostResident)
		{
			Volatile.Write(ref _pendingMovedAwayDeviceBytes, _pendingMovedAwayDeviceBytes - deferredGpuTextureDisposal.Bytes);
		}
	}

	public static void RetireAllNow(long movedAwayRetireFrame)
	{
		using (Gate.EnterScope())
		{
			if (Volatile.Read(in _retirementHolds) > 0)
			{
				return;
			}
			for (int num = DeferredDisposals.Count - 1; num >= 0; num--)
			{
				DeferredGpuTextureDisposal deferredGpuTextureDisposal = DeferredDisposals[num];
				if (!deferredGpuTextureDisposal.MovedAway || deferredGpuTextureDisposal.MinForcedRetireFrame <= movedAwayRetireFrame)
				{
					RetireLocked(num);
				}
			}
		}
	}

	public static void DisposeAllImmediate()
	{
		using (Gate.EnterScope())
		{
			foreach (WeakReference<GpuTextureAllocation> liveAllocation in LiveAllocations)
			{
				if (liveAllocation.TryGetTarget(out var target) && target.TryDetachForDisposal(out var disposal))
				{
					disposal.DisposeNow();
				}
			}
			LiveAllocations.Clear();
			_addsSinceCompact = 0;
			for (int i = 0; i < DeferredDisposals.Count; i++)
			{
				if (DeferredDisposals[i].Allocation.TryDetachForDisposal(out var disposal2))
				{
					disposal2.DisposeNow();
				}
			}
			DeferredDisposals.Clear();
			Volatile.Write(ref _pendingBytes, 0L);
			Volatile.Write(ref _pendingMovedAwayDeviceBytes, 0L);
		}
	}

	private static void CompactLiveLocked()
	{
		int num = 0;
		for (int i = 0; i < LiveAllocations.Count; i++)
		{
			if (LiveAllocations[i].TryGetTarget(out GpuTextureAllocation _))
			{
				LiveAllocations[num++] = LiveAllocations[i];
			}
		}
		if (num < LiveAllocations.Count)
		{
			LiveAllocations.RemoveRange(num, LiveAllocations.Count - num);
		}
	}
}
