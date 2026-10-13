using System;
using System.Runtime.InteropServices;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;
using Waylith.Graphics;

namespace UnityEngine;

public class SystemInfo
{
	public const string unsupportedIdentifier = "n/a";

	public static int graphicsDeviceVendorID => GpuInfo.VendorId;

	public static int graphicsMemorySize => (int)Math.Min(2147483647L, GpuInfo.DeviceLocalMemoryBytes / 1048576);

	public static string graphicsDeviceName => GpuInfo.DeviceName;

	public static GraphicsDeviceType graphicsDeviceType => GraphicsDeviceType.Vulkan;

	public static int graphicsShaderLevel => 50;

	public static bool graphicsUVStartsAtTop => true;

	public static int maxTextureSize => Math.Min(GpuInfo.MaxTextureSize, 16384);

	public static float batteryLevel => -1f;

	public static BatteryStatus batteryStatus => BatteryStatus.Unknown;

	public static string operatingSystem => RuntimeInformation.OSDescription;

	public static int systemMemorySize => (int)(GC.GetGCMemoryInfo().TotalAvailableMemoryBytes / 1048576);

	public static string deviceModel => "n/a";

	public static string deviceName => Environment.MachineName;

	public static int processorCount => Environment.ProcessorCount;

	public static int processorFrequency => 0;

	public static string graphicsDeviceVersion => "Vulkan 1.2";

	public static bool supportsAsyncGPUReadback => true;

	public static OperatingSystemFamily operatingSystemFamily
	{
		get
		{
			switch (Environment.OSVersion.Platform)
			{
			case PlatformID.Unix:
				if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
				{
					return OperatingSystemFamily.MacOSX;
				}
				return OperatingSystemFamily.Linux;
			case PlatformID.Win32NT:
				return OperatingSystemFamily.Windows;
			default:
				return OperatingSystemFamily.Other;
			}
		}
	}

	public static string processorType => RuntimeInformation.ProcessArchitecture.ToString();

	public static bool supportsComputeShaders => true;

	public static bool supportsInstancing => true;

	public static bool usesReversedZBuffer => false;

	public static bool supports2DArrayTextures => true;

	public static CopyTextureSupport copyTextureSupport => CopyTextureSupport.Basic | CopyTextureSupport.TextureToRT | CopyTextureSupport.RTToTexture;

	public static GraphicsFormat GetGraphicsFormat(DefaultFormat format)
	{
		return format switch
		{
			DefaultFormat.HDR => GraphicsFormat.R16G16B16A16_SFloat, 
			DefaultFormat.DepthStencil => GraphicsFormat.D24_UNorm_S8_UInt, 
			DefaultFormat.Shadow => GraphicsFormat.D16_UNorm, 
			_ => GraphicsFormat.R8G8B8A8_UNorm, 
		};
	}

	public static bool SupportsTextureFormat(TextureFormat format)
	{
		return format switch
		{
			TextureFormat.RGBA32 => true, 
			TextureFormat.ARGB32 => true, 
			TextureFormat.RGB24 => true, 
			TextureFormat.R8 => true, 
			TextureFormat.RFloat => true, 
			TextureFormat.RGFloat => true, 
			TextureFormat.RGBAFloat => true, 
			TextureFormat.DXT1 => true, 
			TextureFormat.DXT5 => true, 
			TextureFormat.BC4 => true, 
			TextureFormat.BC5 => true, 
			TextureFormat.BC7 => true, 
			_ => false, 
		};
	}

	public static bool SupportsRenderTextureFormat(RenderTextureFormat format)
	{
		return format switch
		{
			RenderTextureFormat.ARGB32 => true, 
			RenderTextureFormat.ARGBHalf => true, 
			RenderTextureFormat.ARGBFloat => true, 
			RenderTextureFormat.Depth => true, 
			RenderTextureFormat.Shadowmap => true, 
			RenderTextureFormat.Default => true, 
			RenderTextureFormat.DefaultHDR => true, 
			RenderTextureFormat.R8 => true, 
			RenderTextureFormat.R16 => true, 
			RenderTextureFormat.RFloat => true, 
			RenderTextureFormat.RHalf => true, 
			RenderTextureFormat.RGHalf => true, 
			RenderTextureFormat.RGFloat => true, 
			_ => false, 
		};
	}
}
