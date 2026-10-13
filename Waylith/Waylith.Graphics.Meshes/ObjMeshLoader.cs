using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Numerics;

namespace Waylith.Graphics.Meshes;

public static class ObjMeshLoader
{
	private struct FaceIndex
	{
		public int V;

		public int T;

		public int N;
	}

	public const int FormatVersion = 1;

	private static readonly string CacheParser = $"ObjMeshLoader/v{1}";

	public static Mesh LoadFromFile(string filePath)
	{
		if (string.IsNullOrWhiteSpace(filePath))
		{
			throw new ArgumentException("Mesh file path must not be null or empty.", "filePath");
		}
		string extension = Path.GetExtension(filePath);
		if (!extension.Equals(".obj", StringComparison.OrdinalIgnoreCase))
		{
			throw new NotSupportedException("Mesh format '" + extension + "' is not supported.");
		}
		string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filePath);
		CpuMeshData cpuMeshData = new CpuMeshData();
		if (CpuMeshCache.TryLoad(filePath, cpuMeshData, CacheParser))
		{
			Mesh mesh = new Mesh();
			mesh.DebugName = (string.IsNullOrWhiteSpace(fileNameWithoutExtension) ? "ObjMesh" : fileNameWithoutExtension);
			mesh.ReplaceCpuStorage(cpuMeshData);
			return mesh;
		}
		cpuMeshData.Dispose();
		(long, long)? sourceStamp = CpuMeshCache.StampSource(filePath);
		Mesh mesh2 = LoadObj(File.ReadLines(filePath), fileNameWithoutExtension);
		CpuMeshData cpuStorage = mesh2.CpuStorage;
		if (cpuStorage != null)
		{
			CpuMeshCache.Store(filePath, sourceStamp, cpuStorage, CacheParser);
		}
		return mesh2;
	}

	public static Mesh Parse(string filePath)
	{
		return LoadObj(File.ReadLines(filePath), Path.GetFileNameWithoutExtension(filePath));
	}

	private static Mesh LoadObj(IEnumerable<string> lines, string meshName)
	{
		List<FaceIndex> list = new List<FaceIndex>();
		List<Vector3> list2 = new List<Vector3>();
		List<Vector3> list3 = new List<Vector3>();
		List<Vector3> list4 = new List<Vector3>();
		List<Vector2> list5 = new List<Vector2>();
		Dictionary<(Vector3, Vector2, Vector3), int> dictionary = new Dictionary<(Vector3, Vector2, Vector3), int>();
		Dictionary<int, int> dictionary2 = new Dictionary<int, int>();
		List<int> list6 = new List<int>();
		bool flag = false;
		foreach (string line in lines)
		{
			if (string.IsNullOrWhiteSpace(line))
			{
				continue;
			}
			ReadOnlySpan<char> span = line.AsSpan().Trim();
			if (span.Length == 0 || span[0] == '#')
			{
				continue;
			}
			int num = FindWhitespace(span);
			if (num < 0)
			{
				continue;
			}
			switch (span.Slice(0, num))
			{
			case "v":
			{
				float[] values;
				int num2 = ReadArbitraryVector(span.Slice(num + 1), out values);
				if (num2 <= 4)
				{
					list2.Add(new Vector3(0f - values[0], values[1], values[2]));
					break;
				}
				if (num2 == 6)
				{
					list2.Add(new Vector3(0f - values[0], values[1], values[2]));
					list4.Add(new Vector3(values[3], values[4], values[5]));
					flag = true;
					break;
				}
				throw new InvalidDataException($"Unsupported OBJ vertex component count '{num2}'.");
			}
			case "vn":
				list3.Add(ReadVector3(span.Slice(num + 1)) * new Vector3(-1f, 1f, 1f));
				break;
			case "vt":
				list5.Add(ReadVector2(span.Slice(num + 1)));
				break;
			case "f":
				AddFaceIndices(list, list2, span.Slice(num + 1));
				break;
			}
		}
		for (int i = 0; i < list.Count; i++)
		{
			FaceIndex faceIndex = list[i];
			Vector3 item = list2[faceIndex.V - 1];
			Vector2 item2 = ((faceIndex.T >= 1) ? list5[faceIndex.T - 1] : Vector2.Zero);
			Vector3 item3 = ((faceIndex.N >= 1) ? list3[faceIndex.N - 1] : Vector3.Zero);
			(Vector3, Vector2, Vector3) key = (item, item2, item3);
			if (dictionary.TryGetValue(key, out var value))
			{
				list6.Add(value);
				continue;
			}
			int num3 = (dictionary[key] = dictionary.Count);
			list6.Add(num3);
			dictionary2.Add(num3, faceIndex.V - 1);
		}
		Vector3[] array = new Vector3[dictionary.Count];
		Vector3[] array2 = new Vector3[dictionary.Count];
		Vector3[] array3 = new Vector3[dictionary.Count];
		Vector2[] array4 = new Vector2[dictionary.Count];
		foreach (var (tuple2, num5) in dictionary)
		{
			array[num5] = tuple2.Item1;
			array4[num5] = tuple2.Item2;
			array2[num5] = tuple2.Item3;
			if (flag)
			{
				array3[num5] = list4[dictionary2[num5]];
			}
		}
		Mesh mesh = new Mesh();
		mesh.DebugName = (string.IsNullOrWhiteSpace(meshName) ? "ObjMesh" : meshName);
		mesh.ReplaceCpuStorage(BuildStorage(array, array2, flag ? array3 : null, array4, list6));
		return mesh;
	}

	private unsafe static CpuMeshData BuildStorage(Vector3[] positions, Vector3[] normals, Vector3[]? colors, Vector2[] uvs, List<int> indices)
	{
		CpuMeshData cpuMeshData = new CpuMeshData();
		if (positions.Length == 0)
		{
			return cpuMeshData;
		}
		List<VertexAttribute> list = new List<VertexAttribute>(4)
		{
			new VertexAttribute(VertexAttributeSemantic.Position, VertexAttributeFormat.Float32, 3),
			new VertexAttribute(VertexAttributeSemantic.Normal, VertexAttributeFormat.Float32, 3)
		};
		if (colors != null)
		{
			list.Add(new VertexAttribute(VertexAttributeSemantic.Color, VertexAttributeFormat.UNorm8, 4));
		}
		list.Add(new VertexAttribute(VertexAttributeSemantic.TexCoord0, VertexAttributeFormat.Float32, 4));
		VertexLayout vertexLayout = new VertexLayout(list);
		cpuMeshData.SetVertexLayout(vertexLayout, positions.Length);
		int num = 0;
		for (int i = 0; i < indices.Count; i++)
		{
			num = System.Math.Max(num, indices[i]);
		}
		MeshIndexFormat meshIndexFormat = ((num > 65535) ? MeshIndexFormat.UInt32 : MeshIndexFormat.UInt16);
		cpuMeshData.SetIndexBufferParams(indices.Count, meshIndexFormat);
		cpuMeshData.SetSubMeshCount((indices.Count > 0) ? 1 : 0);
		byte* pointer = (byte*)cpuMeshData.GetVertexStream(0).Pointer;
		int streamStride = vertexLayout.GetStreamStride(0);
		vertexLayout.TryGetAttribute(VertexAttributeSemantic.Position, out var attribute);
		vertexLayout.TryGetAttribute(VertexAttributeSemantic.Normal, out var attribute2);
		vertexLayout.TryGetAttribute(VertexAttributeSemantic.Color, out var attribute3);
		vertexLayout.TryGetAttribute(VertexAttributeSemantic.TexCoord0, out var attribute4);
		MeshBounds bounds = MeshBounds.Empty;
		for (int j = 0; j < positions.Length; j++)
		{
			byte* ptr = pointer + j * streamStride;
			Vector3 vector = positions[j];
			bounds = bounds.Encapsulate(vector.X, vector.Y, vector.Z);
			float* ptr2 = (float*)(ptr + attribute.OffsetBytes);
			*ptr2 = vector.X;
			ptr2[1] = vector.Y;
			ptr2[2] = vector.Z;
			ptr2 = (float*)(ptr + attribute2.OffsetBytes);
			*ptr2 = normals[j].X;
			ptr2[1] = normals[j].Y;
			ptr2[2] = normals[j].Z;
			if (colors != null)
			{
				byte* num2 = ptr + attribute3.OffsetBytes;
				*num2 = (byte)System.Math.Clamp((int)MathF.Round(colors[j].X * 255f), 0, 255);
				num2[1] = (byte)System.Math.Clamp((int)MathF.Round(colors[j].Y * 255f), 0, 255);
				num2[2] = (byte)System.Math.Clamp((int)MathF.Round(colors[j].Z * 255f), 0, 255);
				num2[3] = byte.MaxValue;
			}
			ptr2 = (float*)(ptr + attribute4.OffsetBytes);
			*ptr2 = uvs[j].X;
			ptr2[1] = uvs[j].Y;
			ptr2[2] = 0f;
			ptr2[3] = 0f;
		}
		if (indices.Count > 0)
		{
			if (meshIndexFormat == MeshIndexFormat.UInt16)
			{
				ushort* pointer2 = (ushort*)cpuMeshData.GetIndexBuffer().Pointer;
				for (int k = 0; k < indices.Count; k++)
				{
					pointer2[k] = (ushort)indices[k];
				}
			}
			else
			{
				int* pointer3 = (int*)cpuMeshData.GetIndexBuffer().Pointer;
				for (int l = 0; l < indices.Count; l++)
				{
					pointer3[l] = indices[l];
				}
			}
			cpuMeshData.SetSubMesh(0, new SubMesh(0, indices.Count, MeshTopology.Triangles, 0, bounds));
		}
		cpuMeshData.Bounds = bounds;
		return cpuMeshData;
	}

	private static int ReadArbitraryVector(ReadOnlySpan<char> span, out float[] values)
	{
		values = new float[6];
		int result = 0;
		int num;
		for (int i = 0; i < span.Length; i += num + 1)
		{
			for (; i < span.Length && char.IsWhiteSpace(span[i]); i++)
			{
			}
			if (i >= span.Length)
			{
				break;
			}
			num = FindWhitespace(span.Slice(i));
			if (num < 0)
			{
				num = span.Length - i;
			}
			values[result++] = ParseFloat(span.Slice(i, num));
		}
		return result;
	}

	private static Vector3 ReadVector3(ReadOnlySpan<char> span)
	{
		float[] array = new float[3];
		int num = 0;
		int num2;
		for (int i = 0; i < span.Length; i += num2 + 1)
		{
			if (num >= 3)
			{
				break;
			}
			for (; i < span.Length && char.IsWhiteSpace(span[i]); i++)
			{
			}
			if (i >= span.Length)
			{
				break;
			}
			num2 = FindWhitespace(span.Slice(i));
			if (num2 < 0)
			{
				num2 = span.Length - i;
			}
			array[num++] = ParseFloat(span.Slice(i, num2));
		}
		return new Vector3(array[0], array[1], array[2]);
	}

	private static Vector2 ReadVector2(ReadOnlySpan<char> span)
	{
		float[] array = new float[2];
		int num = 0;
		int num2;
		for (int i = 0; i < span.Length; i += num2 + 1)
		{
			if (num >= 2)
			{
				break;
			}
			for (; i < span.Length && char.IsWhiteSpace(span[i]); i++)
			{
			}
			if (i >= span.Length)
			{
				break;
			}
			num2 = FindWhitespace(span.Slice(i));
			if (num2 < 0)
			{
				num2 = span.Length - i;
			}
			array[num++] = ParseFloat(span.Slice(i, num2));
		}
		return new Vector2(array[0], array[1]);
	}

	private static void AddFaceIndices(List<FaceIndex> destination, List<Vector3> vertices, ReadOnlySpan<char> span)
	{
		List<FaceIndex> list = new List<FaceIndex>(4);
		int num;
		for (int i = 0; i < span.Length; i += num + 1)
		{
			for (; i < span.Length && char.IsWhiteSpace(span[i]); i++)
			{
			}
			if (i >= span.Length)
			{
				break;
			}
			num = FindWhitespace(span.Slice(i));
			if (num < 0)
			{
				num = span.Length - i;
			}
			list.Add(ParseFaceVertex(span.Slice(i, num)));
		}
		if (list.Count >= 3)
		{
			if (list.Count == 3)
			{
				EmitTriangle(destination, list, 0, 1, 2);
			}
			else
			{
				TriangulatePolygon(destination, vertices, list);
			}
		}
	}

	private static void TriangulatePolygon(List<FaceIndex> destination, List<Vector3> vertices, List<FaceIndex> parsed)
	{
		int count = parsed.Count;
		Vector3 zero = Vector3.Zero;
		for (int i = 0; i < count; i++)
		{
			Vector3 vector = vertices[parsed[i].V - 1];
			Vector3 vector2 = vertices[parsed[(i + 1) % count].V - 1];
			zero.X += (vector.Y - vector2.Y) * (vector.Z + vector2.Z);
			zero.Y += (vector.Z - vector2.Z) * (vector.X + vector2.X);
			zero.Z += (vector.X - vector2.X) * (vector.Y + vector2.Y);
		}
		if (zero.LengthSquared() < 1E-20f)
		{
			FanTriangulate(destination, parsed);
			return;
		}
		zero = Vector3.Normalize(zero);
		Vector3 vector3 = Vector3.Normalize((MathF.Abs(zero.X) > 0.9f) ? new Vector3(zero.Y, 0f - zero.X, 0f) : new Vector3(0f, zero.Z, 0f - zero.Y));
		Vector3 vector4 = Vector3.Cross(zero, vector3);
		Vector2[] array = new Vector2[count];
		for (int j = 0; j < count; j++)
		{
			Vector3 vector5 = vertices[parsed[j].V - 1];
			array[j] = new Vector2(Vector3.Dot(vector5, vector3), Vector3.Dot(vector5, vector4));
		}
		float num = 0f;
		for (int k = 0; k < count; k++)
		{
			Vector2 vector6 = array[k];
			Vector2 vector7 = array[(k + 1) % count];
			num += vector6.X * vector7.Y - vector7.X * vector6.Y;
		}
		float winding = ((num >= 0f) ? 1f : (-1f));
		List<int> list = new List<int>(count);
		for (int l = 0; l < count; l++)
		{
			list.Add(l);
		}
		int num2 = count * count;
		while (list.Count > 3 && num2-- > 0)
		{
			bool flag = false;
			int count2 = list.Count;
			for (int m = 0; m < count2; m++)
			{
				int num3 = list[(m + count2 - 1) % count2];
				int num4 = list[m];
				int num5 = list[(m + 1) % count2];
				if (IsEar(array, list, num3, num4, num5, winding))
				{
					EmitTriangle(destination, parsed, num3, num4, num5);
					list.RemoveAt(m);
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				break;
			}
		}
		for (int n = 1; n < list.Count - 1; n++)
		{
			EmitTriangle(destination, parsed, list[0], list[n], list[n + 1]);
		}
	}

	private static bool IsEar(Vector2[] points, List<int> ring, int prev, int cur, int next, float winding)
	{
		Vector2 a = points[prev];
		Vector2 b = points[cur];
		Vector2 c = points[next];
		if (((b.X - a.X) * (c.Y - a.Y) - (b.Y - a.Y) * (c.X - a.X)) * winding <= 0f)
		{
			return false;
		}
		foreach (int item in ring)
		{
			if (item != prev && item != cur && item != next && PointInTriangle(points[item], a, b, c))
			{
				return false;
			}
		}
		return true;
	}

	private static bool PointInTriangle(Vector2 p, Vector2 a, Vector2 b, Vector2 c)
	{
		float num = EdgeSign(p, a, b);
		float num2 = EdgeSign(p, b, c);
		float num3 = EdgeSign(p, c, a);
		bool flag = num < 0f || num2 < 0f || num3 < 0f;
		bool flag2 = num > 0f || num2 > 0f || num3 > 0f;
		return !(flag && flag2);
		static float EdgeSign(Vector2 p1, Vector2 p2, Vector2 p3)
		{
			return (p1.X - p3.X) * (p2.Y - p3.Y) - (p2.X - p3.X) * (p1.Y - p3.Y);
		}
	}

	private static void FanTriangulate(List<FaceIndex> destination, List<FaceIndex> parsed)
	{
		for (int i = 1; i < parsed.Count - 1; i++)
		{
			EmitTriangle(destination, parsed, 0, i, i + 1);
		}
	}

	private static void EmitTriangle(List<FaceIndex> destination, List<FaceIndex> parsed, int a, int b, int c)
	{
		destination.Add(parsed[c]);
		destination.Add(parsed[b]);
		destination.Add(parsed[a]);
	}

	private static FaceIndex ParseFaceVertex(ReadOnlySpan<char> span)
	{
		FaceIndex result = default(FaceIndex);
		int num = 0;
		for (int i = 0; i < 3; i++)
		{
			if (num > span.Length)
			{
				break;
			}
			int num2 = span.Slice(num).IndexOf('/');
			ReadOnlySpan<char> s;
			if (num2 < 0)
			{
				s = span.Slice(num);
				num = span.Length + 1;
			}
			else
			{
				s = span.Slice(num, num2);
				num += num2 + 1;
			}
			if (!s.IsEmpty && int.TryParse(s, NumberStyles.Integer, CultureInfo.InvariantCulture, out var result2))
			{
				switch (i)
				{
				case 0:
					result.V = result2;
					break;
				case 1:
					result.T = result2;
					break;
				case 2:
					result.N = result2;
					break;
				}
			}
		}
		return result;
	}

	private static float ParseFloat(ReadOnlySpan<char> span)
	{
		return float.Parse(span, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture);
	}

	private static int FindWhitespace(ReadOnlySpan<char> span)
	{
		for (int i = 0; i < span.Length; i++)
		{
			if (char.IsWhiteSpace(span[i]))
			{
				return i;
			}
		}
		return -1;
	}
}
