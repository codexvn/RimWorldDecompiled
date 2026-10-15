using System.Collections.Generic;
using System.Text;
using RimWorld;
using UnityEngine;
using UnityStubs.Gui;
using Verse.Sound;

namespace Verse.ProfilerUI;

public static class ProfilerGui
{
	public static readonly Color TextMain = Hex(4293126642u);

	public static readonly Color TextBright = Hex(4291416541u);

	public static readonly Color TextDim = Hex(4288324786u);

	public static readonly Color TextMuted = Hex(4290232524u);

	public static readonly Color TextFaint = Hex(4284245104u);

	public static readonly Color TextDisabled = Hex(4286416788u);

	public static readonly Color Regression = Hex(4292898911u);

	public static readonly Color Improvement = Hex(4284465030u);

	public static readonly Color WindowBackground = Hex(4279638046u);

	public static readonly Color SectionBackground = Hex(4279506460u);

	public static readonly Color SunkenBackground = Hex(4279374874u);

	public static readonly Color TitleBar = Hex(4280099114u);

	public static readonly Color TitleBarUnfocused = Hex(4279967270u);

	public static readonly Color ChipBackground = Hex(4280559922u);

	public static readonly Color ChipHover = Hex(4281152578u);

	public static readonly Color FieldBackground = Hex(4280165165u);

	public static readonly Color RowAlternate = Hex(4279835941u);

	public static readonly Color RowSelected = Hex(4280495165u);

	public static readonly Color BarTrack = Hex(4280033321u);

	public static readonly Color BarFill = Hex(4284773519u);

	public static readonly Color Divider = Hex(4281020476u);

	public static readonly Color SurfaceBorder = Hex(4280691509u);

	public static readonly Color FocusBorder = Hex(4282007890u);

	public static readonly Color ScrollThumb = Hex(4282139473u);

	public static readonly Color ScrollThumbHover = Hex(4283061352u);

	public static readonly Color SpikeChip = Hex(4283050533u);

	public static readonly StringBuilder Scratch = new StringBuilder(256);

	private static string tooltipText;

	private static Vector2 tooltipAt;

	private static readonly List<Rect> overlays = new List<Rect>();

	private static float spaceWidth = -1f;

	public static bool Painting => Event.current.type == EventType.repaint;

	public static Color Hex(uint argb)
	{
		return new Color32((byte)(argb >> 16), (byte)(argb >> 8), (byte)argb, (byte)(argb >> 24));
	}

	public static void Fill(Rect rect, Color color)
	{
		if (Painting && rect.width > 0f && rect.height > 0f)
		{
			GuiBackend.Current.FillRect(rect, color);
		}
	}

	public static void Fill(float x0, float y0, float x1, float y1, Color color)
	{
		Fill(Rect.MinMaxRect(x0, y0, x1, y1), color);
	}

	public static void Arrow(Rect cell, bool down, Color color)
	{
		if (Painting)
		{
			Vector2 center = cell.center;
			if (down)
			{
				GuiBackend.Current.DrawTextureQuad(new Vector2(center.x - 3.5f, center.y - 2.5f), new Vector2(center.x + 3.5f, center.y - 2.5f), new Vector2(center.x, center.y + 2.5f), new Vector2(center.x, center.y + 2.5f), null, color);
			}
			else
			{
				GuiBackend.Current.DrawTextureQuad(new Vector2(center.x - 2.5f, center.y - 3.5f), new Vector2(center.x + 2.5f, center.y), new Vector2(center.x - 2.5f, center.y + 3.5f), new Vector2(center.x - 2.5f, center.y + 3.5f), null, color);
			}
		}
	}

	public static void Outline(Rect rect, Color color)
	{
		Fill(new Rect(rect.x, rect.y, rect.width, 1f), color);
		Fill(new Rect(rect.x, rect.yMax - 1f, rect.width, 1f), color);
		Fill(new Rect(rect.x, rect.y + 1f, 1f, rect.height - 2f), color);
		Fill(new Rect(rect.xMax - 1f, rect.y + 1f, 1f, rect.height - 2f), color);
	}

	public static void Bordered(Rect rect, Color fill, Color border)
	{
		Fill(rect, border);
		Fill(rect.ContractedBy(1f), fill);
	}

	public static void Label(Rect rect, string text, Color color, TextAnchor anchor = TextAnchor.MiddleLeft, GameFont font = GameFont.Tiny)
	{
		if (Painting && !string.IsNullOrEmpty(text) && !(rect.width <= 0f))
		{
			GUIStyle gUIStyle = Text.fontStyles[(uint)font];
			RectOffset padding = gUIStyle.padding;
			Rect rect2 = new Rect(rect.x + (float)padding.left, rect.y + (float)padding.top, Mathf.Max(0f, rect.width - (float)padding.horizontal), Mathf.Max(0f, rect.height - (float)padding.vertical));
			GuiBackend.Current.DrawText(rect2, text, gUIStyle.FontIndex, gUIStyle.fontSize, gUIStyle.fontStyle, anchor, wordWrap: false, color, rect);
		}
	}

	public static float Width(string text, GameFont font = GameFont.Tiny)
	{
		GUIStyle gUIStyle = Text.fontStyles[(uint)font];
		return GuiBackend.Current.MeasureText(gUIStyle.FontIndex, gUIStyle.fontSize, gUIStyle.fontStyle, text, -1f).x + (float)gUIStyle.padding.horizontal;
	}

	public static string Ellipsize(string text, float width)
	{
		Text.Font = GameFont.Tiny;
		string result = ((Text.CalcSize(text).x <= width) ? text : text.Truncate(width));
		Text.Font = GameFont.Small;
		return result;
	}

	private static void MeasureRun(string text, out string trimmed, out float leadingPad, out float width)
	{
		if (spaceWidth < 0f)
		{
			spaceWidth = Mathf.Max(3f, Width("a a") - Width("aa"));
		}
		trimmed = text.Trim(' ');
		int i;
		for (i = 0; i < text.Length && text[i] == ' '; i++)
		{
		}
		leadingPad = (float)i * spaceWidth;
		width = Width(trimmed) + (float)(text.Length - trimmed.Length) * spaceWidth;
	}

	public static float Run(float x, float y, float height, string text, Color color)
	{
		MeasureRun(text, out var trimmed, out var leadingPad, out var width);
		Label(new Rect(x + leadingPad, y, width + 4f, height), trimmed, color);
		return x + width;
	}

	public static float RunRightAligned(float right, float y, float height, string text, Color color)
	{
		MeasureRun(text, out var _, out var _, out var width);
		Run(right - width, y, height, text, color);
		return right - width;
	}

	public static void ClearOverlays()
	{
		overlays.Clear();
	}

	public static void AddOverlay(Rect rect)
	{
		overlays.Add(new Rect(GUIUtility.GUIToScreenPoint(rect.position), rect.size));
	}

	private static bool UnderOverlay()
	{
		Vector2 point = GUIUtility.GUIToScreenPoint(Event.current.mousePosition);
		for (int i = 0; i < overlays.Count; i++)
		{
			if (overlays[i].Contains(point))
			{
				return true;
			}
		}
		return false;
	}

	public static bool Hovered(Rect rect)
	{
		if (rect.Contains(Event.current.mousePosition))
		{
			return !UnderOverlay();
		}
		return false;
	}

	public static void MouseoverSound(Rect rect, SoundDef sound = null)
	{
		if (Painting && !UnderOverlay())
		{
			MouseoverSounds.DoRegion(rect, sound ?? SoundDefOf.Mouseover_Standard);
		}
	}

	public static bool Pressed(Rect rect, SoundDef sound = null)
	{
		MouseoverSound(rect);
		if (!Clicked(rect))
		{
			return false;
		}
		(sound ?? SoundDefOf.Click).PlayOneShotOnCamera();
		return true;
	}

	public static bool Clicked(Rect rect, int button = 0)
	{
		Event current = Event.current;
		if (current.type != 0 || current.button != button || !rect.Contains(current.mousePosition))
		{
			return false;
		}
		current.Use();
		return true;
	}

	public static bool Chip(Rect rect, string label, bool active = false, bool enabled = true, Color? activeFill = null)
	{
		bool flag = enabled && Hovered(rect);
		Color color = ((!active) ? (flag ? ChipHover : ChipBackground) : (activeFill ?? ChipHover));
		Fill(rect, color);
		Label(rect, label, (!enabled) ? TextFaint : (active ? TextMain : (flag ? TextBright : TextDim)), TextAnchor.MiddleCenter);
		if (enabled)
		{
			return Pressed(rect);
		}
		return false;
	}

	public static bool Ghost(Rect rect, string label, bool active = false)
	{
		bool flag = Hovered(rect);
		if (active || flag)
		{
			Fill(rect, active ? ChipHover : ChipBackground);
		}
		Label(rect, label, active ? TextMain : (flag ? TextBright : TextDim), TextAnchor.MiddleCenter);
		return Pressed(rect);
	}

	public static int Segments(ref float x, float y, string[] labels, int activeIndex, Color? activeFill = null, int redIndex = -1)
	{
		float num = 4f;
		for (int i = 0; i < labels.Length; i++)
		{
			num += Width(labels[i]) + 18f;
		}
		Bordered(new Rect(x, y, num, 22f), SunkenBackground, Divider);
		int result = -1;
		float num2 = x + 2f;
		for (int j = 0; j < labels.Length; j++)
		{
			Rect rect = new Rect(num2, y + 2f, Width(labels[j]) + 18f, 18f);
			bool flag = j == activeIndex;
			if (flag)
			{
				Fill(rect, (j == redIndex) ? SpikeChip : ChipHover);
			}
			Label(rect, labels[j], flag ? TextMain : (Hovered(rect) ? TextDim : TextFaint), TextAnchor.MiddleCenter);
			MouseoverSound(rect, SoundDefOf.Mouseover_Tab);
			if (!flag && Clicked(rect))
			{
				SoundDefOf.RowTabSelect.PlayOneShotOnCamera();
				result = j;
			}
			num2 += rect.width;
		}
		x += num;
		return result;
	}

	public static void SetTooltip(string text)
	{
		tooltipText = text;
		tooltipAt = GUIUtility.GUIToScreenPoint(Event.current.mousePosition);
	}

	public static void ClearTooltip()
	{
		tooltipText = null;
	}

	public static void FlushTooltip(Rect bounds)
	{
		if (!Painting)
		{
			return;
		}
		string text = tooltipText;
		tooltipText = null;
		if (text != null)
		{
			Text.Font = GameFont.Tiny;
			Text.WordWrap = false;
			Vector2 vector = Text.CalcSize(text);
			Text.WordWrap = true;
			Text.Font = GameFont.Small;
			vector += new Vector2(16f, 10f);
			Vector2 vector2 = GUIUtility.ScreenToGUIPoint(tooltipAt);
			float num = vector2.x + 12f;
			if (num + vector.x > bounds.xMax)
			{
				num = vector2.x - 12f - vector.x;
			}
			float b = Mathf.Min(vector2.y + 14f, bounds.yMax - vector.y);
			Rect rect = new Rect(Mathf.Max(bounds.x, num), Mathf.Max(bounds.y, b), vector.x, vector.y);
			Bordered(rect, TitleBar, FocusBorder);
			Label(new Rect(rect.x + 8f, rect.y + 5f, vector.x, vector.y), text, TextMain, TextAnchor.UpperLeft);
		}
	}
}
