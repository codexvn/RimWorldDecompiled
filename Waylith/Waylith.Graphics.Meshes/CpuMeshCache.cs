using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using Waylith.Assets;
using Waylith.Diagnostics;
using Waylith.Runtime;
using Waylith.Utils;

namespace Waylith.Graphics.Meshes;

public static class CpuMeshCache
{
	private const uint Magic = 1213416781u;

	private const int Version = 1;

	private static int _sweepStarted;

	public const int PackFormatVersion = 1;

	public static bool TryLoad(string sourcePath, CpuMeshData storage, string? parser = null)
	{
		try
		{
			if (TryLoadFromPack(sourcePath, storage))
			{
				return true;
			}
			if (ColdBoot.Enabled || !TryGetCachePath(sourcePath, parser, out string cachePath) || !File.Exists(cachePath))
			{
				return false;
			}
			FileInfo fileInfo = new FileInfo(sourcePath);
			if (!fileInfo.Exists)
			{
				return false;
			}
			using FileStream input = File.OpenRead(cachePath);
			using BinaryReader binaryReader = new BinaryReader(input);
			if (binaryReader.ReadUInt32() != 1213416781 || binaryReader.ReadInt32() != 1)
			{
				return false;
			}
			if (binaryReader.ReadInt64() != fileInfo.Length || binaryReader.ReadInt64() != fileInfo.LastWriteTimeUtc.Ticks)
			{
				return false;
			}
			return TryReadRecord(binaryReader, storage);
		}
		catch
		{
			return false;
		}
	}

	private static bool TryLoadFromPack(string sourcePath, CpuMeshData storage)
	{
		if (!ContentPack.TryGetCurrentEntry(sourcePath, ".nbmesh", out AssetPack pack, out string name) || !pack.TryRead(name, out byte[] data))
		{
			return false;
		}
		using BinaryReader binaryReader = new BinaryReader(new MemoryStream(data));
		return binaryReader.ReadUInt32() == 1213416781 && binaryReader.ReadInt32() == 1 && TryReadRecord(binaryReader, storage);
	}

	public static byte[] EncodePackEntry(CpuMeshData storage)
	{
		using MemoryStream memoryStream = new MemoryStream();
		using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
		{
			binaryWriter.Write(1213416781u);
			binaryWriter.Write(1);
			WriteRecord(binaryWriter, storage);
		}
		return memoryStream.ToArray();
	}

	private static bool TryReadRecord(BinaryReader reader, CpuMeshData storage)
	{
		int num = reader.ReadInt32();
		VertexAttribute[] array = new VertexAttribute[num];
		for (int i = 0; i < num; i++)
		{
			array[i] = new VertexAttribute((VertexAttributeSemantic)reader.ReadInt32(), (VertexAttributeFormat)reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32());
		}
		int vertexCount = reader.ReadInt32();
		int indexCount = reader.ReadInt32();
		MeshIndexFormat format = (MeshIndexFormat)reader.ReadInt32();
		storage.SetVertexLayout(new VertexLayout(array), vertexCount);
		storage.SetIndexBufferParams(indexCount, format);
		storage.Bounds = ReadBounds(reader);
		int num2 = reader.ReadInt32();
		storage.SetSubMeshCount(num2);
		for (int j = 0; j < num2; j++)
		{
			int indexStart = reader.ReadInt32();
			int indexCount2 = reader.ReadInt32();
			int baseVertex = reader.ReadInt32();
			MeshTopology topology = (MeshTopology)reader.ReadInt32();
			storage.SetSubMesh(j, new SubMesh(indexStart, indexCount2, topology, baseVertex, ReadBounds(reader)));
		}
		for (int k = 0; k < storage.Layout.StreamCount; k++)
		{
			if (!TryFill(reader, storage.GetVertexStream(k)))
			{
				return false;
			}
		}
		return TryFill(reader, storage.GetIndexBuffer());
	}

	public static (long Length, long WriteTicks)? StampSource(string sourcePath)
	{
		FileInfo fileInfo = new FileInfo(sourcePath);
		if (!fileInfo.Exists)
		{
			return null;
		}
		return (fileInfo.Length, fileInfo.LastWriteTimeUtc.Ticks);
	}

	public static void Store(string sourcePath, (long Length, long WriteTicks)? sourceStamp, CpuMeshData storage, string? parser = null)
	{
		try
		{
			if (!sourceStamp.HasValue)
			{
				return;
			}
			(long, long) valueOrDefault = sourceStamp.GetValueOrDefault();
			if (!TryGetCachePath(sourcePath, parser, out string cachePath))
			{
				return;
			}
			using MemoryStream memoryStream = new MemoryStream();
			using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
			{
				binaryWriter.Write(1213416781u);
				binaryWriter.Write(1);
				binaryWriter.Write(valueOrDefault.Item1);
				binaryWriter.Write(valueOrDefault.Item2);
				WriteRecord(binaryWriter, storage);
			}
			CacheFileWriter.Enqueue(cachePath, memoryStream.ToArray());
		}
		catch
		{
		}
	}

	private static void WriteRecord(BinaryWriter writer, CpuMeshData storage)
	{
		ReadOnlySpan<VertexAttribute> attributes = storage.Layout.Attributes;
		writer.Write(attributes.Length);
		ReadOnlySpan<VertexAttribute> readOnlySpan = attributes;
		for (int i = 0; i < readOnlySpan.Length; i++)
		{
			VertexAttribute vertexAttribute = readOnlySpan[i];
			writer.Write((int)vertexAttribute.Semantic);
			writer.Write((int)vertexAttribute.Format);
			writer.Write(vertexAttribute.Dimension);
			writer.Write(vertexAttribute.Stream);
		}
		writer.Write(storage.VertexCount);
		writer.Write(storage.IndexCount);
		writer.Write((int)storage.IndexFormat);
		WriteBounds(writer, storage.Bounds);
		writer.Write(storage.SubMeshes.Length);
		SubMesh[] subMeshes = storage.SubMeshes;
		for (int i = 0; i < subMeshes.Length; i++)
		{
			SubMesh subMesh = subMeshes[i];
			writer.Write(subMesh.IndexStart);
			writer.Write(subMesh.IndexCount);
			writer.Write(subMesh.BaseVertex);
			writer.Write((int)subMesh.Topology);
			WriteBounds(writer, subMesh.Bounds);
		}
		for (int j = 0; j < storage.Layout.StreamCount; j++)
		{
			WriteBuffer(writer, storage.GetVertexStream(j));
		}
		WriteBuffer(writer, storage.GetIndexBuffer());
	}

	private static bool TryFill(BinaryReader reader, NativeByteBuffer buffer)
	{
		int num = reader.ReadInt32();
		if (num != buffer.LengthBytes)
		{
			return false;
		}
		if (num == 0)
		{
			return true;
		}
		reader.BaseStream.ReadExactly(buffer.AsSpan());
		return true;
	}

	private static void WriteBuffer(BinaryWriter writer, NativeByteBuffer buffer)
	{
		writer.Write(buffer.LengthBytes);
		if (buffer.LengthBytes > 0)
		{
			writer.Write(buffer.AsSpan());
		}
	}

	private static MeshBounds ReadBounds(BinaryReader reader)
	{
		return new MeshBounds(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
	}

	private static void WriteBounds(BinaryWriter writer, MeshBounds bounds)
	{
		writer.Write(bounds.MinX);
		writer.Write(bounds.MinY);
		writer.Write(bounds.MinZ);
		writer.Write(bounds.MaxX);
		writer.Write(bounds.MaxY);
		writer.Write(bounds.MaxZ);
	}

	private static bool TryGetCachePath(string sourcePath, string? parser, out string cachePath)
	{
		cachePath = string.Empty;
		string text = WaylithRuntime.Current?.App.CacheDataPath;
		if (string.IsNullOrWhiteSpace(text))
		{
			return false;
		}
		byte[] inArray = SHA256.HashData(Encoding.UTF8.GetBytes((parser == null) ? Path.GetFullPath(sourcePath) : (parser + "\0" + Path.GetFullPath(sourcePath))));
		string text2 = Path.Combine(text, "MeshCache", $"v{1}");
		if (Interlocked.Exchange(ref _sweepStarted, 1) == 0)
		{
			CacheFileWriter.SweepLater(text2);
		}
		cachePath = Path.Combine(text2, Convert.ToHexString(inArray).ToLowerInvariant() + ".nbmesh");
		return true;
	}
}
