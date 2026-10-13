using System;
using System.Buffers;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Brutal;
using Brutal.Logging;
using Brutal.Logging.Internal;
using Brutal.Numerics;
using Brutal.Pointers;
using Brutal.TextureApi;
using Brutal.VulkanApi;
using Brutal.VulkanApi.Abstractions;
using Brutal.VulkanApi.Vma;
using Microsoft.Win32.SafeHandles;
using PsdSharp;
using PsdSharp.Images;
using PsdSharp.Images.DataConversion;
using PsdSharp.Parsing;
using Waylith.Assets;
using Waylith.Graphics;
using Waylith.Graphics.Meshes;
using Waylith.Logging;
using Waylith.Profiling;
using Waylith.Rendering.Compute;
using Waylith.Runtime;
using Waylith.Utils;

namespace Waylith.Rendering;

public static class GpuTextureService
{
	public readonly struct ReadbackTicket
	{
		internal VkFence Fence { get; init; }

		internal CommandBuffer CommandBuffer { get; init; }

		internal BufferAllocationInfo Buffer { get; init; }

		internal long[] Offsets { get; init; }

		internal long[] Sizes { get; init; }

		internal int MipCount { get; init; }

		public bool IsValid => Offsets != null;
	}

	private readonly record struct PendingMipmap(GpuTextureAllocation Texture, int Width, int Height, int MipCount, int MinMipDimension, bool PreserveAlphaCoverage, float AlphaCoverageCutoff, bool Premultiplied);

	internal const uint TextureCacheMagic = 1480741966u;

	public const int TextureCacheVersion = 23;

	private const int MinimumDeferredDisposalDelay = 2;

	private static long _nextHandle = 1L;

	private static readonly object _sync = new object();

	private static readonly Stack<VkFence> _fencePool = new Stack<VkFence>();

	private static readonly List<BufferAllocationInfo> _stagingBufferPool = new List<BufferAllocationInfo>();

	private static readonly Lock _stagingSync = new Lock();

	private static GraphicsDevice? _graphicsDevice;

	private static DeviceEx? _device;

	private static VmaAllocator? _allocator;

	private static PhysicalDevice? _physicalDevice;

	private static Queue? _graphicsQueue;

	private static VkCommandPool _commandPool;

	private static PendingTextureBatch? _pendingTextureBatch;

	private static readonly Queue<(VkFence Fence, CommandBuffer CommandBuffer, List<BufferAllocationInfo> StagingBuffers, HashSet<(GpuTextureAllocation texture, int mipLevel)> TouchedSubresources)> _submittedTextureBatches = new Queue<(VkFence, CommandBuffer, List<BufferAllocationInfo>, HashSet<(GpuTextureAllocation, int)>)>();

	private static long _renderFrameSerial;

	private static int _deferredDisposalDelay = 3;

	private static int _uploadDeferralCount;

	private static BufferAllocationInfo _readbackBuffer;

	private static long _readbackBufferSize;

	private static readonly List<PendingMipmap> _pendingMipmaps = new List<PendingMipmap>();

	private static Texture2D? _whiteFallbackTexture;

	private static Texture2D? _blackFallbackTexture;

	private static Texture2D? _grayFallbackTexture;

	private static Texture2D? _whiteArrayFallbackTexture;

	private const long TextureStagingBudgetBytes = 134217728L;

	private const VkImageUsageFlags SampledTextureUsage = VkImageUsageFlags.TransferSrcBit | VkImageUsageFlags.TransferDstBit | VkImageUsageFlags.SampledBit;

	private const long ProactiveRetireBytes = 268435456L;

	private const long PlacementRetireMinBytes = 67108864L;

	private static readonly TimeSpan RenderThreadDrainTimeout = TimeSpan.FromSeconds(1L);

	private static long _proactiveRetireAtBytes = 268435456L;

	[ThreadStatic]
	private static bool _retiring;

	private static bool _ticketOutstanding;

	private static BufferAllocationInfo _ticketBuffer;

	private static long _ticketBufferSize;

	private static readonly ConcurrentDictionary<string, string?> _modRootByDir = new ConcurrentDictionary<string, string>();

	private const long NestedRetire = -1L;

	private const long MovedAwayFrameGap = 3L;

	private const int MaxPooledStagingBuffers = 2;

	private static readonly string? LoadTimeEncodeMode = Environment.GetEnvironmentVariable("WAYLITH_LOAD_ENCODE")?.ToLowerInvariant();

	public static int PackTextureHits;

	public static int PackTextureMisses;

	private static readonly ConcurrentDictionary<string, int> Rehydrated = new ConcurrentDictionary<string, int>(StringComparer.Ordinal);

	private static readonly ConcurrentDictionary<string, byte> _keyedCacheUsed = new ConcurrentDictionary<string, byte>(StringComparer.Ordinal);

	public static bool IsConfigured => _device != null;

	internal static long RenderFrameSerial => Interlocked.Read(in _renderFrameSerial);

	internal static DeviceEx? Device => _device;

	internal static Queue? GraphicsQueue => _graphicsQueue;

	internal static VkCommandPool CommandPool => _commandPool;

	internal static object CommandPoolSyncRoot => _sync;

	public static bool UploadsDeferred => Volatile.Read(in _uploadDeferralCount) > 0;

	internal static bool CanRunResidencyWork
	{
		get
		{
			if (!_retiring && !Monitor.IsEntered(_sync))
			{
				return RenderThread.IsProducerThread;
			}
			return false;
		}
	}

	public static Func<string, bool>? WantsBorderMipMaps { get; set; }

	private static bool LoadTimeEncodeOnGpu
	{
		get
		{
			string loadTimeEncodeMode = LoadTimeEncodeMode;
			if (loadTimeEncodeMode == "gpu" || loadTimeEncodeMode == "verify")
			{
				return true;
			}
			return false;
		}
	}

	public static bool TrackRehydration { get; set; }

	public static IReadOnlyDictionary<string, int> RehydratedTextures => Rehydrated;

	public static bool TextureCacheWrites { get; set; } = true;

	public static TextureCacheCodec CacheCodec { get; set; } = TextureCacheCodec.Lz4;

	internal static string? TextureCacheRoot
	{
		get
		{
			string text = WaylithRuntime.Current?.App.CacheDataPath;
			if (string.IsNullOrWhiteSpace(text))
			{
				text = Path.GetTempPath();
			}
			string cacheTag = TextureCompressionSettings.CacheTag;
			string path = cacheTag + TexturePixelData.Rgba32MipChainFilter switch
			{
				MipFilter.Kaiser => "k", 
				MipFilter.Dpid => "d", 
				_ => "", 
			} + (LoadTimeEncodeOnGpu ? "-gpu" : "");
			return Path.Combine(text, "TextureCache", $"v{23}", path);
		}
	}

	internal static VkFence GetOrCreateFence()
	{
		lock (_sync)
		{
			return GetOrCreateFenceLocked();
		}
	}

	internal static void ReturnFence(VkFence fence)
	{
		lock (_sync)
		{
			ReturnFenceLocked(fence);
		}
	}

	public static void Configure(GraphicsDevice graphicsDevice, int framesInFlight)
	{
		ArgumentNullException.ThrowIfNull(graphicsDevice, "graphicsDevice");
		AssetBakeHooks.BootComplete -= TextureCacheStore.ReleaseBuffers;
		AssetBakeHooks.BootComplete += TextureCacheStore.ReleaseBuffers;
		Configure(graphicsDevice.Device, graphicsDevice.Allocator, graphicsDevice.PhysicalDevice, graphicsDevice.GraphicsQueue, framesInFlight);
		lock (_sync)
		{
			_graphicsDevice = graphicsDevice;
			graphicsDevice.BeforeSubmit = SubmitPendingTextureCommands;
			Mipmapper.Configure(graphicsDevice);
		}
		TextureResidency.Configure(graphicsDevice);
	}

	public static void Configure(DeviceEx device, VmaAllocator allocator, PhysicalDevice physicalDevice, Queue graphicsQueue, int framesInFlight)
	{
		lock (_sync)
		{
			ShutdownLocked();
			VkCommandPoolCreateInfo pCreateInfo = new VkCommandPoolCreateInfo
			{
				QueueFamilyIndex = graphicsQueue.Family,
				Flags = VkCommandPoolCreateFlags.ResetCommandBufferBit
			};
			VkCommandPool commandPool = device.CreateCommandPool(in pCreateInfo, device.VkAllocator);
			_device = device;
			_allocator = allocator;
			_physicalDevice = physicalDevice;
			_graphicsQueue = graphicsQueue;
			_deferredDisposalDelay = System.Math.Max(2, framesInFlight + 1);
			_commandPool = commandPool;
		}
	}

	public static void Shutdown()
	{
		TextureBuildBatch.Shutdown();
		TextureCacheStore.Shutdown();
		lock (_sync)
		{
			ShutdownLocked();
		}
	}

	private static VkFence GetOrCreateFenceLocked()
	{
		if (_device == null)
		{
			return default(VkFence);
		}
		if (_fencePool.TryPop(out var result))
		{
			DeviceEx? device = _device;
			VkFence reference = result;
			device.ResetFences(new ReadOnlySpan<VkFence>(in reference));
			return result;
		}
		return _device.CreateFence(new VkFenceCreateInfo(), _device.VkAllocator);
	}

	private static void ReturnFenceLocked(VkFence fence)
	{
		if (_device != null && !fence.IsNull())
		{
			_fencePool.Push(fence);
		}
	}

	public static void BeginRenderFrame()
	{
		lock (_sync)
		{
			Interlocked.Increment(ref _renderFrameSerial);
			TextureResourceManager.ProcessDeferredDisposals(_renderFrameSerial);
			DrainDeferredDisposalsLocked(force: false);
		}
	}

	internal static GpuTextureAllocation? AllocateTexture2D(string label, int width, int height, int mipCount, VkFormat format, GpuMemoryCategory category = GpuMemoryCategory.GameTexture, TextureSwizzle swizzle = default(TextureSwizzle))
	{
		if (_device == null || _allocator == null || width <= 0 || height <= 0)
		{
			return null;
		}
		DeviceEx device = _device;
		VmaAllocator allocator = _allocator;
		VkImageCreateInfo createInfo = new VkImageCreateInfo
		{
			ImageType = VkImageType._2D,
			Format = format,
			Extent = new VkExtent3D(width, height, 1),
			MipLevels = System.Math.Max(1, mipCount),
			ArrayLayers = 1,
			Samples = VkSampleCountFlags._1Bit,
			Tiling = VkImageTiling.Optimal,
			Usage = (VkImageUsageFlags.TransferSrcBit | VkImageUsageFlags.TransferDstBit | VkImageUsageFlags.SampledBit),
			SharingMode = VkSharingMode.Exclusive,
			InitialLayout = VkImageLayout.Undefined
		};
		ImageAllocationInfo allocation = AllocateTextureImage(allocator, label, category, in createInfo);
		VkImageViewCreateInfo pCreateInfo = new VkImageViewCreateInfo
		{
			Image = allocation.Image,
			ViewType = VkImageViewType._2D,
			Format = format,
			Components = swizzle.ToVk(),
			SubresourceRange = new VkImageSubresourceRange
			{
				AspectMask = VkImageAspectFlags.ColorBit,
				BaseMipLevel = 0,
				LevelCount = System.Math.Max(1, mipCount),
				BaseArrayLayer = 0,
				LayerCount = 1
			}
		};
		VkImageView imageView = device.CreateImageView(in pCreateInfo, device.VkAllocator);
		GpuTextureAllocation obj = new GpuTextureAllocation(handle: new IntPtr(Interlocked.Increment(ref _nextHandle)), device: device, allocator: allocator, allocation: allocation, imageView: imageView, mipCount: System.Math.Max(1, mipCount), format: format, debugLabel: label, category: category);
		obj.SetImageInfo(VkImageType._2D, VkImageUsageFlags.TransferSrcBit | VkImageUsageFlags.TransferDstBit | VkImageUsageFlags.SampledBit, VkImageViewType._2D, swizzle);
		GpuMemoryTracker.SetDetail(in allocation, TextureDetail(width, height, format, mipCount));
		return obj;
	}

	public static GpuTextureAllocation? CreateTexture2D(string label, int width, int height, int mipCount, VkFormat format, ReadOnlySpan<TextureLevelUpload> levels, GpuMemoryCategory category = GpuMemoryCategory.GameTexture, TextureLevelFill? fill = null, bool batchUpload = false, TextureSwizzle swizzle = default(TextureSwizzle))
	{
		GpuTextureAllocation gpuTexture = AllocateTexture2D(label, width, height, mipCount, format, category, swizzle);
		if (gpuTexture == null)
		{
			return null;
		}
		_ = _allocator;
		ImageAllocationInfo allocation = gpuTexture.AllocationInfo;
		if (levels.Length == 0)
		{
			ExecuteImageCommands(delegate(CommandBuffer commandBuffer)
			{
				TransitionLayout(commandBuffer, gpuTexture, VkImageLayout.ShaderReadOnlyOptimal, VkAccessFlags.None, VkAccessFlags.ShaderReadBit, VkPipelineStageFlags.TopOfPipeBit, VkPipelineStageFlags.FragmentShaderBit);
			}, false);
			return gpuTexture;
		}
		TextureLevelUpload[] array = levels.ToArray();
		List<(BufferAllocationInfo staging, TextureLevelUpload level)> stagedLevels = new List<(BufferAllocationInfo, TextureLevelUpload)>(array.Length);
		TextureLevelUpload[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			TextureLevelUpload item = array2[i];
			if (item.Size != 0)
			{
				string label2 = $"{label}.Upload[{item.Level}]";
				stagedLevels.Add(((fill == null) ? RentFilledStagingBuffer(label2, item.Bytes) : RentFilledStagingBuffer(label2, item.Size, item.Level, fill), item));
			}
		}
		ExecuteImageCommands(delegate(CommandBuffer commandBuffer, List<BufferAllocationInfo> stagingBuffers)
		{
			TransitionLayout(commandBuffer, gpuTexture, VkImageLayout.TransferDstOptimal, VkAccessFlags.None, VkAccessFlags.TransferWriteBit, VkPipelineStageFlags.TopOfPipeBit, VkPipelineStageFlags.AllCommandsBit);
			foreach (var (item2, textureLevelUpload) in stagedLevels)
			{
				stagingBuffers.Add(item2);
				VkBuffer vkBuffer = item2.VkBuffer;
				VkImage image = allocation.Image;
				VkBufferImageCopy reference = new VkBufferImageCopy
				{
					BufferOffset = ByteSize.Zero,
					BufferRowLength = textureLevelUpload.RowLengthTexels,
					BufferImageHeight = 0,
					ImageSubresource = new VkImageSubresourceLayers
					{
						AspectMask = VkImageAspectFlags.ColorBit,
						MipLevel = textureLevelUpload.Level,
						BaseArrayLayer = 0,
						LayerCount = 1
					},
					ImageOffset = new VkOffset3D(0, 0, 0),
					ImageExtent = new VkExtent3D(textureLevelUpload.Width, textureLevelUpload.Height, 1)
				};
				commandBuffer.CopyBufferToImage(vkBuffer, image, VkImageLayout.TransferDstOptimal, new ReadOnlySpan<VkBufferImageCopy>(in reference));
			}
		}, batchUpload || UploadsDeferred, (gpuTexture, -1));
		return gpuTexture;
	}

	public static GpuTextureAllocation? CreateTexture3D(string label, int width, int height, int depth, int mipCount, VkFormat format, ReadOnlySpan<TextureLevelUpload> levels, GpuMemoryCategory category = GpuMemoryCategory.GameTexture)
	{
		if (_device == null || _allocator == null || width <= 0 || height <= 0 || depth <= 0)
		{
			return null;
		}
		DeviceEx device = _device;
		VmaAllocator allocator = _allocator;
		ImageAllocationInfo allocation = AllocateTextureImage(allocator, label, category, new VkImageCreateInfo
		{
			ImageType = VkImageType._3D,
			Format = format,
			Extent = new VkExtent3D(width, height, depth),
			MipLevels = System.Math.Max(1, mipCount),
			ArrayLayers = 1,
			Samples = VkSampleCountFlags._1Bit,
			Tiling = VkImageTiling.Optimal,
			Usage = (VkImageUsageFlags.TransferSrcBit | VkImageUsageFlags.TransferDstBit | VkImageUsageFlags.SampledBit),
			SharingMode = VkSharingMode.Exclusive,
			InitialLayout = VkImageLayout.Undefined
		});
		VkImageView imageView = device.CreateImageView(new VkImageViewCreateInfo
		{
			Image = allocation.Image,
			ViewType = VkImageViewType._3D,
			Format = format,
			SubresourceRange = new VkImageSubresourceRange
			{
				AspectMask = VkImageAspectFlags.ColorBit,
				BaseMipLevel = 0,
				LevelCount = System.Math.Max(1, mipCount),
				BaseArrayLayer = 0,
				LayerCount = 1
			}
		}, device.VkAllocator);
		GpuTextureAllocation gpuTexture = new GpuTextureAllocation(handle: new IntPtr(Interlocked.Increment(ref _nextHandle)), device: device, allocator: allocator, allocation: allocation, imageView: imageView, mipCount: System.Math.Max(1, mipCount), format: format, debugLabel: label, category: category);
		gpuTexture.SetImageInfo(VkImageType._3D, VkImageUsageFlags.TransferSrcBit | VkImageUsageFlags.TransferDstBit | VkImageUsageFlags.SampledBit, VkImageViewType._3D);
		GpuMemoryTracker.SetDetail(in allocation, TextureDetail(width, height, format, mipCount, 1, depth));
		if (levels.Length == 0)
		{
			ExecuteImageCommands(delegate(CommandBuffer commandBuffer)
			{
				TransitionLayout(commandBuffer, gpuTexture, VkImageLayout.ShaderReadOnlyOptimal, VkAccessFlags.None, VkAccessFlags.ShaderReadBit, VkPipelineStageFlags.TopOfPipeBit, VkPipelineStageFlags.FragmentShaderBit);
			}, false);
			return gpuTexture;
		}
		lock (_sync)
		{
			CommandBuffer commandBuffer2 = device.AllocateCommandBuffer(new VkCommandBufferAllocateInfo
			{
				CommandPool = _commandPool,
				Level = VkCommandBufferLevel.Primary
			});
			List<BufferAllocationInfo> list = new List<BufferAllocationInfo>(levels.Length);
			try
			{
				commandBuffer2.Begin(VkCommandBufferUsageFlags.OneTimeSubmitBit);
				TransitionLayout(commandBuffer2, gpuTexture, VkImageLayout.TransferDstOptimal, VkAccessFlags.None, VkAccessFlags.TransferWriteBit, VkPipelineStageFlags.TopOfPipeBit, VkPipelineStageFlags.AllCommandsBit);
				ReadOnlySpan<TextureLevelUpload> readOnlySpan = levels;
				for (int i = 0; i < readOnlySpan.Length; i++)
				{
					TextureLevelUpload textureLevelUpload = readOnlySpan[i];
					if (textureLevelUpload.Bytes != null && textureLevelUpload.Bytes.Length != 0)
					{
						BufferAllocationInfo item = RentStagingBuffer($"{label}.Upload[{textureLevelUpload.Level}]", textureLevelUpload.Bytes.Length);
						AllocationMapInfo allocationMapInfo = new AllocationMapInfo(item.AllocationInfo);
						allocationMapInfo.Region = new MapRegion
						{
							Offset = ByteSize.Zero,
							Size = item.BufferSize
						};
						AllocationMapInfo allocationMapInfo2 = allocationMapInfo;
						Ptr ptr = allocator.MapMemory(in allocationMapInfo2);
						Marshal.Copy(textureLevelUpload.Bytes, 0, (nint)ptr, textureLevelUpload.Bytes.Length);
						allocator.UnmapMemory(in allocationMapInfo2);
						list.Add(item);
						CommandBuffer handle2 = commandBuffer2;
						VkBuffer vkBuffer = item.VkBuffer;
						VkImage image = allocation.Image;
						VkBufferImageCopy reference = new VkBufferImageCopy
						{
							BufferOffset = ByteSize.Zero,
							BufferRowLength = textureLevelUpload.RowLengthTexels,
							BufferImageHeight = 0,
							ImageSubresource = new VkImageSubresourceLayers
							{
								AspectMask = VkImageAspectFlags.ColorBit,
								MipLevel = textureLevelUpload.Level,
								BaseArrayLayer = 0,
								LayerCount = 1
							},
							ImageOffset = new VkOffset3D(0, 0, 0),
							ImageExtent = new VkExtent3D(textureLevelUpload.Width, textureLevelUpload.Height, textureLevelUpload.Depth)
						};
						handle2.CopyBufferToImage(vkBuffer, image, VkImageLayout.TransferDstOptimal, new ReadOnlySpan<VkBufferImageCopy>(in reference));
					}
				}
				TransitionLayout(commandBuffer2, gpuTexture, VkImageLayout.ShaderReadOnlyOptimal, VkAccessFlags.TransferWriteBit, VkAccessFlags.ShaderReadBit, VkPipelineStageFlags.AllCommandsBit, VkPipelineStageFlags.FragmentShaderBit);
				commandBuffer2.End();
				VkFence orCreateFenceLocked = GetOrCreateFenceLocked();
				try
				{
					CommandBuffer reference2 = commandBuffer2;
					SubmitGraphicsLocked(new Span<CommandBuffer>(ref reference2), orCreateFenceLocked);
					device.WaitForFence(orCreateFenceLocked, -1);
				}
				finally
				{
					ReturnFenceLocked(orCreateFenceLocked);
				}
			}
			finally
			{
				if (!commandBuffer2.Handle.IsNull())
				{
					VkCommandPool commandPool = _commandPool;
					CommandBuffer reference3 = commandBuffer2;
					device.FreeCommandBuffers(commandPool, new ReadOnlySpan<CommandBuffer>(in reference3));
				}
				foreach (BufferAllocationInfo item2 in list)
				{
					ReturnStagingBuffer(item2);
				}
			}
		}
		return gpuTexture;
	}

	public unsafe static GpuTextureAllocation? CreateEmptyTexture2D(string label, int width, int height, int mipCount, VkFormat format, VkImageUsageFlags additionalUsage = VkImageUsageFlags.None, GpuMemoryCategory category = GpuMemoryCategory.GameTexture)
	{
		if (_device == null || _allocator == null || width <= 0 || height <= 0)
		{
			return null;
		}
		DeviceEx device = _device;
		VmaAllocator allocator = _allocator;
		bool flag = IsDepthFormat(format);
		VkImageUsageFlags vkImageUsageFlags = VkImageUsageFlags.TransferSrcBit | VkImageUsageFlags.TransferDstBit | VkImageUsageFlags.SampledBit | additionalUsage;
		if (flag)
		{
			vkImageUsageFlags |= VkImageUsageFlags.DepthStencilAttachmentBit;
		}
		ImageAllocationInfo allocation = AllocateTextureImage(allocator, label, category, new VkImageCreateInfo
		{
			ImageType = VkImageType._2D,
			Format = format,
			Extent = new VkExtent3D(width, height, 1),
			MipLevels = System.Math.Max(1, mipCount),
			ArrayLayers = 1,
			Samples = VkSampleCountFlags._1Bit,
			Tiling = VkImageTiling.Optimal,
			Usage = vkImageUsageFlags,
			SharingMode = VkSharingMode.Exclusive,
			InitialLayout = VkImageLayout.Undefined
		});
		VkImageView imageView = device.CreateImageView(new VkImageViewCreateInfo
		{
			Image = allocation.Image,
			ViewType = VkImageViewType._2D,
			Format = format,
			SubresourceRange = new VkImageSubresourceRange
			{
				AspectMask = GetSampledImageAspectMask(format),
				BaseMipLevel = 0,
				LevelCount = System.Math.Max(1, mipCount),
				BaseArrayLayer = 0,
				LayerCount = 1
			}
		}, device.VkAllocator);
		GpuTextureAllocation gpuTexture = new GpuTextureAllocation(handle: new IntPtr(Interlocked.Increment(ref _nextHandle)), device: device, allocator: allocator, allocation: allocation, imageView: imageView, mipCount: System.Math.Max(1, mipCount), format: format, debugLabel: label, category: category);
		gpuTexture.SetImageInfo(VkImageType._2D, vkImageUsageFlags, VkImageViewType._2D);
		GpuMemoryTracker.SetDetail(in allocation, TextureDetail(width, height, format, mipCount));
		bool flag2 = !flag;
		if (flag2)
		{
			bool flag3 = ((format < VkFormat.Bc1RGBUNormBlock || format > VkFormat.Bc7SRGBBlock) ? true : false);
			flag2 = flag3;
		}
		bool clearable = flag2;
		ExecuteImageCommands(delegate(CommandBuffer commandBuffer)
		{
			if (!clearable)
			{
				TransitionLayout(commandBuffer, gpuTexture, VkImageLayout.ShaderReadOnlyOptimal, VkAccessFlags.None, VkAccessFlags.ShaderReadBit, VkPipelineStageFlags.TopOfPipeBit, VkPipelineStageFlags.FragmentShaderBit);
			}
			else
			{
				TransitionLayout(commandBuffer, gpuTexture, VkImageLayout.TransferDstOptimal, VkAccessFlags.None, VkAccessFlags.TransferWriteBit, VkPipelineStageFlags.TopOfPipeBit, VkPipelineStageFlags.TransferBit);
				VkClearColorValue vkClearColorValue = default(VkClearColorValue);
				VkImageSubresourceRange vkImageSubresourceRange = new VkImageSubresourceRange
				{
					AspectMask = VkImageAspectFlags.ColorBit,
					BaseMipLevel = 0,
					LevelCount = gpuTexture.MipCount,
					BaseArrayLayer = 0,
					LayerCount = 1
				};
				commandBuffer.ClearColorImage(gpuTexture.Image, VkImageLayout.TransferDstOptimal, &vkClearColorValue, new ReadOnlySpan<VkImageSubresourceRange>(&vkImageSubresourceRange, 1));
				TransitionLayout(commandBuffer, gpuTexture, VkImageLayout.ShaderReadOnlyOptimal, VkAccessFlags.TransferWriteBit, VkAccessFlags.ShaderReadBit, VkPipelineStageFlags.TransferBit, VkPipelineStageFlags.FragmentShaderBit);
			}
		}, false);
		return gpuTexture;
	}

	public static byte[][]? ReadbackTexture2DLevels(GpuTextureAllocation texture, int width, int height, int mipCount, int bytesPerPixel, byte[][]? into = null)
	{
		lock (_sync)
		{
			ReadbackTicket ticket = SubmitReadback(texture, width, height, mipCount, bytesPerPixel, ticketed: false);
			return ticket.IsValid ? CompleteReadback(ticket, into, ticketed: false) : null;
		}
	}

	public static ReadbackTicket BeginReadbackLevels(GpuTextureAllocation texture, int width, int height, int mipCount, int bytesPerPixel)
	{
		return SubmitReadback(texture, width, height, mipCount, bytesPerPixel, ticketed: true);
	}

	public static byte[][]? EndReadbackLevels(ReadbackTicket ticket, byte[][]? into = null)
	{
		if (!ticket.IsValid)
		{
			return null;
		}
		return CompleteReadback(ticket, into, ticketed: true);
	}

	private static ReadbackTicket SubmitReadback(GpuTextureAllocation texture, int width, int height, int mipCount, int bytesPerPixel, bool ticketed)
	{
		if (_device == null || _allocator == null || texture == null || texture.Image.IsNull() || width <= 0 || height <= 0 || mipCount <= 0 || bytesPerPixel <= 0)
		{
			return default(ReadbackTicket);
		}
		mipCount = System.Math.Min(mipCount, texture.MipCount);
		long[] array = new long[mipCount];
		long[] array2 = new long[mipCount];
		long num = 0L;
		for (int i = 0; i < mipCount; i++)
		{
			array[i] = num;
			array2[i] = (long)System.Math.Max(1, width >> i) * (long)System.Math.Max(1, height >> i) * bytesPerPixel;
			num += array2[i];
		}
		lock (_sync)
		{
			if (ticketed && _ticketOutstanding)
			{
				return default(ReadbackTicket);
			}
			FlushPendingTextureCommandsLocked();
			BufferAllocationInfo buffer = (ticketed ? AcquireTicketBufferLocked(num) : AcquireReadbackBufferLocked(num));
			CommandBuffer commandBuffer = _device.AllocateCommandBuffer(new VkCommandBufferAllocateInfo
			{
				CommandPool = _commandPool,
				Level = VkCommandBufferLevel.Primary
			});
			commandBuffer.Begin(VkCommandBufferUsageFlags.OneTimeSubmitBit);
			TransitionMipRangeLayout(commandBuffer, texture, mipCount, VkImageLayout.TransferSrcOptimal, VkAccessFlags.ShaderReadBit, VkAccessFlags.TransferReadBit, VkPipelineStageFlags.AllCommandsBit, VkPipelineStageFlags.TransferBit);
			VkBufferImageCopy[] array3 = new VkBufferImageCopy[mipCount];
			for (int j = 0; j < mipCount; j++)
			{
				array3[j] = new VkBufferImageCopy
				{
					BufferOffset = ByteSize.Of(array[j]),
					BufferRowLength = 0,
					BufferImageHeight = 0,
					ImageSubresource = new VkImageSubresourceLayers
					{
						AspectMask = VkImageAspectFlags.ColorBit,
						MipLevel = j,
						BaseArrayLayer = 0,
						LayerCount = 1
					},
					ImageOffset = new VkOffset3D(0, 0, 0),
					ImageExtent = new VkExtent3D(System.Math.Max(1, width >> j), System.Math.Max(1, height >> j), 1)
				};
			}
			commandBuffer.CopyImageToBuffer(texture.Image, VkImageLayout.TransferSrcOptimal, buffer.VkBuffer, array3);
			VkMemoryBarrier reference = new VkMemoryBarrier
			{
				SrcAccessMask = VkAccessFlags.TransferWriteBit,
				DstAccessMask = VkAccessFlags.HostReadBit
			};
			commandBuffer.PipelineBarrier(VkPipelineStageFlags.TransferBit, VkPipelineStageFlags.HostBit, VkDependencyFlags.None, new ReadOnlySpan<VkMemoryBarrier>(in reference), default(Span<VkBufferMemoryBarrier>), default(Span<VkImageMemoryBarrier>));
			TransitionMipRangeLayout(commandBuffer, texture, mipCount, VkImageLayout.ShaderReadOnlyOptimal, VkAccessFlags.TransferReadBit, VkAccessFlags.ShaderReadBit, VkPipelineStageFlags.TransferBit, VkPipelineStageFlags.FragmentShaderBit);
			commandBuffer.End();
			VkFence orCreateFenceLocked = GetOrCreateFenceLocked();
			CommandBuffer reference2 = commandBuffer;
			SubmitGraphicsLocked(new Span<CommandBuffer>(ref reference2), orCreateFenceLocked);
			if (ticketed)
			{
				_ticketOutstanding = true;
			}
			ReadbackTicket result = default(ReadbackTicket);
			result.Fence = orCreateFenceLocked;
			result.CommandBuffer = commandBuffer;
			result.Buffer = buffer;
			result.Offsets = array;
			result.Sizes = array2;
			result.MipCount = mipCount;
			return result;
		}
	}

	private unsafe static byte[][]? CompleteReadback(ReadbackTicket ticket, byte[][]? into, bool ticketed)
	{
		lock (_sync)
		{
			try
			{
				_device.WaitForFence(ticket.Fence, -1);
			}
			finally
			{
				ReturnFenceLocked(ticket.Fence);
				if (ticketed)
				{
					_ticketOutstanding = false;
				}
			}
			try
			{
				int mipCount = ticket.MipCount;
				byte[][] array = (byte[][])((into != null && into.Length >= mipCount) ? ((Array)into) : ((Array)new byte[mipCount][]));
				AllocationMapInfo allocationMapInfo = new AllocationMapInfo(ticket.Buffer.AllocationInfo);
				allocationMapInfo.Region = new MapRegion
				{
					Offset = ByteSize.Zero,
					Size = ticket.Buffer.BufferSize
				};
				AllocationMapInfo allocationMapInfo2 = allocationMapInfo;
				Ptr ptr = _allocator.MapMemory(in allocationMapInfo2);
				try
				{
					for (int i = 0; i < mipCount; i++)
					{
						if (array[i] == null || array[i].Length < ticket.Sizes[i])
						{
							array[i] = new byte[ticket.Sizes[i]];
						}
						new ReadOnlySpan<byte>((void*)((nint)ptr + (nint)ticket.Offsets[i]), (int)ticket.Sizes[i]).CopyTo(array[i]);
					}
				}
				finally
				{
					_allocator.UnmapMemory(in allocationMapInfo2);
				}
				return array;
			}
			finally
			{
				if (!ticket.CommandBuffer.Handle.IsNull())
				{
					DeviceEx? device = _device;
					VkCommandPool commandPool = _commandPool;
					CommandBuffer reference = ticket.CommandBuffer;
					device.FreeCommandBuffers(commandPool, new ReadOnlySpan<CommandBuffer>(in reference));
				}
			}
		}
	}

	private static BufferAllocationInfo AcquireTicketBufferLocked(long size)
	{
		if (_ticketBufferSize >= size)
		{
			return _ticketBuffer;
		}
		if (_ticketBufferSize > 0)
		{
			_allocator.FreeAllocation(in _ticketBuffer);
		}
		_ticketBuffer = AllocateReadbackBuffer("Texture2DReadbackTicket", size);
		_ticketBufferSize = size;
		return _ticketBuffer;
	}

	private static BufferAllocationInfo AcquireReadbackBufferLocked(long size)
	{
		if (_readbackBufferSize >= size)
		{
			return _readbackBuffer;
		}
		if (_readbackBufferSize > 0)
		{
			_allocator.FreeAllocation(in _readbackBuffer);
		}
		_readbackBuffer = AllocateReadbackBuffer("Texture2DReadbackLevels", size);
		_readbackBufferSize = size;
		return _readbackBuffer;
	}

	private static BufferAllocationInfo AllocateReadbackBuffer(string label, long size)
	{
		VmaAllocator? allocator = _allocator;
		VkBufferCreateInfo bufferCreateInfo = new VkBufferCreateInfo
		{
			Size = ByteSize.Of(size),
			Usage = VkBufferUsageFlags.TransferDstBit,
			SharingMode = VkSharingMode.Exclusive
		};
		AllocationInfo.CreateInfo allocateCreateInfo = new AllocationInfo.CreateInfo
		{
			Preference = MemoryPreference.PreferCpu,
			RequiredProperties = (VkMemoryPropertyFlags.HostVisibleBit | VkMemoryPropertyFlags.HostCoherentBit),
			PreferredProperties = (VkMemoryPropertyFlags.HostVisibleBit | VkMemoryPropertyFlags.HostCoherentBit | VkMemoryPropertyFlags.HostCachedBit),
			Priority = 1f
		};
		return allocator.AllocateBufferChecked(label, in bufferCreateInfo, in allocateCreateInfo);
	}

	private static void TransitionMipRangeLayout(CommandBuffer commandBuffer, GpuTextureAllocation texture, int mipCount, VkImageLayout newLayout, VkAccessFlags sourceAccessMask, VkAccessFlags destinationAccessMask, VkPipelineStageFlags sourceStageMask, VkPipelineStageFlags destinationStageMask)
	{
		List<VkImageMemoryBarrier> list = new List<VkImageMemoryBarrier>(1);
		int num = 0;
		for (int i = 1; i <= mipCount; i++)
		{
			if (i >= mipCount || texture.GetMipLayout(i) != texture.GetMipLayout(num))
			{
				VkImageLayout mipLayout = texture.GetMipLayout(num);
				if (mipLayout != newLayout || sourceAccessMask != destinationAccessMask || sourceStageMask != destinationStageMask)
				{
					list.Add(new VkImageMemoryBarrier
					{
						SrcAccessMask = sourceAccessMask,
						DstAccessMask = destinationAccessMask,
						OldLayout = mipLayout,
						NewLayout = newLayout,
						SrcQueueFamilyIndex = -1,
						DstQueueFamilyIndex = -1,
						Image = texture.Image,
						SubresourceRange = new VkImageSubresourceRange
						{
							AspectMask = VkImageAspectFlags.ColorBit,
							BaseMipLevel = num,
							LevelCount = i - num,
							BaseArrayLayer = 0,
							LayerCount = 1
						}
					});
				}
				num = i;
			}
		}
		for (int j = 0; j < mipCount; j++)
		{
			texture.SetMipLayout(j, newLayout);
		}
		if (list.Count > 0)
		{
			commandBuffer.PipelineBarrier(sourceStageMask, destinationStageMask, VkDependencyFlags.None, default(ReadOnlySpan<VkMemoryBarrier>), default(ReadOnlySpan<VkBufferMemoryBarrier>), list.ToArray());
		}
	}

	public unsafe static byte[]? ReadbackTexture2D(GpuTextureAllocation texture, int width, int height, int bytesPerPixel)
	{
		if (_device == null || _allocator == null || texture == null || texture.Image.IsNull() || width <= 0 || height <= 0 || bytesPerPixel <= 0)
		{
			return null;
		}
		long num = (long)width * (long)height * bytesPerPixel;
		lock (_sync)
		{
			FlushPendingTextureCommandsLocked();
			VmaAllocator? allocator = _allocator;
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
			BufferAllocationInfo inAllocation = allocator.AllocateBufferChecked("Texture2DReadback", in bufferCreateInfo, in allocateCreateInfo);
			CommandBuffer commandBuffer = _device.AllocateCommandBuffer(new VkCommandBufferAllocateInfo
			{
				CommandPool = _commandPool,
				Level = VkCommandBufferLevel.Primary
			});
			try
			{
				commandBuffer.Begin(VkCommandBufferUsageFlags.OneTimeSubmitBit);
				TransitionSubresourceLayout(commandBuffer, texture, 0, VkImageLayout.TransferSrcOptimal, VkAccessFlags.ShaderReadBit, VkAccessFlags.TransferReadBit, VkPipelineStageFlags.AllCommandsBit, VkPipelineStageFlags.TransferBit);
				CommandBuffer handle = commandBuffer;
				VkImage image = texture.Image;
				VkBuffer vkBuffer = inAllocation.VkBuffer;
				VkBufferImageCopy reference = new VkBufferImageCopy
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
				handle.CopyImageToBuffer(image, VkImageLayout.TransferSrcOptimal, vkBuffer, new ReadOnlySpan<VkBufferImageCopy>(in reference));
				TransitionSubresourceLayout(commandBuffer, texture, 0, VkImageLayout.ShaderReadOnlyOptimal, VkAccessFlags.TransferReadBit, VkAccessFlags.ShaderReadBit, VkPipelineStageFlags.TransferBit, VkPipelineStageFlags.FragmentShaderBit);
				commandBuffer.End();
				VkFence orCreateFenceLocked = GetOrCreateFenceLocked();
				try
				{
					CommandBuffer reference2 = commandBuffer;
					SubmitGraphicsLocked(new Span<CommandBuffer>(ref reference2), orCreateFenceLocked);
					_device.WaitForFence(orCreateFenceLocked, -1);
				}
				finally
				{
					ReturnFenceLocked(orCreateFenceLocked);
				}
				byte[] array = new byte[num];
				AllocationMapInfo allocationMapInfo = new AllocationMapInfo(inAllocation.AllocationInfo);
				allocationMapInfo.Region = new MapRegion
				{
					Offset = ByteSize.Zero,
					Size = inAllocation.BufferSize
				};
				AllocationMapInfo allocationMapInfo2 = allocationMapInfo;
				Ptr ptr = _allocator.MapMemory(in allocationMapInfo2);
				try
				{
					new ReadOnlySpan<byte>((void*)(nint)ptr, array.Length).CopyTo(array);
				}
				finally
				{
					_allocator.UnmapMemory(in allocationMapInfo2);
				}
				return array;
			}
			finally
			{
				if (!commandBuffer.Handle.IsNull())
				{
					DeviceEx? device = _device;
					VkCommandPool commandPool = _commandPool;
					CommandBuffer reference3 = commandBuffer;
					device.FreeCommandBuffers(commandPool, new ReadOnlySpan<CommandBuffer>(in reference3));
				}
				_allocator.FreeAllocation(in inAllocation);
			}
		}
	}

	public static GpuTextureAllocation? CreateTexture2DArray(string label, int width, int height, int layers, int mipCount, VkFormat format, VkImageUsageFlags additionalUsage = VkImageUsageFlags.None, GpuMemoryCategory category = GpuMemoryCategory.GameTexture)
	{
		return CreateTexture2DArrayCore(label, width, height, layers, mipCount, format, additionalUsage, initializeForSampling: true, category);
	}

	internal static GpuTextureAllocation? CreateUninitializedTexture2DArray(string label, int width, int height, int layers, int mipCount, VkFormat format, GpuMemoryCategory category = GpuMemoryCategory.GameTexture, TextureSwizzle swizzle = default(TextureSwizzle))
	{
		return CreateTexture2DArrayCore(label, width, height, layers, mipCount, format, VkImageUsageFlags.None, initializeForSampling: false, category, swizzle);
	}

	private static GpuTextureAllocation? CreateTexture2DArrayCore(string label, int width, int height, int layers, int mipCount, VkFormat format, VkImageUsageFlags additionalUsage, bool initializeForSampling, GpuMemoryCategory category, TextureSwizzle swizzle = default(TextureSwizzle))
	{
		if (_device == null || _allocator == null || width <= 0 || height <= 0 || layers <= 0)
		{
			return null;
		}
		DeviceEx device = _device;
		VmaAllocator allocator = _allocator;
		VkImageUsageFlags usage = VkImageUsageFlags.TransferSrcBit | VkImageUsageFlags.TransferDstBit | VkImageUsageFlags.SampledBit | additionalUsage;
		ImageAllocationInfo allocation = AllocateTextureImage(allocator, label, category, new VkImageCreateInfo
		{
			ImageType = VkImageType._2D,
			Format = format,
			Extent = new VkExtent3D(width, height, 1),
			MipLevels = System.Math.Max(1, mipCount),
			ArrayLayers = layers,
			Samples = VkSampleCountFlags._1Bit,
			Tiling = VkImageTiling.Optimal,
			Usage = usage,
			SharingMode = VkSharingMode.Exclusive,
			InitialLayout = VkImageLayout.Undefined
		});
		VkImageView imageView = device.CreateImageView(new VkImageViewCreateInfo
		{
			Image = allocation.Image,
			ViewType = VkImageViewType._2DArray,
			Format = format,
			Components = swizzle.ToVk(),
			SubresourceRange = new VkImageSubresourceRange
			{
				AspectMask = VkImageAspectFlags.ColorBit,
				BaseMipLevel = 0,
				LevelCount = System.Math.Max(1, mipCount),
				BaseArrayLayer = 0,
				LayerCount = layers
			}
		}, device.VkAllocator);
		GpuTextureAllocation gpuTexture = new GpuTextureAllocation(handle: new IntPtr(Interlocked.Increment(ref _nextHandle)), device: device, allocator: allocator, allocation: allocation, imageView: imageView, mipCount: System.Math.Max(1, mipCount), format: format, debugLabel: label, category: category);
		gpuTexture.SetImageInfo(VkImageType._2D, usage, VkImageViewType._2DArray, swizzle);
		GpuMemoryTracker.SetDetail(in allocation, TextureDetail(width, height, format, mipCount, layers));
		if (initializeForSampling)
		{
			ExecuteImageCommands(delegate(CommandBuffer commandBuffer)
			{
				TransitionLayout(commandBuffer, gpuTexture, VkImageLayout.ShaderReadOnlyOptimal, VkAccessFlags.None, VkAccessFlags.ShaderReadBit, VkPipelineStageFlags.TopOfPipeBit, VkPipelineStageFlags.FragmentShaderBit);
			}, false);
		}
		return gpuTexture;
	}

	public static void UploadRgba32ToArrayLayer(GpuTextureAllocation texture, int textureWidth, int textureHeight, int arrayLayer, byte[] bytes, bool batch = false)
	{
		if (_device != null && _allocator != null && bytes.Length != 0)
		{
			VmaAllocator allocator = _allocator;
			ExecuteImageCommands(delegate(CommandBuffer commandBuffer, List<BufferAllocationInfo> stagingBuffers)
			{
				BufferAllocationInfo item = RentStagingBuffer($"{texture.DebugLabel}.ArrayLayerUpload[{arrayLayer}]", bytes.Length);
				AllocationMapInfo allocationMapInfo = new AllocationMapInfo(item.AllocationInfo)
				{
					Region = new MapRegion
					{
						Offset = ByteSize.Zero,
						Size = item.BufferSize
					}
				};
				Ptr ptr = allocator.MapMemory(in allocationMapInfo);
				Marshal.Copy(bytes, 0, (nint)ptr, bytes.Length);
				allocator.UnmapMemory(in allocationMapInfo);
				stagingBuffers.Add(item);
				TransitionLayout(commandBuffer, texture, VkImageLayout.TransferDstOptimal, VkAccessFlags.None, VkAccessFlags.TransferWriteBit, VkPipelineStageFlags.TopOfPipeBit, VkPipelineStageFlags.TransferBit);
				VkBuffer vkBuffer = item.VkBuffer;
				VkImage image = texture.Image;
				VkBufferImageCopy reference = new VkBufferImageCopy
				{
					BufferOffset = ByteSize.Zero,
					BufferRowLength = textureWidth,
					BufferImageHeight = 0,
					ImageSubresource = new VkImageSubresourceLayers
					{
						AspectMask = VkImageAspectFlags.ColorBit,
						MipLevel = 0,
						BaseArrayLayer = arrayLayer,
						LayerCount = 1
					},
					ImageOffset = new VkOffset3D(0, 0, 0),
					ImageExtent = new VkExtent3D(textureWidth, textureHeight, 1)
				};
				commandBuffer.CopyBufferToImage(vkBuffer, image, VkImageLayout.TransferDstOptimal, new ReadOnlySpan<VkBufferImageCopy>(in reference));
			}, batch, (texture, -1));
		}
	}

	public static GpuComputeBuffer? CreateComputeBuffer(string label, long sizeBytes)
	{
		if (_device == null || _allocator == null)
		{
			return null;
		}
		return GpuComputeBuffer.Create(_device, _allocator, label, sizeBytes);
	}

	private static bool IsDepthFormat(VkFormat format)
	{
		switch (format)
		{
		case VkFormat.D16UNorm:
		case VkFormat.D32SFloat:
		case VkFormat.D16UNormS8UInt:
		case VkFormat.D24UNormS8UInt:
		case VkFormat.D32SFloatS8UInt:
			return true;
		default:
			return false;
		}
	}

	private static VkImageAspectFlags GetSampledImageAspectMask(VkFormat format)
	{
		if (!IsDepthFormat(format))
		{
			return VkImageAspectFlags.ColorBit;
		}
		return VkImageAspectFlags.DepthBit;
	}

	public static void BeginUploadDeferral()
	{
		Interlocked.Increment(ref _uploadDeferralCount);
	}

	public static void EndUploadDeferral()
	{
		Interlocked.Decrement(ref _uploadDeferralCount);
	}

	public static int SuspendUploadDeferral()
	{
		int num = Interlocked.Exchange(ref _uploadDeferralCount, 0);
		if (num > 0)
		{
			FlushPendingTextureCommands();
		}
		return num;
	}

	public static void ResumeUploadDeferral(int suspended)
	{
		Interlocked.Add(ref _uploadDeferralCount, suspended);
	}

	public static void SubmitPendingUploads()
	{
		FlushPendingUploadsBeforeDisposal();
	}

	internal static void FlushPendingUploadsBeforeDisposal()
	{
		lock (_sync)
		{
			FlushPendingTextureCommandsLocked();
		}
	}

	public static void FlushPendingTextureCommands()
	{
		if (UploadsDeferred)
		{
			return;
		}
		PendingMipmap[] array;
		lock (_sync)
		{
			FlushPendingTextureCommandsLocked();
			TextureResourceManager.ProcessDeferredDisposals(_renderFrameSerial);
			array = ((_pendingMipmaps.Count > 0) ? _pendingMipmaps.ToArray() : Array.Empty<PendingMipmap>());
			_pendingMipmaps.Clear();
		}
		if (array.Length != 0)
		{
			Mipmapper.GenerateMipmaps(Array.ConvertAll(array, (PendingMipmap m) => new Mipmapper.MipmapRequest(m.Texture, m.Width, m.Height, m.MipCount, m.MinMipDimension, m.PreserveAlphaCoverage, m.AlphaCoverageCutoff, m.Premultiplied)));
		}
	}

	public static bool TryGetSampledImageInfo(Texture texture, out VkImageView imageView, out VkImageLayout imageLayout)
	{
		GpuTextureAllocation gpuResource = texture.GpuResource;
		if (gpuResource == null && texture.EnsureResident())
		{
			gpuResource = texture.GpuResource;
		}
		if (gpuResource == null)
		{
			imageView = default(VkImageView);
			imageLayout = VkImageLayout.Undefined;
			return false;
		}
		gpuResource.MarkUsed();
		imageView = gpuResource.ImageView;
		VkImageLayout currentLayout = gpuResource.CurrentLayout;
		bool flag = (uint)(currentLayout - 6) <= 1u;
		imageLayout = (flag ? VkImageLayout.ShaderReadOnlyOptimal : currentLayout);
		return true;
	}

	public static void DeferDispose(GpuTextureAllocation allocation)
	{
		allocation.Dispose();
	}

	internal static string? FindModRoot(string dir)
	{
		return _modRootByDir.GetOrAdd(dir, delegate(string dir)
		{
			for (string text = dir; text != null; text = Path.GetDirectoryName(text))
			{
				if (ResourceFiles.DirectoryExists(Path.Combine(text, "About")))
				{
					return text;
				}
			}
			return (string?)null;
		});
	}

	internal static string TextureTrackerName(string fullPath)
	{
		string directoryName = Path.GetDirectoryName(fullPath);
		if (directoryName == null)
		{
			return Path.GetFileName(fullPath);
		}
		string text = FindModRoot(directoryName);
		string text2 = ((text == null) ? null : Path.GetDirectoryName(text));
		if (text2 == null)
		{
			int num = fullPath.Replace('\\', '/').LastIndexOf("/Resources/", StringComparison.Ordinal);
			if (num >= 0)
			{
				return fullPath.Replace('\\', '/').Substring(num + 1);
			}
		}
		return ((text2 == null) ? fullPath : Path.GetRelativePath(text2, fullPath)).Replace('\\', '/');
	}

	public static void RetireDeferredDisposalsAfterIdle()
	{
		TryRetireDeferredDisposalsAfterIdle(RenderFrameSerial);
	}

	internal static bool TryRetireDeferredDisposalsAfterIdle(long movedAwayRetireFrame)
	{
		if (_retiring || Monitor.IsEntered(_sync) || !RenderThread.TryWaitUntilIdleFromProducer(RenderThreadDrainTimeout))
		{
			return false;
		}
		_retiring = true;
		try
		{
			FlushPendingTextureCommands();
			lock (_sync)
			{
				if (_graphicsDevice == null)
				{
					return false;
				}
				_graphicsDevice.WaitIdleAndRun(delegate
				{
					TextureResourceManager.RetireAllNow(movedAwayRetireFrame);
					ComputeBufferResourceManager.DisposeAllImmediate();
					DrainDeferredDisposalsLocked(force: true);
				});
			}
		}
		finally
		{
			_retiring = false;
		}
		return true;
	}

	public static ImageAllocationInfo AllocateTextureImage(VmaAllocator allocator, string label, GpuMemoryCategory category, in VkImageCreateInfo createInfo)
	{
		bool flag = TextureResidency.IsOffloadable(category, createInfo.Usage);
		AllocationInfo.CreateInfo createInfo2 = default(AllocationInfo.CreateInfo);
		createInfo2.Preference = MemoryPreference.PreferGpu;
		createInfo2.RequiredProperties = ((!flag) ? VkMemoryPropertyFlags.DeviceLocalBit : VkMemoryPropertyFlags.None);
		createInfo2.PreferredProperties = VkMemoryPropertyFlags.DeviceLocalBit;
		createInfo2.Priority = 1f;
		AllocationInfo.CreateInfo allocationInfo = createInfo2;
		long pendingDisposalBytes = TextureResourceManager.PendingDisposalBytes;
		if (pendingDisposalBytes >= Volatile.Read(in _proactiveRetireAtBytes))
		{
			Volatile.Write(ref _proactiveRetireAtBytes, TryRetireDeferredDisposalsAfterIdle(-1L) ? 268435456 : (pendingDisposalBytes + 268435456));
		}
		long num = EstimateImageBytes(in createInfo);
		ImageAllocationInfo allocation2 = default(ImageAllocationInfo);
		try
		{
			if (!TextureResidency.IsPastHighWatermark(num))
			{
				return AllocateImageRetryingOnce(allocator, label, in createInfo, in allocationInfo);
			}
			if (TextureResourceManager.PendingDisposalBytes >= System.Math.Max(67108864L, num) && TryRetireDeferredDisposalsAfterIdle(-1L) && !TextureResidency.IsPastHighWatermark(num))
			{
				return AllocateImageRetryingOnce(allocator, label, in createInfo, in allocationInfo);
			}
			if (flag && TextureResidency.TryAllocateInSystemMemory(allocator, label, in createInfo, out var allocation))
			{
				return allocation;
			}
			if (!flag)
			{
				TextureResidency.MakeRoom(num);
			}
			return AllocateImageRetryingOnce(allocator, label, in createInfo, in allocationInfo);
		}
		catch (VmaAllocationException ex) when (ex.Result == VkResult.ErrorOutOfDeviceMemory && flag && TextureResidency.TryAllocateInSystemMemory(allocator, label, in createInfo, out allocation2))
		{
			return allocation2;
		}
	}

	internal static ImageAllocationInfo AllocateImageRetryingOnce(VmaAllocator allocator, string label, in VkImageCreateInfo createInfo, in AllocationInfo.CreateInfo allocationInfo, uint memoryTypeBits = 0u)
	{
		return allocator.AllocateImageChecked(label, in createInfo, in allocationInfo, memoryTypeBits);
	}

	internal static long EstimateImageBytes(in VkImageCreateInfo createInfo)
	{
		(long Bytes, long Size) tuple = FormatBlock(createInfo.Format);
		long item = tuple.Bytes;
		long item2 = tuple.Size;
		long num = System.Math.Max(1, createInfo.Extent.Width);
		long num2 = System.Math.Max(1, createInfo.Extent.Height);
		long num3 = System.Math.Max(1, createInfo.Extent.Depth);
		long num4 = 0L;
		for (int i = 0; i < System.Math.Max(1, createInfo.MipLevels); i++)
		{
			num4 += (num + item2 - 1) / item2 * ((num2 + item2 - 1) / item2) * num3 * item;
			num = System.Math.Max(1L, num / 2);
			num2 = System.Math.Max(1L, num2 / 2);
			num3 = System.Math.Max(1L, num3 / 2);
		}
		return num4 * System.Math.Max(1, createInfo.ArrayLayers);
	}

	private static (long Bytes, long Size) FormatBlock(VkFormat format)
	{
		(int, int) tuple;
		switch (format)
		{
		case VkFormat.Bc1RGBUNormBlock:
		case VkFormat.Bc1RGBSRGBBlock:
		case VkFormat.Bc1RGBAUNormBlock:
		case VkFormat.Bc1RGBASRGBBlock:
		case VkFormat.Bc4UNormBlock:
		case VkFormat.Bc4SNormBlock:
			tuple = (8, 4);
			break;
		case VkFormat.Bc2UNormBlock:
		case VkFormat.Bc2SRGBBlock:
		case VkFormat.Bc3UNormBlock:
		case VkFormat.Bc3SRGBBlock:
		case VkFormat.Bc5UNormBlock:
		case VkFormat.Bc5SNormBlock:
		case VkFormat.Bc6HUfloatBlock:
		case VkFormat.Bc6HSFloatBlock:
		case VkFormat.Bc7UNormBlock:
		case VkFormat.Bc7SRGBBlock:
			tuple = (16, 4);
			break;
		case VkFormat.R8UNorm:
		case VkFormat.R8SNorm:
		case VkFormat.A8UNorm:
			tuple = (1, 1);
			break;
		case VkFormat.R8G8UNorm:
		case VkFormat.R16UNorm:
		case VkFormat.R16SFloat:
		case VkFormat.D16UNorm:
			tuple = (2, 1);
			break;
		case VkFormat.R16G16B16A16UNorm:
		case VkFormat.R16G16B16A16SFloat:
		case VkFormat.R32G32SFloat:
		case VkFormat.D32SFloatS8UInt:
			tuple = (8, 1);
			break;
		case VkFormat.R32G32B32SFloat:
			tuple = (12, 1);
			break;
		case VkFormat.R32G32B32A32SFloat:
			tuple = (16, 1);
			break;
		default:
			tuple = (4, 1);
			break;
		}
		(int, int) tuple2 = tuple;
		return (Bytes: tuple2.Item1, Size: tuple2.Item2);
	}

	internal static long MoveTextures(List<Texture> textures, bool toSystemMemory)
	{
		if (_device == null || _allocator == null || textures.Count == 0)
		{
			return 0L;
		}
		DeviceEx device = _device;
		VmaAllocator allocator = _allocator;
		List<(Texture, GpuTextureAllocation, GpuTextureAllocation)> list = new List<(Texture, GpuTextureAllocation, GpuTextureAllocation)>(textures.Count);
		lock (_sync)
		{
			FlushPendingTextureCommandsLocked();
			foreach (Texture texture2 in textures)
			{
				GpuTextureAllocation gpuResource = texture2.GpuResource;
				if (gpuResource != null && CanMoveLocked(gpuResource, toSystemMemory))
				{
					GpuTextureAllocation gpuTextureAllocation = TryAllocateMoveTarget(device, allocator, gpuResource, toSystemMemory);
					if (gpuTextureAllocation != null)
					{
						list.Add((texture2, gpuResource, gpuTextureAllocation));
					}
				}
			}
			if (list.Count == 0)
			{
				return 0L;
			}
			CommandBuffer commandBuffer = device.AllocateCommandBuffer(new VkCommandBufferAllocateInfo
			{
				CommandPool = _commandPool,
				Level = VkCommandBufferLevel.Primary
			});
			try
			{
				commandBuffer.Begin(VkCommandBufferUsageFlags.OneTimeSubmitBit);
				foreach (var item in list)
				{
					RecordMoveCopy(commandBuffer, item.Item2, item.Item3);
				}
				commandBuffer.End();
				VkFence orCreateFenceLocked = GetOrCreateFenceLocked();
				try
				{
					CommandBuffer reference = commandBuffer;
					SubmitGraphicsLocked(new Span<CommandBuffer>(ref reference), orCreateFenceLocked);
					device.WaitForFence(orCreateFenceLocked, -1);
				}
				finally
				{
					ReturnFenceLocked(orCreateFenceLocked);
				}
			}
			catch
			{
				foreach (var item2 in list)
				{
					item2.Item3.DisposeNow();
				}
				throw;
			}
			finally
			{
				if (!commandBuffer.Handle.IsNull())
				{
					VkCommandPool commandPool = _commandPool;
					CommandBuffer reference2 = commandBuffer;
					device.FreeCommandBuffers(commandPool, new ReadOnlySpan<CommandBuffer>(in reference2));
				}
			}
		}
		long num = 0L;
		foreach (var (texture, gpuTextureAllocation2, gpuTextureAllocation3) in list)
		{
			if (texture.GpuResource != gpuTextureAllocation2)
			{
				gpuTextureAllocation3.DisposeNow();
				continue;
			}
			num += gpuTextureAllocation2.AllocatedBytes;
			gpuTextureAllocation3.InheritUse(gpuTextureAllocation2);
			texture.GpuResource = gpuTextureAllocation3;
			gpuTextureAllocation2.DisposeMovedAway((gpuTextureAllocation2.LastUseFrame < 0) ? 0 : (gpuTextureAllocation2.LastUseFrame + 3));
		}
		return num;
	}

	private static bool CanMoveLocked(GpuTextureAllocation allocation, bool toSystemMemory)
	{
		if (!allocation.Movable || allocation.IsDisposed || allocation.IsHostResident == toSystemMemory || IsDepthFormat(allocation.Format))
		{
			return false;
		}
		if (allocation.LastWriteFrame >= 0 && allocation.LastWriteFrame + 3 > RenderFrameSerial)
		{
			return false;
		}
		VkImageLayout currentLayout = allocation.CurrentLayout;
		if ((currentLayout == VkImageLayout.Undefined || (uint)(currentLayout - 6) <= 1u) ? true : false)
		{
			return false;
		}
		for (int i = 0; i < allocation.MipCount; i++)
		{
			if (allocation.GetMipLayout(i) != allocation.CurrentLayout)
			{
				return false;
			}
		}
		foreach (PendingMipmap pendingMipmap in _pendingMipmaps)
		{
			if (pendingMipmap.Texture == allocation)
			{
				return false;
			}
		}
		return true;
	}

	private static GpuTextureAllocation? TryAllocateMoveTarget(DeviceEx device, VmaAllocator allocator, GpuTextureAllocation from, bool toSystemMemory)
	{
		ref readonly ImageAllocationInfo allocationInfo = ref from.AllocationInfo;
		VkImageCreateInfo vkImageCreateInfo = new VkImageCreateInfo();
		vkImageCreateInfo.ImageType = from.ImageType;
		vkImageCreateInfo.Format = from.Format;
		vkImageCreateInfo.Extent = allocationInfo.Extent;
		vkImageCreateInfo.MipLevels = allocationInfo.MipLevels;
		vkImageCreateInfo.ArrayLayers = allocationInfo.ArrayLayers;
		vkImageCreateInfo.Samples = VkSampleCountFlags._1Bit;
		vkImageCreateInfo.Tiling = VkImageTiling.Optimal;
		vkImageCreateInfo.Usage = from.Usage;
		vkImageCreateInfo.SharingMode = VkSharingMode.Exclusive;
		vkImageCreateInfo.InitialLayout = VkImageLayout.Undefined;
		VkImageCreateInfo imageCreateInfo = vkImageCreateInfo;
		ImageAllocationInfo allocation;
		if (toSystemMemory)
		{
			if (!TextureResidency.TryAllocateInSystemMemory(allocator, from.DebugLabel, in imageCreateInfo, out allocation))
			{
				return null;
			}
		}
		else
		{
			try
			{
				string debugLabel = from.DebugLabel;
				AllocationInfo.CreateInfo allocateCreateInfo = new AllocationInfo.CreateInfo
				{
					Preference = MemoryPreference.PreferGpu,
					RequiredProperties = VkMemoryPropertyFlags.DeviceLocalBit,
					PreferredProperties = VkMemoryPropertyFlags.DeviceLocalBit,
					Priority = 1f
				};
				allocation = allocator.AllocateImageChecked(debugLabel, in imageCreateInfo, in allocateCreateInfo);
			}
			catch (VmaAllocationException)
			{
				return null;
			}
		}
		VkImageView imageView = device.CreateImageView(new VkImageViewCreateInfo
		{
			Image = allocation.Image,
			ViewType = from.ViewType,
			Format = from.Format,
			Components = from.Swizzle.ToVk(),
			SubresourceRange = new VkImageSubresourceRange
			{
				AspectMask = VkImageAspectFlags.ColorBit,
				BaseMipLevel = 0,
				LevelCount = from.MipCount,
				BaseArrayLayer = 0,
				LayerCount = allocationInfo.ArrayLayers
			}
		}, device.VkAllocator);
		GpuTextureAllocation obj = new GpuTextureAllocation(handle: new IntPtr(Interlocked.Increment(ref _nextHandle)), device: device, allocator: allocator, allocation: allocation, imageView: imageView, mipCount: from.MipCount, format: from.Format, debugLabel: from.DebugLabel, category: from.Category);
		obj.SetImageInfo(from.ImageType, from.Usage, from.ViewType, from.Swizzle);
		GpuMemoryTracker.SetDetail(in allocation, TextureDetail(allocationInfo.Extent.Width, allocationInfo.Extent.Height, from.Format, from.MipCount, allocationInfo.ArrayLayers, allocationInfo.Extent.Depth));
		return obj;
	}

	private static void RecordMoveCopy(CommandBuffer commandBuffer, GpuTextureAllocation from, GpuTextureAllocation to)
	{
		ref readonly ImageAllocationInfo allocationInfo = ref from.AllocationInfo;
		int arrayLayers = allocationInfo.ArrayLayers;
		VkImageLayout currentLayout = from.CurrentLayout;
		InlineArray2<VkImageMemoryBarrier> buffer = default(InlineArray2<VkImageMemoryBarrier>);
		global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray2<VkImageMemoryBarrier>, VkImageMemoryBarrier>(ref buffer, 0) = WholeImageBarrier(from.Image, currentLayout, VkImageLayout.TransferSrcOptimal, VkAccessFlags.MemoryWriteBit, VkAccessFlags.TransferReadBit);
		global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray2<VkImageMemoryBarrier>, VkImageMemoryBarrier>(ref buffer, 1) = WholeImageBarrier(to.Image, VkImageLayout.Undefined, VkImageLayout.TransferDstOptimal, VkAccessFlags.None, VkAccessFlags.TransferWriteBit);
		commandBuffer.PipelineBarrier(VkPipelineStageFlags.AllCommandsBit, VkPipelineStageFlags.TransferBit, VkDependencyFlags.None, default(ReadOnlySpan<VkMemoryBarrier>), default(ReadOnlySpan<VkBufferMemoryBarrier>), global::_003CPrivateImplementationDetails_003E.InlineArrayAsReadOnlySpan<InlineArray2<VkImageMemoryBarrier>, VkImageMemoryBarrier>(in buffer, 2));
		VkImageCopy[] array = new VkImageCopy[from.MipCount];
		for (int i = 0; i < array.Length; i++)
		{
			VkImageSubresourceLayers vkImageSubresourceLayers = new VkImageSubresourceLayers();
			vkImageSubresourceLayers.AspectMask = VkImageAspectFlags.ColorBit;
			vkImageSubresourceLayers.MipLevel = i;
			vkImageSubresourceLayers.BaseArrayLayer = 0;
			vkImageSubresourceLayers.LayerCount = arrayLayers;
			VkImageSubresourceLayers vkImageSubresourceLayers2 = vkImageSubresourceLayers;
			array[i] = new VkImageCopy
			{
				SrcSubresource = vkImageSubresourceLayers2,
				DstSubresource = vkImageSubresourceLayers2,
				SrcOffset = new VkOffset3D(0, 0, 0),
				DstOffset = new VkOffset3D(0, 0, 0),
				Extent = new VkExtent3D(System.Math.Max(1, allocationInfo.Extent.Width >> i), System.Math.Max(1, allocationInfo.Extent.Height >> i), System.Math.Max(1, allocationInfo.Extent.Depth >> i))
			};
		}
		commandBuffer.CopyImage(from.Image, VkImageLayout.TransferSrcOptimal, to.Image, VkImageLayout.TransferDstOptimal, array);
		InlineArray2<VkImageMemoryBarrier> buffer2 = default(InlineArray2<VkImageMemoryBarrier>);
		global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray2<VkImageMemoryBarrier>, VkImageMemoryBarrier>(ref buffer2, 0) = WholeImageBarrier(from.Image, VkImageLayout.TransferSrcOptimal, currentLayout, VkAccessFlags.TransferReadBit, VkAccessFlags.ShaderReadBit);
		global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray2<VkImageMemoryBarrier>, VkImageMemoryBarrier>(ref buffer2, 1) = WholeImageBarrier(to.Image, VkImageLayout.TransferDstOptimal, currentLayout, VkAccessFlags.TransferWriteBit, VkAccessFlags.ShaderReadBit);
		commandBuffer.PipelineBarrier(VkPipelineStageFlags.TransferBit, VkPipelineStageFlags.AllCommandsBit, VkDependencyFlags.None, default(ReadOnlySpan<VkMemoryBarrier>), default(ReadOnlySpan<VkBufferMemoryBarrier>), global::_003CPrivateImplementationDetails_003E.InlineArrayAsReadOnlySpan<InlineArray2<VkImageMemoryBarrier>, VkImageMemoryBarrier>(in buffer2, 2));
		for (int j = 0; j < to.MipCount; j++)
		{
			to.SetMipLayout(j, currentLayout);
		}
	}

	private static VkImageMemoryBarrier WholeImageBarrier(VkImage image, VkImageLayout oldLayout, VkImageLayout newLayout, VkAccessFlags sourceAccess, VkAccessFlags destinationAccess)
	{
		VkImageMemoryBarrier result = new VkImageMemoryBarrier();
		result.SrcAccessMask = sourceAccess;
		result.DstAccessMask = destinationAccess;
		result.OldLayout = oldLayout;
		result.NewLayout = newLayout;
		result.SrcQueueFamilyIndex = -1;
		result.DstQueueFamilyIndex = -1;
		result.Image = image;
		result.SubresourceRange = new VkImageSubresourceRange
		{
			AspectMask = VkImageAspectFlags.ColorBit,
			BaseMipLevel = 0,
			LevelCount = -1,
			BaseArrayLayer = 0,
			LayerCount = -1
		};
		return result;
	}

	private static void ShutdownLocked()
	{
		if (_device != null)
		{
			FlushPendingTextureCommandsLocked();
			DrainDeferredDisposalsLocked(force: true);
			_whiteFallbackTexture?.Dispose();
			_whiteFallbackTexture = null;
			_blackFallbackTexture?.Dispose();
			_blackFallbackTexture = null;
			_grayFallbackTexture?.Dispose();
			_grayFallbackTexture = null;
			_whiteArrayFallbackTexture?.Dispose();
			_whiteArrayFallbackTexture = null;
			Mipmapper.Shutdown();
			VkFence result;
			while (_fencePool.TryPop(out result))
			{
				_device.DestroyFence(result, _device.VkAllocator);
			}
			_device.DestroyCommandPool(_commandPool, _device.VkAllocator);
			_commandPool = default(VkCommandPool);
			_graphicsQueue = null;
			_physicalDevice = null;
			if (_graphicsDevice != null)
			{
				_graphicsDevice.BeforeSubmit = null;
			}
			_graphicsDevice = null;
			_pendingTextureBatch = null;
			_pendingMipmaps.Clear();
			_renderFrameSerial = 0L;
			_device = null;
		}
	}

	internal static void SubmitGraphicsLocked(Span<CommandBuffer> commandBuffers, VkFence fence)
	{
		if (_graphicsDevice != null)
		{
			_graphicsDevice.SubmitGraphics(default(Span<VkSemaphore>), default(Span<VkPipelineStageFlags>), commandBuffers, default(Span<VkSemaphore>), fence);
		}
		else
		{
			_graphicsQueue.Submit(default(Span<VkSemaphore>), default(Span<VkPipelineStageFlags>), commandBuffers, default(Span<VkSemaphore>), fence);
		}
	}

	private static void SubmitPendingTextureCommands()
	{
		if ((Volatile.Read(in _pendingTextureBatch) == null && _submittedTextureBatches.Count == 0) || Monitor.IsEntered(_sync))
		{
			return;
		}
		lock (_sync)
		{
			FlushPendingTextureCommandsLocked(wait: false);
		}
	}

	private static void FlushPendingTextureCommandsLocked(bool wait = true)
	{
		if (_device == null)
		{
			return;
		}
		RetireSubmittedTextureBatchesLocked(wait);
		if (_pendingTextureBatch == null)
		{
			return;
		}
		PendingTextureBatch pendingTextureBatch = _pendingTextureBatch;
		_pendingTextureBatch = null;
		if (!pendingTextureBatch.CommandBuffer.Handle.IsNull())
		{
			foreach (var (gpuTextureAllocation, num) in pendingTextureBatch.TouchedSubresources)
			{
				if (gpuTextureAllocation.Image.IsNull())
				{
					LogCategory log = RenderLog.Log;
					bool shouldAppend;
					LogString<Error> message = new LogString<Error>(89, 2, log, out shouldAppend);
					if (shouldAppend)
					{
						message.AppendLiteral("Texture upload batch names a freed allocation '");
						message.AppendFormatted(gpuTextureAllocation.DebugLabel);
						message.AppendLiteral("' (mip ");
						message.AppendFormatted(num);
						message.AppendLiteral("); skipping its completion barrier.");
					}
					log.Error(message, "FlushPendingTextureCommandsLocked", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Rendering\\GpuTextureService.cs", 1842);
				}
				else
				{
					TransitionTouchedToShaderRead(pendingTextureBatch.CommandBuffer, gpuTextureAllocation, num);
				}
			}
			pendingTextureBatch.CommandBuffer.End();
			VkFence orCreateFenceLocked = GetOrCreateFenceLocked();
			CommandBuffer reference = pendingTextureBatch.CommandBuffer;
			SubmitGraphicsLocked(new Span<CommandBuffer>(ref reference), orCreateFenceLocked);
			_submittedTextureBatches.Enqueue((orCreateFenceLocked, pendingTextureBatch.CommandBuffer, pendingTextureBatch.StagingBuffers, pendingTextureBatch.TouchedSubresources));
			RetireSubmittedTextureBatchesLocked(wait);
			return;
		}
		foreach (BufferAllocationInfo stagingBuffer in pendingTextureBatch.StagingBuffers)
		{
			ReturnStagingBuffer(stagingBuffer);
		}
	}

	private static void RetireSubmittedTextureBatchesLocked(bool wait)
	{
		DeviceEx device = _device;
		(VkFence, CommandBuffer, List<BufferAllocationInfo>, HashSet<(GpuTextureAllocation, int)>) result;
		while (_submittedTextureBatches.TryPeek(out result))
		{
			if (wait)
			{
				using (FrameProfiler.Scope("TextureBatchGpuWait"))
				{
					device.WaitForFence(result.Item1, -1);
				}
			}
			else if (device.GetFenceStatus(result.Item1) != 0)
			{
				break;
			}
			_submittedTextureBatches.Dequeue();
			ReturnFenceLocked(result.Item1);
			VkCommandPool commandPool = _commandPool;
			CommandBuffer reference = result.Item2;
			device.FreeCommandBuffers(commandPool, new ReadOnlySpan<CommandBuffer>(in reference));
			foreach (BufferAllocationInfo item in result.Item3)
			{
				ReturnStagingBuffer(item);
			}
			foreach (var item2 in result.Item4)
			{
				item2.Item1.ReleasePendingUpload();
			}
		}
	}

	private static void DrainDeferredDisposalsLocked(bool force)
	{
		if (!force)
		{
			return;
		}
		using (_stagingSync.EnterScope())
		{
			foreach (BufferAllocationInfo item in _stagingBufferPool)
			{
				BufferAllocationInfo allocation = item;
				GpuMemoryTracker.Remove(GpuMemoryCategory.Staging, in allocation);
				_allocator?.FreeAllocation(in allocation);
			}
			_stagingBufferPool.Clear();
		}
		if (_readbackBufferSize > 0)
		{
			_allocator?.FreeAllocation(in _readbackBuffer);
			_readbackBuffer = default(BufferAllocationInfo);
			_readbackBufferSize = 0L;
		}
		if (_ticketBufferSize > 0 && !_ticketOutstanding)
		{
			_allocator?.FreeAllocation(in _ticketBuffer);
			_ticketBuffer = default(BufferAllocationInfo);
			_ticketBufferSize = 0L;
		}
	}

	private static BufferAllocationInfo RentStagingBuffer(string label, long size)
	{
		if (_device == null || _allocator == null)
		{
			return default(BufferAllocationInfo);
		}
		VmaAllocator allocator = _allocator;
		using (_stagingSync.EnterScope())
		{
			for (int i = 0; i < _stagingBufferPool.Count; i++)
			{
				BufferAllocationInfo result = _stagingBufferPool[i];
				if ((long)result.BufferSize >= size)
				{
					_stagingBufferPool.RemoveAt(i);
					string.IsNullOrEmpty(label);
					return result;
				}
			}
		}
		BufferAllocationInfo allocation = allocator.AllocateBufferChecked(label, new VkBufferCreateInfo
		{
			Size = ByteSize.Of(size),
			Usage = VkBufferUsageFlags.TransferSrcBit,
			SharingMode = VkSharingMode.Exclusive
		}, GpuInfo.StageUploadsInVram ? GpuComputeBuffer.UploadStaging : new AllocationInfo.CreateInfo
		{
			Preference = MemoryPreference.PreferCpu,
			RequiredProperties = (VkMemoryPropertyFlags.HostVisibleBit | VkMemoryPropertyFlags.HostCoherentBit),
			PreferredProperties = (VkMemoryPropertyFlags.HostVisibleBit | VkMemoryPropertyFlags.HostCoherentBit),
			Priority = 1f
		});
		GpuMemoryTracker.Add(GpuMemoryCategory.Staging, in allocation, "Staging/Texture");
		return allocation;
	}

	private static void ReturnStagingBuffer(BufferAllocationInfo buffer)
	{
		if (buffer.VkBuffer.IsNull())
		{
			return;
		}
		using (_stagingSync.EnterScope())
		{
			if (_stagingBufferPool.Count < 2)
			{
				_stagingBufferPool.Add(buffer);
				return;
			}
		}
		if (_allocator != null)
		{
			GpuMemoryTracker.Remove(GpuMemoryCategory.Staging, in buffer);
			_allocator.FreeAllocation(in buffer);
		}
	}

	private unsafe static BufferAllocationInfo RentFilledStagingBuffer(string label, int byteCount, int level, TextureLevelFill fill)
	{
		BufferAllocationInfo result = RentStagingBuffer(label, byteCount);
		AllocationMapInfo allocationMapInfo = new AllocationMapInfo(result.AllocationInfo);
		allocationMapInfo.Region = new MapRegion
		{
			Offset = ByteSize.Zero,
			Size = result.BufferSize
		};
		AllocationMapInfo allocationMapInfo2 = allocationMapInfo;
		Ptr ptr = _allocator.MapMemory(in allocationMapInfo2);
		fill(level, new Span<byte>((void*)(nint)ptr, byteCount));
		_allocator.UnmapMemory(in allocationMapInfo2);
		return result;
	}

	private static BufferAllocationInfo RentFilledStagingBuffer(string label, byte[] bytes)
	{
		BufferAllocationInfo result = RentStagingBuffer(label, bytes.Length);
		AllocationMapInfo allocationMapInfo = new AllocationMapInfo(result.AllocationInfo);
		allocationMapInfo.Region = new MapRegion
		{
			Offset = ByteSize.Zero,
			Size = result.BufferSize
		};
		AllocationMapInfo allocationMapInfo2 = allocationMapInfo;
		Ptr ptr = _allocator.MapMemory(in allocationMapInfo2);
		Marshal.Copy(bytes, 0, (nint)ptr, bytes.Length);
		_allocator.UnmapMemory(in allocationMapInfo2);
		return result;
	}

	public unsafe static void Clear(GpuTextureAllocation texture, float4 color, bool batch = false)
	{
		if (_device == null)
		{
			return;
		}
		VkImageSubresourceRange range = new VkImageSubresourceRange
		{
			AspectMask = VkImageAspectFlags.ColorBit,
			BaseMipLevel = 0,
			LevelCount = texture.MipCount,
			BaseArrayLayer = 0,
			LayerCount = 1
		};
		ExecuteImageCommands(delegate(CommandBuffer commandBuffer)
		{
			for (int i = 0; i < texture.MipCount; i++)
			{
				TransitionSubresourceLayout(commandBuffer, texture, i, VkImageLayout.TransferDstOptimal, VkAccessFlags.None, VkAccessFlags.TransferWriteBit, VkPipelineStageFlags.TopOfPipeBit, VkPipelineStageFlags.TransferBit);
			}
			VkClearColorValue vkClearColorValue = new VkClearColorValue
			{
				Float32 = color
			};
			VkImage image = texture.Image;
			VkClearColorValue* pColor = &vkClearColorValue;
			VkImageSubresourceRange reference = range;
			commandBuffer.ClearColorImage(image, VkImageLayout.TransferDstOptimal, pColor, new ReadOnlySpan<VkImageSubresourceRange>(in reference));
		}, batch, (texture, -1));
	}

	public static void UploadRgba32(GpuTextureAllocation texture, int textureWidth, int textureHeight, int mipLevel, ReadOnlySpan<byte> bytes, bool batch = false)
	{
		if (_device != null && _allocator != null && bytes.Length != 0)
		{
			VmaAllocator allocator = _allocator;
			byte[] data = bytes.ToArray();
			ExecuteImageCommands(delegate(CommandBuffer commandBuffer, List<BufferAllocationInfo> stagingBuffers)
			{
				BufferAllocationInfo item = RentStagingBuffer($"{texture.DebugLabel}.Upload[{mipLevel}]", data.Length);
				AllocationMapInfo allocationMapInfo = new AllocationMapInfo(item.AllocationInfo)
				{
					Region = new MapRegion
					{
						Offset = ByteSize.Zero,
						Size = item.BufferSize
					}
				};
				Ptr ptr = allocator.MapMemory(in allocationMapInfo);
				Marshal.Copy(data, 0, (nint)ptr, data.Length);
				allocator.UnmapMemory(in allocationMapInfo);
				stagingBuffers.Add(item);
				TransitionSubresourceLayout(commandBuffer, texture, mipLevel, VkImageLayout.TransferDstOptimal, VkAccessFlags.None, VkAccessFlags.TransferWriteBit, VkPipelineStageFlags.TopOfPipeBit, VkPipelineStageFlags.TransferBit);
				VkBuffer vkBuffer = item.VkBuffer;
				VkImage image = texture.Image;
				VkBufferImageCopy reference = new VkBufferImageCopy
				{
					BufferOffset = ByteSize.Zero,
					BufferRowLength = textureWidth,
					BufferImageHeight = 0,
					ImageSubresource = new VkImageSubresourceLayers
					{
						AspectMask = VkImageAspectFlags.ColorBit,
						MipLevel = mipLevel,
						BaseArrayLayer = 0,
						LayerCount = 1
					},
					ImageOffset = new VkOffset3D(0, 0, 0),
					ImageExtent = new VkExtent3D(textureWidth, textureHeight, 1)
				};
				commandBuffer.CopyBufferToImage(vkBuffer, image, VkImageLayout.TransferDstOptimal, new ReadOnlySpan<VkBufferImageCopy>(in reference));
			}, batch, (texture, mipLevel));
		}
	}

	public static void UploadRgbaFloat(GpuTextureAllocation texture, int textureWidth, int textureHeight, int mipLevel, ReadOnlySpan<byte> bytes, bool batch = false)
	{
		if (_device != null && _allocator != null && bytes.Length != 0)
		{
			VmaAllocator allocator = _allocator;
			byte[] data = bytes.ToArray();
			ExecuteImageCommands(delegate(CommandBuffer commandBuffer, List<BufferAllocationInfo> stagingBuffers)
			{
				BufferAllocationInfo item = RentStagingBuffer($"{texture.DebugLabel}.UploadFloat[{mipLevel}]", data.Length);
				AllocationMapInfo allocationMapInfo = new AllocationMapInfo(item.AllocationInfo)
				{
					Region = new MapRegion
					{
						Offset = ByteSize.Zero,
						Size = item.BufferSize
					}
				};
				Ptr ptr = allocator.MapMemory(in allocationMapInfo);
				Marshal.Copy(data, 0, (nint)ptr, data.Length);
				allocator.UnmapMemory(in allocationMapInfo);
				stagingBuffers.Add(item);
				TransitionSubresourceLayout(commandBuffer, texture, mipLevel, VkImageLayout.TransferDstOptimal, VkAccessFlags.None, VkAccessFlags.TransferWriteBit, VkPipelineStageFlags.TopOfPipeBit, VkPipelineStageFlags.TransferBit);
				VkBuffer vkBuffer = item.VkBuffer;
				VkImage image = texture.Image;
				VkBufferImageCopy reference = new VkBufferImageCopy
				{
					BufferOffset = ByteSize.Zero,
					BufferRowLength = textureWidth,
					BufferImageHeight = 0,
					ImageSubresource = new VkImageSubresourceLayers
					{
						AspectMask = VkImageAspectFlags.ColorBit,
						MipLevel = mipLevel,
						BaseArrayLayer = 0,
						LayerCount = 1
					},
					ImageOffset = new VkOffset3D(0, 0, 0),
					ImageExtent = new VkExtent3D(textureWidth, textureHeight, 1)
				};
				commandBuffer.CopyBufferToImage(vkBuffer, image, VkImageLayout.TransferDstOptimal, new ReadOnlySpan<VkBufferImageCopy>(in reference));
			}, batch, (texture, mipLevel));
		}
	}

	public static void CopyTexture(GpuTextureAllocation source, int srcWidth, int srcHeight, int srcMip, int srcX, int srcY, int copyWidth, int copyHeight, GpuTextureAllocation destination, int dstMip, int dstX, int dstY, bool batch = false)
	{
		ExecuteImageCommands(delegate(CommandBuffer commandBuffer)
		{
			VkImageLayout mipLayout = source.GetMipLayout(srcMip);
			VkImageLayout mipLayout2 = destination.GetMipLayout(dstMip);
			TransitionSubresourceLayout(commandBuffer, source, srcMip, VkImageLayout.TransferSrcOptimal, GetTransferSourceAccessMask(mipLayout), VkAccessFlags.TransferReadBit, GetTransferSourceStageMask(mipLayout), VkPipelineStageFlags.TransferBit);
			TransitionSubresourceLayout(commandBuffer, destination, dstMip, VkImageLayout.TransferDstOptimal, GetTransferSourceAccessMask(mipLayout2), VkAccessFlags.TransferWriteBit, GetTransferSourceStageMask(mipLayout2), VkPipelineStageFlags.TransferBit);
			VkImage image = source.Image;
			VkImage image2 = destination.Image;
			VkImageCopy reference = new VkImageCopy
			{
				SrcSubresource = new VkImageSubresourceLayers
				{
					AspectMask = VkImageAspectFlags.ColorBit,
					MipLevel = srcMip,
					BaseArrayLayer = 0,
					LayerCount = 1
				},
				SrcOffset = new VkOffset3D(srcX, srcY, 0),
				DstSubresource = new VkImageSubresourceLayers
				{
					AspectMask = VkImageAspectFlags.ColorBit,
					MipLevel = dstMip,
					BaseArrayLayer = 0,
					LayerCount = 1
				},
				DstOffset = new VkOffset3D(dstX, dstY, 0),
				Extent = new VkExtent3D(copyWidth, copyHeight, 1)
			};
			commandBuffer.CopyImage(image, VkImageLayout.TransferSrcOptimal, image2, VkImageLayout.TransferDstOptimal, new ReadOnlySpan<VkImageCopy>(in reference));
		}, batch, (source, srcMip), (destination, dstMip));
	}

	internal static void CopyTexturesToArray(IReadOnlyList<GpuTextureAllocation> sources, GpuTextureAllocation destination, int width, int height, int mipCount)
	{
		ArgumentNullException.ThrowIfNull(sources, "sources");
		ArgumentNullException.ThrowIfNull(destination, "destination");
		if (sources.Count == 0)
		{
			throw new ArgumentException("At least one source texture is required.", "sources");
		}
		ExecuteImageCommands(delegate(CommandBuffer commandBuffer)
		{
			foreach (GpuTextureAllocation source in sources)
			{
				VkImageLayout currentLayout = source.CurrentLayout;
				TransitionLayout(commandBuffer, source, VkImageLayout.TransferSrcOptimal, GetTransferSourceAccessMask(currentLayout), VkAccessFlags.TransferReadBit, GetTransferSourceStageMask(currentLayout), VkPipelineStageFlags.TransferBit);
			}
			VkImageLayout currentLayout2 = destination.CurrentLayout;
			TransitionLayout(commandBuffer, destination, VkImageLayout.TransferDstOptimal, GetTransferSourceAccessMask(currentLayout2), VkAccessFlags.TransferWriteBit, GetTransferSourceStageMask(currentLayout2), VkPipelineStageFlags.TransferBit);
			for (int i = 0; i < sources.Count; i++)
			{
				VkImageCopy[] array = new VkImageCopy[mipCount];
				for (int j = 0; j < mipCount; j++)
				{
					array[j] = new VkImageCopy
					{
						SrcSubresource = new VkImageSubresourceLayers
						{
							AspectMask = VkImageAspectFlags.ColorBit,
							MipLevel = j,
							BaseArrayLayer = 0,
							LayerCount = 1
						},
						DstSubresource = new VkImageSubresourceLayers
						{
							AspectMask = VkImageAspectFlags.ColorBit,
							MipLevel = j,
							BaseArrayLayer = i,
							LayerCount = 1
						},
						SrcOffset = new VkOffset3D(0, 0, 0),
						DstOffset = new VkOffset3D(0, 0, 0),
						Extent = new VkExtent3D(System.Math.Max(1, width >> j), System.Math.Max(1, height >> j), 1)
					};
				}
				commandBuffer.CopyImage(sources[i].Image, VkImageLayout.TransferSrcOptimal, destination.Image, VkImageLayout.TransferDstOptimal, array);
			}
			foreach (GpuTextureAllocation source2 in sources)
			{
				TransitionLayout(commandBuffer, source2, VkImageLayout.ShaderReadOnlyOptimal, VkAccessFlags.TransferReadBit, VkAccessFlags.ShaderReadBit, VkPipelineStageFlags.TransferBit, VkPipelineStageFlags.FragmentShaderBit);
			}
		}, false, (destination, -1));
	}

	public static void BlitTexture(GpuTextureAllocation source, int srcWidth, int srcHeight, int srcMip, int srcX, int srcY, int srcRectWidth, int srcRectHeight, GpuTextureAllocation destination, int dstMip, int dstX, int dstY, int dstRectWidth, int dstRectHeight, VkFilter filter, bool batch = false)
	{
		if (!source.Swizzle.IsIdentity)
		{
			LogCategory log = RenderLog.Log;
			bool shouldAppend;
			LogString<Warning> message = new LogString<Warning>(96, 2, log, out shouldAppend);
			if (shouldAppend)
			{
				message.AppendLiteral("Blitting from '");
				message.AppendFormatted(source.DebugLabel);
				message.AppendLiteral("' drops its view swizzle ");
				message.AppendFormatted(source.Swizzle);
				message.AppendLiteral(": image blits copy texels, not what a sampler would see.");
			}
			log.Warning(message, "BlitTexture", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Rendering\\GpuTextureService.cs", 2405);
		}
		ExecuteImageCommands(delegate(CommandBuffer commandBuffer)
		{
			VkImageLayout mipLayout = source.GetMipLayout(srcMip);
			VkImageLayout mipLayout2 = destination.GetMipLayout(dstMip);
			TransitionSubresourceLayout(commandBuffer, source, srcMip, VkImageLayout.TransferSrcOptimal, GetTransferSourceAccessMask(mipLayout), VkAccessFlags.TransferReadBit, GetTransferSourceStageMask(mipLayout), VkPipelineStageFlags.TransferBit);
			TransitionSubresourceLayout(commandBuffer, destination, dstMip, VkImageLayout.TransferDstOptimal, GetTransferSourceAccessMask(mipLayout2), VkAccessFlags.TransferWriteBit, GetTransferSourceStageMask(mipLayout2), VkPipelineStageFlags.TransferBit);
			VkImageBlit vkImageBlit = new VkImageBlit
			{
				SrcSubresource = new VkImageSubresourceLayers
				{
					AspectMask = VkImageAspectFlags.ColorBit,
					MipLevel = srcMip,
					BaseArrayLayer = 0,
					LayerCount = 1
				},
				DstSubresource = new VkImageSubresourceLayers
				{
					AspectMask = VkImageAspectFlags.ColorBit,
					MipLevel = dstMip,
					BaseArrayLayer = 0,
					LayerCount = 1
				}
			};
			global::_003CPrivateImplementationDetails_003E.InlineArrayFirstElementRef<VkOffset3D_2, VkOffset3D>(ref vkImageBlit.SrcOffsets) = new VkOffset3D(srcX, srcY, 0);
			global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<VkOffset3D_2, VkOffset3D>(ref vkImageBlit.SrcOffsets, 1) = new VkOffset3D(srcX + srcRectWidth, srcY + srcRectHeight, 1);
			global::_003CPrivateImplementationDetails_003E.InlineArrayFirstElementRef<VkOffset3D_2, VkOffset3D>(ref vkImageBlit.DstOffsets) = new VkOffset3D(dstX, dstY, 0);
			global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<VkOffset3D_2, VkOffset3D>(ref vkImageBlit.DstOffsets, 1) = new VkOffset3D(dstX + dstRectWidth, dstY + dstRectHeight, 1);
			VkImage image = source.Image;
			VkImage image2 = destination.Image;
			VkImageBlit reference = vkImageBlit;
			commandBuffer.BlitImage(image, VkImageLayout.TransferSrcOptimal, image2, VkImageLayout.TransferDstOptimal, new ReadOnlySpan<VkImageBlit>(in reference), filter);
		}, batch, (source, srcMip), (destination, dstMip));
	}

	private static VkAccessFlags GetTransferSourceAccessMask(VkImageLayout layout)
	{
		return layout switch
		{
			VkImageLayout.TransferDstOptimal => VkAccessFlags.TransferWriteBit, 
			VkImageLayout.TransferSrcOptimal => VkAccessFlags.TransferReadBit, 
			VkImageLayout.ShaderReadOnlyOptimal => VkAccessFlags.ShaderReadBit, 
			VkImageLayout.ColorAttachmentOptimal => VkAccessFlags.ColorAttachmentWriteBit, 
			_ => VkAccessFlags.None, 
		};
	}

	private static VkPipelineStageFlags GetTransferSourceStageMask(VkImageLayout layout)
	{
		return layout switch
		{
			VkImageLayout.TransferDstOptimal => VkPipelineStageFlags.TransferBit, 
			VkImageLayout.TransferSrcOptimal => VkPipelineStageFlags.TransferBit, 
			VkImageLayout.ShaderReadOnlyOptimal => VkPipelineStageFlags.FragmentShaderBit, 
			VkImageLayout.ColorAttachmentOptimal => VkPipelineStageFlags.ColorAttachmentOutputBit, 
			_ => VkPipelineStageFlags.TopOfPipeBit, 
		};
	}

	public static void GenerateMipmaps(GpuTextureAllocation texture, int width, int height, int mipCount, int minMipDimension = 1, bool preserveAlphaCoverage = false, float alphaCoverageCutoff = 0.5f, bool premultiplied = false)
	{
		if (UploadsDeferred)
		{
			lock (_sync)
			{
				_pendingMipmaps.Add(new PendingMipmap(texture, width, height, mipCount, minMipDimension, preserveAlphaCoverage, alphaCoverageCutoff, premultiplied));
				return;
			}
		}
		Mipmapper.GenerateMipmaps(texture, width, height, mipCount, minMipDimension, preserveAlphaCoverage, alphaCoverageCutoff, premultiplied);
	}

	private static bool BorderMipMapsFor(string fullPath)
	{
		return WantsBorderMipMaps?.Invoke(fullPath) ?? false;
	}

	public static LoadedTextureData LoadTextureFromFile(string filePath, bool premultiplyAlpha = false)
	{
		bool cached;
		return LoadTextureFromFile(filePath, premultiplyAlpha, out cached);
	}

	public static LoadedTextureData LoadTextureFromFile(string filePath, bool premultiplyAlpha, out bool cached)
	{
		bool gpuBuild;
		return LoadTextureFromFile(filePath, premultiplyAlpha, allowGpuBuild: false, out gpuBuild, out cached);
	}

	internal static LoadedTextureData LoadTextureFromFile(string filePath, bool premultiplyAlpha, bool allowGpuBuild, out bool gpuBuild, out bool cached)
	{
		string fullPath = Path.GetFullPath(filePath);
		bool flag = BorderMipMapsFor(fullPath);
		cached = false;
		(long, long)? sourceStamp = CpuMeshCache.StampSource(fullPath);
		LoadedTextureData loaded = DecodeTextureFile(fullPath)with
		{
			SourceStamp = sourceStamp
		};
		if (premultiplyAlpha)
		{
			PremultiplyAlphaInPlace(in loaded, fullPath);
		}
		gpuBuild = allowGpuBuild && !flag && TextureBuildBatch.CanBuild(in loaded);
		if (gpuBuild)
		{
			return loaded;
		}
		LoadedTextureData obj = ((LoadTimeEncodeOnGpu && !flag && TextureCompressionSettings.RdoRules.Length == 0) ? TryCompressAtLoadOnGpu(in loaded, fullPath, premultiplyAlpha) : ((LoadedTextureData?)null)) ?? CompressAtLoad(in loaded, fullPath, flag, premultiplyAlpha);
		loaded = obj with
		{
			SourceStamp = sourceStamp
		};
		cached = TryWriteTextureCache(fullPath, in loaded, premultiplyAlpha);
		return loaded;
	}

	internal static LoadedTextureData DecodeTextureFile(string fullPath)
	{
		if (!ExtensionMatchesContent(fullPath))
		{
			LogCategory log = RenderLog.Log;
			bool shouldAppend;
			LogString<Error> message = new LogString<Error>(108, 1, log, out shouldAppend);
			if (shouldAppend)
			{
				message.AppendFormatted(fullPath);
				message.AppendLiteral(": the file's contents are not the format its extension names; rename it so the decoder meant for it reads it");
			}
			log.Error(message, "DecodeTextureFile", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Rendering\\GpuTextureService.cs", 2552);
		}
		if (TryLoadTextureFromUnsupportedFile(fullPath, out var loadedTexture))
		{
			return loadedTexture;
		}
		if (TryDecodePngManaged(fullPath, out var loaded))
		{
			return loaded;
		}
		string extension = Path.GetExtension(fullPath);
		string text = extension.ToLowerInvariant();
		if (!TextureLoader.TryGetFormatType(text, out var formatType))
		{
			throw new NotSupportedException("Texture format '" + fullPath + "' is not supported.");
		}
		using (FrameProfiler.Scope("TextureFileDecode"))
		{
			ITexture texture;
			byte[] sanitized = default(byte[]);
			try
			{
				texture = ((extension == text) ? TextureLoader.LoadFromFile(fullPath) : TextureLoader.LoadFromMemory(File.ReadAllBytes(fullPath), formatType));
			}
			catch when (formatType == TextureLoader.FormatType.Png && PngChunkSanitizer.TryStripInvalidChunks(File.ReadAllBytes(fullPath), out sanitized))
			{
				texture = TextureLoader.LoadFromMemory(sanitized, formatType);
			}
			try
			{
				return ConvertToLoadedTextureData(texture);
			}
			finally
			{
				TextureLoader.Unload(texture);
			}
		}
	}

	private static LoadedTextureData? TryCompressAtLoadOnGpu(in LoadedTextureData loaded, string fullPath, bool premultiplied)
	{
		if (loaded.Format != LoadedTextureFormat.Rgba32 || loaded.Levels.Length == 0 || loaded.Levels[0] == null || TexturePixelData.Rgba32MipChainFilter != 0)
		{
			return null;
		}
		var (bcEncodeTarget2, textureSwizzle2) = (TextureEncoding)(ref TextureCompressionSettings.SelectLoadTimeEncoding(in loaded, fullPath));
		if (bcEncodeTarget2 == BcEncodeTarget.None)
		{
			return null;
		}
		long timestamp = Stopwatch.GetTimestamp();
		int mipCount = ((loaded.MipCount > 1) ? loaded.MipCount : TexturePixelData.CalculateMipCount(loaded.Width, loaded.Height));
		byte[][] array;
		using (FrameProfiler.Scope("GpuLoadTimeEncode"))
		{
			array = TextureBlockEncoder.TryEncodeFromPixelsOnGpu(loaded.Levels, loaded.Width, loaded.Height, mipCount, bcEncodeTarget2, TextureCompressionSettings.Quality, premultiplied);
		}
		if (array == null)
		{
			return null;
		}
		VerboseLog.Log($"Load-time encode {bcEncodeTarget2} {loaded.Width}x{loaded.Height} on GPU: {Stopwatch.GetElapsedTime(timestamp).TotalMilliseconds:F2} ms");
		if (LoadTimeEncodeMode == "verify")
		{
			VerifyAgainstCpu(EnsureFullMipChain(in loaded, borderMipMaps: false, premultiplied, TexturePixelData.Rgba32MipChainFilter), bcEncodeTarget2, array);
		}
		int width = loaded.Width;
		int height = loaded.Height;
		int mipCount2 = array.Length;
		LoadedTextureFormat format = ToLoadedFormat(bcEncodeTarget2);
		bool opaque = loaded.Opaque;
		TextureSwizzle swizzle = textureSwizzle2;
		return new LoadedTextureData(width, height, mipCount2, format, array, opaque, null, swizzle);
	}

	private static void VerifyAgainstCpu(in LoadedTextureData loaded, BcEncodeTarget target, byte[][] gpuBlocks)
	{
		byte[][] array = CpuBlockEncoder.EncodeLevels(loaded.Levels, loaded.Width, loaded.Height, loaded.MipCount, target, parallel: false);
		int num = target.BlockBytes();
		long num2 = 0L;
		long num3 = 0L;
		for (int i = 0; i < gpuBlocks.Length; i++)
		{
			byte[] array2 = gpuBlocks[i];
			byte[] array3 = array[i];
			for (int j = 0; j < array2.Length; j += num)
			{
				num2++;
				if (!((ReadOnlySpan<byte>)array2.AsSpan(j, num)).SequenceEqual((ReadOnlySpan<byte>)array3.AsSpan(j, num)))
				{
					num3++;
				}
			}
		}
		VerboseLog.Log($"Load-time encode verify {target} {loaded.Width}x{loaded.Height}: {num3}/{num2} blocks differ");
	}

	private static LoadedTextureData CompressAtLoad(in LoadedTextureData decoded, string fullPath, bool borderMipMaps, bool premultiplied)
	{
		return CompressMipChain(in decoded, fullPath, borderMipMaps, premultiplied, TextureBakeSettings.FromGlobals());
	}

	public static LoadedTextureData LoadTextureForBake(string filePath, bool premultiplyAlpha, TextureBakeSettings settings)
	{
		string fullPath = Path.GetFullPath(filePath);
		LoadedTextureData loaded = DecodeTextureFile(fullPath);
		if (premultiplyAlpha)
		{
			PremultiplyAlphaInPlace(in loaded, fullPath);
		}
		return CompressMipChain(in loaded, fullPath, settings.BorderMipMaps?.Invoke(fullPath) ?? false, premultiplyAlpha, settings);
	}

	private static LoadedTextureData CompressMipChain(in LoadedTextureData decoded, string fullPath, bool borderMipMaps, bool premultiplied, TextureBakeSettings settings)
	{
		long timestamp = Stopwatch.GetTimestamp();
		LoadedTextureData result = EnsureFullMipChain(in decoded, borderMipMaps, premultiplied, settings.MipFilter);
		if (result.Format != LoadedTextureFormat.Rgba32 || result.Levels.Length == 0)
		{
			return result;
		}
		TextureUsage textureUsage = settings.Usage(fullPath);
		var (bcEncodeTarget2, textureSwizzle2) = (TextureEncoding)(ref TextureEncodingRules.Refine(settings.TargetFor(result.Width, result.Height, result.Opaque, fullPath), result.Levels[0], result.Width, result.Height, result.Opaque, textureUsage));
		if (bcEncodeTarget2 == BcEncodeTarget.None)
		{
			return result;
		}
		BcRdoRule? bcRdoRule = settings.SelectRdo(bcEncodeTarget2, fullPath);
		bool num = settings.EncodeOnGpu && !bcRdoRule.HasValue && TextureBlockEncoder.IsConfigured && (bcEncodeTarget2 != BcEncodeTarget.Bc7 || settings.Bc7Flags == Bc7PackFlags.WaylithDefault);
		byte[][] array = null;
		if (num)
		{
			using (FrameProfiler.Scope("GpuLoadTimeEncode"))
			{
				array = TextureBlockEncoder.TryEncodeFromPixelsOnGpu(result.Levels, result.Width, result.Height, result.MipCount, bcEncodeTarget2, settings.Quality, premultiplied);
			}
		}
		bool flag = array != null;
		if (array == null)
		{
			using (FrameProfiler.Scope("CpuLoadTimeEncode"))
			{
				array = CpuBlockEncoder.EncodeLevels(result.Levels, result.Width, result.Height, result.MipCount, bcEncodeTarget2, settings.Bc7Flags, parallel: false);
			}
			if (bcRdoRule.HasValue)
			{
				BcRdoRule valueOrDefault = bcRdoRule.GetValueOrDefault();
				int levelCount = ((textureUsage == TextureUsage.AtlasTile) ? 1 : result.MipCount);
				using (FrameProfiler.Scope("CpuRdo"))
				{
					CpuBlockEncoder.ReduceEntropy(array, result.Levels, result.Width, result.Height, levelCount, bcEncodeTarget2, valueOrDefault.Lambda, valueOrDefault.Window, parallel: false);
				}
			}
		}
		VerboseLog.Log($"Load-time encode {bcEncodeTarget2} {result.Width}x{result.Height} on {(flag ? "GPU" : "CPU")}: {Stopwatch.GetElapsedTime(timestamp).TotalMilliseconds:F2} ms");
		int width = result.Width;
		int height = result.Height;
		int mipCount = array.Length;
		LoadedTextureFormat format = ToLoadedFormat(bcEncodeTarget2);
		byte[][] levels = array;
		bool opaque = result.Opaque;
		TextureSwizzle swizzle = textureSwizzle2;
		return new LoadedTextureData(width, height, mipCount, format, levels, opaque, null, swizzle);
	}

	private static LoadedTextureData EnsureFullMipChain(in LoadedTextureData loaded, bool borderMipMaps, bool premultiplied, MipFilter filter)
	{
		if (loaded.Format != LoadedTextureFormat.Rgba32 || loaded.MipCount > 1 || loaded.Levels.Length == 0 || loaded.Levels[0] == null)
		{
			return loaded;
		}
		int num = TexturePixelData.CalculateMipCount(loaded.Width, loaded.Height);
		if (num <= 1)
		{
			return loaded;
		}
		using (FrameProfiler.Scope("CpuMipChainBuild"))
		{
			byte[][] levels = TexturePixelData.GenerateRgba32MipChain(loaded.Levels[0], loaded.Width, loaded.Height, num, filter, parallel: false, null, premultiplied);
			if (borderMipMaps)
			{
				TexturePixelData.ApplyBorderMipMaps(levels, loaded.Width, loaded.Height);
			}
			return new LoadedTextureData(loaded.Width, loaded.Height, num, loaded.Format, levels, loaded.Opaque, null);
		}
	}

	private static void PremultiplyAlphaInPlace(in LoadedTextureData loaded, string fullPath)
	{
		if (loaded.Format != LoadedTextureFormat.Rgba32)
		{
			LogCategory log = RenderLog.Log;
			bool shouldAppend;
			LogString<Warning> message = new LogString<Warning>(107, 2, log, out shouldAppend);
			if (shouldAppend)
			{
				message.AppendLiteral("[UITextures] '");
				message.AppendFormatted(fullPath);
				message.AppendLiteral("' decoded as ");
				message.AppendFormatted(loaded.Format);
				message.AppendLiteral(", not RGBA32; cannot premultiply. It will render with incorrect alpha in Noesis.");
			}
			log.Warning(message, "PremultiplyAlphaInPlace", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Rendering\\GpuTextureService.cs", 2717);
			return;
		}
		byte[][] levels = loaded.Levels;
		foreach (byte[] array in levels)
		{
			if (array == null)
			{
				continue;
			}
			for (int j = 0; j + 3 < array.Length; j += 4)
			{
				int num = array[j + 3];
				if (num != 255)
				{
					array[j] = (byte)((array[j] * num + 127) / 255);
					array[j + 1] = (byte)((array[j + 1] * num + 127) / 255);
					array[j + 2] = (byte)((array[j + 2] * num + 127) / 255);
				}
			}
		}
	}

	public static string TextureDetail(int width, int height, VkFormat format, int mipCount, int layers = 1, int depth = 1)
	{
		int num = System.Math.Max(1, mipCount);
		string text = $"{width}x{height} {format}, {num} mip{((num == 1) ? "" : "s")}";
		if (layers > 1)
		{
			text += $", {layers} layers";
		}
		if (depth > 1)
		{
			text += $", {depth} deep";
		}
		return text;
	}

	public static LoadedTextureFormat ToLoadedFormat(BcEncodeTarget target)
	{
		return target switch
		{
			BcEncodeTarget.Bc1 => LoadedTextureFormat.Dxt1, 
			BcEncodeTarget.Bc7 => LoadedTextureFormat.Bc7, 
			BcEncodeTarget.Bc5 => LoadedTextureFormat.Bc5, 
			BcEncodeTarget.Bc4 => LoadedTextureFormat.Bc4, 
			_ => LoadedTextureFormat.Rgba32, 
		};
	}

	public static BcEncodeTarget ToTarget(LoadedTextureFormat format)
	{
		return format switch
		{
			LoadedTextureFormat.Dxt1 => BcEncodeTarget.Bc1, 
			LoadedTextureFormat.Bc7 => BcEncodeTarget.Bc7, 
			LoadedTextureFormat.Bc5 => BcEncodeTarget.Bc5, 
			LoadedTextureFormat.Bc4 => BcEncodeTarget.Bc4, 
			_ => BcEncodeTarget.None, 
		};
	}

	private static bool TryLoadTextureFromUnsupportedFile(string filePath, out LoadedTextureData loadedTexture)
	{
		loadedTexture = default(LoadedTextureData);
		if (!string.Equals(Path.GetExtension(filePath), ".psd", StringComparison.OrdinalIgnoreCase))
		{
			return false;
		}
		return TryLoadTextureWithPsdSharp(filePath, out loadedTexture);
	}

	private static bool TryLoadTextureWithPsdSharp(string filePath, out LoadedTextureData loadedTexture)
	{
		loadedTexture = default(LoadedTextureData);
		try
		{
			using FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
			return TryLoadTextureWithPsdSharp(stream, filePath, out loadedTexture);
		}
		catch (Exception ex)
		{
			LogCategory log = RenderLog.Log;
			bool shouldAppend;
			LogString<Error> message = new LogString<Error>(59, 2, log, out shouldAppend);
			if (shouldAppend)
			{
				message.AppendLiteral("Unable to load PSD file ");
				message.AppendFormatted(Path.GetFileName(filePath));
				message.AppendLiteral(" due to an exception in PsdSharp: ");
				message.AppendFormatted(ex.InnerException?.Message ?? ex.Message);
				message.AppendLiteral(")");
			}
			log.Error(message, "TryLoadTextureWithPsdSharp", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Rendering\\GpuTextureService.cs", 2795);
			return false;
		}
	}

	private static bool TryLoadTextureWithPsdSharp(Stream stream, string nameForLog, out LoadedTextureData loadedTexture)
	{
		loadedTexture = default(LoadedTextureData);
		bool shouldAppend;
		try
		{
			PsdFile psdFile = PsdFile.Open(stream, new PsdLoadOptions
			{
				LeaveInputOpen = false,
				ImageDataLoading = ImageDataLoading.LoadImmediately
			});
			foreach (string warning in psdFile.Warnings)
			{
				LogCategory log = RenderLog.Log;
				LogCategory logCategory = log;
				LogString<Warning> message = new LogString<Warning>(11, 2, log, out shouldAppend);
				if (shouldAppend)
				{
					message.AppendLiteral("PSD file ");
					message.AppendFormatted(nameForLog);
					message.AppendLiteral(": ");
					message.AppendFormatted(warning);
				}
				logCategory.Warning(message, "TryLoadTextureWithPsdSharp", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Rendering\\GpuTextureService.cs", 2813);
			}
			if (psdFile?.ImageData == null)
			{
				shouldAppend = false;
				return shouldAppend;
			}
			int num;
			int num2;
			checked
			{
				num = (int)psdFile.Header.WidthInPixels;
				num2 = (int)psdFile.Header.HeightInPixels;
			}
			byte[] array;
			if (psdFile.Header.ChannelDepth == 16 && psdFile.ImageData.ColorMode == ColorMode.Rgb)
			{
				List<byte[]> list = new List<byte[]>();
				foreach (ImageData.ChannelData channel in psdFile.ImageData.GetChannels())
				{
					list.Add(channel.GetData());
				}
				int num3 = num * num2;
				array = new byte[num3 * 4];
				for (int i = 0; i < num3; i++)
				{
					for (int j = 0; j < 4; j++)
					{
						array[i * 4 + j] = ((j < list.Count) ? list[j][i * 2] : byte.MaxValue);
					}
				}
			}
			else
			{
				array = PixelDataConverter.GetInterleavedBuffer(psdFile.ImageData, ColorType.Rgba8888);
				if (array.Length == 0)
				{
					shouldAppend = false;
					return shouldAppend;
				}
			}
			RemoveMatte(array);
			BleedTransparentPixels(array, num, num2);
			loadedTexture = new LoadedTextureData(num, num2, 1, LoadedTextureFormat.Rgba32, new byte[1][] { array }, TexturePixelData.IsOpaqueRgba32(array), null);
			shouldAppend = true;
			return shouldAppend;
		}
		catch (Exception ex)
		{
			LogCategory log = RenderLog.Log;
			LogCategory logCategory2 = log;
			bool shouldAppend2;
			LogString<Error> message2 = new LogString<Error>(54, 2, log, out shouldAppend2);
			if (shouldAppend2)
			{
				message2.AppendLiteral("Unable to load PSD ");
				message2.AppendFormatted(nameForLog);
				message2.AppendLiteral(" due to an exception in PsdSharp: ");
				message2.AppendFormatted(ex.InnerException?.Message ?? ex.Message);
				message2.AppendLiteral(")");
			}
			logCategory2.Error(message2, "TryLoadTextureWithPsdSharp", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Rendering\\GpuTextureService.cs", 2858);
			shouldAppend = false;
			return shouldAppend;
		}
	}

	public static bool ExtensionMatchesContent(string path)
	{
		string text = Path.GetExtension(path).ToLowerInvariant();
		bool flag;
		switch (text)
		{
		case ".png":
		case ".jpg":
		case ".jpeg":
		case ".psd":
			flag = true;
			break;
		default:
			flag = false;
			break;
		}
		if (!flag)
		{
			return true;
		}
		Span<byte> span = stackalloc byte[8];
		using (SafeFileHandle handle = File.OpenHandle(path, FileMode.Open, FileAccess.Read, FileShare.Read, FileOptions.None, 0L))
		{
			span = span.Slice(0, RandomAccess.Read(handle, span, 0L));
		}
		if (text == ".psd" || IsPsd(span))
		{
			if (text == ".psd")
			{
				return IsPsd(span);
			}
			return false;
		}
		if (TryDetectFormat(span, out var format))
		{
			return format == (TextureLoader.FormatType)((text == ".png") ? 10 : 4);
		}
		return false;
	}

	private static bool IsPsd(ReadOnlySpan<byte> bytes)
	{
		if (bytes.Length >= 4 && bytes[0] == 56 && bytes[1] == 66 && bytes[2] == 80)
		{
			return bytes[3] == 83;
		}
		return false;
	}

	private static void RemoveMatte(byte[] rgba)
	{
		long num = 0L;
		long num2 = 0L;
		long num3 = 0L;
		long num4 = 0L;
		for (int i = 0; i + 3 < rgba.Length; i += 4)
		{
			if (rgba[i + 3] == 0)
			{
				num += rgba[i];
				num2 += rgba[i + 1];
				num3 += rgba[i + 2];
				num4++;
			}
		}
		float num5 = ((num4 > 0) ? ((float)num / (float)num4) : 255f);
		float num6 = ((num4 > 0) ? ((float)num2 / (float)num4) : 255f);
		float num7 = ((num4 > 0) ? ((float)num3 / (float)num4) : 255f);
		for (int j = 0; j + 3 < rgba.Length; j += 4)
		{
			int num8 = rgba[j + 3];
			if (num8 != 0 && num8 != 255)
			{
				float invA = 255f / (float)num8;
				float num9 = (float)(255 - num8) / 255f;
				UnMatteChannel(rgba, j, num5 * num9, invA);
				UnMatteChannel(rgba, j + 1, num6 * num9, invA);
				UnMatteChannel(rgba, j + 2, num7 * num9, invA);
			}
		}
	}

	private static void UnMatteChannel(byte[] rgba, int idx, float matteTerm, float invA)
	{
		float num = ((float)(int)rgba[idx] - matteTerm) * invA;
		rgba[idx] = (byte)((num < 0f) ? 0f : ((num > 255f) ? 255f : num));
	}

	public static bool TryLoadTextureFromMemory(ReadOnlySpan<byte> bytes, out LoadedTextureData loadedTexture)
	{
		loadedTexture = default(LoadedTextureData);
		if (IsPsd(bytes))
		{
			using (MemoryStream stream = new MemoryStream(bytes.ToArray()))
			{
				return TryLoadTextureWithPsdSharp(stream, "<memory>", out loadedTexture);
			}
		}
		if (!TryDetectFormat(bytes, out var format))
		{
			return false;
		}
		ITexture texture = TextureLoader.LoadFromMemory(bytes, format);
		try
		{
			loadedTexture = ConvertToLoadedTextureData(texture);
			return true;
		}
		finally
		{
			TextureLoader.Unload(texture);
		}
	}

	private static bool TryDecodePngManaged(string fullPath, out LoadedTextureData loaded)
	{
		loaded = default(LoadedTextureData);
		if (!fullPath.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
		{
			return false;
		}
		using (FrameProfiler.Scope("TextureFileDecode"))
		{
			int width;
			int height;
			byte[] array = PngDecoder.TryDecodeRgba32(File.ReadAllBytes(fullPath), out width, out height);
			if (array == null)
			{
				return false;
			}
			BleedTransparentPixels(array, width, height);
			loaded = new LoadedTextureData(width, height, 1, LoadedTextureFormat.Rgba32, new byte[1][] { array }, TexturePixelData.IsOpaqueRgba32(array), null);
			return true;
		}
	}

	private static LoadedTextureData ConvertToLoadedTextureData(ITexture source)
	{
		int x = source.Extent.X;
		int y = source.Extent.Y;
		int num = System.Math.Max(1, source.LevelCount);
		byte[][] array = new byte[num][];
		for (int i = 0; i < num; i++)
		{
			int width = System.Math.Max(1, x >> i);
			int num2 = System.Math.Max(1, y >> i);
			Span<byte> span = source.ImageData(i, 0, 0);
			int sourceRowPitch = ((num2 > 0) ? (span.Length / num2) : 0);
			array[i] = ConvertToRgba32(source.Format, span, width, num2, sourceRowPitch);
			BleedTransparentPixels(array[i], width, num2);
		}
		return new LoadedTextureData(x, y, num, LoadedTextureFormat.Rgba32, array, TexturePixelData.IsOpaqueRgba32(array[0]), null);
	}

	private static byte[] ConvertToRgba32(TextureFormat sourceFormat, ReadOnlySpan<byte> sourceBytes, int width, int height, int sourceRowPitch)
	{
		byte[] array = new byte[width * height * 4];
		int num = GetSourceBytesPerPixel(sourceFormat) * width;
		int num2 = ((sourceRowPitch <= 0) ? num : sourceRowPitch);
		switch (sourceFormat)
		{
		case TextureFormat.R8G8B8A8_UNorm:
		case TextureFormat.R8G8B8A8_SRGB:
			if (num2 == num)
			{
				sourceBytes.Slice(0, array.Length).CopyTo(array);
			}
			else
			{
				for (int m = 0; m < height; m++)
				{
					sourceBytes.Slice(m * num2, num).CopyTo(array.AsSpan(m * num));
				}
			}
			return array;
		case TextureFormat.R8G8B8_UNorm:
		case TextureFormat.R8G8B8_SRGB:
		{
			for (int k = 0; k < height; k++)
			{
				ReadOnlySpan<byte> readOnlySpan = sourceBytes.Slice(k * num2, num);
				Span<byte> span = array.AsSpan(k * width * 4, width * 4);
				int num6 = 0;
				for (int l = 0; l < span.Length; l += 4)
				{
					span[l] = readOnlySpan[num6];
					span[l + 1] = readOnlySpan[num6 + 1];
					span[l + 2] = readOnlySpan[num6 + 2];
					span[l + 3] = byte.MaxValue;
					num6 += 3;
				}
			}
			return array;
		}
		default:
		{
			for (int i = 0; i < height; i++)
			{
				ReadOnlySpan<byte> row = sourceBytes.Slice(i * num2, System.Math.Min(num2, sourceBytes.Length - i * num2));
				int num3 = i * width * 4;
				for (int j = 0; j < width; j++)
				{
					int num4 = j * GetSourceBytesPerPixel(sourceFormat);
					int num5 = num3 + j * 4;
					switch (sourceFormat)
					{
					case TextureFormat.R8_UNorm:
						array[num5 + 2] = (array[num5 + 1] = (array[num5] = row[num4]));
						array[num5 + 3] = byte.MaxValue;
						break;
					case TextureFormat.R8G8_UNorm:
						array[num5 + 2] = (array[num5 + 1] = (array[num5] = row[num4]));
						array[num5 + 3] = row[num4 + 1];
						break;
					case TextureFormat.B8G8R8_UNorm:
					case TextureFormat.B8G8R8_SRGB:
						array[num5] = row[num4 + 2];
						array[num5 + 1] = row[num4 + 1];
						array[num5 + 2] = row[num4];
						array[num5 + 3] = byte.MaxValue;
						break;
					case TextureFormat.B8G8R8A8_UNorm:
					case TextureFormat.B8G8R8A8_SRGB:
						array[num5] = row[num4 + 2];
						array[num5 + 1] = row[num4 + 1];
						array[num5 + 2] = row[num4];
						array[num5 + 3] = row[num4 + 3];
						break;
					case TextureFormat.R16_UNorm:
						array[num5 + 2] = (array[num5 + 1] = (array[num5] = Sample16To8(row, num4)));
						array[num5 + 3] = byte.MaxValue;
						break;
					case TextureFormat.R16G16_UNorm:
						array[num5 + 2] = (array[num5 + 1] = (array[num5] = Sample16To8(row, num4)));
						array[num5 + 3] = Sample16To8(row, num4 + 2);
						break;
					case TextureFormat.R16G16B16_UNorm:
						array[num5] = Sample16To8(row, num4);
						array[num5 + 1] = Sample16To8(row, num4 + 2);
						array[num5 + 2] = Sample16To8(row, num4 + 4);
						array[num5 + 3] = byte.MaxValue;
						break;
					case TextureFormat.R16G16B16A16_UNorm:
						array[num5] = Sample16To8(row, num4);
						array[num5 + 1] = Sample16To8(row, num4 + 2);
						array[num5 + 2] = Sample16To8(row, num4 + 4);
						array[num5 + 3] = Sample16To8(row, num4 + 6);
						break;
					default:
						throw new NotSupportedException($"Texture format '{sourceFormat}' is not supported yet.");
					}
				}
			}
			return array;
		}
		}
	}

	private static void BleedTransparentPixels(byte[] rgba, int width, int height)
	{
		if (rgba == null || rgba.Length != width * height * 4 || width <= 0 || height <= 0)
		{
			return;
		}
		int num = width * height;
		int num2 = 0;
		for (int i = 3; i < rgba.Length; i += 4)
		{
			if (rgba[i] != 0)
			{
				num2++;
			}
		}
		if (num2 == 0 || num2 == num)
		{
			return;
		}
		int[] array = ArrayPool<int>.Shared.Rent(num);
		bool[] array2 = ArrayPool<bool>.Shared.Rent(num);
		try
		{
			int tail = 0;
			for (int j = 0; j < num; j++)
			{
				if (array2[j] = rgba[j * 4 + 3] != 0)
				{
					array[tail++] = j;
				}
			}
			for (int k = 0; k < tail; k++)
			{
				int num3 = array[k];
				int num4 = num3 / width;
				int num5 = num3 - num4 * width;
				int sourceOffset = num3 * 4;
				if (num5 > 0 && !array2[num3 - 1])
				{
					Bleed(rgba, array2, array, ref tail, sourceOffset, num3 - 1);
				}
				if (num5 < width - 1 && !array2[num3 + 1])
				{
					Bleed(rgba, array2, array, ref tail, sourceOffset, num3 + 1);
				}
				if (num4 > 0 && !array2[num3 - width])
				{
					Bleed(rgba, array2, array, ref tail, sourceOffset, num3 - width);
				}
				if (num4 < height - 1 && !array2[num3 + width])
				{
					Bleed(rgba, array2, array, ref tail, sourceOffset, num3 + width);
				}
			}
		}
		finally
		{
			ArrayPool<int>.Shared.Return(array);
			ArrayPool<bool>.Shared.Return(array2);
		}
	}

	private static void Bleed(byte[] rgba, bool[] visited, int[] queue, ref int tail, int sourceOffset, int neighborIndex)
	{
		int num = neighborIndex * 4;
		rgba[num] = rgba[sourceOffset];
		rgba[num + 1] = rgba[sourceOffset + 1];
		rgba[num + 2] = rgba[sourceOffset + 2];
		visited[neighborIndex] = true;
		queue[tail++] = neighborIndex;
	}

	private static byte Sample16To8(ReadOnlySpan<byte> row, int index)
	{
		return (byte)(((row[index] | (row[index + 1] << 8)) * 255 + 32767) / 65535);
	}

	private static int GetSourceBytesPerPixel(TextureFormat format)
	{
		return format switch
		{
			TextureFormat.R8_UNorm => 1, 
			TextureFormat.R16_UNorm => 2, 
			TextureFormat.R16G16_UNorm => 4, 
			TextureFormat.R16G16B16_UNorm => 6, 
			TextureFormat.R16G16B16A16_UNorm => 8, 
			TextureFormat.R8G8B8_UNorm => 3, 
			TextureFormat.R8G8B8_SRGB => 3, 
			TextureFormat.B8G8R8_UNorm => 3, 
			TextureFormat.B8G8R8_SRGB => 3, 
			TextureFormat.R8G8B8A8_UNorm => 4, 
			TextureFormat.R8G8B8A8_SRGB => 4, 
			TextureFormat.B8G8R8A8_UNorm => 4, 
			TextureFormat.B8G8R8A8_SRGB => 4, 
			TextureFormat.R8G8_UNorm => 2, 
			_ => throw new NotSupportedException($"Texture format '{format}' is not supported yet."), 
		};
	}

	private static bool TryDetectFormat(ReadOnlySpan<byte> bytes, out TextureLoader.FormatType format)
	{
		format = TextureLoader.FormatType.Bmp;
		if (bytes.Length < 4)
		{
			return false;
		}
		if (bytes.Length >= 8 && bytes[0] == 137 && bytes[1] == 80 && bytes[2] == 78 && bytes[3] == 71 && bytes[4] == 13 && bytes[5] == 10 && bytes[6] == 26 && bytes[7] == 10)
		{
			format = TextureLoader.FormatType.Png;
			return true;
		}
		if (bytes[0] == byte.MaxValue && bytes[1] == 216)
		{
			format = TextureLoader.FormatType.Jpg;
			return true;
		}
		return false;
	}

	public static bool TryGetPackTextureInfo(string sourcePath, bool premultiplied, out AssetPackTextureInfo.Info info)
	{
		info = default(AssetPackTextureInfo.Info);
		if (TryGetCurrentPackEntry(sourcePath, premultiplied, out ContentPack.MountPoint mount, out string name))
		{
			return mount.TextureInfo.TryGetValue(name, out info);
		}
		return false;
	}

	public static bool TryDescribeTexture(string sourcePath, bool premultiplied, out AssetPackTextureInfo.Info info)
	{
		if (TryGetPackTextureInfo(sourcePath, premultiplied, out info))
		{
			return true;
		}
		string fullPath = Path.GetFullPath(sourcePath);
		return TextureCacheStore.TryGetInfo(fullPath, CacheVariant(fullPath, premultiplied), premultiplied, out info);
	}

	public static void NoteRehydrated(string sourcePath, int width, int height)
	{
		if (TrackRehydration)
		{
			Rehydrated.AddOrUpdate($"{width}x{height} {sourcePath}", 1, (string _, int count) => count + 1);
		}
	}

	private static bool TryGetCurrentPackEntry(string sourcePath, bool premultiplied, out ContentPack.MountPoint mount, out string name)
	{
		return ContentPack.TryGetCurrentMount(sourcePath, premultiplied ? "-premul.dds" : ".dds", out mount, out name);
	}

	private static bool TryOpenPackTexture(string sourcePath, bool premultiplied, [NotNullWhen(true)] out ITextureCacheSource? source)
	{
		source = null;
		if (!TryGetCurrentPackEntry(sourcePath, premultiplied, out ContentPack.MountPoint mount, out string name))
		{
			return false;
		}
		if (!mount.Pack.TryRent(name, out byte[] buffer, out int length) || !PackTextureSource.TryCreate(buffer, length, pooled: true, out PackTextureSource source2))
		{
			return false;
		}
		ITextureCacheSource textureCacheSource2;
		if (!mount.TextureInfo.TryGetValue(name, out var value) || value.MipCount <= source2.MipCount)
		{
			ITextureCacheSource textureCacheSource = source2;
			textureCacheSource2 = textureCacheSource;
		}
		else
		{
			ITextureCacheSource textureCacheSource = new CompletedChainTextureSource(source2, value.MipCount, premultiplied, BorderMipMapsFor(Path.GetFullPath(sourcePath)));
			textureCacheSource2 = textureCacheSource;
		}
		source = textureCacheSource2;
		return true;
	}

	internal static bool TryOpenTextureCache(string sourcePath, bool premultiplied, [NotNullWhen(true)] out ITextureCacheSource? file)
	{
		if (TryOpenPackTexture(sourcePath, premultiplied, out file))
		{
			Interlocked.Increment(ref PackTextureHits);
			return true;
		}
		if (ContentPack.Mounts.Count > 0)
		{
			Interlocked.Increment(ref PackTextureMisses);
			VerboseLog.Log("Asset pack miss: " + sourcePath);
		}
		file = null;
		string fullPath = Path.GetFullPath(sourcePath);
		return TextureCacheStore.TryOpen(fullPath, CacheVariant(fullPath, premultiplied), premultiplied, out file);
	}

	internal static bool TryLoadTextureFromCache(string sourcePath, bool premultiplied, out LoadedTextureData loadedTexture)
	{
		if (TryLoadTextureFromCacheDirect(sourcePath, premultiplied, out loadedTexture))
		{
			return true;
		}
		if (!File.Exists(sourcePath))
		{
			return false;
		}
		loadedTexture = LoadTextureFromFile(sourcePath, premultiplied, out var _);
		return true;
	}

	internal static bool TryLoadPersistedTexture(string fullPath, out LoadedTextureData loadedTexture)
	{
		if (AssetPrefetcher.TryTakeCached(fullPath, out loadedTexture, out var hit))
		{
			return hit;
		}
		return TryLoadTextureFromCacheDirect(fullPath, premultiplied: false, out loadedTexture);
	}

	internal static bool TryLoadTextureFromCacheDirect(string sourcePath, bool premultiplied, out LoadedTextureData loadedTexture)
	{
		loadedTexture = default(LoadedTextureData);
		if (!TryOpenTextureCache(sourcePath, premultiplied, out ITextureCacheSource file))
		{
			return false;
		}
		using (file)
		{
			return file.TryReadAll(out loadedTexture);
		}
	}

	public static bool TryOpenKeyedTextureCache(string cacheKey, [NotNullWhen(true)] out ITextureCacheSource? source)
	{
		source = null;
		if (string.IsNullOrWhiteSpace(cacheKey) || !TextureCacheStore.TryOpenKeyed(cacheKey, out source))
		{
			return false;
		}
		_keyedCacheUsed[cacheKey] = 0;
		return true;
	}

	public static bool TryLoadKeyedTextureCache(string cacheKey, out LoadedTextureData loadedTexture)
	{
		loadedTexture = default(LoadedTextureData);
		if (!TryOpenKeyedTextureCache(cacheKey, out ITextureCacheSource source))
		{
			return false;
		}
		using (source)
		{
			return source.TryReadAll(out loadedTexture);
		}
	}

	internal static bool HasValidTextureCache(string sourcePath, bool premultiplied, int width, int height)
	{
		if (TryGetPackTextureInfo(sourcePath, premultiplied, out var info))
		{
			if (info.Width == width)
			{
				return info.Height == height;
			}
			return false;
		}
		string fullPath = Path.GetFullPath(sourcePath);
		return TextureCacheStore.HasValid(fullPath, CacheVariant(fullPath, premultiplied), premultiplied, width, height);
	}

	public static bool WriteTextureCache(string sourcePath, Texture2D texture)
	{
		return TryWriteTextureCache(sourcePath, texture.GetLoadedTextureData(), texture.CpuData?.AlphaIsPremultiplied ?? false);
	}

	internal static void WriteTextureCache(string sourcePath, bool premultiplied, in LoadedTextureData texture)
	{
		TryWriteTextureCache(sourcePath, in texture, premultiplied);
	}

	public static void FlushTextureCache()
	{
		TextureCacheStore.FlushAll();
	}

	public static void WriteKeyedTextureCache(string cacheKey, Texture2D texture)
	{
		BeginWriteKeyedTextureCache(cacheKey, texture).GetAwaiter().GetResult();
	}

	public static Task BeginWriteKeyedTextureCache(string cacheKey, Texture2D texture)
	{
		return BeginWriteKeyedTextureCache(cacheKey, texture.GetLoadedTextureData());
	}

	public static Task BeginWriteKeyedTextureCache(string cacheKey, LoadedTextureData loaded)
	{
		if (AssetPack.CorruptEntries > 0 || !TextureCacheWrites || string.IsNullOrWhiteSpace(cacheKey))
		{
			return Task.CompletedTask;
		}
		_keyedCacheUsed[cacheKey] = 0;
		return TextureCacheStore.StoreKeyed(cacheKey, loaded);
	}

	private static bool TryWriteTextureCache(string sourcePath, in LoadedTextureData loadedTexture, bool premultiplied)
	{
		if (loadedTexture.Levels.Length == 0 || !TextureCacheWrites)
		{
			return false;
		}
		string fullPath = Path.GetFullPath(sourcePath);
		(long, long)? tuple = loadedTexture.SourceStamp ?? CpuMeshCache.StampSource(fullPath);
		if (tuple.HasValue)
		{
			(long, long) valueOrDefault = tuple.GetValueOrDefault();
			return TextureCacheStore.StoreLater(fullPath, CacheVariant(fullPath, premultiplied), premultiplied, in loadedTexture, valueOrDefault);
		}
		return false;
	}

	private static string CacheVariant(string fullPath, bool premultiplied)
	{
		string obj = (BorderMipMapsFor(fullPath) ? "#border" : "");
		string obj2 = (premultiplied ? "#premul" : "");
		TextureUsage textureUsage = TextureUsageHints.For(fullPath);
		return obj + obj2 + ((textureUsage != 0) ? ("#" + textureUsage) : "");
	}

	public static void BeginSweepCache(IReadOnlyCollection<string> contentRoots)
	{
		if (!TextureCacheWrites)
		{
			return;
		}
		Task.Run(delegate
		{
			try
			{
				TextureCacheStore.Sweep(contentRoots, (IReadOnlyCollection<string>)(object)_keyedCacheUsed.Keys.ToArray());
			}
			catch (Exception ex)
			{
				LogCategory log = RenderLog.Log;
				bool shouldAppend;
				LogString<Warning> message = new LogString<Warning>(28, 1, log, out shouldAppend);
				if (shouldAppend)
				{
					message.AppendLiteral("Texture cache sweep failed: ");
					message.AppendFormatted(ex.Message);
				}
				log.Warning(message, "BeginSweepCache", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Rendering\\GpuTextureService.cs", 3437);
			}
		});
	}

	private static void TransitionLayout(CommandBuffer commandBuffer, GpuTextureAllocation texture, VkImageLayout newLayout, VkAccessFlags sourceAccessMask, VkAccessFlags destinationAccessMask, VkPipelineStageFlags sourceStageMask, VkPipelineStageFlags destinationStageMask)
	{
		if (texture.CurrentLayout != newLayout || sourceAccessMask != destinationAccessMask || sourceStageMask != destinationStageMask)
		{
			VkImageMemoryBarrier reference = new VkImageMemoryBarrier
			{
				SrcAccessMask = sourceAccessMask,
				DstAccessMask = destinationAccessMask,
				OldLayout = texture.CurrentLayout,
				NewLayout = newLayout,
				SrcQueueFamilyIndex = -1,
				DstQueueFamilyIndex = -1,
				Image = texture.Image,
				SubresourceRange = new VkImageSubresourceRange
				{
					AspectMask = VkImageAspectFlags.ColorBit,
					BaseMipLevel = 0,
					LevelCount = texture.MipCount,
					BaseArrayLayer = 0,
					LayerCount = -1
				}
			};
			commandBuffer.PipelineBarrier(sourceStageMask, destinationStageMask, VkDependencyFlags.None, default(ReadOnlySpan<VkMemoryBarrier>), default(ReadOnlySpan<VkBufferMemoryBarrier>), new ReadOnlySpan<VkImageMemoryBarrier>(in reference));
			texture.CurrentLayout = newLayout;
			for (int i = 0; i < texture.MipCount; i++)
			{
				texture.SetMipLayout(i, newLayout);
			}
		}
	}

	private static void TransitionSubresourceLayout(CommandBuffer commandBuffer, GpuTextureAllocation texture, int mipLevel, VkImageLayout newLayout, VkAccessFlags sourceAccessMask, VkAccessFlags destinationAccessMask, VkPipelineStageFlags sourceStageMask, VkPipelineStageFlags destinationStageMask)
	{
		if (texture.GetMipLayout(mipLevel) != newLayout || sourceAccessMask != destinationAccessMask || sourceStageMask != destinationStageMask)
		{
			VkImageMemoryBarrier reference = new VkImageMemoryBarrier
			{
				SrcAccessMask = sourceAccessMask,
				DstAccessMask = destinationAccessMask,
				OldLayout = texture.GetMipLayout(mipLevel),
				NewLayout = newLayout,
				SrcQueueFamilyIndex = -1,
				DstQueueFamilyIndex = -1,
				Image = texture.Image,
				SubresourceRange = new VkImageSubresourceRange
				{
					AspectMask = VkImageAspectFlags.ColorBit,
					BaseMipLevel = mipLevel,
					LevelCount = 1,
					BaseArrayLayer = 0,
					LayerCount = 1
				}
			};
			commandBuffer.PipelineBarrier(sourceStageMask, destinationStageMask, VkDependencyFlags.None, default(ReadOnlySpan<VkMemoryBarrier>), default(ReadOnlySpan<VkBufferMemoryBarrier>), new ReadOnlySpan<VkImageMemoryBarrier>(in reference));
			texture.SetMipLayout(mipLevel, newLayout);
		}
	}

	private static void TransitionTouchedToShaderRead(CommandBuffer commandBuffer, GpuTextureAllocation texture, int mipLevel)
	{
		VkImageLayout layout = ((mipLevel < 0) ? texture.CurrentLayout : texture.GetMipLayout(mipLevel));
		VkAccessFlags transferSourceAccessMask = GetTransferSourceAccessMask(layout);
		VkPipelineStageFlags transferSourceStageMask = GetTransferSourceStageMask(layout);
		if (mipLevel < 0)
		{
			TransitionLayout(commandBuffer, texture, VkImageLayout.ShaderReadOnlyOptimal, transferSourceAccessMask, VkAccessFlags.ShaderReadBit, transferSourceStageMask, VkPipelineStageFlags.FragmentShaderBit);
		}
		else
		{
			TransitionSubresourceLayout(commandBuffer, texture, mipLevel, VkImageLayout.ShaderReadOnlyOptimal, transferSourceAccessMask, VkAccessFlags.ShaderReadBit, transferSourceStageMask, VkPipelineStageFlags.FragmentShaderBit);
		}
	}

	private static void ExecuteImageCommands(Action<CommandBuffer, List<BufferAllocationInfo>> record, bool batch = false, params (GpuTextureAllocation texture, int mipLevel)[] touchedSubresources)
	{
		if (_device == null)
		{
			return;
		}
		FrameProfiler.ScopeHandle scopeHandle = FrameProfiler.Scope("TexUploadLockWait");
		lock (_sync)
		{
			scopeHandle.Dispose();
			using (FrameProfiler.Scope("TexUploadLocked"))
			{
				if (batch)
				{
					PendingTextureBatch orCreatePendingTextureBatch = GetOrCreatePendingTextureBatch();
					int count = orCreatePendingTextureBatch.StagingBuffers.Count;
					record(orCreatePendingTextureBatch.CommandBuffer, orCreatePendingTextureBatch.StagingBuffers);
					(GpuTextureAllocation, int)[] array = touchedSubresources;
					for (int i = 0; i < array.Length; i++)
					{
						var (gpuTextureAllocation, item) = array[i];
						if (orCreatePendingTextureBatch.TouchedSubresources.Add((gpuTextureAllocation, item)))
						{
							gpuTextureAllocation.AddPendingUpload();
						}
					}
					for (int j = count; j < orCreatePendingTextureBatch.StagingBuffers.Count; j++)
					{
						orCreatePendingTextureBatch.StagingBytes += (long)orCreatePendingTextureBatch.StagingBuffers[j].BufferSize;
					}
					if (orCreatePendingTextureBatch.StagingBytes >= 134217728)
					{
						FlushPendingTextureCommandsLocked(wait: false);
					}
					return;
				}
				FlushPendingTextureCommandsLocked();
				CommandBuffer commandBuffer = _device.AllocateCommandBuffer(new VkCommandBufferAllocateInfo
				{
					CommandPool = _commandPool,
					Level = VkCommandBufferLevel.Primary
				});
				List<BufferAllocationInfo> list = new List<BufferAllocationInfo>();
				try
				{
					commandBuffer.Begin(VkCommandBufferUsageFlags.OneTimeSubmitBit);
					record(commandBuffer, list);
					(GpuTextureAllocation, int)[] array = touchedSubresources;
					for (int i = 0; i < array.Length; i++)
					{
						var (texture, mipLevel) = array[i];
						TransitionTouchedToShaderRead(commandBuffer, texture, mipLevel);
					}
					commandBuffer.End();
					VkFence orCreateFenceLocked = GetOrCreateFenceLocked();
					try
					{
						CommandBuffer reference = commandBuffer;
						SubmitGraphicsLocked(new Span<CommandBuffer>(ref reference), orCreateFenceLocked);
						_device.WaitForFence(orCreateFenceLocked, -1);
					}
					finally
					{
						ReturnFenceLocked(orCreateFenceLocked);
					}
				}
				finally
				{
					if (!commandBuffer.Handle.IsNull())
					{
						DeviceEx? device = _device;
						VkCommandPool commandPool = _commandPool;
						CommandBuffer reference2 = commandBuffer;
						device.FreeCommandBuffers(commandPool, new ReadOnlySpan<CommandBuffer>(in reference2));
					}
					foreach (BufferAllocationInfo item2 in list)
					{
						ReturnStagingBuffer(item2);
					}
				}
			}
		}
	}

	private static void ExecuteImageCommands(Action<CommandBuffer> record, bool batch = false, params (GpuTextureAllocation texture, int mipLevel)[] touchedSubresources)
	{
		ExecuteImageCommands(delegate(CommandBuffer commandBuffer, List<BufferAllocationInfo> _)
		{
			record(commandBuffer);
		}, batch, touchedSubresources);
	}

	private static PendingTextureBatch GetOrCreatePendingTextureBatch()
	{
		if (_pendingTextureBatch != null)
		{
			return _pendingTextureBatch;
		}
		CommandBuffer commandBuffer = _device.AllocateCommandBuffer(new VkCommandBufferAllocateInfo
		{
			CommandPool = _commandPool,
			Level = VkCommandBufferLevel.Primary
		});
		commandBuffer.Begin(VkCommandBufferUsageFlags.OneTimeSubmitBit);
		_pendingTextureBatch = new PendingTextureBatch(commandBuffer);
		return _pendingTextureBatch;
	}

	public static Texture GetOrCreateWhiteFallbackTexture()
	{
		return GetOrCreateFallbackTexture(MaterialTextureDefault.White);
	}

	public static Texture GetOrCreateFallbackTexture(MaterialTextureDefault textureDefault)
	{
		lock (_sync)
		{
			return textureDefault switch
			{
				MaterialTextureDefault.Black => GetOrCreateSolidFallbackLocked(ref _blackFallbackTexture, "Core.BlackFallback", 0f), 
				MaterialTextureDefault.Gray => GetOrCreateSolidFallbackLocked(ref _grayFallbackTexture, "Core.GrayFallback", 0.5f), 
				_ => GetOrCreateSolidFallbackLocked(ref _whiteFallbackTexture, "Core.WhiteFallback", 1f), 
			};
		}
	}

	private static Texture2D GetOrCreateSolidFallbackLocked(ref Texture2D? texture, string name, float value)
	{
		if (texture?.GpuResource != null)
		{
			return texture;
		}
		if (texture == null)
		{
			texture = Texture2D.CreateUnbacked(1, 1);
		}
		texture.Name = name;
		texture.GpuResource?.Dispose();
		texture.GpuResource = CreateSolidColorTexture2DLocked(texture.Name, 1, 1, value, value, value, value);
		return texture;
	}

	public static Texture GetOrCreateWhiteArrayFallbackTexture()
	{
		lock (_sync)
		{
			if (_whiteArrayFallbackTexture?.GpuResource != null)
			{
				return _whiteArrayFallbackTexture;
			}
			if (_whiteArrayFallbackTexture == null)
			{
				_whiteArrayFallbackTexture = Texture2D.CreateUnbacked(1, 1);
			}
			_whiteArrayFallbackTexture.Name = "Core.WhiteArrayFallback";
			_whiteArrayFallbackTexture.GpuResource?.Dispose();
			_whiteArrayFallbackTexture.GpuResource = CreateSolidColorTexture2DLocked(_whiteArrayFallbackTexture.Name, 1, 1, 1f, 1f, 1f, 1f, VkImageViewType._2DArray);
			return _whiteArrayFallbackTexture;
		}
	}

	private unsafe static GpuTextureAllocation? CreateSolidColorTexture2DLocked(string label, int width, int height, float r, float g, float b, float a, VkImageViewType viewType = VkImageViewType._2D)
	{
		if (_device == null || _allocator == null || width <= 0 || height <= 0)
		{
			return null;
		}
		DeviceEx device = _device;
		VmaAllocator allocator = _allocator;
		ImageAllocationInfo allocation = allocator.AllocateImageChecked(label, new VkImageCreateInfo
		{
			ImageType = VkImageType._2D,
			Format = VkFormat.R8G8B8A8UNorm,
			Extent = new VkExtent3D(width, height, 1),
			MipLevels = 1,
			ArrayLayers = 1,
			Samples = VkSampleCountFlags._1Bit,
			Tiling = VkImageTiling.Optimal,
			Usage = (VkImageUsageFlags.TransferSrcBit | VkImageUsageFlags.TransferDstBit | VkImageUsageFlags.SampledBit),
			SharingMode = VkSharingMode.Exclusive,
			InitialLayout = VkImageLayout.Undefined
		}, new AllocationInfo.CreateInfo
		{
			Preference = MemoryPreference.PreferGpu,
			RequiredProperties = VkMemoryPropertyFlags.DeviceLocalBit,
			PreferredProperties = VkMemoryPropertyFlags.DeviceLocalBit,
			Priority = 1f
		});
		VkImageView imageView = device.CreateImageView(new VkImageViewCreateInfo
		{
			Image = allocation.Image,
			ViewType = viewType,
			Format = VkFormat.R8G8B8A8UNorm,
			SubresourceRange = new VkImageSubresourceRange
			{
				AspectMask = VkImageAspectFlags.ColorBit,
				BaseMipLevel = 0,
				LevelCount = 1,
				BaseArrayLayer = 0,
				LayerCount = 1
			}
		}, device.VkAllocator);
		GpuTextureAllocation gpuTexture = new GpuTextureAllocation(device, allocator, allocation, imageView, new IntPtr(label.GetHashCode()), 1, VkFormat.R8G8B8A8UNorm, label);
		GpuMemoryTracker.SetDetail(in allocation, TextureDetail(width, height, VkFormat.R8G8B8A8UNorm, 1));
		ExecuteImageCommands(delegate(CommandBuffer commandBuffer)
		{
			VkClearColorValue vkClearColorValue = new VkClearColorValue
			{
				Float32 = new float4(r, g, b, a)
			};
			TransitionLayout(commandBuffer, gpuTexture, VkImageLayout.TransferDstOptimal, VkAccessFlags.None, VkAccessFlags.TransferWriteBit, VkPipelineStageFlags.TopOfPipeBit, VkPipelineStageFlags.TransferBit);
			VkImageSubresourceRange vkImageSubresourceRange = new VkImageSubresourceRange
			{
				AspectMask = VkImageAspectFlags.ColorBit,
				BaseMipLevel = 0,
				LevelCount = 1,
				BaseArrayLayer = 0,
				LayerCount = 1
			};
			commandBuffer.ClearColorImage(gpuTexture.Image, VkImageLayout.TransferDstOptimal, &vkClearColorValue, new ReadOnlySpan<VkImageSubresourceRange>(&vkImageSubresourceRange, 1));
			TransitionLayout(commandBuffer, gpuTexture, VkImageLayout.ShaderReadOnlyOptimal, VkAccessFlags.TransferWriteBit, VkAccessFlags.ShaderReadBit, VkPipelineStageFlags.TransferBit, VkPipelineStageFlags.FragmentShaderBit);
		}, false);
		return gpuTexture;
	}
}
