using System;
using System.Collections.Generic;
using System.Text;
using Brutal.ImGuiApi;
using UnityEngine;
using UnityStubs.Gui;
using Waylith.Graphics;

namespace RimWorldWaylith.Imgui;

internal sealed class ImGuiTextMeshProBackend : ITextMeshProBackend
{
	private sealed class FontData
	{
		public TmpFontMetrics metrics;

		public readonly Dictionary<char, TmpGlyphMetrics> glyphs = new Dictionary<char, TmpGlyphMetrics>();

		public string characters = string.Empty;

		public char sentinel;
	}

	private const float BakeSize = 48f;

	private readonly IReadOnlyList<ImFontPtr> imFonts;

	private readonly List<FontData> fonts = new List<FontData>();

	private byte[]? atlasRgba;

	private int atlasWidth;

	private int atlasHeight;

	private nint atlasPixels;

	private Texture2D? atlasTexture;

	private int atlasVersion;

	public float BakePixelSize => 48f;

	public int AtlasVersion => atlasVersion;

	public ImGuiTextMeshProBackend(IReadOnlyList<ImFontPtr> imFonts)
	{
		this.imFonts = imFonts;
		BakeNewFonts();
	}

	private bool BakeNewFonts()
	{
		bool result = fonts.Count < imFonts.Count;
		for (int i = fonts.Count; i < imFonts.Count; i++)
		{
			fonts.Add(Bake(imFonts[i]));
		}
		return result;
	}

	private static FontData Bake(ImFontPtr font)
	{
		FontData fontData = new FontData();
		if (font.IsNull())
		{
			return fontData;
		}
		ImFontBakedPtr fontBaked = font.GetFontBaked(48f);
		if (fontBaked.IsNull())
		{
			return fontData;
		}
		float ascent = fontBaked.Ascent;
		float descent = fontBaked.Descent;
		fontData.metrics = new TmpFontMetrics
		{
			found = true,
			bakedPixelSize = 48f,
			ascent = ascent,
			descent = descent,
			lineHeight = ascent - descent
		};
		StringBuilder stringBuilder = new StringBuilder();
		BakeRange(fontBaked, fontData, stringBuilder, 32, 126);
		BakeRange(fontBaked, fontData, stringBuilder, 160, 255);
		fontData.characters = stringBuilder.ToString();
		return fontData;
	}

	private static void BakeRange(ImFontBakedPtr baked, FontData data, StringBuilder sb, int lo, int hi)
	{
		for (int i = lo; i <= hi; i++)
		{
			ImFontGlyphPtr imFontGlyphPtr = baked.FindGlyphNoFallback((uint)i);
			if (!imFontGlyphPtr.IsNull())
			{
				float x = imFontGlyphPtr.X0;
				float y = imFontGlyphPtr.Y0;
				float x2 = imFontGlyphPtr.X1;
				float y2 = imFontGlyphPtr.Y1;
				char c = (char)i;
				data.glyphs[c] = new TmpGlyphMetrics
				{
					found = true,
					visible = (x2 > x && y2 > y),
					advanceX = imFontGlyphPtr.AdvanceX,
					x0 = x,
					y0 = y,
					x1 = x2,
					y1 = y2,
					u0 = imFontGlyphPtr.U0,
					v0 = imFontGlyphPtr.V0,
					u1 = imFontGlyphPtr.U1,
					v1 = imFontGlyphPtr.V1
				};
				sb.Append(c);
				if (data.sentinel == '\0' && x2 > x && y2 > y)
				{
					data.sentinel = c;
				}
			}
		}
	}

	private FontData? Get(int fontIndex)
	{
		if (fontIndex < 0 || fontIndex >= fonts.Count)
		{
			return null;
		}
		return fonts[fontIndex];
	}

	public bool TryGetFontMetrics(int fontIndex, out TmpFontMetrics metrics)
	{
		FontData fontData = Get(fontIndex);
		if (fontData == null)
		{
			metrics = default(TmpFontMetrics);
			return false;
		}
		metrics = fontData.metrics;
		return metrics.found;
	}

	public bool TryGetGlyph(int fontIndex, char c, out TmpGlyphMetrics glyph)
	{
		FontData fontData = Get(fontIndex);
		if (fontData != null && fontData.glyphs.TryGetValue(c, out glyph))
		{
			return true;
		}
		glyph = default(TmpGlyphMetrics);
		return false;
	}

	public bool TryGetGlyphAtSize(int fontIndex, char c, float pixelSize, out TmpGlyphMetrics glyph, out float ascent)
	{
		glyph = default(TmpGlyphMetrics);
		ascent = 0f;
		if (fontIndex < 0 || fontIndex >= imFonts.Count || imFonts[fontIndex].IsNull() || pixelSize <= 0f)
		{
			return false;
		}
		ImFontBakedPtr fontBaked = imFonts[fontIndex].GetFontBaked(pixelSize);
		if (fontBaked.IsNull())
		{
			return false;
		}
		ImFontGlyphPtr imFontGlyphPtr = fontBaked.FindGlyphNoFallback(c);
		if (imFontGlyphPtr.IsNull())
		{
			return false;
		}
		ascent = fontBaked.Ascent;
		glyph = new TmpGlyphMetrics
		{
			found = true,
			visible = (imFontGlyphPtr.X1 > imFontGlyphPtr.X0 && imFontGlyphPtr.Y1 > imFontGlyphPtr.Y0),
			advanceX = imFontGlyphPtr.AdvanceX,
			x0 = imFontGlyphPtr.X0,
			y0 = imFontGlyphPtr.Y0,
			x1 = imFontGlyphPtr.X1,
			y1 = imFontGlyphPtr.Y1,
			u0 = imFontGlyphPtr.U0,
			v0 = imFontGlyphPtr.V0,
			u1 = imFontGlyphPtr.U1,
			v1 = imFontGlyphPtr.V1
		};
		return true;
	}

	public string GetCharacters(int fontIndex)
	{
		return Get(fontIndex)?.characters ?? string.Empty;
	}

	public unsafe void TryCaptureAtlas(ImFontAtlasPtr atlas)
	{
		if (atlas.IsNull())
		{
			return;
		}
		ImTextureDataPtr target = atlas.TexData;
		if (target.IsNull() || target.Width <= 0 || target.Height <= 0 || target.BytesPerPixel != 4)
		{
			return;
		}
		bool flag = TouchBakes();
		byte* ptr = (byte*)(void*)target.GetPixelsAt(0, 0);
		if (ptr == null || (!flag && atlasRgba != null && ptr == (byte*)atlasPixels && target.Width == atlasWidth && target.Height == atlasHeight))
		{
			return;
		}
		for (int i = 0; i < 4; i++)
		{
			BakeNewFonts();
			for (int j = 0; j < fonts.Count; j++)
			{
				fonts[j] = Bake(imFonts[j]);
			}
			ImTextureDataPtr texData = atlas.TexData;
			byte* ptr2 = (byte*)(texData.IsNull() ? null : ((void*)texData.GetPixelsAt(0, 0)));
			if (ptr2 == null)
			{
				return;
			}
			if (ptr2 == ptr && texData.Width == target.Width && texData.Height == target.Height)
			{
				break;
			}
			target = texData;
			ptr = ptr2;
		}
		atlasVersion++;
		int num = target.Width * target.Height * 4;
		byte[] array = new byte[num];
		fixed (byte* destination = array)
		{
			Buffer.MemoryCopy(ptr, destination, num, num);
		}
		atlasWidth = target.Width;
		atlasHeight = target.Height;
		atlasPixels = (nint)ptr;
		atlasRgba = array;
		if (atlasTexture != null)
		{
			UploadAtlas(atlasTexture);
		}
	}

	private bool TouchBakes()
	{
		bool result = BakeNewFonts();
		for (int i = 0; i < fonts.Count; i++)
		{
			FontData fontData = fonts[i];
			if (!imFonts[i].IsNull() && fontData.sentinel != 0)
			{
				ImFontBakedPtr fontBaked = imFonts[i].GetFontBaked(48f);
				ImFontGlyphPtr imFontGlyphPtr = (fontBaked.IsNull() ? default(ImFontGlyphPtr) : fontBaked.FindGlyphNoFallback(fontData.sentinel));
				TmpGlyphMetrics tmpGlyphMetrics = fontData.glyphs[fontData.sentinel];
				if (imFontGlyphPtr.IsNull() || imFontGlyphPtr.U0 != tmpGlyphMetrics.u0 || imFontGlyphPtr.V0 != tmpGlyphMetrics.v0 || imFontGlyphPtr.U1 != tmpGlyphMetrics.u1 || imFontGlyphPtr.V1 != tmpGlyphMetrics.v1)
				{
					result = true;
				}
			}
		}
		return result;
	}

	private void UploadAtlas(Texture2D tex)
	{
		Color32[] array = new Color32[atlasWidth * atlasHeight];
		for (int i = 0; i < atlasHeight; i++)
		{
			int num = i * atlasWidth * 4;
			int num2 = (atlasHeight - 1 - i) * atlasWidth;
			int num3 = 0;
			while (num3 < atlasWidth)
			{
				array[num2 + num3] = new Color32(atlasRgba[num], atlasRgba[num + 1], atlasRgba[num + 2], atlasRgba[num + 3]);
				num3++;
				num += 4;
			}
		}
		if (tex.width != atlasWidth || tex.height != atlasHeight)
		{
			tex.Reinitialize(atlasWidth, atlasHeight);
		}
		tex.SetPixels32(array);
		tex.Apply(updateMipmaps: false, makeNoLongerReadable: false);
	}

	public Texture2D GetAtlasTexture(int fontIndex)
	{
		if (atlasTexture != null)
		{
			return atlasTexture;
		}
		if (atlasRgba == null)
		{
			return null;
		}
		Texture2D texture2D = new Texture2D(atlasWidth, atlasHeight, TextureFormat.RGBA32, mipChain: false)
		{
			name = "ImGuiFontAtlas"
		};
		texture2D.CoreTexture.MemoryCategory = GpuMemoryCategory.Font;
		UploadAtlas(texture2D);
		atlasTexture = texture2D;
		return atlasTexture;
	}
}
