using System;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;
using UnityStubs.Internal;

namespace UnityEngine.InputSystem;

public class Keyboard : InputDevice, ITextInputReceiver, IEventPreProcessor
{
	private readonly KeyControl[] _keys = new KeyControl[127];

	private readonly AnyKeyControl _anyKey;

	public const int ExtendedKeyCount = 126;

	public const int KeyCount = 110;

	public new int valueSizeInBytes => 0;

	public new Type valueType => typeof(object);

	public AnyKeyControl anyKey => _anyKey;

	public static Keyboard current
	{
		get
		{
			InputRuntime.EnsureInitialized();
			return InputRuntime.Keyboard;
		}
	}

	public KeyControl this[Key key]
	{
		get
		{
			if (key < Key.None || (int)key >= _keys.Length)
			{
				return null;
			}
			return _keys[(int)key];
		}
	}

	public KeyControl leftAltKey => this[Key.LeftAlt];

	public KeyControl leftCommandKey => this[Key.LeftApple];

	public KeyControl leftCtrlKey => this[Key.LeftCtrl];

	public KeyControl leftShiftKey => this[Key.LeftShift];

	public KeyControl rightAltKey => this[Key.AltGr];

	public KeyControl rightCommandKey => this[Key.RightApple];

	public KeyControl rightCtrlKey => this[Key.RightCtrl];

	public KeyControl rightShiftKey => this[Key.RightShift];

	public event Action<IMECompositionString> onIMECompositionChange;

	public event Action<char> onTextInput;

	public Keyboard()
		: base("<Keyboard>", "Keyboard")
	{
		_anyKey = new AnyKeyControl("<Keyboard>/anyKey", "Any Key");
		InputRuntime.RegisterControl(_anyKey);
		RegisterKey(Key.A, "a", "a");
		RegisterKey(Key.B, "b", "b");
		RegisterKey(Key.C, "c", "c");
		RegisterKey(Key.D, "d", "d");
		RegisterKey(Key.E, "e", "e");
		RegisterKey(Key.F, "f", "f");
		RegisterKey(Key.G, "g", "g");
		RegisterKey(Key.H, "h", "h");
		RegisterKey(Key.I, "i", "i");
		RegisterKey(Key.J, "j", "j");
		RegisterKey(Key.K, "k", "k");
		RegisterKey(Key.L, "l", "l");
		RegisterKey(Key.M, "m", "m");
		RegisterKey(Key.N, "n", "n");
		RegisterKey(Key.O, "o", "o");
		RegisterKey(Key.P, "p", "p");
		RegisterKey(Key.Q, "q", "q");
		RegisterKey(Key.R, "r", "r");
		RegisterKey(Key.S, "s", "s");
		RegisterKey(Key.T, "t", "t");
		RegisterKey(Key.U, "u", "u");
		RegisterKey(Key.V, "v", "v");
		RegisterKey(Key.W, "w", "w");
		RegisterKey(Key.X, "x", "x");
		RegisterKey(Key.Y, "y", "y");
		RegisterKey(Key.Z, "z", "z");
		RegisterKey(Key.Digit0, "digit0", "0");
		RegisterKey(Key.Digit1, "digit1", "1");
		RegisterKey(Key.Digit2, "digit2", "2");
		RegisterKey(Key.Digit3, "digit3", "3");
		RegisterKey(Key.Digit4, "digit4", "4");
		RegisterKey(Key.Digit5, "digit5", "5");
		RegisterKey(Key.Digit6, "digit6", "6");
		RegisterKey(Key.Digit7, "digit7", "7");
		RegisterKey(Key.Digit8, "digit8", "8");
		RegisterKey(Key.Digit9, "digit9", "9");
		RegisterKey(Key.Space, "space", "Space");
		RegisterKey(Key.Tab, "tab", "Tab");
		RegisterKey(Key.Enter, "enter", "Enter");
		RegisterKey(Key.Escape, "escape", "Escape");
		RegisterKey(Key.Backspace, "backspace", "Backspace");
		RegisterKey(Key.Delete, "delete", "Delete");
		RegisterKey(Key.Insert, "insert", "Insert");
		RegisterKey(Key.Home, "home", "Home");
		RegisterKey(Key.End, "end", "End");
		RegisterKey(Key.PageUp, "pageUp", "Page Up");
		RegisterKey(Key.PageDown, "pageDown", "Page Down");
		RegisterKey(Key.UpArrow, "upArrow", "Up");
		RegisterKey(Key.DownArrow, "downArrow", "Down");
		RegisterKey(Key.LeftArrow, "leftArrow", "Left");
		RegisterKey(Key.RightArrow, "rightArrow", "Right");
		RegisterKey(Key.LeftShift, "leftShift", "Left Shift");
		RegisterKey(Key.RightShift, "rightShift", "Right Shift");
		RegisterKey(Key.LeftCtrl, "leftCtrl", "Left Ctrl");
		RegisterKey(Key.RightCtrl, "rightCtrl", "Right Ctrl");
		RegisterKey(Key.LeftAlt, "leftAlt", "Left Alt");
		RegisterKey(Key.AltGr, "rightAlt", "Right Alt");
		RegisterKey(Key.LeftApple, "leftMeta", "Left Meta");
		RegisterKey(Key.RightApple, "rightMeta", "Right Meta");
		RegisterKey(Key.Backquote, "backquote", "`");
		RegisterKey(Key.Quote, "quote", "'");
		RegisterKey(Key.Semicolon, "semicolon", ";");
		RegisterKey(Key.Comma, "comma", ",");
		RegisterKey(Key.Period, "period", ".");
		RegisterKey(Key.Slash, "slash", "/");
		RegisterKey(Key.Backslash, "backslash", "\\");
		RegisterKey(Key.LeftBracket, "leftBracket", "[");
		RegisterKey(Key.RightBracket, "rightBracket", "]");
		RegisterKey(Key.Minus, "minus", "-");
		RegisterKey(Key.Equals, "equals", "=");
		RegisterKey(Key.CapsLock, "capsLock", "Caps Lock");
		RegisterKey(Key.NumLock, "numLock", "Num Lock");
		RegisterKey(Key.ScrollLock, "scrollLock", "Scroll Lock");
		RegisterKey(Key.PrintScreen, "printScreen", "Print Screen");
		RegisterKey(Key.Pause, "pause", "Pause");
		RegisterKey(Key.ContextMenu, "contextMenu", "Menu");
		RegisterKey(Key.Numpad0, "numpad0", "Num 0");
		RegisterKey(Key.Numpad1, "numpad1", "Num 1");
		RegisterKey(Key.Numpad2, "numpad2", "Num 2");
		RegisterKey(Key.Numpad3, "numpad3", "Num 3");
		RegisterKey(Key.Numpad4, "numpad4", "Num 4");
		RegisterKey(Key.Numpad5, "numpad5", "Num 5");
		RegisterKey(Key.Numpad6, "numpad6", "Num 6");
		RegisterKey(Key.Numpad7, "numpad7", "Num 7");
		RegisterKey(Key.Numpad8, "numpad8", "Num 8");
		RegisterKey(Key.Numpad9, "numpad9", "Num 9");
		RegisterKey(Key.NumpadDivide, "numpadDivide", "Num /");
		RegisterKey(Key.NumpadMultiply, "numpadMultiply", "Num *");
		RegisterKey(Key.NumpadMinus, "numpadMinus", "Num -");
		RegisterKey(Key.NumpadPlus, "numpadPlus", "Num +");
		RegisterKey(Key.NumpadEnter, "numpadEnter", "Num Enter");
		RegisterKey(Key.NumpadPeriod, "numpadPeriod", "Num .");
		RegisterKey(Key.NumpadEquals, "numpadEquals", "Num =");
		RegisterKey(Key.F1, "f1", "F1");
		RegisterKey(Key.F2, "f2", "F2");
		RegisterKey(Key.F3, "f3", "F3");
		RegisterKey(Key.F4, "f4", "F4");
		RegisterKey(Key.F5, "f5", "F5");
		RegisterKey(Key.F6, "f6", "F6");
		RegisterKey(Key.F7, "f7", "F7");
		RegisterKey(Key.F8, "f8", "F8");
		RegisterKey(Key.F9, "f9", "F9");
		RegisterKey(Key.F10, "f10", "F10");
		RegisterKey(Key.F11, "f11", "F11");
		RegisterKey(Key.F12, "f12", "F12");
		RegisterKey(Key.F13, "f13", "F13");
		RegisterKey(Key.F14, "f14", "F14");
		RegisterKey(Key.F15, "f15", "F15");
		RegisterKey(Key.F16, "f16", "F16");
		RegisterKey(Key.F17, "f17", "F17");
		RegisterKey(Key.F18, "f18", "F18");
		RegisterKey(Key.F19, "f19", "F19");
		RegisterKey(Key.F20, "f20", "F20");
		RegisterKey(Key.F21, "f21", "F21");
		RegisterKey(Key.F22, "f22", "F22");
		RegisterKey(Key.F23, "f23", "F23");
		RegisterKey(Key.F24, "f24", "F24");
		RegisterKey(Key.OEM1, "oem1", "OEM1");
		RegisterKey(Key.OEM2, "oem2", "OEM2");
		RegisterKey(Key.OEM3, "oem3", "OEM3");
		RegisterKey(Key.OEM4, "oem4", "OEM4");
		RegisterKey(Key.OEM5, "oem5", "OEM5");
	}

	private void RegisterKey(Key key, string controlName, string displayName)
	{
		if (key >= Key.None && (int)key < _keys.Length && _keys[(int)key] == null)
		{
			KeyControl keyControl = new KeyControl(key, "<Keyboard>/" + controlName, displayName);
			_keys[(int)key] = keyControl;
			InputRuntime.RegisterControl(keyControl);
		}
	}

	internal void BeginFrame()
	{
		for (int i = 0; i < _keys.Length; i++)
		{
			_keys[i]?.BeginFrame();
		}
		_anyKey.BeginFrame();
	}

	internal void ResetState()
	{
		for (int i = 0; i < _keys.Length; i++)
		{
			_keys[i]?.ResetState();
		}
		_anyKey.ResetState();
	}

	internal void UpdateAnyKeyState()
	{
		for (int i = 0; i < _keys.Length; i++)
		{
			KeyControl obj = _keys[i];
			if (obj != null && obj.isPressed)
			{
				_anyKey.SetPressed(pressed: true);
				return;
			}
		}
		_anyKey.SetPressed(pressed: false);
	}

	public new unsafe bool CompareValue(void* firstStatePtr, void* secondStatePtr)
	{
		return false;
	}

	public void OnIMECompositionChanged(IMECompositionString compositionString)
	{
		this.onIMECompositionChange?.Invoke(compositionString);
	}

	public void OnTextInput(char character)
	{
		this.onTextInput?.Invoke(character);
	}

	public new unsafe object ReadValueFromBufferAsObject(void* buffer, int bufferSize)
	{
		return null;
	}

	public new unsafe object ReadValueFromStateAsObject(void* statePtr)
	{
		return null;
	}

	public new unsafe void ReadValueFromStateIntoBuffer(void* statePtr, void* bufferPtr, int bufferSize)
	{
		StubWarner.LogStub("ReadValueFromStateIntoBuffer", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\Keyboard.cs", 208);
	}

	public bool PreProcessEvent(InputEventPtr currentEventPtr)
	{
		return false;
	}
}
