namespace Unity.Collections.LowLevel.Unsafe;

public struct UnsafeBitArray
{
	private NativeBitArray _bits;

	public bool IsCreated => _bits.IsCreated;

	public int Length => _bits.Length;

	public UnsafeBitArray(int numBits, Allocator allocator)
	{
		_bits = new NativeBitArray(numBits, allocator);
	}

	public void Set(int pos, bool value)
	{
		_bits.Set(pos, value);
	}

	public bool IsSet(int pos)
	{
		return _bits.IsSet(pos);
	}

	public void Clear()
	{
		_bits.Clear();
	}

	public void Dispose()
	{
		_bits.Dispose();
	}
}
