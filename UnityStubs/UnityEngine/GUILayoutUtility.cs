using System;
using System.Collections.Generic;

namespace UnityEngine;

public class GUILayoutUtility
{
	internal sealed class LayoutCache
	{
		internal GUILayoutGroup topLevel = new GUILayoutGroup();

		internal Stack<GUILayoutGroup> layoutGroups = new Stack<GUILayoutGroup>();

		internal GUILayoutGroup windows = new GUILayoutGroup();

		internal int id { get; private set; }

		internal LayoutCache(int instanceID = -1)
		{
			id = instanceID;
			layoutGroups.Push(topLevel);
		}
	}

	private static readonly Dictionary<int, LayoutCache> s_StoredLayouts = new Dictionary<int, LayoutCache>();

	private static readonly Dictionary<int, LayoutCache> s_StoredWindows = new Dictionary<int, LayoutCache>();

	internal static LayoutCache current = new LayoutCache();

	internal static readonly Rect kDummyRect = new Rect(0f, 0f, 1f, 1f);

	private static GUIStyle s_SpaceStyle;

	internal static GUILayoutGroup topLevel => current.topLevel;

	internal static GUIStyle spaceStyle
	{
		get
		{
			if (s_SpaceStyle == null)
			{
				s_SpaceStyle = new GUIStyle();
			}
			s_SpaceStyle.stretchWidth = false;
			return s_SpaceStyle;
		}
	}

	public static void BeginBehaviourGUI(int instanceID)
	{
		Begin(instanceID);
	}

	public static void EndBehaviourGUI()
	{
		Event @event = Event.current;
		if (@event != null && @event.type == EventType.layout)
		{
			Layout();
		}
	}

	internal static LayoutCache SelectIDList(int instanceID, bool isWindow)
	{
		Dictionary<int, LayoutCache> dictionary = (isWindow ? s_StoredWindows : s_StoredLayouts);
		if (!dictionary.TryGetValue(instanceID, out var value))
		{
			value = (dictionary[instanceID] = new LayoutCache(instanceID));
		}
		current.topLevel = value.topLevel;
		current.layoutGroups = value.layoutGroups;
		current.windows = value.windows;
		return value;
	}

	internal static void Begin(int instanceID)
	{
		BeginCache(SelectIDList(instanceID, isWindow: false), null, -1, null);
	}

	internal static void BeginWindow(int windowID, GUIStyle style, GUILayoutOption[] options)
	{
		BeginCache(SelectIDList(windowID, isWindow: true), style, windowID, options);
	}

	private static void BeginCache(LayoutCache cache, GUIStyle style, int windowID, GUILayoutOption[] options)
	{
		if (Event.current.type == EventType.layout)
		{
			current.topLevel = (cache.topLevel = new GUILayoutGroup());
			if (style != null)
			{
				current.topLevel.style = style;
			}
			current.topLevel.windowID = windowID;
			if (options != null)
			{
				current.topLevel.ApplyOptions(options);
			}
			current.layoutGroups.Clear();
			current.layoutGroups.Push(current.topLevel);
			current.windows = (cache.windows = new GUILayoutGroup());
		}
		else
		{
			current.topLevel = cache.topLevel;
			current.layoutGroups = cache.layoutGroups;
			current.windows = cache.windows;
			cache.topLevel.ResetCursor();
			cache.windows.ResetCursor();
		}
	}

	internal static void Layout()
	{
		if (current.topLevel.windowID == -1)
		{
			current.topLevel.CalcWidth();
			current.topLevel.SetHorizontal(0f, Mathf.Min(Screen.width, current.topLevel.maxWidth));
			current.topLevel.CalcHeight();
			current.topLevel.SetVertical(0f, Mathf.Min(Screen.height, current.topLevel.maxHeight));
		}
		else
		{
			LayoutSingleGroup(current.topLevel);
		}
		LayoutFreeGroup(current.windows);
	}

	internal static void LayoutFreeGroup(GUILayoutGroup toplevel)
	{
		foreach (GUILayoutGroup entry in toplevel.entries)
		{
			LayoutSingleGroup(entry);
		}
		toplevel.ResetCursor();
	}

	private static void LayoutSingleGroup(GUILayoutGroup group)
	{
		float minWidth = group.minWidth;
		float maxWidth = group.maxWidth;
		group.CalcWidth();
		group.SetHorizontal(group.rect.x, Mathf.Clamp(group.maxWidth, minWidth, maxWidth));
		float minHeight = group.minHeight;
		float maxHeight = group.maxHeight;
		group.CalcHeight();
		group.SetVertical(group.rect.y, Mathf.Clamp(group.maxHeight, minHeight, maxHeight));
	}

	internal static GUILayoutGroup BeginLayoutGroup(GUIStyle style, GUILayoutOption[] options, Type layoutType)
	{
		EventType type = Event.current.type;
		GUILayoutGroup gUILayoutGroup;
		if ((type == EventType.layout || type == EventType.used) ? true : false)
		{
			gUILayoutGroup = (GUILayoutGroup)Activator.CreateInstance(layoutType);
			gUILayoutGroup.style = style;
			if (options != null)
			{
				gUILayoutGroup.ApplyOptions(options);
			}
			current.topLevel.Add(gUILayoutGroup);
		}
		else
		{
			gUILayoutGroup = (current.topLevel.GetNext() as GUILayoutGroup) ?? throw new ExitGUIException("GUILayout: Mismatched LayoutGroup." + Event.current.type);
			gUILayoutGroup.ResetCursor();
		}
		current.layoutGroups.Push(gUILayoutGroup);
		current.topLevel = gUILayoutGroup;
		return gUILayoutGroup;
	}

	internal static void EndLayoutGroup()
	{
		if (current.layoutGroups.Count == 0)
		{
			Debug.LogError("EndLayoutGroup: BeginLayoutGroup must be called first.");
			return;
		}
		current.layoutGroups.Pop();
		current.topLevel = ((current.layoutGroups.Count > 0) ? current.layoutGroups.Peek() : new GUILayoutGroup());
	}

	internal static GUILayoutGroup BeginLayoutArea(GUIStyle style, Type layoutType)
	{
		EventType type = Event.current.type;
		GUILayoutGroup gUILayoutGroup;
		if ((type == EventType.layout || type == EventType.used) ? true : false)
		{
			gUILayoutGroup = (GUILayoutGroup)Activator.CreateInstance(layoutType);
			gUILayoutGroup.style = style;
			current.windows.Add(gUILayoutGroup);
		}
		else
		{
			gUILayoutGroup = (current.windows.GetNext() as GUILayoutGroup) ?? throw new ExitGUIException("GUILayout: Mismatched LayoutGroup." + Event.current.type);
			gUILayoutGroup.ResetCursor();
		}
		current.layoutGroups.Push(gUILayoutGroup);
		current.topLevel = gUILayoutGroup;
		return gUILayoutGroup;
	}

	internal static void EndLayoutArea()
	{
		current.layoutGroups.Pop();
		current.topLevel = current.layoutGroups.Peek();
	}

	public static Rect GetRect(GUIContent content, GUIStyle style)
	{
		return DoGetRect(content, style, null);
	}

	public static Rect GetRect(GUIContent content, GUIStyle style, params GUILayoutOption[] options)
	{
		return DoGetRect(content, style, options);
	}

	private static Rect DoGetRect(GUIContent content, GUIStyle style, GUILayoutOption[] options)
	{
		switch (Event.current.type)
		{
		case EventType.layout:
			if (style.isHeightDependantOnWidth)
			{
				current.topLevel.Add(new GUIWordWrapSizer(style, content, options));
			}
			else
			{
				Vector2 vector = CalcSizeWithConstraints(style, content, options);
				current.topLevel.Add(new GUILayoutEntry(vector.x, vector.x, vector.y, vector.y, style, options));
			}
			return kDummyRect;
		case EventType.used:
			return kDummyRect;
		default:
			return current.topLevel.GetNext().rect;
		}
	}

	private static Vector2 CalcSizeWithConstraints(GUIStyle style, GUIContent content, GUILayoutOption[] options)
	{
		Vector2 vector = style.CalcSize(content);
		if (style.fixedWidth != 0f)
		{
			vector.x = style.fixedWidth;
		}
		if (style.fixedHeight != 0f)
		{
			vector.y = style.fixedHeight;
		}
		if (options != null)
		{
			foreach (GUILayoutOption gUILayoutOption in options)
			{
				if (gUILayoutOption.type == GUILayoutOption.Type.maxWidth)
				{
					vector.x = Mathf.Min(vector.x, Convert.ToSingle(gUILayoutOption.value));
				}
				else if (gUILayoutOption.type == GUILayoutOption.Type.maxHeight)
				{
					vector.y = Mathf.Min(vector.y, Convert.ToSingle(gUILayoutOption.value));
				}
			}
		}
		return new Vector2(Mathf.Ceil(vector.x), Mathf.Ceil(vector.y));
	}

	public static Rect GetRect(float width, float height)
	{
		return DoGetRect(width, width, height, height, GUIStyle.none, null);
	}

	public static Rect GetRect(float width, float height, GUIStyle style)
	{
		return DoGetRect(width, width, height, height, style, null);
	}

	public static Rect GetRect(float width, float height, params GUILayoutOption[] options)
	{
		return DoGetRect(width, width, height, height, GUIStyle.none, options);
	}

	public static Rect GetRect(float width, float height, GUIStyle style, params GUILayoutOption[] options)
	{
		return DoGetRect(width, width, height, height, style, options);
	}

	public static Rect GetRect(float minWidth, float maxWidth, float minHeight, float maxHeight)
	{
		return DoGetRect(minWidth, maxWidth, minHeight, maxHeight, GUIStyle.none, null);
	}

	public static Rect GetRect(float minWidth, float maxWidth, float minHeight, float maxHeight, GUIStyle style)
	{
		return DoGetRect(minWidth, maxWidth, minHeight, maxHeight, style, null);
	}

	public static Rect GetRect(float minWidth, float maxWidth, float minHeight, float maxHeight, params GUILayoutOption[] options)
	{
		return DoGetRect(minWidth, maxWidth, minHeight, maxHeight, GUIStyle.none, options);
	}

	public static Rect GetRect(float minWidth, float maxWidth, float minHeight, float maxHeight, GUIStyle style, params GUILayoutOption[] options)
	{
		return DoGetRect(minWidth, maxWidth, minHeight, maxHeight, style, options);
	}

	private static Rect DoGetRect(float minWidth, float maxWidth, float minHeight, float maxHeight, GUIStyle style, GUILayoutOption[] options)
	{
		switch (Event.current.type)
		{
		case EventType.layout:
			current.topLevel.Add(new GUILayoutEntry(minWidth, maxWidth, minHeight, maxHeight, style, options));
			return kDummyRect;
		case EventType.used:
			return kDummyRect;
		default:
			return current.topLevel.GetNext().rect;
		}
	}

	public static Rect GetLastRect()
	{
		EventType type = Event.current.type;
		if ((type != EventType.layout && type != EventType.used) || 1 == 0)
		{
			return current.topLevel.GetLast();
		}
		return kDummyRect;
	}

	public static Rect GetAspectRect(float aspect)
	{
		return DoGetAspectRect(aspect, null);
	}

	public static Rect GetAspectRect(float aspect, GUIStyle style)
	{
		return DoGetAspectRect(aspect, null);
	}

	public static Rect GetAspectRect(float aspect, params GUILayoutOption[] options)
	{
		return DoGetAspectRect(aspect, options);
	}

	public static Rect GetAspectRect(float aspect, GUIStyle style, params GUILayoutOption[] options)
	{
		return DoGetAspectRect(aspect, options);
	}

	private static Rect DoGetAspectRect(float aspect, GUILayoutOption[] options)
	{
		switch (Event.current.type)
		{
		case EventType.layout:
			current.topLevel.Add(new GUIAspectSizer(aspect, options));
			return kDummyRect;
		case EventType.used:
			return kDummyRect;
		default:
			return current.topLevel.GetNext().rect;
		}
	}
}
