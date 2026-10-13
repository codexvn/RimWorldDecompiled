using System;
using System.Numerics;
using Brutal.GlfwApi;
using Brutal.Numerics;

namespace Waylith.Input;

internal sealed class GlfwInputSource
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

	private bool _isFocused = true;

	public GlfwInputSource(EngineApplication app)
	{
		_app = app;
		_window = app.Window;
		_window.OnCursorPos += OnCursorPos;
		_window.OnMouseButton += OnMouseButton;
		_window.OnScroll += OnScroll;
		_window.OnKey += OnKey;
		_window.OnChar += OnChar;
		_window.OnFocus += OnFocus;
		_window.OnCursorEnter += OnCursorEnter;
		KeyboardLayout.Available = true;
		InputState.OnMouseMove(ToFramebufferPosition(_window.CursorPos));
	}

	private void OnCursorPos(GlfwWindow window, double2 pos)
	{
		_lastCursorPos = pos;
		_hasCursorPos = true;
		InputState.OnMouseMove(ToFramebufferPosition(pos));
	}

	private void OnMouseButton(GlfwWindow window, GlfwMouseButton button, GlfwButtonAction action, GlfwModifier mods)
	{
		if (action == GlfwButtonAction.Press || action == GlfwButtonAction.Release)
		{
			Vector2 position = ToFramebufferPosition(GetCurrentCursorPos(window));
			InputState.OnMouseMove(position);
			InputState.OnMouseButton((MouseButton)button, action == GlfwButtonAction.Press, position);
		}
	}

	private void OnScroll(GlfwWindow window, double2 offset)
	{
		if (OperatingSystem.IsMacOS())
		{
			offset /= 3.0;
		}
		Vector2 position = ToFramebufferPosition(GetCurrentCursorPos(window));
		InputState.OnMouseMove(position);
		InputState.OnScroll(new Vector2((float)offset.X, (float)offset.Y), position);
	}

	private void OnKey(GlfwWindow window, GlfwKey key, int scancode, GlfwKeyAction action, GlfwModifier mods)
	{
		if ((action == GlfwKeyAction.Press || action == GlfwKeyAction.Release) && _isFocused && !TryHandleDiagnosticShortcut(key, action, mods) && !TryHandleDisplayModeToggle(key, action, mods))
		{
			Key key2 = GlfwKeyMap.ToKey(key);
			if (key2 != 0)
			{
				InputState.OnKey(key2, action == GlfwKeyAction.Press);
			}
		}
	}

	private void OnChar(GlfwWindow window, uint codepoint)
	{
		if (_isFocused && codepoint <= 1114111)
		{
			string text = char.ConvertFromUtf32((int)codepoint);
			for (int i = 0; i < text.Length; i++)
			{
				InputState.OnTextInput(text[i]);
			}
		}
	}

	private void OnFocus(GlfwWindow window, GlfwFocusEvent focusEvent)
	{
		_isFocused = focusEvent == GlfwFocusEvent.GainFocus;
		_hasCursorPos = false;
		_suppressDiagnosticShortcutRelease = false;
		_suppressDisplayModeToggleRelease = false;
		InputState.ResetAll();
		if (_isFocused)
		{
			KeyboardLayout.Invalidate();
		}
	}

	private void OnCursorEnter(GlfwWindow window, bool entered)
	{
		EngineWindow.IsHovered = entered;
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

	private bool TryHandleDisplayModeToggle(GlfwKey key, GlfwKeyAction action, GlfwModifier mods)
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
		_app.RequestDisplayModeToggle();
		return true;
	}

	private void TriggerDiagnosticDump()
	{
		try
		{
			Console.Out.WriteLine("[Diagnostics] Dump triggered by Alt+GraveAccent.");
			_app.DumpDiagnostics(null, "GlfwInputSource.Alt+GraveAccent");
			string text = _app.TryWriteDiagnosticsReport("GlfwInputSource.Alt+GraveAccent");
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

	private Vector2 ToFramebufferPosition(double2 pos)
	{
		int2 framebufferSize = _window.FramebufferSize;
		int2 size = _window.Size;
		float num = ((size.X > 0) ? ((float)framebufferSize.X / (float)size.X) : 1f);
		float num2 = ((size.Y > 0) ? ((float)framebufferSize.Y / (float)size.Y) : 1f);
		return new Vector2((float)pos.X * num, (float)pos.Y * num2);
	}
}
