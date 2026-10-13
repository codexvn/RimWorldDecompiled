namespace Waylith.Rendering;

public readonly record struct TextureLevelUpload(int Level, int Width, int Height, int Depth, int RowLengthTexels, byte[]? Bytes, int ByteCount)
{
	public int Size
	{
		get
		{
			byte[]? bytes = Bytes;
			if (bytes == null)
			{
				return ByteCount;
			}
			return bytes.Length;
		}
	}
}
