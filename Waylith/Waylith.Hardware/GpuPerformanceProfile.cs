using System;

namespace Waylith.Hardware;

public readonly struct GpuPerformanceProfile
{
	public const float ReferenceAluGFlops = 1036f;

	public const float ReferenceBandwidthGB = 71.6f;

	public const float MediumTierThreshold = 6.5f;

	public const float HighTierThreshold = 12f;

	public required float AluGFlopsPerSecond { get; init; }

	public required float MemoryBandwidthGBPerSecond { get; init; }

	public required int AluSaturationThreads { get; init; }

	public required bool Measured { get; init; }

	public float ComputeScore => AluGFlopsPerSecond / 1036f;

	public float BandwidthScore => MemoryBandwidthGBPerSecond / 71.6f;

	public float OverallScore => MathF.Sqrt(MathF.Max(0f, ComputeScore) * MathF.Max(0f, BandwidthScore));

	public static GpuPerformanceProfile Conservative
	{
		get
		{
			GpuPerformanceProfile result = default(GpuPerformanceProfile);
			result.AluGFlopsPerSecond = 0f;
			result.MemoryBandwidthGBPerSecond = 0f;
			result.AluSaturationThreads = 0;
			result.Measured = false;
			return result;
		}
	}

	public GpuPerformanceTier ClassifyTier()
	{
		return ClassifyTier(OverallScore);
	}

	public static GpuPerformanceTier ClassifyTier(float overallScore)
	{
		if (!(overallScore >= 12f))
		{
			if (overallScore >= 6.5f)
			{
				return GpuPerformanceTier.Medium;
			}
			return GpuPerformanceTier.Low;
		}
		return GpuPerformanceTier.High;
	}

	public override string ToString()
	{
		if (!Measured)
		{
			return "GPU probe: not measured (conservative / low tier)";
		}
		return $"GPU probe: {AluGFlopsPerSecond:0} GFLOP/s (sat @ {AluSaturationThreads} threads), {MemoryBandwidthGBPerSecond:0.0} GB/s -> compute {ComputeScore:0.00}x, bandwidth {BandwidthScore:0.00}x, overall {OverallScore:0.00}x ({ClassifyTier()})";
	}
}
