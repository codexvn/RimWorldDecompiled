using System;

namespace Waylith.Graphics.Meshes;

public sealed class ReadOnlyMeshData : IDisposable
{
	private CpuMeshData? _storage;

	private CpuMeshData Storage => _storage ?? throw new ObjectDisposedException("ReadOnlyMeshData");

	public VertexLayout Layout => Storage.Layout;

	public int VertexCount => Storage.VertexCount;

	public int IndexCount => Storage.IndexCount;

	public MeshIndexFormat IndexFormat => Storage.IndexFormat;

	public int SubMeshCount => Storage.SubMeshes.Length;

	public MeshBounds Bounds => Storage.Bounds;

	public ReadOnlyMeshData(CpuMeshData storage)
	{
		_storage = storage ?? throw new ArgumentNullException("storage");
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

	public ReadOnlySpan<SubMesh> GetSubMeshes()
	{
		return Storage.SubMeshes;
	}

	public void Dispose()
	{
		_storage?.Dispose();
		_storage = null;
	}
}
