using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Waylith.Rendering;

namespace Waylith.Assets;

public static class AssetPackTextureInfo
{
	public readonly record struct Info(int Width, int Height, int MipCount, LoadedTextureFormat Format, bool Opaque, TextureSwizzle Swizzle = default(TextureSwizzle));

	public const string EntryName = "textures.info";

	public static Dictionary<string, Info> Read(AssetPack? pack)
	{
		Dictionary<string, Info> dictionary = new Dictionary<string, Info>(StringComparer.Ordinal);
		if (pack == null || !pack.TryRead("textures.info", out byte[] data))
		{
			return dictionary;
		}
		string[] array = Encoding.UTF8.GetString(data).Split('\n', StringSplitOptions.RemoveEmptyEntries);
		foreach (string text in array)
		{
			int[] array2 = new int[6];
			int num = text.Length;
			bool flag = false;
			int num2 = 5;
			while (num2 >= 0 && !flag)
			{
				int num3 = text.LastIndexOf(' ', num - 1);
				if (num3 <= 0 || !int.TryParse(text.AsSpan(num3 + 1, num - num3 - 1), NumberStyles.Integer, CultureInfo.InvariantCulture, out array2[num2]))
				{
					flag = true;
				}
				else
				{
					num = num3;
				}
				num2--;
			}
			if (!flag)
			{
				dictionary[text.Substring(0, num)] = new Info(array2[0], array2[1], array2[2], (LoadedTextureFormat)array2[3], array2[4] != 0, TextureSwizzle.Unpack(array2[5]));
			}
		}
		return dictionary;
	}

	public static byte[] Write(IEnumerable<KeyValuePair<string, Info>> infos)
	{
		StringBuilder stringBuilder = new StringBuilder();
		foreach (var (value, info2) in infos.OrderBy<KeyValuePair<string, Info>, string>((KeyValuePair<string, Info> m) => m.Key, StringComparer.Ordinal))
		{
			stringBuilder.Append(value).Append(' ').Append(info2.Width)
				.Append(' ')
				.Append(info2.Height)
				.Append(' ')
				.Append(info2.MipCount)
				.Append(' ')
				.Append((int)info2.Format)
				.Append(' ')
				.Append(info2.Opaque ? 1 : 0)
				.Append(' ')
				.Append(info2.Swizzle.Pack())
				.Append('\n');
		}
		return Encoding.UTF8.GetBytes(stringBuilder.ToString());
	}
}
