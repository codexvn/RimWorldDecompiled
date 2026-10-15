using System;
using System.Collections.Generic;
using UnityStubs.Gui;
using UnityStubs.Internal;
using Waylith.UnityCompat;

namespace UnityEngine;

public class GUI
{
	private sealed class ScrollViewState
	{
		public int id;

		public Rect position;

		public Vector2 scrollPosition;

		public float maxX;

		public float maxY;
	}

	private struct PendingWindow
	{
		public int Id;

		public Rect Rect;

		public WindowFunction Func;

		public Matrix4x4 Matrix;

		public GUIStyle Style;
	}

	public delegate void WindowFunction(int id);

	public abstract class Scope : IDisposable
	{
		private bool m_Disposed;

		protected abstract void CloseScope();

		~Scope()
		{
			if (!m_Disposed)
			{
				Console.WriteLine("Scope was not disposed! You should use the 'using' keyword or manually call Dispose.");
			}
		}

		public void Dispose()
		{
			if (!m_Disposed)
			{
				m_Disposed = true;
				CloseScope();
			}
		}
	}

	public const float s_ScrollStepSize = 10f;

	private static GUISkin _skin = new GUISkin();

	private static Color _color = Color.white;

	private static Matrix4x4 _matrix = Matrix4x4.identity;

	private static Color _contentColor = Color.white;

	private static Material _blitMaterial;

	private static Material _blendMaterial;

	private const float ScrollWheelSpeed = 40f;

	private static readonly List<ScrollViewState> _scrollViewStates = new List<ScrollViewState>();

	private static int _scrollViewDepth;

	private static readonly Dictionary<int, Vector2> _pendingScroll = new Dictionary<int, Vector2>();

	private static Rect _buttonDownRect;

	private static bool _buttonDownActive;

	private static int _buttonDownButton;

	private static Rect _repeatDownRect;

	private static bool _repeatDownActive;

	private static readonly GUIContent _tempContent = new GUIContent();

	private const float TextFieldPadX = 4f;

	private static int _syncedEditorId;

	private static int _syncedCaret;

	private static int _syncedAnchor;

	private static string _nextControlName;

	private static int _caretIndex;

	private static int _selectAnchor;

	private static float _textScrollX;

	private static float _textScrollY;

	private static readonly Dictionary<int, string> _idToName = new Dictionary<int, string>();

	private static readonly Dictionary<string, int> _nameToId = new Dictionary<string, int>();

	private static readonly Dictionary<int, int> _idToWindowId = new Dictionary<int, int>();

	private static string _pendingFocusName;

	private static readonly HashSet<int> _fieldsDrawnThisPass = new HashSet<int>();

	private static bool _focusMovedThisPass;

	private static int _selectAllOnFocusId;

	private static int _nextNamedControlId = int.MinValue;

	private static readonly Dictionary<(string name, int occurrence), int> _repeatedNameToId = new Dictionary<(string, int), int>();

	private static readonly Dictionary<string, int> _nameOccurrences = new Dictionary<string, int>();

	private static readonly List<Rect> _selectionRects = new List<Rect>();

	internal const float ScrollBarThickness = 16f;

	private static int _scrollDragId;

	private static float _scrollDragGrab;

	private static int _guiSliderDragId;

	private static int _draggingWindowId = -1;

	private static Vector2 _dragStartMouseGui;

	private static Vector2 _dragStartWindowPos;

	internal const int NoWindowId = int.MinValue;

	private static int _currentWindowId = int.MinValue;

	private static Rect _currentWindowRect;

	private static bool _currentWindowDragged;

	private static Vector2 _draggedWindowPos;

	private static readonly List<PendingWindow> _pendingWindows = new List<PendingWindow>();

	private static readonly List<PendingWindow> _lastWindows = new List<PendingWindow>();

	private static readonly List<PendingWindow> _layoutWindows = new List<PendingWindow>();

	private static readonly Dictionary<int, Rect> _movedWindowRects = new Dictionary<int, Rect>();

	private static int _focusedWindowId = int.MinValue;

	private const int WindowControlIdBase = 2000100000;

	private const int ControlIdsPerWindow = 100000;

	private const int WindowControlIdSlots = 1400;

	private static readonly List<int> _staleMovedRectIds = new List<int>();

	public static bool enabled { get; set; } = true;

	public static Color color
	{
		get
		{
			return _color;
		}
		set
		{
			_color = value;
		}
	}

	public static Color backgroundColor { get; set; } = Color.white;

	public static Color contentColor
	{
		get
		{
			return _contentColor;
		}
		set
		{
			_contentColor = value;
		}
	}

	private static Color ContentTint => _color * _contentColor;

	public static bool changed { get; set; }

	public static Matrix4x4 matrix
	{
		get
		{
			return _matrix;
		}
		set
		{
			_matrix = value;
			GuiBackend.Current?.SetMatrix(value);
		}
	}

	public static GUISkin skin
	{
		get
		{
			return _skin;
		}
		set
		{
			_skin = value ?? _skin;
		}
	}

	internal static Material blitMaterial
	{
		get
		{
			object obj = _blitMaterial;
			if (obj == null)
			{
				obj = new Material((Shader)null)
				{
					name = "Hidden/Internal-GUITextureBlit"
				};
				_blitMaterial = (Material)obj;
			}
			return (Material)obj;
		}
	}

	internal static Material blendMaterial
	{
		get
		{
			object obj = _blendMaterial;
			if (obj == null)
			{
				obj = new Material((Shader)null)
				{
					name = "Hidden/Internal-GUITextureBlend"
				};
				_blendMaterial = (Material)obj;
			}
			return (Material)obj;
		}
	}

	public static int depth
	{
		get
		{
			return UnityRuntime.Current?.World.GuiDepth ?? 0;
		}
		set
		{
			UnityRuntime current = UnityRuntime.Current;
			if (current != null)
			{
				current.World.GuiDepth = value;
			}
		}
	}

	private static bool HasSelection => _selectAnchor != _caretIndex;

	private static int SelMin => Mathf.Min(_selectAnchor, _caretIndex);

	private static int SelMax => Mathf.Max(_selectAnchor, _caretIndex);

	public static string TextField(Rect position, string text)
	{
		return TextField(position, text, -1, null);
	}

	public static void DrawTextureWithTexCoords(Rect position, Texture image, Rect texCoords, bool alphaBlend)
	{
		DrawTextureWithTexCoordsImpl(position, image, texCoords);
	}

	public static void DrawTexture(Rect position, Texture image, ScaleMode scaleMode)
	{
		DrawTextureImpl(position, image, scaleMode, color);
	}

	public static void BeginGroup(Rect position)
	{
		GuiBackend.Current?.PushGroup(position);
	}

	public static void EndGroup()
	{
		GuiBackend.Current?.PopGroup();
	}

	public static void BeginGroup(Rect position, GUIStyle style)
	{
		BeginGroup(position);
	}

	public static void BeginGroup(Rect position, string text)
	{
		BeginGroup(position);
	}

	public static void BeginGroup(Rect position, GUIContent content, GUIStyle style)
	{
		BeginGroup(position);
	}

	public static void BeginClip(Rect position)
	{
		GuiBackend.Current?.PushGroup(position);
	}

	public static void BeginClip(Rect position, Vector2 scrollOffset, Vector2 renderOffset, bool resetOffset)
	{
		BeginClip(position);
	}

	public static void EndClip()
	{
		GuiBackend.Current?.PopGroup();
	}

	public static Vector2 BeginScrollView(Rect position, Vector2 scrollPosition, Rect viewRect)
	{
		return BeginScrollViewImpl(position, scrollPosition, viewRect);
	}

	public static Vector2 BeginScrollView(Rect position, Vector2 scrollPosition, Rect viewRect, GUIStyle horizontalScrollbar, GUIStyle verticalScrollbar)
	{
		return BeginScrollViewImpl(position, scrollPosition, viewRect, horizontalScrollbar, verticalScrollbar);
	}

	public static Vector2 BeginScrollView(Rect position, Vector2 scrollPosition, Rect viewRect, bool alwaysShowHorizontal, bool alwaysShowVertical)
	{
		return BeginScrollViewImpl(position, scrollPosition, viewRect);
	}

	public static Vector2 BeginScrollView(Rect position, Vector2 scrollPosition, Rect viewRect, bool alwaysShowHorizontal, bool alwaysShowVertical, GUIStyle horizontalScrollbar, GUIStyle verticalScrollbar)
	{
		return BeginScrollViewImpl(position, scrollPosition, viewRect, horizontalScrollbar, verticalScrollbar);
	}

	private static Vector2 BeginScrollViewImpl(Rect position, Vector2 scrollPosition, Rect viewRect, GUIStyle horizontalScrollbar = null, GUIStyle verticalScrollbar = null)
	{
		bool flag = horizontalScrollbar != GUIStyle.none && viewRect.width > position.width;
		bool flag2 = verticalScrollbar != GUIStyle.none && viewRect.height > position.height;
		int num = ControlIdFor(position);
		if (_pendingScroll.Remove(num, out var value))
		{
			scrollPosition = value;
		}
		float num2 = Mathf.Max(0f, viewRect.width - position.width);
		float num3 = Mathf.Max(0f, viewRect.height - position.height);
		Event current = Event.current;
		if (current == null || current.type != EventType.layout)
		{
			scrollPosition.x = Mathf.Clamp(scrollPosition.x, 0f, num2);
			scrollPosition.y = Mathf.Clamp(scrollPosition.y, 0f, num3);
			scrollPosition = HandleScrollbars(position, scrollPosition, viewRect, flag ? num2 : 0f, flag2 ? num3 : 0f);
		}
		if (_scrollViewDepth == _scrollViewStates.Count)
		{
			_scrollViewStates.Add(new ScrollViewState());
		}
		ScrollViewState scrollViewState = _scrollViewStates[_scrollViewDepth++];
		scrollViewState.id = num;
		scrollViewState.position = position;
		scrollViewState.scrollPosition = scrollPosition;
		scrollViewState.maxX = num2;
		scrollViewState.maxY = num3;
		Vector2 contentOffset = new Vector2(position.x - scrollPosition.x - viewRect.x, position.y - scrollPosition.y - viewRect.y);
		Rect clipRect = position;
		if (flag2)
		{
			clipRect.width -= 16f;
		}
		if (flag)
		{
			clipRect.height -= 16f;
		}
		GuiBackend.Current?.PushScrollView(clipRect, contentOffset);
		return scrollPosition;
	}

	public static void EndScrollView()
	{
		EndScrollView(handleScrollWheel: true);
	}

	public static void EndScrollView(bool handleScrollWheel)
	{
		GuiBackend.Current?.PopGroup();
		if (_scrollViewDepth == 0)
		{
			return;
		}
		ScrollViewState scrollViewState = _scrollViewStates[--_scrollViewDepth];
		Event current = Event.current;
		if (handleScrollWheel && current != null && current.type == EventType.scrollWheel)
		{
			bool num = scrollViewState.maxX > 0f && !Mathf.Approximately(0f, current.delta.x);
			bool flag = scrollViewState.maxY > 0f && !Mathf.Approximately(0f, current.delta.y);
			if ((num || flag) && scrollViewState.position.Contains(current.mousePosition))
			{
				Vector2 scrollPosition = scrollViewState.scrollPosition;
				scrollPosition.x = Mathf.Clamp(scrollPosition.x + current.delta.x * 40f, 0f, scrollViewState.maxX);
				scrollPosition.y = Mathf.Clamp(scrollPosition.y + current.delta.y * 40f, 0f, scrollViewState.maxY);
				_pendingScroll[scrollViewState.id] = scrollPosition;
				current.Use();
			}
		}
	}

	public static void DrawTexture(Rect position, Texture image)
	{
		DrawTextureImpl(position, image, ScaleMode.StretchToFill, _color);
	}

	public static void DrawTexture(Rect position, Texture image, ScaleMode scaleMode, bool alphaBlend)
	{
		DrawTextureImpl(position, image, scaleMode, _color);
	}

	public static void DrawTexture(Rect position, Texture image, ScaleMode scaleMode, bool alphaBlend, float imageAspect)
	{
		DrawTextureImpl(position, image, scaleMode, _color);
	}

	public static void DrawTexture(Rect position, Texture image, ScaleMode scaleMode, bool alphaBlend, float imageAspect, Color color, float borderWidth, float borderRadius)
	{
		DrawTextureImpl(position, image, scaleMode, _color * color);
	}

	public static void DrawTexture(Rect position, Texture image, ScaleMode scaleMode, bool alphaBlend, float imageAspect, Color color, Vector4 borderWidths, float borderRadius)
	{
		DrawTextureImpl(position, image, scaleMode, _color * color);
	}

	public static void DrawTexture(Rect position, Texture image, ScaleMode scaleMode, bool alphaBlend, float imageAspect, Color color, Vector4 borderWidths, Vector4 borderRadiuses)
	{
		DrawTextureImpl(position, image, scaleMode, _color * color);
	}

	public static void DrawTextureWithTexCoords(Rect position, Texture image, Rect texCoords)
	{
		DrawTextureWithTexCoordsImpl(position, image, texCoords);
	}

	private static void DrawTextureWithTexCoordsImpl(Rect position, Texture image, Rect texCoords)
	{
		if (image == null)
		{
			Event current = Event.current;
			if (current != null && current.type == EventType.repaint)
			{
				Debug.LogWarning("null texture passed to GUI.DrawTextureWithTexCoords");
			}
		}
		else
		{
			GuiBackend.Current?.DrawTextureWithTexCoords(position, image, texCoords, _color);
		}
	}

	private static void DrawTextureImpl(Rect position, Texture image, ScaleMode scaleMode, Color color)
	{
		IGuiBackend current = GuiBackend.Current;
		if (current == null)
		{
			return;
		}
		Matrix4x4 m;
		if (image == null)
		{
			Event current2 = Event.current;
			if (current2 != null && current2.type == EventType.repaint)
			{
				Debug.LogWarning("null texture passed to GUI.DrawTexture");
			}
		}
		else if (GL.GuiTransformActive)
		{
			m = GL.GuiTransform;
			current.DrawTextureQuad(Corner(position.xMin, position.yMin), Corner(position.xMax, position.yMin), Corner(position.xMax, position.yMax), Corner(position.xMin, position.yMax), image, color);
		}
		else
		{
			current.DrawTexture(position, image, scaleMode, color);
		}
		Vector2 Corner(float x, float y)
		{
			Vector3 vector = m.MultiplyPoint3x4(new Vector3(x, y, 0f));
			return new Vector2(vector.x, vector.y);
		}
	}

	public static void Label(Rect position, string text, GUIStyle style)
	{
		ConsumeNextControlName();
		IGuiBackend current = GuiBackend.Current;
		if (current != null)
		{
			(style ?? _skin.label).DrawText(current, position, text, ContentTint);
		}
	}

	public static void Label(Rect position, string text)
	{
		Label(position, text, _skin.label);
	}

	public static void Label(Rect position, GUIContent content)
	{
		Label(position, content, _skin.label);
	}

	public static void Label(Rect position, Texture image)
	{
		Label(position, image, _skin.label);
	}

	public static void Label(Rect position, GUIContent content, GUIStyle style)
	{
		if (content?.image == null)
		{
			Label(position, content?.text ?? "", style);
			return;
		}
		ConsumeNextControlName();
		IGuiBackend current = GuiBackend.Current;
		if (current != null)
		{
			if (style == null)
			{
				style = _skin.label;
			}
			Texture2D texture2D = style.normal?.background;
			if (texture2D != null)
			{
				current.DrawTexture(position, texture2D, ScaleMode.StretchToFill, _color * backgroundColor);
			}
			style.DrawContent(current, position, content, ContentTint, ContentTint);
		}
	}

	public static void Label(Rect position, Texture image, GUIStyle style)
	{
		ConsumeNextControlName();
		IGuiBackend current = GuiBackend.Current;
		if (current != null)
		{
			Texture2D texture2D = style?.normal?.background;
			if (texture2D != null)
			{
				current.DrawTexture(position, texture2D, ScaleMode.StretchToFill, _color * backgroundColor);
			}
			if (image != null)
			{
				(style ?? GUIStyle.none).DrawImage(current, position, image, ContentTint);
			}
		}
	}

	public static void Box(Rect position, GUIContent content)
	{
		Box(position, content, _skin.box);
	}

	public static void Box(Rect position, string text)
	{
		Box(position, new GUIContent(text), _skin.box);
	}

	public static void Box(Rect position, string text, GUIStyle style)
	{
		Box(position, new GUIContent(text), style);
	}

	public static void Box(Rect position, Texture image)
	{
		Box(position, new GUIContent("", image), _skin.box);
	}

	public static void Box(Rect position, Texture image, GUIStyle style)
	{
		Box(position, new GUIContent("", image), style);
	}

	public static float HorizontalScrollbar(Rect position, float value, float size, float leftValue, float rightValue)
	{
		return HorizontalScrollbar(position, value, size, leftValue, rightValue, _skin.horizontalScrollbar);
	}

	public static float HorizontalScrollbar(Rect position, float value, float size, float leftValue, float rightValue, GUIStyle style)
	{
		return Scrollbar(position, value, size, leftValue, rightValue, horizontal: true);
	}

	public static float VerticalScrollbar(Rect position, float value, float size, float topValue, float bottomValue)
	{
		return VerticalScrollbar(position, value, size, topValue, bottomValue, _skin.verticalScrollbar);
	}

	public static float VerticalScrollbar(Rect position, float value, float size, float topValue, float bottomValue, GUIStyle style)
	{
		return Scrollbar(position, value, size, topValue, bottomValue, horizontal: false);
	}

	private static float Scrollbar(Rect position, float value, float size, float startValue, float endValue, bool horizontal)
	{
		IGuiBackend current = GuiBackend.Current;
		Event current2 = Event.current;
		int num = ConsumeNextControlName() ?? ControlIdFor(position);
		float num2 = Mathf.Min(startValue, endValue);
		float num3 = Mathf.Max(startValue, endValue);
		float num4 = Mathf.Max(0f, num3 - num2 - size);
		float num5 = (horizontal ? position.width : position.height);
		float num6 = ((num3 > num2) ? Mathf.Clamp(num5 * size / (num3 - num2), 8f, num5) : num5);
		float num7 = Mathf.Max(0f, num5 - num6);
		if (current2 != null && current2.type == EventType.mouseDown && current2.button == 0 && position.Contains(current2.mousePosition))
		{
			_guiSliderDragId = num;
			current2.Use();
		}
		if (_guiSliderDragId == num)
		{
			if (current2 != null && Input.GetMouseButton(0) && current2.type != EventType.mouseUp)
			{
				float num8 = (horizontal ? (current2.mousePosition.x - position.x) : (current2.mousePosition.y - position.y));
				float num9 = ((num7 > 0f) ? Mathf.Clamp01((num8 - num6 * 0.5f) / num7) : 0f);
				value = num2 + num9 * num4;
				if (current2.type == EventType.mouseDrag)
				{
					current2.Use();
				}
			}
			else
			{
				_guiSliderDragId = 0;
			}
		}
		value = Mathf.Clamp(value, num2, num2 + num4);
		if (current != null)
		{
			current.FillRect(position, new Color(0.15f, 0.15f, 0.15f, 0.8f));
			float num10 = ((num4 > 0f) ? ((value - num2) / num4) : 0f);
			Rect rect = (horizontal ? new Rect(position.x + num10 * num7, position.y, num6, position.height) : new Rect(position.x, position.y + num10 * num7, position.width, num6));
			current.FillRect(rect, new Color(0.6f, 0.6f, 0.6f, 0.95f));
		}
		return value;
	}

	public static void Box(Rect position, GUIContent content, GUIStyle style)
	{
		ConsumeNextControlName();
		IGuiBackend current = GuiBackend.Current;
		if (current == null)
		{
			return;
		}
		Texture2D texture2D = style?.normal?.background;
		if (texture2D != null)
		{
			Color color = _color * backgroundColor;
			if ((object)texture2D == Texture2D.whiteTexture)
			{
				current.FillRect(position, color);
			}
			else
			{
				current.DrawTexture(position, texture2D, ScaleMode.StretchToFill, color);
			}
		}
		(style ?? _skin.label).DrawContent(current, position, content, ContentTint, ContentTint);
	}

	public static bool Button(Rect position, string text, GUIStyle style)
	{
		return ButtonImpl(position, TempContent(text, null), style);
	}

	public static bool Button(Rect position, string text)
	{
		return ButtonImpl(position, TempContent(text, null), _skin.button);
	}

	public static bool Button(Rect position, GUIContent content)
	{
		return ButtonImpl(position, content, _skin.button);
	}

	public static bool Button(Rect position, GUIContent content, GUIStyle style)
	{
		return ButtonImpl(position, content, style);
	}

	public static bool Button(Rect position, Texture image)
	{
		return ButtonImpl(position, TempContent("", image), _skin.button);
	}

	public static bool Button(Rect position, Texture image, GUIStyle style)
	{
		return ButtonImpl(position, TempContent("", image), style);
	}

	public static bool RepeatButton(Rect position, string text)
	{
		return RepeatButtonImpl(position, TempContent(text, null), _skin.button);
	}

	public static bool RepeatButton(Rect position, Texture image)
	{
		return RepeatButtonImpl(position, TempContent("", image), _skin.button);
	}

	public static bool RepeatButton(Rect position, GUIContent content)
	{
		return RepeatButtonImpl(position, content, _skin.button);
	}

	public static bool RepeatButton(Rect position, string text, GUIStyle style)
	{
		return RepeatButtonImpl(position, TempContent(text, null), style);
	}

	public static bool RepeatButton(Rect position, Texture image, GUIStyle style)
	{
		return RepeatButtonImpl(position, TempContent("", image), style);
	}

	public static bool RepeatButton(Rect position, GUIContent content, GUIStyle style)
	{
		return RepeatButtonImpl(position, content, style);
	}

	private static bool RepeatButtonImpl(Rect position, GUIContent content, GUIStyle style)
	{
		ConsumeNextControlName();
		Event current = Event.current;
		if (current == null)
		{
			return false;
		}
		bool flag = GuiBackend.Current?.InlineControlsObscured ?? false;
		bool flag2 = !flag && position.Contains(current.mousePosition);
		bool flag3 = _repeatDownActive && position == _repeatDownRect;
		if (flag3 && !Input.GetMouseButton(0))
		{
			_repeatDownActive = false;
			flag3 = false;
		}
		switch (current.type)
		{
		case EventType.repaint:
			style?.Draw(position, content, flag2, flag3 && flag2, on: false, hasKeyboardFocus: false);
			return flag3 && flag2;
		case EventType.mouseDown:
			if (flag2 && !flag && current.button == 0)
			{
				_repeatDownRect = position;
				_repeatDownActive = true;
				current.Use();
				return false;
			}
			break;
		case EventType.mouseUp:
			if (flag3)
			{
				_repeatDownActive = false;
				current.Use();
				return false;
			}
			break;
		}
		return false;
	}

	public static bool Toggle(Rect position, bool value, string text)
	{
		return Toggle(position, value, TempContent(text, null), _skin.toggle);
	}

	public static bool Toggle(Rect position, bool value, Texture image)
	{
		return Toggle(position, value, TempContent("", image), _skin.toggle);
	}

	public static bool Toggle(Rect position, bool value, GUIContent content)
	{
		return Toggle(position, value, content, _skin.toggle);
	}

	public static bool Toggle(Rect position, bool value, string text, GUIStyle style)
	{
		return Toggle(position, value, TempContent(text, null), style);
	}

	public static bool Toggle(Rect position, bool value, Texture image, GUIStyle style)
	{
		return Toggle(position, value, TempContent("", image), style);
	}

	public static bool Toggle(Rect position, bool value, GUIContent content, GUIStyle style)
	{
		if (!ButtonImpl(position, content, style, value))
		{
			return value;
		}
		return !value;
	}

	internal static GUIContent TempContent(string text, Texture image)
	{
		_tempContent.text = text;
		_tempContent.image = image;
		return _tempContent;
	}

	private static bool ButtonImpl(Rect position, GUIContent content, GUIStyle style, bool on = false)
	{
		ConsumeNextControlName();
		Event current = Event.current;
		if (current == null)
		{
			return false;
		}
		bool flag = GuiBackend.Current?.InlineControlsObscured ?? false;
		bool flag2 = !flag && position.Contains(current.mousePosition);
		if (current.type == EventType.repaint)
		{
			style?.Draw(position, content, flag2, flag2 && _buttonDownActive && position == _buttonDownRect, on, hasKeyboardFocus: false);
			return false;
		}
		if (flag)
		{
			return false;
		}
		switch (current.type)
		{
		case EventType.mouseDown:
			if (flag2)
			{
				_buttonDownRect = position;
				_buttonDownButton = current.button;
				_buttonDownActive = true;
				current.Use();
				return false;
			}
			break;
		case EventType.mouseUp:
			if (current.button == _buttonDownButton && flag2 && _buttonDownActive && position == _buttonDownRect)
			{
				_buttonDownActive = false;
				current.Use();
				return true;
			}
			break;
		}
		return false;
	}

	internal static Rect TextFieldTextRect(Rect rect, bool multiline, GUIStyle style = null)
	{
		RectOffset rectOffset = style?.padding;
		if (rectOffset != null)
		{
			return new Rect(rect.x + (float)rectOffset.left, rect.y + (float)rectOffset.top, Mathf.Max(0f, rect.width - (float)rectOffset.left - (float)rectOffset.right), Mathf.Max(0f, rect.height - (float)rectOffset.top - (float)rectOffset.bottom));
		}
		if (!multiline)
		{
			return new Rect(rect.x + 4f, rect.y, Mathf.Max(0f, rect.width - 8f), rect.height);
		}
		return new Rect(rect.x + 4f, rect.y + 2f, Mathf.Max(0f, rect.width - 8f), Mathf.Max(0f, rect.height - 4f));
	}

	private static float TextAreaWrapWidth(Rect area, GUIStyle style)
	{
		if (style != null && !style.wordWrap)
		{
			return -1f;
		}
		return area.width;
	}

	internal static void ResetNamedControlOccurrences()
	{
		_nameOccurrences.Clear();
		_scrollViewDepth = 0;
	}

	public static string TextField(Rect position, string text, GUIStyle style)
	{
		return TextField(position, text, -1, style);
	}

	public static string TextField(Rect position, string text, int maxLength)
	{
		return TextField(position, text, maxLength, null);
	}

	public static string TextArea(Rect position, string text, GUIStyle style)
	{
		return TextArea(position, text, -1, style);
	}

	public static string TextArea(Rect position, string text)
	{
		return TextArea(position, text, -1, null);
	}

	public static string TextArea(Rect position, string text, int maxLength)
	{
		return TextArea(position, text, maxLength, null);
	}

	public static string PasswordField(Rect position, string password, char maskChar)
	{
		return PasswordField(position, password, maskChar, null);
	}

	public static string TextField(Rect position, string text, int maxLength, GUIStyle style)
	{
		GUIContent gUIContent = GUIContent.Temp(text);
		DoTextField(position, GUIUtility.GetControlID(FocusType.Keyboard, position), gUIContent, multiline: false, maxLength, style);
		return gUIContent.text;
	}

	public static string TextArea(Rect position, string text, int maxLength, GUIStyle style)
	{
		GUIContent gUIContent = GUIContent.Temp(text);
		DoTextField(position, GUIUtility.GetControlID(FocusType.Keyboard, position), gUIContent, multiline: true, maxLength, style);
		return gUIContent.text;
	}

	public static string PasswordField(Rect position, string password, char maskChar, GUIStyle style)
	{
		GUIContent gUIContent = GUIContent.Temp(password);
		DoTextField(position, GUIUtility.GetControlID(FocusType.Keyboard, position), gUIContent, multiline: false, -1, style);
		return gUIContent.text;
	}

	internal static void DoTextField(Rect position, int id, GUIContent content, bool multiline, int maxLength, GUIStyle style)
	{
		DoTextField(position, id, content, multiline, maxLength, style, null, '\0');
	}

	internal static void DoTextField(Rect position, int id, GUIContent content, bool multiline, int maxLength, GUIStyle style, string secureText)
	{
		DoTextField(position, id, content, multiline, maxLength, style, secureText, '\0');
	}

	internal static void DoTextField(Rect position, int id, GUIContent content, bool multiline, int maxLength, GUIStyle style, string secureText, char maskChar)
	{
		content.text = Truncate(TextFieldImpl(position, secureText ?? content.text, style, multiline), maxLength);
	}

	private static string Truncate(string text, int maxLength)
	{
		if (maxLength < 0 || text == null || text.Length <= maxLength)
		{
			return text;
		}
		_caretIndex = Mathf.Min(_caretIndex, maxLength);
		_selectAnchor = Mathf.Min(_selectAnchor, maxLength);
		return text.Substring(0, maxLength);
	}

	private static int ControlIdFor(Rect rect)
	{
		Vector2 vector = GuiBackend.Current?.GuiToScreenPoint(new Vector2(rect.x, rect.y)) ?? new Vector2(rect.x, rect.y);
		int num = 17;
		num = num * 31 + Mathf.RoundToInt(vector.x);
		num = num * 31 + Mathf.RoundToInt(vector.y);
		num = num * 31 + Mathf.RoundToInt(rect.width);
		num = num * 31 + Mathf.RoundToInt(rect.height);
		if (num != 0)
		{
			return num;
		}
		return 1;
	}

	private static string TextFieldImpl(Rect rect, string text, GUIStyle style, bool multiline)
	{
		if (text == null)
		{
			text = "";
		}
		IGuiBackend current = GuiBackend.Current;
		bool num = _nextControlName != null && _pendingFocusName == _nextControlName;
		int num2 = ConsumeNextControlName() ?? ControlIdFor(rect);
		TrackControlWindow(num2);
		_fieldsDrawnThisPass.Add(num2);
		int fontIndex = style?.FontIndex ?? 1;
		if (num || _selectAllOnFocusId == num2)
		{
			_selectAllOnFocusId = 0;
			_selectAnchor = 0;
			_caretIndex = ((!multiline) ? text.Length : 0);
			_textScrollX = (_textScrollY = 0f);
		}
		Event current2 = Event.current;
		TextEditor textEditor = (TextEditor)GUIUtility.GetStateObject(typeof(TextEditor), num2);
		textEditor.controlID = num2;
		textEditor.position = rect;
		textEditor.style = style;
		textEditor.multiline = multiline;
		if (GUIUtility.keyboardControl == num2 && _syncedEditorId == num2 && (textEditor.cursorIndex != _syncedCaret || textEditor.selectIndex != _syncedAnchor))
		{
			_caretIndex = textEditor.cursorIndex;
			_selectAnchor = textEditor.selectIndex;
		}
		if (current2 != null && current2.type == EventType.mouseUp && current2.button == 0 && GUIUtility.hotControl == num2)
		{
			GUIUtility.hotControl = 0;
		}
		if (current2 != null && current2.type == EventType.mouseDown && current2.button == 0 && rect.Contains(current2.mousePosition))
		{
			bool num3 = GUIUtility.keyboardControl == num2;
			if (!num3)
			{
				_textScrollX = (_textScrollY = 0f);
			}
			GUIUtility.keyboardControl = num2;
			GUIUtility.hotControl = num2;
			TrackControlWindow(num2);
			int num4 = (_caretIndex = TextIndexAtMouse(rect, text, fontIndex, multiline, style, current2.mousePosition, current));
			if (!num3 || (current2.modifiers & EventModifiers.Shift) == 0)
			{
				_selectAnchor = num4;
			}
			if (current2.clickCount == 2)
			{
				SelectWordAt(text, num4);
			}
			else if (current2.clickCount >= 3)
			{
				SelectLineAt(text, num4);
			}
			current2.Use();
		}
		if (current2 != null && current2.type == EventType.mouseDrag && current2.button == 0 && GUIUtility.hotControl == num2)
		{
			_caretIndex = TextIndexAtMouse(rect, text, fontIndex, multiline, style, current2.mousePosition, current);
			current2.Use();
		}
		bool flag = GUIUtility.keyboardControl == num2;
		bool flag2 = multiline && style != null && style.normal.background == null && style.focused.background == null;
		if (flag)
		{
			TextFocus.ReadOnlyId = (flag2 ? num2 : 0);
		}
		if (flag && current2 != null && current2.type == EventType.keyDown && (!flag2 || TextFocus.ReadOnlyKeeps(current2.keyCode, (current2.modifiers & EventModifiers.Control) != 0)) && (!multiline || !ApplyTextAreaNavKey(rect, text, fontIndex, style, current2, current)))
		{
			text = ApplyTextKey(text, current2, multiline);
		}
		DrawTextFieldChrome(rect, text, fontIndex, flag, multiline, style, current);
		textEditor.text = text;
		if (flag)
		{
			textEditor.cursorIndex = _caretIndex;
			textEditor.selectIndex = _selectAnchor;
			textEditor.scrollOffset = new Vector2(_textScrollX, multiline ? _textScrollY : 0f);
			_syncedEditorId = num2;
			_syncedCaret = textEditor.cursorIndex;
			_syncedAnchor = textEditor.selectIndex;
		}
		return text;
	}

	private static int TextIndexAtMouse(Rect rect, string text, int fontIndex, bool multiline, GUIStyle style, Vector2 mouse, IGuiBackend be)
	{
		Rect area = TextFieldTextRect(rect, multiline, style);
		if (!multiline || be == null)
		{
			return CaretIndexAtX(text, fontIndex, mouse.x - area.x - AlignOffsetX(area.width, text, fontIndex, (style ?? _skin.textField).alignment, be) + _textScrollX, be);
		}
		return be.TextIndexAt(fontIndex, 0f, FontStyle.Normal, text, TextAreaWrapWidth(area, style), new Vector2(mouse.x - area.x + _textScrollX, mouse.y - area.y + _textScrollY));
	}

	private static void SelectWordAt(string text, int index)
	{
		int num = Mathf.Clamp(index, 0, text.Length);
		int i = num;
		while (num > 0 && char.IsLetterOrDigit(text[num - 1]))
		{
			num--;
		}
		for (; i < text.Length && char.IsLetterOrDigit(text[i]); i++)
		{
		}
		_selectAnchor = num;
		_caretIndex = i;
	}

	private static void SelectLineAt(string text, int index)
	{
		int num = Mathf.Clamp(index, 0, text.Length);
		int i = num;
		while (num > 0 && text[num - 1] != '\n')
		{
			num--;
		}
		for (; i < text.Length && text[i] != '\n'; i++)
		{
		}
		_selectAnchor = num;
		_caretIndex = i;
	}

	private static bool ApplyTextAreaNavKey(Rect rect, string text, int fontIndex, GUIStyle style, Event e, IGuiBackend be)
	{
		bool flag = be == null;
		if (!flag)
		{
			KeyCode keyCode = e.keyCode;
			bool flag2 = (((uint)(keyCode - 273) <= 1u || (uint)(keyCode - 278) <= 3u) ? true : false);
			flag = !flag2;
		}
		if (flag)
		{
			return false;
		}
		Rect area = TextFieldTextRect(rect, multiline: true, style);
		float wrapWidth = TextAreaWrapWidth(area, style);
		float lineHeight;
		Vector2 vector = be.TextCursorPos(fontIndex, 0f, FontStyle.Normal, text, wrapWidth, Mathf.Clamp(_caretIndex, 0, text.Length), out lineHeight);
		Vector2 local = e.keyCode switch
		{
			KeyCode.UpArrow => new Vector2(vector.x, vector.y - lineHeight * 0.5f), 
			KeyCode.DownArrow => new Vector2(vector.x, vector.y + lineHeight * 1.5f), 
			KeyCode.Home => new Vector2(0f, vector.y + lineHeight * 0.5f), 
			KeyCode.PageUp => new Vector2(vector.x, vector.y + lineHeight * 0.5f - area.height), 
			KeyCode.PageDown => new Vector2(vector.x, vector.y + lineHeight * 0.5f + area.height), 
			_ => new Vector2(float.MaxValue, vector.y + lineHeight * 0.5f), 
		};
		if (local.y >= 0f)
		{
			_caretIndex = be.TextIndexAt(fontIndex, 0f, FontStyle.Normal, text, wrapWidth, local);
		}
		else
		{
			_caretIndex = 0;
		}
		if ((e.modifiers & EventModifiers.Shift) == 0)
		{
			_selectAnchor = _caretIndex;
		}
		e.Use();
		return true;
	}

	private static string DeleteSelection(string text)
	{
		if (!HasSelection)
		{
			return text;
		}
		int selMin = SelMin;
		int selMax = SelMax;
		text = text.Remove(selMin, selMax - selMin);
		_caretIndex = (_selectAnchor = selMin);
		return text;
	}

	private static string ApplyTextKey(string text, Event e, bool multiline)
	{
		_caretIndex = Mathf.Clamp(_caretIndex, 0, text.Length);
		_selectAnchor = Mathf.Clamp(_selectAnchor, 0, text.Length);
		bool flag = (e.modifiers & EventModifiers.Shift) != 0;
		if ((e.modifiers & EventModifiers.Control) != 0)
		{
			switch (e.keyCode)
			{
			case KeyCode.A:
				_selectAnchor = 0;
				_caretIndex = text.Length;
				e.Use();
				return text;
			case KeyCode.C:
				if (HasSelection)
				{
					GUIUtility.systemCopyBuffer = text.Substring(SelMin, SelMax - SelMin);
				}
				e.Use();
				return text;
			case KeyCode.X:
				if (HasSelection)
				{
					GUIUtility.systemCopyBuffer = text.Substring(SelMin, SelMax - SelMin);
					text = DeleteSelection(text);
				}
				e.Use();
				return text;
			case KeyCode.V:
			{
				text = DeleteSelection(text);
				string text2 = GUIUtility.systemCopyBuffer ?? "";
				if (!multiline)
				{
					text2 = text2.Replace("\n", "").Replace("\r", "");
				}
				if (text2.Length > 0)
				{
					text = text.Insert(_caretIndex, text2);
					_caretIndex += text2.Length;
				}
				_selectAnchor = _caretIndex;
				e.Use();
				return text;
			}
			}
		}
		switch (e.keyCode)
		{
		case KeyCode.Backspace:
			if (HasSelection)
			{
				text = DeleteSelection(text);
			}
			else if (_caretIndex > 0)
			{
				text = text.Remove(_caretIndex - 1, 1);
				_caretIndex--;
			}
			_selectAnchor = _caretIndex;
			e.Use();
			return text;
		case KeyCode.Delete:
			if (HasSelection)
			{
				text = DeleteSelection(text);
			}
			else if (_caretIndex < text.Length)
			{
				text = text.Remove(_caretIndex, 1);
			}
			_selectAnchor = _caretIndex;
			e.Use();
			return text;
		case KeyCode.LeftArrow:
			if (HasSelection && !flag)
			{
				_caretIndex = SelMin;
			}
			else if (_caretIndex > 0)
			{
				_caretIndex--;
			}
			if (!flag)
			{
				_selectAnchor = _caretIndex;
			}
			e.Use();
			return text;
		case KeyCode.RightArrow:
			if (HasSelection && !flag)
			{
				_caretIndex = SelMax;
			}
			else if (_caretIndex < text.Length)
			{
				_caretIndex++;
			}
			if (!flag)
			{
				_selectAnchor = _caretIndex;
			}
			e.Use();
			return text;
		case KeyCode.Home:
			_caretIndex = 0;
			if (!flag)
			{
				_selectAnchor = 0;
			}
			e.Use();
			return text;
		case KeyCode.End:
			_caretIndex = text.Length;
			if (!flag)
			{
				_selectAnchor = text.Length;
			}
			e.Use();
			return text;
		case KeyCode.Return:
		case KeyCode.KeypadEnter:
			if (multiline)
			{
				text = DeleteSelection(text);
				text = text.Insert(_caretIndex, "\n");
				_caretIndex++;
				_selectAnchor = _caretIndex;
				e.Use();
			}
			return text;
		default:
		{
			char character = e.character;
			if (character != 0 && !char.IsControl(character))
			{
				text = DeleteSelection(text);
				text = text.Insert(_caretIndex, character.ToString());
				_caretIndex++;
				_selectAnchor = _caretIndex;
				e.Use();
			}
			return text;
		}
		}
	}

	private static int CaretIndexAtX(string text, int fontIndex, float localX, IGuiBackend be)
	{
		if (be == null || string.IsNullOrEmpty(text) || localX <= 0f)
		{
			if (!(localX <= 0f))
			{
				return text?.Length ?? 0;
			}
			return 0;
		}
		return be.TextIndexAt(fontIndex, 0f, FontStyle.Normal, text, -1f, new Vector2(localX, 0f));
	}

	private static float CaretX(string text, int fontIndex, int index, IGuiBackend be)
	{
		float lineHeight;
		return be.TextCursorPos(fontIndex, 0f, FontStyle.Normal, text, -1f, index, out lineHeight).x;
	}

	private static float AlignOffsetX(float width, string text, int fontIndex, TextAnchor alignment, IGuiBackend be)
	{
		int num = (int)alignment % 3;
		if (num == 0 || be == null)
		{
			return 0f;
		}
		float num2 = width - CaretX(text, fontIndex, text.Length, be);
		if (!(num2 <= 0f))
		{
			if (num != 1)
			{
				return num2;
			}
			return num2 * 0.5f;
		}
		return 0f;
	}

	private static void DrawTextFieldChrome(Rect rect, string text, int fontIndex, bool focused, bool multiline, GUIStyle style, IGuiBackend be)
	{
		if (be == null)
		{
			return;
		}
		GUIStyle obj = style ?? _skin.textField;
		TextAnchor alignment = obj.alignment;
		if (obj.normal.background != null)
		{
			Color color = _color * backgroundColor;
			be.FillRect(rect, color * new Color(0.16f, 0.16f, 0.16f, 0.6f));
			Color color2 = (focused ? new Color(0.55f, 0.78f, 1f, 0.9f) : new Color(0.5f, 0.5f, 0.5f, 0.6f));
			DrawBorder(rect, color * color2, be);
		}
		Color textColor = ContentTint * ((style == null) ? new Color(0.92f, 0.92f, 0.92f, 1f) : (focused ? style.focused.textColor : style.normal.textColor));
		int num = Mathf.Clamp(_caretIndex, 0, text.Length);
		int num2 = Mathf.Clamp(SelMin, 0, text.Length);
		int num3 = Mathf.Clamp(SelMax, 0, text.Length);
		int num4 = -1;
		int num5 = -1;
		if (focused && ImeComposition.Active)
		{
			string text2 = ImeComposition.Text;
			text = text.Remove(num2, num3 - num2).Insert(num2, text2);
			num4 = num2;
			num5 = num2 + text2.Length;
			num = (num2 = (num3 = num4 + ImeComposition.Cursor));
		}
		if (multiline)
		{
			DrawTextAreaContent(rect, text, fontIndex, focused, style, textColor, be, num, num2, num3, num4, num5);
			return;
		}
		Rect rect2 = TextFieldTextRect(rect, multiline: false, style);
		float num6 = CaretX(text, fontIndex, num, be);
		float num7 = AlignOffsetX(rect2.width, text, fontIndex, alignment, be);
		TextAnchor anchor = (TextAnchor)((int)alignment / 3 * 3);
		float num8 = 0f;
		if (focused)
		{
			float num9 = CaretX(text, fontIndex, text.Length, be);
			if (num6 - _textScrollX > rect2.width - 2f)
			{
				_textScrollX = num6 - rect2.width + 2f;
			}
			if (num6 - _textScrollX < 0f)
			{
				_textScrollX = num6;
			}
			_textScrollX = Mathf.Clamp(_textScrollX, 0f, Mathf.Max(0f, num9 - rect2.width + 2f));
			num8 = _textScrollX;
		}
		be.PushClipRect(rect2);
		if (focused && num3 > num2)
		{
			float num10 = CaretX(text, fontIndex, num2, be);
			float num11 = CaretX(text, fontIndex, num3, be);
			be.FillRect(new Rect(rect2.x + num7 + num10 - num8, rect.y + 3f, Mathf.Max(1f, num11 - num10), Mathf.Max(2f, rect.height - 6f)), new Color(0.3f, 0.45f, 0.75f, 0.55f));
		}
		be.DrawText(new Rect(rect2.x + num7 - num8, rect.y, rect2.width, rect.height), text, fontIndex, 0f, FontStyle.Normal, anchor, wordWrap: false, textColor);
		if (num5 > num4)
		{
			float num12 = CaretX(text, fontIndex, num4, be);
			float num13 = CaretX(text, fontIndex, num5, be);
			be.FillRect(new Rect(rect2.x + num7 + num12 - num8, rect.yMax - 4f, Mathf.Max(1f, num13 - num12), 1f), textColor);
		}
		if (focused)
		{
			be.FillRect(new Rect(rect2.x + num7 + num6 - num8, rect.y + 3f, 1.5f, Mathf.Max(2f, rect.height - 6f)), new Color(0.92f, 0.92f, 0.92f, 0.9f));
			ImeComposition.SetCandidatePos(be.GuiToScreenPoint(new Vector2(rect2.x + num7 + num6 - num8, rect.yMax - 3f)));
		}
		be.PopClipRect();
	}

	private static void DrawTextAreaContent(Rect rect, string text, int fontIndex, bool focused, GUIStyle style, Color textColor, IGuiBackend be, int caret, int selA, int selB, int compA, int compB)
	{
		Rect rect2 = TextFieldTextRect(rect, multiline: true, style);
		float num = TextAreaWrapWidth(rect2, style);
		float num2 = 0f;
		float num3 = 0f;
		Vector2 vector = default(Vector2);
		float lineHeight = 0f;
		if (focused)
		{
			vector = be.TextCursorPos(fontIndex, 0f, FontStyle.Normal, text, num, caret, out lineHeight);
			if (vector.y + lineHeight - _textScrollY > rect2.height)
			{
				_textScrollY = vector.y + lineHeight - rect2.height;
			}
			if (vector.y - _textScrollY < 0f)
			{
				_textScrollY = vector.y;
			}
			_textScrollY = Mathf.Max(0f, _textScrollY);
			num3 = _textScrollY;
			if (num < 0f)
			{
				if (vector.x - _textScrollX > rect2.width - 2f)
				{
					_textScrollX = vector.x - rect2.width + 2f;
				}
				if (vector.x - _textScrollX < 0f)
				{
					_textScrollX = vector.x;
				}
				_textScrollX = Mathf.Max(0f, _textScrollX);
				num2 = _textScrollX;
			}
		}
		be.PushClipRect(rect2);
		if (focused && selB > selA)
		{
			be.TextSelectionRects(fontIndex, 0f, FontStyle.Normal, text, num, selA, selB, _selectionRects);
			foreach (Rect selectionRect in _selectionRects)
			{
				be.FillRect(new Rect(rect2.x + selectionRect.x - num2, rect2.y + selectionRect.y - num3, selectionRect.width, selectionRect.height), new Color(0.3f, 0.45f, 0.75f, 0.55f));
			}
		}
		be.DrawText(new Rect(rect2.x - num2, rect2.y - num3, rect2.width + num2, rect2.height + num3), text, fontIndex, 0f, FontStyle.Normal, TextAnchor.UpperLeft, num > 0f, textColor);
		if (compB > compA)
		{
			be.TextSelectionRects(fontIndex, 0f, FontStyle.Normal, text, num, compA, compB, _selectionRects);
			foreach (Rect selectionRect2 in _selectionRects)
			{
				be.FillRect(new Rect(rect2.x + selectionRect2.x - num2, rect2.y + selectionRect2.y + selectionRect2.height - 1f - num3, selectionRect2.width, 1f), textColor);
			}
		}
		if (focused)
		{
			be.FillRect(new Rect(rect2.x + vector.x - num2, rect2.y + vector.y - num3, 1.5f, lineHeight), new Color(0.92f, 0.92f, 0.92f, 0.9f));
			ImeComposition.SetCandidatePos(be.GuiToScreenPoint(new Vector2(rect2.x + vector.x - num2, rect2.y + vector.y + lineHeight - num3)));
		}
		be.PopClipRect();
	}

	private static void DrawBorder(Rect r, Color c, IGuiBackend be, float t = 1f)
	{
		be.FillRect(new Rect(r.x, r.y, r.width, t), c);
		be.FillRect(new Rect(r.x, r.yMax - t, r.width, t), c);
		be.FillRect(new Rect(r.x, r.y, t, r.height), c);
		be.FillRect(new Rect(r.xMax - t, r.y, t, r.height), c);
	}

	private static Vector2 HandleScrollbars(Rect position, Vector2 scroll, Rect viewRect, float maxX, float maxY)
	{
		IGuiBackend current = GuiBackend.Current;
		Event current2 = Event.current;
		if (maxY > 0f)
		{
			int num = ControlIdFor(position) * 2 + 1;
			float num2 = Mathf.Max(20f, position.height * Mathf.Clamp01(position.height / viewRect.height));
			float num3 = position.height - num2;
			float num4 = position.xMax - 16f;
			if (current2 != null && num3 > 0f)
			{
				float num5 = position.y + scroll.y / maxY * num3;
				Rect rect = new Rect(num4, num5, 16f, num2);
				if (current2.type == EventType.mouseDown && current2.button == 0 && new Rect(num4, position.y, 16f, position.height).Contains(current2.mousePosition))
				{
					_scrollDragId = num;
					_scrollDragGrab = (rect.Contains(current2.mousePosition) ? (current2.mousePosition.y - num5) : (num2 * 0.5f));
					scroll.y = Mathf.Clamp((current2.mousePosition.y - _scrollDragGrab - position.y) / num3, 0f, 1f) * maxY;
					current2.Use();
				}
				else if (_scrollDragId == num)
				{
					if (Input.GetMouseButton(0) && current2.type != EventType.mouseUp)
					{
						scroll.y = Mathf.Clamp((current2.mousePosition.y - _scrollDragGrab - position.y) / num3, 0f, 1f) * maxY;
						if (current2.type == EventType.mouseDrag)
						{
							current2.Use();
						}
					}
					else
					{
						_scrollDragId = 0;
					}
				}
			}
			if (current != null)
			{
				float y = position.y + ((maxY > 0f) ? (scroll.y / maxY * num3) : 0f);
				current.FillRect(new Rect(num4, position.y, 16f, position.height), new Color(0.07f, 0.07f, 0.08f, 0.9f));
				float num6 = 12f;
				current.FillRectRounded(new Rect(num4 + 2f, y, num6, num2), new Color(0.62f, 0.62f, 0.64f, 0.95f), num6 * 0.5f);
			}
		}
		if (maxX > 0f)
		{
			int num7 = ControlIdFor(position) * 2;
			float num8 = Mathf.Max(20f, position.width * Mathf.Clamp01(position.width / viewRect.width));
			float num9 = position.width - num8;
			float num10 = position.yMax - 16f;
			if (current2 != null && num9 > 0f)
			{
				float num11 = position.x + scroll.x / maxX * num9;
				Rect rect2 = new Rect(num11, num10, num8, 16f);
				if (current2.type == EventType.mouseDown && current2.button == 0 && new Rect(position.x, num10, position.width, 16f).Contains(current2.mousePosition))
				{
					_scrollDragId = num7;
					_scrollDragGrab = (rect2.Contains(current2.mousePosition) ? (current2.mousePosition.x - num11) : (num8 * 0.5f));
					scroll.x = Mathf.Clamp((current2.mousePosition.x - _scrollDragGrab - position.x) / num9, 0f, 1f) * maxX;
					current2.Use();
				}
				else if (_scrollDragId == num7)
				{
					if (Input.GetMouseButton(0) && current2.type != EventType.mouseUp)
					{
						scroll.x = Mathf.Clamp((current2.mousePosition.x - _scrollDragGrab - position.x) / num9, 0f, 1f) * maxX;
						if (current2.type == EventType.mouseDrag)
						{
							current2.Use();
						}
					}
					else
					{
						_scrollDragId = 0;
					}
				}
			}
			if (current != null)
			{
				float x = position.x + ((maxX > 0f) ? (scroll.x / maxX * num9) : 0f);
				current.FillRect(new Rect(position.x, num10, position.width, 16f), new Color(0.07f, 0.07f, 0.08f, 0.9f));
				float num12 = 12f;
				current.FillRectRounded(new Rect(x, num10 + 2f, num8, num12), new Color(0.62f, 0.62f, 0.64f, 0.95f), num12 * 0.5f);
			}
		}
		return scroll;
	}

	public static float Slider(Rect position, float value, float size, float start, float end, GUIStyle slider, GUIStyle thumb, bool horiz, int id, GUIStyle thumbExtent = null)
	{
		float num = Mathf.Min(start, end);
		float num2 = Mathf.Max(start, end);
		float value2 = Mathf.Clamp(value, num, Mathf.Max(num, num2 - Mathf.Abs(size)));
		return Mathf.Clamp(horiz ? HorizontalSlider(position, value2, start, end) : VerticalSlider(position, value2, start, end), num, Mathf.Max(num, num2 - Mathf.Abs(size)));
	}

	public static float HorizontalSlider(Rect position, float value, float leftValue, float rightValue)
	{
		IGuiBackend current = GuiBackend.Current;
		Event current2 = Event.current;
		int num = ConsumeNextControlName() ?? ControlIdFor(position);
		float num2 = Mathf.Min(leftValue, rightValue);
		float num3 = Mathf.Max(leftValue, rightValue);
		float num4 = position.y + position.height * 0.5f;
		if (current2 != null && current2.type == EventType.mouseDown && current2.button == 0 && position.Contains(current2.mousePosition))
		{
			_guiSliderDragId = num;
			current2.Use();
		}
		if (_guiSliderDragId == num)
		{
			if (current2 != null && Input.GetMouseButton(0) && current2.type != EventType.mouseUp)
			{
				float t = ((position.width > 0f) ? Mathf.Clamp01((current2.mousePosition.x - position.x) / position.width) : 0f);
				value = Mathf.Lerp(num2, num3, t);
				if (current2.type == EventType.mouseDrag)
				{
					current2.Use();
				}
			}
			else
			{
				_guiSliderDragId = 0;
			}
		}
		if (current != null)
		{
			current.FillRect(new Rect(position.x, num4 - 1f, position.width, 2f), new Color(0.5f, 0.5f, 0.5f, 0.8f));
			float num5 = ((num3 > num2) ? Mathf.Clamp01((value - num2) / (num3 - num2)) : 0f);
			float num6 = position.x + num5 * position.width;
			current.FillRect(new Rect(num6 - 4f, position.y, 8f, position.height), new Color(0.8f, 0.8f, 0.8f, 0.95f));
		}
		return value;
	}

	public static float VerticalSlider(Rect position, float value, float topValue, float bottomValue)
	{
		IGuiBackend current = GuiBackend.Current;
		Event current2 = Event.current;
		int num = ConsumeNextControlName() ?? ControlIdFor(position);
		float num2 = position.x + position.width * 0.5f;
		if (current2 != null && current2.type == EventType.mouseDown && current2.button == 0 && position.Contains(current2.mousePosition))
		{
			_guiSliderDragId = num;
			current2.Use();
		}
		if (_guiSliderDragId == num)
		{
			if (current2 != null && Input.GetMouseButton(0) && current2.type != EventType.mouseUp)
			{
				float t = ((position.height > 0f) ? Mathf.Clamp01((current2.mousePosition.y - position.y) / position.height) : 0f);
				value = Mathf.Lerp(topValue, bottomValue, t);
				if (current2.type == EventType.mouseDrag)
				{
					current2.Use();
				}
			}
			else
			{
				_guiSliderDragId = 0;
			}
		}
		if (current != null)
		{
			current.FillRect(new Rect(num2 - 1f, position.y, 2f, position.height), new Color(0.5f, 0.5f, 0.5f, 0.8f));
			float num3 = ((bottomValue != topValue) ? Mathf.Clamp01((value - topValue) / (bottomValue - topValue)) : 0f);
			float num4 = position.y + num3 * position.height;
			current.FillRect(new Rect(position.x, num4 - 4f, position.width, 8f), new Color(0.8f, 0.8f, 0.8f, 0.95f));
		}
		return value;
	}

	public static void DragWindow()
	{
		DragWindow(new Rect(0f, 0f, _currentWindowRect.width, _currentWindowRect.height));
	}

	public static void DragWindow(Rect position)
	{
		Event current = Event.current;
		if (current == null)
		{
			return;
		}
		Vector2 dragStartMouseGui = new Vector2(current.mousePosition.x + _currentWindowRect.x, current.mousePosition.y + _currentWindowRect.y);
		if (current.type == EventType.mouseDown && current.button == 0 && position.Contains(current.mousePosition))
		{
			_draggingWindowId = _currentWindowId;
			_dragStartMouseGui = dragStartMouseGui;
			_dragStartWindowPos = new Vector2(_currentWindowRect.x, _currentWindowRect.y);
			current.Use();
		}
		if (_draggingWindowId == _currentWindowId)
		{
			_draggedWindowPos = new Vector2(_dragStartWindowPos.x + (dragStartMouseGui.x - _dragStartMouseGui.x), _dragStartWindowPos.y + (dragStartMouseGui.y - _dragStartMouseGui.y));
			_currentWindowDragged = true;
			if (current.type == EventType.mouseUp || !Input.GetMouseButton(0))
			{
				_draggingWindowId = -1;
			}
		}
	}

	public static void BringWindowToFront(int windowID)
	{
	}

	public static void FocusWindow(int windowID)
	{
		_focusedWindowId = windowID;
	}

	public static void SetNextControlName(string name)
	{
		_nextControlName = name;
	}

	private static int? ConsumeNextControlName()
	{
		if (_nextControlName == null)
		{
			return null;
		}
		string nextControlName = _nextControlName;
		_nextControlName = null;
		_nameOccurrences.TryGetValue(nextControlName, out var value);
		_nameOccurrences[nextControlName] = value + 1;
		int value2;
		if (value == 0)
		{
			if (!_nameToId.TryGetValue(nextControlName, out value2))
			{
				value2 = _nextNamedControlId++;
				_nameToId.Add(nextControlName, value2);
				_idToName.Add(value2, nextControlName);
			}
		}
		else if (!_repeatedNameToId.TryGetValue((nextControlName, value), out value2))
		{
			value2 = _nextNamedControlId++;
			_repeatedNameToId.Add((nextControlName, value), value2);
			_idToName.Add(value2, nextControlName);
		}
		if (_pendingFocusName == nextControlName)
		{
			GUIUtility.keyboardControl = value2;
			_pendingFocusName = null;
		}
		TrackControlWindow(value2);
		return value2;
	}

	private static void TrackControlWindow(int id)
	{
		int value;
		if (_currentWindowId == int.MinValue)
		{
			_idToWindowId.Remove(id);
		}
		else if (GUIUtility.keyboardControl == id || !_idToWindowId.TryGetValue(id, out value) || IndexOfPendingWindow(value) < 0)
		{
			_idToWindowId[id] = _currentWindowId;
		}
	}

	public static string GetNameOfFocusedControl()
	{
		if (!_idToName.TryGetValue(GUIUtility.keyboardControl, out var value))
		{
			return "";
		}
		return value;
	}

	public static int GetFocusedControlWindowId()
	{
		if (!_idToWindowId.TryGetValue(GUIUtility.keyboardControl, out var value))
		{
			return int.MinValue;
		}
		return value;
	}

	public static void FocusControl(string name)
	{
		int value;
		if (string.IsNullOrEmpty(name))
		{
			GUIUtility.keyboardControl = 0;
			_pendingFocusName = null;
		}
		else if (_nameToId.TryGetValue(name, out value))
		{
			if (GUIUtility.keyboardControl != value)
			{
				_selectAllOnFocusId = value;
			}
			GUIUtility.keyboardControl = value;
			_focusMovedThisPass = true;
			if (_currentWindowId != int.MinValue)
			{
				_idToWindowId[value] = _currentWindowId;
			}
		}
		else
		{
			_pendingFocusName = name;
		}
	}

	public static Rect Window(int id, Rect clientRect, WindowFunction func, string text, GUIStyle style)
	{
		if (_movedWindowRects.Remove(id, out var value))
		{
			clientRect = new Rect(value.x, value.y, clientRect.width, clientRect.height);
		}
		_pendingWindows.Add(new PendingWindow
		{
			Id = id,
			Rect = clientRect,
			Func = func,
			Matrix = _matrix,
			Style = style
		});
		return clientRect;
	}

	public static Rect ModalWindow(int id, Rect clientRect, WindowFunction func, string text, GUIStyle style)
	{
		return Window(id, clientRect, func, text, style);
	}

	public static void ProcessPendingWindows()
	{
		try
		{
			RunPendingWindows();
		}
		finally
		{
			DropFocusOfUndrawnField();
		}
	}

	private static void DropFocusOfUndrawnField()
	{
		Event current = Event.current;
		if (current != null && current.rawType == EventType.repaint)
		{
			if (GUIUtility.keyboardControl != 0 && !_focusMovedThisPass && !_fieldsDrawnThisPass.Contains(GUIUtility.keyboardControl))
			{
				GUIUtility.keyboardControl = 0;
			}
			_focusMovedThisPass = false;
		}
		_fieldsDrawnThisPass.Clear();
	}

	private static void RunPendingWindows()
	{
		int focusedControlWindowId = GetFocusedControlWindowId();
		if (focusedControlWindowId != int.MinValue && IndexOfPendingWindow(focusedControlWindowId) < 0)
		{
			GUIUtility.keyboardControl = 0;
		}
		Event current = Event.current;
		if (current != null && current.rawType == EventType.repaint)
		{
			KeepLayoutWindows();
		}
		if (current == null || _pendingWindows.Count == 0)
		{
			ClearPendingWindows();
			return;
		}
		Matrix4x4 matrix4x = _matrix;
		try
		{
			if (!IsWindowRoutedEvent(current.originalType))
			{
				for (int i = 0; i < _pendingWindows.Count; i++)
				{
					matrix = _pendingWindows[i].Matrix;
					RunPendingWindow(_pendingWindows[i]);
				}
			}
			if (_movedWindowRects.Count > 0)
			{
				PruneMovedRects();
			}
		}
		finally
		{
			matrix = matrix4x;
			ClearPendingWindows();
		}
	}

	public static void ClearPendingWindows()
	{
		Event current = Event.current;
		if (current != null && current.rawType == EventType.layout)
		{
			_layoutWindows.Clear();
			_layoutWindows.AddRange(_pendingWindows);
		}
		_lastWindows.Clear();
		_lastWindows.AddRange(_pendingWindows);
		_pendingWindows.Clear();
	}

	private static void KeepLayoutWindows()
	{
		int num = 0;
		foreach (PendingWindow layoutWindow in _layoutWindows)
		{
			int num2 = IndexOfPendingWindow(layoutWindow.Id);
			if (num2 >= 0)
			{
				num = num2 + 1;
			}
			else
			{
				_pendingWindows.Insert(num++, layoutWindow);
			}
		}
		_layoutWindows.Clear();
	}

	private static bool IsWindowRoutedEvent(EventType type)
	{
		if ((uint)type <= 1u || (uint)(type - 3) <= 3u)
		{
			return true;
		}
		return false;
	}

	public static void DispatchInputToWindows()
	{
		Event current = Event.current;
		if (current == null || _lastWindows.Count == 0 || !IsWindowRoutedEvent(current.originalType))
		{
			return;
		}
		Matrix4x4 matrix4x = _matrix;
		try
		{
			int num = -1;
			EventType originalType = current.originalType;
			if ((uint)(originalType - 4) <= 1u)
			{
				num = IndexOfWindow(_lastWindows, GetFocusedControlWindowId());
				if (num < 0)
				{
					num = IndexOfWindow(_lastWindows, _focusedWindowId);
				}
			}
			else
			{
				for (int num2 = _lastWindows.Count - 1; num2 >= 0; num2--)
				{
					matrix = _lastWindows[num2].Matrix;
					if (_lastWindows[num2].Rect.Contains(current.mousePosition))
					{
						num = num2;
						break;
					}
				}
			}
			if (num >= 0)
			{
				PendingWindow window = _lastWindows[num];
				if (current.originalType == EventType.mouseDown)
				{
					_focusedWindowId = window.Id;
				}
				matrix = window.Matrix;
				RunPendingWindow(in window);
			}
		}
		finally
		{
			matrix = matrix4x;
		}
	}

	private static int IndexOfWindow(List<PendingWindow> windows, int id)
	{
		if (id == int.MinValue)
		{
			return -1;
		}
		for (int i = 0; i < windows.Count; i++)
		{
			if (windows[i].Id == id)
			{
				return i;
			}
		}
		return -1;
	}

	private static int IndexOfPendingWindow(int id)
	{
		return IndexOfWindow(_pendingWindows, id);
	}

	private static void RunPendingWindow(in PendingWindow window)
	{
		int currentWindowId = _currentWindowId;
		_currentWindowId = window.Id;
		_currentWindowRect = window.Rect;
		_currentWindowDragged = false;
		_nameOccurrences.Clear();
		int nextControlId = GUIUtility.NextControlId;
		GUIUtility.NextControlId = WindowFirstControlId(window.Id);
		Event current = Event.current;
		bool flag = current != null && current.type == EventType.layout;
		if (flag)
		{
			GUILayoutUtility.BeginWindow(window.Id, window.Style ?? GUIStyle.none, new GUILayoutOption[2]
			{
				GUILayout.Width(window.Rect.width),
				GUILayout.Height(window.Rect.height)
			});
		}
		else
		{
			GUILayoutUtility.BeginWindow(window.Id, GUIStyle.none, null);
		}
		GuiBackend.Current?.BeginWindowContent(window.Rect);
		GuiBackend.Current?.PushGroup(window.Rect);
		try
		{
			window.Func?.Invoke(window.Id);
			if (flag)
			{
				GUILayoutUtility.Layout();
			}
		}
		finally
		{
			GuiBackend.Current?.PopGroup();
			GuiBackend.Current?.EndWindowContent();
			_currentWindowId = currentWindowId;
			_nameOccurrences.Clear();
			GUIUtility.NextControlId = nextControlId;
		}
		if (_currentWindowDragged)
		{
			_movedWindowRects[window.Id] = new Rect(_draggedWindowPos.x, _draggedWindowPos.y, window.Rect.width, window.Rect.height);
		}
	}

	private static int WindowFirstControlId(int windowId)
	{
		return (int)(2000100000 + (uint)windowId % 1400u * 100000);
	}

	private static void PruneMovedRects()
	{
		_staleMovedRectIds.Clear();
		foreach (int key in _movedWindowRects.Keys)
		{
			if (IndexOfPendingWindow(key) < 0)
			{
				_staleMovedRectIds.Add(key);
			}
		}
		for (int i = 0; i < _staleMovedRectIds.Count; i++)
		{
			_movedWindowRects.Remove(_staleMovedRectIds[i]);
		}
	}
}
