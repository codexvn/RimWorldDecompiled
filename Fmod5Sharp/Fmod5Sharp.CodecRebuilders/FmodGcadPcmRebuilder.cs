using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Fmod5Sharp.ChunkData;
using Fmod5Sharp.FmodTypes;
using Fmod5Sharp.Util;
using NAudio.Wave;

namespace Fmod5Sharp.CodecRebuilders;

public static class FmodGcadPcmRebuilder
{
	private const int BytesPerFrame = 8;

	private const int SamplesPerFrame = 14;

	private const int NibblesPerFrame = 16;

	private static short[] GetPcmData(FmodSample sample)
	{
		int num = ByteCountToSampleCount(sample.SampleBytes.Length);
		double num2 = Math.Ceiling((double)num / 14.0);
		short[] array = new short[num];
		byte[] sampleBytes = sample.SampleBytes;
		List<short> list = ((DspCoefficientsBlockData)sample.Metadata.Chunks.First((FmodSampleChunk c) => c.ChunkType == FmodSampleChunkType.DSPCOEFF).ChunkData).ChannelData[0];
		int num3 = 0;
		int num4 = 0;
		int num5 = 0;
		short num6 = 0;
		short num7 = 0;
		for (int i = 0; (double)i < num2; i++)
		{
			byte b = sampleBytes[num5++];
			int num8 = 1 << (b & 0xF);
			int num9 = b >> 4;
			short num10 = list[num9 * 2];
			short num11 = list[num9 * 2 + 1];
			int num12 = Math.Min(14, num - num3);
			for (int j = 0; j < num12; j++)
			{
				short num13 = Clamp16((((j % 2 == 0) ? Utils.GetHighNibbleSigned(sampleBytes[num5]) : Utils.GetLowNibbleSigned(sampleBytes[num5++])) * num8 << 11) + 1024 + num10 * num6 + num11 * num7 >> 11);
				num7 = num6;
				num6 = num13;
				array[num4++] = num13;
				num3++;
			}
		}
		return array;
	}

	public static byte[] Rebuild(FmodSample sample)
	{
		int num = ((!sample.Metadata.IsStereo) ? 1 : 2);
		WaveFormat format = WaveFormat.CreateCustomFormat(WaveFormatEncoding.Pcm, sample.Metadata.Frequency, num, sample.Metadata.Frequency * num * 2, num * 2, 16);
		using MemoryStream memoryStream = new MemoryStream();
		using WaveFileWriter waveFileWriter = new WaveFileWriter(memoryStream, format);
		short[] pcmData = GetPcmData(sample);
		waveFileWriter.WriteSamples(pcmData, 0, pcmData.Length);
		return memoryStream.ToArray();
	}

	private static int NibbleCountToSampleCount(int nibbleCount)
	{
		int num = nibbleCount / 16;
		int num2 = nibbleCount % 16;
		int num3 = ((num2 >= 2) ? (num2 - 2) : 0);
		return 14 * num + num3;
	}

	private static int ByteCountToSampleCount(int byteCount)
	{
		return NibbleCountToSampleCount(byteCount * 2);
	}

	private static short Clamp16(int value)
	{
		if (value > 32767)
		{
			return short.MaxValue;
		}
		if (value < -32768)
		{
			return short.MinValue;
		}
		return (short)value;
	}
}
