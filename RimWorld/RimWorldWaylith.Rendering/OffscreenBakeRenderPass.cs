using Brutal.VulkanApi;
using Brutal.VulkanApi.Abstractions;

namespace RimWorldWaylith.Rendering;

internal static class OffscreenBakeRenderPass
{
	public unsafe static VkRenderPass Create(DeviceEx device, VkFormat colorFormat = VkFormat.R8G8B8A8UNorm, VkAttachmentLoadOp colorLoadOp = VkAttachmentLoadOp.Load)
	{
		VkAttachmentDescription* ptr = stackalloc VkAttachmentDescription[2];
		*ptr = new VkAttachmentDescription
		{
			Format = colorFormat,
			Samples = VkSampleCountFlags._1Bit,
			LoadOp = colorLoadOp,
			StoreOp = VkAttachmentStoreOp.Store,
			StencilLoadOp = VkAttachmentLoadOp.DontCare,
			StencilStoreOp = VkAttachmentStoreOp.DontCare,
			InitialLayout = VkImageLayout.ShaderReadOnlyOptimal,
			FinalLayout = VkImageLayout.ShaderReadOnlyOptimal
		};
		ptr[1] = new VkAttachmentDescription
		{
			Format = VkFormat.D32SFloatS8UInt,
			Samples = VkSampleCountFlags._1Bit,
			LoadOp = VkAttachmentLoadOp.Clear,
			StoreOp = VkAttachmentStoreOp.DontCare,
			StencilLoadOp = VkAttachmentLoadOp.Clear,
			StencilStoreOp = VkAttachmentStoreOp.DontCare,
			InitialLayout = VkImageLayout.Undefined,
			FinalLayout = VkImageLayout.DepthStencilAttachmentOptimal
		};
		VkAttachmentReference vkAttachmentReference = new VkAttachmentReference();
		vkAttachmentReference.Attachment = 0;
		vkAttachmentReference.Layout = VkImageLayout.ColorAttachmentOptimal;
		VkAttachmentReference vkAttachmentReference2 = vkAttachmentReference;
		vkAttachmentReference = new VkAttachmentReference();
		vkAttachmentReference.Attachment = 1;
		vkAttachmentReference.Layout = VkImageLayout.DepthStencilAttachmentOptimal;
		VkAttachmentReference vkAttachmentReference3 = vkAttachmentReference;
		VkSubpassDescription vkSubpassDescription = new VkSubpassDescription();
		vkSubpassDescription.PipelineBindPoint = VkPipelineBindPoint.Graphics;
		vkSubpassDescription.ColorAttachmentCount = 1;
		vkSubpassDescription.ColorAttachments = &vkAttachmentReference2;
		vkSubpassDescription.DepthStencilAttachment = &vkAttachmentReference3;
		VkSubpassDescription vkSubpassDescription2 = vkSubpassDescription;
		VkSubpassDependency* ptr2 = stackalloc VkSubpassDependency[2];
		*ptr2 = new VkSubpassDependency
		{
			SrcSubpass = -1,
			DstSubpass = 0,
			SrcStageMask = VkPipelineStageFlags.FragmentShaderBit,
			DstStageMask = (VkPipelineStageFlags.EarlyFragmentTestsBit | VkPipelineStageFlags.LateFragmentTestsBit | VkPipelineStageFlags.ColorAttachmentOutputBit),
			SrcAccessMask = VkAccessFlags.ShaderReadBit,
			DstAccessMask = (VkAccessFlags.ColorAttachmentReadBit | VkAccessFlags.ColorAttachmentWriteBit | VkAccessFlags.DepthStencilAttachmentReadBit | VkAccessFlags.DepthStencilAttachmentWriteBit)
		};
		ptr2[1] = new VkSubpassDependency
		{
			SrcSubpass = 0,
			DstSubpass = -1,
			SrcStageMask = (VkPipelineStageFlags.EarlyFragmentTestsBit | VkPipelineStageFlags.LateFragmentTestsBit | VkPipelineStageFlags.ColorAttachmentOutputBit),
			DstStageMask = VkPipelineStageFlags.FragmentShaderBit,
			SrcAccessMask = (VkAccessFlags.ColorAttachmentReadBit | VkAccessFlags.ColorAttachmentWriteBit | VkAccessFlags.DepthStencilAttachmentReadBit | VkAccessFlags.DepthStencilAttachmentWriteBit),
			DstAccessMask = VkAccessFlags.ShaderReadBit
		};
		VkRenderPassCreateInfo pCreateInfo = new VkRenderPassCreateInfo
		{
			AttachmentCount = 2,
			Attachments = ptr,
			SubpassCount = 1,
			Subpasses = &vkSubpassDescription2,
			DependencyCount = 2,
			Dependencies = ptr2
		};
		return device.CreateRenderPass(in pCreateInfo, device.VkAllocator);
	}
}
