using System.IO;

namespace Waylith.VideoDecode;

public sealed class VideoMedia
{
	public string Path;

	public byte[] Data;

	public string Label;

	public Stream OpenStream()
	{
		if (Data == null)
		{
			return File.OpenRead(Path);
		}
		return new MemoryStream(Data, writable: false);
	}
}
