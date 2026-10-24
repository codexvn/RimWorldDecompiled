using System.Collections.Generic;

namespace PsdSharp.TaggedBlocks;

public class TaggedBlocksCollection(List<TaggedBlock> blocks)
{
	public IReadOnlyCollection<TaggedBlock> Blocks => blocks;

	public T? TryGet<T>() where T : TaggedBlock
	{
		AdditionalLayerInfoKey valueOrDefault = TaggedBlocksRegistry.KeyByLayer.GetValueOrDefault(typeof(T));
		if (valueOrDefault == null)
		{
			return null;
		}
		return TryGet(valueOrDefault) as T;
	}

	public TaggedBlock? TryGet(AdditionalLayerInfoKey key)
	{
		return TryGet(key.Key);
	}

	public TaggedBlock? TryGet(string key)
	{
		return blocks.Find((TaggedBlock block) => block.Key.Key == key);
	}
}
