using System;
using System.Collections.Generic;
using System.Threading;
using Brutal.Numerics;
using Brutal.VulkanApi;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;
using UnityStubs.Gui;
using UnityStubs.Internal;
using UnityStubs.Rendering;
using Waylith.Graphics;
using Waylith.Rendering;

namespace UnityEngine;

public class RenderTexture : Texture
{
	public delegate bool ReadPixelsHandler(RenderTexture renderTexture, Rect source, out Color color);

	private static long _nextHandle = 1L;

	private static RenderTexture _active;

	private static ReadPixelsHandler _readPixelsCallback;

	private GraphicsFormat _graphicsFormat;

	private int _depth;

	private bool _created;

	private bool _enableRandomWrite;

	private bool _useMipMap;

	private int _volumeDepth = 1;

	private Waylith.Rendering.Texture2D _core;

	private static readonly List<RenderTexture> _releasedTemporaries = new List<RenderTexture>();

	public new GraphicsFormat graphicsFormat
	{
		get
		{
			return _graphicsFormat;
		}
		set
		{
			_graphicsFormat = value;
		}
	}

	public RenderTextureFormat format
	{
		get
		{
			return ToRenderTextureFormat(_graphicsFormat);
		}
		set
		{
			_graphicsFormat = ToGraphicsFormat(value);
		}
	}

	public int depth
	{
		get
		{
			return _depth;
		}
		set
		{
			_depth = value;
		}
	}

	public override Waylith.Rendering.Texture2D CoreTexture => _core;

	public GpuTextureAllocation GpuTexture => CoreTexture?.GpuResource;

	public static RenderTexture active
	{
		get
		{
			return _active;
		}
		set
		{
			if ((object)_active != value)
			{
				GpuTextureService.FlushPendingTextureCommands();
				_active = value;
				GuiBackend.Current?.ActiveRenderTextureChanged(value);
			}
		}
	}

	public RenderBuffer colorBuffer => new RenderBuffer(this);

	public RenderBuffer depthBuffer => new RenderBuffer(this);

	public RenderTextureDescriptor descriptor
	{
		get
		{
			RenderTextureDescriptor result = new RenderTextureDescriptor(base.width, base.height, _graphicsFormat, _depth, base.mipmapCount);
			result.enableRandomWrite = _enableRandomWrite;
			result.useMipMap = _useMipMap;
			result.autoGenerateMips = autoGenerateMips;
			result.volumeDepth = _volumeDepth;
			result.dimension = base.dimension;
			result.msaaSamples = antiAliasing;
			return result;
		}
	}

	public bool sRGB
	{
		get
		{
			GraphicsFormat graphicsFormat = _graphicsFormat;
			if (graphicsFormat == GraphicsFormat.R8G8B8A8_SRGB || graphicsFormat == GraphicsFormat.B8G8R8A8_SRGB)
			{
				return true;
			}
			return false;
		}
	}

	public bool useMipMap
	{
		get
		{
			return _useMipMap;
		}
		set
		{
			_useMipMap = value;
		}
	}

	public bool autoGenerateMips { get; set; } = true;

	public int volumeDepth
	{
		get
		{
			return _volumeDepth;
		}
		set
		{
			_volumeDepth = Math.Max(1, value);
		}
	}

	public int antiAliasing { get; set; } = 1;

	public bool enableRandomWrite
	{
		get
		{
			return _enableRandomWrite;
		}
		set
		{
			if (_enableRandomWrite != value)
			{
				_enableRandomWrite = value;
				ReleaseGpuTextureOnly();
			}
		}
	}

	public nint GetNativeDepthBufferPtr()
	{
		StubWarner.LogStub("GetNativeDepthBufferPtr", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\RenderTexture.cs", 27);
		return IntPtr.Zero;
	}

	public static RenderTexture GetTemporary(int width, int height, int depthBuffer, RenderTextureFormat format, RenderTextureReadWrite readWrite, int antiAliasing)
	{
		return GetTemporary(width, height, depthBuffer, format, readWrite);
	}

	public RenderTexture(int width, int height, GraphicsFormat colorFormat, GraphicsFormat depthStencilFormat, int mipCount)
	{
		Initialize(width, height, 0, colorFormat, mipCount);
	}

	public RenderTexture(int width, int height, int depth, GraphicsFormat format, int mipCount)
	{
		Initialize(width, height, depth, format, mipCount);
	}

	public RenderTexture(int width, int height, int depth, RenderTextureFormat format, RenderTextureReadWrite readWrite)
	{
		Initialize(width, height, depth, ToGraphicsFormat(format), 1);
	}

	public RenderTexture(int width, int height, int depth, RenderTextureFormat format, int mipCount)
	{
		Initialize(width, height, depth, ToGraphicsFormat(format), mipCount);
	}

	public RenderTexture(RenderTexture textureToCopy)
	{
		Initialize(textureToCopy.width, textureToCopy.height, textureToCopy._depth, textureToCopy._graphicsFormat, textureToCopy.mipmapCount);
		base.filterMode = textureToCopy.filterMode;
		base.wrapMode = textureToCopy.wrapMode;
		useMipMap = textureToCopy.useMipMap;
		volumeDepth = textureToCopy.volumeDepth;
		_enableRandomWrite = textureToCopy._enableRandomWrite;
		base.name = textureToCopy.name;
	}

	public RenderTexture(RenderTextureDescriptor desc)
	{
		Initialize(desc.width, desc.height, desc.depthBufferBits, (desc.graphicsFormat == GraphicsFormat.None) ? GraphicsFormat.R8G8B8A8_UNorm : desc.graphicsFormat, (desc.mipCount >= 0) ? desc.mipCount : ((!desc.useMipMap) ? 1 : TexturePixelData.CalculateMipCount(desc.width, desc.height)));
		_useMipMap = desc.useMipMap;
		autoGenerateMips = desc.autoGenerateMips;
		_enableRandomWrite = desc.enableRandomWrite;
		volumeDepth = desc.volumeDepth;
		base.dimension = desc.dimension;
		antiAliasing = Math.Max(1, desc.msaaSamples);
	}

	public RenderTexture(int width, int height, int depth)
	{
		Initialize(width, height, depth, GraphicsFormat.R8G8B8A8_UNorm, 1);
	}

	public RenderTexture(int width, int height, int depth, RenderTextureFormat format)
	{
		Initialize(width, height, depth, ToGraphicsFormat(format), 1);
	}

	public void GenerateMips()
	{
	}

	public static void SetReadPixelsHandler(ReadPixelsHandler handler)
	{
		_readPixelsCallback = handler;
	}

	internal static bool TryReadPixels(RenderTexture renderTexture, Rect source, out Color color)
	{
		if (_readPixelsCallback != null && _readPixelsCallback(renderTexture, source, out color))
		{
			return true;
		}
		color = default(Color);
		return false;
	}

	public bool Create()
	{
		if (_created)
		{
			return true;
		}
		if (CoreTexture.GpuResource == null)
		{
			string label = (string.IsNullOrWhiteSpace(base.name) ? $"RenderTexture_{Interlocked.Increment(ref _nextHandle)}" : base.name);
			if (base.dimension == TextureDimension.Tex2DArray)
			{
				CoreTexture.GpuResource = GpuTextureService.CreateTexture2DArray(label, base.width, base.height, _volumeDepth, Math.Max(1, base.mipmapCount), ResolveGpuVkFormat(), ResolveAdditionalUsage(), GpuMemoryCategory.RenderTarget);
			}
			else
			{
				CoreTexture.GpuResource = GpuTextureService.CreateEmptyTexture2D(label, base.width, base.height, Math.Max(1, base.mipmapCount), ResolveGpuVkFormat(), ResolveAdditionalUsage(), GpuMemoryCategory.RenderTarget);
			}
		}
		_created = true;
		return true;
	}

	public void DiscardContents()
	{
		StubWarner.LogStub("DiscardContents", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\RenderTexture.cs", 181);
	}

	public void DiscardContents(bool discardColor, bool discardDepth)
	{
		StubWarner.LogStub("DiscardContents", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\RenderTexture.cs", 182);
	}

	public static RenderTexture GetTemporary(RenderTextureDescriptor desc)
	{
		return new RenderTexture(desc);
	}

	public static RenderTexture GetTemporary(int width, int height, int depthBuffer, RenderTextureFormat format, RenderTextureReadWrite readWrite)
	{
		return new RenderTexture(width, height, depthBuffer, format, readWrite);
	}

	public static RenderTexture GetTemporary(int width, int height, int depthBuffer, RenderTextureFormat format)
	{
		return new RenderTexture(width, height, depthBuffer, format);
	}

	public static RenderTexture GetTemporary(int width, int height)
	{
		return new RenderTexture(width, height, 0);
	}

	public static RenderTexture GetTemporary(int width, int height, int depthBuffer)
	{
		return new RenderTexture(width, height, depthBuffer);
	}

	public bool IsCreated()
	{
		return _created;
	}

	public void Release()
	{
		if ((object)_active == this)
		{
			active = null;
		}
		GpuTextureService.FlushPendingTextureCommands();
		_created = false;
		if (CoreTexture.GpuResource != null)
		{
			CameraTextureRenderer.Current?.ReleaseTarget(this);
		}
		CoreTexture.GpuResource?.Dispose();
		CoreTexture.GpuResource = null;
	}

	public static void ReleaseTemporary(RenderTexture temp)
	{
		if (temp != null)
		{
			_releasedTemporaries.Add(temp);
		}
	}

	internal static void FreeReleasedTemporaries()
	{
		foreach (RenderTexture releasedTemporary in _releasedTemporaries)
		{
			releasedTemporary.Release();
		}
		_releasedTemporaries.Clear();
	}

	protected override void Dispose(bool disposing)
	{
		Release();
		base.Dispose(disposing);
	}

	private void Initialize(int renderWidth, int renderHeight, int depth, GraphicsFormat graphicsFormat, int mipCount)
	{
		_core = Waylith.Rendering.Texture2D.CreateUnbacked(Math.Max(1, renderWidth), Math.Max(1, renderHeight));
		_core.Name = base.name;
		_core.MipCount = Math.Max(1, mipCount);
		_depth = depth;
		_graphicsFormat = graphicsFormat;
	}

	internal void ClearPixels(Color color)
	{
		if (!_created)
		{
			Create();
		}
		if (TryEnsureGpuReady())
		{
			GpuTextureService.Clear(CoreTexture.GpuResource, new float4(color.r, color.g, color.b, color.a));
		}
	}

	internal bool TryEnsureGpuReady(bool batch = false)
	{
		if (!_created && !Create())
		{
			return false;
		}
		if (CoreTexture.GpuResource == null)
		{
			string label = (string.IsNullOrWhiteSpace(base.name) ? $"RenderTexture_{GetHashCode()}" : base.name);
			if (base.dimension == TextureDimension.Tex2DArray)
			{
				CoreTexture.GpuResource = GpuTextureService.CreateTexture2DArray(label, base.width, base.height, _volumeDepth, Math.Max(1, base.mipmapCount), ResolveGpuVkFormat(), ResolveAdditionalUsage(), GpuMemoryCategory.RenderTarget);
			}
			else
			{
				CoreTexture.GpuResource = GpuTextureService.CreateEmptyTexture2D(label, base.width, base.height, Math.Max(1, base.mipmapCount), ResolveGpuVkFormat(), ResolveAdditionalUsage(), GpuMemoryCategory.RenderTarget);
			}
		}
		return CoreTexture.GpuResource != null;
	}

	internal static GraphicsFormat ToGraphicsFormat(RenderTextureFormat format)
	{
		return format switch
		{
			RenderTextureFormat.ARGB32 => GraphicsFormat.R8G8B8A8_UNorm, 
			RenderTextureFormat.ARGBHalf => GraphicsFormat.R16G16B16A16_SFloat, 
			RenderTextureFormat.ARGBFloat => GraphicsFormat.R32G32B32A32_SFloat, 
			RenderTextureFormat.Shadowmap => GraphicsFormat.D24_UNorm_S8_UInt, 
			_ => GraphicsFormat.R8G8B8A8_UNorm, 
		};
	}

	internal static RenderTextureFormat ToRenderTextureFormat(GraphicsFormat format)
	{
		return format switch
		{
			GraphicsFormat.None => RenderTextureFormat.Depth, 
			GraphicsFormat.R16G16B16A16_SFloat => RenderTextureFormat.ARGBHalf, 
			GraphicsFormat.R32G32B32A32_SFloat => RenderTextureFormat.ARGBFloat, 
			GraphicsFormat.D24_UNorm_S8_UInt => RenderTextureFormat.Shadowmap, 
			_ => RenderTextureFormat.ARGB32, 
		};
	}

	private static VkFormat ToVkFormat(GraphicsFormat format)
	{
		return format switch
		{
			GraphicsFormat.R16G16B16A16_SFloat => VkFormat.R16G16B16A16SFloat, 
			GraphicsFormat.R32G32B32A32_SFloat => VkFormat.R32G32B32A32SFloat, 
			_ => VkFormat.R8G8B8A8UNorm, 
		};
	}

	private VkFormat ResolveGpuVkFormat()
	{
		VkFormat vkFormat = ToVkFormat(_graphicsFormat);
		if (_enableRandomWrite && (vkFormat == VkFormat.R8G8B8A8UNorm || vkFormat == VkFormat.R8G8B8A8SRGB))
		{
			return VkFormat.R32G32B32A32SFloat;
		}
		return vkFormat;
	}

	private VkImageUsageFlags ResolveAdditionalUsage()
	{
		VkImageUsageFlags vkImageUsageFlags = VkImageUsageFlags.ColorAttachmentBit;
		if (_enableRandomWrite)
		{
			vkImageUsageFlags |= VkImageUsageFlags.StorageBit;
		}
		return vkImageUsageFlags;
	}

	private void ReleaseGpuTextureOnly()
	{
		CoreTexture.GpuResource?.Dispose();
		CoreTexture.GpuResource = null;
	}
}
