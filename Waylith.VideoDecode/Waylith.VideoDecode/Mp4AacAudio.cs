using System;
using System.IO;
using Waylith.VideoDecode.Aac;

namespace Waylith.VideoDecode;

internal sealed class Mp4AacAudio : IDisposable
{
	private const int FrameLength = 1024;

	private readonly Stream stream;

	private readonly Mp4Sample[] packets;

	private readonly AacDecoder decoder;

	private readonly long trimmed;

	private readonly float[] frame;

	private byte[] buffer = Array.Empty<byte>();

	private int nextPacket;

	private int frameOffset = 1024;

	private long position;

	public int SampleRate => decoder.SampleRate;

	public int Channels => decoder.Channels;

	public long Length { get; }

	public static Mp4AacAudio TryOpen(Stream stream)
	{
		Mp4Track audio = Mp4Demuxer.Open(stream).Audio;
		if (audio == null || audio.Samples.Length == 0)
		{
			return null;
		}
		if (audio.Codec != "mp4a" || audio.CodecConfig == null)
		{
			throw new NotSupportedException("audio codec '" + audio.Codec + "' is not supported");
		}
		return new Mp4AacAudio(stream, audio);
	}

	private Mp4AacAudio(Stream stream, Mp4Track track)
	{
		this.stream = stream;
		packets = track.Samples;
		decoder = new AacDecoder(AacDecoder.ConfigFromEsds(track.CodecConfig));
		frame = new float[1024 * decoder.Channels];
		trimmed = System.Math.Max(0L, -packets[0].Pts * decoder.SampleRate / track.Timescale);
		Length = System.Math.Max(0L, (long)packets.Length * 1024L - trimmed);
		if (track.EditDuration > 0.0)
		{
			Length = System.Math.Min(Length, (long)System.Math.Round(track.EditDuration * (double)decoder.SampleRate));
		}
	}

	public void Seek(long sample)
	{
		position = System.Math.Clamp(sample, 0L, Length);
		long num = position + trimmed;
		int num2 = (int)System.Math.Min(num / 1024, packets.Length);
		decoder.Reset();
		nextPacket = System.Math.Max(num2 - 1, 0);
		if (nextPacket < num2)
		{
			DecodeNext();
		}
		frameOffset = 1024;
		if (num2 < packets.Length)
		{
			DecodeNext();
			frameOffset = (int)(num - (long)num2 * 1024L);
		}
	}

	public int Read(Span<float> interleaved)
	{
		int channels = decoder.Channels;
		int num = (int)System.Math.Min(interleaved.Length / channels, Length - position);
		int i;
		int num2;
		for (i = 0; i < num; i += num2)
		{
			if (frameOffset == 1024)
			{
				if (nextPacket >= packets.Length)
				{
					break;
				}
				DecodeNext();
				frameOffset = 0;
			}
			num2 = System.Math.Min(num - i, 1024 - frameOffset);
			frame.AsSpan(frameOffset * channels, num2 * channels).CopyTo(interleaved.Slice(i * channels));
			frameOffset += num2;
		}
		position += i;
		return i;
	}

	private void DecodeNext()
	{
		Mp4Sample mp4Sample = packets[nextPacket++];
		if (buffer.Length < mp4Sample.Size)
		{
			buffer = new byte[mp4Sample.Size];
		}
		stream.Position = mp4Sample.Offset;
		stream.ReadExactly(buffer, 0, mp4Sample.Size);
		try
		{
			decoder.Decode(buffer.AsSpan(0, mp4Sample.Size), frame);
		}
		catch (InvalidDataException)
		{
			Array.Clear(frame);
			decoder.Reset();
		}
	}

	public void Dispose()
	{
		stream.Dispose();
	}
}
