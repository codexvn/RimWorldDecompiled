using System;
using System.Globalization;

namespace Waylith.Math;

public readonly struct half : IEquatable<half>, IFormattable
{
	private readonly ushort _value;

	private half(ushort value)
	{
		_value = value;
	}

	public bool Equals(half rhs)
	{
		return _value == rhs._value;
	}

	public override bool Equals(object obj)
	{
		if (obj is half rhs)
		{
			return Equals(rhs);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return _value.GetHashCode();
	}

	public override string ToString()
	{
		return ((float)this).ToString(CultureInfo.InvariantCulture);
	}

	public string ToString(string format, IFormatProvider formatProvider)
	{
		return ((float)this).ToString(format, formatProvider);
	}

	public static explicit operator half(float v)
	{
		return new half(BitConverter.HalfToUInt16Bits((Half)v));
	}

	public static explicit operator float(half v)
	{
		return (float)BitConverter.UInt16BitsToHalf(v._value);
	}
}
