using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Waylith.Assets;
using Waylith.Rendering.Compute;

namespace Waylith.Rendering;

public static class ShaderPackBaker
{
	public static ConcurrentDictionary<string, (byte[] Record, bool Reused)> Bake(ShaderWarmupPlan plan, AssetPack? previous, ConcurrentQueue<string> failures)
	{
		ConcurrentDictionary<string, (byte[] Record, bool Reused)> baked = new ConcurrentDictionary<string, (byte[], bool)>(StringComparer.Ordinal);
		List<Action> list = new List<Action>(plan.Count);
		foreach (ComputeShaderJob job2 in plan.Compute)
		{
			list.Add(delegate
			{
				Add(job2.SourceFile, () => ComputeKernelCache.CompileForPack(job2.SourceFile, job2.EntryPoints, job2.Language, previous));
			});
		}
		foreach (GraphicsShaderJob job3 in plan.Graphics)
		{
			list.Add(delegate
			{
				Add(job3.SourceFile, delegate
				{
					var (source, sourceName) = ShaderModuleCache.BuildVariantSource(job3.Variant, job3.Stage);
					return RuntimeShaderCompiler.CompileForPack(source, sourceName, new string[1] { "main" }, RuntimeShaderLanguage.Glsl, job3.Stage, previous);
				});
			});
		}
		foreach (FixedGraphicsShaderJob job4 in plan.FixedGraphics)
		{
			list.Add(delegate
			{
				Add(job4.SourceFile, () => RuntimeShaderCompiler.CompileForPack(ShaderSourceLoader.ReadShaderSource(job4.SourceFile), job4.DiagnosticName, new string[1] { "main" }, RuntimeShaderLanguage.Glsl, job4.Stage, previous));
			});
		}
		foreach (InlineShaderJob job5 in plan.Inline)
		{
			list.Add(delegate
			{
				Add(job5.DiagnosticName, () => RuntimeShaderCompiler.CompileForPack(job5.Source, job5.DiagnosticName, new string[1] { "main" }, RuntimeShaderLanguage.Glsl, job5.Stage, previous));
			});
		}
		Parallel.ForEach(Partitioner.Create(list, EnumerablePartitionerOptions.NoBuffering), delegate(Action job)
		{
			job();
		});
		return baked;
		void Add(string label, Func<List<(string Name, byte[] Record, bool Reused)>> compile)
		{
			try
			{
				foreach (var (key, item, item2) in compile())
				{
					baked.TryAdd(key, (item, item2));
				}
			}
			catch (Exception ex)
			{
				failures.Enqueue(label + ": " + ex.Message);
			}
		}
	}
}
