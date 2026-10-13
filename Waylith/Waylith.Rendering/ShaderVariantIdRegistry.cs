using System.Collections.Generic;

namespace Waylith.Rendering;

public static class ShaderVariantIdRegistry
{
	private static readonly Dictionary<ShaderVariantKey, long> IdByKey = new Dictionary<ShaderVariantKey, long>();

	private static readonly object Lock = new object();

	private static long NextId = 1L;

	public static long GetOrCreateId(ShaderVariantKey key)
	{
		lock (Lock)
		{
			if (IdByKey.TryGetValue(key, out var value))
			{
				return value;
			}
			value = NextId++;
			IdByKey.Add(key, value);
			return value;
		}
	}
}
