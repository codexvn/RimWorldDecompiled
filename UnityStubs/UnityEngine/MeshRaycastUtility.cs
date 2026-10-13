using UnityStubs.Internal;

namespace UnityEngine;

public static class MeshRaycastUtility
{
	private const int BvhMinTriangles = 64;

	private const float DistanceEpsilon = 1E-06f;

	public static bool Raycast(Ray ray, Mesh mesh, Vector3 position, Quaternion rotation, Vector3 scale, out RaycastHit hitInfo)
	{
		return Raycast(ray, mesh, Matrix4x4.TRS(position, rotation, scale).inverse, float.PositiveInfinity, null, out hitInfo);
	}

	internal static bool Raycast(Ray ray, Mesh mesh, in Matrix4x4 worldToLocal, float maxDistance, Collider collider, out RaycastHit hitInfo)
	{
		hitInfo = default(RaycastHit);
		if (mesh == null || mesh.vertexCount == 0)
		{
			return false;
		}
		Vector3 origin = worldToLocal.MultiplyPoint3x4(ray.origin);
		Vector3 dir = worldToLocal.MultiplyVector(ray.direction);
		Vector3[] verticesForReading = mesh.verticesForReading;
		int[] trianglesForReading = mesh.trianglesForReading;
		float bestT = float.PositiveInfinity;
		int bestTriangle = 0;
		bool flag;
		if (trianglesForReading.Length >= 192)
		{
			MeshRaycastBvh meshRaycastBvh = mesh.RaycastBvh as MeshRaycastBvh;
			if (meshRaycastBvh == null || !meshRaycastBvh.IsFor(verticesForReading, trianglesForReading))
			{
				meshRaycastBvh = (MeshRaycastBvh)(mesh.RaycastBvh = new MeshRaycastBvh(verticesForReading, trianglesForReading));
			}
			flag = meshRaycastBvh.Raycast(origin, dir, 1E-06f, maxDistance, out bestT, out bestTriangle);
		}
		else
		{
			flag = false;
			for (int i = 0; i + 2 < trianglesForReading.Length; i += 3)
			{
				if (IntersectTriangle(origin, dir, verticesForReading[trianglesForReading[i]], verticesForReading[trianglesForReading[i + 1]], verticesForReading[trianglesForReading[i + 2]], out var t) && !(t < 1E-06f) && !(t > maxDistance) && !(t >= bestT))
				{
					bestT = t;
					bestTriangle = i / 3;
					flag = true;
				}
			}
		}
		if (!flag)
		{
			return false;
		}
		Vector3 vector = verticesForReading[trianglesForReading[bestTriangle * 3]];
		Vector3 normalized = Vector3.Cross(verticesForReading[trianglesForReading[bestTriangle * 3 + 1]] - vector, verticesForReading[trianglesForReading[bestTriangle * 3 + 2]] - vector).normalized;
		hitInfo = new RaycastHit
		{
			point = ray.GetPoint(bestT),
			normal = worldToLocal.transpose.MultiplyVector(normalized).normalized,
			distance = bestT,
			collider = collider,
			triangleIndex = bestTriangle
		};
		return true;
	}

	internal static bool IntersectTriangle(Vector3 origin, Vector3 dir, Vector3 v0, Vector3 v1, Vector3 v2, out float t)
	{
		t = 0f;
		Vector3 vector = v1 - v0;
		Vector3 vector2 = v2 - v0;
		Vector3 rhs = Vector3.Cross(dir, vector2);
		float num = Vector3.Dot(vector, rhs);
		if (num > -1E-08f && num < 1E-08f)
		{
			return false;
		}
		float num2 = 1f / num;
		Vector3 lhs = origin - v0;
		float num3 = num2 * Vector3.Dot(lhs, rhs);
		if (num3 < 0f || num3 > 1f)
		{
			return false;
		}
		Vector3 rhs2 = Vector3.Cross(lhs, vector);
		float num4 = num2 * Vector3.Dot(dir, rhs2);
		if (num4 < 0f || num3 + num4 > 1f)
		{
			return false;
		}
		t = num2 * Vector3.Dot(vector2, rhs2);
		return true;
	}
}
