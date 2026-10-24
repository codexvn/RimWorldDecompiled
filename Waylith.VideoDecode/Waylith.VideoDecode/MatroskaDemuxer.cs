using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;

namespace Waylith.VideoDecode;

internal sealed class MatroskaDemuxer
{
	private delegate void ElementHandler(uint id, ReadOnlySpan<byte> payload);

	private const uint IdEbml = 440786851u;

	private const uint IdSegment = 408125543u;

	private const uint IdInfo = 357149030u;

	private const uint IdTimecodeScale = 2807729u;

	private const uint IdDuration = 17545u;

	private const uint IdTracks = 374648427u;

	private const uint IdTrackEntry = 174u;

	private const uint IdTrackNumber = 215u;

	private const uint IdTrackType = 131u;

	private const uint IdCodecId = 134u;

	private const uint IdDefaultDuration = 2352003u;

	private const uint IdVideo = 224u;

	private const uint IdPixelWidth = 176u;

	private const uint IdPixelHeight = 186u;

	private const uint IdCluster = 524531317u;

	private const uint IdClusterTimecode = 231u;

	private const uint IdSimpleBlock = 163u;

	private const uint IdBlockGroup = 160u;

	private const uint IdBlock = 161u;

	private const uint IdReferenceBlock = 251u;

	private const long UnknownSize = -1L;

	public readonly List<MatroskaTrack> Tracks = new List<MatroskaTrack>();

	public double Duration;

	private long timecodeScale = 1000000L;

	private double durationTicks;

	private long clusterTimecode;

	public MatroskaTrack Video => Tracks.FirstOrDefault((MatroskaTrack t) => t.IsVideo);

	public static bool IsMatroska(ReadOnlySpan<byte> head)
	{
		if (head.Length >= 4)
		{
			return BinaryPrimitives.ReadUInt32BigEndian(head) == 440786851;
		}
		return false;
	}

	public static MatroskaDemuxer Open(Stream stream)
	{
		MatroskaDemuxer matroskaDemuxer = new MatroskaDemuxer();
		matroskaDemuxer.Scan(stream);
		matroskaDemuxer.Duration = matroskaDemuxer.durationTicks * (double)matroskaDemuxer.timecodeScale / 1000000000.0;
		return matroskaDemuxer;
	}

	private void Scan(Stream stream)
	{
		long length = stream.Length;
		stream.Position = 0L;
		if (!TryReadHeader(stream, out var id, out var size) || id != 440786851 || size == -1)
		{
			throw new InvalidDataException("matroska: no EBML header");
		}
		stream.Position += size;
		uint id2;
		long size2;
		while (stream.Position < length && TryReadHeader(stream, out id2, out size2))
		{
			long position = stream.Position;
			switch (id2)
			{
			case 408125543u:
			case 524531317u:
				continue;
			case 357149030u:
				ParseInfo(ReadPayload(stream, size2));
				break;
			case 374648427u:
				ParseTracks(ReadPayload(stream, size2));
				break;
			case 231u:
				clusterTimecode = (long)ReadUnsigned(ReadPayload(stream, size2));
				break;
			case 163u:
				AddBlock(stream, position, size2, null);
				break;
			case 160u:
				ParseBlockGroup(stream, position, size2);
				break;
			}
			if (size2 == -1)
			{
				throw new InvalidDataException("matroska: element of unknown size");
			}
			stream.Position = position + size2;
		}
	}

	private static bool TryReadHeader(Stream stream, out uint id, out long size)
	{
		id = 0u;
		size = 0L;
		int num = stream.ReadByte();
		if (num <= 0)
		{
			return false;
		}
		int num2 = BitOperations.LeadingZeroCount((uint)num) - 24 + 1;
		if (num2 > 4)
		{
			return false;
		}
		id = (uint)num;
		for (int i = 1; i < num2; i++)
		{
			int num3 = stream.ReadByte();
			if (num3 < 0)
			{
				return false;
			}
			id = (id << 8) | (uint)num3;
		}
		num = stream.ReadByte();
		if (num <= 0)
		{
			return false;
		}
		int num4 = BitOperations.LeadingZeroCount((uint)num) - 24 + 1;
		long num5 = num & (255 >> num4);
		bool flag = num5 == 255 >> num4;
		for (int j = 1; j < num4; j++)
		{
			int num6 = stream.ReadByte();
			if (num6 < 0)
			{
				return false;
			}
			num5 = (num5 << 8) | num6;
			flag = flag && num6 == 255;
		}
		size = (flag ? (-1) : num5);
		return true;
	}

	private static byte[] ReadPayload(Stream stream, long size)
	{
		if ((size > 16777216 || size == -1) ? true : false)
		{
			throw new InvalidDataException("matroska: bad element size");
		}
		byte[] array = new byte[size];
		stream.ReadExactly(array);
		return array;
	}

	private static ulong ReadUnsigned(ReadOnlySpan<byte> payload)
	{
		ulong num = 0uL;
		ReadOnlySpan<byte> readOnlySpan = payload;
		for (int i = 0; i < readOnlySpan.Length; i++)
		{
			byte b = readOnlySpan[i];
			num = (num << 8) | b;
		}
		return num;
	}

	private static double ReadFloat(ReadOnlySpan<byte> payload)
	{
		return payload.Length switch
		{
			4 => BinaryPrimitives.ReadSingleBigEndian(payload), 
			8 => BinaryPrimitives.ReadDoubleBigEndian(payload), 
			_ => 0.0, 
		};
	}

	private static void ForEachChild(ReadOnlySpan<byte> parent, ElementHandler handler)
	{
		using MemoryStream memoryStream = new MemoryStream(parent.ToArray(), writable: false);
		uint id;
		long size;
		while (memoryStream.Position < memoryStream.Length && TryReadHeader(memoryStream, out id, out size))
		{
			if (size == -1 || size > memoryStream.Length - memoryStream.Position)
			{
				throw new InvalidDataException("matroska: bad element size");
			}
			handler(id, parent.Slice((int)memoryStream.Position, (int)size));
			memoryStream.Position += size;
		}
	}

	private void ParseInfo(byte[] info)
	{
		ForEachChild(info, delegate(uint id, ReadOnlySpan<byte> payload)
		{
			switch (id)
			{
			case 2807729u:
				timecodeScale = (long)ReadUnsigned(payload);
				break;
			case 17545u:
				durationTicks = ReadFloat(payload);
				break;
			}
		});
	}

	private void ParseTracks(byte[] tracks)
	{
		ForEachChild(tracks, delegate(uint id, ReadOnlySpan<byte> entry)
		{
			if (id == 174)
			{
				MatroskaTrack track = new MatroskaTrack();
				ForEachChild(entry, delegate(uint field, ReadOnlySpan<byte> payload)
				{
					switch (field)
					{
					case 215u:
						track.Number = (int)ReadUnsigned(payload);
						break;
					case 131u:
						track.IsVideo = ReadUnsigned(payload) == 1;
						break;
					case 134u:
						track.Codec = Encoding.ASCII.GetString(payload).TrimEnd('\0');
						break;
					case 2352003u:
						track.FrameDuration = (double)ReadUnsigned(payload) / 1000000000.0;
						break;
					case 224u:
						ForEachChild(payload, delegate(uint videoField, ReadOnlySpan<byte> value)
						{
							switch (videoField)
							{
							case 176u:
								track.Width = (int)ReadUnsigned(value);
								break;
							case 186u:
								track.Height = (int)ReadUnsigned(value);
								break;
							}
						});
						break;
					}
				});
				Tracks.Add(track);
			}
		});
	}

	private void ParseBlockGroup(Stream stream, long start, long size)
	{
		if (size == -1)
		{
			throw new InvalidDataException("matroska: block group of unknown size");
		}
		long num = -1L;
		long size2 = 0L;
		bool flag = false;
		long num2 = start + size;
		uint id;
		long size3;
		while (stream.Position < num2 && TryReadHeader(stream, out id, out size3))
		{
			if (size3 == -1)
			{
				throw new InvalidDataException("matroska: element of unknown size");
			}
			switch (id)
			{
			case 161u:
				num = stream.Position;
				size2 = size3;
				break;
			case 251u:
				flag = true;
				break;
			}
			stream.Position += size3;
		}
		if (num >= 0)
		{
			stream.Position = num;
			AddBlock(stream, num, size2, !flag);
		}
	}

	private void AddBlock(Stream stream, long start, long size, bool? key)
	{
		bool flag;
		if (size < 4)
		{
			if (size == -1)
			{
			}
		}
		else if (size <= int.MaxValue)
		{
			flag = false;
			goto IL_0023;
		}
		flag = true;
		goto IL_0023;
		IL_0023:
		if (flag)
		{
			throw new InvalidDataException("matroska: bad block size");
		}
		Span<byte> buffer = stackalloc byte[4];
		stream.ReadExactly(buffer);
		if ((buffer[0] & 0x80) == 0)
		{
			return;
		}
		int number = buffer[0] & 0x7F;
		MatroskaTrack matroskaTrack = Tracks.FirstOrDefault((MatroskaTrack t) => t.Number == number);
		if (matroskaTrack != null && matroskaTrack.IsVideo)
		{
			if ((buffer[3] & 6) != 0)
			{
				throw new NotSupportedException("matroska: laced video blocks are not supported");
			}
			short num = BinaryPrimitives.ReadInt16BigEndian(buffer.Slice(1));
			matroskaTrack.Frames.Add(new MatroskaFrame
			{
				Offset = start + 4,
				Size = (int)size - 4,
				Time = (double)(clusterTimecode + num) * ((double)timecodeScale / 1000000000.0),
				Key = (key ?? ((buffer[3] & 0x80) != 0))
			});
		}
	}
}
