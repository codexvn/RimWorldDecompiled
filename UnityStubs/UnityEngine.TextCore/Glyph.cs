namespace UnityEngine.TextCore;

public class Glyph
{
	public uint index { get; set; }

	public GlyphMetrics metrics { get; set; }

	public GlyphRect glyphRect { get; set; }

	public float scale { get; set; } = 1f;

	public int atlasIndex { get; set; }

	public Glyph()
	{
	}

	public Glyph(uint index, GlyphMetrics metrics, GlyphRect glyphRect)
	{
		this.index = index;
		this.metrics = metrics;
		this.glyphRect = glyphRect;
	}
}
