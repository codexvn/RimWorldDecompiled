using System;

namespace Waylith.VideoDecode;

internal sealed class Mp4Track
{
	public bool IsVideo;

	public bool IsAudio;

	public string Codec;

	public int Width;

	public int Height;

	public int Channels;

	public int SampleRate;

	public uint Timescale;

	public long Duration;

	public double EditDuration;

	public byte[] CodecConfig;

	public Mp4Sample[] Samples = Array.Empty<Mp4Sample>();

	public double Seconds(long ticks)
	{
		return (double)ticks / (double)Timescale;
	}
}
