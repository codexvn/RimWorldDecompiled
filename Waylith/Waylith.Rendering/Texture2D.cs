using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Brutal.Logging;
using Brutal.Logging.Internal;
using Brutal.VulkanApi;
using Waylith.Jobs;
using Waylith.Logging;
using Waylith.Math;

namespace Waylith.Rendering;

public class Texture2D : Texture
{
	private int _width;

	private int _height;

	private static Texture2D? _whiteTexture;

	private static Texture2D? _redTexture;

	private static ConcurrentBag<Texture2D>? _deferredLoads;

	private static readonly ReaderWriterLockSlim DeferredLoadsLock = new ReaderWriterLockSlim();

	private static readonly Lock PendingEvictionsGate = new Lock();

	private static List<(Texture2D Texture, int Attempts)> _pendingEvictions = new List<(Texture2D, int)>();

	private const int MaxEvictionAttempts = 600;

	private const int MaxEvictionsPerDrain = 8192;

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

	public int minimumMipmapLevel { get; set; }

	public TexturePixelData? CpuData { get; private set; }

	public string? SourcePath { get; private set; }

	internal (long Length, long WriteTicks)? SourceStamp { get; private set; }

	public bool Opaque { get; set; }

	public string? TrackerName { get; set; }

	public override string AllocationLabel => TrackerName ?? base.Name;

	public override bool IsReadable => CpuData?.HasData ?? false;

	public override bool CanRehydrate
	{
		get
		{
			if (!IsReadable)
			{
				return SourcePath != null;
			}
			return true;
		}
	}

	public static Texture2D whiteTexture => _whiteTexture ?? (_whiteTexture = CreateSolid("WhiteTexture", new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)));

	public static Texture2D redTexture => _redTexture ?? (_redTexture = CreateSolid("RedTexture", new Color32(byte.MaxValue, 0, 0, byte.MaxValue)));

	public static bool DeferringLoads => _deferredLoads != null;

	public Texture2D(int width, int height)
		: this(width, height, LoadedTextureFormat.Rgba32, linear: false, 1, createUninitialized: false)
	{
	}

	public Texture2D(int width, int height, LoadedTextureFormat format, bool mipChain, bool linear = false)
		: this(width, height, format, linear, (!mipChain) ? 1 : TexturePixelData.CalculateMipCount(width, height), createUninitialized: false)
	{
	}

	public static Texture2D CreateUnbacked(int width, int height)
	{
		return new Texture2D(System.Math.Max(1, width), System.Math.Max(1, height), attachStorage: false);
	}

	private Texture2D(int width, int height, bool attachStorage)
	{
		_width = width;
		_height = height;
	}

	public Texture2D(int width, int height, LoadedTextureFormat format, bool linear, int mipCount, bool createUninitialized)
	{
		_width = System.Math.Max(1, width);
		_height = System.Math.Max(1, height);
		base.MipCount = System.Math.Max(1, mipCount);
		CpuData = new TexturePixelData(_width, _height, format, linear, base.MipCount, !createUninitialized);
		if (!createUninitialized)
		{
			UploadCpuData();
		}
	}

	public Color GetPixel(int x, int y)
	{
		return RequireCpuData().GetPixel(x, y);
	}

	public Color[] GetPixels()
	{
		return RequireCpuData().GetPixels();
	}

	public Color32[] GetPixels32()
	{
		return RequireCpuData().GetPixels32();
	}

	public void SetPixel(int x, int y, Color color)
	{
		RequireCpuData().SetPixel(x, y, color);
	}

	public void SetPixels(Color[] colors)
	{
		RequireCpuData().SetPixels(colors);
	}

	public void SetPixels32(Color32[] colors)
	{
		RequireCpuData().SetPixels32(colors);
	}

	private TexturePixelData RequireCpuData()
	{
		TexturePixelData texturePixelData = CpuData ?? throw new InvalidOperationException("Texture '" + base.Name + "' has no CPU pixel data.");
		if (!texturePixelData.HasData && SourcePath != null && GpuTextureService.TryLoadTextureFromCache(SourcePath, texturePixelData.AlphaIsPremultiplied, out var loadedTexture))
		{
			texturePixelData.Replace(loadedTexture.Width, loadedTexture.Height, loadedTexture.Format, loadedTexture.Levels, loadedTexture.MipCount, texturePixelData.Linear);
			texturePixelData.TrackCurrentBytes();
			texturePixelData.MarkClean();
		}
		return texturePixelData;
	}

	public TexturePixelData? EnsureCpuData()
	{
		if (CpuData != null)
		{
			return RequireCpuData();
		}
		return null;
	}

	public void Apply(bool updateMipmaps = true, bool makeNoLongerReadable = false)
	{
		TexturePixelData cpuData = CpuData;
		if (cpuData == null || !cpuData.HasData)
		{
			return;
		}
		if (PendingBuild != null)
		{
			if (!CpuData.Dirty)
			{
				if (makeNoLongerReadable)
				{
					ReleaseCpuData();
				}
				return;
			}
			TextureBuildBatch.Cancel(this);
		}
		if (updateMipmaps && (CpuData.Dirty || CpuData.Levels.Length < base.MipCount))
		{
			CpuData.GenerateMipmaps();
		}
		if (CpuData.Dirty || base.GpuResource == null)
		{
			UploadCpuData();
		}
		if (makeNoLongerReadable)
		{
			ReleaseCpuData();
		}
	}

	public void UploadCpuData()
	{
		if (CpuData != null)
		{
			if (PendingBuild != null)
			{
				TextureBuildBatch.Cancel(this);
			}
			UploadLevels(CpuData.BuildLevelUploads(), null);
		}
	}

	private void UploadLevels(TextureLevelUpload[] uploads, TextureLevelFill? fill)
	{
		TexturePixelData cpuData = CpuData;
		base.GpuResource?.Dispose();
		base.GpuResource = null;
		cpuData.Name = base.Name;
		int num = System.Math.Max(base.MipCount, uploads.Length);
		GpuTextureAllocation gpuTextureAllocation = GpuTextureService.CreateTexture2D(string.IsNullOrEmpty(AllocationLabel) ? $"Texture2D_{GetHashCode()}" : AllocationLabel, _width, _height, num, TexturePixelData.ToVkFormat(cpuData.Format), uploads, base.MemoryCategory, fill, batchUpload: true, cpuData.Swizzle);
		if (num > uploads.Length && uploads.Length != 0 && gpuTextureAllocation != null && !TexturePixelData.IsBlockCompressed(cpuData.Format))
		{
			GpuTextureService.GenerateMipmaps(gpuTextureAllocation, _width, _height, num, 1, preserveAlphaCoverage: false, 0.5f, cpuData.AlphaIsPremultiplied);
		}
		base.GpuResource = gpuTextureAllocation;
		cpuData.TrackCurrentBytes();
		cpuData.MarkClean();
	}

	private static TextureLevelUpload[] BuildCacheUploads(ITextureCacheSource cached)
	{
		TextureLevelUpload[] array = new TextureLevelUpload[cached.MipCount];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = TexturePixelData.BuildLevelUpload(i, cached.Width, cached.Height, 1, cached.Format, null, cached.LevelLength(i));
		}
		return array;
	}

	public byte[]? TryGetCpuMip0()
	{
		TexturePixelData cpuData = CpuData;
		if (cpuData != null && cpuData.HasData && cpuData.Format == LoadedTextureFormat.Rgba32)
		{
			return cpuData.Levels[0];
		}
		if (SourcePath == null)
		{
			return null;
		}
		if (!GpuTextureService.TryOpenTextureCache(SourcePath, CpuData?.AlphaIsPremultiplied ?? false, out ITextureCacheSource file))
		{
			return null;
		}
		using (file)
		{
			if (file.Width != _width || file.Height != _height || file.MipCount <= 0 || file.LevelLength(0) != DdsFile.LevelLength(_width, _height, file.Format) || (file.Format != LoadedTextureFormat.Rgba32 && !TexturePixelData.IsDecodable(file.Format)))
			{
				return null;
			}
			byte[] array = new byte[file.LevelLength(0)];
			file.ReadLevel(0, array);
			return (file.Format == LoadedTextureFormat.Rgba32) ? array : TexturePixelData.DecodeToRgba32(array, _width, _height, file.Format, file.Swizzle);
		}
	}

	private void ReleaseCpuLevels()
	{
		TexturePixelData cpuData = CpuData;
		if (cpuData != null && cpuData.HasData)
		{
			cpuData.ReleaseTracked();
			cpuData.Release();
		}
	}

	public void ReplaceCpuData(int newWidth, int newHeight, LoadedTextureFormat format, byte[][] levels, int mipCount, bool linear, bool upload = true)
	{
		_width = newWidth;
		_height = newHeight;
		base.MipCount = System.Math.Max(1, mipCount);
		if (CpuData == null)
		{
			CpuData = new TexturePixelData(newWidth, newHeight, format, linear, base.MipCount, allocate: false)
			{
				Name = base.Name
			};
		}
		CpuData.Replace(newWidth, newHeight, format, levels, base.MipCount, linear);
		if (upload)
		{
			UploadCpuData();
		}
	}

	public bool TryEnsureGpuAllocated()
	{
		if (base.GpuResource != null)
		{
			return true;
		}
		if (PendingBuild != null)
		{
			return false;
		}
		base.GpuResource = GpuTextureService.CreateEmptyTexture2D(string.IsNullOrEmpty(base.Name) ? $"Texture2D_{GetHashCode()}" : base.Name, _width, _height, System.Math.Max(1, base.MipCount), TexturePixelData.ToVkFormat(CpuData?.Format ?? LoadedTextureFormat.Rgba32), VkImageUsageFlags.None, base.MemoryCategory);
		return base.GpuResource != null;
	}

	public GpuTextureAllocation? GetOrAllocateGpuResource()
	{
		TryEnsureGpuAllocated();
		return base.GpuResource;
	}

	public void Evict()
	{
		if (base.GpuResource != null && CanRehydrate)
		{
			base.GpuResource.Dispose();
			base.GpuResource = null;
		}
	}

	public override bool EnsureResident()
	{
		if (base.GpuResource != null)
		{
			return true;
		}
		if (CpuData == null || PendingBuild != null)
		{
			return false;
		}
		if (CpuData.HasData)
		{
			UploadCpuData();
			return base.GpuResource != null;
		}
		if (!TryUploadFromCache())
		{
			return false;
		}
		GpuTextureService.NoteRehydrated(SourcePath, _width, _height);
		return true;
	}

	private bool TryUploadFromCache()
	{
		if (SourcePath == null || CpuData == null)
		{
			return false;
		}
		if (!GpuTextureService.TryOpenTextureCache(SourcePath, CpuData.AlphaIsPremultiplied, out ITextureCacheSource file))
		{
			return TryUploadFromSource();
		}
		bool flag;
		using (file)
		{
			flag = file.Format == CpuData.Format;
			if (flag)
			{
				UploadLevels(BuildCacheUploads(file), file.ReadLevel);
			}
		}
		if (!flag)
		{
			return TryUploadFromSource();
		}
		return base.GpuResource != null;
	}

	private bool TryUploadFromSource()
	{
		if (!RequireCpuData().HasData)
		{
			return false;
		}
		UploadCpuData();
		ReleaseCpuLevels();
		return base.GpuResource != null;
	}

	internal static void EvictWhenCached(Texture2D texture)
	{
		if (!TextureResidency.OnDemand || texture.SourcePath == null)
		{
			return;
		}
		using (PendingEvictionsGate.EnterScope())
		{
			_pendingEvictions.Add((texture, 0));
		}
	}

	public static void DrainPendingEvictions()
	{
		if (_pendingEvictions.Count == 0)
		{
			return;
		}
		List<(Texture2D, int)> pendingEvictions;
		using (PendingEvictionsGate.EnterScope())
		{
			pendingEvictions = _pendingEvictions;
			_pendingEvictions = new List<(Texture2D, int)>();
		}
		int num = 0;
		List<(Texture2D, int)> list = new List<(Texture2D, int)>();
		foreach (var (texture2D, num2) in pendingEvictions)
		{
			if (texture2D.GpuResource == null || texture2D.CpuData == null || texture2D.SourcePath == null)
			{
				continue;
			}
			if (num >= 8192)
			{
				list.Add((texture2D, num2));
			}
			else if (!GpuTextureService.HasValidTextureCache(texture2D.SourcePath, texture2D.CpuData.AlphaIsPremultiplied, texture2D._width, texture2D._height))
			{
				if (num2 + 1 < 600)
				{
					list.Add((texture2D, num2 + 1));
				}
			}
			else
			{
				texture2D.Evict();
				num++;
			}
		}
		if (list.Count == 0)
		{
			return;
		}
		using (PendingEvictionsGate.EnterScope())
		{
			_pendingEvictions.AddRange(list);
		}
	}

	public static void BeginDeferredLoading()
	{
		_deferredLoads = new ConcurrentBag<Texture2D>();
	}

	public static void EndDeferredLoading(Func<Texture2D, bool> stayEvicted)
	{
		DeferredLoadsLock.EnterWriteLock();
		ConcurrentBag<Texture2D> deferredLoads;
		try
		{
			deferredLoads = _deferredLoads;
			_deferredLoads = null;
		}
		finally
		{
			DeferredLoadsLock.ExitWriteLock();
		}
		if (deferredLoads == null || !GpuTextureService.IsConfigured)
		{
			return;
		}
		if (TextureResidency.OnDemand)
		{
			VerboseLog.Log($"Deferred loading: {deferredLoads.Count} textures are metadata only, and will load on first use (texture policy is set to on-demand).");
			return;
		}
		Texture2D[] pending = deferredLoads.Where((Texture2D texture) => texture.GpuResource == null && !stayEvicted(texture)).ToArray();
		ConcurrentQueue<Texture2D> failed = new ConcurrentQueue<Texture2D>();
		int skipped = 0;
		WaylithJobRuntime.For(0, pending.Length, delegate(int i)
		{
			if (TextureResidency.IsPastHighWatermark(0L))
			{
				Interlocked.Increment(ref skipped);
			}
			else if (!pending[i].TryUploadFromCache())
			{
				failed.Enqueue(pending[i]);
			}
		});
		VerboseLog.Log($"Deferred loading: {deferredLoads.Count} textures are metadata only, {pending.Length - skipped} uploaded, {skipped} will load on first use (device-local memory is high).");
		if (failed.IsEmpty)
		{
			return;
		}
		LogCategory log = RenderLog.Log;
		bool shouldAppend;
		LogString<Warning> message = new LogString<Warning>(87, 2, log, out shouldAppend);
		if (shouldAppend)
		{
			message.AppendLiteral("Deferred loading: ");
			message.AppendFormatted(failed.Count);
			message.AppendLiteral(" textures have no pixels, as they failed to load from pack or cache: ");
			message.AppendFormatted(string.Join(", ", from texture in failed.Take(5)
				select texture.SourcePath));
		}
		log.Warning(message, "EndDeferredLoading", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Rendering\\Texture2D.cs", 436);
	}

	internal override bool TryEvictForResidency()
	{
		if (base.GpuResource == null || CpuData == null)
		{
			return false;
		}
		if (IsReadable)
		{
			if (!TextureResidency.OnDemand || CpuData.Dirty)
			{
				return false;
			}
		}
		else if (SourcePath == null || !GpuTextureService.HasValidTextureCache(SourcePath, CpuData.AlphaIsPremultiplied, _width, _height))
		{
			return false;
		}
		Evict();
		return true;
	}

	public void FinishPendingBuild()
	{
		if (PendingBuild != null)
		{
			TextureBuildBatch.Flush();
		}
	}

	public bool TryDeferCompression(BcEncodeTarget target, float quality)
	{
		if (PendingBuild != null)
		{
			return TextureBuildBatch.RequestEncode(this, target, quality);
		}
		return false;
	}

	internal void CompleteBuild(GpuTextureAllocation image)
	{
		base.GpuResource = image;
		CpuData?.TrackCurrentBytes();
		CpuData?.MarkClean();
	}

	internal void CompleteBuild(LoadedTextureFormat format, byte[][] levels, bool keepCpuData)
	{
		CpuData.ReplaceLevels(format, levels, CpuData.Linear);
		base.MipCount = levels.Length;
		UploadCpuData();
		if (!keepCpuData)
		{
			ReleaseCpuData();
		}
	}

	internal void ReleaseCpuData()
	{
		CpuData?.ReleaseTracked();
		CpuData?.Release();
	}

	public bool CompressInPlace(BcEncodeTarget target, float quality, TextureSwizzle swizzle = default(TextureSwizzle))
	{
		if (CpuData == null)
		{
			return false;
		}
		TexturePixelData cpuData = CpuData;
		byte[][] array = ((cpuData != null && cpuData.HasData && cpuData.Format == LoadedTextureFormat.Rgba32) ? TextureBlockEncoder.TryEncodeFromPixels(cpuData.Levels, _width, _height, base.MipCount, target, cpuData.AlphaIsPremultiplied) : null);
		if (array == null && base.GpuResource != null)
		{
			array = TextureBlockEncoder.EncodeFromImage(base.GpuResource, _width, _height, base.MipCount, target, quality);
		}
		if (array == null)
		{
			return false;
		}
		CpuData.ReplaceLevels(GpuTextureService.ToLoadedFormat(target), array, CpuData.Linear);
		CpuData.Swizzle = swizzle;
		base.MipCount = array.Length;
		UploadCpuData();
		return base.GpuResource != null;
	}

	public bool TryAdoptCompressed(in LoadedTextureData cached)
	{
		if (CpuData == null || cached.Width != _width || cached.Height != _height || cached.MipCount <= 0 || cached.Levels.Length < cached.MipCount || !TexturePixelData.IsBlockCompressed(cached.Format))
		{
			return false;
		}
		CpuData.Replace(cached.Width, cached.Height, cached.Format, cached.Levels, cached.MipCount, CpuData.Linear);
		CpuData.Swizzle = cached.Swizzle;
		base.MipCount = cached.MipCount;
		UploadCpuData();
		return base.GpuResource != null;
	}

	public bool TryAdoptCompressed(ITextureCacheSource cached)
	{
		if (CpuData == null || cached.Width != _width || cached.Height != _height || cached.MipCount <= 0 || !TexturePixelData.IsBlockCompressed(cached.Format))
		{
			return false;
		}
		CpuData.ReplaceWithoutLevels(cached.Width, cached.Height, cached.Format, cached.MipCount, CpuData.Linear);
		CpuData.Swizzle = cached.Swizzle;
		base.MipCount = cached.MipCount;
		UploadLevels(BuildCacheUploads(cached), cached.ReadLevel);
		return base.GpuResource != null;
	}

	public void AdoptEncodedImage(GpuTextureAllocation image, LoadedTextureFormat format, int mipCount)
	{
		GpuTextureAllocation? gpuResource = base.GpuResource;
		CpuData?.ReplaceWithoutLevels(_width, _height, format, mipCount, CpuData.Linear);
		base.MipCount = mipCount;
		base.GpuResource = image;
		gpuResource?.Dispose();
	}

	public LoadedTextureData GetLoadedTextureData()
	{
		if (CpuData == null)
		{
			return new LoadedTextureData(_width, _height, 0, LoadedTextureFormat.Rgba32, Array.Empty<byte[]>(), Opaque: false, null);
		}
		return CpuData.ToLoadedTextureData()with
		{
			Opaque = Opaque,
			SourceStamp = SourceStamp
		};
	}

	private static Texture2D? TryLoadDeferred(string filePath, string fullPath, bool premultiplyAlpha)
	{
		DeferredLoadsLock.EnterReadLock();
		try
		{
			ConcurrentBag<Texture2D> deferredLoads = _deferredLoads;
			if ((deferredLoads == null && !TextureResidency.OnDemand) || !GpuTextureService.TryDescribeTexture(fullPath, premultiplyAlpha, out var info))
			{
				return null;
			}
			Texture2D texture2D = NewFileTexture(filePath, fullPath, info.Width, info.Height, info.Format, info.MipCount, info.Opaque, premultiplyAlpha, info.Swizzle);
			deferredLoads?.Add(texture2D);
			return texture2D;
		}
		finally
		{
			DeferredLoadsLock.ExitReadLock();
		}
	}

	public static Texture2D LoadFromFile(string filePath, bool premultiplyAlpha = false)
	{
		string fullPath = Path.GetFullPath(filePath);
		if (_deferredLoads != null || TextureResidency.OnDemand)
		{
			Texture2D texture2D = TryLoadDeferred(filePath, fullPath, premultiplyAlpha);
			if (texture2D != null)
			{
				return texture2D;
			}
		}
		LoadedTextureData data = default(LoadedTextureData);
		bool gpuBuild = false;
		bool cached = false;
		bool flag;
		try
		{
			flag = !premultiplyAlpha && AssetPrefetcher.TryTake(fullPath, out data, out gpuBuild, out cached);
		}
		catch (Exception e)
		{
			return DecodeFailurePlaceholder(filePath, e);
		}
		if (!flag)
		{
			if (GpuTextureService.TryOpenTextureCache(fullPath, premultiplyAlpha, out ITextureCacheSource file))
			{
				using (file)
				{
					Texture2D texture2D2 = NewFileTexture(filePath, fullPath, file.Width, file.Height, file.Format, file.MipCount, file.Opaque, premultiplyAlpha, file.Swizzle);
					texture2D2.UploadLevels(BuildCacheUploads(file), file.ReadLevel);
					return texture2D2;
				}
			}
			try
			{
				data = GpuTextureService.LoadTextureFromFile(fullPath, premultiplyAlpha, allowGpuBuild: true, out gpuBuild, out cached);
			}
			catch (Exception e2)
			{
				return DecodeFailurePlaceholder(filePath, e2);
			}
		}
		Texture2D texture2D3 = NewFileTexture(filePath, fullPath, data.Width, data.Height, data.Format, data.MipCount, data.Opaque, premultiplyAlpha, data.Swizzle);
		texture2D3.SourceStamp = data.SourceStamp;
		if (gpuBuild)
		{
			texture2D3.CpuData.Replace(data.Width, data.Height, data.Format, data.Levels, texture2D3.MipCount, linear: false);
			texture2D3.CpuData.AlphaIsPremultiplied = premultiplyAlpha;
			texture2D3.CpuData.MarkClean();
			TextureBuildBatch.Enqueue(texture2D3, data.Levels[0], premultiplyAlpha);
			return texture2D3;
		}
		texture2D3.ReplaceCpuData(data.Width, data.Height, data.Format, data.Levels, texture2D3.MipCount, linear: false);
		if (cached)
		{
			texture2D3.ReleaseCpuLevels();
			EvictWhenCached(texture2D3);
		}
		return texture2D3;
	}

	private static Texture2D DecodeFailurePlaceholder(string filePath, Exception e)
	{
		LogCategory log = RenderLog.Log;
		bool shouldAppend;
		LogString<Error> message = new LogString<Error>(62, 2, log, out shouldAppend);
		if (shouldAppend)
		{
			message.AppendLiteral("Failed to load texture '");
			message.AppendFormatted(filePath);
			message.AppendLiteral("': ");
			message.AppendFormatted(e.Message);
			message.AppendLiteral(". Substituting magenta placeholder.");
		}
		log.Error(message, "DecodeFailurePlaceholder", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Rendering\\Texture2D.cs", 650);
		return CreatePlaceholder(Path.GetFileNameWithoutExtension(filePath), new Color32(byte.MaxValue, 0, byte.MaxValue, byte.MaxValue));
	}

	private static Texture2D NewFileTexture(string filePath, string fullPath, int width, int height, LoadedTextureFormat format, int mipCount, bool opaque, bool premultiplyAlpha, TextureSwizzle swizzle)
	{
		int mipCount2 = ((mipCount > 1) ? mipCount : TexturePixelData.CalculateMipCount(width, height));
		Texture2D texture2D = new Texture2D(width, height, format, linear: false, mipCount2, createUninitialized: true);
		texture2D.Name = Path.GetFileNameWithoutExtension(filePath);
		texture2D.SourcePath = fullPath;
		texture2D.Opaque = opaque;
		texture2D.TrackerName = GpuTextureService.TextureTrackerName(fullPath);
		texture2D.filterMode = FilterMode.Trilinear;
		texture2D.anisoLevel = 2;
		texture2D.CpuData.AlphaIsPremultiplied = premultiplyAlpha;
		texture2D.CpuData.Swizzle = swizzle;
		return texture2D;
	}

	public static Texture2D? LoadFromTextureCache(string filePath)
	{
		string fullPath = Path.GetFullPath(filePath);
		if (!GpuTextureService.TryLoadPersistedTexture(fullPath, out var loadedTexture))
		{
			return null;
		}
		Texture2D texture2D = new Texture2D(loadedTexture.Width, loadedTexture.Height, loadedTexture.Format, linear: false, loadedTexture.MipCount, createUninitialized: true);
		texture2D.Name = Path.GetFileNameWithoutExtension(filePath);
		texture2D.SourcePath = fullPath;
		texture2D.TrackerName = GpuTextureService.TextureTrackerName(fullPath);
		texture2D.ReplaceCpuData(loadedTexture.Width, loadedTexture.Height, loadedTexture.Format, loadedTexture.Levels, loadedTexture.MipCount, linear: false);
		texture2D.CpuData.Swizzle = loadedTexture.Swizzle;
		texture2D.CpuData.ReleaseTracked();
		texture2D.CpuData.Release();
		return texture2D;
	}

	public void PersistToTextureCache(string filePath)
	{
		TexturePixelData cpuData = CpuData;
		if (cpuData != null && cpuData.HasData)
		{
			SourcePath = Path.GetFullPath(filePath);
			SourceStamp = null;
			TrackerName = GpuTextureService.TextureTrackerName(SourcePath);
			GpuTextureService.WriteTextureCache(SourcePath, this);
		}
	}

	public bool LoadEncodedImage(byte[] data)
	{
		ArgumentNullException.ThrowIfNull(data, "data");
		if (!GpuTextureService.TryLoadTextureFromMemory(data, out var loadedTexture))
		{
			return false;
		}
		Opaque = loadedTexture.Opaque;
		ReplaceCpuData(loadedTexture.Width, loadedTexture.Height, loadedTexture.Format, loadedTexture.Levels, loadedTexture.MipCount, linear: false);
		return true;
	}

	public static Texture2D CreatePlaceholder(string name, Color32 color)
	{
		Texture2D texture2D = new Texture2D(2, 2, LoadedTextureFormat.Rgba32, linear: false, 1, createUninitialized: false)
		{
			Name = name
		};
		texture2D.CpuData.SetPixels32(new Color32[4] { color, color, color, color });
		texture2D.Apply(updateMipmaps: false);
		return texture2D;
	}

	public static Texture2D CreateSolid(string name, Color32 color)
	{
		Texture2D texture2D = new Texture2D(1, 1, LoadedTextureFormat.Rgba32, linear: false, 1, createUninitialized: false)
		{
			Name = name
		};
		texture2D.CpuData.SetPixels32(new Color32[1] { color });
		texture2D.Apply(updateMipmaps: false, makeNoLongerReadable: true);
		return texture2D;
	}

	public static Texture2D CreateRenderTarget(int width, int height, string? name = null)
	{
		Texture2D texture2D = CreateUnbacked(width, height);
		texture2D.filterMode = FilterMode.Bilinear;
		if (!string.IsNullOrEmpty(name))
		{
			texture2D.Name = name;
		}
		texture2D.TryEnsureGpuAllocated();
		return texture2D;
	}

	public override void Dispose()
	{
		if (PendingBuild != null)
		{
			TextureBuildBatch.Cancel(this);
		}
		ReleaseCpuData();
		base.Dispose();
		GC.SuppressFinalize(this);
	}
}
