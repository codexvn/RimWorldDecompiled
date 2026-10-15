using System;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityStubs.Gui;
using UnityStubs.Internal;

namespace UnityEngine;

public class Input
{
	private static readonly Dictionary<string, KeyCode> KeyNames = new Dictionary<string, KeyCode>
	{
		["backspace"] = KeyCode.Backspace,
		["tab"] = KeyCode.Tab,
		["return"] = KeyCode.Return,
		["escape"] = KeyCode.Escape,
		["space"] = KeyCode.Space,
		["delete"] = KeyCode.Delete,
		["insert"] = KeyCode.Insert,
		["home"] = KeyCode.Home,
		["end"] = KeyCode.End,
		["page up"] = KeyCode.PageUp,
		["page down"] = KeyCode.PageDown,
		["up"] = KeyCode.UpArrow,
		["down"] = KeyCode.DownArrow,
		["left"] = KeyCode.LeftArrow,
		["right"] = KeyCode.RightArrow,
		["left shift"] = KeyCode.LeftShift,
		["right shift"] = KeyCode.RightShift,
		["left ctrl"] = KeyCode.LeftControl,
		["right ctrl"] = KeyCode.RightControl,
		["left alt"] = KeyCode.LeftAlt,
		["right alt"] = KeyCode.RightAlt,
		["left cmd"] = KeyCode.LeftApple,
		["right cmd"] = KeyCode.RightApple,
		["[+]"] = KeyCode.KeypadPlus,
		["[-]"] = KeyCode.KeypadMinus,
		["[*]"] = KeyCode.KeypadMultiply,
		["[/]"] = KeyCode.KeypadDivide,
		["[.]"] = KeyCode.KeypadPeriod,
		["enter"] = KeyCode.KeypadEnter,
		["`"] = KeyCode.BackQuote,
		[","] = KeyCode.Comma,
		["."] = KeyCode.Period,
		["/"] = KeyCode.Slash,
		[";"] = KeyCode.Semicolon,
		["'"] = KeyCode.Quote,
		["="] = KeyCode.Equals,
		["-"] = KeyCode.Minus,
		["["] = KeyCode.LeftBracket,
		["]"] = KeyCode.RightBracket,
		["\\"] = KeyCode.Backslash
	};

	public static string compositionString => ImeComposition.Text;

	public static Vector3 mousePosition
	{
		get
		{
			Vector2 vector = Mouse.current?.position.ReadValue() ?? Vector2.zero;
			return new Vector3(vector.x, vector.y, 0f);
		}
	}

	public static bool mousePresent => Mouse.current != null;

	public static bool simulateMouseWithTouches => false;

	public static int touchCount => 0;

	public static bool anyKey
	{
		get
		{
			Keyboard current = Keyboard.current;
			if ((current == null || !current.anyKey.isPressed) && !GetMouseButton(0) && !GetMouseButton(1))
			{
				return GetMouseButton(2);
			}
			return true;
		}
	}

	public static bool anyKeyDown
	{
		get
		{
			Keyboard current = Keyboard.current;
			if ((current == null || !current.anyKey.WasPressedThisFrame) && !GetMouseButtonDown(0) && !GetMouseButtonDown(1))
			{
				return GetMouseButtonDown(2);
			}
			return true;
		}
	}

	public static Vector2 mouseScrollDelta => Mouse.current?.scroll.ReadValue() ?? Vector2.zero;

	public static Vector2 compositionCursorPos
	{
		set
		{
			ImeComposition.SetCandidatePos(GuiBackend.Current?.GuiToScreenPoint(value) ?? value);
		}
	}

	public static IMECompositionMode imeCompositionMode
	{
		get
		{
			return ImeComposition.Mode;
		}
		set
		{
			ImeComposition.Mode = value;
		}
	}

	internal static void get_mousePosition_Injected(out Vector3 ret)
	{
		ret = mousePosition;
	}

	public static bool GetKey(KeyCode key)
	{
		if (!SwallowedByTextField(key))
		{
			return GetKeyRaw(key);
		}
		return false;
	}

	public static bool GetKeyDown(KeyCode key)
	{
		if (!SwallowedByTextField(key))
		{
			return GetKeyDownRaw(key);
		}
		return false;
	}

	public static bool GetKeyUp(KeyCode key)
	{
		if (!SwallowedByTextField(key))
		{
			return GetKeyUpRaw(key);
		}
		return false;
	}

	public static bool GetKey(string name)
	{
		return GetKey(KeyCodeFromName(name));
	}

	public static bool GetKeyDown(string name)
	{
		return GetKeyDown(KeyCodeFromName(name));
	}

	public static bool GetKeyUp(string name)
	{
		return GetKeyUp(KeyCodeFromName(name));
	}

	internal static bool GetKeyRaw(KeyCode key)
	{
		if (!IsPressed(key))
		{
			return IsPressed(MacCommandAlias(key));
		}
		return true;
	}

	internal static bool GetKeyDownRaw(KeyCode key)
	{
		if (!WasPressed(key))
		{
			return WasPressed(MacCommandAlias(key));
		}
		return true;
	}

	internal static bool GetKeyUpRaw(KeyCode key)
	{
		if (!WasReleased(key))
		{
			return WasReleased(MacCommandAlias(key));
		}
		return true;
	}

	internal static bool GetKeyRepeatRaw(KeyCode key)
	{
		if (!WasRepeated(key))
		{
			return WasRepeated(MacCommandAlias(key));
		}
		return true;
	}

	private static bool IsPressed(KeyCode key)
	{
		if (TryGetKeyControl(key, out var control))
		{
			return control.isPressed;
		}
		return false;
	}

	private static bool WasPressed(KeyCode key)
	{
		if (TryGetKeyControl(key, out var control))
		{
			return control.WasPressedThisFrame;
		}
		return false;
	}

	private static bool WasReleased(KeyCode key)
	{
		if (TryGetKeyControl(key, out var control))
		{
			return control.WasReleasedThisFrame;
		}
		return false;
	}

	private static bool WasRepeated(KeyCode key)
	{
		if (TryGetKeyControl(key, out var control))
		{
			return control.WasRepeatedThisFrame;
		}
		return false;
	}

	private static KeyCode MacCommandAlias(KeyCode key)
	{
		if (!OperatingSystem.IsMacOS())
		{
			return KeyCode.None;
		}
		return key switch
		{
			KeyCode.LeftControl => KeyCode.LeftApple, 
			KeyCode.RightControl => KeyCode.RightApple, 
			_ => KeyCode.None, 
		};
	}

	private static bool SwallowedByTextField(KeyCode key)
	{
		if ((uint)(key - 303) > 9u)
		{
			return TextFocus.Consumes(key);
		}
		return false;
	}

	private static KeyCode KeyCodeFromName(string name)
	{
		ArgumentNullException.ThrowIfNull(name, "name");
		if (name.Length == 1)
		{
			char c = name[0];
			if (c >= 'a' && c <= 'z')
			{
				return (KeyCode)(97 + (c - 97));
			}
			if (c >= '0' && c <= '9')
			{
				return (KeyCode)(48 + (c - 48));
			}
		}
		if (name.Length == 3 && name[0] == '[' && name[2] == ']')
		{
			char c2 = name[1];
			if (c2 >= '0' && c2 <= '9')
			{
				return (KeyCode)(256 + (name[1] - 48));
			}
		}
		int length = name.Length;
		bool flag = (uint)(length - 2) <= 1u;
		if (flag && name[0] == 'f' && int.TryParse(name.AsSpan(1), out var result) && result >= 1 && result <= 15)
		{
			return (KeyCode)(282 + (result - 1));
		}
		if (!KeyNames.TryGetValue(name, out var value))
		{
			throw new ArgumentException("Input Key named: " + name + " is unknown");
		}
		return value;
	}

	public static bool GetMouseButton(int button)
	{
		Mouse current = Mouse.current;
		if (current == null)
		{
			return false;
		}
		return current.GetButtonControl(button)?.isPressed == true;
	}

	public static bool GetMouseButtonDown(int button)
	{
		Mouse current = Mouse.current;
		if (current == null)
		{
			return false;
		}
		return current.GetButtonControl(button)?.WasPressedThisFrame == true;
	}

	public static bool GetMouseButtonUp(int button)
	{
		Mouse current = Mouse.current;
		if (current == null)
		{
			return false;
		}
		return current.GetButtonControl(button)?.WasReleasedThisFrame == true;
	}

	public static float GetAxis(string axisName)
	{
		switch (axisName)
		{
		case "Horizontal":
			return Axis(KeyCode.D, KeyCode.RightArrow) - Axis(KeyCode.A, KeyCode.LeftArrow);
		case "Vertical":
			return Axis(KeyCode.W, KeyCode.UpArrow) - Axis(KeyCode.S, KeyCode.DownArrow);
		case "Mouse X":
			return Mouse.current?.delta.ReadValue().x ?? 0f;
		case "Mouse Y":
			return Mouse.current?.delta.ReadValue().y ?? 0f;
		case "Mouse ScrollWheel":
			return mouseScrollDelta.y;
		default:
			StubWarner.LogStub("GetAxis", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\Input.cs", 120);
			return 0f;
		}
		static float Axis(KeyCode a, KeyCode b)
		{
			if (!GetKey(a) && !GetKey(b))
			{
				return 0f;
			}
			return 1f;
		}
	}

	public static float GetAxisRaw(string axisName)
	{
		return GetAxis(axisName);
	}

	public static Touch GetTouch(int index)
	{
		StubWarner.LogStub("GetTouch", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\Input.cs", 135);
		return default(Touch);
	}

	private static bool TryGetKeyControl(KeyCode keyCode, out KeyControl control)
	{
		control = null;
		Key key;
		switch (keyCode)
		{
		case KeyCode.A:
			key = Key.A;
			break;
		case KeyCode.B:
			key = Key.B;
			break;
		case KeyCode.C:
			key = Key.C;
			break;
		case KeyCode.D:
			key = Key.D;
			break;
		case KeyCode.E:
			key = Key.E;
			break;
		case KeyCode.F:
			key = Key.F;
			break;
		case KeyCode.G:
			key = Key.G;
			break;
		case KeyCode.H:
			key = Key.H;
			break;
		case KeyCode.I:
			key = Key.I;
			break;
		case KeyCode.J:
			key = Key.J;
			break;
		case KeyCode.K:
			key = Key.K;
			break;
		case KeyCode.L:
			key = Key.L;
			break;
		case KeyCode.M:
			key = Key.M;
			break;
		case KeyCode.N:
			key = Key.N;
			break;
		case KeyCode.O:
			key = Key.O;
			break;
		case KeyCode.P:
			key = Key.P;
			break;
		case KeyCode.Q:
			key = Key.Q;
			break;
		case KeyCode.R:
			key = Key.R;
			break;
		case KeyCode.S:
			key = Key.S;
			break;
		case KeyCode.T:
			key = Key.T;
			break;
		case KeyCode.U:
			key = Key.U;
			break;
		case KeyCode.V:
			key = Key.V;
			break;
		case KeyCode.W:
			key = Key.W;
			break;
		case KeyCode.X:
			key = Key.X;
			break;
		case KeyCode.Y:
			key = Key.Y;
			break;
		case KeyCode.Z:
			key = Key.Z;
			break;
		case KeyCode.Alpha0:
			key = Key.Digit0;
			break;
		case KeyCode.Alpha1:
			key = Key.Digit1;
			break;
		case KeyCode.Alpha2:
			key = Key.Digit2;
			break;
		case KeyCode.Alpha3:
			key = Key.Digit3;
			break;
		case KeyCode.Alpha4:
			key = Key.Digit4;
			break;
		case KeyCode.Alpha5:
			key = Key.Digit5;
			break;
		case KeyCode.Alpha6:
			key = Key.Digit6;
			break;
		case KeyCode.Alpha7:
			key = Key.Digit7;
			break;
		case KeyCode.Alpha8:
			key = Key.Digit8;
			break;
		case KeyCode.Alpha9:
			key = Key.Digit9;
			break;
		case KeyCode.LeftShift:
			key = Key.LeftShift;
			break;
		case KeyCode.RightShift:
			key = Key.RightShift;
			break;
		case KeyCode.LeftAlt:
			key = Key.LeftAlt;
			break;
		case KeyCode.RightAlt:
			key = Key.AltGr;
			break;
		case KeyCode.LeftControl:
			key = Key.LeftCtrl;
			break;
		case KeyCode.RightControl:
			key = Key.RightCtrl;
			break;
		case KeyCode.LeftApple:
		case KeyCode.LeftWindows:
			key = Key.LeftApple;
			break;
		case KeyCode.RightApple:
		case KeyCode.RightWindows:
			key = Key.RightApple;
			break;
		case KeyCode.Keypad0:
			key = Key.Numpad0;
			break;
		case KeyCode.Tab:
			key = Key.Tab;
			break;
		case KeyCode.Return:
			key = Key.Enter;
			break;
		case KeyCode.Escape:
			key = Key.Escape;
			break;
		case KeyCode.Space:
			key = Key.Space;
			break;
		case KeyCode.Backspace:
			key = Key.Backspace;
			break;
		case KeyCode.Delete:
			key = Key.Delete;
			break;
		case KeyCode.UpArrow:
			key = Key.UpArrow;
			break;
		case KeyCode.DownArrow:
			key = Key.DownArrow;
			break;
		case KeyCode.LeftArrow:
			key = Key.LeftArrow;
			break;
		case KeyCode.RightArrow:
			key = Key.RightArrow;
			break;
		case KeyCode.Home:
			key = Key.Home;
			break;
		case KeyCode.End:
			key = Key.End;
			break;
		case KeyCode.PageUp:
			key = Key.PageUp;
			break;
		case KeyCode.PageDown:
			key = Key.PageDown;
			break;
		case KeyCode.Insert:
			key = Key.Insert;
			break;
		case KeyCode.KeypadEnter:
			key = Key.NumpadEnter;
			break;
		case KeyCode.Keypad1:
			key = Key.Numpad1;
			break;
		case KeyCode.Keypad2:
			key = Key.Numpad2;
			break;
		case KeyCode.Keypad3:
			key = Key.Numpad3;
			break;
		case KeyCode.Keypad4:
			key = Key.Numpad4;
			break;
		case KeyCode.Keypad5:
			key = Key.Numpad5;
			break;
		case KeyCode.Keypad6:
			key = Key.Numpad6;
			break;
		case KeyCode.Keypad7:
			key = Key.Numpad7;
			break;
		case KeyCode.Keypad8:
			key = Key.Numpad8;
			break;
		case KeyCode.Keypad9:
			key = Key.Numpad9;
			break;
		case KeyCode.KeypadPlus:
			key = Key.NumpadPlus;
			break;
		case KeyCode.KeypadMinus:
			key = Key.NumpadMinus;
			break;
		case KeyCode.KeypadMultiply:
			key = Key.NumpadMultiply;
			break;
		case KeyCode.KeypadDivide:
			key = Key.NumpadDivide;
			break;
		case KeyCode.KeypadPeriod:
			key = Key.NumpadPeriod;
			break;
		case KeyCode.BackQuote:
			key = Key.Backquote;
			break;
		case KeyCode.Comma:
			key = Key.Comma;
			break;
		case KeyCode.Period:
			key = Key.Period;
			break;
		case KeyCode.Slash:
			key = Key.Slash;
			break;
		case KeyCode.Semicolon:
			key = Key.Semicolon;
			break;
		case KeyCode.Quote:
			key = Key.Quote;
			break;
		case KeyCode.Equals:
			key = Key.Equals;
			break;
		case KeyCode.Minus:
			key = Key.Minus;
			break;
		case KeyCode.LeftBracket:
			key = Key.LeftBracket;
			break;
		case KeyCode.RightBracket:
			key = Key.RightBracket;
			break;
		case KeyCode.Backslash:
			key = Key.Backslash;
			break;
		case KeyCode.F1:
			key = Key.F1;
			break;
		case KeyCode.F2:
			key = Key.F2;
			break;
		case KeyCode.F3:
			key = Key.F3;
			break;
		case KeyCode.F4:
			key = Key.F4;
			break;
		case KeyCode.F5:
			key = Key.F5;
			break;
		case KeyCode.F6:
			key = Key.F6;
			break;
		case KeyCode.F7:
			key = Key.F7;
			break;
		case KeyCode.F8:
			key = Key.F8;
			break;
		case KeyCode.F9:
			key = Key.F9;
			break;
		case KeyCode.F10:
			key = Key.F10;
			break;
		case KeyCode.F11:
			key = Key.F11;
			break;
		case KeyCode.F12:
			key = Key.F12;
			break;
		default:
			key = Key.None;
			break;
		}
		Key key2 = key;
		if (key2 == Key.None)
		{
			return false;
		}
		control = Keyboard.current?[key2];
		return control != null;
	}
}
