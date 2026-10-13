using System;
using System.Numerics;

namespace Waylith.Graphics.Meshes;

public struct MeshData
{
	private readonly WritableMeshData? _writable;

	private readonly ReadOnlyMeshData? _readOnly;

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

	public MeshIndexFormat indexFormat
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

	public unsafe MeshBufferView<T> GetIndexData<T>() where T : unmanaged
	{
		int num = ((_writable != null) ? _writable.GetIndexDataByteLength() : _readOnly.GetIndexDataByteLength());
		nint pointer = ((_writable != null) ? _writable.GetIndexDataPointer() : _readOnly.GetIndexDataPointer());
		int num2 = sizeof(T);
		if (num2 <= 0 || num % num2 != 0)
		{
			throw new InvalidOperationException($"Index buffer byte size {num} is not divisible by element size {num2}.");
		}
		return new MeshBufferView<T>(pointer, num / num2);
	}

	public int GetVertexBufferStride(int stream)
	{
		return Layout.GetStreamStride(stream);
	}

	public unsafe MeshBufferView<T> GetVertexData<T>(int stream = 0) where T : unmanaged
	{
		int num = ((_writable != null) ? _writable.GetVertexDataByteLength(stream) : _readOnly.GetVertexDataByteLength(stream));
		nint pointer = ((_writable != null) ? _writable.GetVertexDataPointer(stream) : _readOnly.GetVertexDataPointer(stream));
		int num2 = sizeof(T);
		if (num2 <= 0 || num % num2 != 0)
		{
			throw new InvalidOperationException($"Vertex buffer byte size {num} is not divisible by element size {num2}.");
		}
		return new MeshBufferView<T>(pointer, num / num2);
	}

	public void GetVertices(Span<Vector3> outVertices)
	{
		CopyVector3Attribute(VertexAttributeSemantic.Position, outVertices);
	}

	public void GetNormals(Span<Vector3> outNormals)
	{
		CopyVector3Attribute(VertexAttributeSemantic.Normal, outNormals);
	}

	public void GetUVs(int channel, Span<Vector2> outUVs)
	{
		VertexAttributeSemantic semantic = (((uint)channel <= 7u) ? ((VertexAttributeSemantic)(4 + channel)) : VertexAttributeSemantic.TexCoord0);
		CopyVector2Attribute(semantic, outUVs);
	}

	public void SetIndexBufferParams(int indexCount, MeshIndexFormat format)
	{
		_writable?.SetIndexBufferParams(indexCount, format);
	}

	public void SetSubMesh(int index, SubMesh subMesh)
	{
		_writable?.SetSubMesh(index, subMesh);
	}

	public void SetVertexBufferParams(int vertexCount, params VertexAttribute[] attributes)
	{
		_writable?.SetVertexBufferParams(vertexCount, attributes);
	}

	private nint StreamPointer(int stream)
	{
		if (_writable == null)
		{
			return _readOnly.GetVertexDataPointer(stream);
		}
		return _writable.GetVertexDataPointer(stream);
	}

	private unsafe void CopyVector3Attribute(VertexAttributeSemantic semantic, Span<Vector3> destination)
	{
		VertexLayout layout = Layout;
		if (layout.TryGetAttribute(semantic, out var attribute))
		{
			int num = System.Math.Min(VertexCountInternal, destination.Length);
			byte* ptr = (byte*)StreamPointer(attribute.Stream);
			int streamStride = layout.GetStreamStride(attribute.Stream);
			int componentByteSize = VertexLayout.GetComponentByteSize(attribute.Format);
			for (int i = 0; i < num; i++)
			{
				byte* ptr2 = ptr + i * streamStride + attribute.OffsetBytes;
				destination[i] = new Vector3((attribute.Dimension > 0) ? MeshArrays.ReadScalar(ptr2, attribute.Format) : 0f, (attribute.Dimension > 1) ? MeshArrays.ReadScalar(ptr2 + componentByteSize, attribute.Format) : 0f, (attribute.Dimension > 2) ? MeshArrays.ReadScalar(ptr2 + 2 * componentByteSize, attribute.Format) : 0f);
			}
		}
	}

	private unsafe void CopyVector2Attribute(VertexAttributeSemantic semantic, Span<Vector2> destination)
	{
		VertexLayout layout = Layout;
		if (layout.TryGetAttribute(semantic, out var attribute))
		{
			int num = System.Math.Min(VertexCountInternal, destination.Length);
			byte* ptr = (byte*)StreamPointer(attribute.Stream);
			int streamStride = layout.GetStreamStride(attribute.Stream);
			int componentByteSize = VertexLayout.GetComponentByteSize(attribute.Format);
			for (int i = 0; i < num; i++)
			{
				byte* ptr2 = ptr + i * streamStride + attribute.OffsetBytes;
				destination[i] = new Vector2((attribute.Dimension > 0) ? MeshArrays.ReadScalar(ptr2, attribute.Format) : 0f, (attribute.Dimension > 1) ? MeshArrays.ReadScalar(ptr2 + componentByteSize, attribute.Format) : 0f);
			}
		}
	}
}
