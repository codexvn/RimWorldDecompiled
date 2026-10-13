using System;
using System.Collections.Generic;
using System.IO;
using AssetsTools.NET.Extra;
using AssetsTools.NET.Extra.Decompressors.LZ4;
using LZ4ps;
using SevenZip.Compression.LZMA;

namespace AssetsTools.NET;

public class ClassPackageFile
{
	public ClassPackageHeader Header { get; set; }

	public ClassPackageTypeTree TpkTypeTree { get; set; }

	public void Read(AssetsFileReader reader)
	{
		Header = new ClassPackageHeader();
		Header.Read(reader);
		AssetsFileReader assetsFileReader;
		if (Header.CompressionType == ClassFileCompressionType.Lz4)
		{
			byte[] buffer = new byte[Header.DecompressedSize];
			using (MemoryStream input = new MemoryStream(reader.ReadBytes((int)Header.CompressedSize)))
			{
				Lz4DecoderStream lz4DecoderStream = new Lz4DecoderStream(input);
				lz4DecoderStream.Read(buffer, 0, (int)Header.DecompressedSize);
				lz4DecoderStream.Dispose();
			}
			assetsFileReader = new AssetsFileReader(new MemoryStream(buffer));
			assetsFileReader.Position = 0L;
		}
		else if (Header.CompressionType == ClassFileCompressionType.Lzma)
		{
			using MemoryStream newInStream = new MemoryStream(reader.ReadBytes((int)Header.CompressedSize));
			assetsFileReader = new AssetsFileReader(SevenZipHelper.StreamDecompress(newInStream, (int)Header.DecompressedSize));
			assetsFileReader.Position = 0L;
		}
		else
		{
			assetsFileReader = reader;
		}
		TpkTypeTree = new ClassPackageTypeTree();
		TpkTypeTree.Read(assetsFileReader);
	}

	public void Read(string path)
	{
		Read(new AssetsFileReader(File.OpenRead(path)));
	}

	public void Write(AssetsFileWriter writer, ClassFileCompressionType compressionType)
	{
		Header.CompressionType = compressionType;
		MemoryStream memoryStream = new MemoryStream();
		AssetsFileWriter writer2 = new AssetsFileWriter(memoryStream);
		TpkTypeTree.Write(writer2);
		if (Header.CompressionType == ClassFileCompressionType.Lz4)
		{
			byte[] array = LZ4Codec.Encode32HC(memoryStream.ToArray(), 0, (int)memoryStream.Length);
			Header.CompressedSize = (uint)array.Length;
			Header.DecompressedSize = (uint)memoryStream.Length;
			Header.Write(writer);
			writer.Write(array);
		}
		else if (Header.CompressionType == ClassFileCompressionType.Lzma)
		{
			MemoryStream memoryStream2 = new MemoryStream();
			SevenZipHelper.Compress(memoryStream, memoryStream2);
			Header.CompressedSize = (uint)memoryStream2.Length;
			Header.DecompressedSize = (uint)memoryStream.Length;
			Header.Write(writer);
			memoryStream2.CopyToCompat(writer.BaseStream, -1L);
		}
		else
		{
			Header.CompressedSize = (uint)memoryStream.Length;
			Header.DecompressedSize = (uint)memoryStream.Length;
			Header.Write(writer);
			memoryStream.CopyToCompat(writer.BaseStream, -1L);
		}
	}

	public void Write(string path, ClassFileCompressionType compressionType)
	{
		Write(new AssetsFileWriter(File.OpenWrite(path)), compressionType);
	}

	public ClassDatabaseFile GetClassDatabase(string version)
	{
		return GetClassDatabase(new UnityVersion(version));
	}

	public ClassDatabaseFile GetClassDatabase(UnityVersion version)
	{
		if (Header == null)
		{
			throw new Exception("Header not loaded! (Did you forget to call package.Read?)");
		}
		ClassDatabaseFile classDatabaseFile = new ClassDatabaseFile();
		classDatabaseFile.Header = new ClassDatabaseFileHeader
		{
			Magic = "CLDB",
			FileVersion = 1,
			Version = version,
			CompressionType = ClassFileCompressionType.Uncompressed,
			CompressedSize = 0,
			DecompressedSize = 0
		};
		classDatabaseFile.Classes = new List<ClassDatabaseType>();
		foreach (ClassPackageClassInfo item in TpkTypeTree.ClassInformation)
		{
			ClassPackageType typeForVersion = item.GetTypeForVersion(version);
			if (typeForVersion != null)
			{
				ClassDatabaseType classDatabaseType = new ClassDatabaseType
				{
					ClassId = item.ClassId,
					Name = typeForVersion.Name,
					BaseName = typeForVersion.BaseName,
					Flags = typeForVersion.Flags
				};
				if (typeForVersion.EditorRootNode != ushort.MaxValue)
				{
					classDatabaseType.EditorRootNode = ConvertNodes(typeForVersion.EditorRootNode);
				}
				if (typeForVersion.ReleaseRootNode != ushort.MaxValue)
				{
					classDatabaseType.ReleaseRootNode = ConvertNodes(typeForVersion.ReleaseRootNode);
				}
				classDatabaseFile.Classes.Add(classDatabaseType);
			}
		}
		classDatabaseFile.StringTable = TpkTypeTree.StringTable;
		byte commonStringLengthForVersion = TpkTypeTree.CommonString.GetCommonStringLengthForVersion(version);
		classDatabaseFile.CommonStringBufferIndices = new List<ushort>(commonStringLengthForVersion);
		for (int i = 0; i < commonStringLengthForVersion; i++)
		{
			classDatabaseFile.CommonStringBufferIndices.Add(TpkTypeTree.CommonString.StringBufferIndices[i]);
		}
		return classDatabaseFile;
	}

	private ClassDatabaseTypeNode ConvertNodes(ushort tpkNodeIdx)
	{
		ClassPackageTypeNode classPackageTypeNode = TpkTypeTree.Nodes[tpkNodeIdx];
		ClassDatabaseTypeNode classDatabaseTypeNode = new ClassDatabaseTypeNode
		{
			TypeName = classPackageTypeNode.TypeName,
			FieldName = classPackageTypeNode.FieldName,
			ByteSize = classPackageTypeNode.ByteSize,
			Version = classPackageTypeNode.Version,
			TypeFlags = classPackageTypeNode.TypeFlags,
			MetaFlag = classPackageTypeNode.MetaFlag
		};
		int num = classPackageTypeNode.SubNodes.Length;
		classDatabaseTypeNode.Children = new List<ClassDatabaseTypeNode>(num);
		for (int i = 0; i < num; i++)
		{
			classDatabaseTypeNode.Children.Add(ConvertNodes(classPackageTypeNode.SubNodes[i]));
		}
		return classDatabaseTypeNode;
	}
}
