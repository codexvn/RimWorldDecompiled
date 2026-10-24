using System;
using System.Collections.Generic;
using Waylith.Assets;
using Waylith.Rendering;

namespace Waylith.AssetBake;

public sealed class ContentBakeRequest
{
	public required string Output { get; init; }

	public required string Base { get; init; }

	public required IReadOnlyList<string> Roots { get; init; }

	public int Level { get; init; } = 19;

	public AssetPackCodec Codec { get; init; } = AssetPackCodec.Zstd;

	public int Alignment { get; init; }

	public TextureBakeSettings Texture { get; init; } = new TextureBakeSettings();

	public IReadOnlyList<string> PremultipliedPaths { get; init; } = Array.Empty<string>();

	public IReadOnlyList<RawSource> RawFiles { get; init; } = Array.Empty<RawSource>();

	public IReadOnlyList<string> StoredRawExtensions { get; init; } = Array.Empty<string>();

	public bool WriteStamps { get; init; } = true;

	public bool WriteRoles { get; init; } = true;

	public string CodeIdentity { get; init; } = DefaultCodeIdentity;

	public int MaxDegreeOfParallelism { get; init; } = -1;

	public static string DefaultCodeIdentity => $"textures:v{23} {typeof(GpuTextureService).Assembly.ManifestModule.ModuleVersionId}";
}
