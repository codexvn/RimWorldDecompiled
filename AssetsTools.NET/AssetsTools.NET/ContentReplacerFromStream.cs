using System;
using System.IO;
using AssetsTools.NET.Extra;

namespace AssetsTools.NET;

public class ContentReplacerFromStream : IContentReplacer
{
	private readonly Stream stream;

	private readonly long offset;

	private readonly long length;

	private readonly bool closeOnWrite;

	public ContentReplacerFromStream(Stream stream, long offset = 0L, int length = -1, bool closeOnWrite = false)
	{
		this.stream = stream;
		this.offset = offset;
		this.length = length;
		this.closeOnWrite = closeOnWrite;
		if (!stream.CanSeek)
		{
			throw new NotSupportedException("Stream needs to be seekable.");
		}
	}

	public void Write(AssetsFileWriter writer)
	{
		if (offset != -1)
		{
			stream.Position = offset;
		}
		stream.CopyToCompat(writer.BaseStream, length);
		if (closeOnWrite)
		{
			stream.Close();
		}
	}

	public bool HasPreview()
	{
		return true;
	}

	public Stream GetPreviewStream()
	{
		if (offset == 0L && length == -1)
		{
			return stream;
		}
		return new SegmentStream(stream, offset, length);
	}

	public ContentReplacerType GetReplacerType()
	{
		return ContentReplacerType.AddOrModify;
	}
}
