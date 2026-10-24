using System.IO;
using System.IO.Compression;

namespace PsdSharp.Compression;

internal static class ZLibHelper
{
	public static void Decompress(Stream input, Stream output)
	{
		using ZLibStream zLibStream = new ZLibStream(input, CompressionMode.Decompress, leaveOpen: true);
		zLibStream.CopyTo(output);
	}
}
