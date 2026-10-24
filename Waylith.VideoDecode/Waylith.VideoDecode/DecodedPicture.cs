using Waylith.Rendering;

namespace Waylith.VideoDecode;

internal struct DecodedPicture
{
	public GpuTextureAllocation Image;

	public double Time;
}
