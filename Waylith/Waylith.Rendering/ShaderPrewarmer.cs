using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Brutal.Logging;
using Brutal.Logging.Internal;
using Waylith.Logging;
using Waylith.Rendering.Compute;

namespace Waylith.Rendering;

public static class ShaderPrewarmer
{
	private static int started;

	public static void WarmInBackground(ShaderWarmupPlan plan)
	{
		if (Interlocked.Exchange(ref started, 1) == 0)
		{
			Task.Run(delegate
			{
				Warm(plan);
			});
		}
	}

	public static void Warm(ShaderWarmupPlan plan)
	{
		Stopwatch stopwatch = Stopwatch.StartNew();
		int compiled = 0;
		int cacheHits = 0;
		int failures = 0;
		ConcurrentDictionary<string, byte> failedFiles = new ConcurrentDictionary<string, byte>(StringComparer.OrdinalIgnoreCase);
		ParallelOptions parallelOptions = new ParallelOptions
		{
			MaxDegreeOfParallelism = System.Math.Max(1, Environment.ProcessorCount)
		};
		Parallel.ForEach(plan.Graphics, parallelOptions, delegate(GraphicsShaderJob job)
		{
			try
			{
				var (source, sourceName) = ShaderModuleCache.BuildVariantSource(job.Variant, job.Stage);
				RuntimeShaderCompiler.Compile(source, sourceName, "main", RuntimeShaderLanguage.Glsl, job.Stage, out var cacheHit);
				if (cacheHit)
				{
					Interlocked.Increment(ref cacheHits);
				}
				else
				{
					Interlocked.Increment(ref compiled);
				}
			}
			catch (Exception ex)
			{
				Interlocked.Increment(ref failures);
				if (failedFiles.TryAdd(job.SourceFile, 0))
				{
					LogCategory log = RenderLog.Log;
					bool shouldAppend;
					LogString<Warning> message = new LogString<Warning>(34, 2, log, out shouldAppend);
					if (shouldAppend)
					{
						message.AppendLiteral("ShaderPrewarm: failed to warm '");
						message.AppendFormatted(job.SourceFile);
						message.AppendLiteral("': ");
						message.AppendFormatted(ex.Message);
					}
					log.Warning(message, "Warm", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Rendering\\ShaderPrewarmer.cs", 57);
				}
			}
		});
		Parallel.ForEach(plan.FixedGraphics, parallelOptions, delegate(FixedGraphicsShaderJob job)
		{
			try
			{
				RuntimeShaderCompiler.Compile(ShaderSourceLoader.ReadShaderSource(job.SourceFile), job.DiagnosticName, "main", RuntimeShaderLanguage.Glsl, job.Stage, out var cacheHit2);
				if (cacheHit2)
				{
					Interlocked.Increment(ref cacheHits);
				}
				else
				{
					Interlocked.Increment(ref compiled);
				}
			}
			catch (Exception ex2)
			{
				Interlocked.Increment(ref failures);
				if (failedFiles.TryAdd(job.SourceFile, 0))
				{
					LogCategory log2 = RenderLog.Log;
					bool shouldAppend2;
					LogString<Warning> message2 = new LogString<Warning>(34, 2, log2, out shouldAppend2);
					if (shouldAppend2)
					{
						message2.AppendLiteral("ShaderPrewarm: failed to warm '");
						message2.AppendFormatted(job.SourceFile);
						message2.AppendLiteral("': ");
						message2.AppendFormatted(ex2.Message);
					}
					log2.Warning(message2, "Warm", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Rendering\\ShaderPrewarmer.cs", 82);
				}
			}
		});
		Parallel.ForEach(plan.Inline, parallelOptions, delegate(InlineShaderJob job)
		{
			try
			{
				RuntimeShaderCompiler.Compile(job.Source, job.DiagnosticName, "main", RuntimeShaderLanguage.Glsl, job.Stage, out var cacheHit3);
				if (cacheHit3)
				{
					Interlocked.Increment(ref cacheHits);
				}
				else
				{
					Interlocked.Increment(ref compiled);
				}
			}
			catch (Exception ex3)
			{
				Interlocked.Increment(ref failures);
				if (failedFiles.TryAdd(job.DiagnosticName, 0))
				{
					LogCategory log3 = RenderLog.Log;
					bool shouldAppend3;
					LogString<Warning> message3 = new LogString<Warning>(41, 2, log3, out shouldAppend3);
					if (shouldAppend3)
					{
						message3.AppendLiteral("ShaderPrewarm: failed to warm inline '");
						message3.AppendFormatted(job.DiagnosticName);
						message3.AppendLiteral("': ");
						message3.AppendFormatted(ex3.Message);
					}
					log3.Warning(message3, "Warm", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Rendering\\ShaderPrewarmer.cs", 107);
				}
			}
		});
		Parallel.ForEach(plan.Compute, parallelOptions, delegate(ComputeShaderJob job)
		{
			try
			{
				ComputeKernelCache.WarmCache(job.SourceFile, job.EntryPoints, job.Language);
			}
			catch (Exception ex4)
			{
				Interlocked.Increment(ref failures);
				if (failedFiles.TryAdd(job.SourceFile, 0))
				{
					LogCategory log4 = RenderLog.Log;
					bool shouldAppend4;
					LogString<Warning> message4 = new LogString<Warning>(42, 2, log4, out shouldAppend4);
					if (shouldAppend4)
					{
						message4.AppendLiteral("ShaderPrewarm: failed to warm compute '");
						message4.AppendFormatted(job.SourceFile);
						message4.AppendLiteral("': ");
						message4.AppendFormatted(ex4.Message);
					}
					log4.Warning(message4, "Warm", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Rendering\\ShaderPrewarmer.cs", 121);
				}
			}
		});
		VerboseLog.Log($"ShaderPrewarm: {plan.Graphics.Count} graphics variants + {plan.FixedGraphics.Count} postfx + {plan.Inline.Count} inline ({cacheHits} cache hits, {compiled} compiled), {plan.Compute.Count} compute kernels, {failures} failures in {stopwatch.Elapsed.TotalMilliseconds:0}ms.");
	}
}
