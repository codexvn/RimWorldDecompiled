using System.IO;
using PsdSharp.Parsing;

namespace PsdSharp.Images;

internal class BufferedSource : ImageDataSource
{
	private readonly byte[] _buffer;

	public BufferedSource(BigEndianReader reader, long? dataLength = null)
	{
		if (dataLength.HasValue)
		{
			_buffer = new byte[dataLength.Value];
			reader.ReadIntoBuffer(_buffer);
		}
		else
		{
			MemoryStream memoryStream = new MemoryStream();
			reader.CopyTo(memoryStream);
			_buffer = memoryStream.ToArray();
		}
	}

	public override Stream GetData()
	{
		return new MemoryStream(_buffer);
	}
}
