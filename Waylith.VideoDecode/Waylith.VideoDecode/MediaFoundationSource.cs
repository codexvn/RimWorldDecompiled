using System;
using System.Collections.Generic;
using System.IO;
using Waylith.Rendering;
using Waylith.VideoDecode.H264;

namespace Waylith.VideoDecode;

internal sealed class MediaFoundationSource : VideoFrameSource
{
	private readonly MediaFoundationReader reader;

	private readonly CpuNv12Converter converter;

	private readonly string temporaryFile;

	private byte[] nv12;

	private double timeOrigin = double.NaN;

	private H264Sps sps;

	public MediaFoundationSource(VideoMedia media)
	{
		if (!MediaFoundationReader.IsAvailable)
		{
			throw new NotSupportedException("no decoder for this video on this platform");
		}
		if (VideoDecodeService.Device == null)
		{
			throw new NotSupportedException("no graphics device");
		}
		string path = media.Path;
		if (media.Data != null)
		{
			temporaryFile = Path.Combine(Path.GetTempPath(), $"waylith-video-{Guid.NewGuid():N}.bin");
			File.WriteAllBytes(temporaryFile, media.Data);
			path = temporaryFile;
		}
		try
		{
			reader = new MediaFoundationReader(Path.GetFullPath(path));
			converter = new CpuNv12Converter(VideoDecodeService.Device);
		}
		catch
		{
			reader?.Dispose();
			DeleteTemporaryFile();
			throw;
		}
		base.Width = reader.Width;
		base.Height = reader.Height;
		base.Length = reader.Length;
		base.FrameRate = ((reader.FrameRate > 0.0) ? reader.FrameRate : 30.0);
		base.FrameCount = (long)System.Math.Round(base.Length * base.FrameRate);
		base.ReorderDepth = 1;
		ReadMp4Details(media);
	}

	private void ReadMp4Details(VideoMedia media)
	{
		try
		{
			using Stream stream = media.OpenStream();
			Mp4Track video = Mp4Demuxer.Open(stream).Video;
			if (video != null && video.Samples.Length != 0 && video.Duration > 0)
			{
				base.Length = (double)video.Duration / (double)video.Timescale;
				base.FrameCount = video.Samples.Length;
				base.FrameRate = (double)base.FrameCount / base.Length;
				byte[] codecConfig = video.CodecConfig;
				string codec = video.Codec;
				bool flag = ((codec == "avc1" || codec == "avc3") ? true : false);
				if (flag && codecConfig != null && codecConfig.Length > 8 && (codecConfig[5] & 0x1F) > 0)
				{
					sps = new H264Parser().ParseSps(codecConfig.AsSpan(8, (codecConfig[6] << 8) | codecConfig[7]));
				}
			}
		}
		catch (Exception ex) when (((ex is InvalidDataException || ex is IOException || ex is IndexOutOfRangeException || ex is ArgumentException || ex is H264Exception) ? 1 : 0) != 0)
		{
		}
	}

	public override bool ReadNext(List<DecodedPicture> output)
	{
		if (!reader.ReadFrame(ref nv12, out var timestamp))
		{
			return false;
		}
		double num = (double)timestamp / 10000000.0;
		if (double.IsNaN(timeOrigin))
		{
			timeOrigin = num;
		}
		GpuTextureAllocation gpuTextureAllocation = AcquireImage();
		int num2;
		YuvMatrix num3;
		if (sps != null)
		{
			num2 = ((!reader.ReportsColour) ? 1 : 0);
			if (num2 != 0)
			{
				num3 = Nv12ToRgbaPush.MatrixFor(sps);
				goto IL_0078;
			}
		}
		else
		{
			num2 = 0;
		}
		num3 = reader.Matrix;
		goto IL_0078;
		IL_0078:
		YuvMatrix matrix = num3;
		bool fullRange = ((num2 == 0) ? reader.FullRange : (sps.VideoSignalTypePresent && sps.VideoFullRange));
		Nv12ToRgbaPush push = Nv12ToRgbaPush.Create(matrix, fullRange, reader.CropX, reader.CropY, base.Width, base.Height);
		converter.Convert(nv12, reader.Stride, reader.Rows, in push, gpuTextureAllocation);
		output.Add(new DecodedPicture
		{
			Image = gpuTextureAllocation,
			Time = System.Math.Max(0.0, num - timeOrigin)
		});
		return true;
	}

	public override void Seek(double seconds)
	{
		reader.Seek((seconds <= 0.0 || double.IsNaN(timeOrigin)) ? 0.0 : (seconds + timeOrigin));
	}

	private void DeleteTemporaryFile()
	{
		if (temporaryFile == null)
		{
			return;
		}
		try
		{
			File.Delete(temporaryFile);
		}
		catch (IOException)
		{
		}
	}

	public override void Dispose()
	{
		converter.Dispose();
		reader.Dispose();
		DeleteTemporaryFile();
	}
}
