using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Waylith.Rendering;

namespace Waylith.VideoDecode;

internal sealed class Mp4VulkanSource : VideoFrameSource
{
	private readonly Stream stream;

	private readonly Mp4Sample[] samples;

	private readonly double timescale;

	private readonly VulkanH264Decoder decoder;

	private readonly List<VulkanH264Decoder.Output> decoded = new List<VulkanH264Decoder.Output>();

	private byte[] buffer = Array.Empty<byte>();

	private int next;

	private bool flushed;

	public Mp4VulkanSource(VideoMedia media)
	{
		stream = media.OpenStream();
		try
		{
			Mp4Track mp4Track = Mp4Demuxer.Open(stream).Video ?? throw new InvalidDataException("no video track");
			if (mp4Track.Samples.Length == 0)
			{
				throw new InvalidDataException("video track has no samples");
			}
			string codec = mp4Track.Codec;
			bool flag = ((codec == "avc1" || codec == "avc3") ? true : false);
			if (!flag || mp4Track.CodecConfig == null)
			{
				throw new NotSupportedException("video codec '" + mp4Track.Codec + "' is not supported");
			}
			if (!VideoDecodeService.HasVulkanVideo)
			{
				throw new NotSupportedException("the graphics driver has no Vulkan Video H.264 decode");
			}
			samples = mp4Track.Samples;
			timescale = mp4Track.Timescale;
			long num = ((mp4Track.Duration > 0) ? mp4Track.Duration : samples.Max((Mp4Sample s) => s.Pts));
			base.Length = (double)num / timescale;
			base.FrameCount = samples.Length;
			base.FrameRate = ((base.Length > 0.0) ? ((double)samples.Length / base.Length) : 30.0);
			decoder = new VulkanH264Decoder(VideoDecodeService.Device, mp4Track.CodecConfig);
			decoder.AcquireImage = () => AcquireImage();
			decoder.ReleaseImage = delegate(GpuTextureAllocation image)
			{
				ReleaseImage(image);
			};
			decoder.Prepare();
			base.Width = decoder.Width;
			base.Height = decoder.Height;
			base.ReorderDepth = decoder.ReorderDepth;
		}
		catch
		{
			decoder?.Dispose();
			stream.Dispose();
			throw;
		}
	}

	public override bool ReadNext(List<DecodedPicture> output)
	{
		decoded.Clear();
		if (next < samples.Length)
		{
			Mp4Sample mp4Sample = samples[next++];
			if (buffer.Length < mp4Sample.Size)
			{
				buffer = new byte[mp4Sample.Size];
			}
			stream.Position = mp4Sample.Offset;
			stream.ReadExactly(buffer, 0, mp4Sample.Size);
			decoder.Decode(buffer.AsSpan(0, mp4Sample.Size), mp4Sample.Pts, decoded);
		}
		else
		{
			if (flushed)
			{
				return false;
			}
			flushed = true;
			decoder.Flush(decoded);
		}
		foreach (VulkanH264Decoder.Output item in decoded)
		{
			output.Add(new DecodedPicture
			{
				Image = item.Image,
				Time = (double)item.Pts / timescale
			});
		}
		return true;
	}

	public override void Seek(double seconds)
	{
		long num = (long)(seconds * timescale);
		next = 0;
		for (int i = 0; i < samples.Length; i++)
		{
			if (samples[i].Sync && samples[i].Pts <= num)
			{
				next = i;
			}
		}
		flushed = false;
		decoder.Reset();
	}

	public override void Dispose()
	{
		decoder.Dispose();
		stream.Dispose();
	}
}
