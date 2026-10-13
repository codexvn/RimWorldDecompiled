using System;

namespace Waylith.Graphics.Meshes;

public sealed class WritableMeshData : IDisposable
{
	private CpuMeshData? _storage;

	public VertexLayout Layout => Storage.Layout;

	public int VertexCount => Storage.VertexCount;

	public int IndexCount => Storage.IndexCount;

	public MeshIndexFormat IndexFormat => Storage.IndexFormat;

	public int SubMeshCount => Storage.SubMeshes.Length;

	private CpuMeshData Storage => _storage ?? throw new ObjectDisposedException("WritableMeshData");

	public WritableMeshData()
	{
		_storage = new CpuMeshData();
	}

	internal WritableMeshData(CpuMeshData storage)
	{
		_storage = storage ?? throw new ArgumentNullException("storage");
	}

	public void SetVertexBufferParams(int vertexCount, ReadOnlySpan<VertexAttribute> attributes)
	{
		Storage.SetVertexLayout(new VertexLayout(attributes.ToArray()), vertexCount);
	}

	public void SetIndexBufferParams(int indexCount, MeshIndexFormat format)
	{
		Storage.SetIndexBufferParams(indexCount, format);
	}

	public void SetSubMeshCount(int count)
	{
		Storage.SetSubMeshCount(count);
	}

	public void SetSubMesh(int index, SubMesh subMesh)
	{
		Storage.SetSubMesh(index, subMesh);
	}

	public int GetVertexBufferStride(int stream)
	{
		return Storage.Layout.GetStreamStride(stream);
	}

	public nint GetVertexDataPointer(int stream)
	{
		return Storage.GetVertexStream(stream).Pointer;
	}

	public int GetVertexDataByteLength(int stream)
	{
		return Storage.GetVertexStream(stream).LengthBytes;
	}

	public nint GetIndexDataPointer()
	{
		return Storage.GetIndexBuffer().Pointer;
	}

	public int GetIndexDataByteLength()
	{
		return Storage.GetIndexBuffer().LengthBytes;
	}

	public void SetBounds(MeshBounds bounds)
	{
		Storage.Bounds = bounds;
	}

	public CpuMeshData DetachStorage()
	{
		CpuMeshData storage = Storage;
		_storage = null;
		return storage;
	}

	public void Dispose()
	{
		_storage?.Dispose();
		_storage = null;
	}
}
