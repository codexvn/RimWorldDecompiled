using System.Collections.Concurrent;
using System.Threading;

namespace Waylith.Rendering;

public static class PipelineSortIdRegistry
{
	private static readonly ConcurrentDictionary<(long ShaderVariantId, PipelineState State), int> IdByKey = new ConcurrentDictionary<(long, PipelineState), int>();

	private static int NextId;

	public static int GetOrCreateId(long shaderVariantId, in PipelineState state)
	{
		return IdByKey.GetOrAdd((shaderVariantId, state), ((long ShaderVariantId, PipelineState State) _) => Interlocked.Increment(ref NextId));
	}
}
