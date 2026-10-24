using UnityStubs.Gui;

namespace UnityEngine;

internal sealed class GUIClip
{
	private static readonly Rect RootRect = new Rect(-10000f, -10000f, 40000f, 40000f);

	internal static bool enabled
	{
		get
		{
			IGuiBackend current = GuiBackend.Current;
			if (current == null)
			{
				return false;
			}
			return current.GroupCount > 0;
		}
	}

	internal static Rect topmostRect
	{
		get
		{
			IGuiBackend current = GuiBackend.Current;
			if (current == null || current.GroupCount == 0)
			{
				return new Rect(0f, 0f, Screen.width, Screen.height);
			}
			current.GetGroup(current.GroupCount - 1, out var rect, out var contentOrigin);
			return new Rect(contentOrigin, rect.size);
		}
	}

	internal static Rect GetTopRect()
	{
		IGuiBackend current = GuiBackend.Current;
		if (current == null || current.GroupCount == 0)
		{
			return RootRect;
		}
		current.GetGroup(current.GroupCount - 1, out var rect, out var _);
		return rect;
	}

	internal static Rect Clip(Rect absoluteRect)
	{
		Rect rect = topmostRect;
		Rect rect2 = new Rect(absoluteRect.position - rect.position, absoluteRect.size);
		return Rect.MinMaxRect(Mathf.Max(rect2.xMin, 0f), Mathf.Max(rect2.yMin, 0f), Mathf.Min(rect2.xMax, rect.width), Mathf.Min(rect2.yMax, rect.height));
	}

	internal static Vector2 Unclip(Vector2 pos)
	{
		return pos + topmostRect.position;
	}

	internal static void Internal_Push(Rect screenRect, Vector2 scrollOffset, Vector2 renderOffset, bool resetOffset)
	{
		if (scrollOffset == Vector2.zero)
		{
			GuiBackend.Current?.PushGroup(screenRect);
		}
		else
		{
			GuiBackend.Current?.PushScrollView(screenRect, screenRect.position + scrollOffset);
		}
	}

	internal static void Internal_Pop()
	{
		GuiBackend.Current?.PopGroup();
	}

	internal static void Push(Rect screenRect, Vector2 scrollOffset, Vector2 renderOffset, bool resetOffset)
	{
		Internal_Push(screenRect, scrollOffset, renderOffset, resetOffset);
	}

	internal static void Pop()
	{
		Internal_Pop();
	}
}
