using Waylith.Rendering;

namespace Waylith.VideoDecode;

internal sealed class VideoFrame
{
	public GpuTextureAllocation Image;

	public double Time;

	public double StreamTime;

	public long Index;

	public int Loop;
}
