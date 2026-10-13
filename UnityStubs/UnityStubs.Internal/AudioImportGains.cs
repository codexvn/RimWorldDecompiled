using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;
using Waylith.UnityCompat;

namespace UnityStubs.Internal;

internal static class AudioImportGains
{
	private readonly record struct WavData(int Format, int Channels, int Bits, int SampleRate, long DataSize);

	private static readonly object Sync = new object();

	private static Dictionary<string, (long Size, long Ticks, float Gain)> _cache;

	private static string _cachePath;

	public static float GetMonoNormalizeGain(string path)
	{
		try
		{
			FileInfo fileInfo = new FileInfo(path);
			if (!fileInfo.Exists)
			{
				return 1f;
			}
			lock (Sync)
			{
				EnsureLoaded();
				if (_cache.TryGetValue(fileInfo.FullName, out (long, long, float) value) && value.Item1 == fileInfo.Length && value.Item2 == fileInfo.LastWriteTimeUtc.Ticks)
				{
					return value.Item3;
				}
				float peak;
				float num = ((TryMeasureMonoPeak(fileInfo.FullName, out peak) && peak > 0.0001f) ? (1f / peak) : 1f);
				_cache[fileInfo.FullName] = (fileInfo.Length, fileInfo.LastWriteTimeUtc.Ticks, num);
				if (_cachePath != null)
				{
					string cachePath = _cachePath;
					IFormatProvider invariantCulture = CultureInfo.InvariantCulture;
					DefaultInterpolatedStringHandler handler = new DefaultInterpolatedStringHandler(4, 4, invariantCulture);
					handler.AppendFormatted(fileInfo.FullName);
					handler.AppendLiteral("|");
					handler.AppendFormatted(fileInfo.Length);
					handler.AppendLiteral("|");
					handler.AppendFormatted(fileInfo.LastWriteTimeUtc.Ticks);
					handler.AppendLiteral("|");
					handler.AppendFormatted(num, "R");
					handler.AppendLiteral("\n");
					File.AppendAllText(cachePath, string.Create(invariantCulture, ref handler));
				}
				return num;
			}
		}
		catch (Exception ex)
		{
			Debug.LogWarning("Audio normalize gain unavailable for " + path + ": " + ex.Message);
			return 1f;
		}
	}

	private static void EnsureLoaded()
	{
		if (_cache != null)
		{
			return;
		}
		_cache = new Dictionary<string, (long, long, float)>(StringComparer.Ordinal);
		string text = UnityRuntime.Current?.App.CacheDataPath ?? Application.persistentDataPath;
		if (string.IsNullOrWhiteSpace(text))
		{
			return;
		}
		string text2 = Path.Combine(text, "AudioImportCache");
		Directory.CreateDirectory(text2);
		_cachePath = Path.Combine(text2, "MonoNormalizeGains.txt");
		if (!File.Exists(_cachePath))
		{
			return;
		}
		foreach (string item in File.ReadLines(_cachePath))
		{
			string[] array = item.Split('|');
			if (array.Length == 4 && long.TryParse(array[1], out var result) && long.TryParse(array[2], out var result2) && float.TryParse(array[3], NumberStyles.Float, CultureInfo.InvariantCulture, out var result3))
			{
				_cache[array[0]] = (result, result2, result3);
			}
		}
	}

	private static bool TryMeasureMonoPeak(string path, out float peak)
	{
		peak = 0f;
		using FileStream stream = OpenSequential(path);
		if (!TryFindData(stream, out var wav) || wav.Channels < 2)
		{
			return false;
		}
		peak = MeasureData(stream, wav.DataSize, wav.Format, wav.Channels, wav.Bits);
		return true;
	}

	public static bool TryReadAsImportedMono(string path, out byte[] pcm16, out int frequency)
	{
		pcm16 = null;
		frequency = 0;
		try
		{
			using FileStream fileStream = OpenSequential(path);
			if (!TryFindData(fileStream, out var wav) || wav.Channels < 2)
			{
				return false;
			}
			int num = wav.Bits / 8;
			int num2 = num * wav.Channels;
			byte[] array = new byte[wav.DataSize - wav.DataSize % num2];
			int num3 = fileStream.ReadAtLeast(array, array.Length, throwOnEndOfStream: false) / num2;
			float[] array2 = new float[num3];
			float num4 = 0f;
			for (int i = 0; i < num3; i++)
			{
				float num5 = 0f;
				for (int j = 0; j < wav.Channels; j++)
				{
					num5 += ReadSample(array.AsSpan(i * num2 + j * num, num), wav.Format, wav.Bits);
				}
				array2[i] = num5 / (float)wav.Channels;
				num4 = MathF.Max(num4, MathF.Abs(array2[i]));
			}
			float num6 = ((num4 > 0.0001f) ? (1f / num4) : 1f);
			pcm16 = new byte[num3 * 2];
			for (int k = 0; k < num3; k++)
			{
				BinaryPrimitives.WriteInt16LittleEndian(pcm16.AsSpan(k * 2), (short)Math.Clamp(MathF.Round(array2[k] * num6 * 32767f), -32768f, 32767f));
			}
			frequency = wav.SampleRate;
			return num3 > 0;
		}
		catch (Exception ex)
		{
			Debug.LogWarning("Could not read " + path + " as an imported mono clip: " + ex.Message);
			return false;
		}
	}

	private static FileStream OpenSequential(string path)
	{
		return new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, 65536, FileOptions.SequentialScan);
	}

	private static bool TryFindData(Stream stream, out WavData wav)
	{
		wav = default(WavData);
		Span<byte> buffer = stackalloc byte[12];
		if (stream.Read(buffer) != 12 || !((ReadOnlySpan<byte>)buffer.Slice(0, 4)).SequenceEqual("RIFF"u8) || !((ReadOnlySpan<byte>)buffer.Slice(8, 4)).SequenceEqual("WAVE"u8))
		{
			return false;
		}
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int sampleRate = 0;
		Span<byte> buffer2 = stackalloc byte[8];
		Span<byte> span = stackalloc byte[40];
		while (stream.Read(buffer2) == 8)
		{
			uint num4 = BinaryPrimitives.ReadUInt32LittleEndian(buffer2.Slice(4));
			if (((ReadOnlySpan<byte>)buffer2.Slice(0, 4)).SequenceEqual("fmt "u8))
			{
				int num5 = (int)Math.Min(num4, (uint)span.Length);
				if (stream.Read(span.Slice(0, num5)) != num5 || num5 < 16)
				{
					return false;
				}
				num = BinaryPrimitives.ReadUInt16LittleEndian(span);
				num2 = BinaryPrimitives.ReadUInt16LittleEndian(span.Slice(2));
				sampleRate = BinaryPrimitives.ReadInt32LittleEndian(span.Slice(4));
				num3 = BinaryPrimitives.ReadUInt16LittleEndian(span.Slice(14));
				if (num == 65534 && num5 >= 26)
				{
					num = BinaryPrimitives.ReadUInt16LittleEndian(span.Slice(24));
				}
				stream.Seek(num4 - num5 + (num4 & 1), SeekOrigin.Current);
				continue;
			}
			if (((ReadOnlySpan<byte>)buffer2.Slice(0, 4)).SequenceEqual("data"u8))
			{
				bool flag = num2 < 1;
				if (!flag)
				{
					bool flag2 = num == 1;
					if (flag2)
					{
						bool flag3;
						switch (num3)
						{
						case 8:
						case 16:
						case 24:
						case 32:
							flag3 = true;
							break;
						default:
							flag3 = false;
							break;
						}
						flag2 = flag3;
					}
					flag = !flag2 && (num != 3 || num3 != 32);
				}
				if (flag)
				{
					return false;
				}
				wav = new WavData(num, num2, num3, sampleRate, Math.Min(num4, stream.Length - stream.Position));
				return true;
			}
			stream.Seek(num4 + (num4 & 1), SeekOrigin.Current);
		}
		return false;
	}

	private static float ReadSample(ReadOnlySpan<byte> s, int format, int bits)
	{
		if (format == 3)
		{
			return BinaryPrimitives.ReadSingleLittleEndian(s);
		}
		return bits switch
		{
			8 => (float)(s[0] - 128) / 128f, 
			16 => (float)BinaryPrimitives.ReadInt16LittleEndian(s) / 32768f, 
			24 => (float)(s[0] | (s[1] << 8) | ((sbyte)s[2] << 16)) / 8388608f, 
			_ => (float)BinaryPrimitives.ReadInt32LittleEndian(s) / 2.1474836E+09f, 
		};
	}

	private static float MeasureData(Stream stream, long dataSize, int format, int channels, int bits)
	{
		int num = bits / 8;
		int num2 = num * channels;
		byte[] array = new byte[num2 * 16384];
		float num3 = 0f;
		long num4 = dataSize;
		while (num4 >= num2)
		{
			int num5 = (int)Math.Min(array.Length, num4 - num4 % num2);
			int num6 = stream.ReadAtLeast(array.AsSpan(0, num5), num5, throwOnEndOfStream: false);
			num6 -= num6 % num2;
			if (num6 <= 0)
			{
				break;
			}
			num4 -= num6;
			for (int i = 0; i < num6; i += num2)
			{
				float num7 = 0f;
				for (int j = 0; j < channels; j++)
				{
					num7 += ReadSample(array.AsSpan(i + j * num, num), format, bits);
				}
				float num8 = MathF.Abs(num7 / (float)channels);
				if (num8 > num3)
				{
					num3 = num8;
				}
			}
		}
		return num3;
	}
}
