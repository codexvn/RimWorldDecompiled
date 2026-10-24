using System;
using System.Collections.Generic;
using System.IO;
using Waylith.Rendering;
using Waylith.VideoDecode.Vp8;

namespace Waylith.VideoDecode;

internal sealed class WebmVp8Source : VideoFrameSource
{
	private readonly Stream stream;

	private readonly List<MatroskaFrame> frames;

	private readonly Vp8Decoder decoder = new Vp8Decoder();

	private readonly CpuNv12Converter converter;

	private readonly Nv12ToRgbaPush push;

	private byte[] buffer = Array.Empty<byte>();

	private int next;

	private double skipBefore;

	public WebmVp8Source(VideoMedia media)
	{
		if (VideoDecodeService.Device == null)
		{
			throw new NotSupportedException("no graphics device");
		}
		stream = media.OpenStream();
		try
		{
			MatroskaDemuxer matroskaDemuxer = MatroskaDemuxer.Open(stream);
			MatroskaTrack matroskaTrack = matroskaDemuxer.Video ?? throw new InvalidDataException("no video track");
			if (matroskaTrack.Codec != "V_VP8")
			{
				throw new NotSupportedException("video codec '" + matroskaTrack.Codec + "' is not supported");
			}
			frames = matroskaTrack.Frames;
			if (frames.Count == 0 || !frames[0].Key)
			{
				throw new InvalidDataException("video track does not start with a key frame");
			}
			ReadOnlySpan<byte> readOnlySpan = ReadFrame(0);
			if (readOnlySpan.Length < 10)
			{
				throw new InvalidDataException("truncated vp8 key frame");
			}
			base.Width = (readOnlySpan[6] | (readOnlySpan[7] << 8)) & 0x3FFF;
			base.Height = (readOnlySpan[8] | (readOnlySpan[9] << 8)) & 0x3FFF;
			if (base.Width == 0 || base.Height == 0)
			{
				throw new InvalidDataException("vp8 key frame has no size");
			}
			base.FrameCount = 0L;
			for (int i = 0; i < frames.Count; i++)
			{
				stream.Position = frames[i].Offset;
				if (frames[i].Size > 0 && (stream.ReadByte() & 0x10) != 0)
				{
					base.FrameCount++;
				}
			}
			List<MatroskaFrame> list = frames;
			double num = list[list.Count - 1].Time - frames[0].Time;
			double num2 = ((matroskaTrack.FrameDuration > 0.0) ? matroskaTrack.FrameDuration : ((base.FrameCount > 1 && num > 0.0) ? (num / (double)(base.FrameCount - 1)) : (1.0 / 30.0)));
			base.FrameRate = 1.0 / num2;
			base.Length = System.Math.Max(matroskaDemuxer.Duration, num + num2);
			converter = new CpuNv12Converter(VideoDecodeService.Device);
			push = Nv12ToRgbaPush.Create(YuvMatrix.Bt601, fullRange: false, 0, 0, base.Width, base.Height);
		}
		catch
		{
			converter?.Dispose();
			decoder.Dispose();
			stream.Dispose();
			throw;
		}
	}

	private ReadOnlySpan<byte> ReadFrame(int index)
	{
		MatroskaFrame matroskaFrame = frames[index];
		if (buffer.Length < matroskaFrame.Size)
		{
			buffer = new byte[matroskaFrame.Size];
		}
		stream.Position = matroskaFrame.Offset;
		stream.ReadExactly(buffer, 0, matroskaFrame.Size);
		return buffer.AsSpan(0, matroskaFrame.Size);
	}

	public unsafe override bool ReadNext(List<DecodedPicture> output)
	{
		if (next >= frames.Count)
		{
			return false;
		}
		double num = frames[next].Time - frames[0].Time;
		if (!decoder.Decode(ReadFrame(next++)) || num < skipBefore)
		{
			return true;
		}
		Vp8Frame shown = decoder.Shown;
		GpuTextureAllocation gpuTextureAllocation = AcquireImage();
		converter.ConvertPlanar(shown.Y, shown.YStride, shown.U, shown.V, shown.UvStride, shown.Width, shown.Height, in push, gpuTextureAllocation);
		output.Add(new DecodedPicture
		{
			Image = gpuTextureAllocation,
			Time = num
		});
		return true;
	}

	public override void Seek(double seconds)
	{
		double num = seconds + frames[0].Time;
		next = 0;
		for (int i = 0; i < frames.Count; i++)
		{
			if (frames[i].Key && frames[i].Time <= num)
			{
				next = i;
			}
		}
		skipBefore = seconds - 0.5 / base.FrameRate;
	}

	public override void Dispose()
	{
		converter.Dispose();
		decoder.Dispose();
		stream.Dispose();
	}
}
