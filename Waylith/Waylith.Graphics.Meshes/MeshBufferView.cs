using System;

namespace Waylith.Graphics.Meshes;

public readonly struct MeshBufferView<T> where T : unmanaged
{
	private unsafe readonly T* _pointer;

	private readonly int _length;

	public int Length => _length;

	public unsafe ref T this[int index]
	{
		get
		{
			if ((uint)index >= (uint)_length)
			{
				throw new IndexOutOfRangeException($"Index {index} out of range for mesh buffer view of length {_length}.");
			}
			return ref _pointer[index];
		}
	}

	public unsafe MeshBufferView(nint pointer, int length)
	{
		_pointer = (T*)pointer;
		_length = length;
	}

	public unsafe Span<T> AsSpan()
	{
		if (_pointer != null)
		{
			return new Span<T>(_pointer, _length);
		}
		return Span<T>.Empty;
	}

	public Span<T> AsSpan(int start, int length)
	{
		return AsSpan().Slice(start, length);
	}

	public unsafe MeshBufferView<TTo> Reinterpret<TTo>() where TTo : unmanaged
	{
		long num = (long)_length * (long)sizeof(T);
		if (num % sizeof(TTo) != 0L)
		{
			throw new InvalidOperationException($"Cannot reinterpret buffer of {num} bytes as {typeof(TTo).Name} (element size {sizeof(TTo)}).");
		}
		return new MeshBufferView<TTo>((nint)_pointer, (int)(num / sizeof(TTo)));
	}
}
