using System;
using Waylith.Runtime;

namespace Waylith.Graphics.Meshes;

public sealed class CpuMeshData : IDisposable
{
	private NativeByteBuffer[] _vertexStreams = Array.Empty<NativeByteBuffer>();

	private readonly NativeByteBuffer _indexBuffer = new NativeByteBuffer(AllocationKind.MeshBacking);

	private SubMesh[] _subMeshes = Array.Empty<SubMesh>();

	public VertexLayout Layout { get; private set; } = VertexLayout.Empty;

	public int VertexCount { get; private set; }

	public int IndexCount { get; private set; }

	public MeshIndexFormat IndexFormat { get; private set; }

	public SubMesh[] SubMeshes => _subMeshes;

	public MeshBounds Bounds { get; set; }

	public void SetVertexLayout(VertexLayout layout, int vertexCount)
	{
		Layout = layout ?? VertexLayout.Empty;
		VertexCount = System.Math.Max(0, vertexCount);
		if (_vertexStreams.Length != Layout.StreamCount)
		{
			for (int i = Layout.StreamCount; i < _vertexStreams.Length; i++)
			{
				_vertexStreams[i]?.Dispose();
			}
			Array.Resize(ref _vertexStreams, Layout.StreamCount);
		}
		for (int j = 0; j < Layout.StreamCount; j++)
		{
			ref NativeByteBuffer reference = ref _vertexStreams[j];
			if (reference == null)
			{
				reference = new NativeByteBuffer(AllocationKind.MeshBacking);
			}
			_vertexStreams[j].Resize(VertexCount * Layout.GetStreamStride(j));
		}
	}

	public void SetIndexBufferParams(int indexCount, MeshIndexFormat format)
	{
		IndexCount = System.Math.Max(0, indexCount);
		IndexFormat = format;
		_indexBuffer.Resize(IndexCount * GetIndexElementSize(format));
	}

	public void SetSubMeshCount(int count)
	{
		count = System.Math.Max(0, count);
		if (_subMeshes.Length != count)
		{
			Array.Resize(ref _subMeshes, count);
		}
	}

	public void SetSubMesh(int index, SubMesh subMesh)
	{
		if ((uint)index >= (uint)_subMeshes.Length)
		{
			throw new ArgumentOutOfRangeException("index");
		}
		_subMeshes[index] = subMesh;
	}

	public NativeByteBuffer GetVertexStream(int stream)
	{
		if ((uint)stream >= (uint)_vertexStreams.Length)
		{
			throw new ArgumentOutOfRangeException("stream");
		}
		return _vertexStreams[stream];
	}

	public NativeByteBuffer GetIndexBuffer()
	{
		return _indexBuffer;
	}

	public CpuMeshData Clone()
	{
		CpuMeshData cpuMeshData = new CpuMeshData
		{
			Bounds = Bounds
		};
		cpuMeshData.SetVertexLayout(Layout, VertexCount);
		for (int i = 0; i < _vertexStreams.Length; i++)
		{
			cpuMeshData._vertexStreams[i].CopyFrom(_vertexStreams[i]);
		}
		cpuMeshData.SetIndexBufferParams(IndexCount, IndexFormat);
		cpuMeshData._indexBuffer.CopyFrom(_indexBuffer);
		cpuMeshData.SetSubMeshCount(_subMeshes.Length);
		for (int j = 0; j < _subMeshes.Length; j++)
		{
			cpuMeshData._subMeshes[j] = _subMeshes[j];
		}
		return cpuMeshData;
	}

	public void Clear()
	{
		for (int i = 0; i < _vertexStreams.Length; i++)
		{
			_vertexStreams[i]?.Resize(0);
		}
		_indexBuffer.Resize(0);
		VertexCount = 0;
		IndexCount = 0;
		_subMeshes = Array.Empty<SubMesh>();
		Bounds = default(MeshBounds);
	}

	public void Dispose()
	{
		for (int i = 0; i < _vertexStreams.Length; i++)
		{
			_vertexStreams[i]?.Dispose();
		}
		_indexBuffer.Dispose();
	}

	public static int GetIndexElementSize(MeshIndexFormat format)
	{
		if (format != 0)
		{
			return 4;
		}
		return 2;
	}
}
