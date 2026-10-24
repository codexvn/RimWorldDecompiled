namespace PsdSharp.Images;

public class ColorType
{
	public static ColorType Rgba8888 = new ColorType(new ChannelInfo[4]
	{
		new ChannelInfo(Channel.Red, 8),
		new ChannelInfo(Channel.Green, 8),
		new ChannelInfo(Channel.Blue, 8),
		new ChannelInfo(Channel.Alpha, 8)
	});

	public static ColorType Bgra8888 = new ColorType(new ChannelInfo[4]
	{
		new ChannelInfo(Channel.Blue, 8),
		new ChannelInfo(Channel.Green, 8),
		new ChannelInfo(Channel.Red, 8),
		new ChannelInfo(Channel.Alpha, 8)
	});

	internal ChannelInfo[] Channels { get; }

	public static ColorType Rgb888x => new ColorType(new ChannelInfo[4]
	{
		new ChannelInfo(Channel.Red, 8),
		new ChannelInfo(Channel.Green, 8),
		new ChannelInfo(Channel.Blue, 8),
		new ChannelInfo(Channel.Discard, 8)
	});

	internal ColorType(ChannelInfo[] channels)
	{
		Channels = channels;
	}
}
