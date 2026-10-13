namespace Waylith.Collections;

public interface IRefLookup<TKey>
{
	int KeyHash();

	bool KeyEquals(in TKey key);

	TKey ToKey();
}
