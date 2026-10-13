using System;
using System.Buffers.Binary;
using System.IO;
using Fmod5Sharp.FmodTypes;
using Fmod5Sharp.Util;
using NAudio.Wave;

namespace Fmod5Sharp.CodecRebuilders;

public class FmodFadPcmRebuilder
{
	private static readonly short[,] FadpcmCoefs = new short[8, 2]
	{
		{ 0, 0 },
		{ 60, 0 },
		{ 122, 60 },
		{ 115, 52 },
		{ 98, 55 },
		{ 0, 0 },
		{ 0, 0 },
		{ 0, 0 }
	};

	public static short[] DecodeFadpcm(FmodSample sample)
	{
		ReadOnlySpan<byte> readOnlySpan = sample.SampleBytes;
		int numChannels = sample.Metadata.NumChannels;
		int num = readOnlySpan.Length / 140;
		short[] array = new short[num * 256];
		Span<short> span = array;
		int[] array2 = new int[numChannels];
		int[] array3 = new int[numChannels];
		for (int i = 0; i < num; i++)
		{
			int num2 = i % numChannels;
			int start = i * 140;
			ReadOnlySpan<byte> readOnlySpan2 = readOnlySpan.Slice(start, 140);
			uint num3 = BinaryPrimitives.ReadUInt32LittleEndian(readOnlySpan2.Slice(0, 4));
			uint num4 = BinaryPrimitives.ReadUInt32LittleEndian(readOnlySpan2.Slice(4, readOnlySpan2.Length - 4));
			array2[num2] = BinaryPrimitives.ReadInt16LittleEndian(readOnlySpan2.Slice(8, readOnlySpan2.Length - 8));
			array3[num2] = BinaryPrimitives.ReadInt16LittleEndian(readOnlySpan2.Slice(10, readOnlySpan2.Length - 10));
			int num5 = i / numChannels * 256 * numChannels + num2;
			for (int j = 0; j < 8; j++)
			{
				int num6 = (int)((num3 >> j * 4) & 0xF) % 7;
				int num7 = (int)((num4 >> j * 4) & 0xF);
				int num8 = FadpcmCoefs[num6, 0];
				int num9 = FadpcmCoefs[num6, 1];
				int num10 = 22 - num7;
				for (int k = 0; k < 4; k++)
				{
					int num11 = 12 + 16 * j + 4 * k;
					uint num12 = BinaryPrimitives.ReadUInt32LittleEndian(readOnlySpan2.Slice(num11, readOnlySpan2.Length - num11));
					for (int l = 0; l < 8; l++)
					{
						short num13 = Utils.ClampToShort(((int)(((num12 >> l * 4) & 0xF) << 28) >> num10) - array3[num2] * num9 + array2[num2] * num8 >> 6);
						int num14 = num5 + (j * 32 + k * 8 + l) * numChannels;
						if (num14 < span.Length)
						{
							span[num14] = num13;
						}
						array3[num2] = array2[num2];
						array2[num2] = num13;
					}
				}
			}
		}
		return array;
	}

	public static byte[] Rebuild(FmodSample sample)
	{
		WaveFormat format = new WaveFormat(sample.Metadata.Frequency, 16, sample.Metadata.NumChannels);
		using MemoryStream memoryStream = new MemoryStream();
		using (WaveFileWriter waveFileWriter = new WaveFileWriter(memoryStream, format))
		{
			short[] array = DecodeFadpcm(sample);
			waveFileWriter.WriteSamples(array, 0, array.Length);
		}
		return memoryStream.ToArray();
	}
}
