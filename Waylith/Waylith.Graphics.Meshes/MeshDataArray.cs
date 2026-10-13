using System;

namespace Waylith.Graphics.Meshes;

public struct MeshDataArray : IDisposable
{
	private readonly WritableMeshData[]? _writable;

	private readonly ReadOnlyMeshData[]? _readOnly;

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
			WritableMeshData[]? writable = _writable;
			if (writable == null)
			{
				ReadOnlyMeshData[]? readOnly = _readOnly;
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
