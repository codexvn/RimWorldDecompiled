using System;
using Brutal;
using Brutal.GlfwApi;
using Brutal.Numerics;
using UnityEngine;
using UnityEngine.InputSystem;
using Waylith;

namespace RimWorldWaylith;

internal sealed class GlfwInputBridge
{
	private const int GlfwModAlt = 4;

	private const int GlfwModControl = 2;

	private const int GlfwModSuper = 8;

	private readonly EngineApplication _app;

	private readonly GlfwWindow _window;

	private double2 _lastCursorPos;

	private bool _hasCursorPos;

	private bool _suppressDiagnosticShortcutRelease;

	private bool _suppressDisplayModeToggleRelease;

	public GlfwInputBridge(EngineApplication app)
	{
		_app = app;
		_window = app.Window;
		_window.OnCursorPos += OnCursorPos;
		_window.OnMouseButton += OnMouseButton;
		_window.OnScroll += OnScroll;
		_window.OnKey += OnKey;
		_window.OnChar += OnChar;
		_window.OnFocus += OnFocus;
		InputSystem.QueueMouseMove(ToUnityMousePosition(_window.CursorPos));
	}

	private void OnCursorPos(GlfwWindow window, double2 pos)
	{
		_lastCursorPos = pos;
		_hasCursorPos = true;
		InputSystem.QueueMouseMove(ToUnityMousePosition(pos));
	}

	private void OnMouseButton(GlfwWindow window, GlfwMouseButton button, GlfwButtonAction action, GlfwModifier mods)
	{
		if (action == GlfwButtonAction.Press || action == GlfwButtonAction.Release)
		{
			double2 currentCursorPos = GetCurrentCursorPos(window);
			Vector2 position = ToUnityMousePosition(currentCursorPos);
			InputSystem.QueueMouseMove(position);
			InputSystem.QueueMouseButton((int)button, action == GlfwButtonAction.Press, position);
		}
	}

	private void OnScroll(GlfwWindow window, double2 offset)
	{
		if (Platform.IsMacOS)
		{
			offset /= 3.0;
		}
		Vector2 position = ToUnityMousePosition(GetCurrentCursorPos(window));
		InputSystem.QueueMouseMove(position);
		InputSystem.QueueMouseScroll(new Vector2((float)offset.X, (float)offset.Y), position);
	}

	private void OnKey(GlfwWindow window, GlfwKey key, int scancode, GlfwKeyAction action, GlfwModifier mods)
	{
		if (TryHandleDiagnosticShortcut(key, action, mods) || IsDisplayModeToggle(key, action, mods))
		{
			return;
		}
		Key key2 = TranslateKey(key);
		if (key2 != 0)
		{
			if (action == GlfwKeyAction.Repeat)
			{
				InputSystem.QueueKeyRepeat(key2);
			}
			else
			{
				InputSystem.QueueKeyChange(key2, action == GlfwKeyAction.Press);
			}
		}
	}

	private void OnChar(GlfwWindow window, uint codepoint)
	{
		if (codepoint <= 1114111)
		{
			string text = char.ConvertFromUtf32((int)codepoint);
			for (int i = 0; i < text.Length; i++)
			{
				InputSystem.QueueTextInput(text[i]);
			}
		}
	}

	private void OnFocus(GlfwWindow window, GlfwFocusEvent focusEvent)
	{
		if (focusEvent == GlfwFocusEvent.LostFocus)
		{
			_hasCursorPos = false;
			_suppressDisplayModeToggleRelease = false;
			InputSystem.ResetAllInput();
		}
	}

	private bool TryHandleDiagnosticShortcut(GlfwKey key, GlfwKeyAction action, GlfwModifier mods)
	{
		if (_suppressDiagnosticShortcutRelease && action == GlfwKeyAction.Release && key == GlfwKey.GraveAccent)
		{
			_suppressDiagnosticShortcutRelease = false;
			return true;
		}
		if (action != GlfwKeyAction.Press || key != GlfwKey.GraveAccent || !HasDiagnosticShortcutModifiers(mods))
		{
			return false;
		}
		_suppressDiagnosticShortcutRelease = true;
		TriggerDiagnosticDump();
		return true;
	}

	private bool IsDisplayModeToggle(GlfwKey key, GlfwKeyAction action, GlfwModifier mods)
	{
		if (key != GlfwKey.Enter && key != GlfwKey.KpEnter)
		{
			return false;
		}
		if (_suppressDisplayModeToggleRelease && action == GlfwKeyAction.Release)
		{
			_suppressDisplayModeToggleRelease = false;
			return true;
		}
		if (action != GlfwKeyAction.Press || !HasDiagnosticShortcutModifiers(mods))
		{
			return false;
		}
		_suppressDisplayModeToggleRelease = true;
		return true;
	}

	private void TriggerDiagnosticDump()
	{
		try
		{
			Console.Out.WriteLine("[Diagnostics] Dump triggered by Alt+GraveAccent.");
			_app.DumpDiagnostics(null, "GlfwInputBridge.Alt+GraveAccent");
			string text = _app.TryWriteDiagnosticsReport("GlfwInputBridge.Alt+GraveAccent");
			if (!string.IsNullOrWhiteSpace(text))
			{
				Console.Out.WriteLine("[Diagnostics] Snapshot written to: " + text);
			}
		}
		catch (Exception value)
		{
			Console.Error.WriteLine($"[Diagnostics] Failed to dump diagnostics: {value}");
		}
	}

	private static bool HasDiagnosticShortcutModifiers(GlfwModifier mods)
	{
		bool flag = (mods & GlfwModifier.Alt) != 0;
		bool flag2 = (mods & GlfwModifier.Control) != 0;
		bool flag3 = (mods & GlfwModifier.Super) != 0;
		if (flag && !flag2)
		{
			return !flag3;
		}
		return false;
	}

	private double2 GetCurrentCursorPos(GlfwWindow window)
	{
		if (_hasCursorPos)
		{
			return _lastCursorPos;
		}
		return window.CursorPos;
	}

	private Vector2 ToUnityMousePosition(double2 pos)
	{
		int2 framebufferSize = _window.FramebufferSize;
		int2 size = _window.Size;
		float num = ((size.X > 0) ? ((float)framebufferSize.X / (float)size.X) : 1f);
		float num2 = ((size.Y > 0) ? ((float)framebufferSize.Y / (float)size.Y) : 1f);
		float x = (float)pos.X * num;
		float y = (float)framebufferSize.Y - (float)pos.Y * num2;
		return new Vector2(x, y);
	}

	private static Key TranslateKey(GlfwKey key)
	{
		return key switch
		{
			GlfwKey.A => Key.A, 
			GlfwKey.B => Key.B, 
			GlfwKey.C => Key.C, 
			GlfwKey.D => Key.D, 
			GlfwKey.E => Key.E, 
			GlfwKey.F => Key.F, 
			GlfwKey.G => Key.G, 
			GlfwKey.H => Key.H, 
			GlfwKey.I => Key.I, 
			GlfwKey.J => Key.J, 
			GlfwKey.K => Key.K, 
			GlfwKey.L => Key.L, 
			GlfwKey.M => Key.M, 
			GlfwKey.N => Key.N, 
			GlfwKey.O => Key.O, 
			GlfwKey.P => Key.P, 
			GlfwKey.Q => Key.Q, 
			GlfwKey.R => Key.R, 
			GlfwKey.S => Key.S, 
			GlfwKey.T => Key.T, 
			GlfwKey.U => Key.U, 
			GlfwKey.V => Key.V, 
			GlfwKey.W => Key.W, 
			GlfwKey.X => Key.X, 
			GlfwKey.Y => Key.Y, 
			GlfwKey.Z => Key.Z, 
			GlfwKey.Number0 => Key.Digit0, 
			GlfwKey.Number1 => Key.Digit1, 
			GlfwKey.Number2 => Key.Digit2, 
			GlfwKey.Number3 => Key.Digit3, 
			GlfwKey.Number4 => Key.Digit4, 
			GlfwKey.Number5 => Key.Digit5, 
			GlfwKey.Number6 => Key.Digit6, 
			GlfwKey.Number7 => Key.Digit7, 
			GlfwKey.Number8 => Key.Digit8, 
			GlfwKey.Number9 => Key.Digit9, 
			GlfwKey.Space => Key.Space, 
			GlfwKey.Tab => Key.Tab, 
			GlfwKey.Enter => Key.Enter, 
			GlfwKey.Escape => Key.Escape, 
			GlfwKey.Backspace => Key.Backspace, 
			GlfwKey.Delete => Key.Delete, 
			GlfwKey.Insert => Key.Insert, 
			GlfwKey.Home => Key.Home, 
			GlfwKey.End => Key.End, 
			GlfwKey.PageUp => Key.PageUp, 
			GlfwKey.PageDown => Key.PageDown, 
			GlfwKey.Left => Key.LeftArrow, 
			GlfwKey.Right => Key.RightArrow, 
			GlfwKey.Up => Key.UpArrow, 
			GlfwKey.Down => Key.DownArrow, 
			GlfwKey.LeftShift => Key.LeftShift, 
			GlfwKey.RightShift => Key.RightShift, 
			GlfwKey.LeftControl => Key.LeftCtrl, 
			GlfwKey.RightControl => Key.RightCtrl, 
			GlfwKey.LeftAlt => Key.LeftAlt, 
			GlfwKey.RightAlt => Key.AltGr, 
			GlfwKey.LeftSuper => Key.LeftApple, 
			GlfwKey.RightSuper => Key.RightApple, 
			GlfwKey.Menu => Key.ContextMenu, 
			GlfwKey.Apostrophe => Key.Quote, 
			GlfwKey.Comma => Key.Comma, 
			GlfwKey.Minus => Key.Minus, 
			GlfwKey.Period => Key.Period, 
			GlfwKey.Slash => Key.Slash, 
			GlfwKey.Semicolon => Key.Semicolon, 
			GlfwKey.Equal => Key.Equals, 
			GlfwKey.LeftBracket => Key.LeftBracket, 
			GlfwKey.Backslash => Key.Backslash, 
			GlfwKey.RightBracket => Key.RightBracket, 
			GlfwKey.GraveAccent => Key.Backquote, 
			GlfwKey.CapsLock => Key.CapsLock, 
			GlfwKey.ScrollLock => Key.ScrollLock, 
			GlfwKey.NumLock => Key.NumLock, 
			GlfwKey.PrintScreen => Key.PrintScreen, 
			GlfwKey.Pause => Key.Pause, 
			GlfwKey.Kp0 => Key.Numpad0, 
			GlfwKey.Kp1 => Key.Numpad1, 
			GlfwKey.Kp2 => Key.Numpad2, 
			GlfwKey.Kp3 => Key.Numpad3, 
			GlfwKey.Kp4 => Key.Numpad4, 
			GlfwKey.Kp5 => Key.Numpad5, 
			GlfwKey.Kp6 => Key.Numpad6, 
			GlfwKey.Kp7 => Key.Numpad7, 
			GlfwKey.Kp8 => Key.Numpad8, 
			GlfwKey.Kp9 => Key.Numpad9, 
			GlfwKey.KpDecimal => Key.NumpadPeriod, 
			GlfwKey.KpDivide => Key.NumpadDivide, 
			GlfwKey.KpMultiply => Key.NumpadMultiply, 
			GlfwKey.KpSubtract => Key.NumpadMinus, 
			GlfwKey.KpAdd => Key.NumpadPlus, 
			GlfwKey.KpEnter => Key.NumpadEnter, 
			GlfwKey.KpEqual => Key.NumpadEquals, 
			GlfwKey.F1 => Key.F1, 
			GlfwKey.F2 => Key.F2, 
			GlfwKey.F3 => Key.F3, 
			GlfwKey.F4 => Key.F4, 
			GlfwKey.F5 => Key.F5, 
			GlfwKey.F6 => Key.F6, 
			GlfwKey.F7 => Key.F7, 
			GlfwKey.F8 => Key.F8, 
			GlfwKey.F9 => Key.F9, 
			GlfwKey.F10 => Key.F10, 
			GlfwKey.F11 => Key.F11, 
			GlfwKey.F12 => Key.F12, 
			GlfwKey.F13 => Key.F13, 
			GlfwKey.F14 => Key.F14, 
			GlfwKey.F15 => Key.F15, 
			GlfwKey.F16 => Key.F16, 
			GlfwKey.F17 => Key.F17, 
			GlfwKey.F18 => Key.F18, 
			GlfwKey.F19 => Key.F19, 
			GlfwKey.F20 => Key.F20, 
			GlfwKey.F21 => Key.F21, 
			GlfwKey.F22 => Key.F22, 
			GlfwKey.F23 => Key.F23, 
			GlfwKey.F24 => Key.F24, 
			_ => Key.None, 
		};
	}
}
