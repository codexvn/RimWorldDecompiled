using System;
using Brutal.VulkanApi;

namespace Waylith.Rendering;

public readonly record struct TextureSwizzle(TextureChannel R, TextureChannel G, TextureChannel B, TextureChannel A)
{
	public bool IsIdentity => this == default(TextureSwizzle);

	public VkComponentMapping ToVk()
	{
		VkComponentMapping result = new VkComponentMapping();
		result.R = (VkComponentSwizzle)R;
		result.G = (VkComponentSwizzle)G;
		result.B = (VkComponentSwizzle)B;
		result.A = (VkComponentSwizzle)A;
		return result;
	}

	public int Pack()
	{
		return (int)((uint)R | ((uint)G << 8) | ((uint)B << 16) | ((uint)A << 24));
	}

	public static TextureSwizzle Unpack(int packed)
	{
		return new TextureSwizzle((TextureChannel)(packed & 0xFF), (TextureChannel)((packed >> 8) & 0xFF), (TextureChannel)((packed >> 16) & 0xFF), (TextureChannel)((packed >> 24) & 0xFF));
	}

	public void ApplyInPlace(Span<byte> rgba)
	{
		if (!IsIdentity)
		{
			for (int i = 0; i + 3 < rgba.Length; i += 4)
			{
				byte b = rgba[i];
				byte b2 = rgba[i + 1];
				byte b3 = rgba[i + 2];
				byte b4 = rgba[i + 3];
				rgba[i] = Select(R, b, b, b2, b3, b4);
				rgba[i + 1] = Select(G, b2, b, b2, b3, b4);
				rgba[i + 2] = Select(B, b3, b, b2, b3, b4);
				rgba[i + 3] = Select(A, b4, b, b2, b3, b4);
			}
		}
	}

	private static byte Select(TextureChannel channel, byte own, byte r, byte g, byte b, byte a)
	{
		return channel switch
		{
			TextureChannel.Zero => 0, 
			TextureChannel.One => byte.MaxValue, 
			TextureChannel.Red => r, 
			TextureChannel.Green => g, 
			TextureChannel.Blue => b, 
			TextureChannel.Alpha => a, 
			_ => own, 
		};
	}
}
