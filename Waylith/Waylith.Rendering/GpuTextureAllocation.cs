using System;
using System.Threading;
using Brutal.VulkanApi;
using Brutal.VulkanApi.Abstractions;
using Brutal.VulkanApi.Vma;
using Waylith.Graphics;

namespace Waylith.Rendering;

public sealed class GpuTextureAllocation : IDisposable
{
	private readonly DeviceEx _device;

	private readonly VmaAllocator _allocator;

	private ImageAllocationInfo _allocation;

	private VkImageView _imageView;

	private readonly VkFormat _format;

	private readonly VkImageLayout[] _mipLayouts;

	private bool _disposed;

	private int _pendingUploads;

	public nint NativeHandle { get; }

	public GpuMemoryCategory Category { get; private set; }

	public bool IsHostResident { get; }

	internal VkImageType ImageType { get; private set; }

	internal VkImageUsageFlags Usage { get; private set; }

	internal VkImageViewType ViewType { get; private set; }

	internal bool Movable { get; private set; }

	internal ref readonly ImageAllocationInfo AllocationInfo => ref _allocation;

	internal long LastUseFrame { get; private set; } = -1L;

	internal long LastUseOrder { get; private set; }

	internal long LastWriteFrame { get; private set; } = -1L;

	private string? TrackerLabel
	{
		get
		{
			if (!IsHostResident)
			{
				return null;
			}
			return "OffloadedTexture";
		}
	}

	public string DebugLabel { get; private set; }

	public VkImage Image => _allocation.Image;

	internal bool HasPendingUpload => Volatile.Read(in _pendingUploads) > 0;

	public VkImageView ImageView => _imageView;

	public VkFormat Format => _format;

	public int MipCount { get; }

	public TextureSwizzle Swizzle { get; private set; }

	internal long AllocatedBytes => (long)_allocation.MemAllocationInfo.AllocSize;

	public VkImageLayout CurrentLayout { get; set; }

	public bool IsDisposed => _disposed;

	public GpuTextureAllocation(DeviceEx device, VmaAllocator allocator, ImageAllocationInfo allocation, VkImageView imageView, nint handle, int mipCount, VkFormat format = VkFormat.Undefined, string? debugLabel = null, GpuMemoryCategory category = GpuMemoryCategory.GameTexture)
	{
		Category = category;
		_device = device;
		_allocator = allocator;
		_allocation = allocation;
		_imageView = imageView;
		NativeHandle = handle;
		_format = format;
		MipCount = System.Math.Max(1, mipCount);
		_mipLayouts = new VkImageLayout[MipCount];
		for (int i = 0; i < _mipLayouts.Length; i++)
		{
			_mipLayouts[i] = VkImageLayout.Undefined;
		}
		string name = (DebugLabel = (string.IsNullOrWhiteSpace(debugLabel) ? $"GpuTexture_{format}_0x{((IntPtr)handle).ToInt64():x}" : debugLabel));
		_allocation.Image.IsNull();
		imageView.IsNull();
		if (!_allocation.MemAllocationInfo.Memory.IsNull())
		{
			allocator.GetMemoryTypeProperties((uint)_allocation.MemAllocationInfo.MemoryTypeIndex, out var pFlags);
			IsHostResident = (pFlags & VkMemoryPropertyFlags.DeviceLocalBit) == 0;
		}
		GpuMemoryTracker.Add(category, in allocation, TrackerLabel, name);
		if (IsHostResident)
		{
			TextureResidency.OnHostAllocationChanged(AllocatedBytes, 1);
		}
		TextureResourceManager.Register(this);
	}

	internal void AddPendingUpload()
	{
		Interlocked.Increment(ref _pendingUploads);
	}

	internal void ReleasePendingUpload()
	{
		Interlocked.Decrement(ref _pendingUploads);
	}

	public VkImageLayout GetMipLayout(int mipLevel)
	{
		if ((uint)mipLevel >= (uint)_mipLayouts.Length)
		{
			return CurrentLayout;
		}
		return _mipLayouts[mipLevel];
	}

	public void SetMipLayout(int mipLevel, VkImageLayout layout)
	{
		if ((uint)mipLevel < (uint)_mipLayouts.Length)
		{
			_mipLayouts[mipLevel] = layout;
		}
		CurrentLayout = layout;
	}

	public void MarkUsed()
	{
		LastUseFrame = GpuTextureService.RenderFrameSerial;
		LastUseOrder = TextureResidency.NextUseOrder();
	}

	public void MarkWritten()
	{
		MarkUsed();
		LastWriteFrame = LastUseFrame;
	}

	public void PinInPlace()
	{
		Movable = false;
	}

	internal void SetImageInfo(VkImageType imageType, VkImageUsageFlags usage, VkImageViewType viewType, TextureSwizzle swizzle = default(TextureSwizzle))
	{
		ImageType = imageType;
		Usage = usage;
		ViewType = viewType;
		Swizzle = swizzle;
		Movable = true;
	}

	internal void InheritUse(GpuTextureAllocation from)
	{
		LastUseFrame = from.LastUseFrame;
		LastUseOrder = from.LastUseOrder;
	}

	internal void SetCategory(GpuMemoryCategory category)
	{
		if (Category != category)
		{
			Category = category;
			if (!_allocation.MemAllocationInfo.Memory.IsNull())
			{
				GpuMemoryTracker.SetCategory(in _allocation, category, TrackerLabel);
			}
		}
	}

	public void Rename(string label)
	{
		if (!string.IsNullOrWhiteSpace(label))
		{
			DebugLabel = label;
			_allocation.Image.IsNull();
			_imageView.IsNull();
			GpuMemoryTracker.SetName(in _allocation, label);
		}
	}

	public void Dispose()
	{
		if (!_disposed)
		{
			_disposed = true;
			if (HasPendingUpload)
			{
				GpuTextureService.FlushPendingUploadsBeforeDisposal();
			}
			TextureResourceManager.DeferDispose(this);
			GC.SuppressFinalize(this);
		}
	}

	internal void DisposeMovedAway(long minForcedRetireFrame)
	{
		if (!_disposed)
		{
			_disposed = true;
			TextureResourceManager.DeferDispose(this, movedAway: true, minForcedRetireFrame);
			GC.SuppressFinalize(this);
		}
	}

	public void DisposeNow()
	{
		_disposed = true;
		GC.SuppressFinalize(this);
		if (TryDetachForDisposal(out var disposal))
		{
			disposal.DisposeNow();
		}
	}

	public bool TryDetachForDisposal(out GpuTextureDisposal disposal)
	{
		if (_imageView.IsNull() && _allocation.MemAllocationInfo.Memory.IsNull())
		{
			disposal = default(GpuTextureDisposal);
			return false;
		}
		disposal = new GpuTextureDisposal(_device, _allocator, _allocation, _imageView, Category);
		if (IsHostResident && !_allocation.MemAllocationInfo.Memory.IsNull())
		{
			TextureResidency.OnHostAllocationChanged(-AllocatedBytes, -1);
		}
		_imageView = default(VkImageView);
		_allocation = default(ImageAllocationInfo);
		return true;
	}

	~GpuTextureAllocation()
	{
		try
		{
			Dispose();
		}
		catch
		{
		}
	}
}
