namespace Unity.Collections;

public struct KVPair<TKey, TValue>
{
	public TKey Key { get; }

	public TValue Value { get; }

	public KVPair(TKey key, TValue value)
	{
		Key = key;
		Value = value;
	}
}
