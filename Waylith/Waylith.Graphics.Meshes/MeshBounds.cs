using System;
using System.Numerics;
using Waylith.Math;

namespace Waylith.Graphics.Meshes;

public readonly struct MeshBounds
{
	public readonly float MinX;

	public readonly float MinY;

	public readonly float MinZ;

	public readonly float MaxX;

	public readonly float MaxY;

	public readonly float MaxZ;

	public static MeshBounds Empty => new MeshBounds(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity, float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity);

	public bool IsEmpty
	{
		get
		{
			if (!(MinX > MaxX) && !(MinY > MaxY))
			{
				return MinZ > MaxZ;
			}
			return true;
		}
	}

	public Vector3 Min => new Vector3(MinX, MinY, MinZ);

	public Vector3 Max => new Vector3(MaxX, MaxY, MaxZ);

	public Vector3 Center => (Min + Max) * 0.5f;

	public Vector3 Size => Max - Min;

	public Vector3 Extents => (Max - Min) * 0.5f;

	public MeshBounds(float minX, float minY, float minZ, float maxX, float maxY, float maxZ)
	{
		MinX = minX;
		MinY = minY;
		MinZ = minZ;
		MaxX = maxX;
		MaxY = maxY;
		MaxZ = maxZ;
	}

	public static implicit operator Bounds(MeshBounds bounds)
	{
		if (bounds.IsEmpty)
		{
			return default(Bounds);
		}
		Bounds result = default(Bounds);
		result.SetMinMax(bounds.Min, bounds.Max);
		return result;
	}

	public static implicit operator MeshBounds(Bounds bounds)
	{
		Vector3 min = bounds.min;
		Vector3 max = bounds.max;
		return new MeshBounds(min.X, min.Y, min.Z, max.X, max.Y, max.Z);
	}

	public MeshBounds Encapsulate(float x, float y, float z)
	{
		if (IsEmpty)
		{
			return new MeshBounds(x, y, z, x, y, z);
		}
		return new MeshBounds(System.Math.Min(MinX, x), System.Math.Min(MinY, y), System.Math.Min(MinZ, z), System.Math.Max(MaxX, x), System.Math.Max(MaxY, y), System.Math.Max(MaxZ, z));
	}
}
