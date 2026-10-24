using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using Brutal;
using Brutal.VulkanApi;
using Brutal.VulkanApi.Abstractions;
using Brutal.VulkanApi.Vma;
using Waylith.Graphics;

namespace Waylith.Rendering.PostFx;

public class PostEffectPass : IRenderPass, IDisposable
{
	private const int MaxBindings = 16;

	private const int UniformSlotBytes = 256;

	private static readonly Lock gate = new Lock();

	private static readonly List<PostEffectPass> live = new List<PostEffectPass>();

	private readonly string fragmentShaderPath;

	private readonly int[] bindings;

	private readonly BlendState blend;

	private readonly int pushConstantSize;

	private readonly int maxPassesPerFrame;

	private readonly bool writeDepth;

	private readonly VkImageView[] inputViews = new VkImageView[16];

	private readonly int uniformBinding;

	private readonly VkSampler[] inputSamplers = new VkSampler[16];

	private readonly byte[] pushConstantData = new byte[128];

	private int pushConstantWritten;

	private readonly byte[] uniformData = new byte[256];

	private int uniformWritten;

	private VmaAllocator allocator;

	private BufferAllocationInfo uniformBuffer;

	private AllocationMapInfo uniformMap;

	private unsafe byte* uniformMapped;

	private int uniformSlotStride;

	private int uniformSlotsPerFrame;

	private int uniformSlotCursor;

	private DeviceEx device;

	private VkShaderModule vertexModule;

	private VkShaderModule fragmentModule;

	private VkDescriptorSetLayout descriptorSetLayout;

	private VkPipelineLayout pipelineLayout;

	private VkRenderPass renderPass;

	private bool ownsRenderPass;

	private VkPipeline pipeline;

	private VkDescriptorPool[] descriptorPools = Array.Empty<VkDescriptorPool>();

	private bool initialized;

	private bool disposed;

	public string Name { get; }

	public byte PassId { get; }

	public RenderTexture? Output { get; set; }

	public PassAttachment? Attachment => null;

	public virtual bool PreBackbuffer => true;

	public PostEffectPass(string name, string fragmentShaderPath, ReadOnlySpan<int> bindings, BlendState blend = default(BlendState), int pushConstantSize = 0, int maxPassesPerFrame = 1, int uniformBinding = -1, bool writeDepth = false)
	{
		Name = name;
		PassId = RenderPassRegistry.Register(name);
		this.fragmentShaderPath = fragmentShaderPath;
		this.bindings = bindings.ToArray();
		this.blend = (blend.Equals(default(BlendState)) ? BlendState.Opaque : blend);
		this.pushConstantSize = pushConstantSize;
		this.maxPassesPerFrame = System.Math.Max(1, maxPassesPerFrame);
		this.uniformBinding = uniformBinding;
		this.writeDepth = writeDepth;
		using (gate.EnterScope())
		{
			live.Add(this);
		}
	}

	public void SetInput(int binding, RenderTexture texture)
	{
		SetInput(binding, texture.Color, texture.Sampler);
	}

	public void SetInput(int binding, GpuTextureAllocation texture, VkSampler sampler)
	{
		SetInput(binding, texture.ImageView, sampler);
	}

	public void SetInput(int binding, VkImageView view, VkSampler sampler)
	{
		inputViews[binding] = view;
		inputSamplers[binding] = sampler;
	}

	public void SetPushConstants<T>(in T data) where T : unmanaged
	{
		pushConstantWritten = Unsafe.SizeOf<T>();
		MemoryMarshal.Write(pushConstantData, in data);
	}

	public void SetUniforms<T>(in T data) where T : unmanaged
	{
		uniformWritten = Unsafe.SizeOf<T>();
		MemoryMarshal.Write(uniformData, in data);
	}

	public virtual void Record(in FrameContext ctx, object? featurePayload)
	{
		RenderTexture output = Output;
		if (output != null)
		{
			EnsureInitialized(in ctx, output.ColorFormat);
			BeginFrame(ctx.SubmissionFrameIndex);
			RecordPass(ctx.CommandBuffer, ctx.SubmissionFrameIndex, output);
		}
	}

	public void RecordInline(in FrameContext ctx, VkRenderPass openRenderPass, VkSampleCountFlags sampleCount)
	{
		EnsureInline(in ctx, openRenderPass, sampleCount);
		int submissionFrameIndex = ctx.SubmissionFrameIndex;
		BeginFrame(submissionFrameIndex);
		VkDescriptorSet vkDescriptorSet = DescriptorSetWriter.AllocateDescriptorSet(device, descriptorPools[submissionFrameIndex], descriptorSetLayout);
		WriteInputs(vkDescriptorSet, submissionFrameIndex);
		CommandBuffer commandBuffer = ctx.CommandBuffer;
		VkExtent2D extent = ctx.Extent;
		commandBuffer.BindPipeline(VkPipelineBindPoint.Graphics, pipeline);
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
		VkRect2D reference2 = new VkRect2D(extent);
		commandBuffer.SetScissor(0, new ReadOnlySpan<VkRect2D>(in reference2));
		commandBuffer.BindDescriptorSet(VkPipelineBindPoint.Graphics, pipelineLayout, 0, vkDescriptorSet, default(Span<ByteSize32>));
		if (pushConstantSize > 0 && pushConstantWritten > 0)
		{
			commandBuffer.PushConstants(pipelineLayout, VkShaderStageFlags.FragmentBit, ByteSize.Zero, new ReadOnlySpan<byte>(pushConstantData, 0, pushConstantWritten));
		}
		commandBuffer.Draw(3, 1, 0, 0);
	}

	protected void BeginFrame(int submissionFrame)
	{
		device.ResetDescriptorPool(descriptorPools[submissionFrame], 0);
		uniformSlotCursor = 0;
	}

	protected void RecordPass(CommandBuffer cmd, int submissionFrame, RenderTexture output)
	{
		VkDescriptorSet vkDescriptorSet = DescriptorSetWriter.AllocateDescriptorSet(device, descriptorPools[submissionFrame], descriptorSetLayout);
		WriteInputs(vkDescriptorSet, submissionFrame);
		VkExtent2D extent = output.Extent;
		VkRenderPassBeginInfo pRenderPassBegin = new VkRenderPassBeginInfo
		{
			RenderPass = renderPass,
			Framebuffer = output.GetFramebuffer(renderPass),
			RenderArea = new VkRect2D(extent),
			ClearValueCount = 0,
			ClearValues = null
		};
		cmd.BeginRenderPass(in pRenderPassBegin, VkSubpassContents.Inline);
		cmd.BindPipeline(VkPipelineBindPoint.Graphics, pipeline);
		VkViewport reference = new VkViewport
		{
			X = 0f,
			Y = 0f,
			Width = extent.Width,
			Height = extent.Height,
			MinDepth = 0f,
			MaxDepth = 1f
		};
		cmd.SetViewport(0, new ReadOnlySpan<VkViewport>(in reference));
		VkRect2D reference2 = new VkRect2D(extent);
		cmd.SetScissor(0, new ReadOnlySpan<VkRect2D>(in reference2));
		cmd.BindDescriptorSet(VkPipelineBindPoint.Graphics, pipelineLayout, 0, vkDescriptorSet, default(Span<ByteSize32>));
		if (pushConstantSize > 0 && pushConstantWritten > 0)
		{
			cmd.PushConstants(pipelineLayout, VkShaderStageFlags.FragmentBit, ByteSize.Zero, new ReadOnlySpan<byte>(pushConstantData, 0, pushConstantWritten));
		}
		cmd.Draw(3, 1, 0, 0);
		cmd.EndRenderPass();
		output.Color.SetMipLayout(0, VkImageLayout.ShaderReadOnlyOptimal);
		if (Platform.IsMacOS)
		{
			VkImageMemoryBarrier vkImageMemoryBarrier = new VkImageMemoryBarrier();
			vkImageMemoryBarrier.SrcAccessMask = VkAccessFlags.ColorAttachmentWriteBit;
			vkImageMemoryBarrier.DstAccessMask = VkAccessFlags.ShaderReadBit;
			vkImageMemoryBarrier.OldLayout = VkImageLayout.ShaderReadOnlyOptimal;
			vkImageMemoryBarrier.NewLayout = VkImageLayout.ShaderReadOnlyOptimal;
			vkImageMemoryBarrier.SrcQueueFamilyIndex = -1;
			vkImageMemoryBarrier.DstQueueFamilyIndex = -1;
			vkImageMemoryBarrier.Image = output.Color.Image;
			vkImageMemoryBarrier.SubresourceRange = new VkImageSubresourceRange
			{
				AspectMask = VkImageAspectFlags.ColorBit,
				BaseMipLevel = 0,
				LevelCount = 1,
				BaseArrayLayer = 0,
				LayerCount = 1
			};
			VkImageMemoryBarrier vkImageMemoryBarrier2 = vkImageMemoryBarrier;
			VkImageMemoryBarrier reference3 = vkImageMemoryBarrier2;
			cmd.PipelineBarrier(VkPipelineStageFlags.ColorAttachmentOutputBit, VkPipelineStageFlags.FragmentShaderBit, VkDependencyFlags.None, default(ReadOnlySpan<VkMemoryBarrier>), default(ReadOnlySpan<VkBufferMemoryBarrier>), new ReadOnlySpan<VkImageMemoryBarrier>(in reference3));
		}
	}

	public virtual void OnRenderTargetRebuilt(IRenderTarget target)
	{
	}

	protected void EnsureInitialized(in FrameContext ctx, VkFormat colorFormat)
	{
		if (!initialized)
		{
			device = ctx.GraphicsDevice.Device;
			Build(in ctx, PostFxPassHelpers.CreateColorRenderPass(device, colorFormat), VkSampleCountFlags._1Bit, owns: true);
		}
	}

	private void EnsureInline(in FrameContext ctx, VkRenderPass openRenderPass, VkSampleCountFlags sampleCount)
	{
		if (!initialized)
		{
			device = ctx.GraphicsDevice.Device;
			Build(in ctx, openRenderPass, sampleCount, owns: false);
		}
	}

	private void Build(in FrameContext ctx, VkRenderPass targetRenderPass, VkSampleCountFlags sampleCount, bool owns)
	{
		renderPass = targetRenderPass;
		ownsRenderPass = owns;
		vertexModule = PostFxPassHelpers.CompileFullscreenVertex(device);
		fragmentModule = PostFxPassHelpers.CompileFragment(device, fragmentShaderPath);
		descriptorSetLayout = CreateDescriptorSetLayout();
		pipelineLayout = CreatePipelineLayout();
		pipeline = PostFxPassHelpers.CreateFullscreenPipeline(device, renderPass, sampleCount, vertexModule, fragmentModule, pipelineLayout, blend, "PostEffect." + Name, writeDepth);
		descriptorPools = CreateDescriptorPools(ctx.RenderContext.FramesInFlight);
		if (uniformBinding >= 0)
		{
			CreateUniformBuffer(in ctx);
		}
		initialized = true;
	}

	private unsafe void CreateUniformBuffer(in FrameContext ctx)
	{
		int val = ctx.GraphicsDevice.PhysicalDevice.GetProperties().Limits.MinUniformBufferOffsetAlignment;
		uniformSlotStride = System.Math.Max(val, 256);
		uniformSlotsPerFrame = maxPassesPerFrame;
		string name = "PostEffect." + Name + ".Uniforms";
		VmaAllocator vmaAllocator = ctx.GraphicsDevice.Allocator;
		VkBufferCreateInfo bufferCreateInfo = new VkBufferCreateInfo
		{
			Size = ByteSize.Of(uniformSlotStride * uniformSlotsPerFrame * ctx.RenderContext.FramesInFlight),
			Usage = VkBufferUsageFlags.UniformBufferBit,
			SharingMode = VkSharingMode.Exclusive
		};
		AllocationInfo.CreateInfo allocateCreateInfo = new AllocationInfo.CreateInfo
		{
			Preference = MemoryPreference.PreferCpu,
			RequiredProperties = (VkMemoryPropertyFlags.HostVisibleBit | VkMemoryPropertyFlags.HostCoherentBit),
			PreferredProperties = (VkMemoryPropertyFlags.HostVisibleBit | VkMemoryPropertyFlags.HostCoherentBit),
			Priority = 1f
		};
		uniformBuffer = vmaAllocator.AllocateBufferChecked(name, in bufferCreateInfo, in allocateCreateInfo);
		uniformMap = new AllocationMapInfo(uniformBuffer.AllocationInfo)
		{
			Region = new MapRegion
			{
				Offset = ByteSize.Zero,
				Size = uniformBuffer.BufferSize
			}
		};
		allocator = ctx.GraphicsDevice.Allocator;
		uniformMapped = (byte*)(void*)allocator.MapMemory(in uniformMap);
		GpuMemoryTracker.Add(GpuMemoryCategory.UniformBuffer, in uniformBuffer, null, name);
	}

	private unsafe void DestroyUniformBuffer()
	{
		if (uniformMapped != null)
		{
			GpuMemoryTracker.Remove(GpuMemoryCategory.UniformBuffer, in uniformBuffer);
			allocator.UnmapMemory(in uniformMap);
			allocator.FreeAllocation(in uniformBuffer);
			uniformMapped = null;
			uniformBuffer = default(BufferAllocationInfo);
		}
	}

	private unsafe void WriteInputs(VkDescriptorSet set, int submissionFrame)
	{
		int num = bindings.Length;
		bool flag = uniformBinding >= 0;
		VkDescriptorImageInfo* ptr = stackalloc VkDescriptorImageInfo[num];
		VkWriteDescriptorSet* ptr2 = stackalloc VkWriteDescriptorSet[num + 1];
		for (int i = 0; i < num; i++)
		{
			int num2 = bindings[i];
			ptr[i] = new VkDescriptorImageInfo
			{
				Sampler = inputSamplers[num2],
				ImageView = inputViews[num2],
				ImageLayout = VkImageLayout.ShaderReadOnlyOptimal
			};
			ptr2[i] = new VkWriteDescriptorSet
			{
				DstSet = set,
				DstBinding = num2,
				DescriptorCount = 1,
				DescriptorType = VkDescriptorType.CombinedImageSampler,
				ImageInfo = ptr + i
			};
		}
		if (flag)
		{
			int num3 = System.Math.Min(uniformSlotCursor++, uniformSlotsPerFrame - 1);
			int num4 = (submissionFrame * uniformSlotsPerFrame + num3) * uniformSlotStride;
			new ReadOnlySpan<byte>(uniformData, 0, uniformWritten).CopyTo(new Span<byte>(uniformMapped + num4, uniformSlotStride));
			VkDescriptorBufferInfo vkDescriptorBufferInfo = new VkDescriptorBufferInfo();
			vkDescriptorBufferInfo.Buffer = uniformBuffer.VkBuffer;
			vkDescriptorBufferInfo.Offset = ByteSize.Of(num4);
			vkDescriptorBufferInfo.Range = ByteSize.Of(256);
			VkDescriptorBufferInfo vkDescriptorBufferInfo2 = vkDescriptorBufferInfo;
			ptr2[num] = new VkWriteDescriptorSet
			{
				DstSet = set,
				DstBinding = uniformBinding,
				DescriptorCount = 1,
				DescriptorType = VkDescriptorType.UniformBuffer,
				BufferInfo = &vkDescriptorBufferInfo2
			};
		}
		device.UpdateDescriptorSets(new ReadOnlySpan<VkWriteDescriptorSet>(ptr2, flag ? (num + 1) : num), default(ReadOnlySpan<VkCopyDescriptorSet>));
	}

	private unsafe VkDescriptorSetLayout CreateDescriptorSetLayout()
	{
		bool flag = uniformBinding >= 0;
		VkDescriptorSetLayoutBinding[] array = new VkDescriptorSetLayoutBinding[bindings.Length + (flag ? 1 : 0)];
		for (int i = 0; i < bindings.Length; i++)
		{
			array[i] = new VkDescriptorSetLayoutBinding
			{
				Binding = bindings[i],
				DescriptorType = VkDescriptorType.CombinedImageSampler,
				DescriptorCount = 1,
				StageFlags = VkShaderStageFlags.FragmentBit
			};
		}
		if (flag)
		{
			array[bindings.Length] = new VkDescriptorSetLayoutBinding
			{
				Binding = uniformBinding,
				DescriptorType = VkDescriptorType.UniformBuffer,
				DescriptorCount = 1,
				StageFlags = VkShaderStageFlags.FragmentBit
			};
		}
		fixed (VkDescriptorSetLayoutBinding* pointer = array)
		{
			return device.CreateDescriptorSetLayout(new DescriptorSetLayoutEx.CreateInfo
			{
				Bindings = new Span<VkDescriptorSetLayoutBinding>(pointer, array.Length)
			}, device.VkAllocator);
		}
	}

	private VkPipelineLayout CreatePipelineLayout()
	{
		VkDescriptorSetLayout reference = descriptorSetLayout;
		if (pushConstantSize <= 0)
		{
			return device.CreatePipelineLayout(new ReadOnlySpan<VkDescriptorSetLayout>(in reference), default(ReadOnlySpan<VkPushConstantRange>), device.VkAllocator);
		}
		VkPushConstantRange vkPushConstantRange = new VkPushConstantRange();
		vkPushConstantRange.StageFlags = VkShaderStageFlags.FragmentBit;
		vkPushConstantRange.Offset = ByteSize.Zero;
		vkPushConstantRange.Size = ByteSize.Of(pushConstantSize);
		VkPushConstantRange vkPushConstantRange2 = vkPushConstantRange;
		DeviceEx inDevice = device;
		ReadOnlySpan<VkDescriptorSetLayout> setLayouts = new ReadOnlySpan<VkDescriptorSetLayout>(in reference);
		VkPushConstantRange reference2 = vkPushConstantRange2;
		return inDevice.CreatePipelineLayout(setLayouts, new ReadOnlySpan<VkPushConstantRange>(in reference2), device.VkAllocator);
	}

	private unsafe VkDescriptorPool[] CreateDescriptorPools(int framesInFlight)
	{
		int descriptorCount = System.Math.Max(1, bindings.Length) * maxPassesPerFrame;
		VkDescriptorPool[] array = new VkDescriptorPool[framesInFlight];
		VkDescriptorPoolSize* ptr = stackalloc VkDescriptorPoolSize[2];
		*ptr = new VkDescriptorPoolSize
		{
			Type = VkDescriptorType.CombinedImageSampler,
			DescriptorCount = descriptorCount
		};
		ptr[1] = new VkDescriptorPoolSize
		{
			Type = VkDescriptorType.UniformBuffer,
			DescriptorCount = maxPassesPerFrame
		};
		for (int i = 0; i < framesInFlight; i++)
		{
			array[i] = device.CreateDescriptorPool(new VkDescriptorPoolCreateInfo
			{
				MaxSets = maxPassesPerFrame,
				PoolSizeCount = ((uniformBinding < 0) ? 1 : 2),
				PoolSizes = ptr
			}, device.VkAllocator);
		}
		return array;
	}

	public virtual void Invalidate()
	{
		if (initialized)
		{
			VkDescriptorPool[] array = descriptorPools;
			foreach (VkDescriptorPool descriptorPool in array)
			{
				device.DestroyDescriptorPool(descriptorPool, device.VkAllocator);
			}
			descriptorPools = Array.Empty<VkDescriptorPool>();
			DestroyUniformBuffer();
			device.DestroyPipeline(pipeline, device.VkAllocator);
			if (ownsRenderPass)
			{
				device.DestroyRenderPass(renderPass, device.VkAllocator);
			}
			device.DestroyPipelineLayout(pipelineLayout, device.VkAllocator);
			device.DestroyDescriptorSetLayout(descriptorSetLayout, device.VkAllocator);
			device.DestroyShaderModule(fragmentModule, device.VkAllocator);
			device.DestroyShaderModule(vertexModule, device.VkAllocator);
			initialized = false;
		}
	}

	public virtual void Dispose()
	{
		if (!disposed)
		{
			disposed = true;
			GC.SuppressFinalize(this);
			using (gate.EnterScope())
			{
				live.Remove(this);
			}
			Invalidate();
		}
	}

	public static void ReloadAll()
	{
		PostEffectPass[] array;
		using (gate.EnterScope())
		{
			if (live.Count == 0)
			{
				return;
			}
			array = live.ToArray();
		}
		PostEffectPass[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			array2[i].Invalidate();
		}
	}
}
