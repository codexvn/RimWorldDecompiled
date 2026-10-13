using System;

namespace UnityEngine;

public struct BoundsInt : IEquatable<BoundsInt>
{
	public Vector3Int position;

	public Vector3Int size;

	public int x
	{
		get
		{
			return position.x;
		}
		set
		{
			position.x = value;
		}
	}

	public int y
	{
		get
		{
			return position.y;
		}
		set
		{
			position.y = value;
		}
	}

	public int z
	{
		get
		{
			return position.z;
		}
		set
		{
			position.z = value;
		}
	}

	public int xMin => Math.Min(position.x, position.x + size.x);

	public int yMin => Math.Min(position.y, position.y + size.y);

	public int zMin => Math.Min(position.z, position.z + size.z);

	public int xMax => Math.Max(position.x, position.x + size.x);

	public int yMax => Math.Max(position.y, position.y + size.y);

	public int zMax => Math.Max(position.z, position.z + size.z);

	public Vector3Int min => new Vector3Int(xMin, yMin, zMin);

	public Vector3Int max => new Vector3Int(xMax, yMax, zMax);

	public BoundsInt(Vector3Int position, Vector3Int size)
	{
		this.position = position;
		this.size = size;
	}

	public BoundsInt(int xMin, int yMin, int zMin, int sizeX, int sizeY, int sizeZ)
		: this(new Vector3Int(xMin, yMin, zMin), new Vector3Int(sizeX, sizeY, sizeZ))
	{
	}

	public bool Contains(Vector3Int p)
	{
		if (p.x >= xMin && p.y >= yMin && p.z >= zMin && p.x < xMax && p.y < yMax)
		{
			return p.z < zMax;
		}
		return false;
	}

	public bool Equals(BoundsInt other)
	{
		if (position.Equals(other.position))
		{
			return size.Equals(other.size);
		}
		return false;
	}

	public override bool Equals(object obj)
	{
		if (obj is BoundsInt other)
		{
			return Equals(other);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(position, size);
	}

	public override string ToString()
	{
		return $"Position: {position}, Size: {size}";
	}
}
