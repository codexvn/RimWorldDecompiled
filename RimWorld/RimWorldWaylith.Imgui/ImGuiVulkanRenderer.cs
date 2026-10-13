using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using Brutal;
using Brutal.ImGuiApi;
using Brutal.Memory;
using Brutal.Numerics;
using Brutal.Pointers;
using Brutal.Pointers.Extensions;
using Brutal.VulkanApi;
using Brutal.VulkanApi.Abstractions;
using Brutal.VulkanApi.Vma;
using UnityStubs;
using Waylith.Graphics;
using Waylith.Rendering;

namespace RimWorldWaylith.Imgui;

internal sealed class ImGuiVulkanRenderer : IDisposable
{
	private struct FrameBuffers
	{
		public BufferAllocationInfo VertexBuffer;

		public BufferAllocationInfo IndexBuffer;
	}

	private struct UserTexture
	{
		public VkDescriptorSet DescriptorSet;

		public int LastUsedFrame;
	}

	private struct BackendTexture
	{
		public nint Allocation;

		public VkImage Image;

		public VkImageView ImageView;

		public VkDescriptorSet DescriptorSet;
	}

	private static readonly Ptr<byte> RendererName = "RimWorldWaylith.ImGuiVulkanRenderer"u8;

	private const int MinimumImageSamplerPoolSize = 8;

	private readonly ByteSize _bufferGrowthGranularity = ByteSize.Of(256);

	private readonly VkSampler _texSampler;

	private readonly VkSampler _texSamplerMipped;

	private DescriptorSetLayoutEx _descriptorSetLayout;

	private readonly List<DescriptorPoolEx> _descriptorPools = new List<DescriptorPoolEx>();

	private readonly VkPipelineLayout _pipelineLayout;

	private readonly VkShaderModule _shaderModuleVert;

	private readonly VkShaderModule _shaderModuleFrag;

	private readonly VkPipeline _pipeline;

	private readonly VkCommandPool _texCommandPool;

	private CommandBuffer _texCommandBuffer;

	private readonly ImGuiMaterialRenderer _materialRenderer;

	private readonly VulkanShaderLayoutCache _layoutCache;

	private readonly ShaderModuleCache _offscreenShaderModules;

	private readonly Dictionary<nint, (VkPipeline Pipeline, ImGuiMaterialRenderer Materials)> _offscreenTargets = new Dictionary<nint, (VkPipeline, ImGuiMaterialRenderer)>();

	private FrameBuffers _offscreenBuffers;

	private int _offscreenShadersStale;

	private readonly FrameBuffers[] _frames;

	private readonly int[] _slotFrameNumbers;

	private int _gpuDoneFrame = int.MinValue;

	private readonly Dictionary<GpuTextureAllocation, UserTexture> _userTextures = new Dictionary<GpuTextureAllocation, UserTexture>();

	private readonly List<GpuTextureAllocation> _idleUserTextures = new List<GpuTextureAllocation>();

	private int _frameNumber;

	private bool _disposed;

	private DeviceEx Device { get; }

	private PhysicalDevice PhysicalDevice => Device.PhysicalDevice;

	private GraphicsDevice GraphicsDevice { get; }

	private VkRenderPass RenderPass { get; }

	private int SubPass { get; }

	private VkSampleCountFlags SampleCount { get; }

	private int ImageCount { get; }

	private int FramesInFlight { get; }

	private int DescriptorPoolSize { get; }

	public ImGuiVulkanRenderer(GraphicsDevice graphicsDevice, VkRenderPass renderPass, int subPass, VkSampleCountFlags sampleCount, int imageCount, int framesInFlight, int descriptorPoolSize, VulkanShaderLayoutCache layoutCache, ShaderModuleCache shaderModules)
	{
		DeviceEx device = (Device = graphicsDevice.Device);
		GraphicsDevice = graphicsDevice;
		RenderPass = renderPass;
		SubPass = subPass;
		SampleCount = sampleCount;
		ImageCount = imageCount;
		FramesInFlight = Math.Max(1, framesInFlight);
		DescriptorPoolSize = descriptorPoolSize;
		ImGuiIOPtr iO = ImGui.GetIO();
		iO.BackendRendererName = RendererName;
		iO.BackendFlags |= ImGuiBackendFlags.RendererHasVtxOffset;
		iO.BackendFlags |= ImGuiBackendFlags.RendererHasTextures;
		_texSampler = CreateSampler(0f);
		_texSamplerMipped = CreateSampler(1000f);
		_descriptorSetLayout = CreateDescriptorSetLayout();
		_descriptorPools.Add(CreateDescriptorPool());
		_pipelineLayout = CreatePipelineLayout();
		CreateShaderModules(out _shaderModuleVert, out _shaderModuleFrag);
		_pipeline = CreatePipeline(RenderPass, SampleCount);
		CreateCommandBuffer(out _texCommandPool, out _texCommandBuffer);
		_layoutCache = layoutCache;
		_offscreenShaderModules = new ShaderModuleCache(device);
		_frames = new FrameBuffers[FramesInFlight];
		_slotFrameNumbers = new int[FramesInFlight];
		Array.Fill(_slotFrameNumbers, int.MinValue);
		_materialRenderer = new ImGuiMaterialRenderer(graphicsDevice, layoutCache, shaderModules, _shaderModuleVert, subPass, sampleCount, _texSampler, _texSamplerMipped, FramesInFlight);
	}

	public void BeginFrame(int frameNumber)
	{
		_frameNumber = frameNumber;
		ReleaseIdleUserTextures();
	}

	public void ProcessTextureUpdates(ImDrawDataPtr drawData)
	{
		if (!drawData.Textures.IsNotNull())
		{
			return;
		}
		foreach (ImTextureDataPtr item in drawData.Textures.Value)
		{
			if (item.Status != 0)
			{
				UpdateTexture(item);
			}
		}
	}

	public void ProcessPendingTextureUpdates()
	{
		foreach (ImTextureDataPtr tex in ImGui.GetIO().Fonts.TexList)
		{
			if (tex.Status != 0)
			{
				UpdateTexture(tex);
			}
		}
	}

	public void Render(ImGuiDrawSnapshot snap, in FrameContext ctx)
	{
		int num = (ctx.SubmissionFrameIndex % _frames.Length + _frames.Length) % _frames.Length;
		if (_slotFrameNumbers[num] > _gpuDoneFrame)
		{
			Volatile.Write(ref _gpuDoneFrame, _slotFrameNumbers[num]);
		}
		_slotFrameNumbers[num] = snap.FrameNumber;
		if (snap.HasContent)
		{
			_materialRenderer.BeginFrame(num);
			RecordDraws(snap, ctx.CommandBuffer, ctx.RenderPass, _pipeline, _materialRenderer, num, ref _frames[num]);
		}
	}

	public void OnShadersReloaded()
	{
		_materialRenderer.DestroyPipelines();
		Volatile.Write(ref _offscreenShadersStale, 1);
	}

	public void RecordOffscreen(ImGuiDrawSnapshot snap, CommandBuffer commandBuffer, VkRenderPass renderPass)
	{
		if (!snap.HasContent)
		{
			return;
		}
		if (Interlocked.Exchange(ref _offscreenShadersStale, 0) == 1)
		{
			_offscreenShaderModules.Clear();
			foreach (var value2 in _offscreenTargets.Values)
			{
				value2.Materials.DestroyPipelines();
			}
		}
		if (!_offscreenTargets.TryGetValue(renderPass.VkHandle, out (VkPipeline, ImGuiMaterialRenderer) value))
		{
			value = (CreatePipeline(renderPass, VkSampleCountFlags._1Bit), new ImGuiMaterialRenderer(GraphicsDevice, _layoutCache, _offscreenShaderModules, _shaderModuleVert, SubPass, VkSampleCountFlags._1Bit, _texSampler, _texSamplerMipped, 1));
			_offscreenTargets[renderPass.VkHandle] = value;
		}
		value.Item2.BeginFrame(0);
		RecordDraws(snap, commandBuffer, renderPass, value.Item1, value.Item2, 0, ref _offscreenBuffers);
	}

	private unsafe void RecordDraws(ImGuiDrawSnapshot snap, CommandBuffer commandBuffer, VkRenderPass renderPass, VkPipeline pipeline, ImGuiMaterialRenderer materialRenderer, int slot, ref FrameBuffers frame)
	{
		int num = (int)(snap.DisplaySize.X * snap.FramebufferScale.X);
		int num2 = (int)(snap.DisplaySize.Y * snap.FramebufferScale.Y);
		if (num <= 0 || num2 <= 0)
		{
			return;
		}
		ByteSize size = ByteSize.Of<ImDrawVert>(snap.VertexCount).AlignTo(_bufferGrowthGranularity);
		ByteSize size2 = ByteSize.Of<ushort>(snap.IndexCount).AlignTo(_bufferGrowthGranularity);
		if (frame.VertexBuffer.VkBuffer.IsNull() || frame.VertexBuffer.BufferSize.Bytes < size.Bytes)
		{
			CreateOrResizeBuffer(ref frame.VertexBuffer, "ImGui.Vertices", size, VkBufferUsageFlags.VertexBufferBit);
		}
		if (frame.IndexBuffer.VkBuffer.IsNull() || frame.IndexBuffer.BufferSize.Bytes < size2.Bytes)
		{
			CreateOrResizeBuffer(ref frame.IndexBuffer, "ImGui.Indices", size2, VkBufferUsageFlags.IndexBufferBit);
		}
		VmaAllocator allocator = GraphicsDevice.Allocator;
		byte* pointer = ImGuiGpuMemory.Map(allocator, in frame.VertexBuffer);
		byte* pointer2 = ImGuiGpuMemory.Map(allocator, in frame.IndexBuffer);
		fixed (ImDrawVert* vertices = snap.Vertices)
		{
			Mem.Copy(new Ptr<ImDrawVert>(vertices), new Ptr<ImDrawVert>((ImDrawVert*)pointer), snap.VertexCount);
		}
		fixed (ushort* indices = snap.Indices)
		{
			Mem.Copy(new Ptr<ushort>(indices), new Ptr<ushort>((ushort*)pointer2), snap.IndexCount);
		}
		ImGuiGpuMemory.Unmap(allocator, in frame.VertexBuffer);
		ImGuiGpuMemory.Unmap(allocator, in frame.IndexBuffer);
		commandBuffer.BindPipeline(VkPipelineBindPoint.Graphics, pipeline);
		commandBuffer.BindVertexBuffer(0, frame.VertexBuffer.VkBuffer);
		VkDeviceExtensions.BindIndexBuffer(commandBuffer, frame.IndexBuffer.VkBuffer, ByteSize.Zero, VkIndexType.UInt16);
		VkViewport reference = new VkViewport
		{
			Width = num,
			Height = num2,
			MinDepth = 0f,
			MaxDepth = 1f
		};
		commandBuffer.SetViewport(0, new ReadOnlySpan<VkViewport>(in reference));
		InlineArray2<float> buffer = default(InlineArray2<float>);
		global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray2<float>, float>(ref buffer, 0) = 2f / snap.DisplaySize.X;
		global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray2<float>, float>(ref buffer, 1) = 2f / snap.DisplaySize.Y;
		Span<float> span = global::_003CPrivateImplementationDetails_003E.InlineArrayAsSpan<InlineArray2<float>, float>(ref buffer, 2);
		InlineArray2<float> buffer2 = default(InlineArray2<float>);
		global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray2<float>, float>(ref buffer2, 0) = -1f - snap.DisplayPos.X * span[0];
		global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray2<float>, float>(ref buffer2, 1) = -1f - snap.DisplayPos.Y * span[1];
		Span<float> span2 = global::_003CPrivateImplementationDetails_003E.InlineArrayAsSpan<InlineArray2<float>, float>(ref buffer2, 2);
		commandBuffer.PushConstants(_pipelineLayout, VkShaderStageFlags.VertexBit, 0, MemoryMarshal.AsBytes(span));
		commandBuffer.PushConstants(_pipelineLayout, VkShaderStageFlags.VertexBit, 8, MemoryMarshal.AsBytes(span2));
		float2 displayPos = snap.DisplayPos;
		float2 framebufferScale = snap.FramebufferScale;
		VkDescriptorSet vkDescriptorSet = default(VkDescriptorSet);
		bool flag = false;
		for (int i = 0; i < snap.CommandCount; i++)
		{
			ref ImGuiDrawCommand reference2 = ref snap.Commands[i];
			float2 a = new float2((reference2.ClipRect.X - displayPos.X) * framebufferScale.X, (reference2.ClipRect.Y - displayPos.Y) * framebufferScale.Y);
			float2 a2 = new float2((reference2.ClipRect.Z - displayPos.X) * framebufferScale.X, (reference2.ClipRect.W - displayPos.Y) * framebufferScale.Y);
			a = float2.Max(a, float2.Zero);
			a2 = float2.Min(a2, new float2(num, num2));
			if (a2.X <= a.X || a2.Y <= a.Y)
			{
				continue;
			}
			VkRect2D vkRect2D = new VkRect2D();
			vkRect2D.Offset = new VkOffset2D
			{
				X = (int)a.X,
				Y = (int)a.Y
			};
			vkRect2D.Extent = new VkExtent2D
			{
				Width = (int)(a2.X - a.X),
				Height = (int)(a2.Y - a.Y)
			};
			VkRect2D reference3 = vkRect2D;
			Span<VkRect2D> span3 = new Span<VkRect2D>(ref reference3);
			commandBuffer.SetScissor(0, span3);
			nint num3 = reference2.TexId;
			if (num3 < 0)
			{
				int num4 = (int)(-num3) - 1;
				if ((uint)num4 >= (uint)snap.MaterialDrawCount)
				{
					continue;
				}
				ref ImGuiMaterialDraw reference4 = ref snap.MaterialDraws[num4];
				if (reference4.Material != null && materialRenderer.TryRecord(commandBuffer, renderPass, slot, in reference4, span, span2, (int)reference2.ElemCount, reference2.IdxOffset, reference2.VtxOffset))
				{
					flag = true;
					vkDescriptorSet = default(VkDescriptorSet);
					continue;
				}
				num3 = reference4.FallbackTextureId;
			}
			if (flag)
			{
				commandBuffer.BindPipeline(VkPipelineBindPoint.Graphics, pipeline);
				commandBuffer.PushConstants(_pipelineLayout, VkShaderStageFlags.VertexBit, 0, MemoryMarshal.AsBytes(span));
				commandBuffer.PushConstants(_pipelineLayout, VkShaderStageFlags.VertexBit, 8, MemoryMarshal.AsBytes(span2));
				flag = false;
				vkDescriptorSet = default(VkDescriptorSet);
			}
			VkDescriptorSet reference5 = new VkDescriptorSet(num3);
			Span<VkDescriptorSet> span4 = new Span<VkDescriptorSet>(ref reference5);
			if (span4[0].VkHandle != vkDescriptorSet.VkHandle)
			{
				commandBuffer.BindDescriptorSets(VkPipelineBindPoint.Graphics, _pipelineLayout, 0, span4, default(Span<ByteSize32>));
			}
			vkDescriptorSet = span4[0];
			commandBuffer.DrawIndexed((int)reference2.ElemCount, 1, reference2.IdxOffset, reference2.VtxOffset, 0);
		}
	}

	private void CreateOrResizeBuffer(ref BufferAllocationInfo buffer, string name, ByteSize size, VkBufferUsageFlags usage)
	{
		ImGuiGpuMemory.Free(GraphicsDevice.Allocator, ref buffer, GpuMemoryCategory.DynamicDraw);
		buffer = ImGuiGpuMemory.CreateHostBuffer(GraphicsDevice.Allocator, name, size, usage, GpuMemoryCategory.DynamicDraw);
	}

	private unsafe void UpdateTexture(ImTextureDataPtr tex)
	{
		if (tex.Status == ImTextureStatus.OK)
		{
			return;
		}
		if (tex.Status == ImTextureStatus.WantCreate)
		{
			Assert(tex.TexID == ImTextureID.Invalid && tex.BackendUserData == 0);
			Assert(tex.Format == ImTextureFormat.RGBA32);
			Ptr<BackendTexture> ptr = Mem.Alloc<BackendTexture>();
			ImageAllocationInfo allocation = GpuTextureService.AllocateTextureImage(GraphicsDevice.Allocator, "ImGui.FontAtlas", GpuMemoryCategory.UITexture, new VkImageCreateInfo
			{
				ImageType = VkImageType._2D,
				Format = VkFormat.R8G8B8A8UNorm,
				Extent = new VkExtent3D
				{
					Width = tex.Width,
					Height = tex.Height,
					Depth = 1
				},
				MipLevels = 1,
				ArrayLayers = 1,
				Samples = VkSampleCountFlags._1Bit,
				Tiling = VkImageTiling.Optimal,
				Usage = (VkImageUsageFlags.TransferDstBit | VkImageUsageFlags.SampledBit),
				SharingMode = VkSharingMode.Exclusive,
				InitialLayout = VkImageLayout.Undefined
			});
			GpuMemoryTracker.Add(GpuMemoryCategory.UITexture, in allocation, "UI/ImGui", "ImGui.FontAtlas");
			ptr.Value.Image = allocation.Image;
			ptr.Value.Allocation = allocation.MemAllocationInfo.AllocHandle;
			ptr.Value.ImageView = Device.CreateImageView(new VkImageViewCreateInfo
			{
				Image = ptr.Value.Image,
				ViewType = VkImageViewType._2D,
				Format = VkFormat.R8G8B8A8UNorm,
				SubresourceRange = new VkImageSubresourceRange
				{
					AspectMask = VkImageAspectFlags.ColorBit,
					LevelCount = 1,
					LayerCount = 1
				}
			}, null);
			ptr.Value.DescriptorSet = AddTextureImpl(_texSampler, ptr.Value.ImageView, VkImageLayout.ShaderReadOnlyOptimal);
			tex.SetTexID(new ImTextureID(ptr.Value.DescriptorSet.VkHandle));
			tex.BackendUserData = (nint)ptr;
		}
		ImTextureStatus status = tex.Status;
		if ((uint)(status - 2) <= 1u)
		{
			BackendTexture* backendUserData = (BackendTexture*)tex.BackendUserData;
			int x = ((tex.Status != ImTextureStatus.WantCreate) ? tex.UpdateRect.x : 0);
			int num = ((tex.Status != ImTextureStatus.WantCreate) ? tex.UpdateRect.y : 0);
			int num2 = ((tex.Status == ImTextureStatus.WantCreate) ? tex.Width : tex.UpdateRect.w);
			int num3 = ((tex.Status == ImTextureStatus.WantCreate) ? tex.Height : tex.UpdateRect.h);
			ByteSize byteSize = ByteSize.Of(num2 * tex.BytesPerPixel);
			ByteSize byteSize2 = ByteSize.Of((ulong)(num3 * byteSize));
			VmaAllocator allocator = GraphicsDevice.Allocator;
			BufferAllocationInfo buffer = ImGuiGpuMemory.CreateHostBuffer(allocator, "ImGui.FontAtlasUpload", byteSize2, VkBufferUsageFlags.TransferSrcBit, GpuMemoryCategory.Staging);
			VkBuffer vkBuffer = buffer.VkBuffer;
			Ptr ptr2 = ImGuiGpuMemory.Map(allocator, in buffer);
			for (int i = 0; i < num3; i++)
			{
				Mem.Copy((void*)tex.GetPixelsAt(x, num + i), ptr2.Offset(byteSize * i), byteSize);
			}
			ImGuiGpuMemory.Unmap(allocator, in buffer);
			Device.ResetCommandPool(_texCommandPool, VkCommandPoolResetFlags.None);
			_texCommandBuffer.Begin(new VkCommandBufferBeginInfo
			{
				Flags = VkCommandBufferUsageFlags.OneTimeSubmitBit
			});
			CommandBuffer texCommandBuffer = _texCommandBuffer;
			VkBufferMemoryBarrier reference = new VkBufferMemoryBarrier
			{
				SrcAccessMask = VkAccessFlags.HostWriteBit,
				DstAccessMask = VkAccessFlags.TransferReadBit,
				SrcQueueFamilyIndex = -1,
				DstQueueFamilyIndex = -1,
				Buffer = vkBuffer,
				Offset = ByteSize.Zero,
				Size = byteSize2
			};
			ReadOnlySpan<VkBufferMemoryBarrier> pBufferMemoryBarriers = new ReadOnlySpan<VkBufferMemoryBarrier>(in reference);
			VkImageMemoryBarrier reference2 = new VkImageMemoryBarrier
			{
				DstAccessMask = VkAccessFlags.TransferWriteBit,
				OldLayout = ((tex.Status != ImTextureStatus.WantCreate) ? VkImageLayout.ShaderReadOnlyOptimal : VkImageLayout.Undefined),
				NewLayout = VkImageLayout.TransferDstOptimal,
				SrcQueueFamilyIndex = -1,
				DstQueueFamilyIndex = -1,
				Image = backendUserData->Image,
				SubresourceRange = new VkImageSubresourceRange
				{
					AspectMask = VkImageAspectFlags.ColorBit,
					LevelCount = 1,
					LayerCount = 1
				}
			};
			texCommandBuffer.PipelineBarrier(VkPipelineStageFlags.FragmentShaderBit | VkPipelineStageFlags.HostBit, VkPipelineStageFlags.TransferBit, VkDependencyFlags.None, default(ReadOnlySpan<VkMemoryBarrier>), pBufferMemoryBarriers, new ReadOnlySpan<VkImageMemoryBarrier>(in reference2));
			CommandBuffer texCommandBuffer2 = _texCommandBuffer;
			VkImage image = backendUserData->Image;
			VkBufferImageCopy reference3 = new VkBufferImageCopy
			{
				ImageSubresource = new VkImageSubresourceLayers
				{
					AspectMask = VkImageAspectFlags.ColorBit,
					LayerCount = 1
				},
				ImageExtent = new VkExtent3D
				{
					Width = num2,
					Height = num3,
					Depth = 1
				},
				ImageOffset = new VkOffset3D
				{
					X = x,
					Y = num,
					Z = 0
				}
			};
			texCommandBuffer2.CopyBufferToImage(vkBuffer, image, VkImageLayout.TransferDstOptimal, new ReadOnlySpan<VkBufferImageCopy>(in reference3));
			CommandBuffer texCommandBuffer3 = _texCommandBuffer;
			VkImageMemoryBarrier reference4 = new VkImageMemoryBarrier
			{
				SrcAccessMask = VkAccessFlags.TransferWriteBit,
				DstAccessMask = VkAccessFlags.ShaderReadBit,
				OldLayout = VkImageLayout.TransferDstOptimal,
				NewLayout = VkImageLayout.ShaderReadOnlyOptimal,
				SrcQueueFamilyIndex = -1,
				DstQueueFamilyIndex = -1,
				Image = backendUserData->Image,
				SubresourceRange = new VkImageSubresourceRange
				{
					AspectMask = VkImageAspectFlags.ColorBit,
					LevelCount = 1,
					LayerCount = 1
				}
			};
			texCommandBuffer3.PipelineBarrier(VkPipelineStageFlags.TransferBit, VkPipelineStageFlags.FragmentShaderBit, VkDependencyFlags.None, default(ReadOnlySpan<VkMemoryBarrier>), default(ReadOnlySpan<VkBufferMemoryBarrier>), new ReadOnlySpan<VkImageMemoryBarrier>(in reference4));
			VkFence vkFence = Device.CreateFence(VkFenceCreateFlags.None, null);
			_texCommandBuffer.End();
			GraphicsDevice graphicsDevice = GraphicsDevice;
			CommandBuffer reference5 = _texCommandBuffer;
			graphicsDevice.SubmitGraphics(default(Span<VkSemaphore>), default(Span<VkPipelineStageFlags>), new Span<CommandBuffer>(ref reference5), default(Span<VkSemaphore>), vkFence);
			Device.WaitForFence(vkFence, -1);
			Device.DestroyFence(vkFence, null);
			ImGuiGpuMemory.Free(allocator, ref buffer, GpuMemoryCategory.Staging);
			tex.SetStatus(ImTextureStatus.OK);
		}
		if (tex.Status == ImTextureStatus.WantDestroy && tex.UnusedFrames >= ImageCount && _frameNumber - tex.UnusedFrames < Volatile.Read(in _gpuDoneFrame))
		{
			DestroyTexture(tex);
		}
	}

	private unsafe void DestroyTexture(ImTextureDataPtr tex)
	{
		BackendTexture* backendUserData = (BackendTexture*)tex.BackendUserData;
		if (backendUserData != null)
		{
			RemoveTextureImpl(backendUserData->DescriptorSet);
			Device.DestroyImageView(backendUserData->ImageView, null);
			ImageAllocationInfo allocation = new ImageAllocationInfo
			{
				Image = backendUserData->Image
			};
			GpuMemoryTracker.Remove(GpuMemoryCategory.UITexture, in allocation);
			GraphicsDevice.Allocator.DestroyImage(backendUserData->Image, new VmaAllocation(backendUserData->Allocation));
			Mem.Free(backendUserData);
			tex.SetTexID(ImTextureID.Invalid);
			tex.SetStatus(ImTextureStatus.Destroyed);
			tex.BackendUserData = 0;
		}
	}

	public VkSampler SamplerFor(string? sourcePath)
	{
		if (!ImportDefaults.IsOfficialUiTexture(sourcePath))
		{
			return _texSamplerMipped;
		}
		return _texSampler;
	}

	public bool TryGetTexRef(GpuTextureAllocation allocation, string? sourcePath, out ImTextureRef texRef)
	{
		texRef = default(ImTextureRef);
		if (allocation.IsDisposed || allocation.ImageView.IsNull())
		{
			return false;
		}
		allocation.MarkUsed();
		bool exists;
		ref UserTexture valueRefOrAddDefault = ref CollectionsMarshal.GetValueRefOrAddDefault(_userTextures, allocation, out exists);
		if (!exists)
		{
			valueRefOrAddDefault.DescriptorSet = AddTextureImpl(SamplerFor(sourcePath), allocation.ImageView, VkImageLayout.ShaderReadOnlyOptimal);
		}
		valueRefOrAddDefault.LastUsedFrame = _frameNumber;
		texRef = new ImTextureRef
		{
			_TexID = (ImTextureID)valueRefOrAddDefault.DescriptorSet.VkHandle
		};
		return true;
	}

	private void ReleaseIdleUserTextures()
	{
		int num = Volatile.Read(in _gpuDoneFrame);
		foreach (var (gpuTextureAllocation2, userTexture2) in _userTextures)
		{
			if (userTexture2.LastUsedFrame <= num && (gpuTextureAllocation2.IsDisposed || _frameNumber - userTexture2.LastUsedFrame > ImageCount))
			{
				_idleUserTextures.Add(gpuTextureAllocation2);
			}
		}
		foreach (GpuTextureAllocation idleUserTexture in _idleUserTextures)
		{
			RemoveTextureImpl(_userTextures[idleUserTexture].DescriptorSet);
			_userTextures.Remove(idleUserTexture);
		}
		_idleUserTextures.Clear();
	}

	private DescriptorPoolEx AcquireDescriptorPool()
	{
		for (int num = _descriptorPools.Count - 1; num >= 0; num--)
		{
			if (_descriptorPools[num].RemainingSets > 0)
			{
				return _descriptorPools[num];
			}
		}
		DescriptorPoolEx descriptorPoolEx = CreateDescriptorPool();
		_descriptorPools.Add(descriptorPoolEx);
		return descriptorPoolEx;
	}

	private unsafe VkDescriptorSet AddTextureImpl(VkSampler sampler, VkImageView imageView, VkImageLayout imageLayout)
	{
		VkDescriptorSet vkDescriptorSet = Device.AllocateDescriptorSet(AcquireDescriptorPool(), _descriptorSetLayout);
		VkDescriptorImageInfo vkDescriptorImageInfo = new VkDescriptorImageInfo();
		vkDescriptorImageInfo.Sampler = sampler;
		vkDescriptorImageInfo.ImageView = imageView;
		vkDescriptorImageInfo.ImageLayout = imageLayout;
		VkDescriptorImageInfo vkDescriptorImageInfo2 = vkDescriptorImageInfo;
		DeviceEx device = Device;
		VkWriteDescriptorSet reference = new VkWriteDescriptorSet
		{
			DstSet = vkDescriptorSet,
			DescriptorCount = 1,
			DescriptorType = VkDescriptorType.CombinedImageSampler,
			ImageInfo = &vkDescriptorImageInfo2
		};
		device.UpdateDescriptorSets(new ReadOnlySpan<VkWriteDescriptorSet>(in reference), default(ReadOnlySpan<VkCopyDescriptorSet>));
		return vkDescriptorSet;
	}

	private void RemoveTextureImpl(VkDescriptorSet descriptorSet)
	{
		foreach (DescriptorPoolEx descriptorPool in _descriptorPools)
		{
			if (descriptorPool.MappedDescriptorSets.ContainsKey(descriptorSet))
			{
				DeviceEx device = Device;
				VkDescriptorSet reference = descriptorSet;
				device.FreeDescriptorSets(descriptorPool, new Span<VkDescriptorSet>(ref reference));
				break;
			}
		}
	}

	private VkSampler CreateSampler(float maxLod)
	{
		DeviceEx device = Device;
		VkSamplerCreateInfo pCreateInfo = new VkSamplerCreateInfo
		{
			MagFilter = VkFilter.Linear,
			MinFilter = VkFilter.Linear,
			MipmapMode = VkSamplerMipmapMode.Linear,
			AddressModeU = VkSamplerAddressMode.ClampToEdge,
			AddressModeV = VkSamplerAddressMode.ClampToEdge,
			AddressModeW = VkSamplerAddressMode.ClampToEdge,
			MinLod = 0f,
			MaxLod = maxLod,
			MaxAnisotropy = 1f
		};
		return device.CreateSampler(in pCreateInfo, null);
	}

	private DescriptorSetLayoutEx CreateDescriptorSetLayout()
	{
		VkDescriptorSetLayoutBinding vkDescriptorSetLayoutBinding = new VkDescriptorSetLayoutBinding();
		vkDescriptorSetLayoutBinding.DescriptorType = VkDescriptorType.CombinedImageSampler;
		vkDescriptorSetLayoutBinding.DescriptorCount = 1;
		vkDescriptorSetLayoutBinding.StageFlags = VkShaderStageFlags.FragmentBit;
		VkDescriptorSetLayoutBinding vkDescriptorSetLayoutBinding2 = vkDescriptorSetLayoutBinding;
		DeviceEx device = Device;
		DescriptorSetLayoutEx.CreateInfo createInfo = default(DescriptorSetLayoutEx.CreateInfo);
		VkDescriptorSetLayoutBinding reference = vkDescriptorSetLayoutBinding2;
		createInfo.Bindings = new Span<VkDescriptorSetLayoutBinding>(ref reference);
		return device.CreateDescriptorSetLayout(createInfo, null);
	}

	private DescriptorPoolEx CreateDescriptorPool()
	{
		Assert(DescriptorPoolSize >= 8, $"ImGui descriptor pool size needs to be at least {8}.");
		VkDescriptorPoolSize vkDescriptorPoolSize = new VkDescriptorPoolSize();
		vkDescriptorPoolSize.Type = VkDescriptorType.CombinedImageSampler;
		vkDescriptorPoolSize.DescriptorCount = DescriptorPoolSize;
		VkDescriptorPoolSize vkDescriptorPoolSize2 = vkDescriptorPoolSize;
		DeviceEx device = Device;
		DescriptorPoolEx.CreateInfo createInfo = new DescriptorPoolEx.CreateInfo
		{
			Flags = VkDescriptorPoolCreateFlags.FreeDescriptorSetBit,
			MaxSets = DescriptorPoolSize
		};
		VkDescriptorPoolSize reference = vkDescriptorPoolSize2;
		createInfo.PoolSizes = new Span<VkDescriptorPoolSize>(ref reference);
		return device.CreateDescriptorPool(createInfo, null);
	}

	private VkPipelineLayout CreatePipelineLayout()
	{
		VkPushConstantRange vkPushConstantRange = new VkPushConstantRange();
		vkPushConstantRange.StageFlags = VkShaderStageFlags.VertexBit;
		vkPushConstantRange.Offset = ByteSize.Zero;
		vkPushConstantRange.Size = ByteSize.Of<float>(4);
		VkPushConstantRange vkPushConstantRange2 = vkPushConstantRange;
		DeviceEx device = Device;
		VkDescriptorSetLayout reference = _descriptorSetLayout;
		ReadOnlySpan<VkDescriptorSetLayout> setLayouts = new ReadOnlySpan<VkDescriptorSetLayout>(in reference);
		VkPushConstantRange reference2 = vkPushConstantRange2;
		return device.CreatePipelineLayout(setLayouts, new ReadOnlySpan<VkPushConstantRange>(in reference2), null);
	}

	private void CreateCommandBuffer(out VkCommandPool texCommandPool, out CommandBuffer texCommandBuffer)
	{
		DeviceEx device = Device;
		VkCommandPoolCreateInfo pCreateInfo = new VkCommandPoolCreateInfo
		{
			QueueFamilyIndex = GraphicsDevice.GraphicsQueue.Family
		};
		texCommandPool = device.CreateCommandPool(in pCreateInfo, null);
		texCommandBuffer = Device.AllocateCommandBuffer(new VkCommandBufferAllocateInfo
		{
			CommandPool = texCommandPool,
			Level = VkCommandBufferLevel.Primary
		});
	}

	private unsafe VkPipeline CreatePipeline(VkRenderPass renderPass, VkSampleCountFlags sampleCount)
	{
		ReadOnlySpan<byte> span = "main"u8;
		InlineArray2<VkPipelineShaderStageCreateInfo> buffer = default(InlineArray2<VkPipelineShaderStageCreateInfo>);
		global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray2<VkPipelineShaderStageCreateInfo>, VkPipelineShaderStageCreateInfo>(ref buffer, 0) = new VkPipelineShaderStageCreateInfo
		{
			Stage = VkShaderStageFlags.VertexBit,
			Module = _shaderModuleVert,
			Name = span.AsPointer()
		};
		global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray2<VkPipelineShaderStageCreateInfo>, VkPipelineShaderStageCreateInfo>(ref buffer, 1) = new VkPipelineShaderStageCreateInfo
		{
			Stage = VkShaderStageFlags.FragmentBit,
			Module = _shaderModuleFrag,
			Name = span.AsPointer()
		};
		Span<VkPipelineShaderStageCreateInfo> span2 = global::_003CPrivateImplementationDetails_003E.InlineArrayAsSpan<InlineArray2<VkPipelineShaderStageCreateInfo>, VkPipelineShaderStageCreateInfo>(ref buffer, 2);
		VkVertexInputBindingDescription vkVertexInputBindingDescription = new VkVertexInputBindingDescription();
		vkVertexInputBindingDescription.Stride = ByteSize.Of<ImDrawVert>();
		vkVertexInputBindingDescription.InputRate = VkVertexInputRate.Vertex;
		VkVertexInputBindingDescription vkVertexInputBindingDescription2 = vkVertexInputBindingDescription;
		InlineArray3<VkVertexInputAttributeDescription> buffer2 = default(InlineArray3<VkVertexInputAttributeDescription>);
		global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray3<VkVertexInputAttributeDescription>, VkVertexInputAttributeDescription>(ref buffer2, 0) = new VkVertexInputAttributeDescription
		{
			Location = 0,
			Binding = vkVertexInputBindingDescription2.Binding,
			Format = VkFormat.R32G32SFloat,
			Offset = ByteSize.OffsetOf<ImDrawVert>("pos")
		};
		global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray3<VkVertexInputAttributeDescription>, VkVertexInputAttributeDescription>(ref buffer2, 1) = new VkVertexInputAttributeDescription
		{
			Location = 1,
			Binding = vkVertexInputBindingDescription2.Binding,
			Format = VkFormat.R32G32SFloat,
			Offset = ByteSize.OffsetOf<ImDrawVert>("uv")
		};
		global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray3<VkVertexInputAttributeDescription>, VkVertexInputAttributeDescription>(ref buffer2, 2) = new VkVertexInputAttributeDescription
		{
			Location = 2,
			Binding = vkVertexInputBindingDescription2.Binding,
			Format = VkFormat.R8G8B8A8UNorm,
			Offset = ByteSize.OffsetOf<ImDrawVert>("col")
		};
		Span<VkVertexInputAttributeDescription> span3 = global::_003CPrivateImplementationDetails_003E.InlineArrayAsSpan<InlineArray3<VkVertexInputAttributeDescription>, VkVertexInputAttributeDescription>(ref buffer2, 3);
		VkPipelineVertexInputStateCreateInfo vkPipelineVertexInputStateCreateInfo = new VkPipelineVertexInputStateCreateInfo();
		vkPipelineVertexInputStateCreateInfo.VertexBindingDescriptionCount = 1;
		vkPipelineVertexInputStateCreateInfo.VertexBindingDescriptions = &vkVertexInputBindingDescription2;
		vkPipelineVertexInputStateCreateInfo.VertexAttributeDescriptionCount = 3;
		vkPipelineVertexInputStateCreateInfo.VertexAttributeDescriptions = span3.AsPointer();
		VkPipelineVertexInputStateCreateInfo vkPipelineVertexInputStateCreateInfo2 = vkPipelineVertexInputStateCreateInfo;
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
		VkPipelineColorBlendAttachmentState vkPipelineColorBlendAttachmentState = new VkPipelineColorBlendAttachmentState();
		vkPipelineColorBlendAttachmentState.BlendEnable = true;
		vkPipelineColorBlendAttachmentState.SrcColorBlendFactor = VkBlendFactor.SrcAlpha;
		vkPipelineColorBlendAttachmentState.DstColorBlendFactor = VkBlendFactor.OneMinusSrcAlpha;
		vkPipelineColorBlendAttachmentState.ColorBlendOp = VkBlendOp.Add;
		vkPipelineColorBlendAttachmentState.SrcAlphaBlendFactor = VkBlendFactor.One;
		vkPipelineColorBlendAttachmentState.DstAlphaBlendFactor = VkBlendFactor.OneMinusSrcAlpha;
		vkPipelineColorBlendAttachmentState.AlphaBlendOp = VkBlendOp.Add;
		vkPipelineColorBlendAttachmentState.ColorWriteMask = VkColorComponentFlags.RBit | VkColorComponentFlags.GBit | VkColorComponentFlags.BBit | VkColorComponentFlags.ABit;
		VkPipelineColorBlendAttachmentState vkPipelineColorBlendAttachmentState2 = vkPipelineColorBlendAttachmentState;
		VkPipelineDepthStencilStateCreateInfo vkPipelineDepthStencilStateCreateInfo = new VkPipelineDepthStencilStateCreateInfo();
		VkPipelineColorBlendStateCreateInfo vkPipelineColorBlendStateCreateInfo = new VkPipelineColorBlendStateCreateInfo();
		vkPipelineColorBlendStateCreateInfo.AttachmentCount = 1;
		vkPipelineColorBlendStateCreateInfo.Attachments = &vkPipelineColorBlendAttachmentState2;
		VkPipelineColorBlendStateCreateInfo vkPipelineColorBlendStateCreateInfo2 = vkPipelineColorBlendStateCreateInfo;
		InlineArray2<VkDynamicState> buffer3 = default(InlineArray2<VkDynamicState>);
		global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray2<VkDynamicState>, VkDynamicState>(ref buffer3, 0) = VkDynamicState.Viewport;
		global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray2<VkDynamicState>, VkDynamicState>(ref buffer3, 1) = VkDynamicState.Scissor;
		Span<VkDynamicState> span4 = global::_003CPrivateImplementationDetails_003E.InlineArrayAsSpan<InlineArray2<VkDynamicState>, VkDynamicState>(ref buffer3, 2);
		VkPipelineDynamicStateCreateInfo vkPipelineDynamicStateCreateInfo = new VkPipelineDynamicStateCreateInfo();
		vkPipelineDynamicStateCreateInfo.DynamicStateCount = span4.Length;
		vkPipelineDynamicStateCreateInfo.DynamicStates = span4.AsPointer();
		VkPipelineDynamicStateCreateInfo vkPipelineDynamicStateCreateInfo2 = vkPipelineDynamicStateCreateInfo;
		DeviceEx device = Device;
		VkGraphicsPipelineCreateInfo reference = new VkGraphicsPipelineCreateInfo
		{
			Flags = VkPipelineCreateFlags.None,
			StageCount = 2,
			Stages = span2.AsPointer(),
			VertexInputState = &vkPipelineVertexInputStateCreateInfo2,
			InputAssemblyState = &vkPipelineInputAssemblyStateCreateInfo2,
			ViewportState = &vkPipelineViewportStateCreateInfo2,
			RasterizationState = &vkPipelineRasterizationStateCreateInfo2,
			MultisampleState = &vkPipelineMultisampleStateCreateInfo2,
			DepthStencilState = &vkPipelineDepthStencilStateCreateInfo,
			ColorBlendState = &vkPipelineColorBlendStateCreateInfo2,
			DynamicState = &vkPipelineDynamicStateCreateInfo2,
			Layout = _pipelineLayout,
			RenderPass = renderPass,
			Subpass = SubPass
		};
		VkPipeline result = default(VkPipeline);
		device.CreateGraphicsPipelines(default(VkPipelineCache), new ReadOnlySpan<VkGraphicsPipelineCreateInfo>(in reference), null, new ReadOnlySpan<VkPipeline>(&result, 1));
		return result;
	}

	private unsafe void CreateShaderModules(out VkShaderModule vert, out VkShaderModule frag)
	{
		fixed (uint* vert2 = ImGuiShaders.Vert)
		{
			DeviceEx device = Device;
			VkShaderModuleCreateInfo pCreateInfo = new VkShaderModuleCreateInfo
			{
				CodeSize = ByteSize.Of(ImGuiShaders.Vert),
				Code = (int*)vert2
			};
			vert = device.CreateShaderModule(in pCreateInfo, null);
		}
		fixed (uint* frag2 = ImGuiShaders.Frag)
		{
			DeviceEx device2 = Device;
			VkShaderModuleCreateInfo pCreateInfo = new VkShaderModuleCreateInfo
			{
				CodeSize = ByteSize.Of(ImGuiShaders.Frag),
				Code = (int*)frag2
			};
			frag = device2.CreateShaderModule(in pCreateInfo, null);
		}
	}

	private void DestroyFrameBuffers(ref FrameBuffers buffers)
	{
		ImGuiGpuMemory.Free(GraphicsDevice.Allocator, ref buffers.VertexBuffer, GpuMemoryCategory.DynamicDraw);
		ImGuiGpuMemory.Free(GraphicsDevice.Allocator, ref buffers.IndexBuffer, GpuMemoryCategory.DynamicDraw);
	}

	public void Dispose()
	{
		if (_disposed)
		{
			return;
		}
		_disposed = true;
		_materialRenderer.Dispose();
		foreach (var value in _offscreenTargets.Values)
		{
			value.Materials.Dispose();
		}
		_offscreenShaderModules.Dispose();
		foreach (ImTextureDataPtr texture in ImGui.GetPlatformIO().Textures)
		{
			if (texture.RefCount == 1)
			{
				DestroyTexture(texture);
			}
		}
		for (int i = 0; i < _frames.Length; i++)
		{
			DestroyFrameBuffers(ref _frames[i]);
		}
		DestroyFrameBuffers(ref _offscreenBuffers);
		DeviceEx device = Device;
		VkCommandPool texCommandPool = _texCommandPool;
		CommandBuffer reference = _texCommandBuffer;
		device.FreeCommandBuffers(texCommandPool, new ReadOnlySpan<CommandBuffer>(in reference));
		Device.DestroyCommandPool(_texCommandPool, null);
		Device.DestroySampler(_texSampler, null);
		Device.DestroySampler(_texSamplerMipped, null);
		Device.DestroyShaderModule(_shaderModuleVert, null);
		Device.DestroyShaderModule(_shaderModuleFrag, null);
		Device.DestroyDescriptorSetLayout(_descriptorSetLayout, null);
		Device.DestroyPipelineLayout(_pipelineLayout, null);
		Device.DestroyPipeline(_pipeline, null);
		foreach (var value2 in _offscreenTargets.Values)
		{
			Device.DestroyPipeline(value2.Pipeline, null);
		}
		_offscreenTargets.Clear();
		foreach (DescriptorPoolEx descriptorPool in _descriptorPools)
		{
			Device.DestroyDescriptorPool(descriptorPool, null);
		}
		ImGuiIOPtr iO = ImGui.GetIO();
		iO.BackendRendererName = null;
		iO.BackendFlags &= ~(ImGuiBackendFlags.RendererHasVtxOffset | ImGuiBackendFlags.RendererHasTextures);
	}

	private static void Assert(bool condition, string message = "")
	{
		if (!condition)
		{
			throw new Exception(message);
		}
	}
}
