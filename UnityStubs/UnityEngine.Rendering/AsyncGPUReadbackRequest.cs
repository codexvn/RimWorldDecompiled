using System;
using System.Runtime.InteropServices;
using Unity.Collections;
using UnityStubs.Rendering;

namespace UnityEngine.Rendering;

public struct AsyncGPUReadbackRequest
{
	private byte[] _rgbaBottomUp;

	private int _width;

	private int _height;

	private bool _error;

	public bool done => true;

	public bool hasError => _error;

	public int width => _width;

	public int height => _height;

	public int depth => 1;

	public int layerCount => 1;

	public int layerDataSize
	{
		get
		{
			byte[] rgbaBottomUp = _rgbaBottomUp;
			if (rgbaBottomUp == null)
			{
				return 0;
			}
			return rgbaBottomUp.Length;
		}
	}

	public void Update()
	{
	}

	public void WaitForCompletion()
	{
	}

	public NativeArray<T> GetData<T>(int layer = 0) where T : struct
	{
		if (_error || _rgbaBottomUp == null)
		{
			throw new InvalidOperationException("Cannot access the data of an unsuccessful request.");
		}
		return new NativeArray<T>(MemoryMarshal.Cast<byte, T>((ReadOnlySpan<byte>)_rgbaBottomUp).ToArray(), Allocator.Persistent);
	}

	internal static AsyncGPUReadbackRequest ReadNow(ComputeBuffer src, int size, int offset)
	{
		AsyncGPUReadbackRequest result;
		if (src == null)
		{
			result = default(AsyncGPUReadbackRequest);
			result._error = true;
			return result;
		}
		ReadOnlySpan<byte> currentBytes = src.CurrentBytes;
		int val = ((size <= 0) ? (src.count * src.stride) : size);
		byte[] array = new byte[Math.Max(0, val)];
		int num = Math.Clamp(currentBytes.Length - offset, 0, array.Length);
		if (num > 0)
		{
			currentBytes.Slice(offset, num).CopyTo(array);
		}
		result = default(AsyncGPUReadbackRequest);
		result._rgbaBottomUp = array;
		result._width = array.Length;
		result._height = 1;
		return result;
	}

	internal static AsyncGPUReadbackRequest ReadNow(Texture src, int mipIndex)
	{
		byte[] rgbaTopDown = null;
		int num = 0;
		int num2 = 0;
		if (!(src is RenderTexture renderTexture))
		{
			if (src is Texture2D { isReadable: not false } texture2D)
			{
				num = Math.Max(1, texture2D.width >> mipIndex);
				num2 = Math.Max(1, texture2D.height >> mipIndex);
				rgbaTopDown = MemoryMarshal.AsBytes(texture2D.GetPixels32(mipIndex).AsSpan()).ToArray();
			}
		}
		else
		{
			ICameraTextureRenderer current = CameraTextureRenderer.Current;
			if (current != null && current.TryReadPixels(renderTexture, out rgbaTopDown))
			{
				num = renderTexture.width;
				num2 = renderTexture.height;
			}
		}
		AsyncGPUReadbackRequest result;
		if (rgbaTopDown == null)
		{
			result = default(AsyncGPUReadbackRequest);
			result._error = true;
			return result;
		}
		int num3 = num * 4;
		byte[] array = new byte[rgbaTopDown.Length];
		for (int i = 0; i < num2; i++)
		{
			Buffer.BlockCopy(rgbaTopDown, i * num3, array, (num2 - 1 - i) * num3, num3);
		}
		result = default(AsyncGPUReadbackRequest);
		result._rgbaBottomUp = array;
		result._width = num;
		result._height = num2;
		return result;
	}
}
