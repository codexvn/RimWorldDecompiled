using System.Numerics;
using Waylith.Math;

namespace Waylith.Graphics.Meshes;

public static class MeshRaycast
{
	public static bool Raycast(Ray ray, Mesh mesh, Vector3 position, Quaternion rotation, Vector3 scale, out MeshRaycastHit hitInfo)
	{
		return Raycast(ray, mesh, Matrix4x4Extensions.Inverse(Matrix4x4Extensions.TRS(position, rotation, scale)), float.PositiveInfinity, out hitInfo);
	}

	public unsafe static bool Raycast(Ray ray, Mesh mesh, in Matrix4x4 worldToLocal, float maxDistance, out MeshRaycastHit hitInfo)
	{
		hitInfo = default(MeshRaycastHit);
		if (mesh == null || mesh.VertexCount == 0 || !mesh.IsReadable)
		{
			return false;
		}
		CpuMeshData cpuStorage = mesh.CpuStorage;
		if (!cpuStorage.Layout.TryGetAttribute(VertexAttributeSemantic.Position, out var attribute) || attribute.Format != 0 || cpuStorage.SubMeshes.Length == 0)
		{
			return false;
		}
		Vector3 vector = worldToLocal.MultiplyPoint3x4(ray.origin);
		Vector3 vector2 = worldToLocal.MultiplyVector(ray.direction);
		byte* streamPtr = (byte*)cpuStorage.GetVertexStream(attribute.Stream).Pointer;
		int stride = cpuStorage.Layout.GetStreamStride(attribute.Stream);
		int positionOffset = attribute.OffsetBytes;
		int vertexCount = cpuStorage.VertexCount;
		SubMesh subMesh = cpuStorage.SubMeshes[0];
		int indexStart = subMesh.IndexStart;
		int indexCount = subMesh.IndexCount;
		ushort* ptr = ((cpuStorage.IndexFormat == MeshIndexFormat.UInt16) ? ((ushort*)cpuStorage.GetIndexBuffer().Pointer) : ((ushort*)null));
		int* ptr2 = ((cpuStorage.IndexFormat == MeshIndexFormat.UInt32) ? ((int*)cpuStorage.GetIndexBuffer().Pointer) : ((int*)null));
		float num = float.PositiveInfinity;
		Vector3 vector3 = Vector3.Zero;
		int triangleIndex = 0;
		bool flag = false;
		for (int i = 0; i + 2 < indexCount; i += 3)
		{
			int num2;
			int num3;
			int num4;
			if (ptr != null)
			{
				num2 = ptr[indexStart + i];
				num3 = ptr[indexStart + i + 1];
				num4 = ptr[indexStart + i + 2];
			}
			else
			{
				num2 = ptr2[indexStart + i];
				num3 = ptr2[indexStart + i + 1];
				num4 = ptr2[indexStart + i + 2];
			}
			if ((uint)num2 >= (uint)vertexCount || (uint)num3 >= (uint)vertexCount || (uint)num4 >= (uint)vertexCount)
			{
				continue;
			}
			Vector3 vector4 = ReadPosition(num2);
			Vector3 vector5 = ReadPosition(num3);
			Vector3 vector6 = ReadPosition(num4);
			Vector3 vector7 = vector5 - vector4;
			Vector3 vector8 = vector6 - vector4;
			Vector3 vector9 = Vector3.Cross(vector2, vector8);
			float num5 = Vector3.Dot(vector7, vector9);
			if (num5 > -1E-08f && num5 < 1E-08f)
			{
				continue;
			}
			float num6 = 1f / num5;
			Vector3 vector10 = vector - vector4;
			float num7 = num6 * Vector3.Dot(vector10, vector9);
			if (num7 < 0f || num7 > 1f)
			{
				continue;
			}
			Vector3 vector11 = Vector3.Cross(vector10, vector7);
			float num8 = num6 * Vector3.Dot(vector2, vector11);
			if (!(num8 < 0f) && !(num7 + num8 > 1f))
			{
				float num9 = num6 * Vector3.Dot(vector8, vector11);
				if (!(num9 < 1E-06f) && !(num9 > maxDistance) && !(num9 >= num))
				{
					num = num9;
					vector3 = Vector3Extensions.get_normalized(Vector3.Cross(vector7, vector8));
					triangleIndex = i / 3;
					flag = true;
				}
			}
		}
		if (!flag)
		{
			return false;
		}
		hitInfo = new MeshRaycastHit
		{
			point = ray.GetPoint(num),
			normal = Vector3Extensions.get_normalized(Matrix4x4.Transpose(worldToLocal).MultiplyVector(vector3)),
			distance = num,
			triangleIndex = triangleIndex
		};
		return true;
		unsafe Vector3 ReadPosition(int index)
		{
			return *(Vector3*)(streamPtr + (long)index * (long)stride + positionOffset);
		}
	}
}
