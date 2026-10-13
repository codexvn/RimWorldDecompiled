using System;
using System.Runtime.CompilerServices;
using System.Text;

namespace Waylith;

public ref struct Utf8SpanBuilder
{
	private readonly Span<byte> _buffer;

	private int _pos;

	public readonly ReadOnlySpan<byte> Written
	{
		get
		{
			Span<byte> buffer = _buffer;
			return buffer.Slice(0, _pos);
		}
	}

	public readonly int Length => _pos;

	public Utf8SpanBuilder(Span<byte> buffer)
	{
		_buffer = buffer;
		_pos = 0;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void Clear()
	{
		_pos = 0;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void Append(byte value)
	{
		if (_pos < _buffer.Length)
		{
			_buffer[_pos++] = value;
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void Append(ReadOnlySpan<byte> utf8)
	{
		int num = System.Math.Min(utf8.Length, _buffer.Length - _pos);
		ReadOnlySpan<byte> readOnlySpan = utf8.Slice(0, num);
		Span<byte> buffer = _buffer;
		readOnlySpan.CopyTo(buffer.Slice(_pos));
		_pos += num;
	}

	public void Append(string? value)
	{
		if (!string.IsNullOrEmpty(value))
		{
			Append(value.AsSpan());
		}
	}

	public void Append(ReadOnlySpan<char> value)
	{
		SpanRuneEnumerator enumerator = value.EnumerateRunes().GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				Rune current = enumerator.Current;
				Span<byte> buffer = _buffer;
				if (!current.TryEncodeToUtf8(buffer.Slice(_pos), out var bytesWritten))
				{
					break;
				}
				_pos += bytesWritten;
			}
		}
		finally
		{
			((IDisposable)enumerator/*cast due to .constrained prefix*/).Dispose();
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void Append(int value)
	{
		AppendFormattable(value);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void Append(uint value)
	{
		AppendFormattable(value);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void Append(long value)
	{
		AppendFormattable(value);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void Append(ulong value)
	{
		AppendFormattable(value);
	}

	public void AppendEnum<TEnum>(TEnum value) where TEnum : struct, Enum
	{
		string name = Enum.GetName(value);
		if (name != null)
		{
			Append(name);
		}
		else
		{
			Append(Convert.ToUInt64(value));
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void AppendFormattable<T>(T value) where T : IUtf8SpanFormattable
	{
		ref T reference = ref value;
		T val = default(T);
		if (val == null)
		{
			val = reference;
			reference = ref val;
		}
		Span<byte> buffer = _buffer;
		if (reference.TryFormat(buffer.Slice(_pos), out var bytesWritten, default(ReadOnlySpan<char>), null))
		{
			_pos += bytesWritten;
		}
	}
}
