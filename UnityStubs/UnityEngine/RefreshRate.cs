using System;

namespace UnityEngine;

public struct RefreshRate : IEquatable<RefreshRate>, IComparable<RefreshRate>
{
	public uint numerator;

	public uint denominator;

	public double value
	{
		get
		{
			if (denominator != 0)
			{
				return (double)numerator / (double)denominator;
			}
			return 0.0;
		}
	}

	public int CompareTo(RefreshRate other)
	{
		return value.CompareTo(other.value);
	}

	public bool Equals(RefreshRate other)
	{
		return value == other.value;
	}
}
