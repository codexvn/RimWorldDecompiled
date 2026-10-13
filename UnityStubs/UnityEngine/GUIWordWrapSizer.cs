namespace UnityEngine;

internal sealed class GUIWordWrapSizer : GUILayoutEntry
{
	private readonly GUIContent m_Content;

	private readonly float m_ForcedMinHeight;

	private readonly float m_ForcedMaxHeight;

	public GUIWordWrapSizer(GUIStyle style, GUIContent content, GUILayoutOption[] options)
		: base(0f, 0f, 0f, 0f, style)
	{
		m_Content = new GUIContent(content);
		ApplyOptions(options);
		m_ForcedMinHeight = minHeight;
		m_ForcedMaxHeight = maxHeight;
	}

	public override void CalcWidth()
	{
		if (minWidth == 0f || maxWidth == 0f)
		{
			base.style.CalcMinMaxWidth(m_Content, out var f, out var fixedWidth);
			if (base.style.fixedWidth != 0f)
			{
				f = (fixedWidth = base.style.fixedWidth);
			}
			if (minWidth == 0f)
			{
				minWidth = Mathf.Ceil(f);
			}
			if (maxWidth == 0f)
			{
				maxWidth = Mathf.Ceil(fixedWidth);
			}
		}
	}

	public override void CalcHeight()
	{
		if (m_ForcedMinHeight == 0f || m_ForcedMaxHeight == 0f)
		{
			float num = base.style.CalcHeight(m_Content, rect.width);
			minHeight = ((m_ForcedMinHeight == 0f) ? num : m_ForcedMinHeight);
			maxHeight = ((m_ForcedMaxHeight == 0f) ? num : m_ForcedMaxHeight);
		}
	}
}
