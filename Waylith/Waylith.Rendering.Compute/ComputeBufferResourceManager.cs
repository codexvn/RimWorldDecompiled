using System.Collections.Generic;
using System.Threading;

namespace Waylith.Rendering.Compute;

public static class ComputeBufferResourceManager
{
	private static long _deferredFrames = 6L;

	private static readonly Lock Gate = new Lock();

	private static readonly List<DeferredComputeBufferDisposal> DeferredDisposals = new List<DeferredComputeBufferDisposal>();

	private static long _frameSerial;

	public static void Configure(int framesInFlight)
	{
		using (Gate.EnterScope())
		{
			_deferredFrames = framesInFlight * 2 + 2;
		}
	}

	internal static void DeferDispose(GpuComputeBuffer buffer)
	{
		using (Gate.EnterScope())
		{
			DeferredDisposals.Add(new DeferredComputeBufferDisposal(buffer, _frameSerial + _deferredFrames));
		}
	}

	public static void OnRenderFrameStarting()
	{
		using (Gate.EnterScope())
		{
			_frameSerial++;
			for (int num = DeferredDisposals.Count - 1; num >= 0; num--)
			{
				if (_frameSerial >= DeferredDisposals[num].RetireFrame)
				{
					DeferredDisposals[num].Buffer.FreeAllocationNow();
					DeferredDisposals.RemoveAt(num);
				}
			}
		}
	}

	public static void DisposeAllImmediate()
	{
		using (Gate.EnterScope())
		{
			for (int i = 0; i < DeferredDisposals.Count; i++)
			{
				DeferredDisposals[i].Buffer.FreeAllocationNow();
			}
			DeferredDisposals.Clear();
		}
	}
}
