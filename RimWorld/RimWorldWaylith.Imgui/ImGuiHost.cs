using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using Brutal.ImGuiApi;
using Brutal.Numerics;
using Brutal.Pointers;
using Brutal.VulkanApi;
using UnityEngine;
using UnityStubs.Gui;
using Waylith;
using Waylith.Assets;
using Waylith.Graphics;
using Waylith.Rendering;

namespace RimWorldWaylith.Imgui;

internal sealed class ImGuiHost : IDisposable
{
	private readonly record struct FontSource(string? Path, byte[]? Data, float Size, uint LoaderFlags);

	private static class FreeTypeNative
	{
		[DllImport("imgui_freetype")]
		public unsafe static extern ImFontLoader* GetFontLoader();
	}

	private const int GameFontCount = 3;

	private static readonly float[] GameFontPixelSizes = new float[3] { 12f, 14f, 21f };

	private static readonly string[] FontFiles = new string[3] { "Calibri_tiny.TTF", "Arial_small.ttf", "Arial_medium.ttf" };

	private readonly List<ImFontPtr> fonts = new List<ImFontPtr>();

	private readonly List<FaceMetrics> faceMetrics = new List<FaceMetrics>();

	private readonly List<float> importSizes = new List<float>();

	private readonly Dictionary<(string, int), int> registeredFonts = new Dictionary<(string, int), int>();

	private readonly Dictionary<string, IReadOnlyList<FontFallbacks.Source>> fallbacksByLocale = new Dictionary<string, IReadOnlyList<FontFallbacks.Source>>();

	private readonly List<FontSource> fontSources = new List<FontSource>();

	private readonly Dictionary<(int, FontStyle, string), ImFontPtr> styledFonts = new Dictionary<(int, FontStyle, string), ImFontPtr>();

	private readonly Dictionary<(int, string), ImFontPtr> localeFonts = new Dictionary<(int, string), ImFontPtr>();

	private string locale = "";

	private readonly IRenderContext renderContext;

	private readonly ImGuiContextPtr context;

	private readonly ImGuiVulkanRenderer renderer;

	private readonly ShaderModuleCache shaderModules;

	private ImGuiGuiBackend? guiBackend;

	private readonly ImGuiDrawSnapshot[] snapshots;

	private bool disposed;

	private int fontGeneration;

	private const int Smooth = 0;

	private const int HintedSmooth = 1;

	private const int HintedRaster = 2;

	private bool freeTypeSizesResettable;

	private const int FreeTypeBakedLastActivatedOffset = 16;

	private const uint FreeTypeNoHinting = 1u;

	private const uint FreeTypeMonoHinting = 16u;

	private const uint FreeTypeBold = 32u;

	private const uint FreeTypeOblique = 64u;

	private const uint FreeTypeMonochrome = 128u;

	private const uint FreeTypeNominalSize = 512u;

	private static readonly uint[] MissingGlyphChars = new uint[2] { 9633u, 65533u };

	private static readonly Dictionary<string, (nint Data, int Length)> fallbackFileData = new Dictionary<string, (nint, int)>();

	public IReadOnlyList<ImFontPtr> Fonts => fonts;

	public int FontGeneration => Volatile.Read(in fontGeneration);

	public ITextMeshProBackend TmpFontBackend { get; }

	internal ImGuiVulkanRenderer Renderer => renderer;

	internal StandaloneDrawList RenderTextureDrawList { get; } = new StandaloneDrawList();

	public float DisplayHeight => renderContext.GetExtent().Height;

	public float ImportSize(int fontIndex)
	{
		return importSizes[fontIndex];
	}

	public float PixelSizeFor(int fontIndex, float fontSize)
	{
		if (!(fontSize > 0f))
		{
			return importSizes[fontIndex];
		}
		return fontSize;
	}

	public LineMetrics LineMetricsFor(int fontIndex, float fontSize)
	{
		return faceMetrics[fontIndex].At(PixelSizeFor(fontIndex, fontSize));
	}

	public void SetTextLanguage(string? languageFolderName)
	{
		string text = FontFallbacks.LocaleFor(languageFolderName) ?? "";
		if (!(text == locale))
		{
			locale = text;
			Volatile.Write(ref fontGeneration, fontGeneration + 1);
		}
	}

	public ImFontPtr Font(int fontIndex)
	{
		if (locale.Length == 0)
		{
			return fonts[fontIndex];
		}
		if (localeFonts.TryGetValue((fontIndex, locale), out var value))
		{
			return value;
		}
		value = AddFontAgain(fontSources[fontIndex], fontSources[fontIndex].LoaderFlags, 0u);
		if (value.IsNull())
		{
			value = fonts[fontIndex];
		}
		localeFonts[(fontIndex, locale)] = value;
		return value;
	}

	private IReadOnlyList<FontFallbacks.Source> Fallbacks()
	{
		if (!fallbacksByLocale.TryGetValue(locale, out IReadOnlyList<FontFallbacks.Source> value))
		{
			value = (fallbacksByLocale[locale] = FontFallbacks.Resolve((locale.Length == 0) ? null : locale));
		}
		return value;
	}

	private unsafe ImFontPtr AddFontAgain(FontSource src, uint loaderFlags, uint styleFlags)
	{
		ImGuiIOPtr iO = ImGui.GetIO();
		ImFontConfig imFontConfig = NewFontConfig(loaderFlags);
		ImFontPtr imFontPtr = default(ImFontPtr);
		if (src.Path != null)
		{
			imFontPtr = iO.Fonts.AddFontFromFileTTF(src.Path, src.Size, new ImFontConfigPtr(&imFontConfig));
		}
		else if (src.Data != null)
		{
			void* ptr = ImGui.MemAlloc((nuint)src.Data.Length);
			src.Data.CopyTo(new Span<byte>(ptr, src.Data.Length));
			imFontPtr = iO.Fonts.AddFontFromMemoryTTF(ptr, src.Data.Length, src.Size, new ImFontConfigPtr(&imFontConfig));
		}
		if (!imFontPtr.IsNull())
		{
			AddFallbacks(iO, imFontPtr, src.Size, Fallbacks(), styleFlags);
		}
		return imFontPtr;
	}

	internal void SetGuiBackend(ImGuiGuiBackend backend)
	{
		guiBackend = backend;
	}

	public ImGuiHost(EngineApplication app, int snapshotRingLength)
	{
		renderContext = app.RenderContext ?? throw new InvalidOperationException("ImGuiHost requires a render context (non-headless).");
		context = ImGui.CreateContext();
		ImGui.SetCurrentContext(context);
		ImGuiIOPtr iO = ImGui.GetIO();
		iO.IniFilename = null;
		iO.LogFilename = null;
		LoadFonts(iO);
		TmpFontBackend = new ImGuiTextMeshProBackend(Fonts);
		shaderModules = new ShaderModuleCache(app.Device);
		renderer = new ImGuiVulkanRenderer(app.GraphicsDevice, renderContext.GetRenderPass(), 0, renderContext.SampleCount, renderContext.ImageCount, renderContext.FramesInFlight, 1024, app.GraphicsDevice.ShaderLayoutCache, shaderModules);
		int num = Math.Max(2, snapshotRingLength);
		snapshots = new ImGuiDrawSnapshot[num];
		for (int i = 0; i < num; i++)
		{
			snapshots[i] = new ImGuiDrawSnapshot();
		}
		ShaderHotReloader.ShadersReloaded += OnShadersReloaded;
	}

	private void OnShadersReloaded()
	{
		shaderModules.Clear();
		renderer.OnShadersReloaded();
	}

	private void LoadFonts(ImGuiIOPtr io)
	{
		freeTypeSizesResettable = TryEnableFreeTypeRasterizer(io);
		if (freeTypeSizesResettable && !ImGui.GetVersion().ToString().StartsWith("1.92.", StringComparison.Ordinal))
		{
			Console.Error.WriteLine($"[ImGuiHost] imgui {(string)ImGui.GetVersion()} may lay out imgui_freetype's source data differently, not resetting its active sizes");
			freeTypeSizesResettable = false;
		}
		string path = Path.Combine(Application.dataPath, "Resources", "Fonts");
		IReadOnlyList<FontFallbacks.Source> fallbacks = Fallbacks();
		for (int i = 0; i < 3; i++)
		{
			string text = Path.Combine(path, FontFiles[i]);
			uint loaderFlags = LoaderFlagsFor((i == 0) ? 2 : 0);
			byte[] data;
			bool flag = ContentPack.TryReadFile(text, out data);
			ImFontPtr imFontPtr = (flag ? AddFont(io, data, GameFontPixelSizes[i], fallbacks, loaderFlags) : AddFont(io, text, GameFontPixelSizes[i], fallbacks, loaderFlags));
			fonts.Add(imFontPtr);
			fontSources.Add(flag ? new FontSource(null, data, GameFontPixelSizes[i], loaderFlags) : new FontSource(File.Exists(text) ? text : null, null, GameFontPixelSizes[i], loaderFlags));
			faceMetrics.Add(flag ? FaceMetrics.Load(data, imFontPtr, GameFontPixelSizes[i]) : FaceMetrics.Load(text, imFontPtr, GameFontPixelSizes[i]));
			importSizes.Add(GameFontPixelSizes[i]);
		}
	}

	public ImFontPtr StyledFont(int fontIndex, FontStyle style)
	{
		if (style == FontStyle.Normal)
		{
			return Font(fontIndex);
		}
		if (styledFonts.TryGetValue((fontIndex, style, locale), out var value))
		{
			return value;
		}
		FontSource src = fontSources[fontIndex];
		uint num = src.LoaderFlags;
		if ((style & FontStyle.Bold) != 0)
		{
			num |= 0x20;
		}
		if ((style & FontStyle.Italic) != 0)
		{
			num |= 0x40;
		}
		value = AddFontAgain(src, num, num & 0x60);
		if (value.IsNull())
		{
			value = Font(fontIndex);
		}
		styledFonts[(fontIndex, style, locale)] = value;
		return value;
	}

	private static uint LoaderFlagsFor(int renderingMode)
	{
		return renderingMode switch
		{
			1 => 512u, 
			2 => 656u, 
			_ => 513u, 
		};
	}

	public unsafe int RegisterFont(string name, byte[] fontData, float importSize, int renderingMode)
	{
		(string, int) key = (name, fontData.Length);
		if (registeredFonts.TryGetValue(key, out var value))
		{
			return value;
		}
		if (!FaceMetrics.TryRead(fontData, out var metrics))
		{
			return -1;
		}
		float num = ((importSize > 0f) ? importSize : 16f);
		void* ptr = ImGui.MemAlloc((nuint)fontData.Length);
		fontData.CopyTo(new Span<byte>(ptr, fontData.Length));
		uint loaderFlags = LoaderFlagsFor(renderingMode);
		ImFontConfig imFontConfig = NewFontConfig(loaderFlags);
		ImGuiIOPtr iO = ImGui.GetIO();
		ImFontPtr imFontPtr = iO.Fonts.AddFontFromMemoryTTF(ptr, fontData.Length, num, new ImFontConfigPtr(&imFontConfig));
		if (imFontPtr.IsNull())
		{
			return -1;
		}
		AddFallbacks(iO, imFontPtr, num, Fallbacks(), 0u);
		value = fonts.Count;
		fonts.Add(imFontPtr);
		fontSources.Add(new FontSource(null, fontData, num, loaderFlags));
		faceMetrics.Add(metrics);
		importSizes.Add(num);
		registeredFonts[key] = value;
		return value;
	}

	private unsafe static bool TryEnableFreeTypeRasterizer(ImGuiIOPtr io)
	{
		try
		{
			ImFontLoader* fontLoader = FreeTypeNative.GetFontLoader();
			if (fontLoader == null)
			{
				return false;
			}
			io.Fonts.SetFontLoader(fontLoader);
			return true;
		}
		catch (Exception ex)
		{
			Console.Error.WriteLine("[ImGuiHost] FreeType rasterizer unavailable, falling back to stb_truetype: " + ex.Message);
			return false;
		}
	}

	private unsafe void ForgetFreeTypeActivatedSizes()
	{
		if (!freeTypeSizesResettable)
		{
			return;
		}
		Span<ImFontConfig> span = ImGui.GetIO().Fonts.Sources.Span;
		for (int i = 0; i < span.Length; i++)
		{
			ImFontConfig imFontConfig = span[i];
			if (imFontConfig.FontLoaderData != 0)
			{
				*(IntPtr*)(imFontConfig.FontLoaderData + 16) = (IntPtr)0;
			}
		}
	}

	private unsafe static ImFontPtr AddFont(ImGuiIOPtr io, string path, float sizePixels, IReadOnlyList<FontFallbacks.Source> fallbacks, uint loaderFlags)
	{
		if (!File.Exists(path))
		{
			Console.Error.WriteLine("[ImGuiHost] Font not found, using default: " + path);
			return io.Fonts.AddFontDefault();
		}
		ImFontConfig imFontConfig = NewFontConfig(loaderFlags);
		ImFontPtr imFontPtr = io.Fonts.AddFontFromFileTTF(path, sizePixels, new ImFontConfigPtr(&imFontConfig));
		AddFallbacks(io, imFontPtr, sizePixels, fallbacks, 0u);
		return imFontPtr;
	}

	private unsafe static ImFontPtr AddFont(ImGuiIOPtr io, byte[] fontData, float sizePixels, IReadOnlyList<FontFallbacks.Source> fallbacks, uint loaderFlags)
	{
		ImFontConfig imFontConfig = NewFontConfig(loaderFlags);
		void* ptr = ImGui.MemAlloc((nuint)fontData.Length);
		fontData.CopyTo(new Span<byte>(ptr, fontData.Length));
		ImFontPtr imFontPtr = io.Fonts.AddFontFromMemoryTTF(ptr, fontData.Length, sizePixels, new ImFontConfigPtr(&imFontConfig));
		AddFallbacks(io, imFontPtr, sizePixels, fallbacks, 0u);
		return imFontPtr;
	}

	private unsafe static void AddFallbacks(ImGuiIOPtr io, ImFontPtr font, float sizePixels, IReadOnlyList<FontFallbacks.Source> fallbacks, uint styleFlags)
	{
		foreach (FontFallbacks.Source fallback in fallbacks)
		{
			ImFontConfig imFontConfig = NewFontConfig(0x200 | styleFlags);
			imFontConfig.MergeMode = true;
			imFontConfig.FontNo = fallback.FontNo;
			imFontConfig.FontDataOwnedByAtlas = false;
			if (fallback.ExcludeRanges != null)
			{
				imFontConfig.GlyphExcludeRanges = new Ptr<uint>((uint*)Marshal.UnsafeAddrOfPinnedArrayElement(fallback.ExcludeRanges, 0));
			}
			var (num, fontDataSize) = FallbackFileData(fallback.Path);
			if (num != 0)
			{
				io.Fonts.AddFontFromMemoryTTF((void*)num, fontDataSize, sizePixels, new ImFontConfigPtr(&imFontConfig));
			}
		}
		uint[] missingGlyphChars = MissingGlyphChars;
		foreach (uint num2 in missingGlyphChars)
		{
			if (font.IsGlyphInFont(num2))
			{
				font.FallbackChar = num2;
				break;
			}
		}
	}

	private unsafe static (nint Data, int Length) FallbackFileData(string path)
	{
		if (fallbackFileData.TryGetValue(path, out (nint, int) value))
		{
			return value;
		}
		try
		{
			byte[] array = File.ReadAllBytes(path);
			void* ptr = ImGui.MemAlloc((nuint)array.Length);
			array.CopyTo(new Span<byte>(ptr, array.Length));
			value = ((nint)ptr, array.Length);
		}
		catch (Exception ex)
		{
			Console.Error.WriteLine($"[ImGuiHost] Fallback font unreadable, skipping: {path} ({ex.Message})");
			value = (0, 0);
		}
		fallbackFileData[path] = value;
		return value;
	}

	private static ImFontConfig NewFontConfig(uint loaderFlags)
	{
		ImFontConfig result = new ImFontConfig();
		result.FontDataOwnedByAtlas = true;
		result.OversampleH = 0;
		result.OversampleV = 0;
		result.GlyphMaxAdvanceX = float.MaxValue;
		result.RasterizerMultiply = 1f;
		result.RasterizerDensity = 1f;
		result.EllipsisChar = 0u;
		result.FontLoaderFlags = loaderFlags;
		result.PixelSnapH = true;
		return result;
	}

	public ImGuiDrawSnapshot BeginFrame(int frameNumber, float deltaTime)
	{
		ImGui.SetCurrentContext(context);
		VkExtent2D extent = renderContext.GetExtent();
		ImGuiIOPtr iO = ImGui.GetIO();
		iO.DisplaySize = new float2(extent.Width, extent.Height);
		iO.DeltaTime = ((deltaTime > 0f) ? deltaTime : (1f / 60f));
		ImGui.NewFrame();
		ForgetFreeTypeActivatedSizes();
		ImString.SharedStorage.Reset();
		renderer.BeginFrame(frameNumber);
		int num = (frameNumber % snapshots.Length + snapshots.Length) % snapshots.Length;
		snapshots[num].FrameNumber = frameNumber;
		return snapshots[num];
	}

	public void EndFrame(ImGuiDrawSnapshot snapshot)
	{
		ImGui.Render();
		ImDrawDataPtr drawData = ImGui.GetDrawData();
		renderer.ProcessTextureUpdates(drawData);
		(TmpFontBackend as ImGuiTextMeshProBackend)?.TryCaptureAtlas(ImGui.GetIO().Fonts);
		snapshot.CopyFrom(drawData, guiBackend?.MaterialDraws ?? Array.Empty<ImGuiMaterialDraw>());
	}

	public void Render(ImGuiDrawSnapshot snapshot, in FrameContext ctx)
	{
		renderer.Render(snapshot, in ctx);
	}

	public void Dispose()
	{
		if (!disposed)
		{
			disposed = true;
			ShaderHotReloader.ShadersReloaded -= OnShadersReloaded;
			renderer.Dispose();
			shaderModules.Dispose();
			RenderTextureDrawList.Dispose();
			ImGui.DestroyContext(context);
		}
	}
}
