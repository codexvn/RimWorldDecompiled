using System;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;
using Waylith.Graphics;
using Waylith.Rendering;

namespace UnityEngine;

public class Texture : Object, IDisposable
{
	private bool _disposed;

	public const int GenerateAllMips = -1;

	internal bool ImportedUnreadable;

	public virtual Waylith.Rendering.Texture CoreTexture => null;

	public int anisoLevel
	{
		get
		{
			return CoreTexture?.anisoLevel ?? 0;
		}
		set
		{
			if (CoreTexture != null)
			{
				CoreTexture.anisoLevel = value;
			}
		}
	}

	public static ulong currentTextureMemory => TexturePixelData.CurrentCpuBytes;

	public static ulong nonStreamingTextureCount => (ulong)SumTextureCategories().count;

	public static ulong nonStreamingTextureMemory => (ulong)SumTextureCategories().bytes;

	public TextureDimension dimension { get; set; } = TextureDimension.Tex2D;

	public virtual GraphicsFormat graphicsFormat => GraphicsFormat.R8G8B8A8_UNorm;

	public FilterMode filterMode
	{
		get
		{
			return (FilterMode)(CoreTexture?.filterMode ?? Waylith.Rendering.FilterMode.Bilinear);
		}
		set
		{
			if (CoreTexture != null)
			{
				CoreTexture.filterMode = (Waylith.Rendering.FilterMode)value;
			}
		}
	}

	public int height
	{
		get
		{
			return CoreTexture?.height ?? 0;
		}
		set
		{
			if (CoreTexture != null)
			{
				CoreTexture.height = value;
			}
		}
	}

	public bool isReadable
	{
		get
		{
			if (!ImportedUnreadable)
			{
				return CoreTexture?.CanRehydrate ?? false;
			}
			return false;
		}
	}

	public float mipMapBias
	{
		get
		{
			return CoreTexture?.mipMapBias ?? 0f;
		}
		set
		{
			if (CoreTexture != null)
			{
				CoreTexture.mipMapBias = value;
			}
		}
	}

	public int mipmapCount => CoreTexture?.MipCount ?? 1;

	public int width
	{
		get
		{
			return CoreTexture?.width ?? 0;
		}
		set
		{
			if (CoreTexture != null)
			{
				CoreTexture.width = value;
			}
		}
	}

	public TextureWrapMode wrapMode
	{
		get
		{
			return (TextureWrapMode)(CoreTexture?.wrapMode ?? Waylith.Rendering.TextureWrapMode.Repeat);
		}
		set
		{
			if (CoreTexture != null)
			{
				CoreTexture.wrapMode = (Waylith.Rendering.TextureWrapMode)value;
			}
		}
	}

	public TextureWrapMode wrapModeU
	{
		get
		{
			return wrapMode;
		}
		set
		{
			wrapMode = value;
		}
	}

	public TextureWrapMode wrapModeV
	{
		get
		{
			return wrapMode;
		}
		set
		{
			wrapMode = value;
		}
	}

	public TextureWrapMode wrapModeW
	{
		get
		{
			return wrapMode;
		}
		set
		{
			wrapMode = value;
		}
	}

	protected nint nativeTexturePtr => CoreTexture?.GpuResource?.NativeHandle ?? IntPtr.Zero;

	private static (long bytes, long count) SumTextureCategories()
	{
		long num = 0L;
		long num2 = 0L;
		GpuMemoryTracker.CategoryStat[] array = GpuMemoryTracker.Snapshot();
		for (int i = 0; i < array.Length; i++)
		{
			GpuMemoryTracker.CategoryStat categoryStat = array[i];
			GpuMemoryCategory category = categoryStat.Category;
			if ((uint)(category - 2) <= 3u)
			{
				num += categoryStat.Bytes;
				num2 += categoryStat.Count;
			}
		}
		return (bytes: num, count: num2);
	}

	public nint GetNativeTexturePtr()
	{
		return nativeTexturePtr;
	}

	internal static void EnsureSampledTextureReady(Texture texture)
	{
		if ((object)texture != null)
		{
			if (texture is RenderTexture renderTexture)
			{
				renderTexture.TryEnsureGpuReady();
			}
			else
			{
				Waylith.Rendering.Texture.EnsureSampledReady(texture.CoreTexture);
			}
		}
	}

	protected virtual void Dispose(bool disposing)
	{
		if (!_disposed)
		{
			_disposed = true;
			if (disposing)
			{
				CoreTexture?.Dispose();
			}
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	~Texture()
	{
		Dispose(disposing: false);
	}

	public override bool Equals(object obj)
	{
		if (obj is Texture texture)
		{
			return this == texture;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	protected override void OnNameChanged(string newName)
	{
		if (CoreTexture != null)
		{
			string text = (string.IsNullOrWhiteSpace(newName) ? $"{GetType().Name}_{GetHashCode()}" : newName);
			CoreTexture.Name = text;
		}
	}

	public static bool operator ==(Texture left, Texture right)
	{
		if ((object)left == right)
		{
			return true;
		}
		bool flag = left?._disposed ?? true;
		bool flag2 = right?._disposed ?? true;
		if (flag || flag2)
		{
			return flag == flag2;
		}
		return false;
	}

	public static bool operator !=(Texture left, Texture right)
	{
		return !(left == right);
	}
}
