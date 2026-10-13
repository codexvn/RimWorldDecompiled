using System;

namespace UnityEngine;

public class GeometryUtility
{
	public static void CalculateFrustumPlanes(Camera camera, Plane[] planes)
	{
		ArgumentNullException.ThrowIfNull(camera, "camera");
		ArgumentNullException.ThrowIfNull(planes, "planes");
		if (planes.Length < 6)
		{
			throw new ArgumentException("Frustum plane array must have length >= 6.", "planes");
		}
		float nearClipPlane = camera.nearClipPlane;
		float farClipPlane = camera.farClipPlane;
		float num = MathF.Max(1E-06f, camera.aspect);
		float num2;
		float num3;
		float num4;
		float num5;
		if (camera.orthographic)
		{
			num2 = camera.orthographicSize;
			num3 = num2 * num;
			num4 = num2;
			num5 = num3;
		}
		else
		{
			float num6 = MathF.Tan(camera.fieldOfView * ((float)Math.PI / 180f) * 0.5f);
			num2 = num6 * nearClipPlane;
			num3 = num2 * num;
			num4 = num6 * farClipPlane;
			num5 = num4 * num;
		}
		Vector3 vector = new Vector3(0f - num3, 0f - num2, 0f - nearClipPlane);
		Vector3 vector2 = new Vector3(num3, 0f - num2, 0f - nearClipPlane);
		Vector3 vector3 = new Vector3(0f - num3, num2, 0f - nearClipPlane);
		Vector3 vector4 = new Vector3(num3, num2, 0f - nearClipPlane);
		Vector3 vector5 = new Vector3(0f - num5, 0f - num4, 0f - farClipPlane);
		Vector3 vector6 = new Vector3(num5, 0f - num4, 0f - farClipPlane);
		Vector3 c = new Vector3(0f - num5, num4, 0f - farClipPlane);
		Vector3 c2 = new Vector3(num5, num4, 0f - farClipPlane);
		Vector3 insidePoint = new Vector3(0f, 0f, (0f - (nearClipPlane + farClipPlane)) * 0.5f);
		Plane plane = CreatePlane(vector, vector3, c, insidePoint);
		Plane plane2 = CreatePlane(vector4, vector2, vector6, insidePoint);
		Plane plane3 = CreatePlane(vector2, vector, vector5, insidePoint);
		Plane plane4 = CreatePlane(vector3, vector4, c2, insidePoint);
		Plane plane5 = CreatePlane(vector, vector2, vector4, insidePoint);
		Plane plane6 = CreatePlane(vector6, vector5, c, insidePoint);
		Matrix4x4 cameraToWorldMatrix = camera.cameraToWorldMatrix;
		planes[0] = TransformPlane(cameraToWorldMatrix, plane);
		planes[1] = TransformPlane(cameraToWorldMatrix, plane2);
		planes[2] = TransformPlane(cameraToWorldMatrix, plane3);
		planes[3] = TransformPlane(cameraToWorldMatrix, plane4);
		planes[4] = TransformPlane(cameraToWorldMatrix, plane5);
		planes[5] = TransformPlane(cameraToWorldMatrix, plane6);
	}

	public static Plane[] CalculateFrustumPlanes(Camera camera)
	{
		Plane[] array = new Plane[6];
		CalculateFrustumPlanes(camera, array);
		return array;
	}

	public static Plane[] CalculateFrustumPlanes(Matrix4x4 worldToProjectionMatrix)
	{
		Plane[] array = new Plane[6];
		CalculateFrustumPlanes(worldToProjectionMatrix, array);
		return array;
	}

	public static void CalculateFrustumPlanes(Matrix4x4 worldToProjectionMatrix, Plane[] planes)
	{
		ArgumentNullException.ThrowIfNull(planes, "planes");
		if (planes.Length < 6)
		{
			throw new ArgumentException("Frustum plane array must have length >= 6.", "planes");
		}
		Matrix4x4 matrix4x = worldToProjectionMatrix;
		Vector4 vector = new Vector4(matrix4x.m00, matrix4x.m01, matrix4x.m02, matrix4x.m03);
		Vector4 vector2 = new Vector4(matrix4x.m10, matrix4x.m11, matrix4x.m12, matrix4x.m13);
		Vector4 vector3 = new Vector4(matrix4x.m20, matrix4x.m21, matrix4x.m22, matrix4x.m23);
		Vector4 vector4 = new Vector4(matrix4x.m30, matrix4x.m31, matrix4x.m32, matrix4x.m33);
		planes[0] = FromRow(vector4 + vector);
		planes[1] = FromRow(vector4 - vector);
		planes[2] = FromRow(vector4 + vector2);
		planes[3] = FromRow(vector4 - vector2);
		planes[4] = FromRow(vector4 + vector3);
		planes[5] = FromRow(vector4 - vector3);
		static Plane FromRow(Vector4 r)
		{
			Vector3 vector5 = new Vector3(r.x, r.y, r.z);
			float magnitude = vector5.magnitude;
			if (!(magnitude <= 1E-12f))
			{
				return new Plane(vector5 / magnitude, r.w / magnitude);
			}
			return new Plane(Vector3.up, 0f);
		}
	}

	public static bool TestPlanesAABB(Plane[] planes, Bounds bounds)
	{
		ArgumentNullException.ThrowIfNull(planes, "planes");
		Vector3 center = bounds.center;
		Vector3 extents = bounds.extents;
		for (int i = 0; i < planes.Length; i++)
		{
			Vector3 normal = planes[i].normal;
			float num = MathF.Abs(normal.x) * extents.x + MathF.Abs(normal.y) * extents.y + MathF.Abs(normal.z) * extents.z;
			if (Vector3.Dot(normal, center) + planes[i].distance + num < 0f)
			{
				return false;
			}
		}
		return true;
	}

	private static Plane CreatePlane(Vector3 a, Vector3 b, Vector3 c, Vector3 insidePoint)
	{
		Vector3 vector = Vector3.Cross(b - a, c - a).normalized;
		if (Vector3.Dot(vector, insidePoint - a) < 0f)
		{
			vector = -vector;
		}
		return new Plane(vector, a);
	}

	private static Plane TransformPlane(Matrix4x4 matrix, Plane plane)
	{
		Matrix4x4 transpose = matrix.inverse.transpose;
		Vector4 vector = new Vector4(plane.normal.x, plane.normal.y, plane.normal.z, plane.distance);
		Vector4 vector2 = transpose * vector;
		Vector3 vector3 = new Vector3(vector2.x, vector2.y, vector2.z);
		float magnitude = vector3.magnitude;
		if (magnitude <= 1E-06f)
		{
			return plane;
		}
		Vector3 inNormal = vector3 / magnitude;
		float d = vector2.w / magnitude;
		return new Plane(inNormal, d);
	}
}
