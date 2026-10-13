namespace Unity.Collections;

public struct NativeParallelMultiHashMapIterator<TKey>
{
	internal nint Header;

	internal TKey Key;

	internal int Index;
}
