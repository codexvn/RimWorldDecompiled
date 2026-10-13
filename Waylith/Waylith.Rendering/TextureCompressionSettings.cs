using System;
using System.IO;
using Brutal.SlangApi;

namespace Waylith.Rendering;

public static class TextureCompressionSettings
{
	public static BcEncodeTarget Target { get; private set; } = BcEncodeTarget.None;

	public static float Quality { get; private set; } = 1f;

	public static bool Enabled => Target != BcEncodeTarget.None;

	public static bool AllowBc1 { get; set; } = true;

	public static Func<int, int, bool, string, BcEncodeTarget>? LoadTimeTarget { get; set; }

	public static int RawMaxSize { get; set; }

	public static string[] RawPaths { get; set; } = Array.Empty<string>();

	public static BcRdoRule[] RdoRules { get; set; } = Array.Empty<BcRdoRule>();

	public static string CacheTag => Target switch
	{
		BcEncodeTarget.None => "raw", 
		BcEncodeTarget.Bc7 => $"bc7f{CpuBlockEncoder.Bc7Flags:x}{(TextureBlockEncoder.Bc7FastPermitted ? "g" : "s")}", 
		_ => $"{Target.ToString().ToLowerInvariant()}q{(int)MathF.Round(Quality * 100f)}{EmitTag}", 
	} + (AllowBc1 ? "" : "nobc1");

	private static string EmitTag
	{
		get
		{
			if (RuntimeShaderCompiler.HlslComputeEmitMethod != EmitSpirvMethod.ViaGlsl)
			{
				return "d";
			}
			return "g";
		}
	}

	public static BcEncodeTarget SelectLoadTimeTarget(int width, int height, bool opaque, string sourcePath)
	{
		return LoadTimeTarget?.Invoke(width, height, opaque, sourcePath) ?? BcEncodeTarget.None;
	}

	public static TextureEncoding SelectLoadTimeEncoding(in LoadedTextureData loaded, string sourcePath)
	{
		return TextureEncodingRules.Refine(SelectLoadTimeTarget(loaded.Width, loaded.Height, loaded.Opaque, sourcePath), (loaded.Format == LoadedTextureFormat.Rgba32 && loaded.Levels.Length != 0) ? loaded.Levels[0] : null, loaded.Width, loaded.Height, loaded.Opaque, sourcePath);
	}

	public static TextureEncoding SelectEncoding(byte[]? rgba, int width, int height, bool opaque, string? sourcePath)
	{
		return TextureEncodingRules.Refine(SelectTarget(width, height, opaque, sourcePath), rgba, width, height, opaque, sourcePath);
	}

	public static BcEncodeTarget SelectTarget(int width, int height, bool opaque, string? sourcePath)
	{
		if (!Enabled)
		{
			return BcEncodeTarget.None;
		}
		if (width <= RawMaxSize && height <= RawMaxSize)
		{
			return BcEncodeTarget.None;
		}
		if (sourcePath != null && (MatchesAny(sourcePath, RawPaths) || Path.GetFileNameWithoutExtension(sourcePath).EndsWith("_Uncompressed", StringComparison.OrdinalIgnoreCase)))
		{
			return BcEncodeTarget.None;
		}
		if (!opaque || !AllowBc1)
		{
			return Target;
		}
		return BcEncodeTarget.Bc1;
	}

	public static bool MatchesAny(string path, string[] patterns)
	{
		string text = path.Replace('\\', '/');
		foreach (string value in patterns)
		{
			if (text.Contains(value, StringComparison.OrdinalIgnoreCase))
			{
				return true;
			}
		}
		return false;
	}

	public static BcRdoRule? SelectRdo(BcEncodeTarget target, string sourcePath)
	{
		return SelectRdo(RdoRules, target, sourcePath);
	}

	public static BcRdoRule? SelectRdo(BcRdoRule[] rules, BcEncodeTarget target, string sourcePath)
	{
		string text = sourcePath.Replace('\\', '/');
		for (int i = 0; i < rules.Length; i++)
		{
			BcRdoRule value = rules[i];
			if (value.Target == target && text.Contains(value.PathContains, StringComparison.OrdinalIgnoreCase))
			{
				if (!(value.Lambda > 0f))
				{
					return null;
				}
				return value;
			}
		}
		return null;
	}

	public static void Configure(BcEncodeTarget target, float quality)
	{
		Target = target;
		Quality = System.Math.Clamp(quality, 0f, 1f);
	}
}
