namespace Waylith.VideoDecode.Aac;

internal sealed class AacHuffman
{
	private readonly short[] tree;

	public AacHuffman(byte[] lengths, uint[] codes)
	{
		tree = new short[2 * lengths.Length];
		int num = 1;
		for (int i = 0; i < lengths.Length; i++)
		{
			int num2 = 0;
			for (int num3 = lengths[i] - 1; num3 > 0; num3--)
			{
				int num4 = 2 * num2 + (int)((codes[i] >> num3) & 1);
				if (tree[num4] == 0)
				{
					tree[num4] = (short)num++;
				}
				num2 = tree[num4];
			}
			tree[2 * num2 + (int)(codes[i] & 1)] = (short)(~i);
		}
	}

	public int Decode(ref AacBits bits)
	{
		int num = 0;
		do
		{
			num = tree[2 * num + bits.Read(1)];
		}
		while (num > 0);
		return ~num;
	}
}
