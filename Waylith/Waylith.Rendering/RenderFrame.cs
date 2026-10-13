using System;
using System.Collections.Generic;
using System.Threading;
using Brutal.Collections;
using Brutal.VulkanApi;

namespace Waylith.Rendering;

public sealed class RenderFrame : IDisposable
{
	private static readonly Stack<RenderFrame> Pool = new Stack<RenderFrame>();

	private static readonly Lock PoolLock = new Lock();

	private object?[] _featurePayloads = Array.Empty<object>();

	private bool[] _featureActive = Array.Empty<bool>();

	private VkClearValue[] _clearValues = Array.Empty<VkClearValue>();

	private int _clearValueCount;

	private bool _pooled = true;

	private FrameRenderer? _owner;

	public FrameTiming Timing { get; private set; }

	internal IRenderFeature[] Features { get; private set; } = Array.Empty<IRenderFeature>();

	public Span<VkClearValue> ClearValues => _clearValues.AsSpan(0, _clearValueCount);

	public bool RequestRenderContextRebuild { get; private set; }

	private RenderFrame()
	{
	}

	internal static RenderFrame Rent(FrameRenderer owner, in FrameTiming timing, IRenderFeature[] features, NativeList<VkClearValue> clearValues, bool requestRenderContextRebuild)
	{
		RenderFrame renderFrame;
		using (PoolLock.EnterScope())
		{
			renderFrame = (Pool.TryPop(out RenderFrame result) ? result : new RenderFrame());
		}
		renderFrame._pooled = false;
		renderFrame._owner = owner;
		renderFrame.Timing = timing;
		renderFrame.RequestRenderContextRebuild = requestRenderContextRebuild;
		renderFrame.Features = features;
		if (renderFrame._featurePayloads.Length != features.Length)
		{
			renderFrame._featurePayloads = new object[features.Length];
			renderFrame._featureActive = new bool[features.Length];
		}
		if (renderFrame._clearValues.Length < (int)clearValues.Count)
		{
			renderFrame._clearValues = new VkClearValue[(int)clearValues.Count];
		}
		renderFrame._clearValueCount = clearValues.Count;
		clearValues.AsSpan().CopyTo(renderFrame._clearValues);
		return renderFrame;
	}

	internal void SetFeaturePayload(int index, object? payload)
	{
		_featurePayloads[index] = payload;
	}

	internal void SetFeatureActive(int index, bool active)
	{
		_featureActive[index] = active;
	}

	public object? GetFeaturePayload(int index)
	{
		if ((uint)index >= (uint)_featurePayloads.Length)
		{
			return null;
		}
		return _featurePayloads[index];
	}

	public bool GetFeatureActive(int index)
	{
		if ((uint)index < (uint)_featureActive.Length)
		{
			return _featureActive[index];
		}
		return false;
	}

	public void Dispose()
	{
		if (_pooled)
		{
			return;
		}
		_pooled = true;
		for (int i = 0; i < _featurePayloads.Length; i++)
		{
			if (_featurePayloads[i] is IDisposable disposable)
			{
				disposable.Dispose();
			}
			_featurePayloads[i] = null;
		}
		_owner?.OnFrameDisposed(Features);
		_owner = null;
		Features = Array.Empty<IRenderFeature>();
		using (PoolLock.EnterScope())
		{
			Pool.Push(this);
		}
	}
}
