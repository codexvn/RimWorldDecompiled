namespace UnityEngine.TextCore;

public struct GlyphMetrics
{
	public float width { get; set; }

	public float height { get; set; }

	public float horizontalBearingX { get; set; }

	public float horizontalBearingY { get; set; }

	public float horizontalAdvance { get; set; }

	public GlyphMetrics(float width, float height, float bearingX, float bearingY, float advance)
	{
		this.width = width;
		this.height = height;
		horizontalBearingX = bearingX;
		horizontalBearingY = bearingY;
		horizontalAdvance = advance;
	}
}
