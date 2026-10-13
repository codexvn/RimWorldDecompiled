namespace AssetsTools.NET;

public class AssetsFileHeader
{
	public long MetadataSize { get; set; }

	public long FileSize { get; set; }

	public uint Version { get; set; }

	public long DataOffset { get; set; }

	public bool Endianness { get; set; }

	public void Read(AssetsFileReader reader)
	{
		reader.BigEndian = true;
		MetadataSize = reader.ReadUInt32();
		FileSize = reader.ReadUInt32();
		Version = reader.ReadUInt32();
		DataOffset = reader.ReadUInt32();
		Endianness = reader.ReadBoolean();
		reader.Position += 3L;
		if (Version >= 22)
		{
			MetadataSize = reader.ReadUInt32();
			FileSize = reader.ReadInt64();
			DataOffset = reader.ReadInt64();
			reader.Position += 8L;
		}
		reader.BigEndian = Endianness;
	}

	public void Write(AssetsFileWriter writer)
	{
		writer.BigEndian = true;
		if (Version >= 22)
		{
			writer.Write(0);
			writer.Write(0);
			writer.Write(Version);
			writer.Write(0);
		}
		else
		{
			writer.Write((uint)MetadataSize);
			writer.Write((uint)FileSize);
			writer.Write(Version);
			writer.Write((uint)DataOffset);
		}
		writer.Write(Endianness);
		writer.Write(new byte[3]);
		if (Version >= 22)
		{
			writer.Write((uint)MetadataSize);
			writer.Write(FileSize);
			writer.Write(DataOffset);
			writer.Write(new byte[8]);
		}
		writer.BigEndian = Endianness;
	}
}
