using System;
using System.Collections.Generic;

namespace UnityEngine;

internal class GUILayoutGroup : GUILayoutEntry
{
	public List<GUILayoutEntry> entries = new List<GUILayoutEntry>();

	public bool isVertical = true;

	public bool resetCoords;

	public float spacing;

	public bool sameSize = true;

	public int windowID = -1;

	private int m_Cursor;

	protected int m_StretchableCountX = 100;

	protected int m_StretchableCountY = 100;

	protected bool m_UserSpecifiedWidth;

	protected bool m_UserSpecifiedHeight;

	protected float m_ChildMinWidth = 100f;

	protected float m_ChildMaxWidth = 100f;

	protected float m_ChildMinHeight = 100f;

	protected float m_ChildMaxHeight = 100f;

	protected int m_MarginLeft;

	protected int m_MarginRight;

	protected int m_MarginTop;

	protected int m_MarginBottom;

	private static readonly GUILayoutEntry none = new GUILayoutEntry(0f, 1f, 0f, 1f, GUIStyle.none);

	public override int marginLeft => m_MarginLeft;

	public override int marginRight => m_MarginRight;

	public override int marginTop => m_MarginTop;

	public override int marginBottom => m_MarginBottom;

	public GUILayoutGroup()
		: base(0f, 0f, 0f, 0f, GUIStyle.none)
	{
	}

	public GUILayoutGroup(GUIStyle _style, GUILayoutOption[] options)
		: base(0f, 0f, 0f, 0f, _style)
	{
		if (options != null)
		{
			ApplyOptions(options);
		}
		m_MarginLeft = _style.margin.left;
		m_MarginRight = _style.margin.right;
		m_MarginTop = _style.margin.top;
		m_MarginBottom = _style.margin.bottom;
	}

	public override void ApplyOptions(GUILayoutOption[] options)
	{
		if (options == null)
		{
			return;
		}
		base.ApplyOptions(options);
		foreach (GUILayoutOption gUILayoutOption in options)
		{
			switch (gUILayoutOption.type)
			{
			case GUILayoutOption.Type.fixedWidth:
			case GUILayoutOption.Type.minWidth:
			case GUILayoutOption.Type.maxWidth:
				m_UserSpecifiedHeight = true;
				break;
			case GUILayoutOption.Type.fixedHeight:
			case GUILayoutOption.Type.minHeight:
			case GUILayoutOption.Type.maxHeight:
				m_UserSpecifiedWidth = true;
				break;
			case GUILayoutOption.Type.spacing:
				spacing = Convert.ToInt32(gUILayoutOption.value);
				break;
			}
		}
	}

	protected override void ApplyStyleSettings(GUIStyle style)
	{
		base.ApplyStyleSettings(style);
		RectOffset margin = style.margin;
		m_MarginLeft = margin.left;
		m_MarginRight = margin.right;
		m_MarginTop = margin.top;
		m_MarginBottom = margin.bottom;
	}

	public void ResetCursor()
	{
		m_Cursor = 0;
	}

	public Rect PeekNext()
	{
		if (m_Cursor < entries.Count)
		{
			return entries[m_Cursor].rect;
		}
		if (Event.current.type == EventType.repaint)
		{
			throw new ArgumentException("Getting control " + m_Cursor + "'s position in a group with only " + entries.Count + " controls when doing " + Event.current.rawType.ToString() + "\nAborting");
		}
		return GUILayoutEntry.kDummyRect;
	}

	public GUILayoutEntry GetNext()
	{
		if (m_Cursor < entries.Count)
		{
			return entries[m_Cursor++];
		}
		if (Event.current.type == EventType.repaint)
		{
			throw new ArgumentException("Getting control " + m_Cursor + "'s position in a group with only " + entries.Count + " controls when doing " + Event.current.rawType.ToString() + "\nAborting");
		}
		return none;
	}

	public Rect GetLast()
	{
		if (m_Cursor == 0)
		{
			if (Event.current.type == EventType.repaint)
			{
				Debug.LogError("You cannot call GetLast immediately after beginning a group.");
			}
			return GUILayoutEntry.kDummyRect;
		}
		if (m_Cursor <= entries.Count)
		{
			return entries[m_Cursor - 1].rect;
		}
		if (Event.current.type == EventType.repaint)
		{
			Debug.LogError("Getting control " + m_Cursor + "'s position in a group with only " + entries.Count + " controls when doing " + Event.current.rawType);
		}
		return GUILayoutEntry.kDummyRect;
	}

	public void Add(GUILayoutEntry e)
	{
		entries.Add(e);
	}

	public override void CalcWidth()
	{
		if (entries.Count == 0)
		{
			maxWidth = (minWidth = base.style.padding.horizontal);
			return;
		}
		int num = 0;
		int num2 = 0;
		m_ChildMinWidth = 0f;
		m_ChildMaxWidth = 0f;
		m_StretchableCountX = 0;
		bool flag = true;
		if (isVertical)
		{
			foreach (GUILayoutEntry entry in entries)
			{
				entry.CalcWidth();
				if (entry.consideredForMargin)
				{
					if (!flag)
					{
						num = Mathf.Min(entry.marginLeft, num);
						num2 = Mathf.Min(entry.marginRight, num2);
					}
					else
					{
						num = entry.marginLeft;
						num2 = entry.marginRight;
						flag = false;
					}
					m_ChildMinWidth = Mathf.Max(entry.minWidth + (float)entry.marginHorizontal, m_ChildMinWidth);
					m_ChildMaxWidth = Mathf.Max(entry.maxWidth + (float)entry.marginHorizontal, m_ChildMaxWidth);
				}
				m_StretchableCountX += entry.stretchWidth;
			}
			m_ChildMinWidth -= num + num2;
			m_ChildMaxWidth -= num + num2;
		}
		else
		{
			int num3 = 0;
			foreach (GUILayoutEntry entry2 in entries)
			{
				entry2.CalcWidth();
				if (entry2.consideredForMargin)
				{
					int num4;
					if (!flag)
					{
						num4 = ((num3 > entry2.marginLeft) ? num3 : entry2.marginLeft);
					}
					else
					{
						num4 = 0;
						flag = false;
					}
					m_ChildMinWidth += entry2.minWidth + spacing + (float)num4;
					m_ChildMaxWidth += entry2.maxWidth + spacing + (float)num4;
					num3 = entry2.marginRight;
				}
				else
				{
					m_ChildMinWidth += entry2.minWidth;
					m_ChildMaxWidth += entry2.maxWidth;
				}
				m_StretchableCountX += entry2.stretchWidth;
			}
			m_ChildMinWidth -= spacing;
			m_ChildMaxWidth -= spacing;
			num = entries[0].marginLeft;
			num2 = num3;
		}
		float num5;
		float num6;
		if (base.style != GUIStyle.none || m_UserSpecifiedWidth)
		{
			num5 = Mathf.Max(base.style.padding.left, num);
			num6 = Mathf.Max(base.style.padding.right, num2);
		}
		else
		{
			m_MarginLeft = num;
			m_MarginRight = num2;
			num5 = (num6 = 0f);
		}
		minWidth = Mathf.Max(minWidth, m_ChildMinWidth + num5 + num6);
		if (maxWidth == 0f)
		{
			stretchWidth += m_StretchableCountX + (base.style.stretchWidth ? 1 : 0);
			maxWidth = m_ChildMaxWidth + num5 + num6;
		}
		else
		{
			stretchWidth = 0;
		}
		maxWidth = Mathf.Max(maxWidth, minWidth);
		if (base.style.fixedWidth != 0f)
		{
			maxWidth = (minWidth = base.style.fixedWidth);
			stretchWidth = 0;
		}
	}

	public override void SetHorizontal(float x, float width)
	{
		base.SetHorizontal(x, width);
		if (resetCoords)
		{
			x = 0f;
		}
		RectOffset padding = base.style.padding;
		if (isVertical)
		{
			if (base.style != GUIStyle.none)
			{
				foreach (GUILayoutEntry entry in entries)
				{
					float num = Mathf.Max(entry.marginLeft, padding.left);
					float num2 = width - (float)Mathf.Max(entry.marginRight, padding.right) - num;
					entry.SetHorizontal(x + num, (entry.stretchWidth != 0) ? num2 : Mathf.Clamp(num2, entry.minWidth, entry.maxWidth));
				}
				return;
			}
			float num3 = x - (float)marginLeft;
			float num4 = width + (float)base.marginHorizontal;
			{
				foreach (GUILayoutEntry entry2 in entries)
				{
					float num5 = num4 - (float)entry2.marginHorizontal;
					entry2.SetHorizontal(num3 + (float)entry2.marginLeft, (entry2.stretchWidth != 0) ? num5 : Mathf.Clamp(num5, entry2.minWidth, entry2.maxWidth));
				}
				return;
			}
		}
		if (base.style != GUIStyle.none)
		{
			float num6 = padding.left;
			float num7 = padding.right;
			if (entries.Count != 0)
			{
				num6 = Mathf.Max(num6, entries[0].marginLeft);
				float a = num7;
				List<GUILayoutEntry> list = entries;
				num7 = Mathf.Max(a, list[list.Count - 1].marginRight);
			}
			x += num6;
			width -= num7 + num6;
		}
		float num8 = width - spacing * (float)(entries.Count - 1);
		float t = 0f;
		if (m_ChildMinWidth != m_ChildMaxWidth)
		{
			t = Mathf.Clamp((num8 - m_ChildMinWidth) / (m_ChildMaxWidth - m_ChildMinWidth), 0f, 1f);
		}
		float num9 = 0f;
		if (num8 > m_ChildMaxWidth && m_StretchableCountX > 0)
		{
			num9 = (num8 - m_ChildMaxWidth) / (float)m_StretchableCountX;
		}
		int num10 = 0;
		bool flag = true;
		foreach (GUILayoutEntry entry3 in entries)
		{
			float num11 = Mathf.Lerp(entry3.minWidth, entry3.maxWidth, t) + num9 * (float)entry3.stretchWidth;
			if (entry3.consideredForMargin)
			{
				int num12 = entry3.marginLeft;
				if (flag)
				{
					num12 = 0;
					flag = false;
				}
				x += (float)((num10 > num12) ? num10 : num12);
				num10 = entry3.marginRight;
			}
			entry3.SetHorizontal(Mathf.Round(x), Mathf.Round(num11));
			x += num11 + spacing;
		}
	}

	public override void CalcHeight()
	{
		if (entries.Count == 0)
		{
			maxHeight = (minHeight = base.style.padding.vertical);
			return;
		}
		int b = 0;
		int b2 = 0;
		m_ChildMinHeight = 0f;
		m_ChildMaxHeight = 0f;
		m_StretchableCountY = 0;
		if (isVertical)
		{
			int num = 0;
			bool flag = true;
			foreach (GUILayoutEntry entry in entries)
			{
				entry.CalcHeight();
				if (entry.consideredForMargin)
				{
					int num2;
					if (!flag)
					{
						num2 = Mathf.Max(num, entry.marginTop);
					}
					else
					{
						num2 = 0;
						flag = false;
					}
					m_ChildMinHeight += entry.minHeight + spacing + (float)num2;
					m_ChildMaxHeight += entry.maxHeight + spacing + (float)num2;
					num = entry.marginBottom;
				}
				else
				{
					m_ChildMinHeight += entry.minHeight;
					m_ChildMaxHeight += entry.maxHeight;
				}
				m_StretchableCountY += entry.stretchHeight;
			}
			m_ChildMinHeight -= spacing;
			m_ChildMaxHeight -= spacing;
			b = entries[0].marginTop;
			b2 = num;
		}
		else
		{
			bool flag2 = true;
			foreach (GUILayoutEntry entry2 in entries)
			{
				entry2.CalcHeight();
				if (entry2.consideredForMargin)
				{
					if (!flag2)
					{
						b = Mathf.Min(entry2.marginTop, b);
						b2 = Mathf.Min(entry2.marginBottom, b2);
					}
					else
					{
						b = entry2.marginTop;
						b2 = entry2.marginBottom;
						flag2 = false;
					}
					m_ChildMinHeight = Mathf.Max(entry2.minHeight, m_ChildMinHeight);
					m_ChildMaxHeight = Mathf.Max(entry2.maxHeight, m_ChildMaxHeight);
				}
				m_StretchableCountY += entry2.stretchHeight;
			}
		}
		float num3;
		float num4;
		if (base.style != GUIStyle.none || m_UserSpecifiedHeight)
		{
			num3 = Mathf.Max(base.style.padding.top, b);
			num4 = Mathf.Max(base.style.padding.bottom, b2);
		}
		else
		{
			m_MarginTop = b;
			m_MarginBottom = b2;
			num3 = (num4 = 0f);
		}
		minHeight = Mathf.Max(minHeight, m_ChildMinHeight + num3 + num4);
		if (maxHeight == 0f)
		{
			stretchHeight += m_StretchableCountY + (base.style.stretchHeight ? 1 : 0);
			maxHeight = m_ChildMaxHeight + num3 + num4;
		}
		else
		{
			stretchHeight = 0;
		}
		maxHeight = Mathf.Max(maxHeight, minHeight);
		if (base.style.fixedHeight != 0f)
		{
			maxHeight = (minHeight = base.style.fixedHeight);
			stretchHeight = 0;
		}
	}

	public override void SetVertical(float y, float height)
	{
		base.SetVertical(y, height);
		if (entries.Count == 0)
		{
			return;
		}
		RectOffset padding = base.style.padding;
		if (resetCoords)
		{
			y = 0f;
		}
		if (isVertical)
		{
			if (base.style != GUIStyle.none)
			{
				float num = Mathf.Max(padding.top, entries[0].marginTop);
				int bottom = padding.bottom;
				List<GUILayoutEntry> list = entries;
				float num2 = Mathf.Max(bottom, list[list.Count - 1].marginBottom);
				y += num;
				height -= num2 + num;
			}
			float num3 = height - spacing * (float)(entries.Count - 1);
			float t = 0f;
			if (m_ChildMinHeight != m_ChildMaxHeight)
			{
				t = Mathf.Clamp((num3 - m_ChildMinHeight) / (m_ChildMaxHeight - m_ChildMinHeight), 0f, 1f);
			}
			float num4 = 0f;
			if (num3 > m_ChildMaxHeight && m_StretchableCountY > 0)
			{
				num4 = (num3 - m_ChildMaxHeight) / (float)m_StretchableCountY;
			}
			int num5 = 0;
			bool flag = true;
			{
				foreach (GUILayoutEntry entry in entries)
				{
					float num6 = Mathf.Lerp(entry.minHeight, entry.maxHeight, t) + num4 * (float)entry.stretchHeight;
					if (entry.consideredForMargin)
					{
						int num7 = entry.marginTop;
						if (flag)
						{
							num7 = 0;
							flag = false;
						}
						y += (float)((num5 > num7) ? num5 : num7);
						num5 = entry.marginBottom;
					}
					entry.SetVertical(Mathf.Round(y), Mathf.Round(num6));
					y += num6 + spacing;
				}
				return;
			}
		}
		if (base.style != GUIStyle.none)
		{
			foreach (GUILayoutEntry entry2 in entries)
			{
				float num8 = Mathf.Max(entry2.marginTop, padding.top);
				float num9 = height - (float)Mathf.Max(entry2.marginBottom, padding.bottom) - num8;
				entry2.SetVertical(y + num8, (entry2.stretchHeight != 0) ? num9 : Mathf.Clamp(num9, entry2.minHeight, entry2.maxHeight));
			}
			return;
		}
		float num10 = y - (float)marginTop;
		float num11 = height + (float)base.marginVertical;
		foreach (GUILayoutEntry entry3 in entries)
		{
			float num12 = num11 - (float)entry3.marginVertical;
			entry3.SetVertical(num10 + (float)entry3.marginTop, (entry3.stretchHeight != 0) ? num12 : Mathf.Clamp(num12, entry3.minHeight, entry3.maxHeight));
		}
	}
}
