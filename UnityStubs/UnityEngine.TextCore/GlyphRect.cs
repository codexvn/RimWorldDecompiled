namespace UnityEngine.TextCore;

public struct GlyphRect
{
	public int x { get; set; }

	public int y { get; set; }

	public int width { get; set; }

	public int height { get; set; }

	public static GlyphRect zero => default(GlyphRect);

	public GlyphRect(int x, int y, int width, int height)
	{
		this.x = x;
		this.y = y;
		this.width = width;
		this.height = height;
	}
}
