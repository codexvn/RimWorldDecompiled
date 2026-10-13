using System;

namespace Waylith.Rendering;

public sealed class ComputeBuffer : IDisposable
{
	private readonly InstanceDataBuffer data = new InstanceDataBuffer();

	private int lastGeneration = -1;

	public int stride { get; }

	public int count { get; }

	public ComputeBufferType type { get; }

	internal InstanceDataBuffer Data => data;

	internal int LastGeneration => lastGeneration;

	public ComputeBuffer(int count, int stride)
	{
		this.count = count;
		this.stride = stride;
	}

	public ComputeBuffer(int count, int stride, ComputeBufferType type)
	{
		this.count = count;
		this.stride = stride;
		this.type = type;
	}

	public unsafe void SetData<T0>(T0[] source) where T0 : unmanaged
	{
		int num = sizeof(T0);
		fixed (T0* pointer = source)
		{
			ReadOnlySpan<byte> bytes = new ReadOnlySpan<byte>(pointer, source.Length * num);
			lastGeneration = data.Write(bytes, num, source.Length);
		}
	}

	public void Release()
	{
	}

	public void Dispose()
	{
		GC.SuppressFinalize(this);
	}
}
