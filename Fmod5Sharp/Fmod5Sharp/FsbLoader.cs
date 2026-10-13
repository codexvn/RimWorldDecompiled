using System;
using System.Collections.Generic;
using System.IO;
using Fmod5Sharp.FmodTypes;
using Fmod5Sharp.Util;

namespace Fmod5Sharp;

public static class FsbLoader
{
	internal static readonly Dictionary<uint, int> Frequencies = new Dictionary<uint, int>
	{
		{ 0u, 4000 },
		{ 1u, 8000 },
		{ 2u, 11000 },
		{ 3u, 12000 },
		{ 4u, 16000 },
		{ 5u, 22050 },
		{ 6u, 24000 },
		{ 7u, 32000 },
		{ 8u, 44100 },
		{ 9u, 48000 },
		{ 10u, 96000 }
	};

	private static FmodSoundBank? LoadInternal(Stream stream, bool throwIfError)
	{
		using BinaryReader binaryReader = new BinaryReader(stream);
		FmodAudioHeader fmodAudioHeader = new FmodAudioHeader(binaryReader);
		if (!fmodAudioHeader.IsValid)
		{
			if (throwIfError)
			{
				throw new Exception("File is probably not an FSB file (magic number mismatch)");
			}
			return null;
		}
		long num = fmodAudioHeader.SizeOfThisHeader + fmodAudioHeader.SizeOfNameTable + fmodAudioHeader.SizeOfSampleHeaders;
		List<FmodSample> list = new List<FmodSample>(fmodAudioHeader.Samples.Count);
		for (int i = 0; i < fmodAudioHeader.Samples.Count; i++)
		{
			FmodSampleMetadata fmodSampleMetadata = fmodAudioHeader.Samples[i];
			long num2 = fmodSampleMetadata.DataOffset;
			long num3 = fmodAudioHeader.SizeOfData;
			if (i < fmodAudioHeader.Samples.Count - 1)
			{
				num3 = fmodAudioHeader.Samples[i + 1].DataOffset;
			}
			byte[] array = new byte[num3 - num2];
			stream.Position = num + num2;
			stream.ReadExactly(array, 0, array.Length);
			FmodSample fmodSample = new FmodSample(fmodSampleMetadata, array);
			if (fmodAudioHeader.SizeOfNameTable != 0)
			{
				long position = fmodAudioHeader.SizeOfThisHeader + fmodAudioHeader.SizeOfSampleHeaders + 4 * i;
				binaryReader.BaseStream.Position = position;
				uint num4 = binaryReader.ReadUInt32();
				num4 += fmodAudioHeader.SizeOfThisHeader + fmodAudioHeader.SizeOfSampleHeaders;
				stream.Position = num4;
				fmodSample.Name = stream.ReadNullTerminatedString();
			}
			list.Add(fmodSample);
		}
		return new FmodSoundBank(fmodAudioHeader, list);
	}

	public static bool TryLoadFsbFromStream(Stream stream, out FmodSoundBank? bank)
	{
		bank = LoadInternal(stream, throwIfError: false);
		return bank != null;
	}

	public static FmodSoundBank LoadFsbFromStream(Stream stream)
	{
		return LoadInternal(stream, throwIfError: true);
	}

	public static bool TryLoadFsbFromByteArray(byte[] bankBytes, out FmodSoundBank? bank)
	{
		using MemoryStream stream = new MemoryStream(bankBytes);
		bank = LoadInternal(stream, throwIfError: false);
		return bank != null;
	}

	public static FmodSoundBank LoadFsbFromByteArray(byte[] bankBytes)
	{
		using MemoryStream stream = new MemoryStream(bankBytes);
		return LoadInternal(stream, throwIfError: true);
	}
}
