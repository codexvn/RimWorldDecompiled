namespace UnityEngine;

public class GUIContent
{
	public static GUIContent none = new GUIContent("");

	private static readonly GUIContent s_Text = new GUIContent();

	public string tooltip { get; set; } = "";

	public string text { get; set; } = "";

	public Texture image { get; set; }

	public GUIContent(string text, Texture image)
	{
		this.text = text;
		this.image = image;
	}

	public GUIContent(string text)
	{
		this.text = text;
	}

	public GUIContent(string text, string tooltip)
	{
		this.text = text;
		this.tooltip = tooltip;
	}

	public GUIContent(string text, Texture image, string tooltip)
	{
		this.text = text;
		this.image = image;
		this.tooltip = tooltip;
	}

	public GUIContent(Texture image)
	{
		this.image = image;
	}

	public GUIContent(Texture image, string tooltip)
	{
		this.image = image;
		this.tooltip = tooltip;
	}

	public GUIContent(GUIContent src)
	{
		text = src.text;
		image = src.image;
		tooltip = src.tooltip;
	}

	public GUIContent()
	{
	}

	internal static GUIContent Temp(string t)
	{
		s_Text.text = t;
		s_Text.tooltip = "";
		return s_Text;
	}

	internal static GUIContent Temp(string t, string tooltip)
	{
		s_Text.text = t;
		s_Text.tooltip = tooltip;
		return s_Text;
	}
}
