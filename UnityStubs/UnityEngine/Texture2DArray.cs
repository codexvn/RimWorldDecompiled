using UnityEngine.Rendering;
using UnityStubs.Internal;
using Waylith.Rendering;

namespace UnityEngine;

public class Texture2DArray : Texture
{
	public override Waylith.Rendering.Texture2DArray CoreTexture { get; }

	public GpuTextureAllocation GpuTexture => CoreTexture?.GpuResource;

	public int depth => CoreTexture.depth;

	public Texture2DArray(int width, int height, int depth, TextureFormat textureFormat, bool mipChain)
		: this(width, height, depth, textureFormat, mipChain, linear: false)
	{
	}

	public Texture2DArray(int width, int height, int depth, TextureFormat textureFormat, bool mipChain, bool linear)
	{
		CoreTexture = new Waylith.Rendering.Texture2DArray(width, height, depth, ToLoadedFormat(textureFormat), mipChain);
		base.dimension = TextureDimension.Tex2DArray;
	}

	public void SetPixelData<T0>(T0[] data, int mipLevel, int element, int sourceDataStartIndex = 0) where T0 : unmanaged
	{
		CoreTexture.SetPixelData(data, mipLevel, element, sourceDataStartIndex);
	}

	public void SetPixels32(Color32[] colors, int arrayElement)
	{
		if (colors == null)
		{
			return;
		}
		if (TexturePixelData.BytesPerPixel(CoreTexture.Format) != 1)
		{
			CoreTexture.SetPixelData(colors, 0, arrayElement);
			return;
		}
		byte[] array = new byte[colors.Length];
		for (int i = 0; i < colors.Length; i++)
		{
			array[i] = colors[i].r;
		}
		CoreTexture.SetPixelData(array, 0, arrayElement);
	}

	public void Apply()
	{
		Apply(updateMipmaps: true, makeNoLongerReadable: false);
	}

	public void Apply(bool updateMipmaps)
	{
		Apply(updateMipmaps, makeNoLongerReadable: false);
	}

	public void Apply(bool updateMipmaps, bool makeNoLongerReadable)
	{
		CoreTexture.Apply(updateMipmaps);
		if (makeNoLongerReadable)
		{
			StubWarner.LogStub("Apply", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\Texture2DArray.cs", 55);
		}
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
