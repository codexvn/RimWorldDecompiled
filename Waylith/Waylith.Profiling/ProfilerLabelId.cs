using System;

namespace Waylith.Profiling;

public readonly struct ProfilerLabelId : IEquatable<ProfilerLabelId>
{
	public static readonly ProfilerLabelId None = new ProfilerLabelId(ushort.MaxValue);

	public ushort Value { get; }

	public bool IsValid => Value != ushort.MaxValue;

	public ProfilerLabelId(ushort value)
	{
		Value = value;
	}

	public bool Equals(ProfilerLabelId other)
	{
		return Value == other.Value;
	}

	public override bool Equals(object? obj)
	{
		if (obj is ProfilerLabelId other)
		{
			return Equals(other);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return Value;
	}

	public static bool operator ==(ProfilerLabelId lhs, ProfilerLabelId rhs)
	{
		return lhs.Value == rhs.Value;
	}

	public static bool operator !=(ProfilerLabelId lhs, ProfilerLabelId rhs)
	{
		return lhs.Value != rhs.Value;
	}

	public override string ToString()
	{
		if (!IsValid)
		{
			return "none";
		}
		return Value.ToString();
	}
}
