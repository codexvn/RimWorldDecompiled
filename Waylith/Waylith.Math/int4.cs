using System;
using System.Globalization;
using System.Runtime.Intrinsics;

namespace Waylith.Math;

public struct int4 : IEquatable<int4>, IFormattable
{
	private Vector128<int> _value;

	public int x => _value.GetElement(0);

	public int y => _value.GetElement(1);

	public int z => _value.GetElement(2);

	public int w => _value.GetElement(3);

	public int this[int index]
	{
		get
		{
			if (index < 0 || index > 3)
			{
				throw new IndexOutOfRangeException($"Index {index} out of range for int4");
			}
			return _value.GetElement(index);
		}
		set
		{
			if (index < 0 || index > 3)
			{
				throw new IndexOutOfRangeException($"Index {index} out of range for int4");
			}
			_value = _value.WithElement(index, value);
		}
	}

	public bool Equals(int4 rhs)
	{
		return _value.Equals(rhs._value);
	}

	public string ToString(string format, IFormatProvider formatProvider)
	{
		return $"({x}, {y}, {z}, {w})";
	}

	public override string ToString()
	{
		return ToString(null, CultureInfo.InvariantCulture);
	}

	public int4(int x, int y, int z, int w)
	{
		_value = Vector128.Create(x, y, z, w);
	}
}
