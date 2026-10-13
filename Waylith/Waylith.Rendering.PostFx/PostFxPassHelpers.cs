using System;
using Brutal.VulkanApi;
using Brutal.VulkanApi.Abstractions;

namespace Waylith.Rendering.PostFx;

public static class PostFxPassHelpers
{
	internal const string FullscreenVertexName = "Fullscreen.vert";

	internal const string FullscreenVertexSource = "#version 450\nlayout(location = 0) out vec2 vUv;\nvoid main() {\n    vUv = vec2((gl_VertexIndex << 1) & 2, gl_VertexIndex & 2);\n    gl_Position = vec4(vUv * 2.0 - 1.0, 0.0, 1.0);\n}\n";

	public static VkShaderModule Compile(DeviceEx device, string source, RuntimeShaderStage stage, string name)
	{
		return RuntimeShaderCompiler.CompileModule(device, source, name, "main", RuntimeShaderLanguage.Glsl, stage);
	}

	public static VkShaderModule CompileFullscreenVertex(DeviceEx device)
	{
		return Compile(device, "#version 450\nlayout(location = 0) out vec2 vUv;\nvoid main() {\n    vUv = vec2((gl_VertexIndex << 1) & 2, gl_VertexIndex & 2);\n    gl_Position = vec4(vUv * 2.0 - 1.0, 0.0, 1.0);\n}\n", RuntimeShaderStage.Vertex, "Fullscreen.vert");
	}

	public static VkShaderModule CompileFragment(DeviceEx device, string path)
	{
		return Compile(device, ShaderSourceLoader.ReadShaderSource(path), RuntimeShaderStage.Fragment, path);
	}

	public unsafe static VkRenderPass CreateColorRenderPass(DeviceEx device, VkFormat colorFormat, VkFormat depthFormat = VkFormat.Undefined, VkAttachmentLoadOp loadOp = VkAttachmentLoadOp.DontCare)
	{
		bool flag = depthFormat != VkFormat.Undefined;
		bool flag2 = loadOp == VkAttachmentLoadOp.Load;
		VkAttachmentDescription* ptr = stackalloc VkAttachmentDescription[2];
		*ptr = new VkAttachmentDescription
		{
			Format = colorFormat,
			Samples = VkSampleCountFlags._1Bit,
			LoadOp = loadOp,
			StoreOp = VkAttachmentStoreOp.Store,
			StencilLoadOp = VkAttachmentLoadOp.DontCare,
			StencilStoreOp = VkAttachmentStoreOp.DontCare,
			InitialLayout = (flag2 ? VkImageLayout.ShaderReadOnlyOptimal : VkImageLayout.Undefined),
			FinalLayout = VkImageLayout.ShaderReadOnlyOptimal
		};
		ptr[1] = new VkAttachmentDescription
		{
			Format = depthFormat,
			Samples = VkSampleCountFlags._1Bit,
			LoadOp = VkAttachmentLoadOp.Clear,
			StoreOp = VkAttachmentStoreOp.Store,
			StencilLoadOp = VkAttachmentLoadOp.Clear,
			StencilStoreOp = VkAttachmentStoreOp.Store,
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
		vkSubpassDescription.DepthStencilAttachment = (flag ? (&vkAttachmentReference3) : null);
		VkSubpassDescription vkSubpassDescription2 = vkSubpassDescription;
		VkPipelineStageFlags dstStageMask = (flag ? (VkPipelineStageFlags.EarlyFragmentTestsBit | VkPipelineStageFlags.LateFragmentTestsBit | VkPipelineStageFlags.ColorAttachmentOutputBit) : VkPipelineStageFlags.ColorAttachmentOutputBit);
		VkAccessFlags vkAccessFlags = (flag ? (VkAccessFlags.ColorAttachmentWriteBit | VkAccessFlags.DepthStencilAttachmentWriteBit) : VkAccessFlags.ColorAttachmentWriteBit);
		VkPipelineStageFlags vkPipelineStageFlags = VkPipelineStageFlags.FragmentShaderBit;
		VkAccessFlags vkAccessFlags2 = VkAccessFlags.ShaderReadBit;
		if (flag)
		{
			vkPipelineStageFlags |= VkPipelineStageFlags.EarlyFragmentTestsBit | VkPipelineStageFlags.LateFragmentTestsBit;
			vkAccessFlags2 |= VkAccessFlags.DepthStencilAttachmentReadBit | VkAccessFlags.DepthStencilAttachmentWriteBit;
		}
		if (flag2)
		{
			vkPipelineStageFlags |= VkPipelineStageFlags.ColorAttachmentOutputBit;
			vkAccessFlags2 |= VkAccessFlags.ColorAttachmentWriteBit;
			vkAccessFlags |= VkAccessFlags.ColorAttachmentReadBit;
		}
		VkSubpassDependency* ptr2 = stackalloc VkSubpassDependency[2];
		*ptr2 = new VkSubpassDependency
		{
			SrcSubpass = -1,
			DstSubpass = 0,
			SrcStageMask = vkPipelineStageFlags,
			DstStageMask = dstStageMask,
			SrcAccessMask = vkAccessFlags2,
			DstAccessMask = vkAccessFlags
		};
		ptr2[1] = new VkSubpassDependency
		{
			SrcSubpass = 0,
			DstSubpass = -1,
			SrcStageMask = VkPipelineStageFlags.ColorAttachmentOutputBit,
			DstStageMask = VkPipelineStageFlags.FragmentShaderBit,
			SrcAccessMask = VkAccessFlags.ColorAttachmentWriteBit,
			DstAccessMask = VkAccessFlags.ShaderReadBit
		};
		return device.CreateRenderPass(new VkRenderPassCreateInfo
		{
			AttachmentCount = ((!flag) ? 1 : 2),
			Attachments = ptr,
			SubpassCount = 1,
			Subpasses = &vkSubpassDescription2,
			DependencyCount = 2,
			Dependencies = ptr2
		}, device.VkAllocator);
	}

	public unsafe static VkRenderPass CreateMrtColorRenderPass(DeviceEx device, ReadOnlySpan<VkFormat> colorFormats, VkFormat depthFormat, bool loadFirstColor = false)
	{
		int length = colorFormats.Length;
		VkAttachmentDescription* ptr = stackalloc VkAttachmentDescription[length + 1];
		VkAttachmentReference* ptr2 = stackalloc VkAttachmentReference[length];
		for (int i = 0; i < length; i++)
		{
			bool flag = loadFirstColor && i == 0;
			ptr[i] = new VkAttachmentDescription
			{
				Format = colorFormats[i],
				Samples = VkSampleCountFlags._1Bit,
				LoadOp = ((!flag) ? VkAttachmentLoadOp.Clear : VkAttachmentLoadOp.Load),
				StoreOp = VkAttachmentStoreOp.Store,
				StencilLoadOp = VkAttachmentLoadOp.DontCare,
				StencilStoreOp = VkAttachmentStoreOp.DontCare,
				InitialLayout = (flag ? VkImageLayout.ShaderReadOnlyOptimal : VkImageLayout.Undefined),
				FinalLayout = VkImageLayout.ShaderReadOnlyOptimal
			};
			ptr2[i] = new VkAttachmentReference
			{
				Attachment = i,
				Layout = VkImageLayout.ColorAttachmentOptimal
			};
		}
		ptr[length] = new VkAttachmentDescription
		{
			Format = depthFormat,
			Samples = VkSampleCountFlags._1Bit,
			LoadOp = VkAttachmentLoadOp.Clear,
			StoreOp = VkAttachmentStoreOp.Store,
			StencilLoadOp = VkAttachmentLoadOp.Clear,
			StencilStoreOp = VkAttachmentStoreOp.Store,
			InitialLayout = VkImageLayout.Undefined,
			FinalLayout = VkImageLayout.DepthStencilAttachmentOptimal
		};
		VkAttachmentReference vkAttachmentReference = new VkAttachmentReference();
		vkAttachmentReference.Attachment = length;
		vkAttachmentReference.Layout = VkImageLayout.DepthStencilAttachmentOptimal;
		VkAttachmentReference vkAttachmentReference2 = vkAttachmentReference;
		VkSubpassDescription vkSubpassDescription = new VkSubpassDescription();
		vkSubpassDescription.PipelineBindPoint = VkPipelineBindPoint.Graphics;
		vkSubpassDescription.ColorAttachmentCount = length;
		vkSubpassDescription.ColorAttachments = ptr2;
		vkSubpassDescription.DepthStencilAttachment = &vkAttachmentReference2;
		VkSubpassDescription vkSubpassDescription2 = vkSubpassDescription;
		VkSubpassDependency* ptr3 = stackalloc VkSubpassDependency[2];
		*ptr3 = new VkSubpassDependency
		{
			SrcSubpass = -1,
			DstSubpass = 0,
			SrcStageMask = (VkPipelineStageFlags)(0x380 | (loadFirstColor ? 1024 : 0)),
			DstStageMask = (VkPipelineStageFlags.EarlyFragmentTestsBit | VkPipelineStageFlags.LateFragmentTestsBit | VkPipelineStageFlags.ColorAttachmentOutputBit),
			SrcAccessMask = (VkAccessFlags)(0x620 | (loadFirstColor ? 256 : 0)),
			DstAccessMask = (VkAccessFlags)(0x500 | (loadFirstColor ? 128 : 0))
		};
		ptr3[1] = new VkSubpassDependency
		{
			SrcSubpass = 0,
			DstSubpass = -1,
			SrcStageMask = (VkPipelineStageFlags.EarlyFragmentTestsBit | VkPipelineStageFlags.LateFragmentTestsBit | VkPipelineStageFlags.ColorAttachmentOutputBit),
			DstStageMask = (VkPipelineStageFlags.FragmentShaderBit | VkPipelineStageFlags.EarlyFragmentTestsBit),
			SrcAccessMask = (VkAccessFlags.ColorAttachmentWriteBit | VkAccessFlags.DepthStencilAttachmentWriteBit),
			DstAccessMask = (VkAccessFlags.ShaderReadBit | VkAccessFlags.DepthStencilAttachmentReadBit)
		};
		return device.CreateRenderPass(new VkRenderPassCreateInfo
		{
			AttachmentCount = length + 1,
			Attachments = ptr,
			SubpassCount = 1,
			Subpasses = &vkSubpassDescription2,
			DependencyCount = 2,
			Dependencies = ptr3
		}, device.VkAllocator);
	}

	public unsafe static VkRenderPass CreateLoadColorDepthRenderPass(DeviceEx device, VkFormat colorFormat, VkFormat depthFormat)
	{
		VkAttachmentDescription* ptr = stackalloc VkAttachmentDescription[2];
		*ptr = new VkAttachmentDescription
		{
			Format = colorFormat,
			Samples = VkSampleCountFlags._1Bit,
			LoadOp = VkAttachmentLoadOp.Load,
			StoreOp = VkAttachmentStoreOp.Store,
			StencilLoadOp = VkAttachmentLoadOp.DontCare,
			StencilStoreOp = VkAttachmentStoreOp.DontCare,
			InitialLayout = VkImageLayout.ShaderReadOnlyOptimal,
			FinalLayout = VkImageLayout.ShaderReadOnlyOptimal
		};
		ptr[1] = new VkAttachmentDescription
		{
			Format = depthFormat,
			Samples = VkSampleCountFlags._1Bit,
			LoadOp = VkAttachmentLoadOp.Load,
			StoreOp = VkAttachmentStoreOp.Store,
			StencilLoadOp = VkAttachmentLoadOp.Load,
			StencilStoreOp = VkAttachmentStoreOp.Store,
			InitialLayout = VkImageLayout.DepthStencilAttachmentOptimal,
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
			SrcStageMask = (VkPipelineStageFlags.FragmentShaderBit | VkPipelineStageFlags.EarlyFragmentTestsBit | VkPipelineStageFlags.LateFragmentTestsBit | VkPipelineStageFlags.ColorAttachmentOutputBit),
			DstStageMask = (VkPipelineStageFlags.EarlyFragmentTestsBit | VkPipelineStageFlags.LateFragmentTestsBit | VkPipelineStageFlags.ColorAttachmentOutputBit),
			SrcAccessMask = (VkAccessFlags.ShaderReadBit | VkAccessFlags.ColorAttachmentWriteBit | VkAccessFlags.DepthStencilAttachmentWriteBit),
			DstAccessMask = (VkAccessFlags.ColorAttachmentReadBit | VkAccessFlags.ColorAttachmentWriteBit | VkAccessFlags.DepthStencilAttachmentReadBit | VkAccessFlags.DepthStencilAttachmentWriteBit)
		};
		ptr2[1] = new VkSubpassDependency
		{
			SrcSubpass = 0,
			DstSubpass = -1,
			SrcStageMask = VkPipelineStageFlags.ColorAttachmentOutputBit,
			DstStageMask = VkPipelineStageFlags.FragmentShaderBit,
			SrcAccessMask = VkAccessFlags.ColorAttachmentWriteBit,
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

	public unsafe static VkPipeline CreateFullscreenPipeline(DeviceEx device, VkRenderPass renderPass, VkSampleCountFlags sampleCount, VkShaderModule vertex, VkShaderModule fragment, VkPipelineLayout layout, BlendState blend, string debugName)
	{
		byte* ptr = stackalloc byte[5];
		*ptr = 109;
		ptr[1] = 97;
		ptr[2] = 105;
		ptr[3] = 110;
		ptr[4] = 0;
		VkPipelineShaderStageCreateInfo* ptr2 = stackalloc VkPipelineShaderStageCreateInfo[2];
		*ptr2 = new VkPipelineShaderStageCreateInfo
		{
			Stage = VkShaderStageFlags.VertexBit,
			Module = vertex,
			Name = ptr
		};
		ptr2[1] = new VkPipelineShaderStageCreateInfo
		{
			Stage = VkShaderStageFlags.FragmentBit,
			Module = fragment,
			Name = ptr
		};
		VkPipelineVertexInputStateCreateInfo vkPipelineVertexInputStateCreateInfo = new VkPipelineVertexInputStateCreateInfo();
		VkPipelineInputAssemblyStateCreateInfo vkPipelineInputAssemblyStateCreateInfo = new VkPipelineInputAssemblyStateCreateInfo();
		vkPipelineInputAssemblyStateCreateInfo.Topology = VkPrimitiveTopology.TriangleList;
		VkPipelineInputAssemblyStateCreateInfo vkPipelineInputAssemblyStateCreateInfo2 = vkPipelineInputAssemblyStateCreateInfo;
		VkPipelineViewportStateCreateInfo vkPipelineViewportStateCreateInfo = new VkPipelineViewportStateCreateInfo();
		vkPipelineViewportStateCreateInfo.ViewportCount = 1;
		vkPipelineViewportStateCreateInfo.ScissorCount = 1;
		VkPipelineViewportStateCreateInfo vkPipelineViewportStateCreateInfo2 = vkPipelineViewportStateCreateInfo;
		VkPipelineRasterizationStateCreateInfo vkPipelineRasterizationStateCreateInfo = new VkPipelineRasterizationStateCreateInfo();
		vkPipelineRasterizationStateCreateInfo.PolygonMode = VkPolygonMode.Fill;
		vkPipelineRasterizationStateCreateInfo.CullMode = VkCullModeFlags.None;
		vkPipelineRasterizationStateCreateInfo.FrontFace = VkFrontFace.CounterClockwise;
		vkPipelineRasterizationStateCreateInfo.LineWidth = 1f;
		VkPipelineRasterizationStateCreateInfo vkPipelineRasterizationStateCreateInfo2 = vkPipelineRasterizationStateCreateInfo;
		VkPipelineMultisampleStateCreateInfo vkPipelineMultisampleStateCreateInfo = new VkPipelineMultisampleStateCreateInfo();
		vkPipelineMultisampleStateCreateInfo.RasterizationSamples = sampleCount;
		VkPipelineMultisampleStateCreateInfo vkPipelineMultisampleStateCreateInfo2 = vkPipelineMultisampleStateCreateInfo;
		VkPipelineDepthStencilStateCreateInfo vkPipelineDepthStencilStateCreateInfo = new VkPipelineDepthStencilStateCreateInfo();
		vkPipelineDepthStencilStateCreateInfo.DepthTestEnable = false;
		vkPipelineDepthStencilStateCreateInfo.DepthWriteEnable = false;
		vkPipelineDepthStencilStateCreateInfo.StencilTestEnable = false;
		VkPipelineDepthStencilStateCreateInfo vkPipelineDepthStencilStateCreateInfo2 = vkPipelineDepthStencilStateCreateInfo;
		VkPipelineColorBlendAttachmentState vkPipelineColorBlendAttachmentState = new VkPipelineColorBlendAttachmentState();
		vkPipelineColorBlendAttachmentState.BlendEnable = blend.Enabled;
		vkPipelineColorBlendAttachmentState.SrcColorBlendFactor = ToVkBlendFactor(blend.SrcColor);
		vkPipelineColorBlendAttachmentState.DstColorBlendFactor = ToVkBlendFactor(blend.DstColor);
		vkPipelineColorBlendAttachmentState.ColorBlendOp = VkBlendOp.Add;
		vkPipelineColorBlendAttachmentState.SrcAlphaBlendFactor = ToVkBlendFactor(blend.SrcAlpha);
		vkPipelineColorBlendAttachmentState.DstAlphaBlendFactor = ToVkBlendFactor(blend.DstAlpha);
		vkPipelineColorBlendAttachmentState.AlphaBlendOp = VkBlendOp.Add;
		vkPipelineColorBlendAttachmentState.ColorWriteMask = VkColorComponentFlags.RBit | VkColorComponentFlags.GBit | VkColorComponentFlags.BBit | VkColorComponentFlags.ABit;
		VkPipelineColorBlendAttachmentState vkPipelineColorBlendAttachmentState2 = vkPipelineColorBlendAttachmentState;
		VkPipelineColorBlendStateCreateInfo vkPipelineColorBlendStateCreateInfo = new VkPipelineColorBlendStateCreateInfo();
		vkPipelineColorBlendStateCreateInfo.AttachmentCount = 1;
		vkPipelineColorBlendStateCreateInfo.Attachments = &vkPipelineColorBlendAttachmentState2;
		VkPipelineColorBlendStateCreateInfo vkPipelineColorBlendStateCreateInfo2 = vkPipelineColorBlendStateCreateInfo;
		VkDynamicState* ptr3 = stackalloc VkDynamicState[2];
		*ptr3 = VkDynamicState.Viewport;
		ptr3[1] = VkDynamicState.Scissor;
		VkPipelineDynamicStateCreateInfo vkPipelineDynamicStateCreateInfo = new VkPipelineDynamicStateCreateInfo();
		vkPipelineDynamicStateCreateInfo.DynamicStateCount = 2;
		vkPipelineDynamicStateCreateInfo.DynamicStates = ptr3;
		VkPipelineDynamicStateCreateInfo vkPipelineDynamicStateCreateInfo2 = vkPipelineDynamicStateCreateInfo;
		VkGraphicsPipelineCreateInfo vkGraphicsPipelineCreateInfo = new VkGraphicsPipelineCreateInfo();
		vkGraphicsPipelineCreateInfo.StageCount = 2;
		vkGraphicsPipelineCreateInfo.Stages = ptr2;
		vkGraphicsPipelineCreateInfo.VertexInputState = &vkPipelineVertexInputStateCreateInfo;
		vkGraphicsPipelineCreateInfo.InputAssemblyState = &vkPipelineInputAssemblyStateCreateInfo2;
		vkGraphicsPipelineCreateInfo.ViewportState = &vkPipelineViewportStateCreateInfo2;
		vkGraphicsPipelineCreateInfo.RasterizationState = &vkPipelineRasterizationStateCreateInfo2;
		vkGraphicsPipelineCreateInfo.MultisampleState = &vkPipelineMultisampleStateCreateInfo2;
		vkGraphicsPipelineCreateInfo.DepthStencilState = &vkPipelineDepthStencilStateCreateInfo2;
		vkGraphicsPipelineCreateInfo.ColorBlendState = &vkPipelineColorBlendStateCreateInfo2;
		vkGraphicsPipelineCreateInfo.DynamicState = &vkPipelineDynamicStateCreateInfo2;
		vkGraphicsPipelineCreateInfo.Layout = layout;
		vkGraphicsPipelineCreateInfo.RenderPass = renderPass;
		vkGraphicsPipelineCreateInfo.Subpass = 0;
		VkGraphicsPipelineCreateInfo vkGraphicsPipelineCreateInfo2 = vkGraphicsPipelineCreateInfo;
		Span<VkPipeline> span = stackalloc VkPipeline[1];
		VkGraphicsPipelineCreateInfo reference = vkGraphicsPipelineCreateInfo2;
		VkResult vkResult = device.CreateGraphicsPipelines(default(VkPipelineCache), new ReadOnlySpan<VkGraphicsPipelineCreateInfo>(in reference), device.VkAllocator, span);
		if (vkResult != 0)
		{
			throw new InvalidOperationException($"Failed to create fullscreen pipeline '{debugName}': {vkResult}");
		}
		return span[0];
	}

	private static VkBlendFactor ToVkBlendFactor(BlendFactor factor)
	{
		return factor switch
		{
			BlendFactor.Zero => VkBlendFactor.Zero, 
			BlendFactor.One => VkBlendFactor.One, 
			BlendFactor.SrcAlpha => VkBlendFactor.SrcAlpha, 
			BlendFactor.OneMinusSrcAlpha => VkBlendFactor.OneMinusSrcAlpha, 
			BlendFactor.SrcColor => VkBlendFactor.SrcColor, 
			BlendFactor.DstColor => VkBlendFactor.DstColor, 
			BlendFactor.OneMinusSrcColor => VkBlendFactor.OneMinusSrcColor, 
			BlendFactor.OneMinusDstColor => VkBlendFactor.OneMinusDstColor, 
			_ => VkBlendFactor.One, 
		};
	}
}
