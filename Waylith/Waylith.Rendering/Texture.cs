using System;
using System.Runtime.CompilerServices;
using System.Threading;
using Waylith.Graphics;

namespace Waylith.Rendering;

public abstract class Texture : IDisposable
{
	[CompilerGenerated]
	private string _003CName_003Ek__BackingField = string.Empty;

	[CompilerGenerated]
	private GpuMemoryCategory _003CMemoryCategory_003Ek__BackingField = GpuMemoryCategory.GameTexture;

	private GpuTextureAllocation? _gpuResource;

	internal TextureBuildBatch.Job? PendingBuild;

	private int _residencyGeneration;

	private bool _residencyRegistered;

	public static int GlobalResidencyEpoch;

	public string Name
	{
		[CompilerGenerated]
		get
		{
			return _003CName_003Ek__BackingField;
		}
		set
		{
			_003CName_003Ek__BackingField = value;
			_gpuResource?.Rename(AllocationLabel);
		}
	}

	public virtual string AllocationLabel => Name;

	public GpuMemoryCategory MemoryCategory
	{
		[CompilerGenerated]
		get
		{
			return _003CMemoryCategory_003Ek__BackingField;
		}
		set
		{
			_003CMemoryCategory_003Ek__BackingField = value;
			_gpuResource?.SetCategory(value);
		}
	}

	public FilterMode filterMode { get; set; } = FilterMode.Bilinear;

	public TextureWrapMode wrapMode { get; set; }

	public int anisoLevel { get; set; } = 1;

	public float mipMapBias { get; set; }

	public abstract int width { get; set; }

	public abstract int height { get; set; }

	public int MipCount { get; set; } = 1;

	public virtual bool IsReadable => false;

	public virtual bool CanRehydrate => IsReadable;

	public GpuTextureAllocation? GpuResource
	{
		get
		{
			if (PendingBuild != null)
			{
				TextureBuildBatch.Resolve(this);
			}
			return _gpuResource;
		}
		set
		{
			if (_gpuResource != value)
			{
				_gpuResource = value;
				Interlocked.Increment(ref _residencyGeneration);
				Interlocked.Increment(ref GlobalResidencyEpoch);
				if (value != null && value.Movable && !_residencyRegistered)
				{
					_residencyRegistered = true;
					TextureResidency.Register(this);
				}
			}
		}
	}

	public int ResidencyGeneration => Volatile.Read(in _residencyGeneration);

	internal virtual bool TryEvictForResidency()
	{
		return false;
	}

	public virtual bool EnsureResident()
	{
		return GpuResource != null;
	}

	public static void EnsureSampledReady(Texture? texture)
	{
		if (!(texture is Texture2D texture2D))
		{
			if (texture is Texture3D { GpuResource: null } texture3D)
			{
				texture3D.Apply(updateMipmaps: false);
			}
		}
		else if (texture2D.GpuResource == null && !texture2D.CanRehydrate)
		{
			texture2D.TryEnsureGpuAllocated();
		}
	}

	public virtual void Dispose()
	{
		GpuResource?.Dispose();
		GpuResource = null;
		GC.SuppressFinalize(this);
	}
}
