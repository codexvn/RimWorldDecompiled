using System;

namespace UnityEngine;

[Serializable]
public struct BoneWeight1 : IEquatable<BoneWeight1>
{
	[SerializeField]
	private float m_Weight;

	[SerializeField]
	private int m_BoneIndex;

	public float weight
	{
		get
		{
			return m_Weight;
		}
		set
		{
			m_Weight = value;
		}
	}

	public int boneIndex
	{
		get
		{
			return m_BoneIndex;
		}
		set
		{
			m_BoneIndex = value;
		}
	}

	public override bool Equals(object other)
	{
		if (other is BoneWeight1 other2)
		{
			return Equals(other2);
		}
		return false;
	}

	public bool Equals(BoneWeight1 other)
	{
		if (boneIndex.Equals(other.boneIndex))
		{
			return weight.Equals(other.weight);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return boneIndex.GetHashCode() ^ weight.GetHashCode();
	}

	public static bool operator ==(BoneWeight1 lhs, BoneWeight1 rhs)
	{
		if (lhs.boneIndex == rhs.boneIndex)
		{
			return lhs.weight == rhs.weight;
		}
		return false;
	}

	public static bool operator !=(BoneWeight1 lhs, BoneWeight1 rhs)
	{
		return !(lhs == rhs);
	}
}
