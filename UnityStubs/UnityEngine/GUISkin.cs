using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using Waylith.Math;
using Waylith.Rendering;

namespace UnityEngine;

public class GUISkin : ScriptableObject
{
	[CompilerGenerated]
	private sealed class _003CAllStyles_003Ed__82 : IEnumerable<(string name, GUIStyle style)>, IEnumerable, IEnumerator<(string name, GUIStyle style)>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private (string name, GUIStyle style) _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public GUISkin _003C_003E4__this;

		private PropertyInfo[] _003C_003E7__wrap1;

		private int _003C_003E7__wrap2;

		private GUIStyle[] _003C_003E7__wrap3;

		(string name, GUIStyle style) IEnumerator<(string, GUIStyle)>.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		[DebuggerHidden]
		public _003CAllStyles_003Ed__82(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003C_003E7__wrap1 = null;
			_003C_003E7__wrap3 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			GUISkin gUISkin = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E7__wrap1 = typeof(GUISkin).GetProperties();
				_003C_003E7__wrap2 = 0;
				goto IL_00a7;
			case 1:
				_003C_003E1__state = -1;
				goto IL_0099;
			case 2:
				{
					_003C_003E1__state = -1;
					goto IL_0106;
				}
				IL_00a7:
				if (_003C_003E7__wrap2 < _003C_003E7__wrap1.Length)
				{
					PropertyInfo propertyInfo = _003C_003E7__wrap1[_003C_003E7__wrap2];
					if (propertyInfo.PropertyType == typeof(GUIStyle))
					{
						_003C_003E2__current = (name: propertyInfo.Name, style: (GUIStyle)propertyInfo.GetValue(gUISkin));
						_003C_003E1__state = 1;
						return true;
					}
					goto IL_0099;
				}
				_003C_003E7__wrap1 = null;
				_003C_003E7__wrap3 = gUISkin.customStyles;
				_003C_003E7__wrap2 = 0;
				goto IL_0114;
				IL_0114:
				if (_003C_003E7__wrap2 < _003C_003E7__wrap3.Length)
				{
					GUIStyle gUIStyle = _003C_003E7__wrap3[_003C_003E7__wrap2];
					if (gUIStyle != null)
					{
						_003C_003E2__current = (name: gUIStyle.name, style: gUIStyle);
						_003C_003E1__state = 2;
						return true;
					}
					goto IL_0106;
				}
				_003C_003E7__wrap3 = null;
				return false;
				IL_0106:
				_003C_003E7__wrap2++;
				goto IL_0114;
				IL_0099:
				_003C_003E7__wrap2++;
				goto IL_00a7;
			}
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<(string name, GUIStyle style)> IEnumerable<(string, GUIStyle)>.GetEnumerator()
		{
			_003CAllStyles_003Ed__82 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CAllStyles_003Ed__82(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<(string, GUIStyle)>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetEnumerator_003Ed__85 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public GUISkin _003C_003E4__this;

		private IEnumerator<(string name, GUIStyle style)> _003C_003E7__wrap1;

		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		[DebuggerHidden]
		public _003CGetEnumerator_003Ed__85(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if (num == -3 || num == 1)
			{
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003C_003E7__wrap1 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				GUISkin gUISkin = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = gUISkin.AllStyles().GetEnumerator();
					_003C_003E1__state = -3;
					break;
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				if (_003C_003E7__wrap1.MoveNext())
				{
					GUIStyle item = _003C_003E7__wrap1.Current.style;
					_003C_003E2__current = item;
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap1 = null;
				return false;
			}
			catch
			{
				//try-fault
				((IDisposable)this).Dispose();
				throw;
			}
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		private void _003C_003Em__Finally1()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap1 != null)
			{
				_003C_003E7__wrap1.Dispose();
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}
	}

	private static Texture2D _textFieldBackground;

	private readonly GUIStyle _label = new GUIStyle
	{
		name = "label",
		padding = new RectOffset(0, 0, 3, 3),
		margin = new RectOffset(4, 4, 4, 4),
		wordWrap = true,
		richText = true,
		clipping = TextClipping.Clip
	};

	private readonly GUIStyle _textField = CreateTextFieldStyle("textfield", wordWrap: false);

	private readonly GUIStyle _textArea = CreateTextFieldStyle("textarea", wordWrap: true);

	private readonly GUIStyle _button = CreateButtonStyle();

	private readonly GUIStyle _box = new GUIStyle
	{
		name = "box",
		padding = new RectOffset(4, 4, 4, 4),
		margin = new RectOffset(4, 4, 4, 4),
		border = new RectOffset(6, 6, 6, 6),
		alignment = TextAnchor.UpperCenter,
		richText = true,
		clipping = TextClipping.Clip
	};

	private readonly GUIStyle _toggle = new GUIStyle
	{
		name = "toggle",
		padding = new RectOffset(15, 0, 3, 0),
		margin = new RectOffset(4, 4, 4, 4),
		border = new RectOffset(14, 0, 14, 0),
		overflow = new RectOffset(-1, 0, -4, 0),
		richText = true,
		clipping = TextClipping.Clip
	};

	private readonly GUIStyle _window = new GUIStyle
	{
		name = "window",
		padding = new RectOffset(10, 10, 20, 10),
		border = new RectOffset(8, 8, 18, 8),
		alignment = TextAnchor.UpperCenter,
		contentOffset = new Vector2(0f, -18f),
		richText = true,
		clipping = TextClipping.Clip
	};

	private readonly GUIStyle _scrollbar = new GUIStyle();

	private readonly GUISettings _settings = new GUISettings();

	private readonly GUIStyle _misc = new GUIStyle();

	private GUIStyle _horizontalScrollbar = new GUIStyle
	{
		name = "horizontalscrollbar",
		fixedHeight = 16f,
		margin = new RectOffset(4, 4, 1, 4)
	};

	private GUIStyle _horizontalScrollbarLeftButton;

	private GUIStyle _horizontalScrollbarRightButton;

	private GUIStyle _horizontalScrollbarThumb;

	private GUIStyle _verticalScrollbar = new GUIStyle
	{
		name = "verticalscrollbar",
		fixedWidth = 16f,
		stretchWidth = false,
		stretchHeight = true,
		margin = new RectOffset(1, 4, 4, 4)
	};

	private GUIStyle _verticalScrollbarDownButton;

	private GUIStyle _verticalScrollbarThumb;

	private GUIStyle _verticalScrollbarUpButton;

	internal static Texture2D TextFieldBackground => _textFieldBackground ?? (_textFieldBackground = new Texture2D(Waylith.Rendering.Texture2D.CreateSolid("TextFieldBackground", new Waylith.Math.Color32(41, 41, 41, 153))));

	public GUIStyle box => _box;

	public GUIStyle button => _button;

	public GUIStyle toggle => _toggle;

	public GUIStyle window => _window;

	public GUIStyle textArea => _textArea;

	public GUIStyle horizontalSlider => _misc;

	public GUIStyle horizontalSliderThumb => _misc;

	public GUIStyle verticalSlider => _misc;

	public GUIStyle verticalSliderThumb => _misc;

	public GUIStyle scrollView => _misc;

	public GUIStyle horizontalScrollbar
	{
		get
		{
			return _horizontalScrollbar ?? _scrollbar;
		}
		set
		{
			_horizontalScrollbar = value;
		}
	}

	public GUIStyle horizontalScrollbarLeftButton
	{
		get
		{
			return _horizontalScrollbarLeftButton ?? _scrollbar;
		}
		set
		{
			_horizontalScrollbarLeftButton = value;
		}
	}

	public GUIStyle horizontalScrollbarRightButton
	{
		get
		{
			return _horizontalScrollbarRightButton ?? _scrollbar;
		}
		set
		{
			_horizontalScrollbarRightButton = value;
		}
	}

	public GUIStyle horizontalScrollbarThumb
	{
		get
		{
			return _horizontalScrollbarThumb ?? _scrollbar;
		}
		set
		{
			_horizontalScrollbarThumb = value;
		}
	}

	public GUIStyle label => _label;

	public GUISettings settings => _settings;

	public GUIStyle textField => _textField;

	public GUIStyle verticalScrollbar
	{
		get
		{
			return _verticalScrollbar ?? _scrollbar;
		}
		set
		{
			_verticalScrollbar = value;
		}
	}

	public GUIStyle verticalScrollbarDownButton
	{
		get
		{
			return _verticalScrollbarDownButton ?? _scrollbar;
		}
		set
		{
			_verticalScrollbarDownButton = value;
		}
	}

	public GUIStyle verticalScrollbarThumb
	{
		get
		{
			return _verticalScrollbarThumb ?? _scrollbar;
		}
		set
		{
			_verticalScrollbarThumb = value;
		}
	}

	public GUIStyle verticalScrollbarUpButton
	{
		get
		{
			return _verticalScrollbarUpButton ?? _scrollbar;
		}
		set
		{
			_verticalScrollbarUpButton = value;
		}
	}

	public Font font { get; set; }

	public GUIStyle[] customStyles { get; set; } = Array.Empty<GUIStyle>();

	private static Texture2D CreateButtonBackground(string name, byte fill, byte rim)
	{
		Waylith.Math.Color32[] array = new Waylith.Math.Color32[256];
		for (int i = 0; i < 16; i++)
		{
			for (int j = 0; j < 16; j++)
			{
				float a = Mathf.Abs((float)j + 0.5f - 8f) - 4.5f;
				float num = Mathf.Abs((float)i + 0.5f - 8f) - 4.5f;
				float num2 = Mathf.Sqrt(Mathf.Max(a, 0f) * Mathf.Max(a, 0f) + Mathf.Max(num, 0f) * Mathf.Max(num, 0f)) + Mathf.Min(Mathf.Max(a, num), 0f) - 3.5f;
				byte b = (byte)Mathf.Lerp((int)rim, (int)fill, Mathf.Clamp01(0f - num2 - 0.5f));
				array[i * 16 + j] = new Waylith.Math.Color32(b, b, b, (byte)(Mathf.Clamp01(0.5f - num2) * 230f));
			}
		}
		Waylith.Rendering.Texture2D texture2D = new Waylith.Rendering.Texture2D(16, 16, LoadedTextureFormat.Rgba32, linear: false, 1, createUninitialized: false);
		texture2D.Name = name;
		texture2D.CpuData.SetPixels32(array);
		texture2D.Apply(updateMipmaps: false, makeNoLongerReadable: true);
		return new Texture2D(texture2D);
	}

	private static GUIStyle CreateButtonStyle()
	{
		GUIStyle gUIStyle = new GUIStyle
		{
			name = "button",
			padding = new RectOffset(6, 6, 3, 3),
			margin = new RectOffset(4, 4, 4, 4),
			border = new RectOffset(6, 6, 6, 4),
			alignment = TextAnchor.MiddleCenter,
			richText = true,
			clipping = TextClipping.Clip
		};
		GUIStyleState normal = gUIStyle.normal;
		Texture2D background = (gUIStyle.focused.background = CreateButtonBackground("ButtonNormal", 62, 140));
		normal.background = background;
		gUIStyle.hover.background = CreateButtonBackground("ButtonHover", 84, 175);
		gUIStyle.active.background = CreateButtonBackground("ButtonActive", 40, 175);
		return gUIStyle;
	}

	private static GUIStyle CreateTextFieldStyle(string name, bool wordWrap)
	{
		GUIStyle gUIStyle = new GUIStyle
		{
			name = name,
			padding = new RectOffset(3, 3, 3, 3),
			margin = new RectOffset(4, 4, 4, 4),
			border = new RectOffset(4, 4, 4, 4),
			wordWrap = wordWrap,
			alignment = ((!wordWrap) ? TextAnchor.MiddleLeft : TextAnchor.UpperLeft),
			clipping = TextClipping.Clip
		};
		GUIStyleState[] array = new GUIStyleState[6] { gUIStyle.normal, gUIStyle.hover, gUIStyle.active, gUIStyle.focused, gUIStyle.onHover, gUIStyle.onFocused };
		for (int i = 0; i < array.Length; i++)
		{
			array[i].background = TextFieldBackground;
		}
		return gUIStyle;
	}

	[IteratorStateMachine(typeof(_003CAllStyles_003Ed__82))]
	private IEnumerable<(string name, GUIStyle style)> AllStyles()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CAllStyles_003Ed__82(-2)
		{
			_003C_003E4__this = this
		};
	}

	public GUIStyle FindStyle(string styleName)
	{
		foreach (var (a, result) in AllStyles())
		{
			if (string.Equals(a, styleName, StringComparison.OrdinalIgnoreCase))
			{
				return result;
			}
		}
		return null;
	}

	public GUIStyle GetStyle(string styleName)
	{
		GUIStyle gUIStyle = FindStyle(styleName);
		if (gUIStyle != null)
		{
			return gUIStyle;
		}
		Debug.LogWarning($"Unable to find style '{styleName}' in skin '{base.name}'");
		return new GUIStyle
		{
			name = "StyleNotFoundError"
		};
	}

	[IteratorStateMachine(typeof(_003CGetEnumerator_003Ed__85))]
	public IEnumerator GetEnumerator()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetEnumerator_003Ed__85(0)
		{
			_003C_003E4__this = this
		};
	}
}
