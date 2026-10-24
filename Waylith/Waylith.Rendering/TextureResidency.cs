using System;
using System.Collections.Generic;
using System.Threading;
using Brutal.Logging;
using Brutal.Logging.Internal;
using Brutal.VulkanApi;
using Brutal.VulkanApi.Abstractions;
using Brutal.VulkanApi.Vma;
using Waylith.Graphics;
using Waylith.Logging;

namespace Waylith.Rendering;

public static class TextureResidency
{
	internal const string OffloadedTrackerLabel = "OffloadedTexture";

	private const VkImageUsageFlags PinnedUsage = VkImageUsageFlags.StorageBit | VkImageUsageFlags.ColorAttachmentBit | VkImageUsageFlags.DepthStencilAttachmentBit | VkImageUsageFlags.InputAttachmentBit;

	private const long MinFreeBytes = 536870912L;

	private const long MinHysteresisBytes = 268435456L;

	private const long MaxOffloadBytesPerUpdate = 1073741824L;

	private const long MaxBytesPerMoveBatch = 268435456L;

	private const int MaxTexturesPerMoveBatch = 512;

	private const long PromoteBytesPerScan = 67108864L;

	private const int PromoteScanIntervalFrames = 10;

	private const long PromoteRecentFrames = 120L;

	private const long DedicatedBytes = 134217728L;

	private const long DedicatedMarginBytes = 67108864L;

	private const long HostEvictColdFrames = 600L;

	private const long OnDemandRecentFrames = 60L;

	private const long HostTrimRetryBytes = 268435456L;

	private const uint HostTrimRetryFrames = 300u;

	private static readonly LogCategory Logger = LogCategory.Make("TextureResidency");

	private static readonly Lock Gate = new Lock();

	private static readonly List<WeakReference<Texture>> Registry = new List<WeakReference<Texture>>();

	private static readonly List<(Texture Texture, GpuTextureAllocation Allocation)> Candidates = new List<(Texture, GpuTextureAllocation)>();

	private static readonly List<Texture> Batch = new List<Texture>();

	private static VmaAllocator? _allocator;

	private static int _heapIndex = -1;

	private static int _hostHeapIndex = -1;

	private static int _heapCount;

	private static uint _systemMemoryTypeBits;

	private static long _useOrder;

	private static uint _frameIndex;

	private static int _registrationsSinceCompact;

	private static bool _busy;

	private static long _offloadedBytes;

	private static int _offloadedCount;

	private static int _systemPlacementLogged;

	private static int _hostEvictionLogged;

	private static long _hostTrimRetryAtBytes;

	private static uint _hostTrimRetryFrame;

	public static TextureResidencyPolicy Policy { get; set; } = TextureResidencyPolicy.OffloadUnderPressure;

	public static bool OnDemand => Policy == TextureResidencyPolicy.OnDemand;

	public static bool OffloadSupported => _systemMemoryTypeBits != 0;

	public static long OffloadedBytes => Interlocked.Read(in _offloadedBytes);

	public static int OffloadedCount => Volatile.Read(in _offloadedCount);

	public static long DeviceLocalHeapBytes { get; private set; }

	internal static long NextUseOrder()
	{
		return Interlocked.Increment(ref _useOrder);
	}

	internal static void OnHostAllocationChanged(long bytes, int count)
	{
		Interlocked.Add(ref _offloadedBytes, bytes);
		Interlocked.Add(ref _offloadedCount, count);
	}

	internal static bool IsOffloadable(GpuMemoryCategory category, VkImageUsageFlags usage)
	{
		if ((category == GpuMemoryCategory.GameTexture || category == GpuMemoryCategory.Atlas) ? true : false)
		{
			return (usage & (VkImageUsageFlags.StorageBit | VkImageUsageFlags.ColorAttachmentBit | VkImageUsageFlags.DepthStencilAttachmentBit | VkImageUsageFlags.InputAttachmentBit)) == 0;
		}
		return false;
	}

	internal static void Configure(GraphicsDevice graphicsDevice)
	{
		VmaAllocator allocator = graphicsDevice.Allocator;
		VkPhysicalDeviceMemoryProperties memoryProperties = graphicsDevice.PhysicalDevice.GetMemoryProperties();
		VkImageCreateInfo vkImageCreateInfo = new VkImageCreateInfo();
		vkImageCreateInfo.ImageType = VkImageType._2D;
		vkImageCreateInfo.Format = VkFormat.R8G8B8A8UNorm;
		vkImageCreateInfo.Extent = new VkExtent3D(256, 256, 1);
		vkImageCreateInfo.MipLevels = 1;
		vkImageCreateInfo.ArrayLayers = 1;
		vkImageCreateInfo.Samples = VkSampleCountFlags._1Bit;
		vkImageCreateInfo.Tiling = VkImageTiling.Optimal;
		vkImageCreateInfo.Usage = VkImageUsageFlags.TransferSrcBit | VkImageUsageFlags.TransferDstBit | VkImageUsageFlags.SampledBit;
		vkImageCreateInfo.SharingMode = VkSharingMode.Exclusive;
		vkImageCreateInfo.InitialLayout = VkImageLayout.Undefined;
		VkImageCreateInfo pImageCreateInfo = vkImageCreateInfo;
		VmaAllocationCreateInfo vmaAllocationCreateInfo = default(VmaAllocationCreateInfo);
		vmaAllocationCreateInfo.Usage = VmaMemoryUsage.AutoPreferDevice;
		vmaAllocationCreateInfo.RequiredFlags = VkMemoryPropertyFlags.DeviceLocalBit;
		vmaAllocationCreateInfo.PreferredFlags = VkMemoryPropertyFlags.DeviceLocalBit;
		VmaAllocationCreateInfo pAllocationCreateInfo = vmaAllocationCreateInfo;
		if (allocator.FindMemoryTypeIndexForImageInfo(pImageCreateInfo, pAllocationCreateInfo, out var pMemoryTypeIndex) != 0)
		{
			Logger.Warning("No device-local memory type takes sampled textures, texture residency is off.", "Configure", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Rendering\\TextureResidency.cs", 117);
			return;
		}
		uint num = 0u;
		for (int i = 0; i < memoryProperties.MemoryTypeCount; i++)
		{
			if ((global::_003CPrivateImplementationDetails_003E.InlineArrayAsSpan<VkMemoryType_32, VkMemoryType>(ref memoryProperties.MemoryTypes, 32)[i].PropertyFlags & (VkMemoryPropertyFlags.DeviceLocalBit | VkMemoryPropertyFlags.LazilyAllocatedBit | VkMemoryPropertyFlags.ProtectedBit)) == 0)
			{
				num |= (uint)(1 << i);
			}
		}
		vmaAllocationCreateInfo = default(VmaAllocationCreateInfo);
		vmaAllocationCreateInfo.Usage = VmaMemoryUsage.AutoPreferDevice;
		vmaAllocationCreateInfo.MemoryTypeBits = num;
		VmaAllocationCreateInfo pAllocationCreateInfo2 = vmaAllocationCreateInfo;
		uint pMemoryTypeIndex2 = 0u;
		bool flag = num != 0 && allocator.FindMemoryTypeIndexForImageInfo(pImageCreateInfo, pAllocationCreateInfo2, out pMemoryTypeIndex2) == VkResult.Success;
		int heapIndex = global::_003CPrivateImplementationDetails_003E.InlineArrayAsSpan<VkMemoryType_32, VkMemoryType>(ref memoryProperties.MemoryTypes, 32)[(int)pMemoryTypeIndex].HeapIndex;
		long num2 = (long)global::_003CPrivateImplementationDetails_003E.InlineArrayAsSpan<VkMemoryHeap_16, VkMemoryHeap>(ref memoryProperties.MemoryHeaps, 16)[heapIndex].Size.Value;
		if (graphicsDevice.VramLimitBytes > 0)
		{
			num2 = System.Math.Min(num2, graphicsDevice.VramLimitBytes);
		}
		using (Gate.EnterScope())
		{
			_allocator = allocator;
			_heapIndex = heapIndex;
			_hostHeapIndex = (flag ? global::_003CPrivateImplementationDetails_003E.InlineArrayAsSpan<VkMemoryType_32, VkMemoryType>(ref memoryProperties.MemoryTypes, 32)[(int)pMemoryTypeIndex2].HeapIndex : (-1));
			_heapCount = memoryProperties.MemoryHeapCount;
			_systemMemoryTypeBits = (flag ? num : 0u);
			DeviceLocalHeapBytes = num2;
		}
		VmaAllocationExtensions.OutOfDeviceMemoryHandler = ReclaimForOutOfMemory;
		string value = GpuMemoryReport.FormatBytes(num2);
		string text = (flag ? $"memory type {pMemoryTypeIndex2} (heap {global::_003CPrivateImplementationDetails_003E.InlineArrayAsSpan<VkMemoryType_32, VkMemoryType>(ref memoryProperties.MemoryTypes, 32)[(int)pMemoryTypeIndex2].HeapIndex})" : null);
		if (OnDemand)
		{
			LogCategory logger = Logger;
			bool shouldAppend;
			LogString<Information> message = new LogString<Information>(133, 3, logger, out shouldAppend);
			if (shouldAppend)
			{
				message.AppendLiteral("Textures are uploaded to heap index ");
				message.AppendFormatted(heapIndex);
				message.AppendLiteral(" (");
				message.AppendFormatted(value);
				message.AppendLiteral(") and upload on-demand. Under pressure, least-recently used ones are evicted to the disk cache");
				message.AppendFormatted((text != null) ? (", if possible, or get moved to " + text + " if not") : "");
				message.AppendLiteral(".");
			}
			logger.Info(message, "Configure", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Rendering\\TextureResidency.cs", 152);
		}
		else
		{
			Logger.Info((text != null) ? $"Textures are uploaded to heap index {heapIndex} ({value}). Under pressure GameTextures get moved to {text}." : $"Textures are uploaded to heap index {heapIndex} ({value}) and there is no system memory heap to offload into (unified memory?), so under pressure only never-used textures are evicted.", "Configure", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Rendering\\TextureResidency.cs", 155);
		}
	}

	internal static void Register(Texture texture)
	{
		using (Gate.EnterScope())
		{
			Registry.Add(new WeakReference<Texture>(texture));
			if (++_registrationsSinceCompact >= 4096)
			{
				_registrationsSinceCompact = 0;
				Registry.RemoveAll((WeakReference<Texture> weak) => !weak.TryGetTarget(out Texture _));
			}
		}
	}

	private static bool TryReadHeap(out long usage, out long rawUsage, out long budget)
	{
		usage = 0L;
		rawUsage = 0L;
		budget = 0L;
		VmaAllocator allocator = _allocator;
		int heapIndex = _heapIndex;
		if (allocator == null || heapIndex < 0)
		{
			return false;
		}
		Span<VmaBudget> pBudgets = stackalloc VmaBudget[_heapCount];
		allocator.GetHeapBudgets(pBudgets);
		VmaBudget vmaBudget = pBudgets[heapIndex];
		budget = (long)vmaBudget.Budget;
		rawUsage = (long)vmaBudget.Statistics.BlockBytes;
		usage = System.Math.Max(0L, (long)vmaBudget.Statistics.AllocationBytes - TextureResourceManager.PendingMovedAwayDeviceBytes);
		return budget > 0;
	}

	private static bool TryReadHostHeapBudget(out long budget)
	{
		budget = 0L;
		VmaAllocator allocator = _allocator;
		int hostHeapIndex = _hostHeapIndex;
		if (allocator == null || hostHeapIndex < 0)
		{
			return false;
		}
		Span<VmaBudget> pBudgets = stackalloc VmaBudget[_heapCount];
		allocator.GetHeapBudgets(pBudgets);
		budget = (long)pBudgets[hostHeapIndex].Budget;
		return budget > 0;
	}

	private static long HighWatermark(long budget)
	{
		return System.Math.Max(budget / 2, System.Math.Min(budget / 5 * 4, budget - 536870912));
	}

	private static long LowWatermark(long budget)
	{
		return System.Math.Max(0L, HighWatermark(budget) - System.Math.Max(budget / 10, 268435456L));
	}

	private static long HostHighWatermark(long budget)
	{
		return budget / 4;
	}

	private static long HostLowWatermark(long budget)
	{
		return budget / 5;
	}

	internal static bool IsPastHighWatermark(long additionalBytes)
	{
		if (!TryReadHeap(out var usage, out var rawUsage, out var budget))
		{
			return false;
		}
		if (additionalBytes >= 134217728 && rawUsage + additionalBytes + 67108864 > budget)
		{
			return true;
		}
		return usage + additionalBytes > HighWatermark(budget);
	}

	internal static bool TryAllocateInSystemMemory(VmaAllocator allocator, string label, in VkImageCreateInfo createInfo, out ImageAllocationInfo allocation)
	{
		allocation = default(ImageAllocationInfo);
		uint systemMemoryTypeBits = _systemMemoryTypeBits;
		if (systemMemoryTypeBits == 0)
		{
			return false;
		}
		try
		{
			AllocationInfo.CreateInfo allocateCreateInfo = new AllocationInfo.CreateInfo
			{
				Preference = MemoryPreference.None
			};
			allocation = allocator.AllocateImageChecked(label, in createInfo, in allocateCreateInfo, systemMemoryTypeBits);
		}
		catch (VmaAllocationException)
		{
			return false;
		}
		if (Interlocked.Exchange(ref _systemPlacementLogged, 1) == 0)
		{
			LogCategory logger = Logger;
			bool shouldAppend;
			LogString<Information> message = new LogString<Information>(121, 3, logger, out shouldAppend);
			if (shouldAppend)
			{
				message.AppendLiteral("Device-local memory is over budget. '");
				message.AppendFormatted(label);
				message.AppendLiteral("' and later GameTextures go to system memory. This message will only be logged once.");
				message.AppendFormatted(Environment.NewLine);
				message.AppendFormatted(GpuMemoryReport.BuildCompactSummary().TrimEnd());
			}
			logger.Info(message, "TryAllocateInSystemMemory", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Rendering\\TextureResidency.cs", 248);
		}
		TrimHostResident(allowUsed: false);
		return true;
	}

	internal static void MakeRoom(long bytes)
	{
		if (!GpuTextureService.CanRunResidencyWork || !TryReadHeap(out var usage, out var rawUsage, out var budget))
		{
			return;
		}
		long num = usage + bytes - LowWatermark(budget);
		if (bytes >= 134217728)
		{
			num = System.Math.Max(num, rawUsage + bytes + 67108864 - budget);
		}
		if (num > 0)
		{
			TrimHostResident(allowUsed: false);
			if (Offload(num, OnDemand) > 0)
			{
				GpuTextureService.TryRetireDeferredDisposalsAfterIdle(0L);
			}
		}
	}

	private static bool ReclaimForOutOfMemory(long bytes, int attempt, bool deviceLocal)
	{
		if (!GpuTextureService.CanRunResidencyWork)
		{
			return false;
		}
		bool flag = GpuTextureService.TryRetireDeferredDisposalsAfterIdle(0L);
		bool shouldAppend;
		LogCategory logger;
		LogString<Warning> message;
		if (!deviceLocal && OffloadSupported)
		{
			if (_busy)
			{
				return flag;
			}
			int evicted;
			long num = EvictHostResident((attempt == 0) ? System.Math.Max(bytes * 4, 134217728L) : long.MaxValue, allowUsed: true, attempt > 0, out evicted);
			if (num > 0)
			{
				GpuTextureService.TryRetireDeferredDisposalsAfterIdle(0L);
			}
			logger = Logger;
			LogCategory logCategory = logger;
			message = new LogString<Warning>(109, 6, logger, out shouldAppend);
			if (shouldAppend)
			{
				message.AppendLiteral("System memory refused a ");
				message.AppendFormatted(GpuMemoryReport.FormatBytes(bytes));
				message.AppendLiteral(" allocation. Evicted ");
				message.AppendFormatted(evicted);
				message.AppendLiteral(" offloaded textures (");
				message.AppendFormatted(GpuMemoryReport.FormatBytes(num));
				message.AppendLiteral(") to the disk cache to retry it (attempt ");
				message.AppendFormatted(attempt + 1);
				message.AppendLiteral(").");
				message.AppendFormatted(Environment.NewLine);
				message.AppendFormatted(GpuMemoryReport.BuildCompactSummary().TrimEnd());
			}
			logCategory.Warning(message, "ReclaimForOutOfMemory", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Rendering\\TextureResidency.cs", 287);
			return num > 0 || flag;
		}
		TrimHostResident(attempt > 0);
		long num2 = Offload((attempt == 0) ? System.Math.Max(bytes * 4, 134217728L) : long.MaxValue, allowEviction: true, attempt > 0 && (OnDemand || !OffloadSupported));
		if (num2 > 0)
		{
			GpuTextureService.TryRetireDeferredDisposalsAfterIdle(0L);
		}
		logger = Logger;
		LogCategory logCategory2 = logger;
		message = new LogString<Warning>(89, 5, logger, out shouldAppend);
		if (shouldAppend)
		{
			message.AppendLiteral("Device-local memory refused a ");
			message.AppendFormatted(GpuMemoryReport.FormatBytes(bytes));
			message.AppendLiteral(" allocation, moved ");
			message.AppendFormatted(GpuMemoryReport.FormatBytes(num2));
			message.AppendLiteral(" of textures out to retry it (attempt ");
			message.AppendFormatted(attempt + 1);
			message.AppendLiteral(").");
			message.AppendFormatted(Environment.NewLine);
			message.AppendFormatted(GpuMemoryReport.BuildCompactSummary().TrimEnd());
		}
		logCategory2.Warning(message, "ReclaimForOutOfMemory", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Rendering\\TextureResidency.cs", 295);
		return num2 > 0 || flag;
	}

	public static void Update()
	{
		VmaAllocator allocator = _allocator;
		if (allocator == null)
		{
			return;
		}
		allocator.SetCurrentFrameIndex(++_frameIndex);
		if (!GpuTextureService.CanRunResidencyWork || !TryReadHeap(out var usage, out var _, out var budget))
		{
			return;
		}
		TrimHostResident(allowUsed: true);
		long num = HighWatermark(budget);
		long num2 = LowWatermark(budget);
		if (usage > num)
		{
			if (Offload(System.Math.Min(usage - num2, 1073741824L), allowEviction: true) > 0)
			{
				GpuTextureService.TryRetireDeferredDisposalsAfterIdle(GpuTextureService.RenderFrameSerial);
			}
		}
		else if (_frameIndex % 10 == 0 && Volatile.Read(in _offloadedCount) > 0)
		{
			Promote(num - usage);
		}
	}

	internal static void TrimHostResident(bool allowUsed)
	{
		if (_busy || !OffloadSupported || !GpuTextureService.CanRunResidencyWork || !TryReadHostHeapBudget(out var budget))
		{
			return;
		}
		long offloadedBytes = OffloadedBytes;
		if (offloadedBytes > HostHighWatermark(budget) && (offloadedBytes >= Volatile.Read(in _hostTrimRetryAtBytes) || _frameIndex >= _hostTrimRetryFrame))
		{
			int evicted;
			long num = EvictHostResident(offloadedBytes - HostLowWatermark(budget), allowUsed, allowRecent: false, out evicted);
			Volatile.Write(ref _hostTrimRetryAtBytes, offloadedBytes - num + 268435456);
			_hostTrimRetryFrame = _frameIndex + 300;
			if (num > 0 && Interlocked.Exchange(ref _hostEvictionLogged, 1) == 0)
			{
				VerboseLog.Log($"System memory holds {GpuMemoryReport.FormatBytes(offloadedBytes)} of offloaded textures, over its {GpuMemoryReport.FormatBytes(HostHighWatermark(budget))} watermark. {evicted} of them ({GpuMemoryReport.FormatBytes(num)}) went back to the disk cache. This message will only be logged once.");
			}
		}
	}

	private static long EvictHostResident(long targetBytes, bool allowUsed, bool allowRecent, out int evicted)
	{
		evicted = 0;
		_busy = true;
		try
		{
			CollectCandidates(toSystemMemory: false);
			Candidates.RemoveAll(((Texture Texture, GpuTextureAllocation Allocation) candidate) => IsPinned(candidate.Allocation));
			Candidates.Sort(delegate((Texture Texture, GpuTextureAllocation Allocation) a, (Texture Texture, GpuTextureAllocation Allocation) b)
			{
				GpuTextureAllocation item = a.Allocation;
				GpuTextureAllocation item2 = b.Allocation;
				int num = (item2.LastUseOrder == 0).CompareTo(item.LastUseOrder == 0);
				if (num != 0)
				{
					return num;
				}
				return (item.LastUseOrder != 0L) ? item.LastUseOrder.CompareTo(item2.LastUseOrder) : item2.AllocatedBytes.CompareTo(item.AllocatedBytes);
			});
			long num2 = (allowRecent ? long.MaxValue : (GpuTextureService.RenderFrameSerial - 600));
			long num3 = 0L;
			foreach (var (texture, gpuTextureAllocation) in Candidates)
			{
				if (num3 >= targetBytes || (gpuTextureAllocation.LastUseOrder != 0L && (!allowUsed || gpuTextureAllocation.LastUseFrame > num2)))
				{
					break;
				}
				if (texture.TryEvictForResidency())
				{
					num3 += gpuTextureAllocation.AllocatedBytes;
					evicted++;
				}
			}
			return num3;
		}
		finally
		{
			Candidates.Clear();
			_busy = false;
		}
	}

	private static long Offload(long targetBytes, bool allowEviction, bool evictUsed = false)
	{
		if (_busy)
		{
			return 0L;
		}
		_busy = true;
		try
		{
			CollectCandidates(toSystemMemory: true);
			Candidates.Sort(delegate((Texture Texture, GpuTextureAllocation Allocation) a, (Texture Texture, GpuTextureAllocation Allocation) b)
			{
				GpuTextureAllocation item = a.Allocation;
				GpuTextureAllocation item2 = b.Allocation;
				int num = (item.Category == GpuMemoryCategory.Atlas).CompareTo(item2.Category == GpuMemoryCategory.Atlas);
				if (num != 0)
				{
					return num;
				}
				int num2 = (item2.LastUseOrder == 0).CompareTo(item.LastUseOrder == 0);
				if (num2 != 0)
				{
					return num2;
				}
				return (item.LastUseOrder != 0L) ? item.LastUseOrder.CompareTo(item2.LastUseOrder) : item2.AllocatedBytes.CompareTo(item.AllocatedBytes);
			});
			long num3 = 0L;
			long num4 = 0L;
			foreach (var (texture, gpuTextureAllocation) in Candidates)
			{
				if (num3 + num4 >= targetBytes)
				{
					break;
				}
				long allocatedBytes = gpuTextureAllocation.AllocatedBytes;
				if (allowEviction && MayEvict(gpuTextureAllocation, evictUsed) && texture.TryEvictForResidency())
				{
					num3 += allocatedBytes;
				}
				else if (OffloadSupported && (!OnDemand || !texture.CanRehydrate))
				{
					Batch.Add(texture);
					num4 += allocatedBytes;
					if (Batch.Count >= 512 || num4 >= 268435456)
					{
						num3 += GpuTextureService.MoveTextures(Batch, toSystemMemory: true);
						Batch.Clear();
						num4 = 0L;
					}
				}
			}
			if (Batch.Count > 0)
			{
				num3 += GpuTextureService.MoveTextures(Batch, toSystemMemory: true);
			}
			return num3;
		}
		finally
		{
			Candidates.Clear();
			Batch.Clear();
			_busy = false;
		}
	}

	private static bool MayEvict(GpuTextureAllocation allocation, bool evictUsed)
	{
		if (allocation.LastUseOrder == 0L)
		{
			return true;
		}
		if (!OnDemand)
		{
			return evictUsed;
		}
		long renderFrameSerial = GpuTextureService.RenderFrameSerial;
		if (!evictUsed)
		{
			return allocation.LastUseFrame + 60 < renderFrameSerial;
		}
		return allocation.LastUseFrame < renderFrameSerial;
	}

	private static void Promote(long room)
	{
		if (_busy || room <= 0)
		{
			return;
		}
		_busy = true;
		try
		{
			CollectCandidates(toSystemMemory: false);
			long recentFrame = GpuTextureService.RenderFrameSerial - 120;
			Candidates.RemoveAll(((Texture Texture, GpuTextureAllocation Allocation) candidate) => !IsPinned(candidate.Allocation) && candidate.Allocation.LastUseFrame < recentFrame);
			Candidates.Sort(delegate((Texture Texture, GpuTextureAllocation Allocation) a, (Texture Texture, GpuTextureAllocation Allocation) b)
			{
				GpuTextureAllocation item = a.Allocation;
				GpuTextureAllocation item2 = b.Allocation;
				int num = IsPinned(item2).CompareTo(IsPinned(item));
				return (num == 0) ? item2.LastUseOrder.CompareTo(item.LastUseOrder) : num;
			});
			long num2 = 0L;
			foreach (var candidate in Candidates)
			{
				Texture item3 = candidate.Texture;
				long allocatedBytes = candidate.Allocation.AllocatedBytes;
				if (num2 + allocatedBytes <= room && (Batch.Count <= 0 || num2 + allocatedBytes <= 67108864))
				{
					Batch.Add(item3);
					num2 += allocatedBytes;
				}
			}
			if (Batch.Count > 0)
			{
				GpuTextureService.MoveTextures(Batch, toSystemMemory: false);
			}
		}
		finally
		{
			Candidates.Clear();
			Batch.Clear();
			_busy = false;
		}
	}

	private static bool IsPinned(GpuTextureAllocation allocation)
	{
		return !IsOffloadable(allocation.Category, allocation.Usage);
	}

	private static void CollectCandidates(bool toSystemMemory)
	{
		using (Gate.EnterScope())
		{
			int num = 0;
			for (int i = 0; i < Registry.Count; i++)
			{
				WeakReference<Texture> weakReference = Registry[i];
				if (weakReference.TryGetTarget(out var target))
				{
					Registry[num++] = weakReference;
					GpuTextureAllocation gpuResource = target.GpuResource;
					if (gpuResource != null && gpuResource.Movable && !gpuResource.IsDisposed && gpuResource.IsHostResident != toSystemMemory && (!toSystemMemory || IsOffloadable(gpuResource.Category, gpuResource.Usage)))
					{
						Candidates.Add((target, gpuResource));
					}
				}
			}
			Registry.RemoveRange(num, Registry.Count - num);
		}
	}
}
