using System;
using System.Threading;
using Brutal.Logging;
using Brutal.Logging.Internal;
using Waylith.Graphics;
using Waylith.Jobs;

namespace Waylith.Hardware;

public static class HardwareProfile
{
	private static readonly Lock Sync = new Lock();

	private static GpuPerformanceProfile? published;

	private static int? jobHandle;

	private static bool started;

	public static GpuPerformanceTier Tier => Get().ClassifyTier();

	public static void BeginProbe(GraphicsDevice graphicsDevice)
	{
		ArgumentNullException.ThrowIfNull(graphicsDevice, "graphicsDevice");
		using (Sync.EnterScope())
		{
			if (!started)
			{
				started = true;
				jobHandle = WaylithJobRuntime.Schedule(delegate
				{
					ProbeJob(graphicsDevice);
				}, default(ReadOnlySpan<int>));
			}
		}
	}

	private static void ProbeJob(GraphicsDevice graphicsDevice)
	{
		GpuPerformanceProfile gpuPerformanceProfile;
		if (GpuPerformanceProfileStore.TryLoad(graphicsDevice, out var profile))
		{
			gpuPerformanceProfile = profile;
			LogCategory log = RenderLog.Log;
			bool shouldAppend;
			LogString<Information> message = new LogString<Information>(31, 1, log, out shouldAppend);
			if (shouldAppend)
			{
				message.AppendLiteral("HardwareProfile: loaded cached ");
				message.AppendFormatted(gpuPerformanceProfile);
			}
			log.Info(message, "ProbeJob", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Hardware\\HardwareProfile.cs", 44);
		}
		else
		{
			gpuPerformanceProfile = GpuProbe.Run(graphicsDevice);
			GpuPerformanceProfileStore.Save(graphicsDevice, gpuPerformanceProfile);
		}
		using (Sync.EnterScope())
		{
			published = gpuPerformanceProfile;
		}
	}

	public static GpuPerformanceProfile Get()
	{
		int handleId;
		using (Sync.EnterScope())
		{
			GpuPerformanceProfile? gpuPerformanceProfile = published;
			if (gpuPerformanceProfile.HasValue)
			{
				return gpuPerformanceProfile.GetValueOrDefault();
			}
			int? num = jobHandle;
			if (!num.HasValue)
			{
				return GpuPerformanceProfile.Conservative;
			}
			int valueOrDefault = num.GetValueOrDefault();
			handleId = valueOrDefault;
		}
		WaylithJobRuntime.Complete(handleId);
		using (Sync.EnterScope())
		{
			return published ?? GpuPerformanceProfile.Conservative;
		}
	}

	public static bool TryGet(out GpuPerformanceProfile profile)
	{
		using (Sync.EnterScope())
		{
			GpuPerformanceProfile? gpuPerformanceProfile = published;
			if (gpuPerformanceProfile.HasValue)
			{
				GpuPerformanceProfile valueOrDefault = gpuPerformanceProfile.GetValueOrDefault();
				profile = valueOrDefault;
				return true;
			}
		}
		profile = GpuPerformanceProfile.Conservative;
		return false;
	}
}
