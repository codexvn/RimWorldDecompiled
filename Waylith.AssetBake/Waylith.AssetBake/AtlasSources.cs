using System.Collections.Generic;

namespace Waylith.AssetBake;

public sealed record AtlasSources(IReadOnlySet<string> Tiles, IReadOnlySet<string> SampledSources);
