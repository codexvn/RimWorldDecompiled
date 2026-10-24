using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using UnityEngine;

namespace UnityStubs.Internal;

internal static class FontFaceReader
{
	internal readonly record struct FaceNames(string Family, string Style)
	{
		public string FullName
		{
			get
			{
				if (Style.Length != 0 && !Style.Equals("Regular", StringComparison.OrdinalIgnoreCase))
				{
					return Family + " " + Style;
				}
				return Family;
			}
		}
	}

	internal readonly record struct OsFace(string Path, int FaceIndex, FaceNames Names);

	private const uint TtcTag = 1953784678u;

	private const uint NameTag = 1851878757u;

	private static readonly Lock IndexLock = new Lock();

	private static Dictionary<string, OsFace> _osFonts;

	internal static IReadOnlyDictionary<string, OsFace> OsFonts
	{
		get
		{
			using (IndexLock.EnterScope())
			{
				return _osFonts ?? (_osFonts = BuildOsIndex());
			}
		}
	}

	internal static bool TryFindOsFont(string name, out OsFace face)
	{
		face = default(OsFace);
		if (!string.IsNullOrEmpty(name))
		{
			return OsFonts.TryGetValue(name.Trim(), out face);
		}
		return false;
	}

	private static Dictionary<string, OsFace> BuildOsIndex()
	{
		Dictionary<string, OsFace> dictionary = new Dictionary<string, OsFace>(StringComparer.OrdinalIgnoreCase);
		string[] pathsToOSFonts = Font.GetPathsToOSFonts();
		foreach (string path in pathsToOSFonts)
		{
			byte[] data;
			try
			{
				data = File.ReadAllBytes(path);
			}
			catch (Exception ex) when (((ex is IOException || ex is UnauthorizedAccessException) ? 1 : 0) != 0)
			{
				continue;
			}
			if (!TryReadFaces(data, out var faces))
			{
				continue;
			}
			for (int j = 0; j < faces.Count; j++)
			{
				OsFace value = new OsFace(path, j, faces[j]);
				dictionary.TryAdd(faces[j].FullName, value);
				if (!dictionary.TryGetValue(faces[j].Family, out var value2) || (!IsRegular(value2.Names) && IsRegular(faces[j])))
				{
					dictionary[faces[j].Family] = value;
				}
			}
		}
		return dictionary;
	}

	private static bool IsRegular(FaceNames names)
	{
		return names.FullName == names.Family;
	}

	internal static bool TryReadFaces(byte[] data, out List<FaceNames> faces)
	{
		faces = new List<FaceNames>();
		try
		{
			Span<byte> span = data.AsSpan();
			if (span.Length < 12)
			{
				return false;
			}
			FaceNames names2;
			if (BinaryPrimitives.ReadUInt32BigEndian(span) == 1953784678)
			{
				uint num = BinaryPrimitives.ReadUInt32BigEndian(span.Slice(8));
				for (int i = 0; i < num && 12 + i * 4 + 4 <= span.Length; i++)
				{
					uint sfntOffset = BinaryPrimitives.ReadUInt32BigEndian(span.Slice(12 + i * 4));
					if (TryReadNames(span, (int)sfntOffset, out var names))
					{
						faces.Add(names);
					}
				}
			}
			else if (TryReadNames(span, 0, out names2))
			{
				faces.Add(names2);
			}
		}
		catch (Exception ex) when (((ex is ArgumentOutOfRangeException || ex is IndexOutOfRangeException || ex is DecoderFallbackException) ? 1 : 0) != 0)
		{
			return false;
		}
		return faces.Count > 0;
	}

	internal static byte[] ExtractFace(byte[] data, int faceIndex)
	{
		Span<byte> span = data.AsSpan();
		if (span.Length < 12 || BinaryPrimitives.ReadUInt32BigEndian(span) != 1953784678)
		{
			return data;
		}
		uint num = BinaryPrimitives.ReadUInt32BigEndian(span.Slice(8));
		if ((uint)faceIndex >= num)
		{
			return data;
		}
		int num2 = (int)BinaryPrimitives.ReadUInt32BigEndian(span.Slice(12 + faceIndex * 4));
		ushort num3 = BinaryPrimitives.ReadUInt16BigEndian(span.Slice(num2 + 4));
		int num4 = 12 + num3 * 16;
		int num5 = num4;
		for (int i = 0; i < num3; i++)
		{
			num5 += (int)((BinaryPrimitives.ReadUInt32BigEndian(span.Slice(num2 + 12 + i * 16 + 12)) + 3) & 0xFFFFFFFCu);
		}
		byte[] array = new byte[num5];
		span.Slice(num2, num4).CopyTo(array);
		int num6 = num4;
		for (int j = 0; j < num3; j++)
		{
			int num7 = num2 + 12 + j * 16;
			int start = (int)BinaryPrimitives.ReadUInt32BigEndian(span.Slice(num7 + 8));
			int num8 = (int)BinaryPrimitives.ReadUInt32BigEndian(span.Slice(num7 + 12));
			span.Slice(start, num8).CopyTo(array.AsSpan(num6));
			BinaryPrimitives.WriteUInt32BigEndian(array.AsSpan(12 + j * 16 + 8), (uint)num6);
			num6 += (num8 + 3) & -4;
		}
		return array;
	}

	private static bool TryReadNames(ReadOnlySpan<byte> span, int sfntOffset, out FaceNames names)
	{
		names = default(FaceNames);
		uint num = BinaryPrimitives.ReadUInt32BigEndian(span.Slice(sfntOffset));
		if ((num != 65536 && num != 1330926671 && num != 1953658213) || 1 == 0)
		{
			return false;
		}
		ushort num2 = BinaryPrimitives.ReadUInt16BigEndian(span.Slice(sfntOffset + 4));
		for (int i = 0; i < num2; i++)
		{
			int num3 = sfntOffset + 12 + i * 16;
			if (BinaryPrimitives.ReadUInt32BigEndian(span.Slice(num3)) == 1851878757)
			{
				int num4 = (int)BinaryPrimitives.ReadUInt32BigEndian(span.Slice(num3 + 8));
				int val = (int)BinaryPrimitives.ReadUInt32BigEndian(span.Slice(num3 + 12));
				return TryReadNameTable(span.Slice(num4, Math.Min(val, span.Length - num4)), out names);
			}
		}
		return false;
	}

	private static bool TryReadNameTable(ReadOnlySpan<byte> table, out FaceNames names)
	{
		names = default(FaceNames);
		ushort num = BinaryPrimitives.ReadUInt16BigEndian(table.Slice(2));
		ushort num2 = BinaryPrimitives.ReadUInt16BigEndian(table.Slice(4));
		string text = null;
		string text2 = null;
		string text3 = null;
		string text4 = null;
		int num3 = 0;
		int num4 = 0;
		int num5 = 0;
		int num6 = 0;
		for (int i = 0; i < num; i++)
		{
			int num7 = 6 + i * 12;
			if (num7 + 12 > table.Length)
			{
				break;
			}
			ushort num8 = BinaryPrimitives.ReadUInt16BigEndian(table.Slice(num7));
			ushort num9 = BinaryPrimitives.ReadUInt16BigEndian(table.Slice(num7 + 2));
			ushort num10 = BinaryPrimitives.ReadUInt16BigEndian(table.Slice(num7 + 4));
			ushort num11 = BinaryPrimitives.ReadUInt16BigEndian(table.Slice(num7 + 6));
			ushort num12 = BinaryPrimitives.ReadUInt16BigEndian(table.Slice(num7 + 8));
			ushort num13 = BinaryPrimitives.ReadUInt16BigEndian(table.Slice(num7 + 10));
			if (((uint)(num11 - 1) > 1u && (uint)(num11 - 16) > 1u) || 1 == 0)
			{
				continue;
			}
			bool flag = num8 == 3;
			if (flag)
			{
				bool flag2 = ((num9 == 1 || num9 == 10) ? true : false);
				flag = flag2;
			}
			int num14;
			Encoding encoding;
			if (flag)
			{
				num14 = ((num10 == 1033) ? 3 : 2);
				encoding = Encoding.BigEndianUnicode;
			}
			else if (num8 == 1 && num9 == 0)
			{
				num14 = 1;
				encoding = Encoding.Latin1;
			}
			else
			{
				if (num8 != 0)
				{
					continue;
				}
				num14 = 2;
				encoding = Encoding.BigEndianUnicode;
			}
			int num15 = num2 + num13;
			if (num15 + num12 > table.Length)
			{
				continue;
			}
			string text5 = encoding.GetString(table.Slice(num15, num12)).Trim();
			if (text5.Length == 0)
			{
				continue;
			}
			switch (num11)
			{
			case 1:
				if (num14 > num3)
				{
					text = text5;
					num3 = num14;
				}
				break;
			case 2:
				if (num14 > num4)
				{
					text2 = text5;
					num4 = num14;
				}
				break;
			case 16:
				if (num14 > num5)
				{
					text3 = text5;
					num5 = num14;
				}
				break;
			case 17:
				if (num14 > num6)
				{
					text4 = text5;
					num6 = num14;
				}
				break;
			}
		}
		string text6 = text3 ?? text;
		if (string.IsNullOrEmpty(text6))
		{
			return false;
		}
		names = new FaceNames(text6, text4 ?? text2 ?? "Regular");
		return true;
	}
}
