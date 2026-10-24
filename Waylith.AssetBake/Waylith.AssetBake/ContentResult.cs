using System.Collections.Generic;

namespace Waylith.AssetBake;

internal sealed record ContentResult(int Entries, int Reused, int Linked, int Failed, IReadOnlyDictionary<string, string> Stamps, IReadOnlyDictionary<string, string> Roles, IReadOnlyList<string> Errors);
