using System.Collections.Generic;
using System.Threading;
using Waylith.Graphics;

namespace Waylith.VideoDecode;

public static class VideoDecodeService
{
	private static readonly Lock Lock = new Lock();

	private static readonly List<VideoPlayback> Live = new List<VideoPlayback>();

	internal static GraphicsDevice Device { get; private set; }

	public static bool PreferSystemDecoderForVp8 { get; set; }

	internal static bool HasVulkanVideo => Device?.VideoDecodeQueue != null;

	public static void Configure(GraphicsDevice graphicsDevice)
	{
		Device = graphicsDevice;
	}

	public static void Shutdown()
	{
		VideoPlayback[] array;
		using (Lock.EnterScope())
		{
			array = Live.ToArray();
		}
		VideoPlayback[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			array2[i].Dispose();
		}
		Device = null;
	}

	internal static void Track(VideoPlayback playback)
	{
		using (Lock.EnterScope())
		{
			Live.Add(playback);
		}
	}

	internal static void Untrack(VideoPlayback playback)
	{
		using (Lock.EnterScope())
		{
			Live.Remove(playback);
		}
	}
}
