using System;
using System.Buffers.Binary;
using System.Text;

namespace PsdSharp.ImageResources;

public class VersionInfo : ImageResource
{
	public int Version { get; set; }

	public bool HasRealMergedData { get; set; }

	public string? WriterName { get; set; }

	public string? ReaderName { get; set; }

	public int FileVersion { get; set; }

	public VersionInfo(string? name, byte[] rawData)
	{
		base.Id = ImageResourceId.ThumbnailResource;
		base.Name = name;
		base.RawData = rawData;
		Version = BinaryPrimitives.ReadInt32BigEndian(rawData.AsSpan(0, 4));
		HasRealMergedData = rawData[4] != 0;
		(string, int) tuple = ReadUnicodeString(rawData.AsSpan(5));
		(string, int) tuple2 = ReadUnicodeString(rawData.AsSpan(5 + tuple.Item2));
		WriterName = tuple.Item1;
		ReaderName = tuple2.Item1;
		FileVersion = BinaryPrimitives.ReadInt32BigEndian(rawData.AsSpan(5 + tuple.Item2 + tuple2.Item2, 4));
	}

	public VersionInfo()
	{
	}

	private (string? String, int BytesRead) ReadUnicodeString(Span<byte> data)
	{
		int num = BinaryPrimitives.ReadInt32BigEndian(data);
		if (num == 0)
		{
			return (String: null, BytesRead: 4);
		}
		int num2 = num * 2;
		Span<byte> span = data.Slice(4, num2);
		return (String: Encoding.BigEndianUnicode.GetString(span), BytesRead: num2 + 4);
	}
}
