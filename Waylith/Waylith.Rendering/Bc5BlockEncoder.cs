namespace Waylith.Rendering;

public static class Bc5BlockEncoder
{
	public unsafe static void EncodeRows(byte* rgba, int width, int height, int rowPitch, int firstBlockRow, int blockRowCount, byte* output)
	{
		Bc4BlockEncoder.EncodeRows(rgba, width, height, rowPitch, firstBlockRow, blockRowCount, 2, output);
	}
}
