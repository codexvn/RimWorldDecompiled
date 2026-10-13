using System;

namespace UnityEngine.Rendering;

public static class AsyncGPUReadback
{
	public static AsyncGPUReadbackRequest Request(Texture src, int mipIndex = 0, Action<AsyncGPUReadbackRequest> callback = null)
	{
		AsyncGPUReadbackRequest asyncGPUReadbackRequest = AsyncGPUReadbackRequest.ReadNow(src, mipIndex);
		callback?.Invoke(asyncGPUReadbackRequest);
		return asyncGPUReadbackRequest;
	}

	public static AsyncGPUReadbackRequest Request(Texture src, int mipIndex, TextureFormat dstFormat, Action<AsyncGPUReadbackRequest> callback = null)
	{
		return Request(src, mipIndex, callback);
	}

	public static AsyncGPUReadbackRequest Request(ComputeBuffer src, Action<AsyncGPUReadbackRequest> callback = null)
	{
		return Request(src, 0, 0, callback);
	}

	public static AsyncGPUReadbackRequest Request(ComputeBuffer src, int size, int offset, Action<AsyncGPUReadbackRequest> callback = null)
	{
		AsyncGPUReadbackRequest asyncGPUReadbackRequest = AsyncGPUReadbackRequest.ReadNow(src, size, offset);
		callback?.Invoke(asyncGPUReadbackRequest);
		return asyncGPUReadbackRequest;
	}

	public static void WaitAllRequests()
	{
	}
}
