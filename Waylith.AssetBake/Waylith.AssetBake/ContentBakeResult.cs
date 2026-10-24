using System.Collections.Generic;

namespace Waylith.AssetBake;

public sealed record ContentBakeResult(bool Succeeded, int Entries, int Reused, int Deduplicated, int Failed, IReadOnlyList<string> Errors, bool Unchanged);
