using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Waylith.Assets;
using Waylith.Rendering;

namespace Waylith.AssetBake;

public static class AssetBaker
{
	public static int Run(string[] args, AssetBakeHost host)
	{
		if (args.Length >= 2 && args[0] == "--verify-decode")
		{
			return VerifyDecode.Run(args[1], (args.Length > 2) ? int.Parse(args[2]) : int.MaxValue);
		}
		AssetBakeOptions options = AssetBakeOptions.Parse(args, host);
		if (options == null)
		{
			Console.Error.WriteLine((host.Usage.Length == 0) ? "AssetBake --out <pack> --root <dir> [--root <dir>...] (--shaders <dir> [--shaders <dir>...] | --no-shaders) [options]\n  --shaders <dir>     a shader source directory, searched the way the game searches it (repeatable)\n  --no-shaders        bake content alone: no shader pass, and the pack carries no shader entries\n  --base <dir>        the game's data directory; entry names are relative to it (default: current directory)\n  --tile-max <n>      textures at or under n on both axes stay uncompressed (default: the game's)\n  --skip <substring>  leave matching paths uncompressed, on top of the game's (repeatable)\n  --premul <substring>  bake matching paths premultiplied instead of straight (repeatable)\n  --level <n>         zstd level (default 19, what ships); 3 bakes ~5x faster for ~18% more size;\n                      0 stores entries uncompressed, a diagnostic\n  --codec <name>      zstd (default) or lz4: LZ4 decodes ~3x faster and packs ~30% larger,\n                      and its entries are stored .lz4 frames rather than zip-compressed. Its levels\n                      are LZ4's own: below 3 fast, 3-9 HC, 10-11 OPT, 12 and up MAX\n  --rdo <bool>        trade a little quality for smaller zstd entries, under the host's per-format rules\n                      (default false; ~10% off the pack, ~12 s more of a clean bake)\n  --align <bytes>     keep entries from straddling multiples of this, so a store that patches in fixed\n                      chunks (Steam: 1048576) only re-downloads the chunks an edit touches (default 0)" : ("AssetBake --out <pack> --root <dir> [--root <dir>...] (--shaders <dir> [--shaders <dir>...] | --no-shaders) [options]\n  --shaders <dir>     a shader source directory, searched the way the game searches it (repeatable)\n  --no-shaders        bake content alone: no shader pass, and the pack carries no shader entries\n  --base <dir>        the game's data directory; entry names are relative to it (default: current directory)\n  --tile-max <n>      textures at or under n on both axes stay uncompressed (default: the game's)\n  --skip <substring>  leave matching paths uncompressed, on top of the game's (repeatable)\n  --premul <substring>  bake matching paths premultiplied instead of straight (repeatable)\n  --level <n>         zstd level (default 19, what ships); 3 bakes ~5x faster for ~18% more size;\n                      0 stores entries uncompressed, a diagnostic\n  --codec <name>      zstd (default) or lz4: LZ4 decodes ~3x faster and packs ~30% larger,\n                      and its entries are stored .lz4 frames rather than zip-compressed. Its levels\n                      are LZ4's own: below 3 fast, 3-9 HC, 10-11 OPT, 12 and up MAX\n  --rdo <bool>        trade a little quality for smaller zstd entries, under the host's per-format rules\n                      (default false; ~10% off the pack, ~12 s more of a clean bake)\n  --align <bytes>     keep entries from straddling multiples of this, so a store that patches in fixed\n                      chunks (Steam: 1048576) only re-downloads the chunks an edit touches (default 0)\n" + host.Usage));
			return 1;
		}
		NativeHeap.KeepLargeBlocksMapped();
		Stopwatch stopwatch = Stopwatch.StartNew();
		List<string> list = options.EnumerateRootFiles();
		List<Source> list2 = options.SelectSources(list);
		ContentSources.AddRaw(list2, host.RawFiles(options));
		BakeFingerprint bakeFingerprint = BakeFingerprint.Compute(options, host, list, list2);
		BakeFingerprint fingerprint;
		AssetPack previous = ContentPackBaker.OpenPrevious(options.Output, out fingerprint, Console.WriteLine);
		bool flag = fingerprint?.Textures == bakeFingerprint.Textures;
		bool flag2 = fingerprint?.Shaders == bakeFingerprint.Shaders;
		if (flag && flag2 && fingerprint?.Layout == bakeFingerprint.Layout)
		{
			previous?.Dispose();
			Console.WriteLine($"{options.Output} is up to date ({list2.Count} sources, {list.Count} files checked in {stopwatch.Elapsed.TotalSeconds:F2}s)");
			return 0;
		}
		bool settingsUnchanged = fingerprint?.Settings == bakeFingerprint.Settings;
		GpuTextureService.TextureCacheWrites = false;
		ShaderWarmupPlan shaderWarmupPlan;
		AtlasSources atlasSources;
		try
		{
			shaderWarmupPlan = ((flag2 || options.NoShaders) ? null : host.BuildShaderPlan(options));
			atlasSources = (flag ? null : host.ResolveAtlasSources(options));
		}
		catch (InvalidDataException ex)
		{
			Console.Error.WriteLine(ex.Message);
			previous?.Dispose();
			return 1;
		}
		TextureCompressionSettings.Configure(BcEncodeTarget.Bc7, 1f);
		CpuBlockEncoder.Bc7Flags = Bc7PackFlags.WaylithDefault;
		TextureCompressionSettings.RawPaths = options.Skip.ToArray();
		TextureCompressionSettings.RdoRules = (options.Rdo ? host.RdoRules.ToArray() : Array.Empty<BcRdoRule>());
		Func<int, int, bool, string, BcEncodeTarget> func = null;
		if (atlasSources == null)
		{
			TextureCompressionSettings.RawMaxSize = options.TileMax;
		}
		else
		{
			TextureCompressionSettings.RawMaxSize = 0;
			string[] smallRawPaths = host.SmallRawPaths.ToArray();
			foreach (string tile in atlasSources.Tiles)
			{
				TextureUsageHints.RegisterSource(tile, TextureUsage.AtlasTile);
			}
			func = (int width, int height, bool opaque, string path) => (!atlasSources.Tiles.Contains(path)) ? ((!atlasSources.SampledSources.Contains(path) && (width > options.TileMax || height > options.TileMax || !TextureCompressionSettings.MatchesAny(path, smallRawPaths))) ? TextureCompressionSettings.SelectTarget(width, height, opaque, path) : BcEncodeTarget.None) : BcEncodeTarget.Bc7;
		}
		TextureCompressionSettings.LoadTimeTarget = func ?? new Func<int, int, bool, string, BcEncodeTarget>(TextureCompressionSettings.SelectTarget);
		ContentPassInput input = new ContentPassInput
		{
			Output = options.Output,
			Base = options.Base,
			Level = options.Level,
			Codec = options.Codec,
			Texture = new TextureBakeSettings
			{
				Target = TextureCompressionSettings.Target,
				Quality = TextureCompressionSettings.Quality,
				Bc7Flags = CpuBlockEncoder.Bc7Flags,
				RawMaxSize = TextureCompressionSettings.RawMaxSize,
				RawPaths = TextureCompressionSettings.RawPaths,
				AllowBc1 = TextureCompressionSettings.AllowBc1,
				SelectTarget = func,
				RdoRules = TextureCompressionSettings.RdoRules,
				BorderMipMaps = GpuTextureService.WantsBorderMipMaps,
				UsageFor = TextureUsageHints.For,
				MipFilter = TexturePixelData.Rgba32MipChainFilter
			},
			Atlas = atlasSources,
			ParseMesh = (string path) => host.ParseMesh(path, options)
		};
		Directory.CreateDirectory(Path.GetDirectoryName(Path.GetFullPath(options.Output)));
		string text = options.Output + ".tmp";
		bool flag3 = true;
		IReadOnlyDictionary<string, string> stamps = null;
		IReadOnlyDictionary<string, string> roles = null;
		AssetPackWriter writer = new AssetPackWriter(text, options.Alignment);
		try
		{
			if (!options.NoShaders)
			{
				if (shaderWarmupPlan == null)
				{
					Carry(previous, writer, IsShaderEntry);
				}
				else
				{
					ConcurrentDictionary<string, (byte[], bool)> concurrentDictionary = ShaderPass.Compile(shaderWarmupPlan, previous);
					if (concurrentDictionary != null)
					{
						Parallel.ForEach<KeyValuePair<string, (byte[], bool)>>(concurrentDictionary, delegate(KeyValuePair<string, (byte[] Record, bool Reused)> shader)
						{
							AddShader(writer, shader.Key, shader.Value.Reused, shader.Value.Record, settingsUnchanged ? previous : null, options);
						});
					}
					else
					{
						flag3 = false;
					}
				}
			}
			if (flag)
			{
				Carry(previous, writer, (string name) => !IsShaderEntry(name));
			}
			else
			{
				Stopwatch stopwatch2 = Stopwatch.StartNew();
				ContentResult contentResult = ContentPass.Run(input, list2, previous, settingsUnchanged, writer);
				foreach (string error in contentResult.Errors)
				{
					Console.Error.WriteLine(error);
				}
				int num = list2.Count((Source source) => source.IsMesh);
				int num2 = list2.Count((Source source) => source.Kind == SourceKind.Raw);
				Console.WriteLine($"Content: {list2.Count - num - num2} images, {num} meshes and {num2} raw files -> {contentResult.Entries} entries ({contentResult.Reused} reused, {contentResult.Linked} deduplicated, {contentResult.Failed} failed) in {stopwatch2.Elapsed.TotalSeconds:F1}s");
				stamps = contentResult.Stamps;
				roles = contentResult.Roles;
				flag3 &= contentResult.Failed == 0;
			}
		}
		finally
		{
			if (writer != null)
			{
				((IDisposable)writer).Dispose();
			}
		}
		previous?.Dispose();
		if (!flag3)
		{
			File.Delete(text);
			return 1;
		}
		bool flag4 = ContentPackBaker.Commit(text, options.Output, stamps, roles, bakeFingerprint);
		Console.WriteLine($"{options.Output}: {(double)new FileInfo(options.Output).Length / 1048576.0:F1} MB in {stopwatch.Elapsed.TotalSeconds:F1}s" + (flag ? ", textures carried over" : "") + (flag2 ? ", shaders carried over" : "") + (flag4 ? ", unchanged" : ""));
		return 0;
	}

	private static bool IsShaderEntry(string name)
	{
		return name.StartsWith("shaders/", StringComparison.Ordinal);
	}

	private static void Carry(AssetPack previous, AssetPackWriter writer, Func<string, bool> include)
	{
		string[] array = previous.Names.Where(include).ToArray();
		foreach (string name in array)
		{
			if (AssetPackWriter.TryReference(previous, name, out var payload, out var length, out var crc, out var codec))
			{
				writer.Add(name, payload, length, crc, codec);
			}
		}
	}

	private static void AddShader(AssetPackWriter writer, string name, bool reused, byte[] record, AssetPack? reusable, AssetBakeOptions options)
	{
		if (reused && reusable != null && AssetPackWriter.TryReference(reusable, name, out var payload, out var length, out var crc, out var codec))
		{
			writer.Add(name, payload, length, crc, codec);
		}
		else
		{
			ContentPass.Add(writer, name, record, options.Codec, options.Level);
		}
	}
}
