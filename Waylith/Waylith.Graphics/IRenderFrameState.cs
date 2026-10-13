using Brutal.VulkanApi;
using Brutal.VulkanApi.Abstractions;

namespace Waylith.Graphics;

public interface IRenderFrameState
{
	DeviceEx Device { get; }

	int FrameIndex { get; }

	int SubmissionFrameIndex { get; }

	int ImageCount { get; }

	int FramesInFlight { get; }

	FrameResources[] FrameResources { get; }

	Device GetDevice();

	VkCommandPool GetCommandPool();
}
