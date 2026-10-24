namespace PsdSharp.Images;

internal struct ChannelInfo
{
	public Channel Channel => _003Cchannel_003EP;

	public int PixelCount => _003CpixelCount_003EP;

	public ChannelInfo(Channel channel, int pixelCount)
	{
		_003Cchannel_003EP = channel;
		_003CpixelCount_003EP = pixelCount;
	}
}
