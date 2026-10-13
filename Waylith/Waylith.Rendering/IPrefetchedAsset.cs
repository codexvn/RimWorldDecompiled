namespace Waylith.Rendering;

public interface IPrefetchedAsset
{
	long Bytes { get; }

	void Release();
}
