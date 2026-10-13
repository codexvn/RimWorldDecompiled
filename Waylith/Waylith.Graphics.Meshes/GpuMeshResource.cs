using System;

namespace Waylith.Graphics.Meshes;

public sealed class GpuMeshResource
{
	private GpuMeshBuffer[] _vertexStreams = Array.Empty<GpuMeshBuffer>();

	private IGpuMeshResourceOwner? _owner;

	public long UploadedVersion { get; private set; } = -1L;

	public long PendingUploadVersion { get; private set; } = -1L;

	public bool RequestedDiscardCpuCopy { get; private set; }

	public bool IsDynamic { get; set; }

	public MeshIndexFormat IndexFormat { get; private set; }

	public VertexLayout Layout { get; private set; } = VertexLayout.Empty;

	public SubMesh[] SubMeshes { get; private set; } = Array.Empty<SubMesh>();

	public MeshBounds Bounds { get; private set; }

	public ReadOnlySpan<GpuMeshBuffer> VertexStreams => _vertexStreams;

	public GpuMeshBuffer? IndexBuffer { get; private set; }

	public bool HasBuffers
	{
		get
		{
			if (_vertexStreams.Length == 0)
			{
				return IndexBuffer != null;
			}
			return true;
		}
	}

	internal void SetOwner(IGpuMeshResourceOwner owner)
	{
		_owner = owner;
	}

	internal void ClearOwner(IGpuMeshResourceOwner owner)
	{
		if (_owner == owner)
		{
			_owner = null;
		}
	}

	internal void ReleaseOwner()
	{
		_owner?.Release(this);
	}

	internal void RenameOwner(string debugName)
	{
		_owner?.Rename(this, debugName);
	}

	public void MarkDirty()
	{
		UploadedVersion = -1L;
	}

	public void RequestUpload(long version, bool discardCpuCopy)
	{
		PendingUploadVersion = version;
		RequestedDiscardCpuCopy |= discardCpuCopy;
	}

	public bool NeedsUpload(long meshVersion)
	{
		if (UploadedVersion == meshVersion)
		{
			return PendingUploadVersion == meshVersion;
		}
		return true;
	}

	public void SetBuffers(GpuMeshBuffer[] vertexStreams, GpuMeshBuffer? indexBuffer, MeshIndexFormat indexFormat, VertexLayout layout, SubMesh[] subMeshes, MeshBounds bounds, long version)
	{
		_vertexStreams = vertexStreams ?? Array.Empty<GpuMeshBuffer>();
		IndexBuffer = indexBuffer;
		IndexFormat = indexFormat;
		Layout = layout ?? VertexLayout.Empty;
		SubMeshes = subMeshes ?? Array.Empty<SubMesh>();
		Bounds = bounds;
		UploadedVersion = version;
		PendingUploadVersion = -1L;
		RequestedDiscardCpuCopy = false;
	}

	public void ClearBuffers(long version = -1L)
	{
		SetBuffers(Array.Empty<GpuMeshBuffer>(), null, IndexFormat, VertexLayout.Empty, Array.Empty<SubMesh>(), default(MeshBounds), version);
	}
}
