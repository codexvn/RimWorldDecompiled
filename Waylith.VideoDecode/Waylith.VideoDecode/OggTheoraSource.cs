using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using Waylith.Rendering;
using Waylith.VideoDecode.Theora;

namespace Waylith.VideoDecode;

internal sealed class OggTheoraSource : VideoFrameSource
{
	private readonly Stream stream;

	private readonly OggDemuxer demuxer;

	private readonly TheoraDecoder decoder = new TheoraDecoder();

	private readonly CpuNv12Converter converter;

	private readonly Nv12ToRgbaPush push;

	private readonly double frameDuration;

	private byte[] buffer = Array.Empty<byte>();

	private unsafe byte* halvedChroma;

	private int next;

	private double skipBefore;

	public unsafe OggTheoraSource(VideoMedia media)
	{
		if (VideoDecodeService.Device == null)
		{
			throw new NotSupportedException("no graphics device");
		}
		stream = media.OpenStream();
		try
		{
			demuxer = OggDemuxer.Open(stream);
			foreach (byte[] header in demuxer.Headers)
			{
				decoder.ReadHeader(header);
			}
			if (demuxer.Frames.Count == 0 || !demuxer.Frames[0].Key)
			{
				throw new InvalidDataException("theora stream does not start with a key frame");
			}
			ref readonly TheoraInfo info = ref decoder.Info;
			base.Width = info.PictureWidth;
			base.Height = info.PictureHeight;
			if (base.Width == 0 || base.Height == 0)
			{
				throw new InvalidDataException("theora stream has an empty picture");
			}
			frameDuration = (double)info.FpsDenominator / (double)info.FpsNumerator;
			base.FrameRate = 1.0 / frameDuration;
			base.FrameCount = demuxer.Frames.Count;
			base.Length = (double)base.FrameCount * frameDuration;
			if (info.PixelFormat != 0)
			{
				halvedChroma = (byte*)NativeMemory.Alloc((nuint)(info.FrameWidth / 2 * (info.FrameHeight / 2) * 2));
			}
			converter = new CpuNv12Converter(VideoDecodeService.Device);
			push = Nv12ToRgbaPush.Create(YuvMatrix.Bt601, fullRange: false, info.PictureX, info.PictureY, base.Width, base.Height);
		}
		catch
		{
			converter?.Dispose();
			Free();
			throw;
		}
	}

	public unsafe override bool ReadNext(List<DecodedPicture> output)
	{
		if (next >= demuxer.Frames.Count)
		{
			return false;
		}
		double num = (double)next * frameDuration;
		decoder.Decode(demuxer.Read(stream, next++, ref buffer));
		if (num < skipBefore || !decoder.HasPicture)
		{
			return true;
		}
		ref readonly TheoraInfo info = ref decoder.Info;
		byte* ptr = decoder.Plane(1);
		byte* ptr2 = decoder.Plane(2);
		int num2 = decoder.PlaneStride(1);
		if (halvedChroma != null)
		{
			num2 = info.FrameWidth / 2;
			ptr = halvedChroma;
			ptr2 = halvedChroma + num2 * (info.FrameHeight / 2);
			HalveChroma(decoder.Plane(1), ptr);
			HalveChroma(decoder.Plane(2), ptr2);
		}
		GpuTextureAllocation gpuTextureAllocation = AcquireImage();
		converter.ConvertPlanar(decoder.Plane(0), decoder.PlaneStride(0), ptr, ptr2, num2, info.FrameWidth, info.FrameHeight, in push, gpuTextureAllocation);
		output.Add(new DecodedPicture
		{
			Image = gpuTextureAllocation,
			Time = num
		});
		return true;
	}

	private unsafe void HalveChroma(byte* source, byte* target)
	{
		ref readonly TheoraInfo info = ref decoder.Info;
		int num = decoder.PlaneStride(1);
		int num2 = info.FrameWidth / 2;
		int num3 = info.FrameHeight / 2;
		bool flag = decoder.PlaneHeight(1) == info.FrameHeight;
		bool flag2 = decoder.PlaneWidth(1) == info.FrameWidth;
		for (int i = 0; i < num3; i++)
		{
			byte* ptr = source + (flag ? (i * 2) : i) * num;
			byte* ptr2 = (flag ? (ptr + num) : ptr);
			if (flag2)
			{
				for (int j = 0; j < num2; j++)
				{
					target[j] = (byte)(ptr[j * 2] + ptr[j * 2 + 1] + ptr2[j * 2] + ptr2[j * 2 + 1] + 2 >> 2);
				}
			}
			else
			{
				for (int k = 0; k < num2; k++)
				{
					target[k] = (byte)(ptr[k] + ptr2[k] + 1 >> 1);
				}
			}
			target += num2;
		}
	}

	public override void Seek(double seconds)
	{
		int num = (int)System.Math.Clamp(System.Math.Round(seconds / frameDuration), 0.0, demuxer.Frames.Count - 1);
		next = 0;
		for (int num2 = num; num2 > 0; num2--)
		{
			if (demuxer.Frames[num2].Key)
			{
				next = num2;
				break;
			}
		}
		decoder.Reset();
		skipBefore = seconds - 0.5 * frameDuration;
	}

	private unsafe void Free()
	{
		if (halvedChroma != null)
		{
			NativeMemory.Free(halvedChroma);
		}
		halvedChroma = null;
		decoder.Dispose();
		stream.Dispose();
	}

	public override void Dispose()
	{
		converter.Dispose();
		Free();
	}
}
