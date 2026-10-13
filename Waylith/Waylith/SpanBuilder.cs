using System;
using System.Runtime.CompilerServices;

namespace Waylith;

public ref struct SpanBuilder
{
	private readonly Span<char> _buffer;

	private int _pos;

	public readonly Span<char> Written
	{
		get
		{
			Span<char> buffer = _buffer;
			return buffer.Slice(0, _pos);
		}
	}

	public readonly int Length => _pos;

	public SpanBuilder(Span<char> buffer)
	{
		_buffer = buffer;
		_pos = 0;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void Append(ReadOnlySpan<char> s)
	{
		int num = System.Math.Min(s.Length, _buffer.Length - _pos);
		ReadOnlySpan<char> readOnlySpan = s.Slice(0, num);
		Span<char> buffer = _buffer;
		readOnlySpan.CopyTo(buffer.Slice(_pos));
		_pos += num;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void Append(char c)
	{
		if (_pos < _buffer.Length)
		{
			_buffer[_pos++] = c;
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void Append<T>(T value, ReadOnlySpan<char> format = default(ReadOnlySpan<char>)) where T : ISpanFormattable
	{
		ref T reference = ref value;
		T val = default(T);
		if (val == null)
		{
			val = reference;
			reference = ref val;
		}
		Span<char> buffer = _buffer;
		if (reference.TryFormat(buffer.Slice(_pos), out var charsWritten, format, null))
		{
			_pos += charsWritten;
		}
	}

	public static implicit operator ReadOnlySpan<char>(SpanBuilder sb)
	{
		return sb.Written;
	}

	public void Clear()
	{
		_pos = 0;
		_buffer.Clear();
	}
}
