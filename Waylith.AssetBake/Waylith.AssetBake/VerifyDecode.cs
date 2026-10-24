using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Waylith.Assets;
using Waylith.Rendering;

namespace Waylith.AssetBake;

internal static class VerifyDecode
{
	public static int Run(string packPath, int limit)
	{
		using AssetPack assetPack = AssetPack.TryOpen(packPath);
		if (assetPack == null)
		{
			Console.Error.WriteLine("No pack at '" + packPath + "'");
			return 1;
		}
		List<string> list = assetPack.Names.Where((string n) => n.EndsWith(".dds", StringComparison.Ordinal)).Order<string>(StringComparer.Ordinal).ToList();
		int num = 0;
		foreach (string item in list)
		{
			if (num >= limit)
			{
				break;
			}
			if (assetPack.TryRead(item, out byte[] data) && DdsFile.TryReadHeader(data, out var info))
			{
				LoadedTextureFormat format = info.Format;
				if ((format == LoadedTextureFormat.Dxt1 || format == LoadedTextureFormat.Bc7) ? true : false)
				{
					byte[] source = DecodeLevel0(data, in info);
					Console.WriteLine($"{Convert.ToHexString(SHA256.HashData(source)).ToLowerInvariant()} {info.Format} {item}");
					num++;
				}
			}
		}
		return 0;
	}

	public static byte[] DecodeLevel0(ReadOnlySpan<byte> dds, in DdsInfo info)
	{
		int num = TexturePixelData.BlockBytes(info.Format);
		int num2 = (info.Width + 3) / 4;
		int num3 = (info.Height + 3) / 4;
		byte[] array = new byte[info.Width * info.Height * 4];
		Span<byte> rgba = stackalloc byte[64];
		for (int i = 0; i < num3; i++)
		{
			for (int j = 0; j < num2; j++)
			{
				int start = 148 + (i * num2 + j) * num;
				ReadOnlySpan<byte> block = dds.Slice(start, num);
				if (num == 16)
				{
					Bc7BlockEncoder.DecodeBc7Block(block, rgba);
				}
				else
				{
					Bc7BlockEncoder.DecodeBc1Block(block, rgba);
				}
				for (int k = 0; k < 4; k++)
				{
					int num4 = i * 4 + k;
					if (num4 >= info.Height)
					{
						break;
					}
					for (int l = 0; l < 4; l++)
					{
						int num5 = j * 4 + l;
						if (num5 >= info.Width)
						{
							break;
						}
						rgba.Slice((k * 4 + l) * 4, 4).CopyTo(array.AsSpan((num4 * info.Width + num5) * 4, 4));
					}
				}
			}
		}
		return array;
	}
}
