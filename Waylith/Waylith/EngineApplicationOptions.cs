using Brutal.Numerics;
using Brutal.VulkanApi;
using Waylith.Audio;
using Waylith.CLI;

namespace Waylith;

public sealed class EngineApplicationOptions
{
	public string Title { get; init; } = "Waylith";

	public string WaylandAppId { get; init; } = "waylith";

	public int2 InitialWindowSize { get; set; } = new int2(1920, 1080);

	public int2? InitialWindowPosition { get; set; }

	public VkFormat ColorFormat { get; init; } = VkFormat.B8G8R8A8UNorm;

	public VkFormat DepthFormat { get; init; } = VkFormat.D32SFloatS8UInt;

	public VkPresentModeKHR PreferredPresentMode { get; init; } = VkPresentModeKHR.MailboxKHR;

	public VkSampleCountFlags SampleCount { get; init; } = VkSampleCountFlags._1Bit;

	public int FramesInFlight { get; init; } = 2;

	public bool Headless { get; init; }

	public bool ShowCrashDialog { get; init; } = true;

	public string? CrashReportInstructions { get; init; }

	public bool HideWindow { get; set; }

	public long VramWarnCapBytes { get; init; }

	public bool UseKosmicKrisp { get; set; }

	public string? PreferredGpuName { get; init; }

	public string? PreferredGpuUuid { get; init; }

	public bool EnableVideoDecode { get; init; }

	public bool DisallowFullscreenExclusive { get; init; }

	public AudioBackend? AudioBackend { get; init; }

	public ICommandLine CommandLineAccessor { get; init; } = new EnvironmentCommandLine();
}
