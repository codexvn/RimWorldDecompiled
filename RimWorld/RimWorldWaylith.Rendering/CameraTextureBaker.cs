using System;
using System.Collections.Generic;
using System.Numerics;
using Brutal;
using Brutal.Numerics;
using Brutal.Pointers;
using Brutal.VulkanApi;
using Brutal.VulkanApi.Abstractions;
using Brutal.VulkanApi.Vma;
using UnityEngine;
using UnityStubs.Rendering;
using Waylith.Graphics;
using Waylith.Rendering;

namespace RimWorldWaylith.Rendering;

public sealed class CameraTextureBaker : ICameraTextureRenderer, IDisposable
{
	private sealed class TargetResources
	{
		public GpuTextureAllocation Color;

		public ImageAllocationInfo Depth;

		public VkImageView DepthView;

		public VkFramebuffer Framebuffer;

		public VkRenderPass RenderPass;

		public VkRenderPass ClearRenderPass;

		public int Width;

		public int Height;
	}

	private readonly GraphicsDevice _graphicsDevice;

	private readonly DeviceEx _device;

	private readonly VmaAllocator _allocator;

	private readonly IRenderContext _renderContext;

	private readonly MeshBatchRenderer _renderer;

	private readonly Dictionary<(VkFormat, VkAttachmentLoadOp), VkRenderPass> _renderPasses = new Dictionary<(VkFormat, VkAttachmentLoadOp), VkRenderPass>();

	private readonly object _sync = new object();

	private readonly Dictionary<UnityEngine.RenderTexture, TargetResources> _targets = new Dictionary<UnityEngine.RenderTexture, TargetResources>(ReferenceEqualityComparer.Instance);

	private VkCommandPool _commandPool;

	private CommandBuffer _commandBuffer;

	private VkFence _fence;

	private int _frameNumber;

	private bool _disposed;

	public event Action<UnityEngine.RenderTexture>? BeforeReadPixels;

	public CameraTextureBaker(GraphicsDevice graphicsDevice, IRenderContext renderContext)
	{
		_graphicsDevice = graphicsDevice;
		_device = graphicsDevice.Device;
		_allocator = graphicsDevice.Allocator;
		_renderContext = renderContext;
		_renderer = new MeshBatchRenderer(graphicsDevice, renderContext);
		DeviceEx device = _device;
		VkCommandPoolCreateInfo pCreateInfo = new VkCommandPoolCreateInfo
		{
			QueueFamilyIndex = graphicsDevice.GraphicsFamilyIndex,
			Flags = VkCommandPoolCreateFlags.ResetCommandBufferBit
		};
		_commandPool = device.CreateCommandPool(in pCreateInfo, _device.VkAllocator);
		_commandBuffer = _device.AllocateCommandBuffer(new VkCommandBufferAllocateInfo
		{
			CommandPool = _commandPool,
			Level = VkCommandBufferLevel.Primary
		});
		_fence = _device.CreateFence(new VkFenceCreateInfo(), _device.VkAllocator);
	}

	public void Render(UnityEngine.RenderTexture target, IReadOnlyList<MeshDrawCommand> commands, CameraView view, Rect viewportRect, UnityEngine.Color? clearColor)
	{
		if (target == null)
		{
			return;
		}
		lock (_sync)
		{
			if (_disposed)
			{
				return;
			}
			DrawItemFactory drawItemMaker = MeshDrawSubmission.DrawItemMaker;
			if (drawItemMaker == null)
			{
				return;
			}
			GpuTextureService.FlushPendingTextureCommands();
			_graphicsDevice.MeshUploads.DrainPendingUploads();
			DrawPropertyArena activeArena = MeshDrawSubmission.ActiveArena;
			string globalKeywordKey = UnityEngine.Shader.GlobalKeywordKey;
			List<FinalDrawItem> list = new List<FinalDrawItem>(commands.Count);
			for (int i = 0; i < commands.Count; i++)
			{
				FinalDrawItem finalDrawItem = drawItemMaker.TryMake(commands[i], activeArena, globalKeywordKey, i, null, 0);
				if (finalDrawItem != null)
				{
					list.Add(finalDrawItem);
				}
			}
			TargetResources orCreateTarget = GetOrCreateTarget(target);
			if (orCreateTarget == null)
			{
				ReturnItems(list);
				return;
			}
			VkRect2D area = ResolveArea(viewportRect, orCreateTarget.Width, orCreateTarget.Height);
			if (area.Extent.Width == 0 || area.Extent.Height == 0)
			{
				ReturnItems(list);
				return;
			}
			RecordAndSubmit(orCreateTarget, list, view, clearColor.GetValueOrDefault(), area, clearColor.HasValue, null);
			ReturnItems(list);
		}
	}

	public void RenderItems(UnityEngine.RenderTexture target, List<FinalDrawItem> items, Camera camera, Rect viewportRect, UnityEngine.Color? clearColor, System.Numerics.Vector3? sunDirection = null)
	{
		if (target == null || camera == null || items.Count == 0)
		{
			return;
		}
		lock (_sync)
		{
			if (_disposed)
			{
				return;
			}
			GpuTextureService.FlushPendingTextureCommands();
			_graphicsDevice.MeshUploads.DrainPendingUploads();
			TargetResources orCreateTarget = GetOrCreateTarget(target);
			if (orCreateTarget != null)
			{
				VkRect2D area = ResolveArea(viewportRect, orCreateTarget.Width, orCreateTarget.Height);
				if (area.Extent.Width != 0 && area.Extent.Height != 0)
				{
					CameraView view = CameraViewFactory.GetView(camera);
					RecordAndSubmit(orCreateTarget, items, view, clearColor.GetValueOrDefault(), area, clearColor.HasValue, sunDirection);
				}
			}
		}
	}

	public unsafe bool TryReadPixels(UnityEngine.RenderTexture source, out byte[] rgbaTopDown)
	{
		rgbaTopDown = Array.Empty<byte>();
		if (source == null)
		{
			return false;
		}
		this.BeforeReadPixels?.Invoke(source);
		lock (_sync)
		{
			if (_disposed || !source.Create() || source.GpuTexture == null)
			{
				return false;
			}
			GpuTextureAllocation gpuTexture = source.GpuTexture;
			VkFormat format = gpuTexture.Format;
			if ((format != 0 && format != VkFormat.R8G8B8A8UNorm && format != VkFormat.R8G8B8A8SRGB) || 1 == 0)
			{
				return false;
			}
			int width = source.width;
			int height = source.height;
			long num = (long)width * (long)height * 4;
			VmaAllocator allocator = _allocator;
			string name = "ReadPixelsStaging";
			VkBufferCreateInfo bufferCreateInfo = new VkBufferCreateInfo
			{
				Size = ByteSize.Of(num),
				Usage = VkBufferUsageFlags.TransferDstBit,
				SharingMode = VkSharingMode.Exclusive
			};
			AllocationInfo.CreateInfo allocateCreateInfo = new AllocationInfo.CreateInfo
			{
				Preference = MemoryPreference.PreferCpu,
				RequiredProperties = (VkMemoryPropertyFlags.HostVisibleBit | VkMemoryPropertyFlags.HostCoherentBit),
				PreferredProperties = (VkMemoryPropertyFlags.HostVisibleBit | VkMemoryPropertyFlags.HostCoherentBit),
				Priority = 1f
			};
			BufferAllocationInfo inAllocation = allocator.AllocateBuffer(in name, in bufferCreateInfo, in allocateCreateInfo);
			try
			{
				VkImageSubresourceRange vkImageSubresourceRange = new VkImageSubresourceRange();
				vkImageSubresourceRange.AspectMask = VkImageAspectFlags.ColorBit;
				vkImageSubresourceRange.BaseMipLevel = 0;
				vkImageSubresourceRange.LevelCount = 1;
				vkImageSubresourceRange.BaseArrayLayer = 0;
				vkImageSubresourceRange.LayerCount = 1;
				VkImageSubresourceRange subresourceRange = vkImageSubresourceRange;
				VkImageLayout oldLayout = ((gpuTexture.CurrentLayout == VkImageLayout.Undefined) ? VkImageLayout.ShaderReadOnlyOptimal : gpuTexture.CurrentLayout);
				_commandBuffer.Begin(VkCommandBufferUsageFlags.OneTimeSubmitBit);
				CommandBuffer commandBuffer = _commandBuffer;
				VkImageMemoryBarrier reference = new VkImageMemoryBarrier
				{
					SrcAccessMask = (VkAccessFlags.ShaderReadBit | VkAccessFlags.ColorAttachmentWriteBit),
					DstAccessMask = VkAccessFlags.TransferReadBit,
					OldLayout = oldLayout,
					NewLayout = VkImageLayout.TransferSrcOptimal,
					SrcQueueFamilyIndex = -1,
					DstQueueFamilyIndex = -1,
					Image = gpuTexture.Image,
					SubresourceRange = subresourceRange
				};
				commandBuffer.PipelineBarrier(VkPipelineStageFlags.FragmentShaderBit | VkPipelineStageFlags.ColorAttachmentOutputBit, VkPipelineStageFlags.TransferBit, VkDependencyFlags.None, default(ReadOnlySpan<VkMemoryBarrier>), default(ReadOnlySpan<VkBufferMemoryBarrier>), new ReadOnlySpan<VkImageMemoryBarrier>(in reference));
				CommandBuffer commandBuffer2 = _commandBuffer;
				VkImage image = gpuTexture.Image;
				VkBuffer vkBuffer = inAllocation.VkBuffer;
				VkBufferImageCopy reference2 = new VkBufferImageCopy
				{
					BufferOffset = ByteSize.Zero,
					BufferRowLength = 0,
					BufferImageHeight = 0,
					ImageSubresource = new VkImageSubresourceLayers
					{
						AspectMask = VkImageAspectFlags.ColorBit,
						MipLevel = 0,
						BaseArrayLayer = 0,
						LayerCount = 1
					},
					ImageOffset = new VkOffset3D(0, 0, 0),
					ImageExtent = new VkExtent3D(width, height, 1)
				};
				commandBuffer2.CopyImageToBuffer(image, VkImageLayout.TransferSrcOptimal, vkBuffer, new ReadOnlySpan<VkBufferImageCopy>(in reference2));
				CommandBuffer commandBuffer3 = _commandBuffer;
				VkImageMemoryBarrier reference3 = new VkImageMemoryBarrier
				{
					SrcAccessMask = VkAccessFlags.TransferReadBit,
					DstAccessMask = VkAccessFlags.ShaderReadBit,
					OldLayout = VkImageLayout.TransferSrcOptimal,
					NewLayout = VkImageLayout.ShaderReadOnlyOptimal,
					SrcQueueFamilyIndex = -1,
					DstQueueFamilyIndex = -1,
					Image = gpuTexture.Image,
					SubresourceRange = subresourceRange
				};
				commandBuffer3.PipelineBarrier(VkPipelineStageFlags.TransferBit, VkPipelineStageFlags.FragmentShaderBit, VkDependencyFlags.None, default(ReadOnlySpan<VkMemoryBarrier>), default(ReadOnlySpan<VkBufferMemoryBarrier>), new ReadOnlySpan<VkImageMemoryBarrier>(in reference3));
				_commandBuffer.End();
				DeviceEx device = _device;
				VkFence reference4 = _fence;
				device.ResetFences(new ReadOnlySpan<VkFence>(in reference4));
				GraphicsDevice graphicsDevice = _graphicsDevice;
				CommandBuffer reference5 = _commandBuffer;
				graphicsDevice.SubmitGraphics(default(Span<VkSemaphore>), default(Span<VkPipelineStageFlags>), new Span<CommandBuffer>(ref reference5), default(Span<VkSemaphore>), _fence);
				_device.WaitForFence(_fence, -1);
				gpuTexture.CurrentLayout = VkImageLayout.ShaderReadOnlyOptimal;
				rgbaTopDown = new byte[num];
				AllocationMapInfo allocationMapInfo = new AllocationMapInfo(inAllocation.AllocationInfo);
				allocationMapInfo.Region = new MapRegion
				{
					Offset = ByteSize.Zero,
					Size = ByteSize.Of(num)
				};
				AllocationMapInfo allocationMapInfo2 = allocationMapInfo;
				Ptr ptr = _allocator.MapMemory(in allocationMapInfo2);
				try
				{
					new ReadOnlySpan<byte>((void*)(nint)ptr, (int)num).CopyTo(rgbaTopDown);
				}
				finally
				{
					_allocator.UnmapMemory(in allocationMapInfo2);
				}
				return true;
			}
			finally
			{
				_allocator.FreeAllocation(in inAllocation);
			}
		}
	}

	public unsafe void RecordIntoTarget(UnityEngine.RenderTexture target, Action<CommandBuffer, VkRenderPass> record)
	{
		if (target == null)
		{
			return;
		}
		lock (_sync)
		{
			if (!_disposed)
			{
				GpuTextureService.FlushPendingTextureCommands();
				TargetResources orCreateTarget = GetOrCreateTarget(target);
				if (orCreateTarget != null)
				{
					VkRect2D vkRect2D = new VkRect2D();
					vkRect2D.Extent = new VkExtent2D
					{
						Width = orCreateTarget.Width,
						Height = orCreateTarget.Height
					};
					VkRect2D renderArea = vkRect2D;
					VkClearValue* ptr = stackalloc VkClearValue[2];
					ptr[1] = new VkClearValue
					{
						DepthStencil = new VkClearDepthStencilValue
						{
							Depth = 0f,
							Stencil = 0
						}
					};
					_commandBuffer.Begin(VkCommandBufferUsageFlags.OneTimeSubmitBit);
					CommandBuffer commandBuffer = _commandBuffer;
					VkRenderPassBeginInfo pRenderPassBegin = new VkRenderPassBeginInfo
					{
						RenderPass = orCreateTarget.RenderPass,
						Framebuffer = orCreateTarget.Framebuffer,
						RenderArea = renderArea,
						ClearValueCount = 2,
						ClearValues = ptr
					};
					commandBuffer.BeginRenderPass(in pRenderPassBegin, VkSubpassContents.Inline);
					record(_commandBuffer, orCreateTarget.RenderPass);
					_commandBuffer.EndRenderPass();
					_commandBuffer.End();
					DeviceEx device = _device;
					VkFence reference = _fence;
					device.ResetFences(new ReadOnlySpan<VkFence>(in reference));
					GraphicsDevice graphicsDevice = _graphicsDevice;
					CommandBuffer reference2 = _commandBuffer;
					graphicsDevice.SubmitGraphics(default(Span<VkSemaphore>), default(Span<VkPipelineStageFlags>), new Span<CommandBuffer>(ref reference2), default(Span<VkSemaphore>), _fence);
					_device.WaitForFence(_fence, -1);
				}
			}
		}
	}

	private static void ReturnItems(List<FinalDrawItem> items)
	{
		for (int i = 0; i < items.Count; i++)
		{
			FinalDrawItem.Return(items[i]);
		}
	}

	private static VkRect2D ResolveArea(Rect rect, int width, int height)
	{
		int value = (int)MathF.Round(rect.x * (float)width);
		int value2 = (int)MathF.Round(rect.width * (float)width);
		int value3 = (int)MathF.Round(rect.height * (float)height);
		int value4 = (int)MathF.Round((1f - rect.y - rect.height) * (float)height);
		value = Math.Clamp(value, 0, width);
		value4 = Math.Clamp(value4, 0, height);
		value2 = Math.Clamp(value2, 0, width - value);
		value3 = Math.Clamp(value3, 0, height - value4);
		VkRect2D result = new VkRect2D();
		result.Offset = new VkOffset2D
		{
			X = value,
			Y = value4
		};
		result.Extent = new VkExtent2D
		{
			Width = value2,
			Height = value3
		};
		return result;
	}

	private unsafe void RecordAndSubmit(TargetResources res, List<FinalDrawItem> items, CameraView view, UnityEngine.Color clearColor, VkRect2D area, bool clearColorEnabled = true, System.Numerics.Vector3? sunDirection = null)
	{
		_commandBuffer.Begin(VkCommandBufferUsageFlags.OneTimeSubmitBit);
		OffscreenRenderContext renderContext = new OffscreenRenderContext(_renderContext, res.RenderPass, new VkExtent2D
		{
			Width = res.Width,
			Height = res.Height
		});
		FrameResources[] frameResources = _renderContext.FrameResources;
		int frameIndex = _renderContext.FrameIndex;
		FrameResources resources = (((uint)frameIndex < (uint)frameResources.Length) ? frameResources[frameIndex] : frameResources[0]);
		FrameContext frameContext = new FrameContext(renderContext, resources, _commandBuffer);
		_renderer.BeginFrame(in frameContext, ++_frameNumber);
		VkClearValue* ptr = stackalloc VkClearValue[2];
		*ptr = new VkClearValue
		{
			Color = new VkClearColorValue
			{
				Float32 = new float4(clearColor.r, clearColor.g, clearColor.b, clearColor.a)
			}
		};
		ptr[1] = new VkClearValue
		{
			DepthStencil = new VkClearDepthStencilValue
			{
				Depth = 0f,
				Stencil = 0
			}
		};
		_commandBuffer.BeginRenderPass(new VkRenderPassBeginInfo
		{
			RenderPass = (clearColorEnabled ? res.ClearRenderPass : res.RenderPass),
			Framebuffer = res.Framebuffer,
			RenderArea = area,
			ClearValueCount = 2,
			ClearValues = ptr
		}, VkSubpassContents.Inline);
		CommandBuffer commandBuffer = _commandBuffer;
		VkViewport reference = new VkViewport
		{
			X = area.Offset.X,
			Y = area.Offset.Y,
			Width = area.Extent.Width,
			Height = area.Extent.Height,
			MinDepth = 0f,
			MaxDepth = 1f
		};
		commandBuffer.SetViewport(0, new ReadOnlySpan<VkViewport>(in reference));
		CommandBuffer commandBuffer2 = _commandBuffer;
		VkRect2D reference2 = area;
		commandBuffer2.SetScissor(0, new ReadOnlySpan<VkRect2D>(in reference2));
		_renderer.RecordBatch(in frameContext, items, view, sunDirection);
		_commandBuffer.EndRenderPass();
		_commandBuffer.End();
		DeviceEx device = _device;
		VkFence reference3 = _fence;
		device.ResetFences(new ReadOnlySpan<VkFence>(in reference3));
		GraphicsDevice graphicsDevice = _graphicsDevice;
		CommandBuffer reference4 = _commandBuffer;
		graphicsDevice.SubmitGraphics(default(Span<VkSemaphore>), default(Span<VkPipelineStageFlags>), new Span<CommandBuffer>(ref reference4), default(Span<VkSemaphore>), _fence);
		_device.WaitForFence(_fence, -1);
	}

	public void ReleaseTarget(UnityEngine.RenderTexture target)
	{
		lock (_sync)
		{
			if (!_disposed && _targets.Remove(target, out TargetResources value))
			{
				DestroyTarget(value);
			}
		}
	}

	private TargetResources? GetOrCreateTarget(UnityEngine.RenderTexture target)
	{
		if (!target.Create() || target.GpuTexture == null)
		{
			return null;
		}
		GpuTextureAllocation gpuTexture = target.GpuTexture;
		if (_targets.TryGetValue(target, out TargetResources value))
		{
			if (value.Color == gpuTexture && value.Width == target.width && value.Height == target.height)
			{
				return value;
			}
			DestroyTarget(value);
			_targets.Remove(target);
		}
		TargetResources targetResources = CreateTarget(target, gpuTexture);
		_targets[target] = targetResources;
		return targetResources;
	}

	private unsafe TargetResources CreateTarget(UnityEngine.RenderTexture target, GpuTextureAllocation gpu)
	{
		int width = target.width;
		int height = target.height;
		string text = (string.IsNullOrWhiteSpace(target.name) ? "CameraTextureBake" : target.name);
		ImageAllocationInfo allocation = _allocator.AllocateImage(text + ".Depth", new VkImageCreateInfo
		{
			ImageType = VkImageType._2D,
			Format = VkFormat.D32SFloatS8UInt,
			Extent = new VkExtent3D(width, height, 1),
			MipLevels = 1,
			ArrayLayers = 1,
			Samples = VkSampleCountFlags._1Bit,
			Tiling = VkImageTiling.Optimal,
			Usage = VkImageUsageFlags.DepthStencilAttachmentBit,
			SharingMode = VkSharingMode.Exclusive,
			InitialLayout = VkImageLayout.Undefined
		}, new AllocationInfo.CreateInfo
		{
			Preference = MemoryPreference.PreferGpu,
			RequiredProperties = VkMemoryPropertyFlags.DeviceLocalBit,
			PreferredProperties = VkMemoryPropertyFlags.DeviceLocalBit,
			Priority = 1f
		});
		GpuMemoryTracker.Add(GpuMemoryCategory.RenderTarget, in allocation, null, text + ".Depth");
		VkImageView vkImageView = _device.CreateImageView(new VkImageViewCreateInfo
		{
			Image = allocation.Image,
			ViewType = VkImageViewType._2D,
			Format = VkFormat.D32SFloatS8UInt,
			SubresourceRange = new VkImageSubresourceRange
			{
				AspectMask = (VkImageAspectFlags.DepthBit | VkImageAspectFlags.StencilBit),
				BaseMipLevel = 0,
				LevelCount = 1,
				BaseArrayLayer = 0,
				LayerCount = 1
			}
		}, _device.VkAllocator);
		VkFormat colorFormat = ((gpu.Format == VkFormat.Undefined) ? VkFormat.R8G8B8A8UNorm : gpu.Format);
		VkRenderPass orCreateRenderPass = GetOrCreateRenderPass(colorFormat, VkAttachmentLoadOp.Load);
		VkRenderPass orCreateRenderPass2 = GetOrCreateRenderPass(colorFormat, VkAttachmentLoadOp.Clear);
		VkImageView* ptr = stackalloc VkImageView[2];
		*ptr = gpu.ImageView;
		ptr[1] = vkImageView;
		VkFramebuffer framebuffer = _device.CreateFramebuffer(new VkFramebufferCreateInfo
		{
			RenderPass = orCreateRenderPass,
			AttachmentCount = 2,
			Attachments = ptr,
			Width = width,
			Height = height,
			Layers = 1
		}, _device.VkAllocator);
		_graphicsDevice.ExecuteOneShot(delegate(CommandBuffer commandBuffer)
		{
			TransitionColor(commandBuffer, gpu, VkImageLayout.TransferDstOptimal, VkAccessFlags.None, VkAccessFlags.TransferWriteBit, VkPipelineStageFlags.TopOfPipeBit, VkPipelineStageFlags.TransferBit);
			VkClearColorValue vkClearColorValue = new VkClearColorValue
			{
				Float32 = new float4(0f, 0f, 0f, 0f)
			};
			VkImageSubresourceRange vkImageSubresourceRange = new VkImageSubresourceRange
			{
				AspectMask = VkImageAspectFlags.ColorBit,
				BaseMipLevel = 0,
				LevelCount = 1,
				BaseArrayLayer = 0,
				LayerCount = 1
			};
			VkImage image = gpu.Image;
			VkClearColorValue* pColor = &vkClearColorValue;
			VkImageSubresourceRange reference = vkImageSubresourceRange;
			commandBuffer.ClearColorImage(image, VkImageLayout.TransferDstOptimal, pColor, new ReadOnlySpan<VkImageSubresourceRange>(in reference));
			TransitionColor(commandBuffer, gpu, VkImageLayout.ShaderReadOnlyOptimal, VkAccessFlags.TransferWriteBit, VkAccessFlags.ShaderReadBit, VkPipelineStageFlags.TransferBit, VkPipelineStageFlags.FragmentShaderBit);
		});
		return new TargetResources
		{
			Color = gpu,
			Depth = allocation,
			DepthView = vkImageView,
			Framebuffer = framebuffer,
			RenderPass = orCreateRenderPass,
			ClearRenderPass = orCreateRenderPass2,
			Width = width,
			Height = height
		};
	}

	private VkRenderPass GetOrCreateRenderPass(VkFormat colorFormat, VkAttachmentLoadOp colorLoadOp)
	{
		if (!_renderPasses.TryGetValue((colorFormat, colorLoadOp), out var value))
		{
			value = (_renderPasses[(colorFormat, colorLoadOp)] = OffscreenBakeRenderPass.Create(_device, colorFormat, colorLoadOp));
		}
		return value;
	}

	private static void TransitionColor(CommandBuffer commandBuffer, GpuTextureAllocation texture, VkImageLayout newLayout, VkAccessFlags srcAccess, VkAccessFlags dstAccess, VkPipelineStageFlags srcStage, VkPipelineStageFlags dstStage)
	{
		VkImageMemoryBarrier reference = new VkImageMemoryBarrier
		{
			SrcAccessMask = srcAccess,
			DstAccessMask = dstAccess,
			OldLayout = texture.CurrentLayout,
			NewLayout = newLayout,
			SrcQueueFamilyIndex = -1,
			DstQueueFamilyIndex = -1,
			Image = texture.Image,
			SubresourceRange = new VkImageSubresourceRange
			{
				AspectMask = VkImageAspectFlags.ColorBit,
				BaseMipLevel = 0,
				LevelCount = 1,
				BaseArrayLayer = 0,
				LayerCount = 1
			}
		};
		commandBuffer.PipelineBarrier(srcStage, dstStage, VkDependencyFlags.None, default(ReadOnlySpan<VkMemoryBarrier>), default(ReadOnlySpan<VkBufferMemoryBarrier>), new ReadOnlySpan<VkImageMemoryBarrier>(in reference));
		texture.CurrentLayout = newLayout;
	}

	private void DestroyTarget(TargetResources res)
	{
		if (!res.Framebuffer.IsNull())
		{
			_device.DestroyFramebuffer(res.Framebuffer, _device.VkAllocator);
		}
		if (!res.DepthView.IsNull())
		{
			_device.DestroyImageView(res.DepthView, _device.VkAllocator);
		}
		if (!res.Depth.MemAllocationInfo.Memory.IsNull())
		{
			GpuMemoryTracker.Remove(GpuMemoryCategory.RenderTarget, in res.Depth);
			_allocator.FreeAllocation(in res.Depth);
		}
	}

	public void Dispose()
	{
		lock (_sync)
		{
			if (_disposed)
			{
				return;
			}
			_disposed = true;
			_graphicsDevice.WaitIdleAndRun(delegate
			{
				foreach (TargetResources value in _targets.Values)
				{
					DestroyTarget(value);
				}
				_targets.Clear();
				_renderer.Dispose();
				if (!_fence.IsNull())
				{
					_device.DestroyFence(_fence, _device.VkAllocator);
				}
				if (!_commandPool.IsNull())
				{
					_device.DestroyCommandPool(_commandPool, _device.VkAllocator);
				}
				foreach (VkRenderPass value2 in _renderPasses.Values)
				{
					_device.DestroyRenderPass(value2, _device.VkAllocator);
				}
				_renderPasses.Clear();
			});
		}
	}
}
