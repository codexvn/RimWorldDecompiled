using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Waylith.VideoDecode;

internal sealed class Mp4Demuxer
{
	private delegate void BoxVisitor(uint type, ReadOnlySpan<byte> payload);

	public readonly List<Mp4Track> Tracks = new List<Mp4Track>();

	public Mp4Track Video => Tracks.FirstOrDefault((Mp4Track t) => t.IsVideo);

	public Mp4Track Audio => Tracks.FirstOrDefault((Mp4Track t) => t.IsAudio);

	public static Mp4Demuxer Open(Stream stream)
	{
		Mp4Demuxer mp4Demuxer = new Mp4Demuxer();
		Span<byte> span = stackalloc byte[16];
		long num2;
		for (long num = 0L; num + 8 <= stream.Length; num += num2)
		{
			stream.Position = num;
			stream.ReadExactly(span.Slice(0, 8));
			num2 = BinaryPrimitives.ReadUInt32BigEndian(span);
			uint num3 = BinaryPrimitives.ReadUInt32BigEndian(span.Slice(4));
			int num4 = 8;
			switch (num2)
			{
			case 1L:
				stream.ReadExactly(span.Slice(8, 8));
				num2 = (long)BinaryPrimitives.ReadUInt64BigEndian(span.Slice(8));
				num4 = 16;
				break;
			case 0L:
				num2 = stream.Length - num;
				break;
			}
			if (num2 < num4)
			{
				throw new InvalidDataException("mp4: bad box size");
			}
			if (num3 == FourCc("moov"))
			{
				byte[] array = new byte[num2 - num4];
				stream.ReadExactly(array);
				mp4Demuxer.ParseMoov(array);
				return mp4Demuxer;
			}
		}
		throw new InvalidDataException("mp4: no moov box");
	}

	private static uint FourCc(string s)
	{
		return ((uint)s[0] << 24) | ((uint)s[1] << 16) | ((uint)s[2] << 8) | s[3];
	}

	private static string FourCcString(uint v)
	{
		Encoding aSCII = Encoding.ASCII;
		InlineArray4<byte> buffer = default(InlineArray4<byte>);
		global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray4<byte>, byte>(ref buffer, 0) = (byte)(v >> 24);
		global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray4<byte>, byte>(ref buffer, 1) = (byte)(v >> 16);
		global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray4<byte>, byte>(ref buffer, 2) = (byte)(v >> 8);
		global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray4<byte>, byte>(ref buffer, 3) = (byte)v;
		return aSCII.GetString(global::_003CPrivateImplementationDetails_003E.InlineArrayAsReadOnlySpan<InlineArray4<byte>, byte>(in buffer, 4));
	}

	private static uint U32(ReadOnlySpan<byte> s, int at)
	{
		return BinaryPrimitives.ReadUInt32BigEndian(s.Slice(at));
	}

	private static int I32(ReadOnlySpan<byte> s, int at)
	{
		return BinaryPrimitives.ReadInt32BigEndian(s.Slice(at));
	}

	private static long I64(ReadOnlySpan<byte> s, int at)
	{
		return BinaryPrimitives.ReadInt64BigEndian(s.Slice(at));
	}

	private static int U16(ReadOnlySpan<byte> s, int at)
	{
		return BinaryPrimitives.ReadUInt16BigEndian(s.Slice(at));
	}

	private static void ForEachBox(ReadOnlySpan<byte> data, BoxVisitor visit)
	{
		long num;
		for (int i = 0; i + 8 <= data.Length; i += (int)num)
		{
			num = U32(data, i);
			uint type = U32(data, i + 4);
			int num2 = 8;
			switch (num)
			{
			case 1L:
				num = I64(data, i + 8);
				num2 = 16;
				break;
			case 0L:
				num = data.Length - i;
				break;
			}
			if (num < num2 || i + num > data.Length)
			{
				break;
			}
			visit(type, data.Slice(i + num2, (int)num - num2));
		}
	}

	private static bool TryFindBox(ReadOnlySpan<byte> data, string name, out ReadOnlySpan<byte> payload)
	{
		uint num = FourCc(name);
		int num2;
		for (int i = 0; i + 8 <= data.Length; i += num2)
		{
			num2 = (int)U32(data, i);
			if (num2 < 8 || i + num2 > data.Length)
			{
				break;
			}
			if (U32(data, i + 4) == num)
			{
				payload = data.Slice(i + 8, num2 - 8);
				return true;
			}
		}
		payload = default(ReadOnlySpan<byte>);
		return false;
	}

	private void ParseMoov(byte[] moov)
	{
		ReadOnlySpan<byte> payload2;
		uint movieTimescale = (TryFindBox(moov, "mvhd", out payload2) ? U32(payload2, (payload2[0] == 1) ? 20 : 12) : 0u);
		ForEachBox(moov, delegate(uint type, ReadOnlySpan<byte> payload)
		{
			if (type == FourCc("trak"))
			{
				Mp4Track mp4Track = ParseTrak(payload, movieTimescale);
				if (mp4Track != null)
				{
					Tracks.Add(mp4Track);
				}
			}
		});
	}

	private static Mp4Track ParseTrak(ReadOnlySpan<byte> trak, uint movieTimescale)
	{
		if (!TryFindBox(trak, "mdia", out var payload) || !TryFindBox(payload, "mdhd", out var payload2) || !TryFindBox(payload, "hdlr", out var payload3))
		{
			return null;
		}
		if (!TryFindBox(payload, "minf", out var payload4) || !TryFindBox(payload4, "stbl", out var payload5))
		{
			return null;
		}
		Mp4Track mp4Track = new Mp4Track();
		if (payload2[0] == 1)
		{
			mp4Track.Timescale = U32(payload2, 20);
			mp4Track.Duration = I64(payload2, 24);
		}
		else
		{
			mp4Track.Timescale = U32(payload2, 12);
			mp4Track.Duration = U32(payload2, 16);
		}
		uint num = U32(payload3, 8);
		mp4Track.IsVideo = num == FourCc("vide");
		mp4Track.IsAudio = num == FourCc("soun");
		if (!mp4Track.IsVideo && !mp4Track.IsAudio)
		{
			return null;
		}
		if (!TryFindBox(payload5, "stsd", out var payload6) || U32(payload6, 4) < 1)
		{
			return null;
		}
		ParseSampleEntry(mp4Track, payload6.Slice(8));
		long mediaStart = 0L;
		if (TryFindBox(trak, "edts", out var payload7) && TryFindBox(payload7, "elst", out var payload8))
		{
			int num2 = (int)U32(payload8, 4);
			int num3 = ((payload8[0] == 1) ? 20 : 12);
			for (int i = 0; i < num2 && 8 + (i + 1) * num3 <= payload8.Length; i++)
			{
				ReadOnlySpan<byte> s = payload8.Slice(8 + i * num3, num3);
				long num4 = ((payload8[0] == 1) ? I64(s, 8) : I32(s, 4));
				if (num4 >= 0)
				{
					mediaStart = num4;
					if (movieTimescale != 0)
					{
						mp4Track.EditDuration = (double)((payload8[0] == 1) ? I64(s, 0) : U32(s, 0)) / (double)movieTimescale;
					}
					break;
				}
			}
		}
		BuildSampleTable(mp4Track, payload5, mediaStart);
		return mp4Track;
	}

	private static void ParseSampleEntry(Mp4Track track, ReadOnlySpan<byte> entry)
	{
		int val = (int)U32(entry, 0);
		track.Codec = FourCcString(U32(entry, 4));
		entry = entry.Slice(0, System.Math.Min(val, entry.Length));
		if (track.IsVideo)
		{
			track.Width = U16(entry, 32);
			track.Height = U16(entry, 34);
			if (TryFindBox(entry.Slice(86), "avcC", out var payload))
			{
				track.CodecConfig = payload.ToArray();
			}
			return;
		}
		int num = U16(entry, 16);
		track.Channels = U16(entry, 24);
		track.SampleRate = (int)(U32(entry, 32) >> 16);
		int num2 = num switch
		{
			2 => 72, 
			1 => 52, 
			_ => 36, 
		};
		if (num2 < entry.Length && TryFindBox(entry.Slice(num2), "esds", out var payload2))
		{
			track.CodecConfig = payload2.ToArray();
		}
	}

	private static void BuildSampleTable(Mp4Track track, ReadOnlySpan<byte> stbl, long mediaStart)
	{
		if (!TryFindBox(stbl, "stsz", out var payload) || !TryFindBox(stbl, "stsc", out var payload2) || !TryFindBox(stbl, "stts", out var payload3))
		{
			throw new InvalidDataException("mp4: incomplete sample table");
		}
		int num = (int)U32(payload, 4);
		int num2 = (int)U32(payload, 8);
		Mp4Sample[] array = new Mp4Sample[num2];
		for (int i = 0; i < num2; i++)
		{
			array[i].Size = ((num != 0) ? num : ((int)U32(payload, 12 + i * 4)));
		}
		bool flag = false;
		if (!TryFindBox(stbl, "stco", out var payload4))
		{
			if (!TryFindBox(stbl, "co64", out payload4))
			{
				throw new InvalidDataException("mp4: no chunk offsets");
			}
			flag = true;
		}
		int num3 = (int)U32(payload4, 4);
		int num4 = (int)U32(payload2, 4);
		int num5 = 0;
		for (int j = 0; j < num4; j++)
		{
			if (num5 >= num2)
			{
				break;
			}
			uint num6 = U32(payload2, 8 + j * 12) - 1;
			int num7 = (int)U32(payload2, 12 + j * 12);
			int num8 = ((j + 1 < num4) ? ((int)(U32(payload2, 8 + (j + 1) * 12) - 1)) : num3);
			for (int k = (int)num6; k < num8; k++)
			{
				if (num5 >= num2)
				{
					break;
				}
				long num9 = (flag ? I64(payload4, 8 + k * 8) : U32(payload4, 8 + k * 4));
				int num10 = 0;
				while (num10 < num7 && num5 < num2)
				{
					array[num5].Offset = num9;
					num9 += array[num5].Size;
					num10++;
					num5++;
				}
			}
		}
		int num11 = (int)U32(payload3, 4);
		long num12 = 0L;
		num5 = 0;
		for (int l = 0; l < num11; l++)
		{
			int num13 = (int)U32(payload3, 8 + l * 8);
			uint num14 = U32(payload3, 12 + l * 8);
			int num15 = 0;
			while (num15 < num13 && num5 < num2)
			{
				array[num5].Dts = num12;
				array[num5].Pts = num12 - mediaStart;
				num12 += num14;
				num15++;
				num5++;
			}
		}
		if (TryFindBox(stbl, "ctts", out var payload5))
		{
			int num16 = (int)U32(payload5, 4);
			num5 = 0;
			for (int m = 0; m < num16; m++)
			{
				int num17 = (int)U32(payload5, 8 + m * 8);
				int num18 = I32(payload5, 12 + m * 8);
				int num19 = 0;
				while (num19 < num17 && num5 < num2)
				{
					array[num5].Pts += num18;
					num19++;
					num5++;
				}
			}
		}
		if (TryFindBox(stbl, "stss", out var payload6))
		{
			int num20 = (int)U32(payload6, 4);
			for (int n = 0; n < num20; n++)
			{
				int num21 = (int)(U32(payload6, 8 + n * 4) - 1);
				if ((uint)num21 < (uint)num2)
				{
					array[num21].Sync = true;
				}
			}
		}
		else
		{
			for (int num22 = 0; num22 < num2; num22++)
			{
				array[num22].Sync = true;
			}
		}
		track.Samples = array;
	}
}
