using System;
using System.Runtime.InteropServices;
using Waylith.Runtime;

namespace Waylith.Graphics.Meshes;

public sealed class NativeByteBuffer : IDisposable
{
	private nint _buffer;

	private int _lengthBytes;

	private int _capacityBytes;

	private readonly int _alignment;

	private readonly AllocationKind _allocationKind;

	private bool _disposed;

	public int LengthBytes => _lengthBytes;

	public int CapacityBytes => _capacityBytes;

	public bool IsCreated => _buffer != IntPtr.Zero;

	public nint Pointer
	{
		get
		{
			EnsureNotDisposed();
			return _buffer;
		}
	}

	public NativeByteBuffer(int alignment = 16)
		: this(AllocationKind.Persistent, alignment)
	{
	}

	public NativeByteBuffer(AllocationKind allocationKind, int alignment = 16)
	{
		if ((uint)allocationKind <= 1u)
		{
			throw new ArgumentOutOfRangeException("allocationKind", $"Allocator '{allocationKind}' is not valid for long-lived raw buffers.");
		}
		_allocationKind = allocationKind;
		_alignment = System.Math.Max(1, alignment);
	}

	public unsafe void Resize(int newLengthBytes, bool clearNewBytes = false)
	{
		EnsureNotDisposed();
		newLengthBytes = System.Math.Max(0, newLengthBytes);
		int lengthBytes = _lengthBytes;
		EnsureCapacity(newLengthBytes);
		_lengthBytes = newLengthBytes;
		if (clearNewBytes && newLengthBytes > lengthBytes && _buffer != IntPtr.Zero)
		{
			new Span<byte>((void*)(_buffer + lengthBytes), newLengthBytes - lengthBytes).Clear();
		}
	}

	public unsafe void EnsureCapacity(int requiredBytes)
	{
		EnsureNotDisposed();
		requiredBytes = System.Math.Max(0, requiredBytes);
		if (requiredBytes > _capacityBytes)
		{
			int num = System.Math.Max(requiredBytes, System.Math.Max(64, (_capacityBytes == 0) ? 64 : (_capacityBytes * 2)));
			nint num2 = NativeAllocationRuntime.Allocate(num, _alignment, _allocationKind);
			if (_buffer != IntPtr.Zero && _lengthBytes > 0)
			{
				Buffer.MemoryCopy((void*)_buffer, (void*)num2, num, _lengthBytes);
				NativeAllocationRuntime.Free(_buffer, _allocationKind);
			}
			_buffer = num2;
			_capacityBytes = num;
		}
	}

	public unsafe Span<byte> AsSpan()
	{
		EnsureNotDisposed();
		if (_buffer != IntPtr.Zero)
		{
			return new Span<byte>((void*)_buffer, _lengthBytes);
		}
		return Span<byte>.Empty;
	}

	public unsafe Span<T> AsSpan<T>() where T : unmanaged
	{
		Span<byte> span = AsSpan();
		if (span.Length % sizeof(T) != 0)
		{
			throw new InvalidOperationException($"Buffer length {span.Length} is not divisible by {sizeof(T)}.");
		}
		return MemoryMarshal.Cast<byte, T>(span);
	}

	public void CopyFrom(NativeByteBuffer other)
	{
		ArgumentNullException.ThrowIfNull(other, "other");
		Resize(other.LengthBytes);
		if (_lengthBytes > 0)
		{
			other.AsSpan().CopyTo(AsSpan());
		}
	}

	public void Dispose()
	{
		if (!_disposed)
		{
			if (_buffer != IntPtr.Zero)
			{
				NativeAllocationRuntime.Free(_buffer, _allocationKind);
				_buffer = IntPtr.Zero;
			}
			_lengthBytes = 0;
			_capacityBytes = 0;
			_disposed = true;
		}
	}

	private void EnsureNotDisposed()
	{
		ObjectDisposedException.ThrowIf(_disposed, "NativeByteBuffer");
	}
}
