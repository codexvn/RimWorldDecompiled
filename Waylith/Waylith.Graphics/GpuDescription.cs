using Brutal.VulkanApi;

namespace Waylith.Graphics;

public sealed class GpuDescription
{
	public required int Index { get; init; }

	public required string Name { get; init; }

	public required string Driver { get; init; }

	public required int ApiVersion { get; init; }

	public required VkPhysicalDeviceType DeviceType { get; init; }

	public required long VramBytes { get; init; }

	public required string Uuid { get; init; }

	public required string? RejectionReason { get; init; }

	public bool Usable => RejectionReason == null;

	public bool IsActive { get; internal set; }

	public bool IsAutomaticChoice { get; internal set; }
}
