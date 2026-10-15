using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse;

public static class MaterialDefUtility
{
	private static Dictionary<string, MaterialDef> materialDefsByResourcePath;

	private static int materialDefCount = -1;

	private static Dictionary<string, Texture2D> alphaFromGrayscaleTextures = new Dictionary<string, Texture2D>();

	public static bool TryGetByResourcePath(string resourcePath, out MaterialDef def)
	{
		EnsureCache();
		return materialDefsByResourcePath.TryGetValue(NormalizeResourcePath(resourcePath), out def);
	}

	public static Texture2D AlphaFromGrayscale(string path, Texture2D source)
	{
		if (alphaFromGrayscaleTextures.TryGetValue(path, out var value) && value != null)
		{
			return value;
		}
		if (!source.isReadable)
		{
			Log.ErrorOnce("Texture '" + path + "' is flagged alphaFromGrayscale but is not readable.", path.GetHashCode() ^ 0x5A17C3);
			return source;
		}
		Color32[] pixels = source.GetPixels32();
		for (int i = 0; i < pixels.Length; i++)
		{
			Color32 color = pixels[i];
			pixels[i].a = (byte)(color.r * 77 + color.g * 150 + color.b * 29 >> 8);
		}
		Texture2D texture2D = new Texture2D(source.width, source.height, TextureFormat.RGBA32, source.mipmapCount > 1);
		texture2D.name = source.name;
		texture2D.wrapMode = source.wrapMode;
		texture2D.filterMode = source.filterMode;
		texture2D.anisoLevel = source.anisoLevel;
		texture2D.SetPixels32(pixels);
		texture2D.Apply(updateMipmaps: true);
		texture2D.Compress(_: true);
		alphaFromGrayscaleTextures[path] = texture2D;
		return texture2D;
	}

	public static string NormalizeResourcePath(string resourcePath)
	{
		string text = (resourcePath ?? string.Empty).Replace('\\', '/').Trim();
		if (text.StartsWith("Materials/", StringComparison.OrdinalIgnoreCase))
		{
			text = text.Substring("Materials/".Length);
		}
		if (text.EndsWith(".mat", StringComparison.OrdinalIgnoreCase))
		{
			text = text.Substring(0, text.Length - ".mat".Length);
		}
		return text;
	}

	private static void EnsureCache()
	{
		if (materialDefsByResourcePath != null && materialDefCount == DefDatabase<MaterialDef>.DefCount)
		{
			return;
		}
		materialDefsByResourcePath = new Dictionary<string, MaterialDef>();
		materialDefCount = DefDatabase<MaterialDef>.DefCount;
		foreach (MaterialDef allDef in DefDatabase<MaterialDef>.AllDefs)
		{
			string text = NormalizeResourcePath(allDef.resourcePath);
			if (text.NullOrEmpty())
			{
				text = NormalizeResourcePath(allDef.defName);
			}
			materialDefsByResourcePath[text] = allDef;
		}
	}
}
