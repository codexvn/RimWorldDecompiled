using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Brutal;
using Brutal.Numerics;
using Brutal.VulkanApi;
using Brutal.VulkanApi.Abstractions;
using Waylith.Graphics;
using Waylith.Rendering.PostFx;

namespace Waylith.Rendering;

public sealed class RenderTextureDrawFeature : IRenderFeature, IRenderPass, IDisposable
{
	private struct BlitPush
	{
		public float4 SrcUvRect;

		public float4 Tint;
	}

	private sealed class SubmittedBatch(IRenderTextureTarget target, float4 clearColor, RenderTextureBatch.BlitDraw[] draws, RenderTextureBatch.CopyOp[] copies, FinalDrawItem[] meshItems, CameraView view, RenderTextureBarrier finalState)
	{
		public IRenderTextureTarget Target { get; } = target;

		public float4 ClearColor { get; } = clearColor;

		public RenderTextureBatch.BlitDraw[] Draws { get; } = draws;

		public RenderTextureBatch.CopyOp[] Copies { get; } = copies;

		public FinalDrawItem[] MeshItems { get; } = meshItems;

		public CameraView View { get; } = view;

		public RenderTextureBarrier FinalState { get; } = finalState;
	}

	private const VkFormat TargetFormat = VkFormat.R8G8B8A8UNorm;

	internal const string BlitFragmentName = "RenderTextureBlit.frag";

	internal const string BlitFragmentSource = "#version 450\nlayout(location = 0) in vec2 vUv;\nlayout(location = 0) out vec4 outColor;\nlayout(set = 0, binding = 0) uniform sampler2D uSource;\nlayout(push_constant) uniform Push { vec4 srcUvRect; vec4 tint; } pc;\nvoid main() {\n    vec2 uv = pc.srcUvRect.xy + vUv * pc.srcUvRect.zw;\n    outColor = texture(uSource, uv) * pc.tint;\n}\n";

	private readonly GraphicsDevice graphicsDevice;

	private readonly DeviceEx device;

	private readonly IRenderPass[] passes;

	private IRenderContext renderContext;

	private VkRenderPass renderPass;

	private VkShaderModule vertexModule;

	private VkShaderModule fragmentModule;

	private VkDescriptorSetLayout descriptorSetLayout;

	private VkPipelineLayout pipelineLayout;

	private VkPipeline pipeline;

	private VkSampler defaultSampler;

	private const VkFormat DepthFormat = VkFormat.D32SFloat;

	private MeshBatchRenderer? meshRenderer;

	private VkRenderPass meshRenderPass;

	private int meshFrameNumber;

	private readonly VkCommandPool immediateCommandPool;

	private readonly object immediateSync = new object();

	private const int FramePoolCapacity = 1024;

	private readonly VkDescriptorPool[] framePools;

	private readonly ConcurrentQueue<SubmittedBatch> pending = new ConcurrentQueue<SubmittedBatch>();

	private bool isActive;

	private int disposed;

	public bool IsActive => isActive;

	public IReadOnlyList<IRenderPass> Passes => passes;

	public byte PassId => 0;

	public PassAttachment? Attachment => null;

	public bool PreBackbuffer => true;

	public RenderTextureDrawFeature(GraphicsDevice graphicsDevice, IRenderContext renderContext)
	{
		this.graphicsDevice = graphicsDevice;
		this.renderContext = renderContext;
		device = graphicsDevice.Device;
		passes = new IRenderPass[1] { this };
		renderPass = PostFxPassHelpers.CreateColorRenderPass(device, VkFormat.R8G8B8A8UNorm, VkFormat.Undefined, VkAttachmentLoadOp.Clear);
		BuildPipeline();
		DeviceEx handle = device;
		VkCommandPoolCreateInfo pCreateInfo = new VkCommandPoolCreateInfo
		{
			QueueFamilyIndex = graphicsDevice.GraphicsQueue.Family,
			Flags = VkCommandPoolCreateFlags.ResetCommandBufferBit
		};
		immediateCommandPool = handle.CreateCommandPool(in pCreateInfo, device.VkAllocator);
		int num = System.Math.Max(1, renderContext.FramesInFlight);
		framePools = new VkDescriptorPool[num];
		for (int i = 0; i < num; i++)
		{
			framePools[i] = CreateDescriptorPool(1024);
		}
	}

	public RenderTextureBatch CreateBatch(IRenderTextureTarget target)
	{
		return new RenderTextureBatch(this, target);
	}

	internal void Submit(RenderTextureBatch batch)
	{
		ObjectDisposedException.ThrowIf(Volatile.Read(in disposed) != 0, this);
		pending.Enqueue(new SubmittedBatch(batch.Target, batch.ClearColor, batch.Draws.ToArray(), batch.Copies.ToArray(), ResolveMeshItems(batch.MeshDraws), batch.View, batch.FinalState));
	}

	public object? CaptureFrame(in FrameTiming timing)
	{
		List<SubmittedBatch> list = null;
		SubmittedBatch result;
		while (pending.TryDequeue(out result))
		{
			(list ?? (list = new List<SubmittedBatch>())).Add(result);
		}
		isActive = list != null;
		return list;
	}

	public void Record(in FrameContext frameContext, object? featurePayload)
	{
		if (!(featurePayload is List<SubmittedBatch> { Count: not 0 } list))
		{
			return;
		}
		VkDescriptorPool descriptorPool = framePools[frameContext.SubmissionFrameIndex % framePools.Length];
		device.ResetDescriptorPool(descriptorPool, 0);
		CommandBuffer commandBuffer = frameContext.CommandBuffer;
		bool flag = false;
		foreach (SubmittedBatch item in list)
		{
			if (item.MeshItems.Length != 0)
			{
				flag = true;
				break;
			}
		}
		if (flag)
		{
			MeshBatchRenderer meshBatchRenderer = EnsureMeshRenderer();
			FrameContext frameContext2 = new FrameContext(frameContext.RenderContext, frameContext.Resources, commandBuffer, meshRenderPass, default(VkFramebuffer), default(VkExtent2D), VkSampleCountFlags._1Bit, VkFormat.R8G8B8A8UNorm, VkFormat.D32SFloat);
			meshBatchRenderer.BeginFrame(in frameContext2, ++meshFrameNumber);
		}
		foreach (SubmittedBatch item2 in list)
		{
			if (item2.Draws.Length != 0 || item2.Copies.Length != 0)
			{
				RecordBatch(commandBuffer, item2.Target, item2.ClearColor, item2.Draws, item2.Copies, item2.FinalState, descriptorPool, deferred: true);
			}
			if (item2.MeshItems.Length != 0)
			{
				RecordMesh(in frameContext, item2.Target, item2.ClearColor, item2.MeshItems, item2.View, item2.FinalState);
				FinalDrawItem[] meshItems = item2.MeshItems;
				for (int i = 0; i < meshItems.Length; i++)
				{
					FinalDrawItem.Return(meshItems[i]);
				}
			}
		}
	}

	internal void ExecuteNow(RenderTextureBatch batch)
	{
		ObjectDisposedException.ThrowIf(Volatile.Read(in disposed) != 0, this);
		if (batch.MeshDraws.Count > 0)
		{
			throw new InvalidOperationException("DrawMesh/DrawTexture need Submit() (deferred); the immediate path can't drive MeshBatchRenderer.");
		}
		IReadOnlyList<RenderTextureBatch.BlitDraw> draws = batch.Draws;
		IReadOnlyList<RenderTextureBatch.CopyOp> copies = batch.Copies;
		lock (immediateSync)
		{
			VkDescriptorPool descriptorPool = CreateDescriptorPool(System.Math.Max(1, draws.Count));
			CommandBuffer commandBuffer = device.AllocateCommandBuffer(new VkCommandBufferAllocateInfo
			{
				CommandPool = immediateCommandPool,
				Level = VkCommandBufferLevel.Primary
			});
			VkFence vkFence = device.CreateFence(new VkFenceCreateInfo(), device.VkAllocator);
			try
			{
				commandBuffer.Begin(VkCommandBufferUsageFlags.OneTimeSubmitBit);
				RecordBatch(commandBuffer, batch.Target, batch.ClearColor, draws, copies, batch.FinalState, descriptorPool, deferred: false);
				commandBuffer.End();
				CommandBuffer reference = commandBuffer;
				Span<CommandBuffer> commandBuffers = new Span<CommandBuffer>(ref reference);
				graphicsDevice.SubmitGraphics(default(Span<VkSemaphore>), default(Span<VkPipelineStageFlags>), commandBuffers, default(Span<VkSemaphore>), vkFence);
				device.WaitForFence(vkFence, -1);
			}
			finally
			{
				device.DestroyFence(vkFence, device.VkAllocator);
				if (!commandBuffer.Handle.IsNull())
				{
					DeviceEx handle = device;
					VkCommandPool commandPool = immediateCommandPool;
					CommandBuffer reference2 = commandBuffer;
					handle.FreeCommandBuffers(commandPool, new ReadOnlySpan<CommandBuffer>(in reference2));
				}
				device.DestroyDescriptorPool(descriptorPool, device.VkAllocator);
			}
		}
	}

	private unsafe void RecordBatch(CommandBuffer cmd, IRenderTextureTarget target, float4 clearColor, IReadOnlyList<RenderTextureBatch.BlitDraw> draws, IReadOnlyList<RenderTextureBatch.CopyOp> copies, RenderTextureBarrier finalState, VkDescriptorPool descriptorPool, bool deferred)
	{
		GpuTextureAllocation color = target.Color;
		VkRect2D renderArea = new VkRect2D(target.Extent);
		for (int i = 0; i < draws.Count; i++)
		{
			EnsureSampleable(cmd, draws[i].Source);
		}
		VkClearValue vkClearValue = new VkClearValue();
		vkClearValue.Color = new VkClearColorValue
		{
			Float32 = clearColor
		};
		VkClearValue vkClearValue2 = vkClearValue;
		VkRenderPassBeginInfo pRenderPassBegin = new VkRenderPassBeginInfo
		{
			RenderPass = renderPass,
			Framebuffer = target.GetFramebuffer(renderPass),
			RenderArea = renderArea,
			ClearValueCount = 1,
			ClearValues = &vkClearValue2
		};
		cmd.BeginRenderPass(in pRenderPassBegin, VkSubpassContents.Inline);
		for (int j = 0; j < draws.Count; j++)
		{
			RenderTextureBatch.BlitDraw blitDraw = draws[j];
			VkDescriptorSet vkDescriptorSet = DescriptorSetWriter.AllocateDescriptorSet(device, descriptorPool, descriptorSetLayout);
			WriteSource(vkDescriptorSet, blitDraw.Sampler.IsNull() ? defaultSampler : blitDraw.Sampler, blitDraw.Source.ImageView);
			cmd.BindPipeline(VkPipelineBindPoint.Graphics, pipeline);
			VkViewport reference = new VkViewport
			{
				X = blitDraw.DstRect.Offset.X,
				Y = blitDraw.DstRect.Offset.Y,
				Width = blitDraw.DstRect.Extent.Width,
				Height = blitDraw.DstRect.Extent.Height,
				MinDepth = 0f,
				MaxDepth = 1f
			};
			cmd.SetViewport(0, new ReadOnlySpan<VkViewport>(in reference));
			VkRect2D reference2 = blitDraw.DstRect;
			cmd.SetScissor(0, new ReadOnlySpan<VkRect2D>(in reference2));
			cmd.BindDescriptorSet(VkPipelineBindPoint.Graphics, pipelineLayout, 0, vkDescriptorSet, default(Span<ByteSize32>));
			BlitPush blitPush = default(BlitPush);
			blitPush.SrcUvRect = blitDraw.SrcUvRect;
			blitPush.Tint = blitDraw.Tint;
			BlitPush blitPush2 = blitPush;
			cmd.PushConstants(pipelineLayout, VkShaderStageFlags.FragmentBit, ByteSize.Zero, new ReadOnlySpan<byte>(&blitPush2, sizeof(BlitPush)));
			cmd.Draw(3, 1, 0, 0);
		}
		cmd.EndRenderPass();
		color.CurrentLayout = VkImageLayout.ShaderReadOnlyOptimal;
		if (deferred && Platform.IsMacOS && copies.Count == 0 && finalState.Layout == VkImageLayout.ShaderReadOnlyOptimal)
		{
			Barrier(cmd, color.Image, VkImageLayout.ShaderReadOnlyOptimal, VkImageLayout.ShaderReadOnlyOptimal, VkPipelineStageFlags.ColorAttachmentOutputBit, VkAccessFlags.ColorAttachmentWriteBit, VkPipelineStageFlags.FragmentShaderBit, VkAccessFlags.ShaderReadBit, 0, color.MipCount);
		}
		if (copies.Count > 0)
		{
			Barrier(cmd, color.Image, VkImageLayout.ShaderReadOnlyOptimal, VkImageLayout.TransferSrcOptimal, VkPipelineStageFlags.ColorAttachmentOutputBit, VkAccessFlags.ColorAttachmentWriteBit, VkPipelineStageFlags.TransferBit, VkAccessFlags.TransferReadBit, 0, color.MipCount);
			color.CurrentLayout = VkImageLayout.TransferSrcOptimal;
			for (int k = 0; k < copies.Count; k++)
			{
				RecordCopy(cmd, color, copies[k]);
			}
		}
		ApplyFinalState(cmd, color, finalState);
	}

	private unsafe void RecordMesh(in FrameContext frameContext, IRenderTextureTarget target, float4 clearColor, FinalDrawItem[] items, in CameraView view, RenderTextureBarrier finalState)
	{
		GpuTextureAllocation color = target.Color;
		VkExtent2D extent = target.Extent;
		VkRect2D vkRect2D = new VkRect2D(extent);
		CommandBuffer commandBuffer = frameContext.CommandBuffer;
		VkFramebuffer framebuffer = target.GetFramebuffer(meshRenderPass);
		FrameContext frameContext2 = new FrameContext(frameContext.RenderContext, frameContext.Resources, commandBuffer, meshRenderPass, framebuffer, extent, VkSampleCountFlags._1Bit, VkFormat.R8G8B8A8UNorm, VkFormat.D32SFloat);
		VkClearValue* ptr = stackalloc VkClearValue[2];
		*ptr = new VkClearColorValue
		{
			Float32 = clearColor
		};
		ptr[1] = new VkClearDepthStencilValue
		{
			Depth = 0f,
			Stencil = 0
		};
		VkRenderPassBeginInfo pRenderPassBegin = new VkRenderPassBeginInfo
		{
			RenderPass = meshRenderPass,
			Framebuffer = framebuffer,
			RenderArea = vkRect2D,
			ClearValueCount = 2,
			ClearValues = ptr
		};
		commandBuffer.BeginRenderPass(in pRenderPassBegin, VkSubpassContents.Inline);
		VkViewport reference = new VkViewport
		{
			X = 0f,
			Y = 0f,
			Width = extent.Width,
			Height = extent.Height,
			MinDepth = 0f,
			MaxDepth = 1f
		};
		commandBuffer.SetViewport(0, new ReadOnlySpan<VkViewport>(in reference));
		VkRect2D reference2 = vkRect2D;
		commandBuffer.SetScissor(0, new ReadOnlySpan<VkRect2D>(in reference2));
		meshRenderer.RecordBatch(in frameContext2, items, view, null);
		commandBuffer.EndRenderPass();
		color.CurrentLayout = VkImageLayout.ShaderReadOnlyOptimal;
		if (Platform.IsMacOS && finalState.Layout == VkImageLayout.ShaderReadOnlyOptimal)
		{
			Barrier(commandBuffer, color.Image, VkImageLayout.ShaderReadOnlyOptimal, VkImageLayout.ShaderReadOnlyOptimal, VkPipelineStageFlags.ColorAttachmentOutputBit, VkAccessFlags.ColorAttachmentWriteBit, VkPipelineStageFlags.FragmentShaderBit, VkAccessFlags.ShaderReadBit, 0, color.MipCount);
		}
		ApplyFinalState(commandBuffer, color, finalState);
	}

	private MeshBatchRenderer EnsureMeshRenderer()
	{
		if (meshRenderer == null)
		{
			meshRenderPass = PostFxPassHelpers.CreateColorRenderPass(device, VkFormat.R8G8B8A8UNorm, VkFormat.D32SFloat, VkAttachmentLoadOp.Clear);
			meshRenderer = new MeshBatchRenderer(graphicsDevice, renderContext);
		}
		return meshRenderer;
	}

	private static FinalDrawItem[] ResolveMeshItems(IReadOnlyList<MeshDrawCommand> commands)
	{
		if (commands.Count == 0)
		{
			return Array.Empty<FinalDrawItem>();
		}
		List<FinalDrawItem> list = new List<FinalDrawItem>(commands.Count);
		for (int i = 0; i < commands.Count; i++)
		{
			FinalDrawItem finalDrawItem = MeshDrawSubmission.TryMakeDrawItem(commands[i], GlobalShaderState.GlobalKeywordKey);
			if (finalDrawItem != null)
			{
				list.Add(finalDrawItem);
			}
		}
		return list.ToArray();
	}

	private void RecordCopy(CommandBuffer cmd, GpuTextureAllocation source, in RenderTextureBatch.CopyOp copy)
	{
		GpuTextureAllocation destination = copy.Destination;
		VkImageLayout mipLayout = destination.GetMipLayout(0);
		var (srcStage, srcAccess) = SourceMasks(mipLayout);
		Barrier(cmd, destination.Image, mipLayout, VkImageLayout.TransferDstOptimal, srcStage, srcAccess, VkPipelineStageFlags.TransferBit, VkAccessFlags.TransferWriteBit, 0, 1);
		VkImage image = source.Image;
		VkImage image2 = destination.Image;
		VkImageCopy reference = new VkImageCopy
		{
			SrcSubresource = ColorLayers(0),
			SrcOffset = new VkOffset3D(copy.SrcOffset.X, copy.SrcOffset.Y, 0),
			DstSubresource = ColorLayers(0),
			DstOffset = new VkOffset3D(copy.DstOffset.X, copy.DstOffset.Y, 0),
			Extent = new VkExtent3D(copy.Extent.Width, copy.Extent.Height, 1)
		};
		cmd.CopyImage(image, VkImageLayout.TransferSrcOptimal, image2, VkImageLayout.TransferDstOptimal, new ReadOnlySpan<VkImageCopy>(in reference));
		Barrier(cmd, destination.Image, VkImageLayout.TransferDstOptimal, VkImageLayout.ShaderReadOnlyOptimal, VkPipelineStageFlags.TransferBit, VkAccessFlags.TransferWriteBit, VkPipelineStageFlags.FragmentShaderBit, VkAccessFlags.ShaderReadBit, 0, 1);
		destination.SetMipLayout(0, VkImageLayout.ShaderReadOnlyOptimal);
	}

	private void ApplyFinalState(CommandBuffer cmd, GpuTextureAllocation color, RenderTextureBarrier finalState)
	{
		if (color.CurrentLayout != finalState.Layout)
		{
			VkPipelineStageFlags srcStage;
			VkAccessFlags srcAccess;
			if (color.CurrentLayout != VkImageLayout.ShaderReadOnlyOptimal)
			{
				(srcStage, srcAccess) = SourceMasks(color.CurrentLayout);
			}
			else
			{
				srcStage = VkPipelineStageFlags.ColorAttachmentOutputBit;
				srcAccess = VkAccessFlags.ColorAttachmentWriteBit;
			}
			Barrier(cmd, color.Image, color.CurrentLayout, finalState.Layout, srcStage, srcAccess, finalState.Stage, finalState.Access, 0, color.MipCount);
			color.CurrentLayout = finalState.Layout;
		}
	}

	private void EnsureSampleable(CommandBuffer cmd, GpuTextureAllocation texture)
	{
		if (texture.CurrentLayout != VkImageLayout.ShaderReadOnlyOptimal)
		{
			var (srcStage, srcAccess) = SourceMasks(texture.CurrentLayout);
			Barrier(cmd, texture.Image, texture.CurrentLayout, VkImageLayout.ShaderReadOnlyOptimal, srcStage, srcAccess, VkPipelineStageFlags.FragmentShaderBit, VkAccessFlags.ShaderReadBit, 0, texture.MipCount);
			texture.CurrentLayout = VkImageLayout.ShaderReadOnlyOptimal;
		}
	}

	private static void Barrier(CommandBuffer cmd, VkImage image, VkImageLayout oldLayout, VkImageLayout newLayout, VkPipelineStageFlags srcStage, VkAccessFlags srcAccess, VkPipelineStageFlags dstStage, VkAccessFlags dstAccess, int baseMip, int levelCount)
	{
		VkImageMemoryBarrier reference = new VkImageMemoryBarrier
		{
			SrcAccessMask = srcAccess,
			DstAccessMask = dstAccess,
			OldLayout = oldLayout,
			NewLayout = newLayout,
			SrcQueueFamilyIndex = -1,
			DstQueueFamilyIndex = -1,
			Image = image,
			SubresourceRange = new VkImageSubresourceRange
			{
				AspectMask = VkImageAspectFlags.ColorBit,
				BaseMipLevel = baseMip,
				LevelCount = levelCount,
				BaseArrayLayer = 0,
				LayerCount = 1
			}
		};
		cmd.PipelineBarrier(srcStage, dstStage, VkDependencyFlags.None, default(ReadOnlySpan<VkMemoryBarrier>), default(ReadOnlySpan<VkBufferMemoryBarrier>), new ReadOnlySpan<VkImageMemoryBarrier>(in reference));
	}

	private static (VkPipelineStageFlags Stage, VkAccessFlags Access) SourceMasks(VkImageLayout layout)
	{
		return layout switch
		{
			VkImageLayout.Undefined => (Stage: VkPipelineStageFlags.TopOfPipeBit, Access: VkAccessFlags.None), 
			VkImageLayout.ShaderReadOnlyOptimal => (Stage: VkPipelineStageFlags.FragmentShaderBit, Access: VkAccessFlags.ShaderReadBit), 
			VkImageLayout.ColorAttachmentOptimal => (Stage: VkPipelineStageFlags.ColorAttachmentOutputBit, Access: VkAccessFlags.ColorAttachmentWriteBit), 
			VkImageLayout.TransferSrcOptimal => (Stage: VkPipelineStageFlags.TransferBit, Access: VkAccessFlags.TransferReadBit), 
			VkImageLayout.TransferDstOptimal => (Stage: VkPipelineStageFlags.TransferBit, Access: VkAccessFlags.TransferWriteBit), 
			_ => (Stage: VkPipelineStageFlags.AllCommandsBit, Access: VkAccessFlags.MemoryReadBit | VkAccessFlags.MemoryWriteBit), 
		};
	}

	private static VkImageSubresourceLayers ColorLayers(int mip)
	{
		VkImageSubresourceLayers result = new VkImageSubresourceLayers();
		result.AspectMask = VkImageAspectFlags.ColorBit;
		result.MipLevel = mip;
		result.BaseArrayLayer = 0;
		result.LayerCount = 1;
		return result;
	}

	private unsafe void WriteSource(VkDescriptorSet set, VkSampler sampler, VkImageView view)
	{
		VkDescriptorImageInfo vkDescriptorImageInfo = new VkDescriptorImageInfo();
		vkDescriptorImageInfo.Sampler = sampler;
		vkDescriptorImageInfo.ImageView = view;
		vkDescriptorImageInfo.ImageLayout = VkImageLayout.ShaderReadOnlyOptimal;
		VkDescriptorImageInfo vkDescriptorImageInfo2 = vkDescriptorImageInfo;
		VkWriteDescriptorSet vkWriteDescriptorSet = new VkWriteDescriptorSet();
		vkWriteDescriptorSet.DstSet = set;
		vkWriteDescriptorSet.DstBinding = 0;
		vkWriteDescriptorSet.DescriptorCount = 1;
		vkWriteDescriptorSet.DescriptorType = VkDescriptorType.CombinedImageSampler;
		vkWriteDescriptorSet.ImageInfo = &vkDescriptorImageInfo2;
		VkWriteDescriptorSet vkWriteDescriptorSet2 = vkWriteDescriptorSet;
		device.UpdateDescriptorSets(new ReadOnlySpan<VkWriteDescriptorSet>(&vkWriteDescriptorSet2, 1), default(ReadOnlySpan<VkCopyDescriptorSet>));
	}

	private unsafe void BuildPipeline()
	{
		vertexModule = PostFxPassHelpers.CompileFullscreenVertex(device);
		fragmentModule = PostFxPassHelpers.Compile(device, "#version 450\nlayout(location = 0) in vec2 vUv;\nlayout(location = 0) out vec4 outColor;\nlayout(set = 0, binding = 0) uniform sampler2D uSource;\nlayout(push_constant) uniform Push { vec4 srcUvRect; vec4 tint; } pc;\nvoid main() {\n    vec2 uv = pc.srcUvRect.xy + vUv * pc.srcUvRect.zw;\n    outColor = texture(uSource, uv) * pc.tint;\n}\n", RuntimeShaderStage.Fragment, "RenderTextureBlit.frag");
		VkDescriptorSetLayoutBinding vkDescriptorSetLayoutBinding = new VkDescriptorSetLayoutBinding();
		vkDescriptorSetLayoutBinding.Binding = 0;
		vkDescriptorSetLayoutBinding.DescriptorType = VkDescriptorType.CombinedImageSampler;
		vkDescriptorSetLayoutBinding.DescriptorCount = 1;
		vkDescriptorSetLayoutBinding.StageFlags = VkShaderStageFlags.FragmentBit;
		VkDescriptorSetLayoutBinding vkDescriptorSetLayoutBinding2 = vkDescriptorSetLayoutBinding;
		DeviceEx handle = device;
		VkDescriptorSetLayoutCreateInfo pCreateInfo = new VkDescriptorSetLayoutCreateInfo
		{
			BindingCount = 1,
			Bindings = &vkDescriptorSetLayoutBinding2
		};
		descriptorSetLayout = handle.CreateDescriptorSetLayout(in pCreateInfo, device.VkAllocator);
		VkPushConstantRange vkPushConstantRange = new VkPushConstantRange();
		vkPushConstantRange.StageFlags = VkShaderStageFlags.FragmentBit;
		vkPushConstantRange.Offset = ByteSize.Zero;
		vkPushConstantRange.Size = ByteSize.Of(sizeof(BlitPush));
		VkPushConstantRange vkPushConstantRange2 = vkPushConstantRange;
		VkDescriptorSetLayout reference = descriptorSetLayout;
		DeviceEx inDevice = device;
		ReadOnlySpan<VkDescriptorSetLayout> setLayouts = new ReadOnlySpan<VkDescriptorSetLayout>(in reference);
		VkPushConstantRange reference2 = vkPushConstantRange2;
		pipelineLayout = inDevice.CreatePipelineLayout(setLayouts, new ReadOnlySpan<VkPushConstantRange>(in reference2), device.VkAllocator);
		DeviceEx handle2 = device;
		VkSamplerCreateInfo pCreateInfo2 = new VkSamplerCreateInfo
		{
			MagFilter = VkFilter.Linear,
			MinFilter = VkFilter.Linear,
			MipmapMode = VkSamplerMipmapMode.Nearest,
			AddressModeU = VkSamplerAddressMode.ClampToEdge,
			AddressModeV = VkSamplerAddressMode.ClampToEdge,
			AddressModeW = VkSamplerAddressMode.ClampToEdge,
			MinLod = 0f,
			MaxLod = 0f,
			BorderColor = VkBorderColor.FloatOpaqueBlack
		};
		defaultSampler = handle2.CreateSampler(in pCreateInfo2, device.VkAllocator);
		pipeline = PostFxPassHelpers.CreateFullscreenPipeline(device, renderPass, VkSampleCountFlags._1Bit, vertexModule, fragmentModule, pipelineLayout, BlendState.Opaque, "RenderTextureBlit");
	}

	private unsafe VkDescriptorPool CreateDescriptorPool(int maxSets)
	{
		VkDescriptorPoolSize vkDescriptorPoolSize = new VkDescriptorPoolSize();
		vkDescriptorPoolSize.Type = VkDescriptorType.CombinedImageSampler;
		vkDescriptorPoolSize.DescriptorCount = maxSets;
		VkDescriptorPoolSize vkDescriptorPoolSize2 = vkDescriptorPoolSize;
		DeviceEx handle = device;
		VkDescriptorPoolCreateInfo pCreateInfo = new VkDescriptorPoolCreateInfo
		{
			MaxSets = maxSets,
			PoolSizeCount = 1,
			PoolSizes = &vkDescriptorPoolSize2
		};
		return handle.CreateDescriptorPool(in pCreateInfo, device.VkAllocator);
	}

	public void OnRenderTargetRebuilt(IRenderTarget renderTarget)
	{
		if (renderTarget is IRenderContext renderContext)
		{
			this.renderContext = renderContext;
			meshRenderer?.OnRenderContextRebuilt(renderContext);
		}
	}

	public void Dispose()
	{
		if (Interlocked.Exchange(ref disposed, 1) != 0)
		{
			return;
		}
		SubmittedBatch result;
		while (pending.TryDequeue(out result))
		{
		}
		meshRenderer?.Dispose();
		if (!meshRenderPass.IsNull())
		{
			device.DestroyRenderPass(meshRenderPass, device.VkAllocator);
		}
		VkDescriptorPool[] array = framePools;
		for (int i = 0; i < array.Length; i++)
		{
			VkDescriptorPool descriptorPool = array[i];
			if (!descriptorPool.IsNull())
			{
				device.DestroyDescriptorPool(descriptorPool, device.VkAllocator);
			}
		}
		if (!immediateCommandPool.IsNull())
		{
			device.DestroyCommandPool(immediateCommandPool, device.VkAllocator);
		}
		if (!pipeline.IsNull())
		{
			device.DestroyPipeline(pipeline, device.VkAllocator);
		}
		if (!defaultSampler.IsNull())
		{
			device.DestroySampler(defaultSampler, device.VkAllocator);
		}
		if (!pipelineLayout.IsNull())
		{
			device.DestroyPipelineLayout(pipelineLayout, device.VkAllocator);
		}
		if (!descriptorSetLayout.IsNull())
		{
			device.DestroyDescriptorSetLayout(descriptorSetLayout, device.VkAllocator);
		}
		if (!fragmentModule.IsNull())
		{
			device.DestroyShaderModule(fragmentModule, device.VkAllocator);
		}
		if (!vertexModule.IsNull())
		{
			device.DestroyShaderModule(vertexModule, device.VkAllocator);
		}
		if (!renderPass.IsNull())
		{
			device.DestroyRenderPass(renderPass, device.VkAllocator);
		}
	}

	object? IRenderFeature.CaptureFrame(in FrameTiming timing)
	{
		return CaptureFrame(in timing);
	}

	void IRenderPass.Record(in FrameContext ctx, object? featurePayload)
	{
		Record(in ctx, featurePayload);
	}
}
