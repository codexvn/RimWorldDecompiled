using System;
using System.Collections.Generic;
using System.IO;
using Waylith.Assets;

namespace Waylith.AssetBake;

public sealed class AssetBakeOptions
{
	public const string Usage = "AssetBake --out <pack> --root <dir> [--root <dir>...] (--shaders <dir> [--shaders <dir>...] | --no-shaders) [options]\n  --shaders <dir>     a shader source directory, searched the way the game searches it (repeatable)\n  --no-shaders        bake content alone: no shader pass, and the pack carries no shader entries\n  --base <dir>        the game's data directory; entry names are relative to it (default: current directory)\n  --tile-max <n>      textures at or under n on both axes stay uncompressed (default: the game's)\n  --skip <substring>  leave matching paths uncompressed, on top of the game's (repeatable)\n  --premul <substring>  bake matching paths premultiplied instead of straight (repeatable)\n  --level <n>         zstd level (default 19, what ships); 3 bakes ~5x faster for ~18% more size;\n                      0 stores entries uncompressed, a diagnostic\n  --codec <name>      zstd (default) or lz4: LZ4 decodes ~3x faster and packs ~30% larger,\n                      and its entries are stored .lz4 frames rather than zip-compressed. Its levels\n                      are LZ4's own: below 3 fast, 3-9 HC, 10-11 OPT, 12 and up MAX\n  --rdo <bool>        trade a little quality for smaller zstd entries, under the host's per-format rules\n                      (default false; ~10% off the pack, ~12 s more of a clean bake)\n  --align <bytes>     keep entries from straddling multiples of this, so a store that patches in fixed\n                      chunks (Steam: 1048576) only re-downloads the chunks an edit touches (default 0)";

	public required string Output { get; init; }

	public required List<string> Roots { get; init; }

	public required List<string> ShaderDirectories { get; init; }

	public bool NoShaders { get; init; }

	public required string Base { get; init; }

	public int TileMax { get; init; } = 512;

	public List<string> Skip { get; init; } = new List<string>();

	public List<string> PremultipliedPaths { get; init; } = new List<string>();

	public int Level { get; init; } = 19;

	public AssetPackCodec Codec { get; init; } = AssetPackCodec.Zstd;

	public int Alignment { get; init; }

	public bool Rdo { get; init; }

	public string TextureSettings => $"{Codec} {Level} {TileMax} skip:{string.Join('|', Skip)} premul:{string.Join('|', PremultipliedPaths)} mesh:{1} obj:{1}";

	public static AssetBakeOptions? Parse(string[] args, AssetBakeHost host)
	{
		string text = null;
		List<string> list = new List<string>();
		List<string> list2 = new List<string>();
		string path = Directory.GetCurrentDirectory();
		int result = host.RawMaxSize;
		int result2 = 19;
		List<string> list3 = new List<string>(host.RawPaths);
		List<string> list4 = new List<string>();
		AssetPackCodec result3 = AssetPackCodec.Zstd;
		int result4 = 0;
		bool result5 = false;
		bool flag = false;
		for (int i = 0; i < args.Length; i++)
		{
			string text2 = ((i + 1 < args.Length) ? args[i + 1] : null);
			switch (args[i])
			{
			case "--no-shaders":
				flag = true;
				continue;
			case "--out":
				if (text2 != null)
				{
					text = text2;
					i++;
					continue;
				}
				break;
			case "--root":
				if (text2 != null)
				{
					list.Add(text2);
					i++;
					continue;
				}
				break;
			case "--shaders":
				if (text2 != null)
				{
					list2.Add(Path.GetFullPath(text2));
					i++;
					continue;
				}
				break;
			case "--base":
				if (text2 != null)
				{
					path = text2;
					i++;
					continue;
				}
				break;
			case "--tile-max":
				if (text2 != null)
				{
					if (!int.TryParse(text2, out result))
					{
						return null;
					}
					i++;
					continue;
				}
				break;
			case "--level":
				if (text2 != null)
				{
					if (!int.TryParse(text2, out result2))
					{
						return null;
					}
					i++;
					continue;
				}
				break;
			case "--skip":
				if (text2 != null)
				{
					list3.Add(text2);
					i++;
					continue;
				}
				break;
			case "--premul":
				if (text2 != null)
				{
					list4.Add(text2);
					i++;
					continue;
				}
				break;
			case "--align":
				if (text2 != null)
				{
					if (!int.TryParse(text2, out result4))
					{
						return null;
					}
					i++;
					continue;
				}
				break;
			case "--rdo":
				if (text2 != null)
				{
					if (!bool.TryParse(text2, out result5))
					{
						return null;
					}
					i++;
					continue;
				}
				break;
			case "--codec":
				if (text2 != null)
				{
					if (!Enum.TryParse<AssetPackCodec>(text2, ignoreCase: true, out result3))
					{
						return null;
					}
					i++;
					continue;
				}
				break;
			}
			if (text2 == null || !args[i].StartsWith("--", StringComparison.Ordinal) || !host.TryParseOption(args[i], text2))
			{
				return null;
			}
			i++;
		}
		if (text == null || list.Count == 0 || list2.Count == 0 != flag)
		{
			return null;
		}
		return new AssetBakeOptions
		{
			Output = text,
			Roots = list,
			ShaderDirectories = list2,
			NoShaders = flag,
			Base = Path.GetFullPath(path),
			TileMax = result,
			Level = result2,
			Skip = list3,
			PremultipliedPaths = list4,
			Codec = result3,
			Alignment = result4,
			Rdo = result5
		};
	}

	public string RelativeToBase(string path)
	{
		return Path.GetRelativePath(Base, path).Replace('\\', '/');
	}

	public List<string> EnumerateRootFiles()
	{
		return ContentSources.Enumerate(Roots);
	}

	public List<Source> SelectSources(IReadOnlyCollection<string> files)
	{
		return ContentSources.Select(files, PremultipliedPaths);
	}

	public string EntryName(Source source, bool premultiplied)
	{
		return ContentSources.EntryName(Base, source, premultiplied);
	}
}
