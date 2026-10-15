using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Brutal.VulkanApi;
using RimWorld;
using UnityEngine;
using Waylith.Graphics;
using Waylith.Logging;
using Waylith.Rendering;

namespace Verse;

public static class GlobalTextureAtlasManager
{
	public static bool rebakeAtlas = false;

	private static List<PawnTextureAtlas> pawnTextureAtlases = new List<PawnTextureAtlas>();

	private static List<StaticTextureAtlas> staticTextureAtlases = new List<StaticTextureAtlas>();

	private static HashSet<UnityEngine.Texture2D> evictableSources = new HashSet<UnityEngine.Texture2D>();

	private static HashSet<Waylith.Rendering.Texture2D> evictableCores = new HashSet<Waylith.Rendering.Texture2D>();

	private static readonly List<Task> pendingCacheWrites = new List<Task>();

	private static Dictionary<TextureAtlasGroupKey, (List<UnityEngine.Texture2D>, HashSet<UnityEngine.Texture2D>)> buildQueue = new Dictionary<TextureAtlasGroupKey, (List<UnityEngine.Texture2D>, HashSet<UnityEngine.Texture2D>)>();

	private static Dictionary<UnityEngine.Texture2D, UnityEngine.Texture2D> buildQueueMasks = new Dictionary<UnityEngine.Texture2D, UnityEngine.Texture2D>();

	private static bool AtlasSourceEvictionEnabled => !GenCommandLine.CommandLineArgPassed("noEvictAtlasSources");

	internal static bool AtlasCompressionEnabled
	{
		get
		{
			if (Prefs.TextureCompression && TextureCompressionSettings.Enabled)
			{
				return !GenCommandLine.CommandLineArgPassed("noCompressAtlases");
			}
			return false;
		}
	}

	public static void ClearStaticAtlasBuildQueue()
	{
		buildQueue.Clear();
	}

	public static bool TryInsertStatic(TextureAtlasGroup group, UnityEngine.Texture2D texture, UnityEngine.Texture2D mask = null)
	{
		if (texture.width >= 512 || texture.height >= 512)
		{
			return false;
		}
		if (mask != null && (texture.width != mask.width || texture.height != mask.height))
		{
			Log.Warning("Texture " + texture.name + " has dimensions of " + texture.width + " x " + texture.height + ", but its mask has " + mask.width + " x " + mask.height + ". This is not supported, texture will be excluded from atlas");
			return false;
		}
		TextureAtlasGroupKey textureAtlasGroupKey = default(TextureAtlasGroupKey);
		textureAtlasGroupKey.group = group;
		textureAtlasGroupKey.hasMask = mask != null;
		TextureAtlasGroupKey key = textureAtlasGroupKey;
		if (!buildQueue.TryGetValue(key, out var value))
		{
			value = (new List<UnityEngine.Texture2D>(), new HashSet<UnityEngine.Texture2D>());
			buildQueue.Add(key, value);
		}
		if (value.Item2.Add(texture))
		{
			value.Item1.Add(texture);
		}
		if (mask != null)
		{
			if (buildQueueMasks.ContainsKey(texture))
			{
				if (buildQueueMasks[texture] != mask)
				{
					Log.Error("Same texture with 2 different masks inserted into texture atlas manager (" + texture.name + ") - " + mask.name + " | " + buildQueueMasks[texture].name + "!");
				}
			}
			else
			{
				buildQueueMasks.Add(texture, mask);
			}
		}
		return true;
	}

	public static void BakeStaticAtlases()
	{
		BuildingsDamageSectionLayerUtility.TryInsertIntoAtlas();
		MinifiedThing.TryInsertIntoAtlas();
		int pixels = 0;
		List<UnityEngine.Texture2D> currentBatch = new List<UnityEngine.Texture2D>();
		try
		{
			foreach (KeyValuePair<TextureAtlasGroupKey, (List<UnityEngine.Texture2D>, HashSet<UnityEngine.Texture2D>)> item in buildQueue.OrderBy<KeyValuePair<TextureAtlasGroupKey, (List<UnityEngine.Texture2D>, HashSet<UnityEngine.Texture2D>)>, string>((KeyValuePair<TextureAtlasGroupKey, (List<UnityEngine.Texture2D>, HashSet<UnityEngine.Texture2D>)> g) => g.Key.ToString(), StringComparer.Ordinal))
			{
				item.Value.Item1.Sort((UnityEngine.Texture2D a, UnityEngine.Texture2D b) => string.CompareOrdinal(a.CoreTexture?.SourcePath ?? a.name, b.CoreTexture?.SourcePath ?? b.name));
				foreach (UnityEngine.Texture2D item2 in item.Value.Item1)
				{
					int num = (item2.width + 8) * (item2.height + 8);
					if (num + pixels > StaticTextureAtlas.MaxPixelsPerAtlas)
					{
						FlushBatch(item.Key);
					}
					pixels += num;
					currentBatch.Add(item2);
				}
				FlushBatch(item.Key);
			}
		}
		finally
		{
			evictableSources = (from texture in staticTextureAtlases.SelectMany((StaticTextureAtlas atlas) => atlas.Members)
				where texture != null
				select texture).ToHashSet();
			evictableCores = (from texture in evictableSources
				select texture.CoreTexture into core
				where core != null
				select core).ToHashSet();
			Waylith.Rendering.Texture2D.EndDeferredLoading(LeavesSourceEvicted);
		}
		DrainCacheWrites();
		VerboseLog.Log($"Static atlases: {staticTextureAtlases.Count} pages, {staticTextureAtlases.Count((StaticTextureAtlas atlas) => atlas.AdoptedFromCache)} loaded from cache, page limit {StaticTextureAtlas.MaxPagePixels / 1048576} Mpx.");
		EvictAtlasSourceTextures();
		void FlushBatch(TextureAtlasGroupKey groupKey)
		{
			StaticTextureAtlas staticTextureAtlas = new StaticTextureAtlas(groupKey);
			foreach (UnityEngine.Texture2D item3 in currentBatch)
			{
				if (!groupKey.hasMask || !buildQueueMasks.TryGetValue(item3, out var value))
				{
					value = null;
				}
				staticTextureAtlas.Insert(item3, value);
			}
			staticTextureAtlas.Bake();
			CompressAtlas(staticTextureAtlas);
			staticTextureAtlases.Add(staticTextureAtlas);
			if (AtlasSourceEvictionEnabled)
			{
				staticTextureAtlas.EvictSourceTextures();
			}
			GpuTextureService.RetireDeferredDisposalsAfterIdle();
			pixels = 0;
			currentBatch.Clear();
		}
	}

	public static bool LeavesSourceEvicted(UnityEngine.Texture2D texture)
	{
		if (AtlasSourceEvictionEnabled)
		{
			return evictableSources.Contains(texture);
		}
		return false;
	}

	public static bool LeavesSourceEvicted(Waylith.Rendering.Texture2D texture)
	{
		if (AtlasSourceEvictionEnabled)
		{
			return evictableCores.Contains(texture);
		}
		return false;
	}

	private static void CompressAtlas(StaticTextureAtlas atlas)
	{
		if (!AtlasCompressionEnabled)
		{
			return;
		}
		using (new DeepProfilerScope("Compress atlas textures"))
		{
			BcEncodeTarget target = TextureCompressionSettings.Target;
			float quality = TextureCompressionSettings.Quality;
			CompressAtlasPage(atlas.ColorTexture, atlas.CacheKey(mask: false), target, quality);
			CompressAtlasPage(atlas.MaskTexture, atlas.CacheKey(mask: true), target, quality);
		}
	}

	private static void CompressAtlasPage(UnityEngine.Texture2D page, string cacheKey, BcEncodeTarget target, float quality)
	{
		Waylith.Rendering.Texture2D texture2D = page?.CoreTexture;
		if (texture2D?.CpuData == null || TexturePixelData.IsBlockCompressed(texture2D.CpuData.Format))
		{
			return;
		}
		if (cacheKey != null && GpuTextureService.TryLoadKeyedTextureCache(cacheKey, out var loadedTexture) && texture2D.TryAdoptCompressed(in loadedTexture))
		{
			ReleaseCpuLevels(texture2D);
		}
		else if (!TryEncodeOnGpu(page, cacheKey, target, quality) && texture2D.CompressInPlace(target, quality))
		{
			if (cacheKey != null)
			{
				QueueCacheWrite(GpuTextureService.BeginWriteKeyedTextureCache(cacheKey, texture2D));
			}
			ReleaseCpuLevels(texture2D);
		}
	}

	private static bool TryEncodeOnGpu(UnityEngine.Texture2D page, string cacheKey, BcEncodeTarget target, float quality)
	{
		Waylith.Rendering.Texture2D coreTexture = page.CoreTexture;
		if (target == BcEncodeTarget.Bc7)
		{
			TexturePixelData cpuData = coreTexture.CpuData;
			if (cpuData != null && !cpuData.HasData && coreTexture.GpuResource != null && TextureBlockEncoder.CanEncodeBc7Fast)
			{
				GpuTextureAllocation gpuTextureAllocation = GpuTextureService.CreateEmptyTexture2D(page.name, page.width, page.height, coreTexture.MipCount, TexturePixelData.ToVkFormat(LoadedTextureFormat.Bc7), VkImageUsageFlags.None, GpuMemoryCategory.Atlas);
				byte[][] array = ((gpuTextureAllocation == null) ? null : TextureBlockEncoder.TryEncodeBc7Fast(coreTexture.GpuResource, gpuTextureAllocation, page.width, page.height, coreTexture.MipCount, quality));
				if (array == null)
				{
					gpuTextureAllocation?.Dispose();
					return false;
				}
				coreTexture.AdoptEncodedImage(gpuTextureAllocation, LoadedTextureFormat.Bc7, array.Length);
				if (cacheKey != null)
				{
					QueueCacheWrite(GpuTextureService.BeginWriteKeyedTextureCache(cacheKey, new LoadedTextureData(page.width, page.height, array.Length, LoadedTextureFormat.Bc7, array, Opaque: false, null)));
				}
				return true;
			}
		}
		return false;
	}

	private static void ReleaseCpuLevels(Waylith.Rendering.Texture2D core)
	{
		core.CpuData?.ReleaseTracked();
		core.CpuData?.Release();
	}

	private static void QueueCacheWrite(Task write)
	{
		if (!write.IsCompleted)
		{
			pendingCacheWrites.Add(write);
		}
	}

	private static void DrainCacheWrites()
	{
		if (pendingCacheWrites.Count != 0)
		{
			using (new DeepProfilerScope("Atlas cache write drain"))
			{
				Task.WaitAll(pendingCacheWrites.ToArray());
			}
			pendingCacheWrites.Clear();
		}
	}

	public static void EvictAtlasSourceTextures()
	{
		if (!AtlasSourceEvictionEnabled)
		{
			return;
		}
		foreach (StaticTextureAtlas staticTextureAtlase in staticTextureAtlases)
		{
			staticTextureAtlase.EvictSourceTextures();
		}
		GpuTextureService.RetireDeferredDisposalsAfterIdle();
	}

	public static bool TryGetStaticTile(TextureAtlasGroup group, UnityEngine.Texture2D texture, out StaticTextureAtlasTile tile, bool ignoreFoundInOtherAtlas = false)
	{
		foreach (StaticTextureAtlas staticTextureAtlase in staticTextureAtlases)
		{
			if (staticTextureAtlase.groupKey.group == group && staticTextureAtlase.TryGetTile(texture, out tile))
			{
				return true;
			}
		}
		foreach (StaticTextureAtlas staticTextureAtlase2 in staticTextureAtlases)
		{
			if (staticTextureAtlase2.TryGetTile(texture, out tile))
			{
				if (!ignoreFoundInOtherAtlas)
				{
					Log.Warning("Found texture " + texture.name + " in another atlas group than requested (found in " + staticTextureAtlase2.groupKey.ToString() + ", requested in " + group.ToString() + ")!");
				}
				return true;
			}
		}
		tile = null;
		return false;
	}

	public static bool TryGetPawnFrameSet(Pawn pawn, out PawnTextureAtlasFrameSet frameSet, out bool createdNew, bool allowCreatingNew = true)
	{
		foreach (PawnTextureAtlas pawnTextureAtlase in pawnTextureAtlases)
		{
			if (pawnTextureAtlase.TryGetFrameSet(pawn, out frameSet, out createdNew))
			{
				return true;
			}
		}
		if (allowCreatingNew)
		{
			PawnTextureAtlas pawnTextureAtlas = new PawnTextureAtlas();
			pawnTextureAtlases.Add(pawnTextureAtlas);
			return pawnTextureAtlas.TryGetFrameSet(pawn, out frameSet, out createdNew);
		}
		createdNew = false;
		frameSet = null;
		return false;
	}

	public static bool TryMarkPawnFrameSetDirty(Pawn pawn)
	{
		if (!TryGetPawnFrameSet(pawn, out var frameSet, out var _, allowCreatingNew: false))
		{
			return false;
		}
		for (int i = 0; i < frameSet.isDirty.Length; i++)
		{
			frameSet.isDirty[i] = true;
		}
		return true;
	}

	public static void GlobalTextureAtlasManagerUpdate()
	{
		if (rebakeAtlas)
		{
			FreeAllRuntimeAtlases();
			PortraitsCache.Clear();
			rebakeAtlas = false;
		}
		foreach (PawnTextureAtlas pawnTextureAtlase in pawnTextureAtlases)
		{
			pawnTextureAtlase.GC();
		}
	}

	public static void FreeAllRuntimeAtlases()
	{
		foreach (PawnTextureAtlas pawnTextureAtlase in pawnTextureAtlases)
		{
			pawnTextureAtlase.Destroy();
		}
		pawnTextureAtlases.Clear();
	}

	public static void DumpPawnAtlases(string folder)
	{
		foreach (PawnTextureAtlas pawnTextureAtlase in pawnTextureAtlases)
		{
			TextureAtlasHelper.WriteDebugPNG(pawnTextureAtlase.RawTexture, folder + "\\dump_" + pawnTextureAtlase.RawTexture.GetInstanceID() + ".png");
		}
		Log.Message("Atlases have been dumped to " + folder);
	}

	public static void DumpStaticAtlases(string folder)
	{
		foreach (StaticTextureAtlas staticTextureAtlase in staticTextureAtlases)
		{
			TextureAtlasHelper.WriteDebugPNG(staticTextureAtlase.ColorTexture, folder + "\\" + staticTextureAtlase.ColorTexture.name + ".png");
			if (staticTextureAtlase.MaskTexture != null)
			{
				TextureAtlasHelper.WriteDebugPNG(staticTextureAtlase.MaskTexture, folder + "\\" + staticTextureAtlase.MaskTexture.name + ".png");
			}
		}
		Log.Message("Atlases have been dumped to " + folder);
	}
}
