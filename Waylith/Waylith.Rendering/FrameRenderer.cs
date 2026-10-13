#define DEV_OR_TESTER
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Brutal.Collections;
using Brutal.Numerics;
using Brutal.VulkanApi;
using Brutal.VulkanApi.Abstractions;
using Waylith.Graphics;
using Waylith.Profiling;

namespace Waylith.Rendering;

public sealed class FrameRenderer : IDisposable
{
	private IRenderFeature[] _features = Array.Empty<IRenderFeature>();

	private readonly Lock _featuresLock = new Lock();

	private IRenderFeature[] _mainThreadFeatures = Array.Empty<IRenderFeature>();

	private readonly List<IRenderFeature[]> _liveFrameFeatures = new List<IRenderFeature[]>();

	private readonly List<IRenderFeature> _pendingDisposals = new List<IRenderFeature>();

	private readonly HashSet<IRenderFeature> _rebuiltFeatures = new HashSet<IRenderFeature>();

	private IRenderFeature[]? _lastRecordedFeatures;

	private static readonly ConcurrentDictionary<Type, string> _featureLabels = new ConcurrentDictionary<Type, string>();

	private Action? _nextFrameEvents;

	public RenderToMainThreadDispatcher RenderToMainThread { get; } = new RenderToMainThreadDispatcher();

	public IReadOnlyList<IRenderFeature> Features => Volatile.Read(in _features);

	public event Action? RenderThreadFrameStarting;

	public event Action<RenderFrame>? RenderThreadFrameEnded;

	public int GetFeatureCount()
	{
		return Volatile.Read(in _features).Length;
	}

	public IRenderFeature GetFeatureAt(int index)
	{
		return Volatile.Read(in _features)[index];
	}

	public void AddFeature(IRenderFeature feature)
	{
		using (_featuresLock.EnterScope())
		{
			InsertFeatureLocked(_features.Length, feature);
		}
	}

	public void InsertFeature(int index, IRenderFeature feature)
	{
		using (_featuresLock.EnterScope())
		{
			InsertFeatureLocked(index, feature);
		}
	}

	private void InsertFeatureLocked(int index, IRenderFeature feature)
	{
		ArgumentNullException.ThrowIfNull(feature, "feature");
		IRenderFeature[] features = _features;
		ArgumentOutOfRangeException.ThrowIfNegative(index, "index");
		ArgumentOutOfRangeException.ThrowIfGreaterThan(index, features.Length, "index");
		if (Array.IndexOf(features, feature) >= 0)
		{
			throw new InvalidOperationException(feature.GetType().Name + " is already registered with the frame renderer.");
		}
		ref IRenderFeature[] features2 = ref _features;
		Span<IRenderFeature> span = features.AsSpan(0, index);
		Span<IRenderFeature> span2 = features.AsSpan(index);
		int num = 0;
		IRenderFeature[] array = new IRenderFeature[1 + (span.Length + span2.Length)];
		span.CopyTo(new Span<IRenderFeature>(array).Slice(num, span.Length));
		num += span.Length;
		array[num] = feature;
		num++;
		span2.CopyTo(new Span<IRenderFeature>(array).Slice(num, span2.Length));
		num += span2.Length;
		Volatile.Write(ref features2, array);
	}

	public bool RemoveFeature(IRenderFeature feature, bool dispose = true)
	{
		using (_featuresLock.EnterScope())
		{
			IRenderFeature[] features = _features;
			int num = Array.IndexOf(features, feature);
			if (num < 0)
			{
				return false;
			}
			ref IRenderFeature[] features2 = ref _features;
			Span<IRenderFeature> span = features.AsSpan(0, num);
			Span<IRenderFeature> span2 = features.AsSpan(num + 1);
			int num2 = 0;
			IRenderFeature[] array = new IRenderFeature[span.Length + span2.Length];
			span.CopyTo(new Span<IRenderFeature>(array).Slice(num2, span.Length));
			num2 += span.Length;
			span2.CopyTo(new Span<IRenderFeature>(array).Slice(num2, span2.Length));
			num2 += span2.Length;
			Volatile.Write(ref features2, array);
			if (dispose && feature is IDisposable)
			{
				_pendingDisposals.Add(feature);
			}
			return true;
		}
	}

	public void AddNextFrameCallback(Action callback)
	{
		Action nextFrameEvents;
		Action value;
		do
		{
			nextFrameEvents = _nextFrameEvents;
			value = (Action)Delegate.Combine(nextFrameEvents, callback);
		}
		while ((Delegate?)Interlocked.CompareExchange(ref _nextFrameEvents, value, nextFrameEvents) != (Delegate?)nextFrameEvents);
	}

	public void DrainRenderThreadCallbacksOnMainThread()
	{
		RenderToMainThread.Invoke();
	}

	internal void NotifyRenderThreadFrameStarting()
	{
		this.RenderThreadFrameStarting?.Invoke();
	}

	internal void NotifyRenderThreadFrameEnded(RenderFrame frame)
	{
		this.RenderThreadFrameEnded?.Invoke(frame);
	}

	public void NotifyRenderTargetRebuilt(IRenderTarget renderTarget)
	{
		IRenderFeature[] array = Volatile.Read(in _features);
		_rebuiltFeatures.Clear();
		for (int i = 0; i < array.Length; i++)
		{
			array[i].OnRenderTargetRebuilt(renderTarget);
			_rebuiltFeatures.Add(array[i]);
		}
		_lastRecordedFeatures = null;
	}

	private void CatchUpRenderTargetRebuilt(IRenderFeature[] features, IRenderTarget renderTarget)
	{
		for (int i = 0; i < features.Length; i++)
		{
			if (_rebuiltFeatures.Add(features[i]))
			{
				features[i].OnRenderTargetRebuilt(renderTarget);
			}
		}
		IRenderFeature[] live = Volatile.Read(in _features);
		_rebuiltFeatures.RemoveWhere((IRenderFeature f) => Array.IndexOf(features, f) < 0 && Array.IndexOf(live, f) < 0);
	}

	public RenderFrame CaptureFrame(in FrameTiming timing, NativeList<VkClearValue> clearValues, bool requestRenderContextRebuild)
	{
		IRenderFeature[] mainThreadFeatures = _mainThreadFeatures;
		RenderFrame renderFrame = RenderFrame.Rent(this, in timing, mainThreadFeatures, clearValues, requestRenderContextRebuild);
		using (_featuresLock.EnterScope())
		{
			_liveFrameFeatures.Add(mainThreadFeatures);
		}
		try
		{
			for (int i = 0; i < mainThreadFeatures.Length; i++)
			{
				IRenderFeature renderFeature = mainThreadFeatures[i];
				using (FrameProfiler.Scope(GetFeatureLabel(renderFeature)))
				{
					renderFrame.SetFeaturePayload(i, renderFeature.CaptureFrame(in timing));
				}
			}
			for (int j = 0; j < mainThreadFeatures.Length; j++)
			{
				renderFrame.SetFeatureActive(j, mainThreadFeatures[j].IsActive);
			}
			return renderFrame;
		}
		catch
		{
			renderFrame.Dispose();
			throw;
		}
	}

	internal void OnFrameDisposed(IRenderFeature[] features)
	{
		using (_featuresLock.EnterScope())
		{
			_liveFrameFeatures.Remove(features);
		}
	}

	private static string GetFeatureLabel(IRenderFeature feature)
	{
		return _featureLabels.GetOrAdd(feature.GetType(), (Type type) => type.Name);
	}

	public void BeginMainThreadFrame(in FrameTiming timing)
	{
		IRenderFeature[] array = (_mainThreadFeatures = Volatile.Read(in _features));
		DisposeRetiredFeatures();
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i] is IEarlyMainThreadRenderFeature earlyMainThreadRenderFeature)
			{
				earlyMainThreadRenderFeature.BeginMainThreadFrame(in timing);
			}
		}
	}

	private void DisposeRetiredFeatures()
	{
		List<IDisposable> list = null;
		using (_featuresLock.EnterScope())
		{
			for (int num = _pendingDisposals.Count - 1; num >= 0; num--)
			{
				IRenderFeature renderFeature = _pendingDisposals[num];
				if (Array.IndexOf(_features, renderFeature) >= 0)
				{
					_pendingDisposals.RemoveAt(num);
				}
				else if (!IsFeatureInFlight(renderFeature))
				{
					_pendingDisposals.RemoveAt(num);
					(list ?? (list = new List<IDisposable>())).Add((IDisposable)renderFeature);
				}
			}
		}
		if (list != null)
		{
			for (int i = 0; i < list.Count; i++)
			{
				list[i].Dispose();
			}
		}
	}

	private bool IsFeatureInFlight(IRenderFeature feature)
	{
		for (int i = 0; i < _liveFrameFeatures.Count; i++)
		{
			if (Array.IndexOf(_liveFrameFeatures[i], feature) >= 0)
			{
				return true;
			}
		}
		return false;
	}

	public unsafe void RecordFrame(in FrameContext frameContext, RenderFrame frame)
	{
		Interlocked.Exchange(ref _nextFrameEvents, null)?.Invoke();
		IRenderFeature[] features = frame.Features;
		if (features != _lastRecordedFeatures)
		{
			CatchUpRenderTargetRebuilt(features, frameContext.Target);
			_lastRecordedFeatures = features;
		}
		for (int i = 0; i < features.Length; i++)
		{
			IRenderFeature renderFeature = features[i];
			if (!frame.GetFeatureActive(i))
			{
				continue;
			}
			object featurePayload = frame.GetFeaturePayload(i);
			IReadOnlyList<IRenderPass> passes = renderFeature.Passes;
			for (int j = 0; j < passes.Count; j++)
			{
				IRenderPass renderPass = passes[j];
				if (renderPass.PreBackbuffer)
				{
					FrameContext ctx = ((renderPass.Attachment != null) ? renderPass.Attachment.MakeFrameContext(in frameContext) : frameContext);
					BeginPassDebugLabel(frameContext.CommandBuffer, renderFeature, renderPass);
					using (FrameProfiler.Scope(renderPass.GetType().Name))
					{
						renderPass.Record(in ctx, featurePayload);
					}
					EndPassDebugLabel(frameContext.CommandBuffer);
				}
			}
		}
		CommandBuffer commandBuffer = frameContext.CommandBuffer;
		fixed (VkClearValue* clearValues = frame.ClearValues)
		{
			VkRenderPassBeginInfo pRenderPassBegin = new VkRenderPassBeginInfo
			{
				RenderPass = frameContext.RenderPass,
				Framebuffer = frameContext.Framebuffer,
				RenderArea = new VkRect2D(frameContext.Extent),
				ClearValues = clearValues,
				ClearValueCount = frame.ClearValues.Length
			};
			commandBuffer.BeginRenderPass(in pRenderPassBegin, VkSubpassContents.Inline);
		}
		for (int k = 0; k < features.Length; k++)
		{
			IRenderFeature renderFeature2 = features[k];
			if (!frame.GetFeatureActive(k))
			{
				continue;
			}
			object featurePayload2 = frame.GetFeaturePayload(k);
			IReadOnlyList<IRenderPass> passes2 = renderFeature2.Passes;
			for (int l = 0; l < passes2.Count; l++)
			{
				IRenderPass renderPass2 = passes2[l];
				if (!renderPass2.PreBackbuffer && !(renderPass2 is IPostBackbufferPass))
				{
					BeginPassDebugLabel(frameContext.CommandBuffer, renderFeature2, renderPass2);
					using (FrameProfiler.Scope(renderPass2.GetType().Name))
					{
						renderPass2.Record(in frameContext, featurePayload2);
					}
					EndPassDebugLabel(frameContext.CommandBuffer);
				}
			}
		}
		commandBuffer.EndRenderPass();
		for (int m = 0; m < features.Length; m++)
		{
			IRenderFeature renderFeature3 = features[m];
			if (!frame.GetFeatureActive(m))
			{
				continue;
			}
			object featurePayload3 = frame.GetFeaturePayload(m);
			IReadOnlyList<IRenderPass> passes3 = renderFeature3.Passes;
			for (int n = 0; n < passes3.Count; n++)
			{
				IRenderPass renderPass3 = passes3[n];
				if (renderPass3 is IPostBackbufferPass)
				{
					BeginPassDebugLabel(frameContext.CommandBuffer, renderFeature3, renderPass3);
					using (FrameProfiler.Scope(renderPass3.GetType().Name))
					{
						renderPass3.Record(in frameContext, featurePayload3);
					}
					EndPassDebugLabel(frameContext.CommandBuffer);
				}
			}
		}
	}

	[Conditional("DEV_OR_TESTER")]
	private static void BeginPassDebugLabel(CommandBuffer commandBuffer, IRenderFeature feature, IRenderPass pass)
	{
		Span<byte> buffer = stackalloc byte[160];
		Utf8SpanBuilder utf8SpanBuilder = new Utf8SpanBuilder(buffer);
		Type type = feature.GetType();
		Type type2 = pass.GetType();
		utf8SpanBuilder.Append(type.Name);
		if (type2 != type)
		{
			utf8SpanBuilder.Append((byte)46);
			utf8SpanBuilder.Append(type2.Name);
		}
		utf8SpanBuilder.Append((byte)0);
		commandBuffer.BeginDebugLabel(utf8SpanBuilder.Written, new float4(0.95f, 0.6f, 0.15f, 1f));
	}

	[Conditional("DEV_OR_TESTER")]
	private static void EndPassDebugLabel(CommandBuffer commandBuffer)
	{
		commandBuffer.EndDebugLabel();
	}

	public void Dispose()
	{
		IRenderFeature[] features;
		IRenderFeature[] array;
		using (_featuresLock.EnterScope())
		{
			features = _features;
			array = _pendingDisposals.ToArray();
			_features = Array.Empty<IRenderFeature>();
			_pendingDisposals.Clear();
		}
		_mainThreadFeatures = Array.Empty<IRenderFeature>();
		for (int i = 0; i < features.Length; i++)
		{
			if (features[i] is IDisposable disposable)
			{
				disposable.Dispose();
			}
		}
		for (int j = 0; j < array.Length; j++)
		{
			((IDisposable)array[j]).Dispose();
		}
	}
}
