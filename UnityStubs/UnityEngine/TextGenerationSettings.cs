namespace UnityEngine;

public struct TextGenerationSettings
{
	public Font font;

	public Color color;

	public int fontSize;

	public float lineSpacing;

	public bool richText;

	public float scaleFactor;

	public FontStyle fontStyle;

	public TextAnchor textAnchor;

	public bool alignByGeometry;

	public bool resizeTextForBestFit;

	public int resizeTextMinSize;

	public int resizeTextMaxSize;

	public bool updateBounds;

	public VerticalWrapMode verticalOverflow;

	public HorizontalWrapMode horizontalOverflow;

	public Vector2 generationExtents;

	public Vector2 pivot;

	public bool generateOutOfBounds;
}
