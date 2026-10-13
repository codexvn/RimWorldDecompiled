using System;
using System.Diagnostics;
using UnityEngine;

namespace Unity.Collections.LowLevel.Unsafe;

public sealed class DisposeSentinel
{
	private readonly string _allocationStackTrace;

	private bool _isDisposed;

	private DisposeSentinel(int callSiteStackDepth)
	{
		_allocationStackTrace = new StackTrace(2 + callSiteStackDepth, fNeedFileInfo: true).ToString();
	}

	~DisposeSentinel()
	{
		if (!_isDisposed)
		{
			UnityEngine.Debug.LogError("A native collection has not been disposed, resulting in a memory leak. Allocated at:\n" + _allocationStackTrace);
		}
	}

	public static void Create(out AtomicSafetyHandle safety, out DisposeSentinel sentinel, int callSiteStackDepth, Allocator allocator)
	{
		safety = AtomicSafetyHandle.Create();
		sentinel = new DisposeSentinel(callSiteStackDepth);
	}

	public static void Dispose(ref AtomicSafetyHandle safety, ref DisposeSentinel sentinel)
	{
		AtomicSafetyHandle.Release(ref safety);
		sentinel._isDisposed = true;
		GC.SuppressFinalize(sentinel);
		sentinel = null;
	}
}
