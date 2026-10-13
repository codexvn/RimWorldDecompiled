using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore;
using UnityEngine.TextCore.LowLevel;
using UnityStubs.Gui;
using UnityStubs.Internal;

namespace TMPro;

public class TMP_FontAsset : TMP_Asset
{
	public int fontIndex;

	public static readonly TMP_FontAsset Default = new TMP_FontAsset(2);

	public Font sourceFontFile { get; set; }

	public FaceInfo faceInfo { get; set; }

	public List<Glyph> glyphTable { get; set; } = new List<Glyph>();

	public List<TMP_Character> characterTable { get; set; } = new List<TMP_Character>();

	public Texture2D atlasTexture => TextMeshProBackend.Current?.GetAtlasTexture(fontIndex);

	public Texture2D[] atlasTextures
	{
		get
		{
			Texture2D texture2D = atlasTexture;
			if ((object)texture2D == null)
			{
				return Array.Empty<Texture2D>();
			}
			return new Texture2D[1] { texture2D };
		}
		set
		{
		}
	}

	public int atlasTextureCount => 1;

	public int atlasWidth => atlasTexture?.width ?? 0;

	public int atlasHeight => atlasTexture?.height ?? 0;

	public List<TMP_FontAsset> fallbackFontAssetTable { get; set; } = new List<TMP_FontAsset>();

	public static TMP_FontAsset CreateFontAsset(Font font)
	{
		StubWarner.LogStub("CreateFontAsset", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\RimWorldPort\\TMPro.cs", 56);
		return new TMP_FontAsset(font?.GameFontIndex ?? 2)
		{
			sourceFontFile = font,
			name = font?.name
		};
	}

	public static TMP_FontAsset CreateFontAsset(Font font, int samplingPointSize, int atlasPadding, GlyphRenderMode renderMode, int atlasWidth, int atlasHeight, AtlasPopulationMode atlasPopulationMode = AtlasPopulationMode.Dynamic, bool enableMultiAtlasSupport = true)
	{
		return CreateFontAsset(font);
	}

	public TMP_FontAsset()
	{
	}

	public TMP_FontAsset(int fontIndex)
	{
		this.fontIndex = fontIndex;
	}

	public static string GetCharacters(TMP_FontAsset font)
	{
		return TextMeshProBackend.Current?.GetCharacters(font?.fontIndex ?? 0) ?? string.Empty;
	}
}
