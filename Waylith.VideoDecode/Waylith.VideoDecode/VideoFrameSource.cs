using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Waylith.Rendering;

namespace Waylith.VideoDecode;

internal abstract class VideoFrameSource : IDisposable
{
	public Func<GpuTextureAllocation> AcquireImage;

	public Action<GpuTextureAllocation> ReleaseImage;

	private static readonly string ForcedBackend = Environment.GetEnvironmentVariable("WAYLITH_VIDEO_BACKEND");

	public int Width { get; protected set; }

	public int Height { get; protected set; }

	public double FrameRate { get; protected set; }

	public long FrameCount { get; protected set; }

	public double Length { get; protected set; }

	public int ReorderDepth { get; protected set; }

	public abstract bool ReadNext(List<DecodedPicture> output);

	public abstract void Seek(double seconds);

	public abstract void Dispose();

	public static VideoFrameSource Open(VideoMedia media)
	{
		Span<byte> span = stackalloc byte[4];
		int length;
		using (Stream stream = media.OpenStream())
		{
			length = stream.ReadAtLeast(span, span.Length, throwOnEndOfStream: false);
		}
		span = span.Slice(0, length);
		string forcedBackend = ForcedBackend;
		bool flag = ((forcedBackend == "vulkan" || forcedBackend == "managed") ? true : false);
		bool flag2 = !flag && MediaFoundationReader.IsAvailable;
		if (((ReadOnlySpan<byte>)span).SequenceEqual("OggS"u8))
		{
			return FirstThatOpens(media, (ForcedBackend != "mf") ? ((Func<VideoMedia, VideoFrameSource>)((VideoMedia m) => new OggTheoraSource(m))) : null);
		}
		if (!MatroskaDemuxer.IsMatroska(span))
		{
			return FirstThatOpens(media, (ForcedBackend != "mf") ? ((Func<VideoMedia, VideoFrameSource>)((VideoMedia m) => new Mp4VulkanSource(m))) : null, flag2 ? ((Func<VideoMedia, VideoFrameSource>)((VideoMedia m) => new MediaFoundationSource(m))) : null);
		}
		Func<VideoMedia, VideoFrameSource> func = ((ForcedBackend != "mf") ? ((Func<VideoMedia, VideoFrameSource>)((VideoMedia m) => new WebmVp8Source(m))) : null);
		Func<VideoMedia, VideoFrameSource> func2 = (flag2 ? ((Func<VideoMedia, VideoFrameSource>)((VideoMedia m) => new MediaFoundationSource(m))) : null);
		if (!VideoDecodeService.PreferSystemDecoderForVp8)
		{
			return FirstThatOpens(media, func, func2);
		}
		return FirstThatOpens(media, func2, func);
	}

	private static VideoFrameSource FirstThatOpens(VideoMedia media, params Func<VideoMedia, VideoFrameSource>[] backends)
	{
		backends = backends.Where((Func<VideoMedia, VideoFrameSource> b) => b != null).ToArray();
		if (backends.Length == 0)
		{
			throw new NotSupportedException("no decoder for this video on this platform");
		}
		List<string> list = new List<string>();
		for (int i = 0; i < backends.Length; i++)
		{
			try
			{
				return backends[i](media);
			}
			catch (Exception ex) when (i < backends.Length - 1)
			{
				list.Add(ex.Message);
			}
			catch (Exception ex2) when (list.Count > 0)
			{
				list.Add(ex2.Message);
				throw new NotSupportedException(string.Join("; ", list));
			}
		}
		throw new NotSupportedException(string.Join("; ", list));
	}
}
