using UnityEngine;

namespace UnityStubs.Gui;

public interface ITextMeshProBackend
{
	float BakePixelSize { get; }

	int AtlasVersion { get; }

	bool TryGetFontMetrics(int fontIndex, out TmpFontMetrics metrics);

	bool TryGetGlyph(int fontIndex, char c, out TmpGlyphMetrics glyph);

	bool TryGetGlyphAtSize(int fontIndex, char c, float pixelSize, out TmpGlyphMetrics glyph, out float ascent);

	string GetCharacters(int fontIndex);

	Texture2D GetAtlasTexture(int fontIndex);
}
