namespace UnityEngine.UI;

public class Text : MaskableGraphic
{
	public string text { get; set; } = "";

	public Font font { get; set; }

	public int fontSize { get; set; } = 14;

	public FontStyle fontStyle { get; set; }

	public TextAnchor alignment { get; set; }

	public bool supportRichText { get; set; } = true;

	public float lineSpacing { get; set; } = 1f;
}
