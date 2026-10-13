using System;
using System.Collections.Concurrent;
using System.Runtime.ExceptionServices;
using System.Threading;
using Brutal.VulkanApi;
using Waylith.Graphics;
using Waylith.Profiling;

namespace Waylith.Rendering;

public sealed class RenderThread : IDisposable
{
	private readonly IRenderContext _renderContext;

	private readonly FrameRenderer _frameRenderer;

	private readonly BlockingCollection<RenderFrame> _frames;

	private readonly Thread _thread;

	private readonly object _stateSync = new object();

	private readonly ManualResetEventSlim _idle = new ManualResetEventSlim(initialState: true);

	private readonly RenderFrame?[] _submittedFrames;

	private ExceptionDispatchInfo? _failure;

	private VkRenderPass _lastRenderPass;

	private VkExtent2D _lastExtent;

	private int _pendingFrames;

	private int _resizeRequested;

	private int _surfaceRecreateRequested;

	private bool _started;

	private bool _stopped;

	private bool _disposed;

	private int _producerThreadId;

	private static RenderThread? _active;

	internal static bool IsProducerThread
	{
		get
		{
			RenderThread renderThread = Volatile.Read(in _active);
			if (renderThread != null)
			{
				return Environment.CurrentManagedThreadId == renderThread._producerThreadId;
			}
			return true;
		}
	}

	public RenderThread(IRenderContext renderContext, FrameRenderer frameRenderer)
	{
		_renderContext = renderContext;
		_frameRenderer = frameRenderer;
		_frames = new BlockingCollection<RenderFrame>(System.Math.Max(1, renderContext.FramesInFlight));
		_submittedFrames = new RenderFrame[System.Math.Max(1, renderContext.FramesInFlight)];
		_thread = new Thread(RenderLoop)
		{
			IsBackground = true,
			Name = "RenderThread"
		};
	}

	public void Start()
	{
		if (!_started)
		{
			_started = true;
			_producerThreadId = Environment.CurrentManagedThreadId;
			Volatile.Write(ref _active, this);
			_thread.Start();
		}
	}

	public void Enqueue(RenderFrame frame)
	{
		ThrowIfFaulted();
		MarkFramePending();
		try
		{
			_frames.Add(frame);
		}
		catch
		{
			MarkFrameComplete();
			frame.Dispose();
			throw;
		}
		ThrowIfFaulted();
	}

	public void Stop()
	{
		if (_started && !_stopped)
		{
			_stopped = true;
			Interlocked.CompareExchange(ref _active, null, this);
			_frames.CompleteAdding();
			_thread.Join();
			_renderContext.Device.WaitIdle();
			DisposeSubmittedFrames();
			ThrowIfFaulted();
		}
	}

	public void ThrowIfFaulted()
	{
		_failure?.Throw();
	}

	public void WaitUntilIdle()
	{
		_idle.Wait();
		ThrowIfFaulted();
	}

	internal static bool TryWaitUntilIdleFromProducer(TimeSpan timeout)
	{
		RenderThread renderThread = Volatile.Read(in _active);
		if (renderThread == null)
		{
			return true;
		}
		if (Environment.CurrentManagedThreadId != renderThread._producerThreadId)
		{
			return false;
		}
		return renderThread._idle.Wait(timeout);
	}

	public void MarkRenderTargetRebuilt()
	{
		if (!_idle.IsSet)
		{
			throw new InvalidOperationException("MarkRenderTargetRebuilt must be called while the render thread is idle. Call WaitUntilIdle first.");
		}
		DisposeSubmittedFrames();
		_lastRenderPass = _renderContext.GetRenderPass();
		_lastExtent = _renderContext.GetExtent();
	}

	public bool ConsumeResizeRequest()
	{
		return Interlocked.Exchange(ref _resizeRequested, 0) != 0;
	}

	public bool ConsumeSurfaceRecreateRequest()
	{
		return Interlocked.Exchange(ref _surfaceRecreateRequested, 0) != 0;
	}

	public void Dispose()
	{
		if (_disposed)
		{
			return;
		}
		_disposed = true;
		try
		{
			Stop();
		}
		finally
		{
			_frames.Dispose();
			_idle.Dispose();
		}
	}

	private void RenderLoop()
	{
		try
		{
			foreach (RenderFrame item in _frames.GetConsumingEnumerable())
			{
				bool flag = false;
				FrameProfiler.ScopeHandle scopeHandle = FrameProfiler.Scope("RenderFrame");
				try
				{
					flag = RenderFrame(item);
				}
				finally
				{
					scopeHandle.Dispose();
					_frameRenderer.NotifyRenderThreadFrameEnded(item);
					if (!flag)
					{
						item.Dispose();
					}
					MarkFrameComplete();
				}
			}
		}
		catch (Exception source)
		{
			_failure = ExceptionDispatchInfo.Capture(source);
			try
			{
				_frames.CompleteAdding();
			}
			catch
			{
			}
		}
	}

	private bool RenderFrame(RenderFrame packet)
	{
		if (packet.RequestRenderContextRebuild)
		{
			RequestResize();
			return false;
		}
		_renderContext.TryAcquireNextFrame().Deconstruct(out var result, out var value);
		FrameResult frameResult = result;
		AcquiredFrame acquiredFrame = value;
		int submissionFrameIndex = _renderContext.SubmissionFrameIndex;
		DisposeSubmittedFrame(submissionFrameIndex);
		switch (frameResult)
		{
		case FrameResult.NeedsResize:
			RequestResize();
			return false;
		case FrameResult.SurfaceLost:
			RequestSurfaceRecreate();
			return false;
		default:
			return false;
		case FrameResult.Success:
		{
			_frameRenderer.NotifyRenderThreadFrameStarting();
			value = acquiredFrame;
			var (resources, commandBuffer2) = (AcquiredFrame)(ref value);
			if (!_lastRenderPass.Equals(_renderContext.GetRenderPass()) || !_lastExtent.Equals(_renderContext.GetExtent()))
			{
				_frameRenderer.NotifyRenderTargetRebuilt(_renderContext);
				_lastRenderPass = _renderContext.GetRenderPass();
				_lastExtent = _renderContext.GetExtent();
			}
			commandBuffer2.Reset(VkCommandBufferResetFlags.None);
			commandBuffer2.Begin(new VkCommandBufferBeginInfo
			{
				Flags = VkCommandBufferUsageFlags.None
			});
			FrameContext frameContext = new FrameContext(_renderContext, resources, commandBuffer2);
			_frameRenderer.RecordFrame(in frameContext, packet);
			commandBuffer2.End();
			switch (_renderContext.TrySubmitFrame())
			{
			case FrameResult.NeedsResize:
				RequestResize();
				break;
			case FrameResult.SurfaceLost:
				RequestSurfaceRecreate();
				break;
			}
			RetainSubmittedFrame(submissionFrameIndex, packet);
			return true;
		}
		}
	}

	private void RequestResize()
	{
		Interlocked.Exchange(ref _resizeRequested, 1);
	}

	private void RequestSurfaceRecreate()
	{
		Interlocked.Exchange(ref _surfaceRecreateRequested, 1);
	}

	private void MarkFramePending()
	{
		lock (_stateSync)
		{
			_pendingFrames++;
			_idle.Reset();
		}
	}

	private void MarkFrameComplete()
	{
		lock (_stateSync)
		{
			if (_pendingFrames > 0)
			{
				_pendingFrames--;
			}
			if (_pendingFrames == 0)
			{
				_idle.Set();
			}
		}
	}

	private void RetainSubmittedFrame(int submissionFrameIndex, RenderFrame frame)
	{
		int submittedFrameIndex = GetSubmittedFrameIndex(submissionFrameIndex);
		_submittedFrames[submittedFrameIndex] = frame;
	}

	private void DisposeSubmittedFrame(int submissionFrameIndex)
	{
		int submittedFrameIndex = GetSubmittedFrameIndex(submissionFrameIndex);
		_submittedFrames[submittedFrameIndex]?.Dispose();
		_submittedFrames[submittedFrameIndex] = null;
	}

	private void DisposeSubmittedFrames()
	{
		for (int i = 0; i < _submittedFrames.Length; i++)
		{
			_submittedFrames[i]?.Dispose();
			_submittedFrames[i] = null;
		}
	}

	private int GetSubmittedFrameIndex(int submissionFrameIndex)
	{
		int num = submissionFrameIndex % _submittedFrames.Length;
		if (num >= 0)
		{
			return num;
		}
		return num + _submittedFrames.Length;
	}
}
