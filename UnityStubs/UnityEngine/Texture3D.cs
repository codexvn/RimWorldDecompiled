using System;
using System.Runtime.InteropServices;
using UnityEngine.Rendering;
using UnityStubs.Internal;
using Waylith.Math;
using Waylith.Rendering;

namespace UnityEngine;

public class Texture3D : Texture
{
	public override Waylith.Rendering.Texture3D CoreTexture { get; }

	public GpuTextureAllocation GpuTexture => CoreTexture?.GpuResource;

	public int depth => CoreTexture.depth;

	public Texture3D(int width, int height, int depth, TextureFormat format, bool mipChain)
		: this(width, height, depth, format, mipChain, linear: false)
	{
	}

	public Texture3D(int width, int height, int depth, TextureFormat format, int mipCount)
		: this(width, height, depth, format, linear: false, Math.Max(1, mipCount))
	{
	}

	public Texture3D(int width, int height, int depth, TextureFormat format, bool mipChain, bool linear)
		: this(width, height, depth, format, linear, (!mipChain) ? 1 : TexturePixelData.CalculateMipCount(width, height, depth))
	{
	}

	private Texture3D(int width, int height, int depth, TextureFormat format, bool linear, int mipCount)
		: this(new Waylith.Rendering.Texture3D(width, height, depth, ToLoadedFormat(format), linear, mipCount))
	{
	}

	internal Texture3D(Waylith.Rendering.Texture3D core)
	{
		CoreTexture = core;
		base.dimension = TextureDimension.Tex3D;
		if (!string.IsNullOrEmpty(core.Name))
		{
			base.name = core.Name;
		}
	}

	public void Apply()
	{
		Apply(updateMipmaps: true, makeNoLongerReadable: false);
	}

	public void Apply(bool updateMipmaps, bool makeNoLongerReadable)
	{
		CoreTexture.Apply(updateMipmaps, makeNoLongerReadable);
	}

	public void SetPixels(Color[] colors)
	{
		ArgumentNullException.ThrowIfNull(colors, "colors");
		Waylith.Math.Color[] array = new Waylith.Math.Color[colors.Length];
		for (int i = 0; i < colors.Length; i++)
		{
			array[i] = Color.ToCore(colors[i]);
		}
		RequireCpuData().SetPixels(array);
	}

	public void SetPixels32(Color32[] colors)
	{
		ArgumentNullException.ThrowIfNull(colors, "colors");
		TexturePixelData texturePixelData = RequireCpuData();
		texturePixelData.EnsureWritableMipZero();
		MemoryMarshal.AsBytes(colors.AsSpan()).CopyTo(texturePixelData.Levels[0]);
	}

	public void SetPixelData<T>(T[] data, int mipLevel, int sourceDataStartIndex = 0) where T : struct
	{
		ArgumentNullException.ThrowIfNull(data, "data");
		if (mipLevel != 0)
		{
			StubWarner.LogStub("SetPixelData", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\Texture3D.cs", 71);
			return;
		}
		TexturePixelData texturePixelData = RequireCpuData();
		texturePixelData.EnsureWritableMipZero();
		Span<byte> span = MemoryMarshal.AsBytes(data.AsSpan(sourceDataStartIndex));
		byte[] array = texturePixelData.Levels[0];
		if (span.Length != array.Length)
		{
			throw new ArgumentException($"SetPixelData: {span.Length} bytes do not fit volume '{base.name}' ({array.Length} bytes).");
		}
		span.CopyTo(array);
	}

	private TexturePixelData RequireCpuData()
	{
		return CoreTexture?.CpuData ?? throw new InvalidOperationException("Texture '" + base.name + "' is not readable.");
	}

	private static LoadedTextureFormat ToLoadedFormat(TextureFormat format)
	{
		return format switch
		{
			TextureFormat.Alpha8 => LoadedTextureFormat.Alpha8, 
			TextureFormat.R8 => LoadedTextureFormat.R8, 
			TextureFormat.RFloat => LoadedTextureFormat.RFloat, 
			TextureFormat.RGBAFloat => LoadedTextureFormat.RgbaFloat, 
			_ => LoadedTextureFormat.Rgba32, 
		};
	}
}
