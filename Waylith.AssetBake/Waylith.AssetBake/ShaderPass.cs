using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using Waylith.Assets;
using Waylith.Rendering;

namespace Waylith.AssetBake;

internal static class ShaderPass
{
	public static ConcurrentDictionary<string, (byte[] Record, bool Reused)>? Compile(ShaderWarmupPlan plan, AssetPack? previous)
	{
		Stopwatch stopwatch = Stopwatch.StartNew();
		RuntimeShaderCompiler.SetMaxConcurrentSessions(System.Math.Max(2, Environment.ProcessorCount / 2));
		ConcurrentQueue<string> concurrentQueue = new ConcurrentQueue<string>();
		ConcurrentDictionary<string, (byte[], bool)> concurrentDictionary = ShaderPackBaker.Bake(plan, previous, concurrentQueue);
		ReleaseSessions();
		int value = concurrentDictionary.Values.Count<(byte[], bool)>(((byte[] Record, bool Reused) shader) => shader.Reused);
		Console.WriteLine($"Shaders: {plan.Count} jobs -> {concurrentDictionary.Count} entries ({value} reused, {concurrentQueue.Count} failed) in {stopwatch.Elapsed.TotalSeconds:F1}s");
		foreach (string item in concurrentQueue)
		{
			Console.Error.WriteLine("  " + item);
		}
		if (!concurrentQueue.IsEmpty)
		{
			return null;
		}
		return concurrentDictionary;
	}

	private static void ReleaseSessions()
	{
		RuntimeShaderCompiler.ReleaseSessions();
		GC.Collect();
		GC.WaitForPendingFinalizers();
		NativeHeap.Trim();
	}
}
