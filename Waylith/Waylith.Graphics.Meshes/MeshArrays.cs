using System;
using System.Collections.Generic;
using System.Numerics;
using Waylith.Math;

namespace Waylith.Graphics.Meshes;

public static class MeshArrays
{
	public static CpuMeshData Build(Vector3[] vertices, int[]? triangles, Vector3[]? normals = null, Color32[]? colors = null, Vector2[]? uv0 = null, Vector2[]? uv1 = null)
	{
		return BuildCore(vertices, new int[1][] { triangles ?? Array.Empty<int>() }, normals, colors, ToVector4(uv0), ToVector4(uv1), null, null);
	}

	public static CpuMeshData Build(Vector3[] vertices, int[] triangles, Vector3[]? normals, Color32[]? colors, Vector4[]? uv0, Vector4[]? uv1 = null, Vector4[]? uv2 = null, Vector4[]? uv3 = null)
	{
		return BuildCore(vertices, new int[1][] { triangles ?? Array.Empty<int>() }, normals, colors, uv0, uv1, uv2, uv3);
	}

	public static CpuMeshData Build(Vector3[] vertices, int[][] submeshTriangles, Vector3[]? normals, Color32[]? colors, Vector4[]? uv0, Vector4[]? uv1 = null, Vector4[]? uv2 = null, Vector4[]? uv3 = null)
	{
		return BuildCore(vertices, submeshTriangles, normals, colors, uv0, uv1, uv2, uv3);
	}

	private unsafe static CpuMeshData BuildCore(Vector3[] vertices, int[][] submeshTriangles, Vector3[]? normals, Color32[]? colors, Vector4[]? uv0, Vector4[]? uv1, Vector4[]? uv2, Vector4[]? uv3)
	{
		ArgumentNullException.ThrowIfNull(vertices, "vertices");
		ArgumentNullException.ThrowIfNull(submeshTriangles, "submeshTriangles");
		int num = 0;
		int num2 = 0;
		foreach (int[] array in submeshTriangles)
		{
			num += array.Length;
			num2 = System.Math.Max(num2, MaxIndex(array));
		}
		CpuMeshData cpuMeshData = new CpuMeshData();
		int num3 = vertices.Length;
		if (num3 == 0)
		{
			return cpuMeshData;
		}
		List<VertexAttribute> list = new List<VertexAttribute>(7)
		{
			new VertexAttribute(VertexAttributeSemantic.Position, VertexAttributeFormat.Float32, 3)
		};
		bool flag = normals != null && normals.Length == num3;
		bool flag2 = colors != null && colors.Length == num3;
		bool flag3 = uv0 != null && uv0.Length == num3;
		bool flag4 = uv1 != null && uv1.Length == num3;
		bool flag5 = uv2 != null && uv2.Length == num3;
		bool flag6 = uv3 != null && uv3.Length == num3;
		if (flag)
		{
			list.Add(new VertexAttribute(VertexAttributeSemantic.Normal, VertexAttributeFormat.Float32, 3));
		}
		if (flag2)
		{
			list.Add(new VertexAttribute(VertexAttributeSemantic.Color, VertexAttributeFormat.UNorm8, 4));
		}
		if (flag3)
		{
			list.Add(new VertexAttribute(VertexAttributeSemantic.TexCoord0, VertexAttributeFormat.Float32, 4));
		}
		if (flag4)
		{
			list.Add(new VertexAttribute(VertexAttributeSemantic.TexCoord1, VertexAttributeFormat.Float32, 4));
		}
		if (flag5)
		{
			list.Add(new VertexAttribute(VertexAttributeSemantic.TexCoord2, VertexAttributeFormat.Float32, 4));
		}
		if (flag6)
		{
			list.Add(new VertexAttribute(VertexAttributeSemantic.TexCoord3, VertexAttributeFormat.Float32, 4));
		}
		VertexLayout vertexLayout = new VertexLayout(list);
		cpuMeshData.SetVertexLayout(vertexLayout, num3);
		MeshIndexFormat meshIndexFormat = ((num3 > 65535 || num2 > 65535) ? MeshIndexFormat.UInt32 : MeshIndexFormat.UInt16);
		cpuMeshData.SetIndexBufferParams(num, meshIndexFormat);
		cpuMeshData.SetSubMeshCount((num > 0) ? submeshTriangles.Length : 0);
		byte* pointer = (byte*)cpuMeshData.GetVertexStream(0).Pointer;
		int streamStride = vertexLayout.GetStreamStride(0);
		vertexLayout.TryGetAttribute(VertexAttributeSemantic.Position, out var attribute);
		vertexLayout.TryGetAttribute(VertexAttributeSemantic.Normal, out var attribute2);
		vertexLayout.TryGetAttribute(VertexAttributeSemantic.Color, out var attribute3);
		vertexLayout.TryGetAttribute(VertexAttributeSemantic.TexCoord0, out var attribute4);
		vertexLayout.TryGetAttribute(VertexAttributeSemantic.TexCoord1, out var attribute5);
		vertexLayout.TryGetAttribute(VertexAttributeSemantic.TexCoord2, out var attribute6);
		vertexLayout.TryGetAttribute(VertexAttributeSemantic.TexCoord3, out var attribute7);
		for (int j = 0; j < num3; j++)
		{
			byte* ptr = pointer + j * streamStride;
			WriteVector3(ptr + attribute.OffsetBytes, vertices[j], attribute.Format, attribute.Dimension);
			if (flag)
			{
				WriteVector3(ptr + attribute2.OffsetBytes, normals[j], attribute2.Format, attribute2.Dimension);
			}
			if (flag2)
			{
				WriteColor32(ptr + attribute3.OffsetBytes, colors[j], attribute3.Format, attribute3.Dimension);
			}
			if (flag3)
			{
				WriteVector4(ptr + attribute4.OffsetBytes, uv0[j], attribute4.Format, attribute4.Dimension);
			}
			if (flag4)
			{
				WriteVector4(ptr + attribute5.OffsetBytes, uv1[j], attribute5.Format, attribute5.Dimension);
			}
			if (flag5)
			{
				WriteVector4(ptr + attribute6.OffsetBytes, uv2[j], attribute6.Format, attribute6.Dimension);
			}
			if (flag6)
			{
				WriteVector4(ptr + attribute7.OffsetBytes, uv3[j], attribute7.Format, attribute7.Dimension);
			}
		}
		MeshBounds bounds = CalculateBounds(vertices);
		if (num > 0)
		{
			int num4 = 0;
			for (int k = 0; k < submeshTriangles.Length; k++)
			{
				int[] array2 = submeshTriangles[k];
				if (meshIndexFormat == MeshIndexFormat.UInt16)
				{
					ushort* ptr2 = (ushort*)(cpuMeshData.GetIndexBuffer().Pointer + (nint)num4 * (nint)2);
					for (int l = 0; l < array2.Length; l++)
					{
						ptr2[l] = (ushort)array2[l];
					}
				}
				else
				{
					int* ptr3 = (int*)(cpuMeshData.GetIndexBuffer().Pointer + (nint)num4 * (nint)4);
					for (int m = 0; m < array2.Length; m++)
					{
						ptr3[m] = array2[m];
					}
				}
				cpuMeshData.SetSubMesh(k, new SubMesh(num4, array2.Length, MeshTopology.Triangles, 0, bounds));
				num4 += array2.Length;
			}
		}
		cpuMeshData.Bounds = bounds;
		return cpuMeshData;
	}

	public static Vector3[] GetPositions(CpuMeshData storage)
	{
		return DecodeVector3(storage, VertexAttributeSemantic.Position);
	}

	public static Vector3[] GetNormals(CpuMeshData storage)
	{
		return DecodeVector3(storage, VertexAttributeSemantic.Normal);
	}

	public static Color32[] GetColors(CpuMeshData storage)
	{
		return DecodeColor(storage, VertexAttributeSemantic.Color);
	}

	public static Vector4[] GetUv(CpuMeshData storage, int channel)
	{
		return DecodeVector4(storage, TexCoordSemantic(channel));
	}

	public static Vector2[] GetUv2D(CpuMeshData storage, int channel)
	{
		Vector4[] uv = GetUv(storage, channel);
		Vector2[] array = new Vector2[uv.Length];
		for (int i = 0; i < uv.Length; i++)
		{
			array[i] = new Vector2(uv[i].X, uv[i].Y);
		}
		return array;
	}

	public unsafe static int[] GetTriangles(CpuMeshData storage, int submesh = 0)
	{
		if ((uint)submesh >= (uint)storage.SubMeshes.Length)
		{
			return Array.Empty<int>();
		}
		SubMesh subMesh = storage.SubMeshes[submesh];
		int[] array = new int[subMesh.IndexCount];
		if (storage.IndexFormat == MeshIndexFormat.UInt16)
		{
			ushort* pointer = (ushort*)storage.GetIndexBuffer().Pointer;
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = pointer[subMesh.IndexStart + i];
			}
		}
		else
		{
			int* pointer2 = (int*)storage.GetIndexBuffer().Pointer;
			for (int j = 0; j < array.Length; j++)
			{
				array[j] = pointer2[subMesh.IndexStart + j];
			}
		}
		return array;
	}

	public static Vector3[] CalculateNormals(Vector3[] vertices, int[] triangles)
	{
		Vector3[] array = new Vector3[vertices.Length];
		for (int i = 0; i + 2 < triangles.Length; i += 3)
		{
			int num = triangles[i];
			int num2 = triangles[i + 1];
			int num3 = triangles[i + 2];
			if ((uint)num < (uint)vertices.Length && (uint)num2 < (uint)vertices.Length && (uint)num3 < (uint)vertices.Length)
			{
				Vector3 vector = vertices[num];
				Vector3 vector2 = vertices[num2];
				Vector3 vector4 = Vector3.Cross(vector2: vertices[num3] - vector, vector1: vector2 - vector);
				array[num] += vector4;
				array[num2] += vector4;
				array[num3] += vector4;
			}
		}
		for (int j = 0; j < array.Length; j++)
		{
			float num4 = array[j].Length();
			if (num4 > 1E-08f)
			{
				array[j] /= num4;
			}
		}
		return array;
	}

	public static MeshBounds CalculateBounds(ReadOnlySpan<Vector3> positions)
	{
		if (positions.Length == 0)
		{
			return default(MeshBounds);
		}
		MeshBounds result = MeshBounds.Empty;
		for (int i = 0; i < positions.Length; i++)
		{
			result = result.Encapsulate(positions[i].X, positions[i].Y, positions[i].Z);
		}
		return result;
	}

	public static CpuMeshData Combine(IReadOnlyList<(Mesh mesh, Matrix4x4 transform)> instances)
	{
		List<Vector3> list = new List<Vector3>();
		List<Vector3> list2 = new List<Vector3>();
		List<Vector2> list3 = new List<Vector2>();
		List<Color32> list4 = new List<Color32>();
		List<int> list5 = new List<int>();
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		foreach (var (mesh, m) in instances)
		{
			if (mesh == null)
			{
				continue;
			}
			CpuMeshData cpuStorage = mesh.CpuStorage;
			Vector3[] positions = GetPositions(cpuStorage);
			if (positions.Length == 0)
			{
				continue;
			}
			int count = list.Count;
			for (int i = 0; i < positions.Length; i++)
			{
				list.Add(m.MultiplyPoint3x4(positions[i]));
			}
			Vector3[] normals = GetNormals(cpuStorage);
			if (normals.Length == positions.Length)
			{
				flag = true;
				for (int j = 0; j < normals.Length; j++)
				{
					list2.Add(Vector3Extensions.get_normalized(m.MultiplyVector(normals[j])));
				}
			}
			else
			{
				for (int k = 0; k < positions.Length; k++)
				{
					list2.Add(Vector3.UnitY);
				}
			}
			Vector2[] uv2D = GetUv2D(cpuStorage, 0);
			if (uv2D.Length == positions.Length)
			{
				flag2 = true;
				list3.AddRange(uv2D);
			}
			else
			{
				for (int l = 0; l < positions.Length; l++)
				{
					list3.Add(default(Vector2));
				}
			}
			Color32[] colors = GetColors(cpuStorage);
			if (colors.Length == positions.Length)
			{
				flag3 = true;
				list4.AddRange(colors);
			}
			else
			{
				for (int n = 0; n < positions.Length; n++)
				{
					list4.Add(new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue));
				}
			}
			int[] triangles = GetTriangles(cpuStorage);
			for (int num = 0; num < triangles.Length; num++)
			{
				list5.Add(triangles[num] + count);
			}
		}
		return Build(list.ToArray(), list5.ToArray(), flag ? list2.ToArray() : null, flag3 ? list4.ToArray() : null, flag2 ? list3.ToArray() : null);
	}

	public unsafe static void RecalculateBounds(CpuMeshData storage)
	{
		if (!storage.Layout.TryGetAttribute(VertexAttributeSemantic.Position, out var attribute) || storage.VertexCount == 0)
		{
			storage.Bounds = default(MeshBounds);
			return;
		}
		byte* pointer = (byte*)storage.GetVertexStream(attribute.Stream).Pointer;
		int streamStride = storage.Layout.GetStreamStride(attribute.Stream);
		int componentByteSize = VertexLayout.GetComponentByteSize(attribute.Format);
		MeshBounds bounds = MeshBounds.Empty;
		for (int i = 0; i < storage.VertexCount; i++)
		{
			byte* ptr = pointer + i * streamStride + attribute.OffsetBytes;
			bounds = bounds.Encapsulate((attribute.Dimension > 0) ? ReadScalar(ptr, attribute.Format) : 0f, (attribute.Dimension > 1) ? ReadScalar(ptr + componentByteSize, attribute.Format) : 0f, (attribute.Dimension > 2) ? ReadScalar(ptr + 2 * componentByteSize, attribute.Format) : 0f);
		}
		storage.Bounds = bounds;
	}

	public static VertexAttributeSemantic TexCoordSemantic(int channel)
	{
		return channel switch
		{
			0 => VertexAttributeSemantic.TexCoord0, 
			1 => VertexAttributeSemantic.TexCoord1, 
			2 => VertexAttributeSemantic.TexCoord2, 
			3 => VertexAttributeSemantic.TexCoord3, 
			4 => VertexAttributeSemantic.TexCoord4, 
			5 => VertexAttributeSemantic.TexCoord5, 
			6 => VertexAttributeSemantic.TexCoord6, 
			7 => VertexAttributeSemantic.TexCoord7, 
			_ => VertexAttributeSemantic.TexCoord0, 
		};
	}

	private static Vector4[]? ToVector4(Vector2[]? source)
	{
		if (source == null)
		{
			return null;
		}
		Vector4[] array = new Vector4[source.Length];
		for (int i = 0; i < source.Length; i++)
		{
			array[i] = new Vector4(source[i].X, source[i].Y, 0f, 0f);
		}
		return array;
	}

	private static int MaxIndex(int[] indices)
	{
		int num = 0;
		for (int i = 0; i < indices.Length; i++)
		{
			num = System.Math.Max(num, indices[i]);
		}
		return num;
	}

	public unsafe static Vector3[] DecodeVector3(CpuMeshData storage, VertexAttributeSemantic semantic)
	{
		if (!storage.Layout.TryGetAttribute(semantic, out var attribute) || storage.VertexCount == 0)
		{
			return Array.Empty<Vector3>();
		}
		Vector3[] array = new Vector3[storage.VertexCount];
		byte* pointer = (byte*)storage.GetVertexStream(attribute.Stream).Pointer;
		int streamStride = storage.Layout.GetStreamStride(attribute.Stream);
		int componentByteSize = VertexLayout.GetComponentByteSize(attribute.Format);
		for (int i = 0; i < storage.VertexCount; i++)
		{
			byte* ptr = pointer + i * streamStride + attribute.OffsetBytes;
			array[i] = new Vector3((attribute.Dimension > 0) ? ReadScalar(ptr, attribute.Format) : 0f, (attribute.Dimension > 1) ? ReadScalar(ptr + componentByteSize, attribute.Format) : 0f, (attribute.Dimension > 2) ? ReadScalar(ptr + 2 * componentByteSize, attribute.Format) : 0f);
		}
		return array;
	}

	public unsafe static Vector4[] DecodeVector4(CpuMeshData storage, VertexAttributeSemantic semantic)
	{
		if (!storage.Layout.TryGetAttribute(semantic, out var attribute) || storage.VertexCount == 0)
		{
			return Array.Empty<Vector4>();
		}
		Vector4[] array = new Vector4[storage.VertexCount];
		byte* pointer = (byte*)storage.GetVertexStream(attribute.Stream).Pointer;
		int streamStride = storage.Layout.GetStreamStride(attribute.Stream);
		int componentByteSize = VertexLayout.GetComponentByteSize(attribute.Format);
		for (int i = 0; i < storage.VertexCount; i++)
		{
			byte* ptr = pointer + i * streamStride + attribute.OffsetBytes;
			array[i] = new Vector4((attribute.Dimension > 0) ? ReadScalar(ptr, attribute.Format) : 0f, (attribute.Dimension > 1) ? ReadScalar(ptr + componentByteSize, attribute.Format) : 0f, (attribute.Dimension > 2) ? ReadScalar(ptr + 2 * componentByteSize, attribute.Format) : 0f, (attribute.Dimension > 3) ? ReadScalar(ptr + 3 * componentByteSize, attribute.Format) : 0f);
		}
		return array;
	}

	public static Color32[] DecodeColor(CpuMeshData storage, VertexAttributeSemantic semantic)
	{
		if (!storage.Layout.TryGetAttribute(semantic, out var attribute) || storage.VertexCount == 0)
		{
			return Array.Empty<Color32>();
		}
		Vector4[] array = DecodeVector4(storage, semantic);
		Color32[] array2 = new Color32[array.Length];
		for (int i = 0; i < array.Length; i++)
		{
			Vector4 vector = array[i];
			array2[i] = new Color32(ClampToByte(vector.X * 255f), ClampToByte(vector.Y * 255f), ClampToByte(vector.Z * 255f), ClampToByte(((attribute.Dimension > 3) ? vector.W : 1f) * 255f));
		}
		return array2;
	}

	public unsafe static float ReadScalar(byte* ptr, VertexAttributeFormat format)
	{
		return format switch
		{
			VertexAttributeFormat.Float32 => *(float*)ptr, 
			VertexAttributeFormat.Float16 => (float)(*(Half*)ptr), 
			VertexAttributeFormat.UNorm8 => (float)(int)(*ptr) / 255f, 
			VertexAttributeFormat.SNorm8 => System.Math.Max((float)(int)(*ptr) / 127f, -1f), 
			VertexAttributeFormat.UNorm16 => (float)(int)(*(ushort*)ptr) / 65535f, 
			VertexAttributeFormat.SNorm16 => System.Math.Max((float)(*(short*)ptr) / 32767f, -1f), 
			VertexAttributeFormat.UInt8 => (int)(*ptr), 
			VertexAttributeFormat.SInt8 => (int)(*ptr), 
			VertexAttributeFormat.UInt16 => (int)(*(ushort*)ptr), 
			VertexAttributeFormat.SInt16 => *(short*)ptr, 
			VertexAttributeFormat.UInt32 => *(uint*)ptr, 
			VertexAttributeFormat.SInt32 => *(int*)ptr, 
			_ => 0f, 
		};
	}

	public unsafe static void WriteVector3(byte* ptr, Vector3 value, VertexAttributeFormat format, int dimension)
	{
		WriteScalar(ptr, value.X, format);
		if (dimension > 1)
		{
			WriteScalar(ptr + VertexLayout.GetComponentByteSize(format), value.Y, format);
		}
		if (dimension > 2)
		{
			WriteScalar(ptr + VertexLayout.GetComponentByteSize(format) * 2, value.Z, format);
		}
	}

	public unsafe static void WriteVector4(byte* ptr, Vector4 value, VertexAttributeFormat format, int dimension)
	{
		WriteScalar(ptr, value.X, format);
		if (dimension > 1)
		{
			WriteScalar(ptr + VertexLayout.GetComponentByteSize(format), value.Y, format);
		}
		if (dimension > 2)
		{
			WriteScalar(ptr + VertexLayout.GetComponentByteSize(format) * 2, value.Z, format);
		}
		if (dimension > 3)
		{
			WriteScalar(ptr + VertexLayout.GetComponentByteSize(format) * 3, value.W, format);
		}
	}

	public unsafe static void WriteColor32(byte* ptr, Color32 value, VertexAttributeFormat format, int dimension)
	{
		if (format == VertexAttributeFormat.UNorm8 || format == VertexAttributeFormat.UInt8)
		{
			*ptr = value.r;
			if (dimension > 1)
			{
				ptr[1] = value.g;
			}
			if (dimension > 2)
			{
				ptr[2] = value.b;
			}
			if (dimension > 3)
			{
				ptr[3] = value.a;
			}
		}
		else
		{
			Color color = value;
			WriteVector4(ptr, new Vector4(color.r, color.g, color.b, color.a), format, dimension);
		}
	}

	public unsafe static void WriteScalar(byte* ptr, float value, VertexAttributeFormat format)
	{
		switch (format)
		{
		case VertexAttributeFormat.Float32:
			*(float*)ptr = value;
			break;
		case VertexAttributeFormat.Float16:
			*(Half*)ptr = (Half)value;
			break;
		case VertexAttributeFormat.UNorm8:
		case VertexAttributeFormat.UInt8:
			*ptr = ClampToByte(value * ((format == VertexAttributeFormat.UNorm8) ? 255f : 1f));
			break;
		case VertexAttributeFormat.UNorm16:
		case VertexAttributeFormat.UInt16:
			*(ushort*)ptr = (ushort)System.Math.Clamp((int)MathF.Round(value * ((format == VertexAttributeFormat.UNorm16) ? 65535f : 1f)), 0, 65535);
			break;
		case VertexAttributeFormat.SInt8:
			*ptr = (byte)(sbyte)System.Math.Clamp((int)MathF.Round(value), -128, 127);
			break;
		case VertexAttributeFormat.SInt16:
			*(short*)ptr = (short)System.Math.Clamp((int)MathF.Round(value), -32768, 32767);
			break;
		case VertexAttributeFormat.UInt32:
			*(int*)ptr = System.Math.Max(0, (int)MathF.Round(value));
			break;
		case VertexAttributeFormat.SInt32:
			*(int*)ptr = (int)MathF.Round(value);
			break;
		case VertexAttributeFormat.SNorm8:
			*ptr = (byte)(sbyte)System.Math.Clamp((int)MathF.Round(System.Math.Clamp(value, -1f, 1f) * 127f), -128, 127);
			break;
		case VertexAttributeFormat.SNorm16:
			*(short*)ptr = (short)System.Math.Clamp((int)MathF.Round(System.Math.Clamp(value, -1f, 1f) * 32767f), -32768, 32767);
			break;
		}
	}

	private static byte ClampToByte(float value)
	{
		return (byte)System.Math.Clamp((int)MathF.Round(value), 0, 255);
	}
}
