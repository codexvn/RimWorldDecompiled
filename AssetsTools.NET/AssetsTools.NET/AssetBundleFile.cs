using System;
using System.Collections.Generic;
using System.IO;
using AssetsTools.NET.Extra;
using AssetsTools.NET.Extra.Decompressors.LZ4;
using LZ4ps;
using SevenZip.Compression.LZMA;

namespace AssetsTools.NET;

public class AssetBundleFile
{
	public AssetsFileReader Reader;

	public AssetBundleHeader Header { get; set; }

	public AssetBundleBlockAndDirInfo BlockAndDirInfo { get; set; }

	public AssetsFileReader DataReader { get; set; }

	public bool DataIsCompressed { get; set; }

	public void Close()
	{
		Reader.Close();
		DataReader.Close();
	}

	public void Read(AssetsFileReader reader)
	{
		Reader = reader;
		Reader.Position = 0L;
		Reader.BigEndian = true;
		reader.ReadNullTerminated();
		uint num = reader.ReadUInt32();
		if (num >= 6 || num <= 8)
		{
			Reader.Position = 0L;
			Header = new AssetBundleHeader();
			Header.Read(reader);
			if (Header.Version >= 7)
			{
				reader.Align16();
			}
			if (Header.Signature == "UnityFS")
			{
				UnpackInfoOnly();
			}
			else
			{
				new NotImplementedException("Non UnityFS bundles are not supported yet.");
			}
		}
		else
		{
			new NotImplementedException($"Version {num} bundles are not supported yet.");
		}
	}

	public void Write(AssetsFileWriter writer, long filePos = 0L)
	{
		if (Header == null)
		{
			throw new Exception("Header must be loaded! (Did you forget to call bundle.Read?)");
		}
		if (Header.Signature != "UnityFS")
		{
			throw new NotImplementedException("Non UnityFS bundles are not supported yet.");
		}
		if (DataIsCompressed)
		{
			throw new Exception("Bundles must be decompressed before writing.");
		}
		long position = filePos;
		if (filePos == -1)
		{
			position = writer.Position;
		}
		else
		{
			writer.Position = filePos;
		}
		List<AssetBundleDirectoryInfo> directoryInfos = BlockAndDirInfo.DirectoryInfos;
		Header.Write(writer);
		if (Header.Version >= 7)
		{
			writer.Align16();
		}
		AssetBundleBlockInfo assetBundleBlockInfo = new AssetBundleBlockInfo
		{
			CompressedSize = 0u,
			DecompressedSize = 0u,
			Flags = 64
		};
		AssetBundleBlockAndDirInfo assetBundleBlockAndDirInfo = new AssetBundleBlockAndDirInfo();
		assetBundleBlockAndDirInfo.Hash = default(Hash128);
		assetBundleBlockAndDirInfo.BlockInfos = new AssetBundleBlockInfo[1] { assetBundleBlockInfo };
		AssetBundleBlockAndDirInfo assetBundleBlockAndDirInfo2 = assetBundleBlockAndDirInfo;
		List<AssetBundleDirectoryInfo> list = new List<AssetBundleDirectoryInfo>();
		int count = directoryInfos.Count;
		for (int i = 0; i < count; i++)
		{
			AssetBundleDirectoryInfo assetBundleDirectoryInfo = directoryInfos[i];
			if (assetBundleDirectoryInfo.ReplacerType != ContentReplacerType.Remove)
			{
				list.Add(new AssetBundleDirectoryInfo
				{
					Offset = assetBundleDirectoryInfo.Offset,
					DecompressedSize = assetBundleDirectoryInfo.DecompressedSize,
					Flags = assetBundleDirectoryInfo.Flags,
					Name = assetBundleDirectoryInfo.Name,
					Replacer = assetBundleDirectoryInfo.Replacer
				});
			}
		}
		long position2 = writer.Position;
		assetBundleBlockAndDirInfo2.DirectoryInfos = list;
		assetBundleBlockAndDirInfo2.Write(writer);
		if ((Header.FileStreamHeader.Flags & AssetBundleFSHeaderFlags.BlockInfoNeedPaddingAtStart) != 0)
		{
			writer.Align16();
		}
		long position3 = writer.Position;
		for (int j = 0; j < list.Count; j++)
		{
			AssetBundleDirectoryInfo assetBundleDirectoryInfo2 = list[j];
			long position4 = writer.Position;
			long offset = position4 - position3;
			if (assetBundleDirectoryInfo2.ReplacerType == ContentReplacerType.AddOrModify)
			{
				assetBundleDirectoryInfo2.Replacer.Write(writer);
			}
			else
			{
				DataReader.Position = assetBundleDirectoryInfo2.Offset;
				DataReader.BaseStream.CopyToCompat(writer.BaseStream, assetBundleDirectoryInfo2.DecompressedSize);
			}
			assetBundleDirectoryInfo2.Offset = offset;
			assetBundleDirectoryInfo2.DecompressedSize = writer.Position - position4;
		}
		long position5 = writer.Position;
		if (position5 - position3 > uint.MaxValue)
		{
			throw new NotImplementedException("Data larger than max uint not supported yet.");
		}
		uint num = (uint)(position5 - position3);
		writer.Position = position2;
		assetBundleBlockInfo.DecompressedSize = num;
		assetBundleBlockInfo.CompressedSize = num;
		assetBundleBlockAndDirInfo2.DirectoryInfos = list;
		assetBundleBlockAndDirInfo2.Write(writer);
		uint num2 = (uint)(position3 - position2);
		writer.Position = position;
		AssetBundleHeader assetBundleHeader = new AssetBundleHeader();
		assetBundleHeader.Signature = Header.Signature;
		assetBundleHeader.Version = Header.Version;
		assetBundleHeader.GenerationVersion = Header.GenerationVersion;
		assetBundleHeader.EngineVersion = Header.EngineVersion;
		assetBundleHeader.FileStreamHeader = new AssetBundleFSHeader
		{
			TotalFileSize = position5,
			CompressedSize = num2,
			DecompressedSize = num2,
			Flags = (Header.FileStreamHeader.Flags & (AssetBundleFSHeaderFlags)(-129) & (AssetBundleFSHeaderFlags)(-64))
		};
		assetBundleHeader.Write(writer);
	}

	public void Unpack(AssetsFileWriter writer)
	{
		if (Header == null)
		{
			new Exception("Header must be loaded! (Did you forget to call bundle.Read?)");
		}
		if (Header.Signature != "UnityFS")
		{
			new NotImplementedException("Non UnityFS bundles are not supported yet.");
		}
		AssetBundleFSHeader fileStreamHeader = Header.FileStreamHeader;
		AssetsFileReader dataReader = DataReader;
		AssetBundleBlockInfo[] blockInfos = BlockAndDirInfo.BlockInfos;
		List<AssetBundleDirectoryInfo> directoryInfos = BlockAndDirInfo.DirectoryInfos;
		AssetBundleHeader assetBundleHeader = new AssetBundleHeader
		{
			Signature = Header.Signature,
			Version = Header.Version,
			GenerationVersion = Header.GenerationVersion,
			EngineVersion = Header.EngineVersion,
			FileStreamHeader = new AssetBundleFSHeader
			{
				TotalFileSize = 0L,
				CompressedSize = fileStreamHeader.DecompressedSize,
				DecompressedSize = fileStreamHeader.DecompressedSize,
				Flags = (AssetBundleFSHeaderFlags)(0x40 | (((fileStreamHeader.Flags & AssetBundleFSHeaderFlags.BlockInfoNeedPaddingAtStart) != 0) ? 512 : 0))
			}
		};
		long num = assetBundleHeader.GetFileDataOffset();
		for (int i = 0; i < blockInfos.Length; i++)
		{
			num += blockInfos[i].DecompressedSize;
		}
		assetBundleHeader.FileStreamHeader.TotalFileSize = num;
		AssetBundleBlockAndDirInfo assetBundleBlockAndDirInfo = new AssetBundleBlockAndDirInfo
		{
			Hash = default(Hash128),
			BlockInfos = new AssetBundleBlockInfo[blockInfos.Length],
			DirectoryInfos = new List<AssetBundleDirectoryInfo>(directoryInfos.Count)
		};
		for (int j = 0; j < blockInfos.Length; j++)
		{
			assetBundleBlockAndDirInfo.BlockInfos[j] = new AssetBundleBlockInfo
			{
				CompressedSize = blockInfos[j].DecompressedSize,
				DecompressedSize = blockInfos[j].DecompressedSize,
				Flags = (ushort)(blockInfos[j].Flags & -64)
			};
		}
		for (int k = 0; k < directoryInfos.Count; k++)
		{
			assetBundleBlockAndDirInfo.DirectoryInfos.Add(new AssetBundleDirectoryInfo
			{
				Offset = directoryInfos[k].Offset,
				DecompressedSize = directoryInfos[k].DecompressedSize,
				Flags = directoryInfos[k].Flags,
				Name = directoryInfos[k].Name
			});
		}
		assetBundleHeader.Write(writer);
		if (assetBundleHeader.Version >= 7)
		{
			writer.Align16();
		}
		assetBundleBlockAndDirInfo.Write(writer);
		if ((assetBundleHeader.FileStreamHeader.Flags & AssetBundleFSHeaderFlags.BlockInfoNeedPaddingAtStart) != 0)
		{
			writer.Align16();
		}
		dataReader.Position = 0L;
		if (DataIsCompressed)
		{
			for (int l = 0; l < assetBundleBlockAndDirInfo.BlockInfos.Length; l++)
			{
				AssetBundleBlockInfo assetBundleBlockInfo = blockInfos[l];
				switch (assetBundleBlockInfo.GetCompressionType())
				{
				case 0:
					dataReader.BaseStream.CopyToCompat(writer.BaseStream, assetBundleBlockInfo.CompressedSize);
					break;
				case 1:
					SevenZipHelper.StreamDecompress(dataReader.BaseStream, writer.BaseStream, assetBundleBlockInfo.CompressedSize, assetBundleBlockInfo.DecompressedSize);
					break;
				case 2:
				case 3:
				{
					using (MemoryStream memoryStream = new MemoryStream())
					{
						dataReader.BaseStream.CopyToCompat(memoryStream, assetBundleBlockInfo.CompressedSize);
						memoryStream.Position = 0L;
						using Lz4DecoderStream input = new Lz4DecoderStream(memoryStream);
						input.CopyToCompat(writer.BaseStream, assetBundleBlockInfo.DecompressedSize);
					}
					break;
				}
				}
			}
		}
		else
		{
			for (int m = 0; m < assetBundleBlockAndDirInfo.BlockInfos.Length; m++)
			{
				AssetBundleBlockInfo assetBundleBlockInfo2 = blockInfos[m];
				dataReader.BaseStream.CopyToCompat(writer.BaseStream, assetBundleBlockInfo2.DecompressedSize);
			}
		}
	}

	public void Pack(AssetsFileWriter writer, AssetBundleCompressionType compType, bool blockDirAtEnd = true, IAssetBundleCompressProgress progress = null)
	{
		if (Header == null)
		{
			throw new Exception("Header must be loaded! (Did you forget to call bundle.Read?)");
		}
		if (Header.Signature != "UnityFS")
		{
			throw new NotImplementedException("Non UnityFS bundles are not supported yet.");
		}
		if (DataIsCompressed)
		{
			throw new Exception("Bundles must be decompressed before writing.");
		}
		Reader.Position = 0L;
		writer.Position = 0L;
		AssetBundleFSHeader assetBundleFSHeader = new AssetBundleFSHeader
		{
			TotalFileSize = 0L,
			CompressedSize = 0u,
			DecompressedSize = 0u,
			Flags = (AssetBundleFSHeaderFlags)(0x43 | (blockDirAtEnd ? 128 : 0))
		};
		AssetBundleHeader assetBundleHeader = new AssetBundleHeader
		{
			Signature = Header.Signature,
			Version = Header.Version,
			GenerationVersion = Header.GenerationVersion,
			EngineVersion = Header.EngineVersion,
			FileStreamHeader = assetBundleFSHeader
		};
		AssetBundleBlockAndDirInfo assetBundleBlockAndDirInfo = new AssetBundleBlockAndDirInfo
		{
			Hash = default(Hash128),
			BlockInfos = null,
			DirectoryInfos = BlockAndDirInfo.DirectoryInfos
		};
		long position = writer.Position;
		assetBundleHeader.Write(writer);
		if (assetBundleHeader.Version >= 7)
		{
			writer.Align16();
		}
		int num = (int)(writer.Position - position);
		long num2 = 0L;
		List<AssetBundleBlockInfo> list = new List<AssetBundleBlockInfo>();
		List<Stream> list2 = new List<Stream>();
		Stream baseStream = DataReader.BaseStream;
		baseStream.Position = 0L;
		int num3 = (int)baseStream.Length;
		switch (compType)
		{
		case AssetBundleCompressionType.LZMA:
		{
			Stream stream2 = ((!blockDirAtEnd) ? GetTempFileStream() : writer.BaseStream);
			AssetBundleLZMAProgress progress2 = new AssetBundleLZMAProgress(progress, baseStream.Length);
			long position2 = stream2.Position;
			SevenZipHelper.Compress(baseStream, stream2, progress2);
			uint compressedSize = (uint)(stream2.Position - position2);
			AssetBundleBlockInfo assetBundleBlockInfo4 = new AssetBundleBlockInfo
			{
				CompressedSize = compressedSize,
				DecompressedSize = (uint)num3,
				Flags = 65
			};
			num2 += assetBundleBlockInfo4.CompressedSize;
			list.Add(assetBundleBlockInfo4);
			if (!blockDirAtEnd)
			{
				list2.Add(stream2);
			}
			progress?.SetProgress(1f);
			break;
		}
		case AssetBundleCompressionType.LZ4:
		{
			BinaryReader binaryReader = new BinaryReader(baseStream);
			Stream stream = ((!blockDirAtEnd) ? GetTempFileStream() : writer.BaseStream);
			byte[] array = binaryReader.ReadBytes(131072);
			while (array.Length != 0)
			{
				byte[] array2 = LZ4Codec.Encode32HC(array, 0, array.Length);
				progress?.SetProgress((float)binaryReader.BaseStream.Position / (float)binaryReader.BaseStream.Length);
				if (array2.Length > array.Length)
				{
					stream.Write(array, 0, array.Length);
					AssetBundleBlockInfo assetBundleBlockInfo2 = new AssetBundleBlockInfo
					{
						CompressedSize = (uint)array.Length,
						DecompressedSize = (uint)array.Length,
						Flags = 0
					};
					num2 += assetBundleBlockInfo2.CompressedSize;
					list.Add(assetBundleBlockInfo2);
				}
				else
				{
					stream.Write(array2, 0, array2.Length);
					AssetBundleBlockInfo assetBundleBlockInfo3 = new AssetBundleBlockInfo
					{
						CompressedSize = (uint)array2.Length,
						DecompressedSize = (uint)array.Length,
						Flags = 3
					};
					num2 += assetBundleBlockInfo3.CompressedSize;
					list.Add(assetBundleBlockInfo3);
				}
				array = binaryReader.ReadBytes(131072);
			}
			if (!blockDirAtEnd)
			{
				list2.Add(stream);
			}
			progress?.SetProgress(1f);
			break;
		}
		case AssetBundleCompressionType.None:
		{
			AssetBundleBlockInfo assetBundleBlockInfo = new AssetBundleBlockInfo
			{
				CompressedSize = (uint)num3,
				DecompressedSize = (uint)num3,
				Flags = 0
			};
			num2 += assetBundleBlockInfo.CompressedSize;
			list.Add(assetBundleBlockInfo);
			if (blockDirAtEnd)
			{
				baseStream.CopyToCompat(writer.BaseStream, -1L);
			}
			else
			{
				list2.Add(baseStream);
			}
			break;
		}
		}
		assetBundleBlockAndDirInfo.BlockInfos = list.ToArray();
		byte[] array3;
		using (MemoryStream memoryStream = new MemoryStream())
		{
			AssetsFileWriter assetsFileWriter = new AssetsFileWriter(memoryStream);
			assetsFileWriter.BigEndian = writer.BigEndian;
			assetBundleBlockAndDirInfo.Write(assetsFileWriter);
			array3 = memoryStream.ToArray();
		}
		byte[] array4 = LZ4Codec.Encode32HC(array3, 0, array3.Length);
		long totalFileSize = num + array4.Length + num2;
		assetBundleFSHeader.TotalFileSize = totalFileSize;
		assetBundleFSHeader.DecompressedSize = (uint)array3.Length;
		assetBundleFSHeader.CompressedSize = (uint)array4.Length;
		if (!blockDirAtEnd)
		{
			writer.Write(array4);
			foreach (Stream item in list2)
			{
				item.Position = 0L;
				item.CopyToCompat(writer.BaseStream, -1L);
				item.Close();
			}
		}
		else
		{
			writer.Write(array4);
		}
		writer.Position = 0L;
		assetBundleHeader.Write(writer);
		if (assetBundleHeader.Version >= 7)
		{
			writer.Align16();
		}
	}

	private void UnpackInfoOnly()
	{
		if (Header == null)
		{
			throw new Exception("Header must be loaded! (Did you forget to call bundle.Read?)");
		}
		Reader.Position = Header.GetBundleInfoOffset();
		if (Header.GetCompressionType() == 0)
		{
			BlockAndDirInfo = new AssetBundleBlockAndDirInfo();
			BlockAndDirInfo.Read(Reader);
		}
		else
		{
			int compressedSize = (int)Header.FileStreamHeader.CompressedSize;
			int decompressedSize = (int)Header.FileStreamHeader.DecompressedSize;
			MemoryStream memoryStream;
			switch (Header.GetCompressionType())
			{
			case 1:
			{
				using (MemoryStream compressedStream = new MemoryStream(Reader.ReadBytes(compressedSize)))
				{
					memoryStream = new MemoryStream();
					SevenZipHelper.StreamDecompress(compressedStream, memoryStream, compressedSize, decompressedSize);
				}
				break;
			}
			case 2:
			case 3:
			{
				byte[] buffer = new byte[Header.FileStreamHeader.DecompressedSize];
				using (MemoryStream input = new MemoryStream(Reader.ReadBytes(compressedSize)))
				{
					Lz4DecoderStream lz4DecoderStream = new Lz4DecoderStream(input);
					lz4DecoderStream.Read(buffer, 0, (int)Header.FileStreamHeader.DecompressedSize);
					lz4DecoderStream.Dispose();
				}
				memoryStream = new MemoryStream(buffer);
				break;
			}
			default:
				memoryStream = null;
				break;
			}
			AssetsFileReader assetsFileReader;
			using (assetsFileReader = new AssetsFileReader(memoryStream))
			{
				assetsFileReader.Position = 0L;
				assetsFileReader.BigEndian = Reader.BigEndian;
				BlockAndDirInfo = new AssetBundleBlockAndDirInfo();
				BlockAndDirInfo.Read(assetsFileReader);
			}
		}
		switch (GetCompressionType())
		{
		case AssetBundleCompressionType.None:
		{
			SegmentStream stream3 = new SegmentStream(Reader.BaseStream, Header.GetFileDataOffset());
			DataReader = new AssetsFileReader(stream3);
			DataIsCompressed = false;
			break;
		}
		case AssetBundleCompressionType.LZMA:
		{
			SegmentStream stream2 = new SegmentStream(Reader.BaseStream, Header.GetFileDataOffset());
			DataReader = new AssetsFileReader(stream2);
			DataIsCompressed = true;
			break;
		}
		case AssetBundleCompressionType.LZ4:
		{
			LZ4BlockStream stream = new LZ4BlockStream(Reader.BaseStream, Header.GetFileDataOffset(), BlockAndDirInfo.BlockInfos);
			DataReader = new AssetsFileReader(stream);
			DataIsCompressed = false;
			break;
		}
		}
	}

	public AssetBundleCompressionType GetCompressionType()
	{
		AssetBundleBlockInfo[] blockInfos = BlockAndDirInfo.BlockInfos;
		for (int i = 0; i < blockInfos.Length; i++)
		{
			switch (blockInfos[i].GetCompressionType())
			{
			case 2:
			case 3:
				return AssetBundleCompressionType.LZ4;
			case 1:
				return AssetBundleCompressionType.LZMA;
			}
		}
		return AssetBundleCompressionType.None;
	}

	public bool IsAssetsFile(int index)
	{
		GetFileRange(index, out var offset, out var length);
		return AssetsFile.IsAssetsFile(DataReader, offset, length);
	}

	public int GetFileIndex(string name)
	{
		if (Header == null)
		{
			throw new Exception("Header must be loaded! (Did you forget to call bundle.Read?)");
		}
		for (int i = 0; i < BlockAndDirInfo.DirectoryInfos.Count; i++)
		{
			if (BlockAndDirInfo.DirectoryInfos[i].Name == name)
			{
				return i;
			}
		}
		return -1;
	}

	public string GetFileName(int index)
	{
		if (Header == null)
		{
			throw new Exception("Header must be loaded! (Did you forget to call bundle.Read?)");
		}
		if (index < 0 || index >= BlockAndDirInfo.DirectoryInfos.Count)
		{
			return null;
		}
		return BlockAndDirInfo.DirectoryInfos[index].Name;
	}

	public void GetFileRange(int index, out long offset, out long length)
	{
		if (Header == null)
		{
			throw new Exception("Header must be loaded! (Did you forget to call bundle.Read?)");
		}
		if (index < 0 || index >= BlockAndDirInfo.DirectoryInfos.Count)
		{
			offset = -1L;
			length = 0L;
		}
		else
		{
			AssetBundleDirectoryInfo assetBundleDirectoryInfo = BlockAndDirInfo.DirectoryInfos[index];
			offset = assetBundleDirectoryInfo.Offset;
			length = assetBundleDirectoryInfo.DecompressedSize;
		}
	}

	public List<string> GetAllFileNames()
	{
		if (Header == null)
		{
			throw new Exception("Header must be loaded! (Did you forget to call bundle.Read?)");
		}
		List<string> list = new List<string>();
		foreach (AssetBundleDirectoryInfo directoryInfo in BlockAndDirInfo.DirectoryInfos)
		{
			list.Add(directoryInfo.Name);
		}
		return list;
	}

	private FileStream GetTempFileStream()
	{
		return new FileStream(Path.GetTempFileName(), FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read, 4096, FileOptions.DeleteOnClose);
	}
}
