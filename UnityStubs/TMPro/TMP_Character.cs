using UnityEngine.TextCore;

namespace TMPro;

public class TMP_Character : TMP_TextElement
{
	public TMP_Character()
	{
		m_ElementType = TextElementType.Character;
		m_Scale = 1f;
	}

	public TMP_Character(uint unicode, Glyph glyph)
		: this()
	{
		m_Unicode = unicode;
		m_TextAsset = null;
		m_Glyph = glyph;
		m_GlyphIndex = glyph?.index ?? 0;
	}

	public TMP_Character(uint unicode, TMP_FontAsset fontAsset, Glyph glyph)
		: this(unicode, glyph)
	{
		m_TextAsset = fontAsset;
	}

	internal TMP_Character(uint unicode, uint glyphIndex)
		: this()
	{
		m_Unicode = unicode;
		m_GlyphIndex = glyphIndex;
	}
}
