using System;
using System.IO;
using K4os.Compression.LZ4;
using K4os.Compression.LZ4.Streams;
using ZstdSharp;

namespace Waylith.Assets;

public static class AssetPackCodecs
{
	public const string Lz4Suffix = ".lz4";

	private const int Lz4BlockSize = 4194304;

	[ThreadStatic]
	private static Decompressor? _decompressor;

	[ThreadStatic]
	private static Compressor? _compressor;

	[ThreadStatic]
	private static int _compressorLevel;

	public static byte[] Compress(ReadOnlySpan<byte> data, AssetPackCodec codec, int level)
	{
		switch (codec)
		{
		case AssetPackCodec.Lz4:
		{
			using MemoryStream memoryStream = new MemoryStream();
			using (LZ4EncoderStream lZ4EncoderStream = LZ4Stream.Encode(memoryStream, new LZ4EncoderSettings
			{
				CompressionLevel = Lz4LevelFor(level),
				BlockSize = 4194304,
				ChainBlocks = false
			}, leaveOpen: true))
			{
				lZ4EncoderStream.Write(data);
			}
			return memoryStream.ToArray();
		}
		case AssetPackCodec.Zstd:
			if (level > 0)
			{
				using (Compressor compressor = new Compressor(level))
				{
					return compressor.Wrap(data).ToArray();
				}
			}
			break;
		}
		return data.ToArray();
	}

	public static void Decode(AssetPackCodec codec, ReadOnlySpan<byte> source, Span<byte> destination)
	{
		switch (codec)
		{
		case AssetPackCodec.Store:
			if (source.Length != destination.Length)
			{
				throw new InvalidDataException("Stored entry has the wrong length");
			}
			source.CopyTo(destination);
			break;
		case AssetPackCodec.Lz4:
			DecodeLz4Frame(source, destination);
			break;
		default:
			if ((_decompressor ?? (_decompressor = new Decompressor())).Unwrap(source, destination) != destination.Length)
			{
				throw new InvalidDataException("Entry decompressed to the wrong length");
			}
			break;
		}
	}

	public static int MaxCompressedLength(AssetPackCodec codec, int length)
	{
		return codec switch
		{
			AssetPackCodec.Zstd => Compressor.GetCompressBound(length), 
			AssetPackCodec.Lz4 => LZ4Codec.MaximumOutputSize(length) + 64 + 4 * (length / 4194304 + 1), 
			_ => length, 
		};
	}

	public static int Compress(ReadOnlySpan<byte> data, AssetPackCodec codec, int level, Span<byte> destination)
	{
		switch (codec)
		{
		case AssetPackCodec.Zstd:
			if (level > 0)
			{
				if (_compressor == null || _compressorLevel != level)
				{
					_compressor?.Dispose();
					_compressor = new Compressor(level);
					_compressorLevel = level;
				}
				return _compressor.Wrap(data, destination);
			}
			break;
		case AssetPackCodec.Lz4:
		{
			byte[] array = Compress(data, codec, level);
			array.CopyTo(destination);
			return array.Length;
		}
		}
		data.CopyTo(destination);
		return data.Length;
	}

	private static LZ4Level Lz4LevelFor(int level)
	{
		if (level >= 3)
		{
			return (LZ4Level)System.Math.Min(level, 12);
		}
		return LZ4Level.L00_FAST;
	}

	public static void DecodeLz4Frame(ReadOnlySpan<byte> source, Span<byte> destination)
	{
		if (source.Length < 7 || source[0] != 4 || source[1] != 34 || source[2] != 77 || source[3] != 24)
		{
			throw new InvalidDataException("Not an LZ4 frame");
		}
		byte num = source[4];
		int num2 = 6;
		if ((num & 8) != 0)
		{
			num2 += 8;
		}
		if ((num & 1) != 0)
		{
			num2 += 4;
		}
		num2++;
		bool flag = (num & 0x10) != 0;
		int num3 = 0;
		while (true)
		{
			uint num4 = BitConverter.ToUInt32(source.Slice(num2));
			num2 += 4;
			if (num4 == 0)
			{
				break;
			}
			int num5 = (int)(num4 & 0x7FFFFFFF);
			if ((num4 & 0x80000000u) != 0)
			{
				source.Slice(num2, num5).CopyTo(destination.Slice(num3));
				num3 += num5;
			}
			else
			{
				num3 += LZ4Codec.Decode(source.Slice(num2, num5), destination.Slice(num3));
			}
			num2 += num5 + (flag ? 4 : 0);
		}
		if (num3 != destination.Length)
		{
			throw new InvalidDataException("LZ4 frame decoded to the wrong length");
		}
	}
}
