using System;
using System.Collections.Generic;
using System.Threading;
using Waylith.Assets;
using Waylith.Graphics.Meshes;
using Waylith.Rendering;

namespace Waylith.AssetBake;

internal sealed class ContentPassInput
{
	public required string Output { get; init; }

	public required string Base { get; init; }

	public required int Level { get; init; }

	public required AssetPackCodec Codec { get; init; }

	public required TextureBakeSettings Texture { get; init; }

	public AtlasSources? Atlas { get; init; }

	public Func<string, CpuMeshData?>? ParseMesh { get; init; }

	public IReadOnlyCollection<string> StoredRawExtensions { get; init; } = (IReadOnlyCollection<string>)(object)Array.Empty<string>();

	public int MaxDegreeOfParallelism { get; init; } = -1;

	public Action<string>? Progress { get; init; }

	public CancellationToken Cancellation { get; init; }

	public AssetPackCodec EntryCodec
	{
		get
		{
			if (Level > 0)
			{
				return Codec;
			}
			return AssetPackCodec.Store;
		}
	}
}
