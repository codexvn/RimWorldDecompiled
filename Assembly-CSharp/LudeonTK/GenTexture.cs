using UnityEngine;
using Waylith.Rendering;

namespace LudeonTK;

public static class GenTexture
{
	public static void GenerateMipmaps(UnityEngine.Texture2D baseTexture, int minMipDimension, bool preserveAlphaCoverage = false, float alphaCoverageCutoff = 0.5f)
	{
		if (baseTexture?.GpuTexture != null)
		{
			Mipmapper.GenerateMipmaps(baseTexture.GpuTexture, baseTexture.width, baseTexture.height, baseTexture.mipmapCount, minMipDimension, preserveAlphaCoverage, alphaCoverageCutoff);
		}
	}
}
