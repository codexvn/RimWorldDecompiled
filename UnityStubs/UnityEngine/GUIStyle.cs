using System;
using System.Runtime.CompilerServices;
using UnityStubs.Gui;

namespace UnityEngine;

public class GUIStyle
{
	private static readonly float[] FallbackFontPixelSizes = new float[3] { 12f, 14f, 21f };

	private static readonly float[] FallbackFontLineHeights = new float[3] { 12f, 16f, 23f };

	public Vector2 contentOffset { get; set; }

	public Font font { get; set; }

	public TextAnchor alignment { get; set; }

	public bool wordWrap { get; set; }

	public float fixedHeight { get; set; }

	public float fixedWidth { get; set; }

	public RectOffset padding { get; set; } = new RectOffset();

	public RectOffset margin { get; set; } = new RectOffset();

	public RectOffset border { get; set; } = new RectOffset();

	public RectOffset overflow { get; set; } = new RectOffset();

	public int fontSize { get; set; }

	public FontStyle fontStyle { get; set; }

	public bool richText { get; set; }

	public TextClipping clipping { get; set; }

	public ImagePosition imagePosition { get; set; }

	public float lineHeight => GuiBackend.Current?.LineSpacing(FontIndex, fontSize) ?? MeasureText("X", -1f).y;

	public GUIStyleState normal { get; set; } = new GUIStyleState();

	public GUIStyleState active { get; set; } = new GUIStyleState();

	public GUIStyleState hover { get; set; } = new GUIStyleState();

	public GUIStyleState focused { get; set; } = new GUIStyleState();

	public GUIStyleState onFocused { get; set; } = new GUIStyleState();

	public GUIStyleState onHover { get; set; } = new GUIStyleState();

	public GUIStyleState onNormal { get; set; } = new GUIStyleState();

	public GUIStyleState onActive { get; set; } = new GUIStyleState();

	public string name { get; set; } = "";

	public bool stretchWidth { get; set; } = true;

	public bool stretchHeight { get; set; }

	public static GUIStyle none { get; } = new GUIStyle();

	public bool isHeightDependantOnWidth
	{
		get
		{
			if (fixedHeight == 0f && wordWrap)
			{
				return imagePosition != ImagePosition.ImageOnly;
			}
			return false;
		}
	}

	public int FontIndex => font?.GameFontIndex ?? 1;

	public GUIStyle()
	{
	}

	public GUIStyle(GUIStyle other)
	{
		if (other != null)
		{
			name = other.name;
			font = other.font;
			alignment = other.alignment;
			wordWrap = other.wordWrap;
			fixedHeight = other.fixedHeight;
			fixedWidth = other.fixedWidth;
			padding = Copy(other.padding);
			margin = Copy(other.margin);
			border = Copy(other.border);
			overflow = Copy(other.overflow);
			contentOffset = other.contentOffset;
			fontSize = other.fontSize;
			fontStyle = other.fontStyle;
			richText = other.richText;
			clipping = other.clipping;
			imagePosition = other.imagePosition;
			normal = Copy(other.normal);
			active = Copy(other.active);
			hover = Copy(other.hover);
			focused = Copy(other.focused);
			onFocused = Copy(other.onFocused);
			onHover = Copy(other.onHover);
		}
	}

	private static GUIStyleState Copy(GUIStyleState o)
	{
		if (o != null)
		{
			return new GUIStyleState
			{
				background = o.background,
				textColor = o.textColor
			};
		}
		return new GUIStyleState();
	}

	private static RectOffset Copy(RectOffset o)
	{
		if (o != null)
		{
			return new RectOffset(o.left, o.right, o.top, o.bottom);
		}
		return new RectOffset();
	}

	public void Draw(Rect position, GUIContent content, bool isHover, bool isActive, bool on, bool hasKeyboardFocus)
	{
		Internal_Draw_Injected(ref position, content, isHover, isActive, on, hasKeyboardFocus);
	}

	private void Internal_Draw(Rect screenRect, GUIContent content, bool isHover, bool isActive, bool on, bool hasKeyboardFocus)
	{
		Internal_Draw_Injected(ref screenRect, content, isHover, isActive, on, hasKeyboardFocus);
	}

	private void Internal_Draw_Injected(ref Rect position, GUIContent content, bool isHover, bool isActive, bool on, bool hasKeyboardFocus)
	{
		IGuiBackend current = GuiBackend.Current;
		if (current != null)
		{
			GUIStyleState gUIStyleState = ((!on) ? (isActive ? active : (isHover ? hover : (hasKeyboardFocus ? focused : normal))) : (isActive ? onActive : (isHover ? onHover : (hasKeyboardFocus ? onFocused : onNormal))));
			if (gUIStyleState?.background != null)
			{
				DrawBackground(current, position, gUIStyleState.background, GUI.color * GUI.backgroundColor);
			}
			DrawContent(current, position, content, GUI.color * GUI.contentColor, GUI.color * GUI.contentColor * (gUIStyleState?.textColor ?? Color.white));
		}
	}

	internal void DrawContent(IGuiBackend be, Rect position, GUIContent content, Color imageColor, Color textColor)
	{
		Texture texture = ((imagePosition == ImagePosition.TextOnly) ? null : content?.image);
		string text = ((imagePosition == ImagePosition.ImageOnly) ? null : content?.text);
		bool num = texture != null && texture.width > 0 && texture.height > 0;
		bool flag = text != null && text.Length > 0;
		if (!num)
		{
			if (flag)
			{
				DrawText(be, position, text, textColor);
			}
			return;
		}
		if (!flag)
		{
			DrawImage(be, position, texture, imageColor);
			return;
		}
		Rect rect = ContentRect(position);
		int num2 = (int)alignment % 3;
		int num3 = (int)alignment / 3;
		if (imagePosition == ImagePosition.ImageAbove)
		{
			float num4 = Mathf.Min(rect.height, MeasureText(text, wordWrap ? rect.width : (-1f)).y);
			Vector2 vector = FitImage(texture, rect.width, rect.height - num4);
			float num5 = rect.y + Mathf.Max(0f, rect.height - vector.y - num4) * (float)num3 * 0.5f;
			be.DrawTexture(new Rect(rect.x + (rect.width - vector.x) * (float)num2 * 0.5f, num5, vector.x, vector.y), texture, ScaleMode.StretchToFill, imageColor);
			DrawTextIn(be, new Rect(rect.x, num5 + vector.y, rect.width, num4), position, text, textColor, (TextAnchor)num2);
		}
		else
		{
			Vector2 vector2 = FitImage(texture, rect.width, rect.height);
			float num6 = Mathf.Min(rect.width - vector2.x, MeasureText(text, -1f).x);
			float num7 = rect.x + Mathf.Max(0f, rect.width - vector2.x - num6) * (float)num2 * 0.5f;
			be.DrawTexture(new Rect(num7, rect.y + (rect.height - vector2.y) * (float)num3 * 0.5f, vector2.x, vector2.y), texture, ScaleMode.StretchToFill, imageColor);
			DrawTextIn(be, Rect.MinMaxRect(num7 + vector2.x, rect.y, rect.xMax, rect.yMax), position, text, textColor, (TextAnchor)(num3 * 3));
		}
	}

	private Rect ContentRect(Rect position)
	{
		return new Rect(position.x + (float)padding.left + contentOffset.x, position.y + (float)padding.top + contentOffset.y, Math.Max(0f, position.width - (float)padding.horizontal), Math.Max(0f, position.height - (float)padding.vertical));
	}

	private static Vector2 FitImage(Texture image, float width, float height)
	{
		float num = Mathf.Clamp01(Mathf.Min(width / (float)image.width, height / (float)image.height));
		return new Vector2((float)image.width * num, (float)image.height * num);
	}

	internal void DrawBackground(IGuiBackend be, Rect position, Texture background, Color color)
	{
		RectOffset rectOffset = border;
		if (rectOffset == null || (rectOffset.horizontal == 0 && rectOffset.vertical == 0) || background.width <= rectOffset.horizontal || background.height <= rectOffset.vertical)
		{
			be.DrawTexture(position, background, ScaleMode.StretchToFill, color);
			return;
		}
		float num = background.width;
		float num2 = background.height;
		InlineArray4<float> buffer = default(InlineArray4<float>);
		global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray4<float>, float>(ref buffer, 0) = position.x;
		global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray4<float>, float>(ref buffer, 1) = position.x + (float)rectOffset.left;
		global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray4<float>, float>(ref buffer, 2) = position.xMax - (float)rectOffset.right;
		global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray4<float>, float>(ref buffer, 3) = position.xMax;
		Span<float> span = global::_003CPrivateImplementationDetails_003E.InlineArrayAsSpan<InlineArray4<float>, float>(ref buffer, 4);
		InlineArray4<float> buffer2 = default(InlineArray4<float>);
		global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray4<float>, float>(ref buffer2, 0) = position.y;
		global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray4<float>, float>(ref buffer2, 1) = position.y + (float)rectOffset.top;
		global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray4<float>, float>(ref buffer2, 2) = position.yMax - (float)rectOffset.bottom;
		global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray4<float>, float>(ref buffer2, 3) = position.yMax;
		Span<float> span2 = global::_003CPrivateImplementationDetails_003E.InlineArrayAsSpan<InlineArray4<float>, float>(ref buffer2, 4);
		InlineArray4<float> buffer3 = default(InlineArray4<float>);
		global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray4<float>, float>(ref buffer3, 0) = 0f;
		global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray4<float>, float>(ref buffer3, 1) = (float)rectOffset.left / num;
		global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray4<float>, float>(ref buffer3, 2) = 1f - (float)rectOffset.right / num;
		global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray4<float>, float>(ref buffer3, 3) = 1f;
		Span<float> span3 = global::_003CPrivateImplementationDetails_003E.InlineArrayAsSpan<InlineArray4<float>, float>(ref buffer3, 4);
		InlineArray4<float> buffer4 = default(InlineArray4<float>);
		global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray4<float>, float>(ref buffer4, 0) = 1f;
		global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray4<float>, float>(ref buffer4, 1) = 1f - (float)rectOffset.top / num2;
		global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray4<float>, float>(ref buffer4, 2) = (float)rectOffset.bottom / num2;
		global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray4<float>, float>(ref buffer4, 3) = 0f;
		Span<float> span4 = global::_003CPrivateImplementationDetails_003E.InlineArrayAsSpan<InlineArray4<float>, float>(ref buffer4, 4);
		for (int i = 0; i < 3; i++)
		{
			for (int j = 0; j < 3; j++)
			{
				Rect rect = Rect.MinMaxRect(span[j], span2[i], span[j + 1], span2[i + 1]);
				if (rect.width > 0f && rect.height > 0f)
				{
					be.DrawTextureWithTexCoords(rect, background, Rect.MinMaxRect(span3[j], span4[i + 1], span3[j + 1], span4[i]), color);
				}
			}
		}
	}

	internal void DrawImage(IGuiBackend be, Rect position, Texture image, Color color)
	{
		if (image.width > 0 && image.height > 0)
		{
			Rect rect = ((padding != null) ? padding.Remove(position) : position);
			Vector2 vector = FitImage(image, rect.width, rect.height);
			int num = (int)alignment;
			float x = rect.x + (rect.width - vector.x) * (float)(num % 3) * 0.5f;
			float y = rect.y + (rect.height - vector.y) * (float)(num / 3) * 0.5f;
			be.DrawTexture(new Rect(x, y, vector.x, vector.y), image, ScaleMode.StretchToFill, color);
		}
	}

	internal void DrawText(IGuiBackend be, Rect position, string text, Color color)
	{
		DrawTextIn(be, ContentRect(position), position, text, color, alignment);
	}

	private void DrawTextIn(IGuiBackend be, Rect content, Rect position, string text, Color color, TextAnchor anchor)
	{
		if (clipping != TextClipping.Clip || (!(position.width <= 0f) && !(position.height <= 0f)))
		{
			be.DrawText(content, text, FontIndex, fontSize, fontStyle, anchor, wordWrap, color, (clipping == TextClipping.Clip) ? position : default(Rect));
		}
	}

	public void Draw(Rect position, GUIContent content, bool isHover, bool isActive, bool on, bool hasKeyboardFocus, bool ignored)
	{
		Draw(position, content, isHover, isActive, on, hasKeyboardFocus);
	}

	public void Draw(Rect position, bool isHover, bool isActive, bool on, bool hasKeyboardFocus)
	{
		Draw(position, GUIContent.none, isHover, isActive, on, hasKeyboardFocus);
	}

	public void Draw(Rect position, string text, bool isHover, bool isActive, bool on, bool hasKeyboardFocus)
	{
		Draw(position, new GUIContent(text), isHover, isActive, on, hasKeyboardFocus);
	}

	public void Draw(Rect position, Texture image, bool isHover, bool isActive, bool on, bool hasKeyboardFocus)
	{
		Draw(position, new GUIContent("", image), isHover, isActive, on, hasKeyboardFocus);
	}

	public void CalcMinMaxWidth(GUIContent content, out float minWidth, out float maxWidth)
	{
		maxWidth = CalcSize(content).x;
		minWidth = maxWidth;
		if (wordWrap)
		{
			minWidth = 0f;
			string[] array = (content?.text ?? "").Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries);
			foreach (string text in array)
			{
				minWidth = Math.Max(minWidth, CalcSize(new GUIContent(text)).x);
			}
		}
	}

	public float CalcHeight(GUIContent content, float width)
	{
		Vector2 vector = LaidOutImage(content);
		string text = ((imagePosition == ImagePosition.ImageOnly) ? "" : (content?.text ?? ""));
		if (vector == Vector2.zero)
		{
			return MeasureText(text, wordWrap ? (width - (float)padding.horizontal) : (-1f)).y + (float)padding.vertical;
		}
		if (text.Length == 0)
		{
			return vector.y + (float)padding.vertical;
		}
		if (imagePosition == ImagePosition.ImageAbove)
		{
			return vector.y + MeasureText(text, wordWrap ? (width - (float)padding.horizontal) : (-1f)).y + (float)padding.vertical;
		}
		return Mathf.Max(vector.y, MeasureText(text, wordWrap ? (width - (float)padding.horizontal - vector.x) : (-1f)).y) + (float)padding.vertical;
	}

	public Vector2 CalcSize(GUIContent content)
	{
		Vector2 vector = LaidOutImage(content);
		string text = ((imagePosition == ImagePosition.ImageOnly) ? "" : (content?.text ?? ""));
		Vector2 vector2 = ((vector == Vector2.zero || text.Length > 0) ? MeasureText(text, -1f) : Vector2.zero);
		if (vector != Vector2.zero && text.Length > 0)
		{
			vector2 = ((imagePosition == ImagePosition.ImageAbove) ? new Vector2(Mathf.Max(vector.x, vector2.x), vector.y + vector2.y) : new Vector2(vector.x + vector2.x, Mathf.Max(vector.y, vector2.y)));
		}
		else if (vector != Vector2.zero)
		{
			vector2 = vector;
		}
		return new Vector2(vector2.x + (float)padding.horizontal, vector2.y + (float)padding.vertical);
	}

	internal int GetNumCharactersThatFitWithinWidth(string text, float width)
	{
		if (string.IsNullOrEmpty(text))
		{
			return 0;
		}
		int num = 0;
		int num2 = text.Length;
		while (num < num2)
		{
			int num3 = (num + num2 + 1) / 2;
			if (MeasureText(text.Substring(0, num3), -1f).x <= width)
			{
				num = num3;
			}
			else
			{
				num2 = num3 - 1;
			}
		}
		return num;
	}

	private Vector2 LaidOutImage(GUIContent content)
	{
		Texture texture = ((imagePosition == ImagePosition.TextOnly) ? null : content?.image);
		if (!(texture != null) || texture.width <= 0 || texture.height <= 0)
		{
			return Vector2.zero;
		}
		return new Vector2(texture.width, texture.height);
	}

	private Vector2 MeasureText(string text, float wrapWidth)
	{
		return GuiBackend.Current?.MeasureText(FontIndex, fontSize, fontStyle, text, wrapWidth) ?? FallbackMeasure(text, wrapWidth);
	}

	private Vector2 FallbackMeasure(string text, float wrapWidth)
	{
		int num = Math.Clamp(FontIndex, 0, FallbackFontPixelSizes.Length - 1);
		float num2 = ((fontSize > 0) ? ((float)fontSize) : FallbackFontPixelSizes[num]);
		float num3 = FallbackFontLineHeights[num] * num2 / FallbackFontPixelSizes[num];
		float num4 = num2 * 0.5f;
		int num5 = 1;
		float num6 = 0f;
		float num7 = 0f;
		string text2 = text ?? "";
		for (int i = 0; i < text2.Length; i++)
		{
			if (text2[i] == '\n')
			{
				num5++;
				if (num7 > num6)
				{
					num6 = num7;
				}
				num7 = 0f;
			}
			else
			{
				num7 += num4;
			}
		}
		if (num7 > num6)
		{
			num6 = num7;
		}
		if (wrapWidth > 0f && num6 > wrapWidth)
		{
			num5 += (int)(num6 / wrapWidth);
		}
		return new Vector2(num6, num3 * (float)num5);
	}
}
