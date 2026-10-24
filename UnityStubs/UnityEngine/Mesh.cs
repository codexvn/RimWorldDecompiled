using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.Rendering;
using UnityStubs.Internal;
using Waylith.Graphics.Meshes;

namespace UnityEngine;

public class Mesh : Object, IDisposable
{
	public struct MeshData
	{
		private readonly WritableMeshData _writable;

		private readonly ReadOnlyMeshData _readOnly;

		private VertexLayout Layout
		{
			get
			{
				if (_writable == null)
				{
					return _readOnly.Layout;
				}
				return _writable.Layout;
			}
		}

		private int VertexCountInternal
		{
			get
			{
				if (_writable == null)
				{
					return _readOnly.VertexCount;
				}
				return _writable.VertexCount;
			}
		}

		private int IndexCountInternal
		{
			get
			{
				if (_writable == null)
				{
					return _readOnly.IndexCount;
				}
				return _writable.IndexCount;
			}
		}

		private MeshIndexFormat IndexFormatInternal
		{
			get
			{
				if (_writable == null)
				{
					return _readOnly.IndexFormat;
				}
				return _writable.IndexFormat;
			}
		}

		public IndexFormat indexFormat => ToUnityIndexFormat(IndexFormatInternal);

		public int vertexCount => VertexCountInternal;

		public int subMeshCount
		{
			set
			{
				_writable?.SetSubMeshCount(value);
			}
		}

		internal MeshData(WritableMeshData writable)
		{
			_writable = writable;
			_readOnly = null;
		}

		internal MeshData(ReadOnlyMeshData readOnly)
		{
			_readOnly = readOnly;
			_writable = null;
		}

		public NativeArray<T0> GetIndexData<T0>() where T0 : struct
		{
			int num = ((_writable != null) ? _writable.GetIndexDataByteLength() : _readOnly.GetIndexDataByteLength());
			nint pointer = ((_writable != null) ? _writable.GetIndexDataPointer() : _readOnly.GetIndexDataPointer());
			int num2 = UnsafeUtility.SizeOf<T0>();
			if (num2 <= 0 || num % num2 != 0)
			{
				throw new InvalidOperationException($"Index buffer byte size {num} is not divisible by element size {num2}.");
			}
			return WrapNativeArray<T0>(pointer, num / num2);
		}

		public void GetNormals(NativeArray<Vector3> outNormals)
		{
			VertexLayout layout = Layout;
			int vertexCountInternal = VertexCountInternal;
			WritableMeshData writable = _writable;
			ReadOnlyMeshData readOnly = _readOnly;
			CopyVector3Attribute(layout, (int stream) => (writable == null) ? readOnly.GetVertexDataPointer(stream) : writable.GetVertexDataPointer(stream), vertexCountInternal, VertexAttributeSemantic.Normal, outNormals);
		}

		public void GetUVs(int channel, NativeArray<Vector2> outUVs)
		{
			VertexAttributeSemantic semantic = channel switch
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
			VertexLayout layout = Layout;
			int vertexCountInternal = VertexCountInternal;
			WritableMeshData writable = _writable;
			ReadOnlyMeshData readOnly = _readOnly;
			CopyVector2Attribute(layout, (int stream) => (writable == null) ? readOnly.GetVertexDataPointer(stream) : writable.GetVertexDataPointer(stream), vertexCountInternal, semantic, outUVs);
		}

		public int GetVertexBufferStride(int stream)
		{
			return Layout.GetStreamStride(stream);
		}

		public NativeArray<T0> GetVertexData<T0>(int stream = 0) where T0 : struct
		{
			int num = ((_writable != null) ? _writable.GetVertexDataByteLength(stream) : _readOnly.GetVertexDataByteLength(stream));
			nint pointer = ((_writable != null) ? _writable.GetVertexDataPointer(stream) : _readOnly.GetVertexDataPointer(stream));
			int num2 = UnsafeUtility.SizeOf<T0>();
			if (num2 <= 0 || num % num2 != 0)
			{
				throw new InvalidOperationException($"Vertex buffer byte size {num} is not divisible by element size {num2}.");
			}
			return WrapNativeArray<T0>(pointer, num / num2);
		}

		public void GetVertices(NativeArray<Vector3> outVertices)
		{
			VertexLayout layout = Layout;
			int vertexCountInternal = VertexCountInternal;
			WritableMeshData writable = _writable;
			ReadOnlyMeshData readOnly = _readOnly;
			CopyVector3Attribute(layout, (int stream) => (writable == null) ? readOnly.GetVertexDataPointer(stream) : writable.GetVertexDataPointer(stream), vertexCountInternal, VertexAttributeSemantic.Position, outVertices);
		}

		public void SetIndexBufferParams(int indexCount, IndexFormat format)
		{
			_writable?.SetIndexBufferParams(indexCount, ToEngineIndexFormat(format));
		}

		public void SetSubMesh(int index, SubMeshDescriptor desc, MeshUpdateFlags flags = MeshUpdateFlags.Default)
		{
			WritableMeshData writable = _writable;
			if (writable != null)
			{
				int indexStart = desc.indexStart;
				int indexCount = desc.indexCount;
				writable.SetSubMesh(index, new SubMesh(indexStart, indexCount, desc.topology switch
				{
					MeshTopology.Lines => Waylith.Graphics.Meshes.MeshTopology.Lines, 
					MeshTopology.LineStrip => Waylith.Graphics.Meshes.MeshTopology.LineStrip, 
					MeshTopology.Points => Waylith.Graphics.Meshes.MeshTopology.Points, 
					MeshTopology.Quads => Waylith.Graphics.Meshes.MeshTopology.Quads, 
					_ => Waylith.Graphics.Meshes.MeshTopology.Triangles, 
				}, desc.baseVertex, FromUnityBounds(desc.bounds)));
			}
		}

		public void SetVertexBufferParams(int vertexCount, params VertexAttributeDescriptor[] attributes)
		{
			if (_writable != null)
			{
				Waylith.Graphics.Meshes.VertexAttribute[] array = new Waylith.Graphics.Meshes.VertexAttribute[(attributes != null) ? attributes.Length : 0];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = new Waylith.Graphics.Meshes.VertexAttribute(ToEngineSemantic(attributes[i].attribute), ToEngineAttributeFormat(attributes[i].format), attributes[i].dimension, attributes[i].stream);
				}
				_writable.SetVertexBufferParams(vertexCount, array);
			}
		}

		public void SetVertexBufferParams(int vertexCount, NativeArray<VertexAttributeDescriptor> attributes)
		{
			Waylith.Graphics.Meshes.VertexAttribute[] array = new Waylith.Graphics.Meshes.VertexAttribute[attributes.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = new Waylith.Graphics.Meshes.VertexAttribute(ToEngineSemantic(attributes[i].attribute), ToEngineAttributeFormat(attributes[i].format), attributes[i].dimension, attributes[i].stream);
			}
			_writable?.SetVertexBufferParams(vertexCount, array);
		}

		private unsafe static void CopyVector3Attribute(VertexLayout layout, Func<int, nint> streamPointer, int vertexCount, VertexAttributeSemantic semantic, NativeArray<Vector3> destination)
		{
			if (layout.TryGetAttribute(semantic, out var attribute))
			{
				int num = Math.Min(vertexCount, destination.Length);
				byte* ptr = (byte*)streamPointer(attribute.Stream);
				int streamStride = layout.GetStreamStride(attribute.Stream);
				int componentSize = GetComponentSize(attribute.Format);
				for (int i = 0; i < num; i++)
				{
					byte* ptr2 = ptr + i * streamStride + attribute.OffsetBytes;
					destination[i] = new Vector3((attribute.Dimension > 0) ? ReadScalar(ptr2, attribute.Format) : 0f, (attribute.Dimension > 1) ? ReadScalar(ptr2 + componentSize, attribute.Format) : 0f, (attribute.Dimension > 2) ? ReadScalar(ptr2 + 2 * componentSize, attribute.Format) : 0f);
				}
			}
		}

		private unsafe static void CopyVector2Attribute(VertexLayout layout, Func<int, nint> streamPointer, int vertexCount, VertexAttributeSemantic semantic, NativeArray<Vector2> destination)
		{
			if (layout.TryGetAttribute(semantic, out var attribute))
			{
				int num = Math.Min(vertexCount, destination.Length);
				byte* ptr = (byte*)streamPointer(attribute.Stream);
				int streamStride = layout.GetStreamStride(attribute.Stream);
				int componentSize = GetComponentSize(attribute.Format);
				for (int i = 0; i < num; i++)
				{
					byte* ptr2 = ptr + i * streamStride + attribute.OffsetBytes;
					destination[i] = new Vector2((attribute.Dimension > 0) ? ReadScalar(ptr2, attribute.Format) : 0f, (attribute.Dimension > 1) ? ReadScalar(ptr2 + componentSize, attribute.Format) : 0f);
				}
			}
		}
	}

	public struct MeshDataArray : IDisposable
	{
		private readonly WritableMeshData[] _writable;

		private readonly ReadOnlyMeshData[] _readOnly;

		public MeshData this[int index]
		{
			get
			{
				if (_writable != null)
				{
					return new MeshData(_writable[index]);
				}
				return new MeshData(_readOnly[index]);
			}
		}

		public int Length
		{
			get
			{
				WritableMeshData[] writable = _writable;
				if (writable == null)
				{
					ReadOnlyMeshData[] readOnly = _readOnly;
					if (readOnly == null)
					{
						return 0;
					}
					return readOnly.Length;
				}
				return writable.Length;
			}
		}

		internal MeshDataArray(WritableMeshData[] writable)
		{
			_writable = writable;
			_readOnly = null;
		}

		internal MeshDataArray(ReadOnlyMeshData[] readOnly)
		{
			_readOnly = readOnly;
			_writable = null;
		}

		internal WritableMeshData GetWritable(int index)
		{
			return _writable[index];
		}

		public void Dispose()
		{
			if (_writable != null)
			{
				for (int i = 0; i < _writable.Length; i++)
				{
					_writable[i]?.Dispose();
				}
			}
			if (_readOnly != null)
			{
				for (int j = 0; j < _readOnly.Length; j++)
				{
					_readOnly[j]?.Dispose();
				}
			}
		}
	}

	private readonly Waylith.Graphics.Meshes.Mesh _mesh;

	private volatile bool _compatCacheLoaded;

	private volatile bool _compatDirty;

	internal object RaycastBvh;

	private Vector3[] _compatVertices = Array.Empty<Vector3>();

	private Vector3[] _compatNormals = Array.Empty<Vector3>();

	private Vector4[] _compatTangents = Array.Empty<Vector4>();

	private Color32[] _compatColors = Array.Empty<Color32>();

	private Vector4[] _compatUv0 = Array.Empty<Vector4>();

	private Vector4[] _compatUv1 = Array.Empty<Vector4>();

	private Vector4[] _compatUv2 = Array.Empty<Vector4>();

	private Vector4[] _compatUv3 = Array.Empty<Vector4>();

	private int[] _compatTriangles = Array.Empty<int>();

	private IndexFormat _compatIndexFormat;

	private Matrix4x4[] _bindposes = Array.Empty<Matrix4x4>();

	private BoneWeight[] _boneWeights = Array.Empty<BoneWeight>();

	private byte[] _bonesPerVertex = Array.Empty<byte>();

	private BoneWeight1[] _allBoneWeights = Array.Empty<BoneWeight1>();

	private bool _unlimitedBoneWeights;

	private NativeArray<Matrix4x4> _nativeBindposes;

	private NativeArray<BoneWeight1> _nativeAllBoneWeights;

	private NativeArray<byte> _nativeBonesPerVertex;

	public Waylith.Graphics.Meshes.Mesh EngineMesh
	{
		get
		{
			FlushCompat();
			return _mesh;
		}
	}

	public bool isReadable => _mesh.IsReadable;

	public Bounds bounds
	{
		get
		{
			FlushCompat();
			return ToUnityBounds(_mesh.Bounds);
		}
		set
		{
			FlushCompat();
			_mesh.Bounds = FromUnityBounds(value);
		}
	}

	public Color[] colors
	{
		get
		{
			EnsureCompatCacheLoaded();
			Color[] array = new Color[_compatColors.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = _compatColors[i];
			}
			return array;
		}
		set
		{
			EnsureCompatCacheLoaded();
			_compatColors = ((value == null) ? Array.Empty<Color32>() : Array.ConvertAll(value, (Converter<Color, Color32>)((Color c) => c)));
			RebuildFromCompat();
		}
	}

	public Color32[] colors32
	{
		get
		{
			EnsureCompatCacheLoaded();
			return (Color32[])_compatColors.Clone();
		}
		set
		{
			EnsureCompatCacheLoaded();
			_compatColors = ((value == null) ? Array.Empty<Color32>() : ((Color32[])value.Clone()));
			RebuildFromCompat();
		}
	}

	public IndexFormat indexFormat
	{
		get
		{
			if (!_compatCacheLoaded)
			{
				return ToUnityIndexFormat(_mesh.CpuStorage.IndexFormat);
			}
			return _compatIndexFormat;
		}
		set
		{
			EnsureCompatCacheLoaded();
			_compatIndexFormat = value;
			RebuildFromCompat();
		}
	}

	public Vector4[] tangents
	{
		get
		{
			EnsureCompatCacheLoaded();
			return (Vector4[])_compatTangents.Clone();
		}
		set
		{
			EnsureCompatCacheLoaded();
			_compatTangents = ((value == null) ? Array.Empty<Vector4>() : ((Vector4[])value.Clone()));
			RebuildFromCompat();
		}
	}

	public Vector3[] normals
	{
		get
		{
			EnsureCompatCacheLoaded();
			return (Vector3[])_compatNormals.Clone();
		}
		set
		{
			EnsureCompatCacheLoaded();
			_compatNormals = ((value == null) ? Array.Empty<Vector3>() : ((Vector3[])value.Clone()));
			RebuildFromCompat();
		}
	}

	public int subMeshCount
	{
		get
		{
			FlushCompat();
			return _mesh.SubMeshCount;
		}
		set
		{
			FlushCompat();
			CpuMeshData cpuMeshData = _mesh.CpuStorage.Clone();
			cpuMeshData.SetSubMeshCount(value);
			_mesh.ReplaceCpuStorage(cpuMeshData);
			ResetCompatCache();
		}
	}

	internal int[] trianglesForReading
	{
		get
		{
			EnsureCompatCacheLoaded();
			return _compatTriangles;
		}
	}

	public int[] triangles
	{
		get
		{
			EnsureCompatCacheLoaded();
			return (int[])_compatTriangles.Clone();
		}
		set
		{
			EnsureCompatCacheLoaded();
			_compatTriangles = ((value == null) ? Array.Empty<int>() : ((int[])value.Clone()));
			if (_compatTriangles.Length != 0 && _compatIndexFormat == IndexFormat.UInt16 && MaxIndex(_compatTriangles) > 65535)
			{
				_compatIndexFormat = IndexFormat.UInt32;
			}
			RebuildFromCompat();
		}
	}

	public Vector2[] uv
	{
		get
		{
			EnsureCompatCacheLoaded();
			return ConvertUvToVector2(_compatUv0);
		}
		set
		{
			EnsureCompatCacheLoaded();
			_compatUv0 = ((value == null) ? Array.Empty<Vector4>() : Array.ConvertAll(value, (Vector2 v) => new Vector4(v.x, v.y, 0f, 0f)));
			RebuildFromCompat();
		}
	}

	public Vector2[] uv2
	{
		get
		{
			EnsureCompatCacheLoaded();
			return ConvertUvToVector2(_compatUv1);
		}
		set
		{
			EnsureCompatCacheLoaded();
			_compatUv1 = ((value == null) ? Array.Empty<Vector4>() : Array.ConvertAll(value, (Vector2 v) => new Vector4(v.x, v.y, 0f, 0f)));
			RebuildFromCompat();
		}
	}

	public Vector2[] uv3
	{
		get
		{
			EnsureCompatCacheLoaded();
			return ConvertUvToVector2(_compatUv2);
		}
		set
		{
			EnsureCompatCacheLoaded();
			_compatUv2 = ((value == null) ? Array.Empty<Vector4>() : Array.ConvertAll(value, (Vector2 v) => new Vector4(v.x, v.y, 0f, 0f)));
			RebuildFromCompat();
		}
	}

	public Vector2[] uv4
	{
		get
		{
			EnsureCompatCacheLoaded();
			return ConvertUvToVector2(_compatUv3);
		}
		set
		{
			EnsureCompatCacheLoaded();
			_compatUv3 = ((value == null) ? Array.Empty<Vector4>() : Array.ConvertAll(value, (Vector2 v) => new Vector4(v.x, v.y, 0f, 0f)));
			RebuildFromCompat();
		}
	}

	public int vertexCount
	{
		get
		{
			if (!_compatCacheLoaded)
			{
				return _mesh.VertexCount;
			}
			return _compatVertices.Length;
		}
	}

	internal Vector3[] verticesForReading
	{
		get
		{
			EnsureCompatCacheLoaded();
			return _compatVertices;
		}
	}

	internal Vector3[] normalsForReading
	{
		get
		{
			EnsureCompatCacheLoaded();
			return _compatNormals;
		}
	}

	public Vector3[] vertices
	{
		get
		{
			EnsureCompatCacheLoaded();
			return (Vector3[])_compatVertices.Clone();
		}
		set
		{
			EnsureCompatCacheLoaded();
			_compatVertices = ((value == null) ? Array.Empty<Vector3>() : ((Vector3[])value.Clone()));
			if (_compatIndexFormat == IndexFormat.UInt16 && _compatVertices.Length > 65535)
			{
				_compatIndexFormat = IndexFormat.UInt32;
			}
			RebuildFromCompat();
		}
	}

	internal Matrix4x4[] bindposesForReading => _bindposes;

	internal BoneWeight[] boneWeightsForReading => _boneWeights;

	internal int SkinDataVersion { get; private set; }

	internal Vector4[] tangentsForReading
	{
		get
		{
			EnsureCompatCacheLoaded();
			return _compatTangents;
		}
	}

	public int bindposeCount => _bindposes.Length;

	public int blendShapeCount => 0;

	public Matrix4x4[] bindposes
	{
		get
		{
			return (Matrix4x4[])_bindposes.Clone();
		}
		set
		{
			_bindposes = ((value == null) ? Array.Empty<Matrix4x4>() : ((Matrix4x4[])value.Clone()));
			SkinDataChanged();
		}
	}

	public BoneWeight[] boneWeights
	{
		get
		{
			return (BoneWeight[])_boneWeights.Clone();
		}
		set
		{
			_boneWeights = ((value == null) ? Array.Empty<BoneWeight>() : ((BoneWeight[])value.Clone()));
			_unlimitedBoneWeights = false;
			_bonesPerVertex = Array.Empty<byte>();
			_allBoneWeights = Array.Empty<BoneWeight1>();
			SkinDataChanged();
		}
	}

	public SkinWeights skinWeightBufferLayout
	{
		get
		{
			if (_boneWeights.Length != 0)
			{
				if (!_unlimitedBoneWeights)
				{
					return SkinWeights.FourBones;
				}
				return SkinWeights.Unlimited;
			}
			return SkinWeights.None;
		}
	}

	public Mesh()
		: this(new Waylith.Graphics.Meshes.Mesh())
	{
	}

	internal Mesh(Waylith.Graphics.Meshes.Mesh core)
	{
		_mesh = core;
		_compatIndexFormat = ToUnityIndexFormat(core.CpuStorage.IndexFormat);
		if (!string.IsNullOrEmpty(core.DebugName))
		{
			base.name = Path.GetFileName(core.DebugName);
		}
	}

	public static MeshDataArray AcquireReadOnlyMeshData(List<Mesh> meshes)
	{
		ArgumentNullException.ThrowIfNull(meshes, "meshes");
		ReadOnlyMeshData[] array = new ReadOnlyMeshData[meshes.Count];
		for (int i = 0; i < meshes.Count; i++)
		{
			array[i] = meshes[i]?.EngineMesh.CreateReadOnlySnapshot() ?? new ReadOnlyMeshData(new CpuMeshData());
		}
		return new MeshDataArray(array);
	}

	public static MeshDataArray AllocateWritableMeshData(int meshCount)
	{
		WritableMeshData[] array = new WritableMeshData[Math.Max(0, meshCount)];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = new WritableMeshData();
		}
		return new MeshDataArray(array);
	}

	public static void ApplyAndDisposeWritableMeshData(MeshDataArray data, Mesh mesh, MeshUpdateFlags flags = MeshUpdateFlags.Default)
	{
		ArgumentNullException.ThrowIfNull(mesh, "mesh");
		if (data.Length != 1)
		{
			throw new InvalidOperationException($"Expected exactly one mesh data item, found {data.Length}.");
		}
		mesh.ApplyWritableMeshData(data.GetWritable(0));
		data.Dispose();
	}

	public static void ApplyAndDisposeWritableMeshData(MeshDataArray data, List<Mesh> meshes, MeshUpdateFlags flags = MeshUpdateFlags.Default)
	{
		ArgumentNullException.ThrowIfNull(meshes, "meshes");
		if (meshes.Count != data.Length)
		{
			throw new InvalidOperationException($"Mesh count {meshes.Count} does not match mesh data count {data.Length}.");
		}
		for (int i = 0; i < meshes.Count; i++)
		{
			if (!(meshes[i] == null))
			{
				meshes[i].ApplyWritableMeshData(data.GetWritable(i));
			}
		}
		data.Dispose();
	}

	public void Clear()
	{
		ResetCompatCache(_compatCacheLoaded ? _compatIndexFormat : ToUnityIndexFormat(_mesh.CpuStorage.IndexFormat));
		_compatCacheLoaded = true;
		_compatDirty = true;
	}

	public void Clear(bool keepVertexLayout)
	{
		Clear();
	}

	public void CombineMeshes(CombineInstance[] combine)
	{
		CombineMeshes(combine, mergeSubMeshes: true);
	}

	public void CombineMeshes(CombineInstance[] combine, bool mergeSubMeshes, bool useMatrices, bool hasLightmapData)
	{
		CombineMeshes(combine, mergeSubMeshes, useMatrices);
	}

	public void CombineMeshes(CombineInstance[] combine, bool mergeSubMeshes, bool useMatrices = true)
	{
		if (combine == null || combine.Length == 0)
		{
			Clear();
			return;
		}
		List<Vector3> list = new List<Vector3>();
		List<Vector3> list2 = new List<Vector3>();
		List<Vector2> list3 = new List<Vector2>();
		List<Color> list4 = new List<Color>();
		List<int> list5 = new List<int>();
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		for (int i = 0; i < combine.Length; i++)
		{
			CombineInstance combineInstance = combine[i];
			Mesh mesh = combineInstance.mesh;
			if (mesh == null)
			{
				continue;
			}
			Vector3[] array = mesh.vertices;
			if (array == null || array.Length == 0)
			{
				continue;
			}
			int count = list.Count;
			Matrix4x4 matrix4x = (useMatrices ? combineInstance.transform : Matrix4x4.identity);
			for (int j = 0; j < array.Length; j++)
			{
				list.Add(matrix4x.MultiplyPoint3x4(array[j]));
			}
			Vector3[] array2 = mesh.normals;
			if (array2 != null && array2.Length == array.Length)
			{
				flag = true;
				for (int k = 0; k < array2.Length; k++)
				{
					list2.Add(matrix4x.MultiplyVector(array2[k]).normalized);
				}
			}
			else
			{
				for (int l = 0; l < array.Length; l++)
				{
					list2.Add(Vector3.up);
				}
			}
			Vector2[] array3 = mesh.uv;
			if (array3 != null && array3.Length == array.Length)
			{
				flag2 = true;
				list3.AddRange(array3);
			}
			else
			{
				for (int m = 0; m < array.Length; m++)
				{
					list3.Add(default(Vector2));
				}
			}
			Color[] array4 = mesh.colors;
			if (array4 != null && array4.Length == array.Length)
			{
				flag3 = true;
				list4.AddRange(array4);
			}
			else
			{
				for (int n = 0; n < array.Length; n++)
				{
					list4.Add(Color.white);
				}
			}
			int[] array5 = mesh.triangles;
			if (array5 != null)
			{
				for (int num = 0; num < array5.Length; num++)
				{
					list5.Add(array5[num] + count);
				}
			}
		}
		Clear();
		if (list.Count != 0)
		{
			vertices = list.ToArray();
			if (flag)
			{
				normals = list2.ToArray();
			}
			if (flag2)
			{
				uv = list3.ToArray();
			}
			if (flag3)
			{
				colors = list4.ToArray();
			}
			SetTriangles(list5.ToArray(), 0);
			RecalculateBounds();
		}
	}

	public void GetTangents(List<Vector4> tangents)
	{
		ArgumentNullException.ThrowIfNull(tangents, "tangents");
		EnsureCompatCacheLoaded();
		tangents.Clear();
		tangents.AddRange(_compatTangents);
	}

	public void SetTangents(List<Vector4> inTangents)
	{
		tangents = inTangents?.ToArray();
	}

	public void SetTangents(Vector4[] inTangents)
	{
		tangents = inTangents;
	}

	public uint GetBaseVertex(int submesh)
	{
		return (uint)GetSubMeshOrDefault(submesh).BaseVertex;
	}

	public void GetColors(List<Color32> colors)
	{
		ArgumentNullException.ThrowIfNull(colors, "colors");
		EnsureCompatCacheLoaded();
		colors.Clear();
		colors.AddRange(_compatColors);
	}

	public uint GetIndexCount(int submesh)
	{
		return (uint)GetSubMeshOrDefault(submesh).IndexCount;
	}

	public uint GetIndexStart(int submesh)
	{
		return (uint)GetSubMeshOrDefault(submesh).IndexStart;
	}

	public void GetNormals(List<Vector3> normals)
	{
		ArgumentNullException.ThrowIfNull(normals, "normals");
		EnsureCompatCacheLoaded();
		normals.Clear();
		normals.AddRange(_compatNormals);
	}

	public void GetTriangles(List<int> triangles, int submesh)
	{
		ArgumentNullException.ThrowIfNull(triangles, "triangles");
		triangles.Clear();
		triangles.AddRange(GetSubMeshTriangles(submesh));
	}

	public int[] GetIndices(int submesh)
	{
		return GetSubMeshTriangles(submesh);
	}

	public MeshTopology GetTopology(int submesh)
	{
		return MeshTopology.Triangles;
	}

	public void GetUVs(int channel, List<Vector2> uvs)
	{
		ArgumentNullException.ThrowIfNull(uvs, "uvs");
		uvs.Clear();
		Vector4[] compatUv = GetCompatUv(channel);
		for (int i = 0; i < compatUv.Length; i++)
		{
			Vector4 vector = compatUv[i];
			uvs.Add(new Vector2(vector.x, vector.y));
		}
	}

	public void GetUVs(int channel, List<Vector4> uvs)
	{
		ArgumentNullException.ThrowIfNull(uvs, "uvs");
		uvs.Clear();
		uvs.AddRange(GetCompatUv(channel));
	}

	public int GetVertexAttributes(List<VertexAttributeDescriptor> attributes)
	{
		ArgumentNullException.ThrowIfNull(attributes, "attributes");
		attributes.Clear();
		FlushCompat();
		ReadOnlySpan<Waylith.Graphics.Meshes.VertexAttribute> attributes2 = _mesh.CpuStorage.Layout.Attributes;
		for (int i = 0; i < attributes2.Length; i++)
		{
			Waylith.Graphics.Meshes.VertexAttribute vertexAttribute = attributes2[i];
			attributes.Add(new VertexAttributeDescriptor(ToUnityAttribute(vertexAttribute.Semantic), ToUnityAttributeFormat(vertexAttribute.Format), vertexAttribute.Dimension, vertexAttribute.Stream));
		}
		return attributes.Count;
	}

	public void GetVertices(List<Vector3> vertices)
	{
		ArgumentNullException.ThrowIfNull(vertices, "vertices");
		EnsureCompatCacheLoaded();
		vertices.Clear();
		vertices.AddRange(_compatVertices);
	}

	public bool HasVertexAttribute(UnityEngine.Rendering.VertexAttribute attr)
	{
		FlushCompat();
		return _mesh.CpuStorage.Layout.HasAttribute(ToEngineSemantic(attr));
	}

	public void MarkDynamic()
	{
		_mesh.MarkDynamic();
	}

	public void Optimize()
	{
		StubWarner.LogStub("Optimize", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\Mesh.cs", 557);
	}

	public void RecalculateBounds()
	{
		EnsureCompatCacheLoaded();
		if (!_compatDirty)
		{
			_mesh.Bounds = CalculateBounds(_compatVertices);
		}
	}

	public void RecalculateNormals()
	{
		EnsureCompatCacheLoaded();
		if (_compatVertices.Length == 0 || _compatTriangles.Length == 0)
		{
			return;
		}
		Vector3[] array = new Vector3[_compatVertices.Length];
		for (int i = 0; i + 2 < _compatTriangles.Length; i += 3)
		{
			int num = _compatTriangles[i];
			int num2 = _compatTriangles[i + 1];
			int num3 = _compatTriangles[i + 2];
			if ((uint)num < (uint)_compatVertices.Length && (uint)num2 < (uint)_compatVertices.Length && (uint)num3 < (uint)_compatVertices.Length)
			{
				Vector3 vector = _compatVertices[num];
				Vector3 vector2 = _compatVertices[num2];
				Vector3 vector3 = Vector3.Cross(rhs: _compatVertices[num3] - vector, lhs: vector2 - vector);
				array[num] += vector3;
				array[num2] += vector3;
				array[num3] += vector3;
			}
		}
		for (int j = 0; j < array.Length; j++)
		{
			array[j].Normalize();
		}
		_compatNormals = array;
		RebuildFromCompat();
	}

	public void SetColors(List<Color> inColors)
	{
		ArgumentNullException.ThrowIfNull(inColors, "inColors");
		EnsureCompatCacheLoaded();
		_compatColors = new Color32[inColors.Count];
		for (int i = 0; i < inColors.Count; i++)
		{
			_compatColors[i] = inColors[i];
		}
		RebuildFromCompat();
	}

	public void SetColors(Color[] inColors)
	{
		ArgumentNullException.ThrowIfNull(inColors, "inColors");
		EnsureCompatCacheLoaded();
		_compatColors = new Color32[inColors.Length];
		for (int i = 0; i < inColors.Length; i++)
		{
			_compatColors[i] = inColors[i];
		}
		RebuildFromCompat();
	}

	public void SetColors(Color32[] inColors)
	{
		ArgumentNullException.ThrowIfNull(inColors, "inColors");
		EnsureCompatCacheLoaded();
		_compatColors = (Color32[])inColors.Clone();
		RebuildFromCompat();
	}

	public void SetIndices(int[] indices, MeshTopology topology, int submesh)
	{
		SetIndices(indices, topology, submesh, calculateBounds: true, 0);
	}

	public void SetIndices(int[] indices, MeshTopology topology, int submesh, bool calculateBounds, int baseVertex)
	{
		if (topology != 0)
		{
			StubWarner.LogStub("SetIndices", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\Mesh.cs", 634);
		}
		SetTriangles(indices, 0, (indices != null) ? indices.Length : 0, submesh, calculateBounds, baseVertex);
	}

	public void SetIndices(List<int> indices, MeshTopology topology, int submesh, bool calculateBounds = true, int baseVertex = 0)
	{
		SetIndices(indices?.ToArray() ?? Array.Empty<int>(), topology, submesh, calculateBounds, baseVertex);
	}

	public void SetIndices(List<ushort> indices, MeshTopology topology, int submesh, bool calculateBounds = true, int baseVertex = 0)
	{
		int[] array = new int[indices?.Count ?? 0];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = indices[i];
		}
		SetIndices(array, topology, submesh, calculateBounds, baseVertex);
	}

	public void SetColors(List<Color32> inColors)
	{
		ArgumentNullException.ThrowIfNull(inColors, "inColors");
		EnsureCompatCacheLoaded();
		_compatColors = inColors.ToArray();
		RebuildFromCompat();
	}

	public void SetNormals(List<Vector3> inNormals)
	{
		ArgumentNullException.ThrowIfNull(inNormals, "inNormals");
		EnsureCompatCacheLoaded();
		_compatNormals = inNormals.ToArray();
		RebuildFromCompat();
	}

	public void SetNormals(List<Vector3> inNormals, int start, int length)
	{
		SetNormals(inNormals, start, length, MeshUpdateFlags.Default);
	}

	public void SetNormals(List<Vector3> inNormals, int start, int length, MeshUpdateFlags flags)
	{
		SetNormals(CollectionsMarshal.AsSpan(inNormals ?? throw new ArgumentNullException("inNormals")).Slice(start, length).ToArray());
	}

	public void SetNormals(Vector3[] inNormals)
	{
		normals = inNormals ?? throw new ArgumentNullException("inNormals");
	}

	public void SetNormals(Vector3[] inNormals, int start, int length)
	{
		SetNormals(inNormals, start, length, MeshUpdateFlags.Default);
	}

	public void SetNormals(Vector3[] inNormals, int start, int length, MeshUpdateFlags flags)
	{
		normals = (inNormals ?? throw new ArgumentNullException("inNormals")).AsSpan(start, length).ToArray();
	}

	public void SetColors(List<Color32> inColors, int start, int length)
	{
		SetColors(inColors, start, length, MeshUpdateFlags.Default);
	}

	public void SetColors(List<Color32> inColors, int start, int length, MeshUpdateFlags flags)
	{
		colors32 = CollectionsMarshal.AsSpan(inColors ?? throw new ArgumentNullException("inColors")).Slice(start, length).ToArray();
	}

	public void SetColors(Color32[] inColors, int start, int length)
	{
		SetColors(inColors, start, length, MeshUpdateFlags.Default);
	}

	public void SetColors(Color32[] inColors, int start, int length, MeshUpdateFlags flags)
	{
		colors32 = (inColors ?? throw new ArgumentNullException("inColors")).AsSpan(start, length).ToArray();
	}

	public void SetColors(List<Color> inColors, int start, int length)
	{
		SetColors(inColors, start, length, MeshUpdateFlags.Default);
	}

	public void SetColors(List<Color> inColors, int start, int length, MeshUpdateFlags flags)
	{
		SetColors(new List<Color>(CollectionsMarshal.AsSpan(inColors ?? throw new ArgumentNullException("inColors")).Slice(start, length).ToArray()));
	}

	public void SetColors(Color[] inColors, int start, int length)
	{
		SetColors(inColors, start, length, MeshUpdateFlags.Default);
	}

	public void SetColors(Color[] inColors, int start, int length, MeshUpdateFlags flags)
	{
		SetColors((inColors ?? throw new ArgumentNullException("inColors")).AsSpan(start, length).ToArray());
	}

	public void SetUVs(int channel, List<Vector2> uvs, int start, int length)
	{
		SetUVs(channel, uvs, start, length, MeshUpdateFlags.Default);
	}

	public void SetUVs(int channel, List<Vector2> uvs, int start, int length, MeshUpdateFlags flags)
	{
		SetUVs(channel, CollectionsMarshal.AsSpan(uvs ?? throw new ArgumentNullException("uvs")).Slice(start, length).ToArray());
	}

	public void SetUVs(int channel, List<Vector3> uvs, int start, int length)
	{
		SetUVs(channel, uvs, start, length, MeshUpdateFlags.Default);
	}

	public void SetUVs(int channel, List<Vector3> uvs, int start, int length, MeshUpdateFlags flags)
	{
		SetUVs(channel, CollectionsMarshal.AsSpan(uvs ?? throw new ArgumentNullException("uvs")).Slice(start, length).ToArray());
	}

	public void SetUVs(int channel, List<Vector4> uvs, int start, int length)
	{
		SetUVs(channel, uvs, start, length, MeshUpdateFlags.Default);
	}

	public void SetUVs(int channel, List<Vector4> uvs, int start, int length, MeshUpdateFlags flags)
	{
		SetUVs(channel, CollectionsMarshal.AsSpan(uvs ?? throw new ArgumentNullException("uvs")).Slice(start, length).ToArray());
	}

	public void SetUVs(int channel, Vector2[] uvs, int start, int length, MeshUpdateFlags flags)
	{
		SetUVs(channel, uvs, start, length);
	}

	public void SetUVs(int channel, Vector3[] uvs, int start, int length, MeshUpdateFlags flags)
	{
		SetUVs(channel, uvs, start, length);
	}

	public void SetUVs(int channel, Vector4[] uvs, int start, int length, MeshUpdateFlags flags)
	{
		SetUVs(channel, uvs, start, length);
	}

	public void SetTriangles(int[] triangles, int submesh)
	{
		SetTriangles(triangles, 0, (triangles != null) ? triangles.Length : 0, submesh);
	}

	public void SetTriangles(int[] triangles, int trianglesStart, int trianglesLength, int submesh, bool calculateBounds = true, int baseVertex = 0)
	{
		if (submesh != 0 || baseVertex != 0)
		{
			int[] trianglesLocal = ((triangles == null) ? Array.Empty<int>() : new ArraySegment<int>(triangles, trianglesStart, trianglesLength).ToArray());
			ApplySingleSubMeshTriangles(trianglesLocal, submesh, baseVertex);
			if (calculateBounds)
			{
				RecalculateBounds();
			}
			return;
		}
		EnsureCompatCacheLoaded();
		if (triangles == null)
		{
			_compatTriangles = Array.Empty<int>();
		}
		else
		{
			_compatTriangles = new int[trianglesLength];
			Array.Copy(triangles, trianglesStart, _compatTriangles, 0, trianglesLength);
		}
		if (_compatIndexFormat == IndexFormat.UInt16 && MaxIndex(_compatTriangles) > 65535)
		{
			_compatIndexFormat = IndexFormat.UInt32;
		}
		RebuildFromCompat();
		if (calculateBounds)
		{
			RecalculateBounds();
		}
	}

	public void SetTriangles(List<int> triangles, int submesh)
	{
		SetTriangles(triangles?.ToArray(), submesh);
	}

	public void SetTriangles(List<int> triangles, int submesh, bool calculateBounds)
	{
		SetTriangles(triangles, submesh, calculateBounds, 0);
	}

	public void SetTriangles(List<int> triangles, int submesh, bool calculateBounds, int baseVertex)
	{
		int[] array = triangles?.ToArray();
		SetTriangles(array, 0, (array != null) ? array.Length : 0, submesh, calculateBounds, baseVertex);
	}

	public void SetTriangles(int[] triangles, int submesh, bool calculateBounds)
	{
		SetTriangles(triangles, 0, (triangles != null) ? triangles.Length : 0, submesh, calculateBounds);
	}

	public void SetTriangles(int[] triangles, int submesh, bool calculateBounds, int baseVertex)
	{
		SetTriangles(triangles, 0, (triangles != null) ? triangles.Length : 0, submesh, calculateBounds, baseVertex);
	}

	public void SetUVs(int channel, List<Vector2> uvs)
	{
		ArgumentNullException.ThrowIfNull(uvs, "uvs");
		EnsureCompatCacheLoaded();
		Vector4[] array = new Vector4[uvs.Count];
		for (int i = 0; i < uvs.Count; i++)
		{
			array[i] = new Vector4(uvs[i].x, uvs[i].y, 0f, 0f);
		}
		SetCompatUv(channel, array);
		RebuildFromCompat();
	}

	public void SetUVs(int channel, List<Vector3> uvs)
	{
		ArgumentNullException.ThrowIfNull(uvs, "uvs");
		EnsureCompatCacheLoaded();
		Vector4[] array = new Vector4[uvs.Count];
		for (int i = 0; i < uvs.Count; i++)
		{
			array[i] = new Vector4(uvs[i].x, uvs[i].y, uvs[i].z, 0f);
		}
		SetCompatUv(channel, array);
		RebuildFromCompat();
	}

	public void SetUVs(int channel, List<Vector4> uvs)
	{
		ArgumentNullException.ThrowIfNull(uvs, "uvs");
		EnsureCompatCacheLoaded();
		SetCompatUv(channel, uvs.ToArray());
		RebuildFromCompat();
	}

	public void SetUVs(int channel, Vector2[] uvs)
	{
		SetUVs(channel, uvs, 0, (uvs != null) ? uvs.Length : 0);
	}

	public void SetUVs(int channel, Vector3[] uvs)
	{
		SetUVs(channel, uvs, 0, (uvs != null) ? uvs.Length : 0);
	}

	public void SetUVs(int channel, Vector4[] uvs)
	{
		SetUVs(channel, uvs, 0, (uvs != null) ? uvs.Length : 0);
	}

	public void SetUVs(int channel, Vector2[] uvs, int start, int length)
	{
		ArgumentNullException.ThrowIfNull(uvs, "uvs");
		SetUVs(channel, new List<Vector2>(new ArraySegment<Vector2>(uvs, start, length)));
	}

	public void SetUVs(int channel, Vector3[] uvs, int start, int length)
	{
		ArgumentNullException.ThrowIfNull(uvs, "uvs");
		SetUVs(channel, new List<Vector3>(new ArraySegment<Vector3>(uvs, start, length)));
	}

	public void SetUVs(int channel, Vector4[] uvs, int start, int length)
	{
		ArgumentNullException.ThrowIfNull(uvs, "uvs");
		EnsureCompatCacheLoaded();
		SetCompatUv(channel, uvs.AsSpan(start, length).ToArray());
		RebuildFromCompat();
	}

	public void SetVertices(List<Vector3> inVertices)
	{
		ArgumentNullException.ThrowIfNull(inVertices, "inVertices");
		EnsureCompatCacheLoaded();
		_compatVertices = inVertices.ToArray();
		if (_compatIndexFormat == IndexFormat.UInt16 && _compatVertices.Length > 65535)
		{
			_compatIndexFormat = IndexFormat.UInt32;
		}
		RebuildFromCompat();
	}

	public void SetVertices(Vector3[] inVertices)
	{
		ArgumentNullException.ThrowIfNull(inVertices, "inVertices");
		EnsureCompatCacheLoaded();
		_compatVertices = (Vector3[])inVertices.Clone();
		if (_compatIndexFormat == IndexFormat.UInt16 && _compatVertices.Length > 65535)
		{
			_compatIndexFormat = IndexFormat.UInt32;
		}
		RebuildFromCompat();
	}

	public void SetVertices(Vector3[] inVertices, int start, int length)
	{
		ArgumentNullException.ThrowIfNull(inVertices, "inVertices");
		SetVertices(inVertices.AsSpan(start, length).ToArray());
	}

	public void SetVertices(List<Vector3> inVertices, int start, int length)
	{
		ArgumentNullException.ThrowIfNull(inVertices, "inVertices");
		SetVertices(inVertices.GetRange(start, length));
	}

	public void UploadMeshData(bool markNoLongerReadable)
	{
		FlushCompat();
		UpdateDebugName();
		_mesh.Upload(markNoLongerReadable);
		if (markNoLongerReadable)
		{
			DiscardCompatCache();
		}
	}

	public void Dispose()
	{
		_mesh.Dispose();
	}

	private void ApplyWritableMeshData(WritableMeshData data)
	{
		CpuMeshData storage = data.DetachStorage();
		_mesh.ReplaceCpuStorage(storage);
		UpdateDebugName();
		ResetCompatCache();
	}

	protected override void OnNameChanged(string newName)
	{
		UpdateDebugName();
	}

	private void ResetCompatCache()
	{
		ResetCompatCache(ToUnityIndexFormat(_mesh.CpuStorage.IndexFormat));
	}

	private void ResetCompatCache(IndexFormat indexFormat)
	{
		_compatCacheLoaded = false;
		_compatDirty = false;
		_compatVertices = Array.Empty<Vector3>();
		_compatNormals = Array.Empty<Vector3>();
		_compatTangents = Array.Empty<Vector4>();
		_compatColors = Array.Empty<Color32>();
		_compatUv0 = Array.Empty<Vector4>();
		_compatUv1 = Array.Empty<Vector4>();
		_compatUv2 = Array.Empty<Vector4>();
		_compatUv3 = Array.Empty<Vector4>();
		_compatTriangles = Array.Empty<int>();
		_compatIndexFormat = indexFormat;
	}

	private void DiscardCompatCache()
	{
		_compatCacheLoaded = false;
		_compatDirty = false;
		_compatVertices = Array.Empty<Vector3>();
		_compatNormals = Array.Empty<Vector3>();
		_compatTangents = Array.Empty<Vector4>();
		_compatColors = Array.Empty<Color32>();
		_compatUv0 = Array.Empty<Vector4>();
		_compatUv1 = Array.Empty<Vector4>();
		_compatUv2 = Array.Empty<Vector4>();
		_compatUv3 = Array.Empty<Vector4>();
		_compatTriangles = Array.Empty<int>();
	}

	private void EnsureCompatCacheLoaded()
	{
		EnsureReadable();
		if (!_compatCacheLoaded)
		{
			CpuMeshData cpuStorage = _mesh.CpuStorage;
			_compatVertices = DecodeVector3Attribute(cpuStorage, VertexAttributeSemantic.Position);
			_compatNormals = DecodeVector3Attribute(cpuStorage, VertexAttributeSemantic.Normal);
			_compatTangents = DecodeVector4Attribute(cpuStorage, VertexAttributeSemantic.Tangent);
			_compatColors = DecodeColorAttribute(cpuStorage, VertexAttributeSemantic.Color);
			_compatUv0 = DecodeVector4Attribute(cpuStorage, VertexAttributeSemantic.TexCoord0);
			_compatUv1 = DecodeVector4Attribute(cpuStorage, VertexAttributeSemantic.TexCoord1);
			_compatUv2 = DecodeVector4Attribute(cpuStorage, VertexAttributeSemantic.TexCoord2);
			_compatUv3 = DecodeVector4Attribute(cpuStorage, VertexAttributeSemantic.TexCoord3);
			_compatTriangles = GetSubMeshTriangles(0);
			_compatIndexFormat = ToUnityIndexFormat(cpuStorage.IndexFormat);
			_compatCacheLoaded = true;
		}
	}

	private void EnsureReadable()
	{
		if (!_mesh.IsReadable)
		{
			throw new InvalidOperationException("Mesh '" + base.name + "' is not readable.");
		}
	}

	private void RebuildFromCompat()
	{
		_compatCacheLoaded = true;
		_compatDirty = true;
	}

	private void FlushCompat()
	{
		if (_compatDirty)
		{
			_compatDirty = false;
			RebuildEngineStorage();
		}
	}

	private unsafe void RebuildEngineStorage()
	{
		CpuMeshData cpuMeshData = new CpuMeshData();
		int num = _compatVertices.Length;
		if (num == 0)
		{
			_mesh.ReplaceCpuStorage(cpuMeshData);
			UpdateDebugName();
			_compatCacheLoaded = true;
			return;
		}
		List<Waylith.Graphics.Meshes.VertexAttribute> list = new List<Waylith.Graphics.Meshes.VertexAttribute>(7)
		{
			new Waylith.Graphics.Meshes.VertexAttribute(VertexAttributeSemantic.Position, Waylith.Graphics.Meshes.VertexAttributeFormat.Float32, 3)
		};
		bool flag = _compatNormals.Length == num;
		bool flag2 = _compatTangents.Length == num;
		bool flag3 = _compatColors.Length == num;
		bool flag4 = _compatUv0.Length == num;
		bool flag5 = _compatUv1.Length == num;
		bool flag6 = _compatUv2.Length == num;
		bool flag7 = _compatUv3.Length == num;
		if (flag)
		{
			list.Add(new Waylith.Graphics.Meshes.VertexAttribute(VertexAttributeSemantic.Normal, Waylith.Graphics.Meshes.VertexAttributeFormat.Float32, 3));
		}
		if (flag2)
		{
			list.Add(new Waylith.Graphics.Meshes.VertexAttribute(VertexAttributeSemantic.Tangent, Waylith.Graphics.Meshes.VertexAttributeFormat.Float32, 4));
		}
		if (flag3)
		{
			list.Add(new Waylith.Graphics.Meshes.VertexAttribute(VertexAttributeSemantic.Color, Waylith.Graphics.Meshes.VertexAttributeFormat.UNorm8, 4));
		}
		if (flag4)
		{
			list.Add(new Waylith.Graphics.Meshes.VertexAttribute(VertexAttributeSemantic.TexCoord0, Waylith.Graphics.Meshes.VertexAttributeFormat.Float32, 4));
		}
		if (flag5)
		{
			list.Add(new Waylith.Graphics.Meshes.VertexAttribute(VertexAttributeSemantic.TexCoord1, Waylith.Graphics.Meshes.VertexAttributeFormat.Float32, 4));
		}
		if (flag6)
		{
			list.Add(new Waylith.Graphics.Meshes.VertexAttribute(VertexAttributeSemantic.TexCoord2, Waylith.Graphics.Meshes.VertexAttributeFormat.Float32, 4));
		}
		if (flag7)
		{
			list.Add(new Waylith.Graphics.Meshes.VertexAttribute(VertexAttributeSemantic.TexCoord3, Waylith.Graphics.Meshes.VertexAttributeFormat.Float32, 4));
		}
		VertexLayout vertexLayout = new VertexLayout(list);
		cpuMeshData.SetVertexLayout(vertexLayout, num);
		cpuMeshData.SetIndexBufferParams(_compatTriangles.Length, ToEngineIndexFormat(_compatIndexFormat));
		cpuMeshData.SetSubMeshCount((_compatTriangles.Length != 0) ? 1 : 0);
		byte* pointer = (byte*)cpuMeshData.GetVertexStream(0).Pointer;
		int streamStride = vertexLayout.GetStreamStride(0);
		vertexLayout.TryGetAttribute(VertexAttributeSemantic.Position, out var attribute);
		vertexLayout.TryGetAttribute(VertexAttributeSemantic.Normal, out var attribute2);
		vertexLayout.TryGetAttribute(VertexAttributeSemantic.Tangent, out var attribute3);
		vertexLayout.TryGetAttribute(VertexAttributeSemantic.Color, out var attribute4);
		vertexLayout.TryGetAttribute(VertexAttributeSemantic.TexCoord0, out var attribute5);
		vertexLayout.TryGetAttribute(VertexAttributeSemantic.TexCoord1, out var attribute6);
		vertexLayout.TryGetAttribute(VertexAttributeSemantic.TexCoord2, out var attribute7);
		vertexLayout.TryGetAttribute(VertexAttributeSemantic.TexCoord3, out var attribute8);
		for (int i = 0; i < num; i++)
		{
			byte* ptr = pointer + i * streamStride;
			WriteVector3(ptr + attribute.OffsetBytes, _compatVertices[i], attribute.Format, attribute.Dimension);
			if (flag)
			{
				WriteVector3(ptr + attribute2.OffsetBytes, _compatNormals[i], attribute2.Format, attribute2.Dimension);
			}
			if (flag2)
			{
				WriteVector4(ptr + attribute3.OffsetBytes, _compatTangents[i], attribute3.Format, attribute3.Dimension);
			}
			if (flag3)
			{
				WriteColor32(ptr + attribute4.OffsetBytes, _compatColors[i], attribute4.Format, attribute4.Dimension);
			}
			if (flag4)
			{
				WriteVector4(ptr + attribute5.OffsetBytes, _compatUv0[i], attribute5.Format, attribute5.Dimension);
			}
			if (flag5)
			{
				WriteVector4(ptr + attribute6.OffsetBytes, _compatUv1[i], attribute6.Format, attribute6.Dimension);
			}
			if (flag6)
			{
				WriteVector4(ptr + attribute7.OffsetBytes, _compatUv2[i], attribute7.Format, attribute7.Dimension);
			}
			if (flag7)
			{
				WriteVector4(ptr + attribute8.OffsetBytes, _compatUv3[i], attribute8.Format, attribute8.Dimension);
			}
		}
		if (_compatTriangles.Length != 0)
		{
			if (_compatIndexFormat == IndexFormat.UInt16)
			{
				ushort* pointer2 = (ushort*)cpuMeshData.GetIndexBuffer().Pointer;
				for (int j = 0; j < _compatTriangles.Length; j++)
				{
					pointer2[j] = (ushort)_compatTriangles[j];
				}
			}
			else
			{
				int* pointer3 = (int*)cpuMeshData.GetIndexBuffer().Pointer;
				for (int k = 0; k < _compatTriangles.Length; k++)
				{
					pointer3[k] = _compatTriangles[k];
				}
			}
			cpuMeshData.SetSubMesh(0, new SubMesh(0, _compatTriangles.Length, Waylith.Graphics.Meshes.MeshTopology.Triangles, 0, CalculateBounds(_compatVertices)));
		}
		cpuMeshData.Bounds = CalculateBounds(_compatVertices);
		_mesh.ReplaceCpuStorage(cpuMeshData);
		UpdateDebugName();
		_compatCacheLoaded = true;
	}

	private unsafe void ApplySingleSubMeshTriangles(int[] trianglesLocal, int submesh, int baseVertex)
	{
		FlushCompat();
		CpuMeshData cpuMeshData = _mesh.CpuStorage.Clone();
		int num = Math.Max(cpuMeshData.SubMeshes.Length, submesh + 1);
		cpuMeshData.SetSubMeshCount(num);
		List<int[]> list = new List<int[]>(num);
		for (int i = 0; i < num; i++)
		{
			list.Add(GetSubMeshTriangles(i));
		}
		list[submesh] = trianglesLocal;
		int num2 = 0;
		for (int j = 0; j < list.Count; j++)
		{
			num2 += list[j].Length;
		}
		MeshIndexFormat meshIndexFormat = cpuMeshData.IndexFormat;
		if (meshIndexFormat == MeshIndexFormat.UInt16 && MaxIndex(list) > 65535)
		{
			meshIndexFormat = MeshIndexFormat.UInt32;
		}
		cpuMeshData.SetIndexBufferParams(num2, meshIndexFormat);
		int num3 = 0;
		if (meshIndexFormat == MeshIndexFormat.UInt16)
		{
			ushort* pointer = (ushort*)cpuMeshData.GetIndexBuffer().Pointer;
			for (int k = 0; k < list.Count; k++)
			{
				int[] array = list[k];
				for (int l = 0; l < array.Length; l++)
				{
					pointer[num3 + l] = (ushort)(array[l] + ((k == submesh) ? baseVertex : 0));
				}
				cpuMeshData.SetSubMesh(k, new SubMesh(num3, array.Length, Waylith.Graphics.Meshes.MeshTopology.Triangles, (k == submesh) ? baseVertex : 0));
				num3 += array.Length;
			}
		}
		else
		{
			int* pointer2 = (int*)cpuMeshData.GetIndexBuffer().Pointer;
			for (int m = 0; m < list.Count; m++)
			{
				int[] array2 = list[m];
				for (int n = 0; n < array2.Length; n++)
				{
					pointer2[num3 + n] = array2[n] + ((m == submesh) ? baseVertex : 0);
				}
				cpuMeshData.SetSubMesh(m, new SubMesh(num3, array2.Length, Waylith.Graphics.Meshes.MeshTopology.Triangles, (m == submesh) ? baseVertex : 0));
				num3 += array2.Length;
			}
		}
		_mesh.ReplaceCpuStorage(cpuMeshData);
		UpdateDebugName();
		ResetCompatCache();
	}

	private void UpdateDebugName()
	{
		_mesh.DebugName = (string.IsNullOrWhiteSpace(base.name) ? $"Mesh#{GetInstanceID()}" : base.name);
	}

	private static Vector2[] ConvertUvToVector2(Vector4[] source)
	{
		Vector2[] array = new Vector2[source.Length];
		for (int i = 0; i < source.Length; i++)
		{
			array[i] = new Vector2(source[i].x, source[i].y);
		}
		return array;
	}

	private Vector4[] GetCompatUv(int channel)
	{
		EnsureCompatCacheLoaded();
		return channel switch
		{
			0 => _compatUv0, 
			1 => _compatUv1, 
			2 => _compatUv2, 
			3 => _compatUv3, 
			_ => Array.Empty<Vector4>(), 
		};
	}

	private void SetCompatUv(int channel, Vector4[] values)
	{
		switch (channel)
		{
		case 0:
			_compatUv0 = values;
			break;
		case 1:
			_compatUv1 = values;
			break;
		case 2:
			_compatUv2 = values;
			break;
		case 3:
			_compatUv3 = values;
			break;
		}
	}

	private unsafe int[] GetSubMeshTriangles(int submesh)
	{
		FlushCompat();
		CpuMeshData cpuStorage = _mesh.CpuStorage;
		if ((uint)submesh >= (uint)cpuStorage.SubMeshes.Length)
		{
			return Array.Empty<int>();
		}
		SubMesh subMesh = cpuStorage.SubMeshes[submesh];
		int[] array = new int[subMesh.IndexCount];
		if (cpuStorage.IndexFormat == MeshIndexFormat.UInt16)
		{
			ushort* pointer = (ushort*)cpuStorage.GetIndexBuffer().Pointer;
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = pointer[subMesh.IndexStart + i];
			}
		}
		else
		{
			int* pointer2 = (int*)cpuStorage.GetIndexBuffer().Pointer;
			for (int j = 0; j < array.Length; j++)
			{
				array[j] = pointer2[subMesh.IndexStart + j];
			}
		}
		return array;
	}

	private SubMesh GetSubMeshOrDefault(int submesh)
	{
		FlushCompat();
		CpuMeshData cpuStorage = _mesh.CpuStorage;
		if ((uint)submesh >= (uint)cpuStorage.SubMeshes.Length)
		{
			return default(SubMesh);
		}
		return cpuStorage.SubMeshes[submesh];
	}

	private static int MaxIndex(int[] indices)
	{
		int num = 0;
		for (int i = 0; i < indices.Length; i++)
		{
			num = Math.Max(num, indices[i]);
		}
		return num;
	}

	private static int MaxIndex(List<int[]> subMeshIndices)
	{
		int num = 0;
		for (int i = 0; i < subMeshIndices.Count; i++)
		{
			num = Math.Max(num, MaxIndex(subMeshIndices[i]));
		}
		return num;
	}

	private static MeshBounds CalculateBounds(Vector3[] positions)
	{
		if (positions == null || positions.Length == 0)
		{
			return default(MeshBounds);
		}
		MeshBounds result = MeshBounds.Empty;
		for (int i = 0; i < positions.Length; i++)
		{
			result = result.Encapsulate(positions[i].x, positions[i].y, positions[i].z);
		}
		return result;
	}

	private static Bounds ToUnityBounds(MeshBounds bounds)
	{
		if (bounds.IsEmpty)
		{
			return default(Bounds);
		}
		Bounds result = default(Bounds);
		result.SetMinMax(new Vector3(bounds.MinX, bounds.MinY, bounds.MinZ), new Vector3(bounds.MaxX, bounds.MaxY, bounds.MaxZ));
		return result;
	}

	private static MeshBounds FromUnityBounds(Bounds bounds)
	{
		Vector3 min = bounds.min;
		Vector3 max = bounds.max;
		return new MeshBounds(min.x, min.y, min.z, max.x, max.y, max.z);
	}

	private static MeshIndexFormat ToEngineIndexFormat(IndexFormat format)
	{
		if (format != IndexFormat.UInt32)
		{
			return MeshIndexFormat.UInt16;
		}
		return MeshIndexFormat.UInt32;
	}

	private static IndexFormat ToUnityIndexFormat(MeshIndexFormat format)
	{
		if (format != MeshIndexFormat.UInt32)
		{
			return IndexFormat.UInt16;
		}
		return IndexFormat.UInt32;
	}

	private static VertexAttributeSemantic ToEngineSemantic(UnityEngine.Rendering.VertexAttribute attribute)
	{
		return attribute switch
		{
			UnityEngine.Rendering.VertexAttribute.Position => VertexAttributeSemantic.Position, 
			UnityEngine.Rendering.VertexAttribute.Normal => VertexAttributeSemantic.Normal, 
			UnityEngine.Rendering.VertexAttribute.Tangent => VertexAttributeSemantic.Tangent, 
			UnityEngine.Rendering.VertexAttribute.Color => VertexAttributeSemantic.Color, 
			UnityEngine.Rendering.VertexAttribute.TexCoord0 => VertexAttributeSemantic.TexCoord0, 
			UnityEngine.Rendering.VertexAttribute.TexCoord1 => VertexAttributeSemantic.TexCoord1, 
			UnityEngine.Rendering.VertexAttribute.TexCoord2 => VertexAttributeSemantic.TexCoord2, 
			UnityEngine.Rendering.VertexAttribute.TexCoord3 => VertexAttributeSemantic.TexCoord3, 
			UnityEngine.Rendering.VertexAttribute.TexCoord4 => VertexAttributeSemantic.TexCoord4, 
			UnityEngine.Rendering.VertexAttribute.TexCoord5 => VertexAttributeSemantic.TexCoord5, 
			UnityEngine.Rendering.VertexAttribute.TexCoord6 => VertexAttributeSemantic.TexCoord6, 
			UnityEngine.Rendering.VertexAttribute.TexCoord7 => VertexAttributeSemantic.TexCoord7, 
			UnityEngine.Rendering.VertexAttribute.BlendWeight => VertexAttributeSemantic.BlendWeight, 
			UnityEngine.Rendering.VertexAttribute.BlendIndices => VertexAttributeSemantic.BlendIndices, 
			_ => VertexAttributeSemantic.Position, 
		};
	}

	private static UnityEngine.Rendering.VertexAttribute ToUnityAttribute(VertexAttributeSemantic semantic)
	{
		return semantic switch
		{
			VertexAttributeSemantic.Position => UnityEngine.Rendering.VertexAttribute.Position, 
			VertexAttributeSemantic.Normal => UnityEngine.Rendering.VertexAttribute.Normal, 
			VertexAttributeSemantic.Tangent => UnityEngine.Rendering.VertexAttribute.Tangent, 
			VertexAttributeSemantic.Color => UnityEngine.Rendering.VertexAttribute.Color, 
			VertexAttributeSemantic.TexCoord0 => UnityEngine.Rendering.VertexAttribute.TexCoord0, 
			VertexAttributeSemantic.TexCoord1 => UnityEngine.Rendering.VertexAttribute.TexCoord1, 
			VertexAttributeSemantic.TexCoord2 => UnityEngine.Rendering.VertexAttribute.TexCoord2, 
			VertexAttributeSemantic.TexCoord3 => UnityEngine.Rendering.VertexAttribute.TexCoord3, 
			VertexAttributeSemantic.TexCoord4 => UnityEngine.Rendering.VertexAttribute.TexCoord4, 
			VertexAttributeSemantic.TexCoord5 => UnityEngine.Rendering.VertexAttribute.TexCoord5, 
			VertexAttributeSemantic.TexCoord6 => UnityEngine.Rendering.VertexAttribute.TexCoord6, 
			VertexAttributeSemantic.TexCoord7 => UnityEngine.Rendering.VertexAttribute.TexCoord7, 
			VertexAttributeSemantic.BlendWeight => UnityEngine.Rendering.VertexAttribute.BlendWeight, 
			VertexAttributeSemantic.BlendIndices => UnityEngine.Rendering.VertexAttribute.BlendIndices, 
			_ => UnityEngine.Rendering.VertexAttribute.Position, 
		};
	}

	private static Waylith.Graphics.Meshes.VertexAttributeFormat ToEngineAttributeFormat(UnityEngine.Rendering.VertexAttributeFormat format)
	{
		return format switch
		{
			UnityEngine.Rendering.VertexAttributeFormat.Float32 => Waylith.Graphics.Meshes.VertexAttributeFormat.Float32, 
			UnityEngine.Rendering.VertexAttributeFormat.Float16 => Waylith.Graphics.Meshes.VertexAttributeFormat.Float16, 
			UnityEngine.Rendering.VertexAttributeFormat.UNorm8 => Waylith.Graphics.Meshes.VertexAttributeFormat.UNorm8, 
			UnityEngine.Rendering.VertexAttributeFormat.SNorm8 => Waylith.Graphics.Meshes.VertexAttributeFormat.SNorm8, 
			UnityEngine.Rendering.VertexAttributeFormat.UNorm16 => Waylith.Graphics.Meshes.VertexAttributeFormat.UNorm16, 
			UnityEngine.Rendering.VertexAttributeFormat.SNorm16 => Waylith.Graphics.Meshes.VertexAttributeFormat.SNorm16, 
			UnityEngine.Rendering.VertexAttributeFormat.UInt8 => Waylith.Graphics.Meshes.VertexAttributeFormat.UInt8, 
			UnityEngine.Rendering.VertexAttributeFormat.SInt8 => Waylith.Graphics.Meshes.VertexAttributeFormat.SInt8, 
			UnityEngine.Rendering.VertexAttributeFormat.UInt16 => Waylith.Graphics.Meshes.VertexAttributeFormat.UInt16, 
			UnityEngine.Rendering.VertexAttributeFormat.SInt16 => Waylith.Graphics.Meshes.VertexAttributeFormat.SInt16, 
			UnityEngine.Rendering.VertexAttributeFormat.UInt32 => Waylith.Graphics.Meshes.VertexAttributeFormat.UInt32, 
			UnityEngine.Rendering.VertexAttributeFormat.SInt32 => Waylith.Graphics.Meshes.VertexAttributeFormat.SInt32, 
			_ => Waylith.Graphics.Meshes.VertexAttributeFormat.Float32, 
		};
	}

	private static UnityEngine.Rendering.VertexAttributeFormat ToUnityAttributeFormat(Waylith.Graphics.Meshes.VertexAttributeFormat format)
	{
		return format switch
		{
			Waylith.Graphics.Meshes.VertexAttributeFormat.Float32 => UnityEngine.Rendering.VertexAttributeFormat.Float32, 
			Waylith.Graphics.Meshes.VertexAttributeFormat.Float16 => UnityEngine.Rendering.VertexAttributeFormat.Float16, 
			Waylith.Graphics.Meshes.VertexAttributeFormat.UNorm8 => UnityEngine.Rendering.VertexAttributeFormat.UNorm8, 
			Waylith.Graphics.Meshes.VertexAttributeFormat.SNorm8 => UnityEngine.Rendering.VertexAttributeFormat.SNorm8, 
			Waylith.Graphics.Meshes.VertexAttributeFormat.UNorm16 => UnityEngine.Rendering.VertexAttributeFormat.UNorm16, 
			Waylith.Graphics.Meshes.VertexAttributeFormat.SNorm16 => UnityEngine.Rendering.VertexAttributeFormat.SNorm16, 
			Waylith.Graphics.Meshes.VertexAttributeFormat.UInt8 => UnityEngine.Rendering.VertexAttributeFormat.UInt8, 
			Waylith.Graphics.Meshes.VertexAttributeFormat.SInt8 => UnityEngine.Rendering.VertexAttributeFormat.SInt8, 
			Waylith.Graphics.Meshes.VertexAttributeFormat.UInt16 => UnityEngine.Rendering.VertexAttributeFormat.UInt16, 
			Waylith.Graphics.Meshes.VertexAttributeFormat.SInt16 => UnityEngine.Rendering.VertexAttributeFormat.SInt16, 
			Waylith.Graphics.Meshes.VertexAttributeFormat.UInt32 => UnityEngine.Rendering.VertexAttributeFormat.UInt32, 
			Waylith.Graphics.Meshes.VertexAttributeFormat.SInt32 => UnityEngine.Rendering.VertexAttributeFormat.SInt32, 
			_ => UnityEngine.Rendering.VertexAttributeFormat.Float32, 
		};
	}

	private unsafe static float ReadScalar(byte* ptr, Waylith.Graphics.Meshes.VertexAttributeFormat format)
	{
		return format switch
		{
			Waylith.Graphics.Meshes.VertexAttributeFormat.Float32 => *(float*)ptr, 
			Waylith.Graphics.Meshes.VertexAttributeFormat.Float16 => (float)(*(Half*)ptr), 
			Waylith.Graphics.Meshes.VertexAttributeFormat.UNorm8 => (float)(int)(*ptr) / 255f, 
			Waylith.Graphics.Meshes.VertexAttributeFormat.SNorm8 => Math.Max((float)(int)(*ptr) / 127f, -1f), 
			Waylith.Graphics.Meshes.VertexAttributeFormat.UNorm16 => (float)(int)(*(ushort*)ptr) / 65535f, 
			Waylith.Graphics.Meshes.VertexAttributeFormat.SNorm16 => Math.Max((float)(*(short*)ptr) / 32767f, -1f), 
			Waylith.Graphics.Meshes.VertexAttributeFormat.UInt8 => (int)(*ptr), 
			Waylith.Graphics.Meshes.VertexAttributeFormat.SInt8 => (int)(*ptr), 
			Waylith.Graphics.Meshes.VertexAttributeFormat.UInt16 => (int)(*(ushort*)ptr), 
			Waylith.Graphics.Meshes.VertexAttributeFormat.SInt16 => *(short*)ptr, 
			Waylith.Graphics.Meshes.VertexAttributeFormat.UInt32 => *(uint*)ptr, 
			Waylith.Graphics.Meshes.VertexAttributeFormat.SInt32 => *(int*)ptr, 
			_ => 0f, 
		};
	}

	private static int GetComponentSize(Waylith.Graphics.Meshes.VertexAttributeFormat format)
	{
		return VertexLayout.GetComponentByteSize(format);
	}

	private unsafe static Vector3[] DecodeVector3Attribute(CpuMeshData storage, VertexAttributeSemantic semantic)
	{
		if (!storage.Layout.TryGetAttribute(semantic, out var attribute) || storage.VertexCount == 0)
		{
			return Array.Empty<Vector3>();
		}
		Vector3[] array = new Vector3[storage.VertexCount];
		byte* pointer = (byte*)storage.GetVertexStream(attribute.Stream).Pointer;
		int streamStride = storage.Layout.GetStreamStride(attribute.Stream);
		int componentSize = GetComponentSize(attribute.Format);
		for (int i = 0; i < storage.VertexCount; i++)
		{
			byte* ptr = pointer + i * streamStride + attribute.OffsetBytes;
			array[i] = new Vector3((attribute.Dimension > 0) ? ReadScalar(ptr, attribute.Format) : 0f, (attribute.Dimension > 1) ? ReadScalar(ptr + componentSize, attribute.Format) : 0f, (attribute.Dimension > 2) ? ReadScalar(ptr + 2 * componentSize, attribute.Format) : 0f);
		}
		return array;
	}

	private unsafe static Vector4[] DecodeVector4Attribute(CpuMeshData storage, VertexAttributeSemantic semantic)
	{
		if (!storage.Layout.TryGetAttribute(semantic, out var attribute) || storage.VertexCount == 0)
		{
			return Array.Empty<Vector4>();
		}
		Vector4[] array = new Vector4[storage.VertexCount];
		byte* pointer = (byte*)storage.GetVertexStream(attribute.Stream).Pointer;
		int streamStride = storage.Layout.GetStreamStride(attribute.Stream);
		int componentSize = GetComponentSize(attribute.Format);
		for (int i = 0; i < storage.VertexCount; i++)
		{
			byte* ptr = pointer + i * streamStride + attribute.OffsetBytes;
			array[i] = new Vector4((attribute.Dimension > 0) ? ReadScalar(ptr, attribute.Format) : 0f, (attribute.Dimension > 1) ? ReadScalar(ptr + componentSize, attribute.Format) : 0f, (attribute.Dimension > 2) ? ReadScalar(ptr + 2 * componentSize, attribute.Format) : 0f, (attribute.Dimension > 3) ? ReadScalar(ptr + 3 * componentSize, attribute.Format) : 0f);
		}
		return array;
	}

	private static Color32[] DecodeColorAttribute(CpuMeshData storage, VertexAttributeSemantic semantic)
	{
		if (!storage.Layout.TryGetAttribute(semantic, out var attribute) || storage.VertexCount == 0)
		{
			return Array.Empty<Color32>();
		}
		Vector4[] array = DecodeVector4Attribute(storage, semantic);
		Color32[] array2 = new Color32[array.Length];
		for (int i = 0; i < array.Length; i++)
		{
			Vector4 vector = array[i];
			array2[i] = new Color32(ClampToByte(vector.x * 255f), ClampToByte(vector.y * 255f), ClampToByte(vector.z * 255f), ClampToByte(((attribute.Dimension > 3) ? vector.w : 1f) * 255f));
		}
		return array2;
	}

	private unsafe static void WriteVector3(byte* ptr, Vector3 value, Waylith.Graphics.Meshes.VertexAttributeFormat format, int dimension)
	{
		WriteScalar(ptr, value.x, format);
		if (dimension > 1)
		{
			WriteScalar(ptr + GetComponentSize(format), value.y, format);
		}
		if (dimension > 2)
		{
			WriteScalar(ptr + GetComponentSize(format) * 2, value.z, format);
		}
	}

	private unsafe static void WriteVector4(byte* ptr, Vector4 value, Waylith.Graphics.Meshes.VertexAttributeFormat format, int dimension)
	{
		WriteScalar(ptr, value.x, format);
		if (dimension > 1)
		{
			WriteScalar(ptr + GetComponentSize(format), value.y, format);
		}
		if (dimension > 2)
		{
			WriteScalar(ptr + GetComponentSize(format) * 2, value.z, format);
		}
		if (dimension > 3)
		{
			WriteScalar(ptr + GetComponentSize(format) * 3, value.w, format);
		}
	}

	private unsafe static void WriteColor32(byte* ptr, Color32 value, Waylith.Graphics.Meshes.VertexAttributeFormat format, int dimension)
	{
		if (format == Waylith.Graphics.Meshes.VertexAttributeFormat.UNorm8 || format == Waylith.Graphics.Meshes.VertexAttributeFormat.UInt8)
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

	private unsafe static void WriteScalar(byte* ptr, float value, Waylith.Graphics.Meshes.VertexAttributeFormat format)
	{
		switch (format)
		{
		case Waylith.Graphics.Meshes.VertexAttributeFormat.Float32:
			*(float*)ptr = value;
			break;
		case Waylith.Graphics.Meshes.VertexAttributeFormat.Float16:
			*(Half*)ptr = (Half)value;
			break;
		case Waylith.Graphics.Meshes.VertexAttributeFormat.UNorm8:
		case Waylith.Graphics.Meshes.VertexAttributeFormat.UInt8:
			*ptr = ClampToByte(value * ((format == Waylith.Graphics.Meshes.VertexAttributeFormat.UNorm8) ? 255f : 1f));
			break;
		case Waylith.Graphics.Meshes.VertexAttributeFormat.UNorm16:
		case Waylith.Graphics.Meshes.VertexAttributeFormat.UInt16:
			*(ushort*)ptr = (ushort)Math.Clamp((int)MathF.Round(value * ((format == Waylith.Graphics.Meshes.VertexAttributeFormat.UNorm16) ? 65535f : 1f)), 0, 65535);
			break;
		case Waylith.Graphics.Meshes.VertexAttributeFormat.SInt8:
			*ptr = (byte)(sbyte)Math.Clamp((int)MathF.Round(value), -128, 127);
			break;
		case Waylith.Graphics.Meshes.VertexAttributeFormat.SInt16:
			*(short*)ptr = (short)Math.Clamp((int)MathF.Round(value), -32768, 32767);
			break;
		case Waylith.Graphics.Meshes.VertexAttributeFormat.UInt32:
			*(int*)ptr = Math.Max(0, (int)MathF.Round(value));
			break;
		case Waylith.Graphics.Meshes.VertexAttributeFormat.SInt32:
			*(int*)ptr = (int)MathF.Round(value);
			break;
		case Waylith.Graphics.Meshes.VertexAttributeFormat.SNorm8:
			*ptr = (byte)(sbyte)Math.Clamp((int)MathF.Round(Math.Clamp(value, -1f, 1f) * 127f), -128, 127);
			break;
		case Waylith.Graphics.Meshes.VertexAttributeFormat.SNorm16:
			*(short*)ptr = (short)Math.Clamp((int)MathF.Round(Math.Clamp(value, -1f, 1f) * 32767f), -32768, 32767);
			break;
		}
	}

	private static byte ClampToByte(float value)
	{
		return (byte)Math.Clamp((int)MathF.Round(value), 0, 255);
	}

	private unsafe static NativeArray<T> WrapNativeArray<T>(nint pointer, int length) where T : struct
	{
		return new NativeArray<T>((void*)pointer, length, Allocator.None, ownsBuffer: false);
	}

	public void SetBoneWeights(NativeArray<byte> bonesPerVertex, NativeArray<BoneWeight1> weights)
	{
		_bonesPerVertex = bonesPerVertex.ToArray();
		_allBoneWeights = weights.ToArray();
		_unlimitedBoneWeights = true;
		_boneWeights = new BoneWeight[_bonesPerVertex.Length];
		int num = 0;
		for (int i = 0; i < _bonesPerVertex.Length; i++)
		{
			byte b = _bonesPerVertex[i];
			int num2 = Math.Min((int)b, 4);
			float num3 = 0f;
			for (int j = 0; j < num2 && num + j < _allBoneWeights.Length; j++)
			{
				num3 += _allBoneWeights[num + j].weight;
			}
			BoneWeight boneWeight = default(BoneWeight);
			for (int k = 0; k < num2 && num + k < _allBoneWeights.Length; k++)
			{
				BoneWeight1 boneWeight2 = _allBoneWeights[num + k];
				float num4 = ((num3 > 0f) ? (boneWeight2.weight / num3) : 0f);
				switch (k)
				{
				case 0:
					boneWeight.weight0 = num4;
					boneWeight.boneIndex0 = boneWeight2.boneIndex;
					break;
				case 1:
					boneWeight.weight1 = num4;
					boneWeight.boneIndex1 = boneWeight2.boneIndex;
					break;
				case 2:
					boneWeight.weight2 = num4;
					boneWeight.boneIndex2 = boneWeight2.boneIndex;
					break;
				default:
					boneWeight.weight3 = num4;
					boneWeight.boneIndex3 = boneWeight2.boneIndex;
					break;
				}
			}
			_boneWeights[i] = boneWeight;
			num += b;
		}
		SkinDataChanged();
	}

	public NativeArray<BoneWeight1> GetAllBoneWeights()
	{
		if (_nativeAllBoneWeights.IsCreated)
		{
			return _nativeAllBoneWeights;
		}
		if (!_unlimitedBoneWeights)
		{
			FlattenBoneWeights();
		}
		return _nativeAllBoneWeights = new NativeArray<BoneWeight1>(_allBoneWeights, Allocator.Persistent);
	}

	public NativeArray<byte> GetBonesPerVertex()
	{
		if (_nativeBonesPerVertex.IsCreated)
		{
			return _nativeBonesPerVertex;
		}
		if (!_unlimitedBoneWeights)
		{
			FlattenBoneWeights();
		}
		return _nativeBonesPerVertex = new NativeArray<byte>(_bonesPerVertex, Allocator.Persistent);
	}

	public NativeArray<Matrix4x4> GetBindposes()
	{
		if (!_nativeBindposes.IsCreated)
		{
			_nativeBindposes = new NativeArray<Matrix4x4>(_bindposes, Allocator.Persistent);
		}
		return _nativeBindposes;
	}

	public void GetBindposes(List<Matrix4x4> bindposes)
	{
		if (bindposes == null)
		{
			throw new ArgumentNullException("bindposes", "The result bindposes list cannot be null.");
		}
		bindposes.Clear();
		bindposes.AddRange(_bindposes);
	}

	public void GetBoneWeights(List<BoneWeight> boneWeights)
	{
		if (boneWeights == null)
		{
			throw new ArgumentNullException("boneWeights", "The result boneWeights list cannot be null.");
		}
		boneWeights.Clear();
		boneWeights.AddRange(_boneWeights);
	}

	private void FlattenBoneWeights()
	{
		_bonesPerVertex = new byte[_boneWeights.Length];
		List<BoneWeight1> list = new List<BoneWeight1>(_boneWeights.Length * 4);
		for (int i = 0; i < _boneWeights.Length; i++)
		{
			BoneWeight boneWeight = _boneWeights[i];
			int count = list.Count;
			if (boneWeight.weight0 > 0f)
			{
				list.Add(new BoneWeight1
				{
					weight = boneWeight.weight0,
					boneIndex = boneWeight.boneIndex0
				});
			}
			if (boneWeight.weight1 > 0f)
			{
				list.Add(new BoneWeight1
				{
					weight = boneWeight.weight1,
					boneIndex = boneWeight.boneIndex1
				});
			}
			if (boneWeight.weight2 > 0f)
			{
				list.Add(new BoneWeight1
				{
					weight = boneWeight.weight2,
					boneIndex = boneWeight.boneIndex2
				});
			}
			if (boneWeight.weight3 > 0f)
			{
				list.Add(new BoneWeight1
				{
					weight = boneWeight.weight3,
					boneIndex = boneWeight.boneIndex3
				});
			}
			if (list.Count == count)
			{
				list.Add(new BoneWeight1
				{
					weight = 0f,
					boneIndex = 0
				});
			}
			_bonesPerVertex[i] = (byte)(list.Count - count);
		}
		_allBoneWeights = list.ToArray();
	}

	private void SkinDataChanged()
	{
		SkinDataVersion++;
		if (_nativeBindposes.IsCreated)
		{
			_nativeBindposes.Dispose();
		}
		if (_nativeAllBoneWeights.IsCreated)
		{
			_nativeAllBoneWeights.Dispose();
		}
		if (_nativeBonesPerVertex.IsCreated)
		{
			_nativeBonesPerVertex.Dispose();
		}
		_nativeBindposes = default(NativeArray<Matrix4x4>);
		_nativeAllBoneWeights = default(NativeArray<BoneWeight1>);
		_nativeBonesPerVertex = default(NativeArray<byte>);
	}

	internal static Mesh FromAssetData(string name, Vector3[] vertices, Vector3[] normals, Vector4[] tangents, Color32[] colors, Vector4[][] uvs, int[][] subMeshTriangles, bool index32)
	{
		Mesh mesh = new Mesh
		{
			name = name
		};
		mesh._compatVertices = vertices;
		mesh._compatNormals = normals;
		mesh._compatTangents = tangents;
		mesh._compatColors = colors;
		mesh._compatUv0 = uvs[0];
		mesh._compatUv1 = uvs[1];
		mesh._compatUv2 = uvs[2];
		mesh._compatUv3 = uvs[3];
		mesh._compatIndexFormat = ((index32 || vertices.Length > 65535) ? IndexFormat.UInt32 : IndexFormat.UInt16);
		mesh._compatTriangles = ((subMeshTriangles.Length != 0) ? subMeshTriangles[0] : Array.Empty<int>());
		mesh.RebuildFromCompat();
		for (int i = 1; i < subMeshTriangles.Length; i++)
		{
			mesh.ApplySingleSubMeshTriangles(subMeshTriangles[i], i, 0);
		}
		return mesh;
	}

	internal Mesh CloneForSkinning()
	{
		FlushCompat();
		Mesh mesh = new Mesh(new Waylith.Graphics.Meshes.Mesh(_mesh.CpuStorage.Clone()));
		mesh.name = base.name + " (skinned)";
		mesh.MarkDynamic();
		return mesh;
	}

	internal unsafe void WriteSkinned(Vector3[] positions, Vector3[] normals, Vector4[] tangents)
	{
		FlushCompat();
		CpuMeshData cpuStorage = _mesh.CpuStorage;
		VertexLayout layout = cpuStorage.Layout;
		int num = Math.Min(positions.Length, cpuStorage.VertexCount);
		if (layout.TryGetAttribute(VertexAttributeSemantic.Position, out var attribute))
		{
			byte* pointer = (byte*)cpuStorage.GetVertexStream(attribute.Stream).Pointer;
			int streamStride = layout.GetStreamStride(attribute.Stream);
			for (int i = 0; i < num; i++)
			{
				WriteVector3(pointer + i * streamStride + attribute.OffsetBytes, positions[i], attribute.Format, attribute.Dimension);
			}
		}
		if (normals.Length >= num && layout.TryGetAttribute(VertexAttributeSemantic.Normal, out attribute))
		{
			byte* pointer2 = (byte*)cpuStorage.GetVertexStream(attribute.Stream).Pointer;
			int streamStride2 = layout.GetStreamStride(attribute.Stream);
			for (int j = 0; j < num; j++)
			{
				WriteVector3(pointer2 + j * streamStride2 + attribute.OffsetBytes, normals[j], attribute.Format, attribute.Dimension);
			}
		}
		if (tangents.Length >= num && layout.TryGetAttribute(VertexAttributeSemantic.Tangent, out attribute))
		{
			byte* pointer3 = (byte*)cpuStorage.GetVertexStream(attribute.Stream).Pointer;
			int streamStride3 = layout.GetStreamStride(attribute.Stream);
			for (int k = 0; k < num; k++)
			{
				WriteVector4(pointer3 + k * streamStride3 + attribute.OffsetBytes, tangents[k], attribute.Format, attribute.Dimension);
			}
		}
		cpuStorage.Bounds = CalculateBounds(positions);
		ResetCompatCache();
		_mesh.MarkCpuStorageChanged();
	}
}
