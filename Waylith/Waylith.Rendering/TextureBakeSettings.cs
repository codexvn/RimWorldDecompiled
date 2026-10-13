using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace Waylith.Rendering;

public sealed record TextureBakeSettings
{
	public BcEncodeTarget Target { get; init; } = BcEncodeTarget.Bc7;

	public float Quality { get; init; } = 1f;

	public Bc7PackFlags Bc7Flags { get; init; } = Bc7PackFlags.WaylithDefault;

	public int RawMaxSize { get; init; } = 512;

	public string[] RawPaths { get; init; } = Array.Empty<string>();

	public bool AllowBc1 { get; init; } = true;

	public Func<int, int, bool, string, BcEncodeTarget>? SelectTarget { get; init; }

	public BcRdoRule[] RdoRules { get; init; } = Array.Empty<BcRdoRule>();

	public Func<string, bool>? BorderMipMaps { get; init; }

	public Func<string, TextureUsage>? UsageFor { get; init; }

	public MipFilter MipFilter { get; init; } = TexturePixelData.Rgba32MipChainFilter;

	public bool EncodeOnGpu { get; init; }

	private static readonly Func<int, int, bool, string, BcEncodeTarget> GlobalLoadTimeTarget = TextureCompressionSettings.SelectLoadTimeTarget;

	private static readonly Func<string, TextureUsage> GlobalUsage = TextureUsageHints.For;

	public BcEncodeTarget TargetFor(int width, int height, bool opaque, string sourcePath)
	{
		return SelectTarget?.Invoke(width, height, opaque, sourcePath) ?? DefaultSelectTarget(width, height, opaque, sourcePath);
	}

	public BcEncodeTarget DefaultSelectTarget(int width, int height, bool opaque, string? sourcePath)
	{
		if (Target == BcEncodeTarget.None)
		{
			return BcEncodeTarget.None;
		}
		if (width <= RawMaxSize && height <= RawMaxSize)
		{
			return BcEncodeTarget.None;
		}
		if (sourcePath != null && (TextureCompressionSettings.MatchesAny(sourcePath, RawPaths) || Path.GetFileNameWithoutExtension(sourcePath).EndsWith("_Uncompressed", StringComparison.OrdinalIgnoreCase)))
		{
			return BcEncodeTarget.None;
		}
		if (!opaque || !AllowBc1)
		{
			return Target;
		}
		return BcEncodeTarget.Bc1;
	}

	public TextureUsage Usage(string sourcePath)
	{
		return UsageFor?.Invoke(sourcePath) ?? TextureUsage.Default;
	}

	public BcRdoRule? SelectRdo(BcEncodeTarget target, string sourcePath)
	{
		return TextureCompressionSettings.SelectRdo(RdoRules, target, sourcePath);
	}

	public static TextureBakeSettings FromGlobals()
	{
		return new TextureBakeSettings
		{
			Target = TextureCompressionSettings.Target,
			Quality = TextureCompressionSettings.Quality,
			Bc7Flags = CpuBlockEncoder.Bc7Flags,
			RawMaxSize = TextureCompressionSettings.RawMaxSize,
			RawPaths = TextureCompressionSettings.RawPaths,
			AllowBc1 = TextureCompressionSettings.AllowBc1,
			SelectTarget = GlobalLoadTimeTarget,
			RdoRules = TextureCompressionSettings.RdoRules,
			BorderMipMaps = GpuTextureService.WantsBorderMipMaps,
			UsageFor = GlobalUsage,
			MipFilter = TexturePixelData.Rgba32MipChainFilter
		};
	}

	[CompilerGenerated]
	private TextureBakeSettings(TextureBakeSettings original)
	{
		Target = original.Target;
		Quality = original.Quality;
		Bc7Flags = original.Bc7Flags;
		RawMaxSize = original.RawMaxSize;
		RawPaths = original.RawPaths;
		AllowBc1 = original.AllowBc1;
		SelectTarget = original.SelectTarget;
		RdoRules = original.RdoRules;
		BorderMipMaps = original.BorderMipMaps;
		UsageFor = original.UsageFor;
		MipFilter = original.MipFilter;
		EncodeOnGpu = original.EncodeOnGpu;
	}

	public TextureBakeSettings()
	{
	}
}
