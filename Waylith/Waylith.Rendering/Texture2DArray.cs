using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Brutal.VulkanApi;
using Waylith.Math;

namespace Waylith.Rendering;

public class Texture2DArray : Texture
{
	private int _width;

	private int _height;

	private readonly int _arrayDepth;

	private readonly byte[]?[] _layerData;

	private readonly bool[] _dirtyLayers;

	private readonly LoadedTextureFormat _format;

	private readonly bool _randomWrite;

	public override int width
	{
		get
		{
			return _width;
		}
		set
		{
			_width = value;
		}
	}

	public override int height
	{
		get
		{
			return _height;
		}
		set
		{
			_height = value;
		}
	}

	public int depth => _arrayDepth;

	public LoadedTextureFormat Format => _format;

	public Texture2DArray(int width, int height, int depth, LoadedTextureFormat format, bool mipChain, bool randomWrite = false, string? name = null)
	{
		_width = width;
		_height = height;
		_arrayDepth = depth;
		_format = ((format == LoadedTextureFormat.Rgb24) ? LoadedTextureFormat.Rgba32 : format);
		_randomWrite = randomWrite;
		if (!string.IsNullOrEmpty(name))
		{
			base.Name = name;
		}
		base.MipCount = ((!mipChain) ? 1 : TexturePixelData.CalculateMipCount(width, height));
		_layerData = new byte[depth][];
		_dirtyLayers = new bool[depth];
		EnsureGpuAllocated();
	}

	private Texture2DArray(int width, int height, int depth, LoadedTextureFormat format, int mipCount, string name)
	{
		_width = width;
		_height = height;
		_arrayDepth = depth;
		_format = format;
		_randomWrite = false;
		base.Name = name;
		base.MipCount = System.Math.Max(1, mipCount);
		_layerData = new byte[depth][];
		_dirtyLayers = new bool[depth];
	}

	public static Texture2DArray CreateStatic(IReadOnlyList<Texture2D> layers, string name)
	{
		ArgumentNullException.ThrowIfNull(layers, "layers");
		if (layers.Count == 0)
		{
			throw new ArgumentException("A static texture array needs at least one layer.", "layers");
		}
		Texture2D texture2D = layers[0] ?? throw new ArgumentException("Texture-array layers cannot be null.", "layers");
		if (!texture2D.EnsureResident() || texture2D.GpuResource == null)
		{
			throw new InvalidOperationException("Texture '" + texture2D.Name + "' is not GPU-resident.");
		}
		int num = texture2D.width;
		int num2 = texture2D.height;
		int mipCount = texture2D.GpuResource.MipCount;
		VkFormat format = texture2D.GpuResource.Format;
		TextureSwizzle swizzle = texture2D.GpuResource.Swizzle;
		LoadedTextureFormat format2 = texture2D.CpuData?.Format ?? LoadedTextureFormat.Rgba32;
		List<GpuTextureAllocation> list = new List<GpuTextureAllocation>(layers.Count);
		foreach (Texture2D layer in layers)
		{
			if (layer == null || layer.width != num || layer.height != num2)
			{
				throw new InvalidOperationException("Static texture-array layers must have identical dimensions.");
			}
			if (!layer.EnsureResident() || layer.GpuResource == null)
			{
				throw new InvalidOperationException("Texture '" + layer.Name + "' is not GPU-resident.");
			}
			if (layer.GpuResource.Format != format || layer.GpuResource.MipCount != mipCount || layer.GpuResource.Swizzle != swizzle)
			{
				throw new InvalidOperationException($"Static texture-array layers must have identical formats, swizzles and mip counts: '{layer.Name}' is {layer.GpuResource.Format} {layer.GpuResource.Swizzle} with {layer.GpuResource.MipCount} mips, '{texture2D.Name}' {format} {swizzle} with {mipCount}.");
			}
			list.Add(layer.GpuResource);
		}
		Texture2DArray texture2DArray = new Texture2DArray(num, num2, layers.Count, format2, mipCount, name)
		{
			filterMode = texture2D.filterMode,
			wrapMode = texture2D.wrapMode,
			anisoLevel = texture2D.anisoLevel,
			mipMapBias = texture2D.mipMapBias
		};
		try
		{
			texture2DArray.GpuResource = GpuTextureService.CreateUninitializedTexture2DArray(name, num, num2, layers.Count, mipCount, format, texture2DArray.MemoryCategory, swizzle);
			if (texture2DArray.GpuResource == null)
			{
				throw new InvalidOperationException("Could not allocate static texture array '" + name + "'.");
			}
			GpuTextureService.CopyTexturesToArray(list, texture2DArray.GpuResource, num, num2, mipCount);
			return texture2DArray;
		}
		catch
		{
			texture2DArray.Dispose();
			throw;
		}
	}

	public void SetPixelData<T0>(T0[] data, int mipLevel, int element, int sourceDataStartIndex = 0) where T0 : unmanaged
	{
		if (mipLevel != 0 || (uint)element >= (uint)_arrayDepth || data == null || (uint)sourceDataStartIndex > (uint)data.Length)
		{
			return;
		}
		Span<byte> span = MemoryMarshal.AsBytes(data.AsSpan(sourceDataStartIndex));
		int layerSizeBytes = GetLayerSizeBytes();
		if (span.Length >= layerSizeBytes)
		{
			byte[] array = _layerData[element];
			if (array == null || array.Length != layerSizeBytes)
			{
				array = (_layerData[element] = new byte[layerSizeBytes]);
			}
			span.Slice(0, layerSizeBytes).CopyTo(array);
			_dirtyLayers[element] = true;
		}
	}

	public void SetPixels32(Color32[] colors, int arrayElement)
	{
		if ((uint)arrayElement >= (uint)_arrayDepth || colors == null)
		{
			return;
		}
		int layerSizeBytes = GetLayerSizeBytes();
		if (TexturePixelData.BytesPerPixel(_format) == 1)
		{
			if (colors.Length < layerSizeBytes)
			{
				return;
			}
			byte[] array = new byte[layerSizeBytes];
			for (int i = 0; i < layerSizeBytes; i++)
			{
				array[i] = colors[i].r;
			}
			_layerData[arrayElement] = array;
		}
		else
		{
			Span<byte> span = MemoryMarshal.AsBytes(colors.AsSpan());
			if (span.Length < layerSizeBytes)
			{
				return;
			}
			byte[] array2 = _layerData[arrayElement];
			if (array2 == null || array2.Length != layerSizeBytes)
			{
				array2 = (_layerData[arrayElement] = new byte[layerSizeBytes]);
			}
			span.Slice(0, layerSizeBytes).CopyTo(array2);
		}
		_dirtyLayers[arrayElement] = true;
	}

	public void Apply(bool updateMipmaps = true, bool batch = false)
	{
		EnsureGpuAllocated();
		GpuTextureAllocation gpuResource = base.GpuResource;
		if (gpuResource == null)
		{
			return;
		}
		for (int i = 0; i < _arrayDepth; i++)
		{
			if (_dirtyLayers[i] && _layerData[i] != null)
			{
				GpuTextureService.UploadRgba32ToArrayLayer(gpuResource, _width, _height, i, _layerData[i], batch);
				_dirtyLayers[i] = false;
			}
		}
	}

	private void EnsureGpuAllocated()
	{
		if (GpuTextureService.IsConfigured && base.GpuResource == null)
		{
			base.GpuResource = GpuTextureService.CreateTexture2DArray(string.IsNullOrEmpty(base.Name) ? $"Texture2DArray_{_width}x{_height}x{_arrayDepth}" : base.Name, _width, _height, _arrayDepth, base.MipCount, ToVkFormat(_format), _randomWrite ? VkImageUsageFlags.StorageBit : VkImageUsageFlags.None, base.MemoryCategory);
		}
	}

	private int GetLayerSizeBytes()
	{
		return _width * _height * TexturePixelData.BytesPerPixel(_format);
	}

	private static VkFormat ToVkFormat(LoadedTextureFormat format)
	{
		switch (format)
		{
		case LoadedTextureFormat.Alpha8:
		case LoadedTextureFormat.R8:
			return VkFormat.R8UNorm;
		case LoadedTextureFormat.RFloat:
			return VkFormat.R32SFloat;
		case LoadedTextureFormat.RgbaFloat:
			return VkFormat.R32G32B32A32SFloat;
		case LoadedTextureFormat.Rgba64:
			return VkFormat.R16G16B16A16UNorm;
		default:
			return VkFormat.R8G8B8A8UNorm;
		}
	}
}
