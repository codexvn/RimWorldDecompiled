using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using LudeonTK;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using Waylith.Assets;
using Waylith.Graphics;
using Waylith.Rendering;

namespace Verse;

public class StaticTextureAtlas
{
	public readonly TextureAtlasGroupKey groupKey;

	private List<UnityEngine.Texture2D> textures = new List<UnityEngine.Texture2D>();

	private Dictionary<UnityEngine.Texture2D, UnityEngine.Texture2D> masks = new Dictionary<UnityEngine.Texture2D, UnityEngine.Texture2D>();

	private Dictionary<UnityEngine.Texture, StaticTextureAtlasTile> tiles = new Dictionary<UnityEngine.Texture, StaticTextureAtlasTile>();

	private UnityEngine.Texture2D colorTexture;

	private UnityEngine.Texture2D maskTexture;

	private string colorCacheKey;

	private string maskCacheKey;

	public const int MaxTextureSizeForTiles = 512;

	public const int TexturePadding = 8;

	private const long BakeBytesPerPagePixel = 16L;

	private const int MinPagePixels = 4194304;

	private const int CacheBakeVersion = 1;

	public UnityEngine.Texture2D ColorTexture => colorTexture;

	public UnityEngine.Texture2D MaskTexture => maskTexture;

	public IEnumerable<UnityEngine.Texture2D> Members => textures.Concat(masks.Values);

	public static int MaxPixelsPerAtlas => MaxPagePixels / 8 * 5;

	public static int MaxAtlasSize => SystemInfo.maxTextureSize;

	public static int MaxPagePixels
	{
		get
		{
			long num = (long)MaxAtlasSize * (long)MaxAtlasSize / 2;
			long deviceLocalHeapBytes = TextureResidency.DeviceLocalHeapBytes;
			if (deviceLocalHeapBytes > 0)
			{
				num = Math.Min(num, deviceLocalHeapBytes / 6 / 16);
			}
			num = 1L << BitOperations.Log2((ulong)num);
			return (int)Math.Max(4194304L, num);
		}
	}

	public bool AdoptedFromCache { get; private set; }

	public string CacheKey(bool mask)
	{
		if (!mask)
		{
			return colorCacheKey;
		}
		return maskCacheKey;
	}

	public StaticTextureAtlas(TextureAtlasGroupKey groupKey)
	{
		this.groupKey = groupKey;
		colorTexture = new UnityEngine.Texture2D(1, 1, TextureFormat.ARGB32, mipChain: false);
		colorTexture.name = "StaticTextureAtlas";
	}

	public void Insert(UnityEngine.Texture2D texture, UnityEngine.Texture2D mask = null)
	{
		if (groupKey.hasMask && mask == null)
		{
			Log.Error("Tried to insert a mask-less texture into a static atlas which does have a mask atlas");
		}
		if (!groupKey.hasMask && mask != null)
		{
			Log.Error("Tried to insert a mask texture into a static atlas which does not have a mask atlas");
		}
		textures.Add(texture);
		if (mask != null && groupKey.hasMask)
		{
			masks.Add(texture, mask);
		}
	}

	public void Bake(bool rebake = false)
	{
		using (new DeepProfilerScope("StaticTextureAtlas.Bake()"))
		{
			if (rebake)
			{
				foreach (KeyValuePair<UnityEngine.Texture, StaticTextureAtlasTile> tile in tiles)
				{
					UnityEngine.Object.Destroy(tile.Value.mesh);
				}
				tiles.Clear();
			}
			Rect[] array = CalcRectsForAtlasNew();
			if (array.Length != textures.Count)
			{
				Log.Error("Texture packing failed! Clearing out atlas...");
				textures.Clear();
				return;
			}
			BuildMeshesForUvs(array);
			if (groupKey.hasMask)
			{
				CreateMaskTexture();
			}
			colorCacheKey = TryGetCacheKey(mask: false);
			maskCacheKey = (groupKey.hasMask ? TryGetCacheKey(mask: true) : null);
			AdoptedFromCache = TryAdoptCachedPages();
			if (!AdoptedFromCache)
			{
				BlitTexturesToColorAtlas(array);
				if (groupKey.hasMask)
				{
					BlitMaskAtlas(array);
				}
			}
		}
	}

	private bool TryAdoptCachedPages()
	{
		if (!GlobalTextureAtlasManager.AtlasCompressionEnabled)
		{
			return false;
		}
		if (!TryOpenCachedPage(mask: false, out var cached))
		{
			return false;
		}
		ITextureCacheSource cached2 = null;
		if (groupKey.hasMask && !TryOpenCachedPage(mask: true, out cached2))
		{
			cached.Dispose();
			return false;
		}
		using (cached)
		{
			using (cached2)
			{
				if (!colorTexture.CoreTexture.TryAdoptCompressed(cached) || (groupKey.hasMask && !maskTexture.CoreTexture.TryAdoptCompressed(cached2)))
				{
					RecreatePages();
					return false;
				}
			}
		}
		NameAtlasTextures();
		return true;
	}

	private void RecreatePages()
	{
		UnityEngine.Texture2D texture2D = colorTexture;
		colorTexture = CreatePage(texture2D.width, texture2D.height, texture2D.mipmapCount, "TextureAtlas_Working");
		UnityEngine.Object.DestroyImmediate(texture2D);
		if (groupKey.hasMask)
		{
			CreateMaskTexture();
		}
	}

	private bool TryOpenCachedPage(bool mask, out ITextureCacheSource cached)
	{
		string text = CacheKey(mask);
		if (text != null)
		{
			return GpuTextureService.TryOpenKeyedTextureCache(text, out cached);
		}
		cached = null;
		return false;
	}

	private void NameAtlasTextures()
	{
		colorTexture.name = "TextureAtlas_" + groupKey.ToString() + "_" + colorTexture.GetInstanceID();
		if (maskTexture != null)
		{
			maskTexture.name = "Mask_" + colorTexture.name;
		}
	}

	private UnityEngine.Texture2D CreatePage(int width, int height, int mipCount, string name)
	{
		UnityEngine.Texture2D texture2D = new UnityEngine.Texture2D(width, height, GraphicsFormat.R8G8B8A8_SRGB, mipCount, TextureCreationFlags.DontInitializePixels | TextureCreationFlags.MipChain);
		texture2D.CoreTexture.MemoryCategory = GpuMemoryCategory.Atlas;
		texture2D.name = name;
		ApplyAtlasTextureSamplingSettings(texture2D);
		return texture2D;
	}

	private string TryGetCacheKey(bool mask)
	{
		UnityEngine.Texture2D texture2D = (mask ? maskTexture : colorTexture);
		if (texture2D == null || tiles.Count != textures.Count)
		{
			return null;
		}
		using MemoryStream memoryStream = new MemoryStream();
		using BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
		binaryWriter.Write(1);
		binaryWriter.Write(typeof(StaticTextureAtlas).Assembly.ManifestModule.ModuleVersionId.ToByteArray());
		binaryWriter.Write(typeof(UnityEngine.Texture2D).Assembly.ManifestModule.ModuleVersionId.ToByteArray());
		binaryWriter.Write(typeof(GpuTextureService).Assembly.ManifestModule.ModuleVersionId.ToByteArray());
		binaryWriter.Write(TextureCompressionSettings.CacheTag);
		binaryWriter.Write(TextureCompressionSettings.Quality);
		binaryWriter.Write(groupKey.ToString());
		binaryWriter.Write(mask);
		binaryWriter.Write(texture2D.width);
		binaryWriter.Write(texture2D.height);
		binaryWriter.Write(texture2D.mipmapCount);
		binaryWriter.Write(8);
		foreach (UnityEngine.Texture2D texture in textures)
		{
			if (!TryWriteSourceStamp(binaryWriter, texture))
			{
				return null;
			}
			if (mask && (!masks.TryGetValue(texture, out var value) || !TryWriteSourceStamp(binaryWriter, value)))
			{
				return null;
			}
			if (!tiles.TryGetValue(texture, out var value2))
			{
				return null;
			}
			binaryWriter.Write(value2.uvRect.x);
			binaryWriter.Write(value2.uvRect.y);
			binaryWriter.Write(value2.uvRect.width);
			binaryWriter.Write(value2.uvRect.height);
		}
		binaryWriter.Flush();
		return Convert.ToHexString(SHA256.HashData(memoryStream.ToArray()));
	}

	private static bool TryWriteSourceStamp(BinaryWriter writer, UnityEngine.Texture2D texture)
	{
		string text = texture?.CoreTexture?.SourcePath;
		if (text == null)
		{
			return false;
		}
		writer.Write(text);
		if (ContentPack.TryGetTextureContentHash(text, out string hash))
		{
			writer.Write(hash);
			return true;
		}
		FileInfo fileInfo = new FileInfo(text);
		if (!fileInfo.Exists)
		{
			return false;
		}
		writer.Write(fileInfo.Length);
		writer.Write(fileInfo.LastWriteTimeUtc.Ticks);
		return true;
	}

	public void EvictSourceTextures()
	{
		foreach (UnityEngine.Texture2D texture in textures)
		{
			texture?.EvictGpuResource();
		}
		foreach (UnityEngine.Texture2D value in masks.Values)
		{
			value?.EvictGpuResource();
		}
	}

	private void BuildMeshesForUvs(Rect[] uvRects)
	{
		for (int i = 0; i < textures.Count; i++)
		{
			Mesh mesh = TextureAtlasHelper.CreateMeshForUV(uvRects[i], 0.5f);
			mesh.name = "TextureAtlasMesh_" + groupKey.ToString() + "_" + mesh.GetInstanceID();
			tiles.Add(textures[i], new StaticTextureAtlasTile
			{
				atlas = this,
				mesh = mesh,
				uvRect = uvRects[i]
			});
		}
	}

	private Rect[] CalcRectsForAtlasNew(int divisor = 1)
	{
		using (new DeepProfilerScope("StaticTextureAtlas.CalcRectsForAtlasNew()"))
		{
			List<UnityEngine.Texture2D> list = textures.OrderByDescending((UnityEngine.Texture2D t) => t.height).ToList();
			int a = Mathf.NextPowerOfTwo((int)((float)Mathf.CeilToInt(Mathf.Sqrt(list.Sum((UnityEngine.Texture2D t) => (t.width + 8) * (t.height + 8)))) * 1.05f));
			a = Mathf.Min(a, MaxAtlasSize);
			List<UnityEngine.Vector2> list2 = list.Select((UnityEngine.Texture2D t) => new UnityEngine.Vector2(Mathf.RoundToInt((float)t.width / (float)divisor), Mathf.RoundToInt((float)t.height / (float)divisor))).ToList();
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			Rect[] array = new Rect[list.Count];
			for (int i = 0; i < list.Count; i++)
			{
				UnityEngine.Vector2 vector = list2[i];
				if ((float)num + vector.x > (float)a)
				{
					num = 0;
					num2 += num3 + 8;
					num3 = 0;
				}
				array[i] = new Rect(num, num2, vector.x, vector.y);
				num += Mathf.CeilToInt(vector.x) + 8;
				if (vector.y > (float)num3)
				{
					num3 = Mathf.CeilToInt(vector.y);
				}
			}
			int num4 = num2 + num3;
			int num5 = Mathf.NextPowerOfTwo(num4);
			if (num4 > MaxAtlasSize || (long)a * (long)num5 > MaxPagePixels)
			{
				if (divisor < 4)
				{
					Log.Warning($"StaticTextureAtlas: Textures pack to {a}x{num4}, over the page limit of {MaxPagePixels} pixels (max axis {MaxAtlasSize}). Retrying with divisor of {divisor * 2}.");
					return CalcRectsForAtlasNew(divisor * 2);
				}
				return CalcRectsForAtlas();
			}
			Rect[] array2 = new Rect[list.Count];
			for (int j = 0; j < textures.Count; j++)
			{
				UnityEngine.Texture2D texture2D = textures[j];
				int num6 = list.IndexOf(texture2D);
				if (num6 < 0)
				{
					Log.Error("Texture " + texture2D.name + " not found in height sorted list, cannot calculate UV rect.");
					continue;
				}
				Rect rect = array[num6];
				array2[j] = new Rect(rect.x / (float)a, rect.y / (float)num5, rect.width / (float)a, rect.height / (float)num5);
			}
			UnityEngine.Texture2D obj = colorTexture;
			int mipCount = Mathf.FloorToInt(Mathf.Log((float)Mathf.Min(a, num5) / 512f, 2f)) + 1;
			colorTexture = CreatePage(a, num5, mipCount, "TextureAtlas_Working");
			UnityEngine.Object.DestroyImmediate(obj);
			return array2;
		}
	}

	private Rect[] CalcRectsForAtlas()
	{
		UnityEngine.Texture2D[] array;
		using (new DeepProfilerScope("Create dummy textures for atlas packing"))
		{
			array = textures.Select(delegate(UnityEngine.Texture2D t)
			{
				int width = t.width;
				int height = t.height;
				bool flag = width % 4 == 0 && height % 4 == 0;
				return new UnityEngine.Texture2D(width, height, (!flag) ? TextureFormat.Alpha8 : TextureFormat.DXT1, 1, linear: false, createUninitialized: true)
				{
					name = t.name + "_DummyForAtlas",
					filterMode = t.filterMode,
					wrapMode = t.wrapMode,
					anisoLevel = t.anisoLevel,
					mipMapBias = t.mipMapBias,
					minimumMipmapLevel = t.minimumMipmapLevel
				};
			}).ToArray();
		}
		using (new DeepProfilerScope("PackTextures() with dummy textures"))
		{
			Rect[] result = colorTexture.PackTextures(array, 8, MaxAtlasSize, makeNoLongerReadable: false);
			UnityEngine.Texture2D texture2D = colorTexture;
			UnityEngine.Texture2D[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				UnityEngine.Object.DestroyImmediate(array2[i]);
			}
			int mipCount = Mathf.FloorToInt(Mathf.Log((float)Mathf.Min(texture2D.width, texture2D.height) / 512f, 2f)) + 1;
			colorTexture = CreatePage(texture2D.width, texture2D.height, mipCount, "TextureAtlas_Working");
			UnityEngine.Object.DestroyImmediate(texture2D);
			return result;
		}
	}

	private void BlitTexturesToColorAtlas(Rect[] uvRects)
	{
		UnityEngine.RenderTexture renderTexture = new UnityEngine.RenderTexture(colorTexture.width, colorTexture.height, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear)
		{
			name = "StaticTextureAtlas_Bake_Temp"
		};
		renderTexture.Create();
		UnityEngine.RenderTexture active = UnityEngine.RenderTexture.active;
		UnityEngine.Graphics.SetRenderTarget(renderTexture);
		GL.Clear(clearDepth: true, clearColor: true, Color.clear);
		GL.PushMatrix();
		GL.LoadPixelMatrix(0f, renderTexture.width, renderTexture.height, 0f);
		using (new DeepProfilerScope("Render real textures to RT"))
		{
			UnityEngine.Material material = MaterialAllocator.Create(UnityEngine.Shader.Find("Custom/BlitExact"));
			material.name = "StaticTextureAtlas.BlitTexturesToColorAtlas.Blit";
			try
			{
				for (int i = 0; i < textures.Count; i++)
				{
					Rect rect = uvRects[i];
					UnityEngine.Texture2D texture = textures[i];
					int num = Mathf.RoundToInt(rect.x * (float)colorTexture.width);
					int num2 = Mathf.RoundToInt((1f - rect.y) * (float)colorTexture.height);
					int num3 = Mathf.RoundToInt(rect.width * (float)colorTexture.width);
					int num4 = Mathf.RoundToInt(rect.height * (float)colorTexture.height);
					num2 -= num4;
					UnityEngine.Graphics.DrawTexture(new Rect(num, num2, num3, num4), texture, new Rect(0f, 0f, 1f, 1f), 0, 0, 0, 0, material, -1);
				}
			}
			finally
			{
				MaterialAllocator.Destroy(material);
			}
		}
		GL.PopMatrix();
		using (new DeepProfilerScope("CopyTexture from RenderTexture to Texture2D"))
		{
			UnityEngine.Graphics.CopyTexture(renderTexture, 0, 0, 0, 0, colorTexture.width, colorTexture.height, colorTexture, 0, 0, 0, 0);
		}
		UnityEngine.RenderTexture.active = active;
		using (new DeepProfilerScope("Generate mipmaps"))
		{
			GenTexture.GenerateMipmaps(colorTexture, 512, preserveAlphaCoverage: false, 0.25f);
		}
		renderTexture.Release();
		UnityEngine.Object.DestroyImmediate(renderTexture);
		colorTexture.name = "TextureAtlas_" + groupKey.ToString() + "_" + colorTexture.GetInstanceID();
	}

	private void CreateMaskTexture()
	{
		if (maskTexture != null)
		{
			UnityEngine.Object.DestroyImmediate(maskTexture);
		}
		maskTexture = CreatePage(colorTexture.width, colorTexture.height, colorTexture.mipmapCount, "TextureAtlasMask_Working");
	}

	private void BlitMaskAtlas(Rect[] uvRects)
	{
		UnityEngine.RenderTexture renderTexture = new UnityEngine.RenderTexture(maskTexture.width, maskTexture.height, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear)
		{
			name = "StaticTextureAtlas_MaskBake_Temp"
		};
		renderTexture.Create();
		UnityEngine.RenderTexture active = UnityEngine.RenderTexture.active;
		UnityEngine.Graphics.SetRenderTarget(renderTexture);
		GL.Clear(clearDepth: true, clearColor: true, Color.clear);
		GL.PushMatrix();
		GL.LoadPixelMatrix(0f, renderTexture.width, renderTexture.height, 0f);
		using (new DeepProfilerScope("Render mask textures to RT"))
		{
			UnityEngine.Material material = MaterialAllocator.Create(UnityEngine.Shader.Find("Custom/BlitExact"));
			material.name = "StaticTextureAtlas.BuildMaskAtlas.Blit";
			try
			{
				for (int i = 0; i < textures.Count; i++)
				{
					UnityEngine.Texture2D key = textures[i];
					if (masks.TryGetValue(key, out var value))
					{
						Rect rect = uvRects[i];
						int num = Mathf.RoundToInt(rect.x * (float)maskTexture.width);
						int num2 = Mathf.RoundToInt((1f - rect.y) * (float)maskTexture.height);
						int num3 = Mathf.RoundToInt(rect.width * (float)maskTexture.width);
						int num4 = Mathf.RoundToInt(rect.height * (float)maskTexture.height);
						num2 -= num4;
						UnityEngine.Graphics.DrawTexture(new Rect(num, num2, num3, num4), value, new Rect(0f, 0f, 1f, 1f), 0, 0, 0, 0, material, -1);
					}
				}
			}
			finally
			{
				MaterialAllocator.Destroy(material);
			}
		}
		GL.PopMatrix();
		using (new DeepProfilerScope("CopyTexture from RenderTexture to mask Texture2D"))
		{
			UnityEngine.Graphics.CopyTexture(renderTexture, 0, 0, 0, 0, maskTexture.width, maskTexture.height, maskTexture, 0, 0, 0, 0);
		}
		UnityEngine.RenderTexture.active = active;
		using (new DeepProfilerScope("Generate mipmaps for mask texture"))
		{
			GenTexture.GenerateMipmaps(maskTexture, 512);
		}
		renderTexture.Release();
		UnityEngine.Object.DestroyImmediate(renderTexture);
		maskTexture.name = "Mask_" + colorTexture.name;
	}

	public bool TryGetTile(UnityEngine.Texture texture, out StaticTextureAtlasTile tile)
	{
		return tiles.TryGetValue(texture, out tile);
	}

	private void ApplyAtlasTextureSamplingSettings(UnityEngine.Texture2D texture)
	{
		if (texture == null)
		{
			return;
		}
		UnityEngine.FilterMode filterMode = UnityEngine.FilterMode.Bilinear;
		int num = 1;
		float num2 = 0f;
		for (int i = 0; i < textures.Count; i++)
		{
			UnityEngine.Texture2D texture2D = textures[i];
			if (!(texture2D == null))
			{
				if (texture2D.filterMode == UnityEngine.FilterMode.Trilinear)
				{
					filterMode = UnityEngine.FilterMode.Trilinear;
				}
				else if (texture2D.filterMode == UnityEngine.FilterMode.Point && filterMode == UnityEngine.FilterMode.Bilinear)
				{
					filterMode = UnityEngine.FilterMode.Point;
				}
				num = Math.Max(num, texture2D.anisoLevel);
				num2 = Math.Min(num2, texture2D.mipMapBias);
			}
		}
		texture.filterMode = filterMode;
		texture.wrapMode = UnityEngine.TextureWrapMode.Clamp;
		texture.anisoLevel = Math.Max(2, num);
		texture.mipMapBias = num2;
		texture.minimumMipmapLevel = 0;
	}

	public static UnityEngine.Texture2D FastCompressDXT(UnityEngine.Texture2D texture, bool deleteOriginal = false)
	{
		if (Mathf.Min(texture.width, texture.height) <= 16)
		{
			return texture;
		}
		texture.Compress(_: true);
		return texture;
	}

	public void Destroy()
	{
		UnityEngine.Object.Destroy(colorTexture);
		UnityEngine.Object.Destroy(maskTexture);
		foreach (KeyValuePair<UnityEngine.Texture, StaticTextureAtlasTile> tile in tiles)
		{
			UnityEngine.Object.Destroy(tile.Value.mesh);
		}
		textures.Clear();
		masks.Clear();
		tiles.Clear();
	}

	public static int CalculateMaxMipmapsForDxtSupport(UnityEngine.Texture2D tex)
	{
		int num = 0;
		int num2 = tex.width;
		int num3 = tex.height;
		while (num2 >= 4 && num3 >= 4 && num2 % 4 == 0 && num3 % 4 == 0)
		{
			num++;
			num2 >>= 1;
			num3 >>= 1;
		}
		if (num == 0)
		{
			num = 1;
		}
		return num;
	}
}
