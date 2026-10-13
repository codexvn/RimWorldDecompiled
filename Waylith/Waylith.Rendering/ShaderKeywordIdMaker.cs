using System;
using System.Collections.Concurrent;
using System.Threading;

namespace Waylith.Rendering;

public static class ShaderKeywordIdMaker
{
	private static readonly ConcurrentDictionary<string, long> IdByKey = new ConcurrentDictionary<string, long>(StringComparer.Ordinal);

	private static long NextId;

	public static long GetOrCreateId(string? keywordKey)
	{
		if (string.IsNullOrEmpty(keywordKey))
		{
			return 0L;
		}
		return IdByKey.GetOrAdd(keywordKey, (string _) => Interlocked.Increment(ref NextId));
	}
}
