using System;

namespace Steamworks;

[Serializable]
public struct HServerListRequest : IEquatable<HServerListRequest>
{
	public static readonly HServerListRequest Invalid = new HServerListRequest(IntPtr.Zero);

	public nint m_HServerListRequest;

	public HServerListRequest(nint value)
	{
		m_HServerListRequest = value;
	}

	public override string ToString()
	{
		return ((IntPtr)m_HServerListRequest).ToString();
	}

	public override bool Equals(object other)
	{
		if (other is HServerListRequest)
		{
			return this == (HServerListRequest)other;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return ((IntPtr)m_HServerListRequest).GetHashCode();
	}

	public static bool operator ==(HServerListRequest x, HServerListRequest y)
	{
		return x.m_HServerListRequest == y.m_HServerListRequest;
	}

	public static bool operator !=(HServerListRequest x, HServerListRequest y)
	{
		return !(x == y);
	}

	public static explicit operator HServerListRequest(nint value)
	{
		return new HServerListRequest(value);
	}

	public static explicit operator nint(HServerListRequest that)
	{
		return that.m_HServerListRequest;
	}

	public bool Equals(HServerListRequest other)
	{
		return m_HServerListRequest == other.m_HServerListRequest;
	}
}
