namespace Waylith.AssetBake;

public sealed record Source(string Path, bool[] Variants, SourceKind Kind, string? RawName = null)
{
	public bool IsMesh => Kind == SourceKind.Mesh;
}
