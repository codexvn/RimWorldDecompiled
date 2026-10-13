namespace Waylith.Rendering;

public static class BcEncodeTargetExtensions
{
	public static int BlockBytes(this BcEncodeTarget target)
	{
		switch (target)
		{
		case BcEncodeTarget.Bc1:
		case BcEncodeTarget.Bc4:
			return 8;
		case BcEncodeTarget.Bc7:
		case BcEncodeTarget.Bc5:
			return 16;
		default:
			return 0;
		}
	}
}
