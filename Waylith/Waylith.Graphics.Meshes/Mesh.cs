using System;
using System.Collections.Generic;
using System.Threading;

namespace Waylith.Graphics.Meshes;

public sealed class Mesh : IDisposable
{
	private static int _nextId = 1;

	private CpuMeshData _cpuStorage = new CpuMeshData();

	private string _debugName = "Mesh";

	private int _disposed;

	public const string DefaultDebugName = "Mesh";

	public int Id { get; } = Interlocked.Increment(ref _nextId);

	public GpuMeshResource GpuResource { get; } = new GpuMeshResource();

	public MeshResidency Residency { get; private set; } = MeshResidency.ReadMostly;

	public bool IsReadable { get; private set; } = true;

	public long Version { get; private set; }

	internal bool IsDisposed => Volatile.Read(in _disposed) != 0;

	public string DebugName
	{
		get
		{
			return _debugName;
		}
		set
		{
			_debugName = (string.IsNullOrWhiteSpace(value) ? "Mesh" : value);
			GpuResource.RenameOwner(_debugName);
		}
	}

	public CpuMeshData CpuStorage => _cpuStorage;

	public MeshIndexFormat IndexFormat => _cpuStorage.IndexFormat;

	public int VertexCount => _cpuStorage.VertexCount;

	public int IndexCount => _cpuStorage.IndexCount;

	public int SubMeshCount => _cpuStorage.SubMeshes.Length;

	public MeshBounds Bounds
	{
		get
		{
			return _cpuStorage.Bounds;
		}
		set
		{
			_cpuStorage.Bounds = value;
		}
	}

	public static event Action<Mesh>? UploadRequested;

	public static event Action<Mesh>? ReleaseRequested;

	public Mesh()
	{
	}

	public Mesh(CpuMeshData storage)
	{
		ReplaceCpuStorage(storage);
	}

	public uint GetBaseVertex(int submesh)
	{
		return (uint)GetSubMeshOrDefault(submesh).BaseVertex;
	}

	public uint GetIndexCount(int submesh)
	{
		return (uint)GetSubMeshOrDefault(submesh).IndexCount;
	}

	public uint GetIndexStart(int submesh)
	{
		return (uint)GetSubMeshOrDefault(submesh).IndexStart;
	}

	public bool HasVertexAttribute(VertexAttributeSemantic semantic)
	{
		return _cpuStorage.Layout.HasAttribute(semantic);
	}

	public void RecalculateBounds()
	{
		MeshArrays.RecalculateBounds(_cpuStorage);
	}

	private SubMesh GetSubMeshOrDefault(int submesh)
	{
		if ((uint)submesh >= (uint)_cpuStorage.SubMeshes.Length)
		{
			return default(SubMesh);
		}
		return _cpuStorage.SubMeshes[submesh];
	}

	public static MeshDataArray AcquireReadOnlyMeshData(List<Mesh> meshes)
	{
		ArgumentNullException.ThrowIfNull(meshes, "meshes");
		ReadOnlyMeshData[] array = new ReadOnlyMeshData[meshes.Count];
		for (int i = 0; i < meshes.Count; i++)
		{
			array[i] = meshes[i]?.CreateReadOnlySnapshot() ?? new ReadOnlyMeshData(new CpuMeshData());
		}
		return new MeshDataArray(array);
	}

	public static MeshDataArray AllocateWritableMeshData(int meshCount)
	{
		WritableMeshData[] array = new WritableMeshData[System.Math.Max(0, meshCount)];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = new WritableMeshData();
		}
		return new MeshDataArray(array);
	}

	public static void ApplyAndDisposeWritableMeshData(MeshDataArray data, Mesh mesh)
	{
		ArgumentNullException.ThrowIfNull(mesh, "mesh");
		if (data.Length != 1)
		{
			throw new InvalidOperationException($"Expected exactly one mesh data item, found {data.Length}.");
		}
		mesh.ReplaceCpuStorage(data.GetWritable(0).DetachStorage());
		data.Dispose();
	}

	public static void ApplyAndDisposeWritableMeshData(MeshDataArray data, List<Mesh> meshes)
	{
		ArgumentNullException.ThrowIfNull(meshes, "meshes");
		if (meshes.Count != data.Length)
		{
			throw new InvalidOperationException($"Mesh count {meshes.Count} does not match mesh data count {data.Length}.");
		}
		for (int i = 0; i < meshes.Count; i++)
		{
			meshes[i]?.ReplaceCpuStorage(data.GetWritable(i).DetachStorage());
		}
		data.Dispose();
	}

	public void ReplaceCpuStorage(CpuMeshData storage)
	{
		ArgumentNullException.ThrowIfNull(storage, "storage");
		_cpuStorage.Dispose();
		_cpuStorage = storage;
		MarkCpuStorageChanged();
		IsReadable = true;
	}

	public void MarkCpuStorageChanged()
	{
		Version++;
		GpuResource.MarkDirty();
		GpuResource.RequestUpload(Version, discardCpuCopy: false);
		Mesh.UploadRequested?.Invoke(this);
	}

	public ReadOnlyMeshData CreateReadOnlySnapshot()
	{
		if (!IsReadable)
		{
			throw new InvalidOperationException("Mesh '" + DebugName + "' is not readable.");
		}
		return new ReadOnlyMeshData(_cpuStorage.Clone());
	}

	public void Clear()
	{
		_cpuStorage.Clear();
		MarkCpuStorageChanged();
	}

	public void MarkDynamic()
	{
		Residency = MeshResidency.DynamicMapped;
		GpuResource.IsDynamic = true;
	}

	public void Upload(bool discardCpuCopy)
	{
		GpuResource.RequestUpload(Version, discardCpuCopy);
		Mesh.UploadRequested?.Invoke(this);
		if (discardCpuCopy)
		{
			IsReadable = false;
		}
	}

	public void Dispose()
	{
		if (Interlocked.Exchange(ref _disposed, 1) == 0)
		{
			Mesh.ReleaseRequested?.Invoke(this);
			GpuResource.ReleaseOwner();
			_cpuStorage.Dispose();
		}
	}

	internal void OnUploadSucceeded(bool discardCpuCopy)
	{
		if (discardCpuCopy)
		{
			_cpuStorage.Dispose();
			_cpuStorage = new CpuMeshData();
			IsReadable = false;
		}
	}
}
