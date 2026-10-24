using System;
using System.IO;

namespace Waylith.VideoDecode.Aac;

internal sealed class AacDecoder
{
	private sealed class Channel
	{
		public int WindowSequence;

		public int WindowShape;

		public int PreviousShape;

		public int MaxSfb;

		public int WindowCount;

		public int GroupCount;

		public ushort[] Bands;

		public readonly int[] GroupLength = new int[8];

		public readonly byte[] Books = new byte[512];

		public readonly int[] ScaleFactors = new int[512];

		public readonly uint[] NoiseSeeds = new uint[512];

		public readonly int[] Quantized = new int[1024];

		public readonly float[] Spectrum = new float[1024];

		public readonly float[] Overlap = new float[1024];

		public bool PulsePresent;

		public int PulseCount;

		public int PulseStartBand;

		public readonly int[] PulseOffset = new int[4];

		public readonly int[] PulseAmplitude = new int[4];

		public bool TnsPresent;

		public readonly int[] TnsFilterCount = new int[8];

		public readonly int[] TnsLength = new int[32];

		public readonly int[] TnsOrder = new int[32];

		public readonly bool[] TnsBackwards = new bool[32];

		public readonly float[] TnsReflection = new float[1024];
	}

	public const int FrameLength = 1024;

	private const int ElementSingle = 0;

	private const int ElementPair = 1;

	private const int ElementCoupling = 2;

	private const int ElementLfe = 3;

	private const int ElementData = 4;

	private const int ElementProgramConfig = 5;

	private const int ElementFill = 6;

	private const int ElementEnd = 7;

	private const int BookZero = 0;

	private const int BookEscape = 11;

	private const int BookNoise = 13;

	private const int BookIntensityOut = 14;

	private const int BookIntensityIn = 15;

	private const int BandStride = 64;

	private const int MaxTnsOrder = 12;

	private static readonly AacHuffman ScaleFactorBook = new AacHuffman(AacTables.ScaleFactorLengths, AacTables.ScaleFactorCodes);

	private static readonly AacHuffman[] SpectrumBooks = new AacHuffman[11]
	{
		new AacHuffman(AacTables.Book1Lengths, AacTables.Book1Codes),
		new AacHuffman(AacTables.Book2Lengths, AacTables.Book2Codes),
		new AacHuffman(AacTables.Book3Lengths, AacTables.Book3Codes),
		new AacHuffman(AacTables.Book4Lengths, AacTables.Book4Codes),
		new AacHuffman(AacTables.Book5Lengths, AacTables.Book5Codes),
		new AacHuffman(AacTables.Book6Lengths, AacTables.Book6Codes),
		new AacHuffman(AacTables.Book7Lengths, AacTables.Book7Codes),
		new AacHuffman(AacTables.Book8Lengths, AacTables.Book8Codes),
		new AacHuffman(AacTables.Book9Lengths, AacTables.Book9Codes),
		new AacHuffman(AacTables.Book10Lengths, AacTables.Book10Codes),
		new AacHuffman(AacTables.Book11Lengths, AacTables.Book11Codes)
	};

	private static readonly sbyte[][] SignedValues = new sbyte[6][]
	{
		AacTables.Book1Values,
		AacTables.Book2Values,
		null,
		null,
		AacTables.Book5Values,
		AacTables.Book6Values
	};

	private static readonly byte[][] UnsignedValues = new byte[11][]
	{
		null,
		null,
		AacTables.Book3Values,
		AacTables.Book4Values,
		null,
		null,
		AacTables.Book7Values,
		AacTables.Book8Values,
		AacTables.Book9Values,
		AacTables.Book10Values,
		AacTables.Book11Values
	};

	private static readonly float[] PowerFourThirds = BuildPowerTable();

	public readonly int SampleRate;

	public readonly int Channels;

	private readonly int rateIndex;

	private readonly Channel[] channels;

	private readonly Channel[] spare = new Channel[2]
	{
		new Channel(),
		new Channel()
	};

	private readonly AacFilterBank filterBank = new AacFilterBank();

	private readonly bool[] midSideUsed = new bool[512];

	private readonly float[] discard = new float[1024];

	private int midSideMask;

	private uint noiseState = 523124044u;

	public AacDecoder(ReadOnlySpan<byte> audioSpecificConfig)
	{
		AacBits bits = new AacBits(audioSpecificConfig);
		int num = ReadObjectType(ref bits);
		rateIndex = bits.Read(4);
		if (rateIndex == 15)
		{
			bits.Skip(24);
		}
		Channels = bits.Read(4);
		if ((num == 5 || num == 29) ? true : false)
		{
			if (bits.Read(4) == 15)
			{
				bits.Skip(24);
			}
			num = ReadObjectType(ref bits);
		}
		if (num != 2)
		{
			throw new NotSupportedException($"aac object type {num} is not supported (only LC)");
		}
		if (bits.Read(1) != 0)
		{
			throw new NotSupportedException("aac with 960 sample frames is not supported");
		}
		if (bits.Overrun || rateIndex >= AacTables.BandsLong.Length)
		{
			throw new InvalidDataException("aac: bad decoder config");
		}
		int num2 = Channels;
		if ((uint)(num2 - 1) > 1u)
		{
			throw new NotSupportedException($"aac channel configuration {Channels} is not supported");
		}
		SampleRate = AacTables.SampleRates[rateIndex];
		channels = new Channel[Channels];
		for (int i = 0; i < Channels; i++)
		{
			channels[i] = new Channel();
		}
	}

	private static int ReadObjectType(ref AacBits bits)
	{
		int num = bits.Read(5);
		if (num != 31)
		{
			return num;
		}
		return 32 + bits.Read(6);
	}

	public static ReadOnlySpan<byte> ConfigFromEsds(ReadOnlySpan<byte> esds)
	{
		int at = 4;
		if (ReadDescriptor(esds, ref at, out var size) != 3)
		{
			throw new InvalidDataException("esds: no ES descriptor");
		}
		byte num = esds[at + 2];
		at += 3;
		if ((num & 0x80) != 0)
		{
			at += 2;
		}
		if ((num & 0x40) != 0)
		{
			at += 1 + esds[at];
		}
		if ((num & 0x20) != 0)
		{
			at += 2;
		}
		if (ReadDescriptor(esds, ref at, out size) != 4)
		{
			throw new InvalidDataException("esds: no decoder config");
		}
		byte b = esds[at];
		if ((b != 64 && (uint)(b - 102) > 2u) || 1 == 0)
		{
			throw new NotSupportedException($"mp4 audio object type 0x{b:x2} is not aac");
		}
		at += 13;
		if (ReadDescriptor(esds, ref at, out var size2) != 5)
		{
			throw new InvalidDataException("esds: no decoder specific info");
		}
		return esds.Slice(at, size2);
	}

	private static int ReadDescriptor(ReadOnlySpan<byte> data, ref int at, out int size)
	{
		byte result = data[at++];
		size = 0;
		for (int i = 0; i < 4; i++)
		{
			byte b = data[at++];
			size = (size << 7) | (b & 0x7F);
			if ((b & 0x80) == 0)
			{
				break;
			}
		}
		return result;
	}

	public void Reset()
	{
		Channel[] array = channels;
		foreach (Channel obj in array)
		{
			Array.Clear(obj.Overlap);
			obj.PreviousShape = 0;
		}
	}

	public void Decode(ReadOnlySpan<byte> frame, Span<float> output)
	{
		AacBits bits = new AacBits(frame);
		int i = 0;
		while (!bits.Overrun)
		{
			int num = bits.Read(3);
			switch (num)
			{
			case 0:
			case 3:
			{
				bits.Skip(4);
				bool flag = num == 0 && i < Channels;
				Channel channel = (flag ? channels[i] : spare[0]);
				ReadChannelStream(ref bits, channel, commonWindow: false);
				Dequantize(channel, null);
				FinishChannel(channel, flag ? output.Slice(i) : ((Span<float>)discard), (!flag) ? 1 : Channels);
				if (num == 0)
				{
					i++;
				}
				continue;
			}
			case 1:
			{
				bits.Skip(4);
				bool flag2 = i < Channels;
				bool flag3 = i + 1 < Channels;
				Channel channel2 = (flag2 ? channels[i] : spare[0]);
				Channel channel3 = (flag3 ? channels[i + 1] : spare[1]);
				ReadPair(ref bits, channel2, channel3);
				FinishChannel(channel2, flag2 ? output.Slice(i) : ((Span<float>)discard), (!flag2) ? 1 : Channels);
				FinishChannel(channel3, flag3 ? output.Slice(i + 1) : ((Span<float>)discard), (!flag3) ? 1 : Channels);
				i += 2;
				continue;
			}
			case 4:
			{
				bits.Skip(4);
				bool num3 = bits.Read(1) != 0;
				int num4 = bits.Read(8);
				if (num4 == 255)
				{
					num4 += bits.Read(8);
				}
				if (num3)
				{
					bits.Align();
				}
				bits.Skip(8 * num4);
				continue;
			}
			case 6:
			{
				int num2 = bits.Read(4);
				if (num2 == 15)
				{
					num2 += bits.Read(8) - 1;
				}
				bits.Skip(8 * num2);
				continue;
			}
			case 5:
				SkipProgramConfig(ref bits);
				continue;
			default:
				throw new NotSupportedException("aac coupling channels are not supported");
			case 7:
				break;
			}
			break;
		}
		if (bits.Overrun)
		{
			throw new InvalidDataException("aac: frame ran past its end");
		}
		for (; i < Channels; i++)
		{
			Channel channel4 = channels[i];
			for (int j = 0; j < 1024; j++)
			{
				output[j * Channels + i] = channel4.Overlap[j];
			}
			Array.Clear(channel4.Overlap);
		}
	}

	private static void SkipProgramConfig(ref AacBits bits)
	{
		bits.Skip(10);
		int num = bits.Read(4);
		int num2 = bits.Read(4);
		int num3 = bits.Read(4);
		int num4 = bits.Read(2);
		int num5 = bits.Read(3);
		int num6 = bits.Read(4);
		if (bits.Read(1) != 0)
		{
			bits.Skip(4);
		}
		if (bits.Read(1) != 0)
		{
			bits.Skip(4);
		}
		if (bits.Read(1) != 0)
		{
			bits.Skip(3);
		}
		bits.Skip(5 * (num + num2 + num3 + num6) + 4 * (num4 + num5));
		bits.Align();
		bits.Skip(8 * bits.Read(8));
	}

	private void ReadPair(ref AacBits bits, Channel left, Channel right)
	{
		bool flag = bits.Read(1) != 0;
		midSideMask = 0;
		if (flag)
		{
			ReadWindowInfo(ref bits, left);
			right.WindowSequence = left.WindowSequence;
			right.WindowShape = left.WindowShape;
			right.MaxSfb = left.MaxSfb;
			right.WindowCount = left.WindowCount;
			right.GroupCount = left.GroupCount;
			right.Bands = left.Bands;
			Array.Copy(left.GroupLength, right.GroupLength, 8);
			midSideMask = bits.Read(2);
			if (midSideMask == 3)
			{
				throw new InvalidDataException("aac: reserved mid side mask");
			}
			for (int i = 0; i < left.GroupCount; i++)
			{
				for (int j = 0; j < left.MaxSfb; j++)
				{
					midSideUsed[i * 64 + j] = midSideMask == 2 || (midSideMask == 1 && bits.Read(1) != 0);
				}
			}
		}
		ReadChannelStream(ref bits, left, flag);
		ReadChannelStream(ref bits, right, flag);
		Dequantize(left, null);
		Dequantize(right, flag ? left : null);
		if (flag)
		{
			ApplyStereo(left, right);
		}
	}

	private void ReadWindowInfo(ref AacBits bits, Channel channel)
	{
		bits.Skip(1);
		channel.WindowSequence = bits.Read(2);
		channel.WindowShape = bits.Read(1);
		channel.GroupCount = 1;
		channel.GroupLength[0] = 1;
		if (channel.WindowSequence == 2)
		{
			channel.MaxSfb = bits.Read(4);
			int num = bits.Read(7);
			channel.WindowCount = 8;
			channel.Bands = AacTables.BandsShort[rateIndex];
			for (int num2 = 6; num2 >= 0; num2--)
			{
				if (((num >> num2) & 1) != 0)
				{
					channel.GroupLength[channel.GroupCount - 1]++;
				}
				else
				{
					channel.GroupLength[channel.GroupCount++] = 1;
				}
			}
		}
		else
		{
			channel.MaxSfb = bits.Read(6);
			if (bits.Read(1) != 0)
			{
				throw new NotSupportedException("aac prediction is not supported");
			}
			channel.WindowCount = 1;
			channel.Bands = AacTables.BandsLong[rateIndex];
		}
		if (channel.MaxSfb > channel.Bands.Length - 1)
		{
			throw new InvalidDataException("aac: more scale factor bands than the sample rate has");
		}
	}

	private void ReadChannelStream(ref AacBits bits, Channel channel, bool commonWindow)
	{
		int globalGain = bits.Read(8);
		if (!commonWindow)
		{
			ReadWindowInfo(ref bits, channel);
		}
		ReadSections(ref bits, channel);
		ReadScaleFactors(ref bits, channel, globalGain);
		channel.PulsePresent = bits.Read(1) != 0;
		if (channel.PulsePresent)
		{
			if (channel.WindowSequence == 2)
			{
				throw new InvalidDataException("aac: pulse data in a short block");
			}
			channel.PulseCount = bits.Read(2) + 1;
			channel.PulseStartBand = bits.Read(6);
			if (channel.PulseStartBand >= channel.Bands.Length - 1)
			{
				throw new InvalidDataException("aac: pulse data starts past the last band");
			}
			for (int i = 0; i < channel.PulseCount; i++)
			{
				channel.PulseOffset[i] = bits.Read(5);
				channel.PulseAmplitude[i] = bits.Read(4);
			}
		}
		channel.TnsPresent = bits.Read(1) != 0;
		if (channel.TnsPresent)
		{
			ReadTns(ref bits, channel);
		}
		if (bits.Read(1) != 0)
		{
			throw new NotSupportedException("aac gain control is not supported");
		}
		ReadSpectrum(ref bits, channel);
	}

	private static void ReadSections(ref AacBits bits, Channel channel)
	{
		int num = ((channel.WindowSequence == 2) ? 3 : 5);
		int num2 = (1 << num) - 1;
		for (int i = 0; i < channel.GroupCount; i++)
		{
			int num3 = 0;
			while (num3 < channel.MaxSfb)
			{
				int num4 = bits.Read(4);
				if (num4 == 12)
				{
					throw new InvalidDataException("aac: reserved codebook");
				}
				int num5 = 0;
				int num6;
				while ((num6 = bits.Read(num)) == num2 && !bits.Overrun)
				{
					num5 += num6;
				}
				num5 += num6;
				if (num5 == 0 || num3 + num5 > channel.MaxSfb)
				{
					throw new InvalidDataException("aac: bad section length");
				}
				for (int j = 0; j < num5; j++)
				{
					channel.Books[i * 64 + num3++] = (byte)num4;
				}
			}
		}
	}

	private static void ReadScaleFactors(ref AacBits bits, Channel channel, int globalGain)
	{
		int num = globalGain;
		int num2 = 0;
		int num3 = globalGain - 90;
		bool flag = true;
		for (int i = 0; i < channel.GroupCount; i++)
		{
			for (int j = 0; j < channel.MaxSfb; j++)
			{
				int num4 = i * 64 + j;
				switch (channel.Books[num4])
				{
				case 0:
					channel.ScaleFactors[num4] = 0;
					break;
				case 14:
				case 15:
					num2 += ReadScaleDelta(ref bits);
					channel.ScaleFactors[num4] = num2;
					break;
				case 13:
					num3 += (flag ? (bits.Read(9) - 256) : ReadScaleDelta(ref bits));
					flag = false;
					channel.ScaleFactors[num4] = num3;
					break;
				default:
					num += ReadScaleDelta(ref bits);
					channel.ScaleFactors[num4] = num;
					break;
				}
			}
		}
	}

	private static int ReadScaleDelta(ref AacBits bits)
	{
		return AacTables.ScaleFactorValues[ScaleFactorBook.Decode(ref bits)] - 60;
	}

	private static void ReadTns(ref AacBits bits, Channel channel)
	{
		bool flag = channel.WindowSequence == 2;
		for (int i = 0; i < channel.WindowCount; i++)
		{
			int num = bits.Read(flag ? 1 : 2);
			channel.TnsFilterCount[i] = num;
			if (num == 0)
			{
				continue;
			}
			int num2 = bits.Read(1) + 3;
			for (int j = 0; j < num; j++)
			{
				int num3 = i * 4 + j;
				channel.TnsLength[num3] = bits.Read(flag ? 4 : 6);
				int num4 = bits.Read(flag ? 3 : 5);
				channel.TnsOrder[num3] = num4;
				if (num4 == 0)
				{
					continue;
				}
				channel.TnsBackwards[num3] = bits.Read(1) != 0;
				int num5 = num2 - bits.Read(1);
				float num6 = (float)(System.Math.PI / 2.0 / ((double)(1 << num2 - 1) - 0.5));
				float num7 = (float)(System.Math.PI / 2.0 / ((double)(1 << num2 - 1) + 0.5));
				for (int k = 0; k < num4; k++)
				{
					int num8 = bits.Read(num5);
					if ((num8 & (1 << num5 - 1)) != 0)
					{
						num8 -= 1 << num5;
					}
					channel.TnsReflection[num3 * 32 + k] = MathF.Sin((float)num8 * ((num8 >= 0) ? num6 : num7));
				}
			}
		}
	}

	private static void ReadSpectrum(ref AacBits bits, Channel channel)
	{
		Array.Clear(channel.Quantized);
		ushort[] bands = channel.Bands;
		int num = 0;
		for (int i = 0; i < channel.GroupCount; i++)
		{
			int num2 = channel.GroupLength[i];
			for (int j = 0; j < channel.MaxSfb; j++)
			{
				byte b = channel.Books[i * 64 + j];
				if ((b >= 13 || b == 0) ? true : false)
				{
					continue;
				}
				AacHuffman huffman = SpectrumBooks[b - 1];
				int length = bands[j + 1] - bands[j];
				for (int k = 0; k < num2; k++)
				{
					int start = (num + k) * 128 + bands[j];
					Span<int> quantized = channel.Quantized.AsSpan(start, length);
					if (b <= 2)
					{
						ReadSigned(ref bits, huffman, SignedValues[b - 1], 4, quantized);
					}
					else if (b <= 4)
					{
						ReadUnsigned(ref bits, huffman, UnsignedValues[b - 1], 4, quantized, escapes: false);
					}
					else if (b <= 6)
					{
						ReadSigned(ref bits, huffman, SignedValues[b - 1], 2, quantized);
					}
					else
					{
						ReadUnsigned(ref bits, huffman, UnsignedValues[b - 1], 2, quantized, b == 11);
					}
				}
			}
			num += num2;
		}
		if (!channel.PulsePresent)
		{
			return;
		}
		int num3 = bands[channel.PulseStartBand];
		for (int l = 0; l < channel.PulseCount; l++)
		{
			num3 += channel.PulseOffset[l];
			if (num3 >= 1024)
			{
				throw new InvalidDataException("aac: pulse past the end of the spectrum");
			}
			channel.Quantized[num3] += ((channel.Quantized[num3] > 0) ? channel.PulseAmplitude[l] : (-channel.PulseAmplitude[l]));
		}
	}

	private static void ReadSigned(ref AacBits bits, AacHuffman huffman, sbyte[] values, int dimension, Span<int> quantized)
	{
		for (int i = 0; i < quantized.Length; i += dimension)
		{
			int num = huffman.Decode(ref bits) * dimension;
			for (int j = 0; j < dimension; j++)
			{
				quantized[i + j] = values[num + j];
			}
		}
	}

	private static void ReadUnsigned(ref AacBits bits, AacHuffman huffman, byte[] values, int dimension, Span<int> quantized, bool escapes)
	{
		for (int i = 0; i < quantized.Length; i += dimension)
		{
			int num = huffman.Decode(ref bits) * dimension;
			for (int j = 0; j < dimension; j++)
			{
				int num2 = values[num + j];
				quantized[i + j] = ((num2 != 0 && bits.Read(1) != 0) ? (-num2) : num2);
			}
			if (!escapes)
			{
				continue;
			}
			for (int k = 0; k < dimension; k++)
			{
				int num3 = quantized[i + k];
				if ((num3 == -16 || num3 == 16) ? true : false)
				{
					int num4 = 4;
					while (bits.Read(1) != 0 && num4 < 13)
					{
						num4++;
					}
					int num5 = (1 << num4) + bits.Read(num4);
					quantized[i + k] = ((num3 < 0) ? (-num5) : num5);
				}
			}
		}
	}

	private static float[] BuildPowerTable()
	{
		float[] array = new float[8192];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = (float)System.Math.Pow(i, 1.3333333333333333);
		}
		return array;
	}

	private void Dequantize(Channel channel, Channel pairedLeft)
	{
		Array.Clear(channel.Spectrum);
		ushort[] bands = channel.Bands;
		int num = 0;
		for (int i = 0; i < channel.GroupCount; i++)
		{
			int num2 = channel.GroupLength[i];
			for (int j = 0; j < channel.MaxSfb; j++)
			{
				int num3 = i * 64 + j;
				byte b = channel.Books[num3];
				if ((b == 0 || (uint)(b - 14) <= 1u) ? true : false)
				{
					continue;
				}
				int num4 = bands[j + 1] - bands[j];
				if (b == 13)
				{
					bool flag = pairedLeft != null && midSideUsed[num3] && pairedLeft.Books[num3] == 13;
					uint num5 = (flag ? pairedLeft.NoiseSeeds[num3] : noiseState);
					channel.NoiseSeeds[num3] = num5;
					float num6 = MathF.Pow(2f, 0.25f * (float)channel.ScaleFactors[num3]);
					for (int k = 0; k < num2; k++)
					{
						Span<float> span = channel.Spectrum.AsSpan((num + k) * 128 + bands[j], num4);
						float num7 = 0f;
						for (int l = 0; l < num4; l++)
						{
							num5 = num5 * 1664525 + 1013904223;
							span[l] = (int)num5;
							num7 += span[l] * span[l];
						}
						float num8 = num6 / MathF.Sqrt(num7);
						for (int m = 0; m < num4; m++)
						{
							span[m] *= num8;
						}
					}
					if (!flag)
					{
						noiseState = num5;
					}
					continue;
				}
				float num9 = MathF.Pow(2f, 0.25f * (float)(channel.ScaleFactors[num3] - 100));
				for (int n = 0; n < num2; n++)
				{
					int num10 = (num + n) * 128 + bands[j];
					for (int num11 = num10; num11 < num10 + num4; num11++)
					{
						int num12 = channel.Quantized[num11];
						int num13 = System.Math.Abs(num12);
						float num14 = ((num13 < PowerFourThirds.Length) ? PowerFourThirds[num13] : MathF.Pow(num13, 1.3333334f));
						channel.Spectrum[num11] = ((num12 < 0) ? ((0f - num14) * num9) : (num14 * num9));
					}
				}
			}
			num += num2;
		}
	}

	private void ApplyStereo(Channel left, Channel right)
	{
		ushort[] bands = left.Bands;
		int num = 0;
		for (int i = 0; i < left.GroupCount; i++)
		{
			int num2 = left.GroupLength[i];
			for (int j = 0; j < left.MaxSfb; j++)
			{
				int num3 = i * 64 + j;
				byte b = right.Books[num3];
				int num4 = bands[j + 1] - bands[j];
				if ((uint)(b - 14) <= 1u)
				{
					float num5 = ((b == 15) ? 1f : (-1f));
					if (midSideMask == 1 && midSideUsed[num3])
					{
						num5 = 0f - num5;
					}
					float num6 = num5 * MathF.Pow(0.5f, 0.25f * (float)right.ScaleFactors[num3]);
					for (int k = 0; k < num2; k++)
					{
						int num7 = (num + k) * 128 + bands[j];
						for (int l = num7; l < num7 + num4; l++)
						{
							right.Spectrum[l] = left.Spectrum[l] * num6;
						}
					}
				}
				else
				{
					if (!midSideUsed[num3] || b == 13 || left.Books[num3] == 13)
					{
						continue;
					}
					for (int m = 0; m < num2; m++)
					{
						int num8 = (num + m) * 128 + bands[j];
						for (int n = num8; n < num8 + num4; n++)
						{
							float num9 = left.Spectrum[n];
							float num10 = right.Spectrum[n];
							left.Spectrum[n] = num9 + num10;
							right.Spectrum[n] = num9 - num10;
						}
					}
				}
			}
			num += num2;
		}
	}

	private void FinishChannel(Channel channel, Span<float> output, int stride)
	{
		if (channel.TnsPresent)
		{
			ApplyTns(channel);
		}
		filterBank.Process(channel.Spectrum, channel.WindowSequence, channel.WindowShape, channel.PreviousShape, channel.Overlap, output, stride);
		channel.PreviousShape = channel.WindowShape;
	}

	private void ApplyTns(Channel channel)
	{
		bool flag = channel.WindowSequence == 2;
		ushort[] bands = channel.Bands;
		int num = bands.Length - 1;
		int val = System.Math.Min(channel.MaxSfb, flag ? AacTables.TnsMaxBandsShort[rateIndex] : AacTables.TnsMaxBandsLong[rateIndex]);
		int val2 = (flag ? 7 : 12);
		Span<float> span = stackalloc float[32];
		Span<float> span2 = stackalloc float[32];
		for (int i = 0; i < channel.WindowCount; i++)
		{
			int num2 = num;
			for (int j = 0; j < channel.TnsFilterCount[i]; j++)
			{
				int num3 = i * 4 + j;
				int num4 = num2;
				num2 = System.Math.Max(num4 - channel.TnsLength[num3], 0);
				int num5 = System.Math.Min(channel.TnsOrder[num3], val2);
				if (num5 == 0)
				{
					continue;
				}
				span[0] = 1f;
				for (int k = 1; k <= num5; k++)
				{
					float num6 = channel.TnsReflection[num3 * 32 + k - 1];
					for (int l = 1; l < k; l++)
					{
						span2[l] = span[l] + num6 * span[k - l];
					}
					for (int m = 1; m < k; m++)
					{
						span[m] = span2[m];
					}
					span[k] = num6;
				}
				int num7 = bands[System.Math.Min(num2, val)];
				int num8 = bands[System.Math.Min(num4, val)];
				int num9 = num8 - num7;
				if (num9 <= 0)
				{
					continue;
				}
				int num10 = 1;
				int num11 = i * 128 + num7;
				if (channel.TnsBackwards[num3])
				{
					num10 = -1;
					num11 = i * 128 + num8 - 1;
				}
				float[] spectrum = channel.Spectrum;
				int num12 = 0;
				while (num12 < num9)
				{
					float num13 = spectrum[num11];
					int num14 = System.Math.Min(num12, num5);
					for (int n = 1; n <= num14; n++)
					{
						num13 -= span[n] * spectrum[num11 - n * num10];
					}
					spectrum[num11] = num13;
					num12++;
					num11 += num10;
				}
			}
		}
	}
}
