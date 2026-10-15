using Brutal.Numerics;
using Waylith.Diagnostics;
using Waylith.Graphics;
using Waylith.Rendering;

namespace Waylith.CLI;

internal static class EngineSwitches
{
	public static void Apply(EngineApplicationOptions options)
	{
		ICommandLine commandLineAccessor = options.CommandLineAccessor;
		ColdBoot.Enabled |= commandLineAccessor.HasSwitch("ColdBoot");
		GpuTextureService.TrackRehydration |= commandLineAccessor.HasSwitch("TrackRehydration");
		options.UseKosmicKrisp |= commandLineAccessor.HasSwitch("KosmicKrisp");
		options.HideWindow |= commandLineAccessor.HasSwitch("NoWindow");
		VulkanStartup.LoaderChoice = (commandLineAccessor.HasSwitch("BundledVulkanLoader") ? VulkanLoaderChoice.Bundled : (commandLineAccessor.HasSwitch("SystemVulkanLoader") ? VulkanLoaderChoice.System : VulkanLoaderChoice.Newest));
		VulkanStartup.DisableImplicitLayers |= commandLineAccessor.HasSwitch("NoVulkanLayers");
		VulkanStartup.FallbackDisabled |= commandLineAccessor.HasSwitch("NoVulkanFallback");
		VulkanStartup.ResetFallback |= commandLineAccessor.HasSwitch("ResetVulkanFallback");
		string value = commandLineAccessor.GetValue("texCacheCompression");
		if (value != null)
		{
			GpuTextureService.CacheCodec = value.Trim().ToLowerInvariant() switch
			{
				"none" => TextureCacheCodec.None, 
				"zstd" => TextureCacheCodec.Zstd, 
				"lz4hc" => TextureCacheCodec.Lz4Hc, 
				_ => TextureCacheCodec.Lz4, 
			};
		}
		int2? @int = commandLineAccessor.GetInt2("WindowSize");
		if (@int.HasValue)
		{
			int2 valueOrDefault = @int.GetValueOrDefault();
			options.InitialWindowSize = valueOrDefault;
			EngineWindow.SizeLocked = true;
		}
		@int = commandLineAccessor.GetInt2("WindowPos");
		if (@int.HasValue)
		{
			int2 valueOrDefault2 = @int.GetValueOrDefault();
			options.InitialWindowPosition = valueOrDefault2;
			EngineWindow.PositionLocked = true;
		}
	}
}
