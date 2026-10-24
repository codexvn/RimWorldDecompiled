using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.IO;

namespace Waylith.VideoDecode;

internal sealed class OggDemuxer
{
	private const int PageHeaderSize = 27;

	public readonly List<byte[]> Headers = new List<byte[]>();

	public readonly List<OggPacket> Frames = new List<OggPacket>();

	public readonly List<(long Offset, int Length)> Pieces = new List<(long, int)>();

	public static OggDemuxer Open(Stream stream)
	{
		OggDemuxer oggDemuxer = new OggDemuxer();
		oggDemuxer.Scan(stream);
		if (oggDemuxer.Headers.Count < 3)
		{
			throw new InvalidDataException("ogg: no theora stream");
		}
		return oggDemuxer;
	}

	private void Scan(Stream stream)
	{
		long length = stream.Length;
		byte[] array = new byte[282];
		byte[] array2 = new byte[65025];
		MemoryStream memoryStream = new MemoryStream();
		uint num = 0u;
		bool flag = false;
		bool flag2 = false;
		OggPacket item = default(OggPacket);
		long num2 = 0L;
		while (num2 + 27 <= length)
		{
			stream.Position = num2;
			stream.ReadExactly(array, 0, 27);
			if (!((ReadOnlySpan<byte>)array.AsSpan(0, 4)).SequenceEqual("OggS"u8))
			{
				num2++;
				flag2 = false;
				continue;
			}
			int num3 = array[26];
			if (num2 + 27 + num3 > length)
			{
				break;
			}
			stream.ReadExactly(array, 27, num3);
			int num4 = 0;
			for (int i = 0; i < num3; i++)
			{
				num4 += array[27 + i];
			}
			long num5 = num2 + 27 + num3;
			num2 = num5 + num4;
			if (num2 > length)
			{
				break;
			}
			byte b = array[5];
			uint num6 = BinaryPrimitives.ReadUInt32LittleEndian(array.AsSpan(14));
			if ((!flag || num6 != num) && (flag || (b & 2) == 0))
			{
				continue;
			}
			stream.ReadExactly(array2, 0, num4);
			if (!flag)
			{
				if (num4 < 7 || array2[0] != 128 || !((ReadOnlySpan<byte>)array2.AsSpan(1, 6)).SequenceEqual("theora"u8))
				{
					continue;
				}
				flag = true;
				num = num6;
			}
			if ((b & 1) == 0)
			{
				flag2 = false;
			}
			int num7 = 0;
			bool flag3 = (b & 1) != 0 && !flag2;
			for (int num8 = 0; num8 < num3; num8++)
			{
				int num9 = array[27 + num8];
				if (!flag3)
				{
					if (!flag2)
					{
						OggPacket oggPacket = default(OggPacket);
						oggPacket.FirstPiece = Pieces.Count;
						oggPacket.Key = num9 > 0 && (array2[num7] & 0x40) == 0;
						item = oggPacket;
						flag2 = true;
						memoryStream.SetLength(0L);
					}
					if (num9 > 0)
					{
						if (item.PieceCount > 0)
						{
							List<(long Offset, int Length)> pieces = Pieces;
							long item2 = pieces[pieces.Count - 1].Offset;
							List<(long Offset, int Length)> pieces2 = Pieces;
							if (item2 + pieces2[pieces2.Count - 1].Length == num5 + num7)
							{
								List<(long Offset, int Length)> pieces3 = Pieces;
								int index = pieces3.Count - 1;
								List<(long Offset, int Length)> pieces4 = Pieces;
								long item3 = pieces4[pieces4.Count - 1].Offset;
								List<(long Offset, int Length)> pieces5 = Pieces;
								pieces3[index] = (item3, pieces5[pieces5.Count - 1].Length + num9);
								goto IL_0298;
							}
						}
						Pieces.Add((num5 + num7, num9));
						item.PieceCount++;
						goto IL_0298;
					}
				}
				goto IL_02be;
				IL_0298:
				item.Size += num9;
				if (Headers.Count < 3)
				{
					memoryStream.Write(array2, num7, num9);
				}
				goto IL_02be;
				IL_02be:
				num7 += num9;
				if (num9 != 255)
				{
					if (flag3)
					{
						flag3 = false;
					}
					else
					{
						flag2 = false;
						if (Headers.Count < 3)
						{
							Headers.Add(memoryStream.ToArray());
						}
						else
						{
							Frames.Add(item);
						}
					}
				}
			}
		}
	}

	public ReadOnlySpan<byte> Read(Stream stream, int frame, ref byte[] buffer)
	{
		OggPacket oggPacket = Frames[frame];
		if (buffer.Length < oggPacket.Size)
		{
			buffer = new byte[oggPacket.Size];
		}
		int num = 0;
		for (int i = 0; i < oggPacket.PieceCount; i++)
		{
			(long Offset, int Length) tuple = Pieces[oggPacket.FirstPiece + i];
			long item = tuple.Offset;
			int item2 = tuple.Length;
			stream.Position = item;
			stream.ReadExactly(buffer, num, item2);
			num += item2;
		}
		return buffer.AsSpan(0, oggPacket.Size);
	}
}
