using System.Collections.Generic;

namespace UnityEngine;

public class GUILayout
{
	public class HorizontalScope : GUI.Scope
	{
		public HorizontalScope(params GUILayoutOption[] options)
		{
			BeginHorizontal(options);
		}

		public HorizontalScope(GUIStyle style, params GUILayoutOption[] options)
		{
			BeginHorizontal(style, options);
		}

		public HorizontalScope(string text, GUIStyle style, params GUILayoutOption[] options)
		{
			BeginHorizontal(text, style, options);
		}

		public HorizontalScope(Texture image, GUIStyle style, params GUILayoutOption[] options)
		{
			BeginHorizontal(image, style, options);
		}

		public HorizontalScope(GUIContent content, GUIStyle style, params GUILayoutOption[] options)
		{
			BeginHorizontal(content, style, options);
		}

		protected override void CloseScope()
		{
			EndHorizontal();
		}
	}

	public class VerticalScope : GUI.Scope
	{
		public VerticalScope(params GUILayoutOption[] options)
		{
			BeginVertical(options);
		}

		public VerticalScope(GUIStyle style, params GUILayoutOption[] options)
		{
			BeginVertical(style, options);
		}

		public VerticalScope(string text, GUIStyle style, params GUILayoutOption[] options)
		{
			BeginVertical(text, style, options);
		}

		public VerticalScope(Texture image, GUIStyle style, params GUILayoutOption[] options)
		{
			BeginVertical(image, style, options);
		}

		public VerticalScope(GUIContent content, GUIStyle style, params GUILayoutOption[] options)
		{
			BeginVertical(content, style, options);
		}

		protected override void CloseScope()
		{
			EndVertical();
		}
	}

	public class AreaScope : GUI.Scope
	{
		public AreaScope(Rect screenRect)
		{
			BeginArea(screenRect);
		}

		public AreaScope(Rect screenRect, string text)
		{
			BeginArea(screenRect, text);
		}

		public AreaScope(Rect screenRect, Texture image)
		{
			BeginArea(screenRect, image);
		}

		public AreaScope(Rect screenRect, GUIContent content)
		{
			BeginArea(screenRect, content);
		}

		public AreaScope(Rect screenRect, string text, GUIStyle style)
		{
			BeginArea(screenRect, text, style);
		}

		public AreaScope(Rect screenRect, Texture image, GUIStyle style)
		{
			BeginArea(screenRect, image, style);
		}

		public AreaScope(Rect screenRect, GUIContent content, GUIStyle style)
		{
			BeginArea(screenRect, content, style);
		}

		protected override void CloseScope()
		{
			EndArea();
		}
	}

	public class ScrollViewScope : GUI.Scope
	{
		public Vector2 scrollPosition { get; private set; }

		public bool handleScrollWheel { get; set; } = true;

		public ScrollViewScope(Vector2 scrollPosition, params GUILayoutOption[] options)
		{
			this.scrollPosition = BeginScrollView(scrollPosition, options);
		}

		public ScrollViewScope(Vector2 scrollPosition, bool alwaysShowHorizontal, bool alwaysShowVertical, params GUILayoutOption[] options)
		{
			this.scrollPosition = BeginScrollView(scrollPosition, alwaysShowHorizontal, alwaysShowVertical, options);
		}

		public ScrollViewScope(Vector2 scrollPosition, GUIStyle horizontalScrollbar, GUIStyle verticalScrollbar, params GUILayoutOption[] options)
		{
			this.scrollPosition = BeginScrollView(scrollPosition, horizontalScrollbar, verticalScrollbar, options);
		}

		public ScrollViewScope(Vector2 scrollPosition, GUIStyle style, params GUILayoutOption[] options)
		{
			this.scrollPosition = BeginScrollView(scrollPosition, style, options);
		}

		public ScrollViewScope(Vector2 scrollPosition, bool alwaysShowHorizontal, bool alwaysShowVertical, GUIStyle horizontalScrollbar, GUIStyle verticalScrollbar, params GUILayoutOption[] options)
		{
			this.scrollPosition = BeginScrollView(scrollPosition, alwaysShowHorizontal, alwaysShowVertical, horizontalScrollbar, verticalScrollbar, options);
		}

		public ScrollViewScope(Vector2 scrollPosition, bool alwaysShowHorizontal, bool alwaysShowVertical, GUIStyle horizontalScrollbar, GUIStyle verticalScrollbar, GUIStyle background, params GUILayoutOption[] options)
		{
			this.scrollPosition = BeginScrollView(scrollPosition, alwaysShowHorizontal, alwaysShowVertical, horizontalScrollbar, verticalScrollbar, background, options);
		}

		protected override void CloseScope()
		{
			EndScrollView(handleScrollWheel);
		}
	}

	public static void Label(Texture image, params GUILayoutOption[] options)
	{
		DoLabel(GUI.TempContent("", image), GUI.skin.label, options);
	}

	public static void Label(string text, params GUILayoutOption[] options)
	{
		DoLabel(GUI.TempContent(text, null), GUI.skin.label, options);
	}

	public static void Label(GUIContent content, params GUILayoutOption[] options)
	{
		DoLabel(content, GUI.skin.label, options);
	}

	public static void Label(Texture image, GUIStyle style, params GUILayoutOption[] options)
	{
		DoLabel(GUI.TempContent("", image), style, options);
	}

	public static void Label(string text, GUIStyle style, params GUILayoutOption[] options)
	{
		DoLabel(GUI.TempContent(text, null), style, options);
	}

	public static void Label(GUIContent content, GUIStyle style, params GUILayoutOption[] options)
	{
		DoLabel(content, style, options);
	}

	private static void DoLabel(GUIContent content, GUIStyle style, GUILayoutOption[] options)
	{
		GUI.Label(GUILayoutUtility.GetRect(content, style, options), content, style);
	}

	public static void Box(Texture image, params GUILayoutOption[] options)
	{
		DoBox(GUI.TempContent("", image), GUI.skin.box, options);
	}

	public static void Box(string text, params GUILayoutOption[] options)
	{
		DoBox(GUI.TempContent(text, null), GUI.skin.box, options);
	}

	public static void Box(GUIContent content, params GUILayoutOption[] options)
	{
		DoBox(content, GUI.skin.box, options);
	}

	public static void Box(Texture image, GUIStyle style, params GUILayoutOption[] options)
	{
		DoBox(GUI.TempContent("", image), style, options);
	}

	public static void Box(string text, GUIStyle style, params GUILayoutOption[] options)
	{
		DoBox(GUI.TempContent(text, null), style, options);
	}

	public static void Box(GUIContent content, GUIStyle style, params GUILayoutOption[] options)
	{
		DoBox(content, style, options);
	}

	private static void DoBox(GUIContent content, GUIStyle style, GUILayoutOption[] options)
	{
		GUI.Box(GUILayoutUtility.GetRect(content, style, options), content, style);
	}

	public static bool Button(Texture image, params GUILayoutOption[] options)
	{
		return DoButton(GUI.TempContent("", image), GUI.skin.button, options);
	}

	public static bool Button(string text, params GUILayoutOption[] options)
	{
		return DoButton(GUI.TempContent(text, null), GUI.skin.button, options);
	}

	public static bool Button(GUIContent content, params GUILayoutOption[] options)
	{
		return DoButton(content, GUI.skin.button, options);
	}

	public static bool Button(Texture image, GUIStyle style, params GUILayoutOption[] options)
	{
		return DoButton(GUI.TempContent("", image), style, options);
	}

	public static bool Button(string text, GUIStyle style, params GUILayoutOption[] options)
	{
		return DoButton(GUI.TempContent(text, null), style, options);
	}

	public static bool Button(GUIContent content, GUIStyle style, params GUILayoutOption[] options)
	{
		return DoButton(content, style, options);
	}

	private static bool DoButton(GUIContent content, GUIStyle style, GUILayoutOption[] options)
	{
		return GUI.Button(GUILayoutUtility.GetRect(content, style, options), content, style);
	}

	public static string TextField(string text, params GUILayoutOption[] options)
	{
		return DoTextField(text, -1, multiline: false, GUI.skin.textField, options);
	}

	public static string TextField(string text, int maxLength, params GUILayoutOption[] options)
	{
		return DoTextField(text, maxLength, multiline: false, GUI.skin.textField, options);
	}

	public static string TextField(string text, GUIStyle style, params GUILayoutOption[] options)
	{
		return DoTextField(text, -1, multiline: false, style, options);
	}

	public static string TextField(string text, int maxLength, GUIStyle style, params GUILayoutOption[] options)
	{
		return DoTextField(text, maxLength, multiline: false, style, options);
	}

	public static string TextArea(string text, params GUILayoutOption[] options)
	{
		return DoTextField(text, -1, multiline: true, GUI.skin.textArea, options);
	}

	public static string TextArea(string text, int maxLength, params GUILayoutOption[] options)
	{
		return DoTextField(text, maxLength, multiline: true, GUI.skin.textArea, options);
	}

	public static string TextArea(string text, GUIStyle style, params GUILayoutOption[] options)
	{
		return DoTextField(text, -1, multiline: true, style, options);
	}

	public static string TextArea(string text, int maxLength, GUIStyle style, params GUILayoutOption[] options)
	{
		return DoTextField(text, maxLength, multiline: true, style, options);
	}

	private static string DoTextField(string text, int maxLength, bool multiline, GUIStyle style, GUILayoutOption[] options)
	{
		Rect rect = GUILayoutUtility.GetRect(GUI.TempContent(text, null), style, options);
		if (!multiline)
		{
			return GUI.TextField(rect, text, maxLength, style);
		}
		return GUI.TextArea(rect, text, maxLength, style);
	}

	public static string PasswordField(string password, char maskChar, params GUILayoutOption[] options)
	{
		return DoPasswordField(password, maskChar, -1, GUI.skin.textField, options);
	}

	public static string PasswordField(string password, char maskChar, int maxLength, params GUILayoutOption[] options)
	{
		return DoPasswordField(password, maskChar, maxLength, GUI.skin.textField, options);
	}

	public static string PasswordField(string password, char maskChar, GUIStyle style, params GUILayoutOption[] options)
	{
		return DoPasswordField(password, maskChar, -1, style, options);
	}

	public static string PasswordField(string password, char maskChar, int maxLength, GUIStyle style, params GUILayoutOption[] options)
	{
		return DoPasswordField(password, maskChar, maxLength, style, options);
	}

	private static string DoPasswordField(string password, char maskChar, int maxLength, GUIStyle style, GUILayoutOption[] options)
	{
		string text = GUI.PasswordField(GUILayoutUtility.GetRect(GUI.TempContent(new string(maskChar, password?.Length ?? 0), null), GUI.skin.textField, options), password, maskChar, style);
		if (maxLength < 0 || text == null || text.Length <= maxLength)
		{
			return text;
		}
		return text.Substring(0, maxLength);
	}

	public static bool Toggle(bool value, Texture image, params GUILayoutOption[] options)
	{
		return DoToggle(value, GUI.TempContent("", image), GUI.skin.toggle, options);
	}

	public static bool Toggle(bool value, string text, params GUILayoutOption[] options)
	{
		return DoToggle(value, GUI.TempContent(text, null), GUI.skin.toggle, options);
	}

	public static bool Toggle(bool value, GUIContent content, params GUILayoutOption[] options)
	{
		return DoToggle(value, content, GUI.skin.toggle, options);
	}

	public static bool Toggle(bool value, Texture image, GUIStyle style, params GUILayoutOption[] options)
	{
		return DoToggle(value, GUI.TempContent("", image), style, options);
	}

	public static bool Toggle(bool value, string text, GUIStyle style, params GUILayoutOption[] options)
	{
		return DoToggle(value, GUI.TempContent(text, null), style, options);
	}

	public static bool Toggle(bool value, GUIContent content, GUIStyle style, params GUILayoutOption[] options)
	{
		return DoToggle(value, content, style, options);
	}

	private static bool DoToggle(bool value, GUIContent content, GUIStyle style, GUILayoutOption[] options)
	{
		return GUI.Toggle(GUILayoutUtility.GetRect(content, style, options), value, content, style);
	}

	public static float HorizontalSlider(float value, float leftValue, float rightValue, params GUILayoutOption[] options)
	{
		return HorizontalSlider(value, leftValue, rightValue, GUI.skin.horizontalSlider, GUI.skin.horizontalSliderThumb, options);
	}

	public static float HorizontalSlider(float value, float leftValue, float rightValue, GUIStyle slider, GUIStyle thumb, params GUILayoutOption[] options)
	{
		return GUI.HorizontalSlider(GUILayoutUtility.GetRect(GUI.TempContent("mmmm", null), slider, options), value, leftValue, rightValue);
	}

	public static float VerticalSlider(float value, float leftValue, float rightValue, params GUILayoutOption[] options)
	{
		return VerticalSlider(value, leftValue, rightValue, GUI.skin.verticalSlider, GUI.skin.verticalSliderThumb, options);
	}

	public static float VerticalSlider(float value, float leftValue, float rightValue, GUIStyle slider, GUIStyle thumb, params GUILayoutOption[] options)
	{
		return GUI.VerticalSlider(GUILayoutUtility.GetRect(GUI.TempContent("\n\n\n\n\n", null), slider, options), value, leftValue, rightValue);
	}

	public static float HorizontalScrollbar(float value, float size, float leftValue, float rightValue, params GUILayoutOption[] options)
	{
		return HorizontalScrollbar(value, size, leftValue, rightValue, GUI.skin.horizontalScrollbar, options);
	}

	public static float HorizontalScrollbar(float value, float size, float leftValue, float rightValue, GUIStyle style, params GUILayoutOption[] options)
	{
		return GUI.HorizontalScrollbar(GUILayoutUtility.GetRect(GUI.TempContent("mmmm", null), style, options), value, size, leftValue, rightValue, style);
	}

	public static float VerticalScrollbar(float value, float size, float topValue, float bottomValue, params GUILayoutOption[] options)
	{
		return VerticalScrollbar(value, size, topValue, bottomValue, GUI.skin.verticalScrollbar, options);
	}

	public static float VerticalScrollbar(float value, float size, float topValue, float bottomValue, GUIStyle style, params GUILayoutOption[] options)
	{
		return GUI.VerticalScrollbar(GUILayoutUtility.GetRect(GUI.TempContent("\n\n\n\n\n", null), style, options), value, size, topValue, bottomValue, style);
	}

	public static void Space(float pixels)
	{
		if (GUILayoutUtility.current.topLevel.isVertical)
		{
			GUILayoutUtility.GetRect(0f, pixels, GUILayoutUtility.spaceStyle, Height(pixels));
		}
		else
		{
			GUILayoutUtility.GetRect(pixels, 0f, GUILayoutUtility.spaceStyle, Width(pixels));
		}
		if (Event.current.type == EventType.layout)
		{
			List<GUILayoutEntry> entries = GUILayoutUtility.current.topLevel.entries;
			entries[entries.Count - 1].consideredForMargin = false;
		}
	}

	public static void FlexibleSpace()
	{
		GUILayoutOption.Type type = (GUILayoutUtility.current.topLevel.isVertical ? GUILayoutOption.Type.stretchHeight : GUILayoutOption.Type.stretchWidth);
		GUILayoutUtility.GetRect(0f, 0f, GUILayoutUtility.spaceStyle, new GUILayoutOption(type, 10000));
		if (Event.current.type == EventType.layout)
		{
			List<GUILayoutEntry> entries = GUILayoutUtility.current.topLevel.entries;
			entries[entries.Count - 1].consideredForMargin = false;
		}
	}

	public static void BeginHorizontal(params GUILayoutOption[] options)
	{
		BeginHorizontal(GUIContent.none, GUIStyle.none, options);
	}

	public static void BeginHorizontal(GUIStyle style, params GUILayoutOption[] options)
	{
		BeginHorizontal(GUIContent.none, style, options);
	}

	public static void BeginHorizontal(string text, GUIStyle style, params GUILayoutOption[] options)
	{
		BeginHorizontal(GUI.TempContent(text, null), style, options);
	}

	public static void BeginHorizontal(Texture image, GUIStyle style, params GUILayoutOption[] options)
	{
		BeginHorizontal(GUI.TempContent("", image), style, options);
	}

	public static void BeginHorizontal(GUIContent content, GUIStyle style, params GUILayoutOption[] options)
	{
		BeginGroup(content, style, options, vertical: false);
	}

	public static void EndHorizontal()
	{
		GUILayoutUtility.EndLayoutGroup();
	}

	public static void BeginVertical(params GUILayoutOption[] options)
	{
		BeginVertical(GUIContent.none, GUIStyle.none, options);
	}

	public static void BeginVertical(GUIStyle style, params GUILayoutOption[] options)
	{
		BeginVertical(GUIContent.none, style, options);
	}

	public static void BeginVertical(string text, GUIStyle style, params GUILayoutOption[] options)
	{
		BeginVertical(GUI.TempContent(text, null), style, options);
	}

	public static void BeginVertical(Texture image, GUIStyle style, params GUILayoutOption[] options)
	{
		BeginVertical(GUI.TempContent("", image), style, options);
	}

	public static void BeginVertical(GUIContent content, GUIStyle style, params GUILayoutOption[] options)
	{
		BeginGroup(content, style, options, vertical: true);
	}

	public static void EndVertical()
	{
		GUILayoutUtility.EndLayoutGroup();
	}

	private static void BeginGroup(GUIContent content, GUIStyle style, GUILayoutOption[] options, bool vertical)
	{
		GUILayoutGroup gUILayoutGroup = GUILayoutUtility.BeginLayoutGroup(style, options, typeof(GUILayoutGroup));
		gUILayoutGroup.isVertical = vertical;
		if (style != GUIStyle.none || content != GUIContent.none)
		{
			GUI.Box(gUILayoutGroup.rect, content, style);
		}
	}

	public static void BeginArea(Rect screenRect)
	{
		BeginArea(screenRect, GUIContent.none, GUIStyle.none);
	}

	public static void BeginArea(Rect screenRect, string text)
	{
		BeginArea(screenRect, GUI.TempContent(text, null), GUIStyle.none);
	}

	public static void BeginArea(Rect screenRect, Texture image)
	{
		BeginArea(screenRect, GUI.TempContent("", image), GUIStyle.none);
	}

	public static void BeginArea(Rect screenRect, GUIContent content)
	{
		BeginArea(screenRect, content, GUIStyle.none);
	}

	public static void BeginArea(Rect screenRect, GUIStyle style)
	{
		BeginArea(screenRect, GUIContent.none, style);
	}

	public static void BeginArea(Rect screenRect, string text, GUIStyle style)
	{
		BeginArea(screenRect, GUI.TempContent(text, null), style);
	}

	public static void BeginArea(Rect screenRect, Texture image, GUIStyle style)
	{
		BeginArea(screenRect, GUI.TempContent("", image), style);
	}

	public static void BeginArea(Rect screenRect, GUIContent content, GUIStyle style)
	{
		GUILayoutGroup gUILayoutGroup = GUILayoutUtility.BeginLayoutArea(style, typeof(GUILayoutGroup));
		if (Event.current.type == EventType.layout)
		{
			gUILayoutGroup.resetCoords = true;
			gUILayoutGroup.minWidth = (gUILayoutGroup.maxWidth = screenRect.width);
			gUILayoutGroup.minHeight = (gUILayoutGroup.maxHeight = screenRect.height);
			gUILayoutGroup.rect = Rect.MinMaxRect(screenRect.xMin, screenRect.yMin, gUILayoutGroup.rect.xMax, gUILayoutGroup.rect.yMax);
		}
		GUI.BeginGroup(gUILayoutGroup.rect, content, style);
	}

	public static void EndArea()
	{
		GUILayoutUtility.EndLayoutArea();
		GUI.EndGroup();
	}

	public static Vector2 BeginScrollView(Vector2 scrollPosition, params GUILayoutOption[] options)
	{
		return BeginScrollView(scrollPosition, alwaysShowHorizontal: false, alwaysShowVertical: false, GUI.skin.horizontalScrollbar, GUI.skin.verticalScrollbar, GUI.skin.scrollView, options);
	}

	public static Vector2 BeginScrollView(Vector2 scrollPosition, bool alwaysShowHorizontal, bool alwaysShowVertical, params GUILayoutOption[] options)
	{
		return BeginScrollView(scrollPosition, alwaysShowHorizontal, alwaysShowVertical, GUI.skin.horizontalScrollbar, GUI.skin.verticalScrollbar, GUI.skin.scrollView, options);
	}

	public static Vector2 BeginScrollView(Vector2 scrollPosition, GUIStyle horizontalScrollbar, GUIStyle verticalScrollbar, params GUILayoutOption[] options)
	{
		return BeginScrollView(scrollPosition, alwaysShowHorizontal: false, alwaysShowVertical: false, horizontalScrollbar, verticalScrollbar, GUI.skin.scrollView, options);
	}

	public static Vector2 BeginScrollView(Vector2 scrollPosition, GUIStyle style)
	{
		return BeginScrollView(scrollPosition, style, (GUILayoutOption[])null);
	}

	public static Vector2 BeginScrollView(Vector2 scrollPosition, GUIStyle style, params GUILayoutOption[] options)
	{
		string name = style.name;
		GUIStyle verticalScrollbar = GUI.skin.FindStyle(name + "VerticalScrollbar") ?? GUI.skin.verticalScrollbar;
		GUIStyle horizontalScrollbar = GUI.skin.FindStyle(name + "HorizontalScrollbar") ?? GUI.skin.horizontalScrollbar;
		return BeginScrollView(scrollPosition, alwaysShowHorizontal: false, alwaysShowVertical: false, horizontalScrollbar, verticalScrollbar, style, options);
	}

	public static Vector2 BeginScrollView(Vector2 scrollPosition, bool alwaysShowHorizontal, bool alwaysShowVertical, GUIStyle horizontalScrollbar, GUIStyle verticalScrollbar, params GUILayoutOption[] options)
	{
		return BeginScrollView(scrollPosition, alwaysShowHorizontal, alwaysShowVertical, horizontalScrollbar, verticalScrollbar, GUI.skin.scrollView, options);
	}

	public static Vector2 BeginScrollView(Vector2 scrollPosition, bool alwaysShowHorizontal, bool alwaysShowVertical, GUIStyle horizontalScrollbar, GUIStyle verticalScrollbar, GUIStyle background, params GUILayoutOption[] options)
	{
		GUIScrollGroup gUIScrollGroup = (GUIScrollGroup)GUILayoutUtility.BeginLayoutGroup(background, null, typeof(GUIScrollGroup));
		if (Event.current.type == EventType.layout)
		{
			gUIScrollGroup.resetCoords = true;
			gUIScrollGroup.isVertical = true;
			gUIScrollGroup.stretchWidth = 1;
			gUIScrollGroup.stretchHeight = 1;
			gUIScrollGroup.verticalScrollbar = verticalScrollbar;
			gUIScrollGroup.horizontalScrollbar = horizontalScrollbar;
			gUIScrollGroup.needsVerticalScrollbar = alwaysShowVertical;
			gUIScrollGroup.needsHorizontalScrollbar = alwaysShowHorizontal;
			gUIScrollGroup.ApplyOptions(options);
		}
		return GUI.BeginScrollView(gUIScrollGroup.rect, scrollPosition, new Rect(0f, 0f, gUIScrollGroup.clientWidth, gUIScrollGroup.clientHeight), alwaysShowHorizontal, alwaysShowVertical, horizontalScrollbar, verticalScrollbar);
	}

	public static void EndScrollView()
	{
		EndScrollView(handleScrollWheel: true);
	}

	internal static void EndScrollView(bool handleScrollWheel)
	{
		GUILayoutUtility.EndLayoutGroup();
		GUI.EndScrollView(handleScrollWheel);
	}

	public static GUILayoutOption Width(float width)
	{
		return new GUILayoutOption(GUILayoutOption.Type.fixedWidth, width);
	}

	public static GUILayoutOption MinWidth(float minWidth)
	{
		return new GUILayoutOption(GUILayoutOption.Type.minWidth, minWidth);
	}

	public static GUILayoutOption MaxWidth(float maxWidth)
	{
		return new GUILayoutOption(GUILayoutOption.Type.maxWidth, maxWidth);
	}

	public static GUILayoutOption Height(float height)
	{
		return new GUILayoutOption(GUILayoutOption.Type.fixedHeight, height);
	}

	public static GUILayoutOption MinHeight(float minHeight)
	{
		return new GUILayoutOption(GUILayoutOption.Type.minHeight, minHeight);
	}

	public static GUILayoutOption MaxHeight(float maxHeight)
	{
		return new GUILayoutOption(GUILayoutOption.Type.maxHeight, maxHeight);
	}

	public static GUILayoutOption ExpandWidth(bool expand)
	{
		return new GUILayoutOption(GUILayoutOption.Type.stretchWidth, expand ? 1 : 0);
	}

	public static GUILayoutOption ExpandHeight(bool expand)
	{
		return new GUILayoutOption(GUILayoutOption.Type.stretchHeight, expand ? 1 : 0);
	}
}
