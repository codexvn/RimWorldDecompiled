using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RimWorldWaylith.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityStubs.Internal;
using Waylith.Input;
using Waylith.Profiling;
using Waylith.UnityCompat;

namespace RimWorldWaylith.Imgui;

internal sealed class ImguiDispatch
{
	private readonly ImGuiGuiBackend backend;

	private readonly Action<object> beginBehaviourGUI;

	private readonly Action<object> endBehaviourGUI = delegate
	{
		GUILayoutUtility.EndBehaviourGUI();
	};

	private Vector2 _lastPhysMouse;

	private bool _hasLastPhysMouse;

	private Vector2 _mouseDelta;

	private static readonly KeyCode[] ControlKeys = new KeyCode[14]
	{
		KeyCode.Backspace,
		KeyCode.Delete,
		KeyCode.LeftArrow,
		KeyCode.RightArrow,
		KeyCode.UpArrow,
		KeyCode.DownArrow,
		KeyCode.Home,
		KeyCode.End,
		KeyCode.PageUp,
		KeyCode.PageDown,
		KeyCode.Return,
		KeyCode.KeypadEnter,
		KeyCode.Escape,
		KeyCode.Tab
	};

	private static readonly KeyCode[] GameKeys = new KeyCode[75]
	{
		KeyCode.A,
		KeyCode.B,
		KeyCode.C,
		KeyCode.D,
		KeyCode.E,
		KeyCode.F,
		KeyCode.G,
		KeyCode.H,
		KeyCode.I,
		KeyCode.J,
		KeyCode.K,
		KeyCode.L,
		KeyCode.M,
		KeyCode.N,
		KeyCode.O,
		KeyCode.P,
		KeyCode.Q,
		KeyCode.R,
		KeyCode.S,
		KeyCode.T,
		KeyCode.U,
		KeyCode.V,
		KeyCode.W,
		KeyCode.X,
		KeyCode.Y,
		KeyCode.Z,
		KeyCode.Alpha0,
		KeyCode.Alpha1,
		KeyCode.Alpha2,
		KeyCode.Alpha3,
		KeyCode.Alpha4,
		KeyCode.Alpha5,
		KeyCode.Alpha6,
		KeyCode.Alpha7,
		KeyCode.Alpha8,
		KeyCode.Alpha9,
		KeyCode.Space,
		KeyCode.BackQuote,
		KeyCode.Comma,
		KeyCode.Period,
		KeyCode.Slash,
		KeyCode.Semicolon,
		KeyCode.Quote,
		KeyCode.Equals,
		KeyCode.Minus,
		KeyCode.LeftBracket,
		KeyCode.RightBracket,
		KeyCode.Backslash,
		KeyCode.F1,
		KeyCode.F2,
		KeyCode.F3,
		KeyCode.F4,
		KeyCode.F5,
		KeyCode.F6,
		KeyCode.F7,
		KeyCode.F8,
		KeyCode.F9,
		KeyCode.F10,
		KeyCode.F11,
		KeyCode.F12,
		KeyCode.Keypad0,
		KeyCode.Keypad1,
		KeyCode.Keypad2,
		KeyCode.Keypad3,
		KeyCode.Keypad4,
		KeyCode.Keypad5,
		KeyCode.Keypad6,
		KeyCode.Keypad7,
		KeyCode.Keypad8,
		KeyCode.Keypad9,
		KeyCode.KeypadPlus,
		KeyCode.KeypadMinus,
		KeyCode.KeypadMultiply,
		KeyCode.KeypadDivide,
		KeyCode.KeypadPeriod
	};

	private static readonly KeyCode[] EditShortcutKeys = new KeyCode[4]
	{
		KeyCode.A,
		KeyCode.C,
		KeyCode.V,
		KeyCode.X
	};

	private readonly List<char> _typedChars = new List<char>();

	private bool _charHooked;

	private const float ScrollEventUnitsPerNotch = 3f;

	private const int FirstControlId = 1000;

	private const int ControlIdsPerSlot = 100000;

	private const int ControlIdSlots = 20000;

	private readonly ConditionalWeakTable<object, StrongBox<int>> controlIdSlots = new ConditionalWeakTable<object, StrongBox<int>>();

	private int nextControlIdSlot = 1;

	public ImguiDispatch(ImGuiGuiBackend backend)
	{
		this.backend = backend;
		beginBehaviourGUI = BeginBehaviourGUI;
	}

	public void DispatchInput(ImGuiHost host)
	{
		backend.BeginInputPhase();
		try
		{
			Vector2 vector = ReadPhysMouse(host);
			EventModifiers mods = ReadModifiers();
			_mouseDelta = (_hasLastPhysMouse ? (vector - _lastPhysMouse) : Vector2.zero);
			_lastPhysMouse = vector;
			_hasLastPhysMouse = true;
			for (int i = 0; i < 3; i++)
			{
				if (Input.GetMouseButtonDown(i))
				{
					RunInputPass(EventType.mouseDown, i, vector, mods);
				}
			}
			if (_mouseDelta != Vector2.zero)
			{
				for (int j = 0; j < 3; j++)
				{
					if (Input.GetMouseButton(j) && !Input.GetMouseButtonDown(j))
					{
						RunInputPass(EventType.mouseDrag, j, vector, mods);
					}
				}
			}
			for (int k = 0; k < 3; k++)
			{
				if (Input.GetMouseButtonUp(k))
				{
					RunInputPass(EventType.mouseUp, k, vector, mods);
					if (k == 0)
					{
						GUIUtility.hotControl = 0;
					}
				}
			}
			DispatchKeyboard(vector, mods);
			Vector2 mouseScrollDelta = Input.mouseScrollDelta;
			if (mouseScrollDelta.x != 0f || mouseScrollDelta.y != 0f)
			{
				RunPass(EventType.layout, 0, vector, mods, drawing: false, _mouseDelta);
				RunScrollPass(vector, mouseScrollDelta, mods);
			}
		}
		catch (Exception value)
		{
			Console.Error.WriteLine($"[ImguiDispatch] input dispatch threw: {value}");
		}
		finally
		{
			backend.EndInputPhase();
		}
	}

	public void Draw(ImGuiHost host)
	{
		backend.BeginFrame();
		UnityBuiltinGlobals.PublishScreen();
		try
		{
			Vector2 physMouse = ReadPhysMouse(host);
			EventModifiers mods = ReadModifiers();
			RunPass(EventType.layout, 0, physMouse, mods, drawing: false, _mouseDelta);
			RunPass(EventType.repaint, 0, physMouse, mods, drawing: true, _mouseDelta);
		}
		catch (Exception value)
		{
			Console.Error.WriteLine($"[ImguiDispatch] draw threw: {value}");
		}
		finally
		{
			backend.EndFrame();
		}
	}

	private static Vector2 ReadPhysMouse(ImGuiHost host)
	{
		Vector3 mousePosition = Input.mousePosition;
		return new Vector2(mousePosition.x, host.DisplayHeight - mousePosition.y);
	}

	private void RunInputPass(EventType type, int button, Vector2 physMouse, EventModifiers mods)
	{
		RunPass(EventType.layout, 0, physMouse, mods, drawing: false, _mouseDelta);
		RunPass(type, button, physMouse, mods, drawing: false, _mouseDelta);
	}

	private void RunPass(EventType type, int button, Vector2 physMouse, EventModifiers mods, bool drawing, Vector2 delta = default(Vector2))
	{
		Event.current = new Event
		{
			type = type,
			rawType = type,
			button = button,
			clickCount = ((type == EventType.mouseDown) ? ClickCountFor(button) : 0),
			delta = delta,
			modifiers = mods,
			shift = ((mods & EventModifiers.Shift) != 0),
			alt = ((mods & EventModifiers.Alt) != 0)
		};
		Dispatch(physMouse, drawing);
	}

	private static int ClickCountFor(int button)
	{
		IReadOnlyList<InputEvent> eventsThisFrame = InputState.EventsThisFrame;
		for (int num = eventsThisFrame.Count - 1; num >= 0; num--)
		{
			if (eventsThisFrame[num].rawType == InputEventType.MouseDown && eventsThisFrame[num].mouseButton == (MouseButton)button)
			{
				return eventsThisFrame[num].clickCount;
			}
		}
		return 1;
	}

	private void DispatchKeyboard(Vector2 physMouse, EventModifiers mods)
	{
		EnsureCharHook();
		bool flag = (mods & EventModifiers.Control) != 0;
		if (_typedChars.Count > 0)
		{
			char[] array = _typedChars.ToArray();
			_typedChars.Clear();
			char[] array2 = array;
			foreach (char character in array2)
			{
				RunKeyPass(KeyCode.None, character, physMouse, mods);
			}
		}
		KeyCode[] controlKeys = ControlKeys;
		foreach (KeyCode keyCode in controlKeys)
		{
			if (RawKeys.GetKeyDownOrRepeat(keyCode) && (!OperatingSystem.IsWindows() || !WindowsImeBridge.Consumed(keyCode)))
			{
				RunKeyPass(keyCode, '\0', physMouse, mods);
			}
		}
		if (flag)
		{
			controlKeys = EditShortcutKeys;
			foreach (KeyCode keyCode2 in controlKeys)
			{
				if (RawKeys.GetKeyDownOrRepeat(keyCode2) && TextFocus.Consumes(keyCode2))
				{
					RunKeyPass(keyCode2, '\0', physMouse, mods);
				}
			}
		}
		if (!TextFocus.CapturesText)
		{
			controlKeys = GameKeys;
			foreach (KeyCode keyCode3 in controlKeys)
			{
				if (RawKeys.GetKeyDownOrRepeat(keyCode3) && !TextFocus.Consumes(keyCode3))
				{
					RunKeyPass(keyCode3, '\0', physMouse, mods);
				}
			}
		}
		controlKeys = ControlKeys;
		foreach (KeyCode keyCode4 in controlKeys)
		{
			if (RawKeys.GetKeyUp(keyCode4))
			{
				RunKeyPass(keyCode4, '\0', physMouse, mods, EventType.keyUp);
			}
		}
		controlKeys = GameKeys;
		foreach (KeyCode keyCode5 in controlKeys)
		{
			if (RawKeys.GetKeyUp(keyCode5))
			{
				RunKeyPass(keyCode5, '\0', physMouse, mods, EventType.keyUp);
			}
		}
	}

	private void EnsureCharHook()
	{
		if (!_charHooked)
		{
			Keyboard current = Keyboard.current;
			if (current != null)
			{
				current.onTextInput += OnTextInput;
				_charHooked = true;
			}
		}
	}

	private void OnTextInput(char c)
	{
		_typedChars.Add(c);
	}

	private void RunKeyPass(KeyCode keyCode, char character, Vector2 physMouse, EventModifiers mods, EventType type = EventType.keyDown)
	{
		RunPass(EventType.layout, 0, physMouse, mods, drawing: false, _mouseDelta);
		Event.current = new Event
		{
			type = type,
			rawType = type,
			keyCode = keyCode,
			character = character,
			modifiers = mods,
			shift = ((mods & EventModifiers.Shift) != 0),
			alt = ((mods & EventModifiers.Alt) != 0)
		};
		Dispatch(physMouse, drawing: false);
	}

	private void RunScrollPass(Vector2 physMouse, Vector2 scroll, EventModifiers mods)
	{
		RunPass(EventType.layout, 0, physMouse, mods, drawing: false, _mouseDelta);
		if ((mods & EventModifiers.Shift) != 0 && scroll.x == 0f)
		{
			scroll = new Vector2(scroll.y, 0f);
		}
		Event.current = new Event
		{
			type = EventType.scrollWheel,
			rawType = EventType.scrollWheel,
			delta = new Vector2(0f - scroll.x, 0f - scroll.y) * 3f,
			modifiers = mods,
			shift = ((mods & EventModifiers.Shift) != 0),
			alt = ((mods & EventModifiers.Alt) != 0)
		};
		Dispatch(physMouse, drawing: false);
	}

	private void Dispatch(Vector2 physMouse, bool drawing)
	{
		GUI.matrix = Matrix4x4.identity;
		GUIUtility.ResetControlIds();
		backend.BeginPass(drawing, physMouse);
		try
		{
			using (FrameProfiler.Scope($"GUI.DispatchInputToWindows ({Event.current.type})"))
			{
				GUI.DispatchInputToWindows();
			}
			Event current = Event.current;
			bool flag = backend.InlineControlsObscured;
			if (flag)
			{
				bool flag2;
				switch (current.rawType)
				{
				case EventType.mouseDown:
				case EventType.mouseUp:
				case EventType.mouseDrag:
				case EventType.scrollWheel:
					flag2 = true;
					break;
				default:
					flag2 = false;
					break;
				}
				flag = flag2;
			}
			if (flag)
			{
				current.type = EventType.used;
			}
			using (FrameProfiler.Scope($"OnGUI ({current.rawType})"))
			{
				UnityRuntime.Current?.World.DispatchOnGUI(current.rawType == EventType.repaint, beginBehaviourGUI, endBehaviourGUI);
			}
			using (FrameProfiler.Scope($"GUI.ProcessPendingWindows ({current.rawType})"))
			{
				GUI.ProcessPendingWindows();
			}
		}
		catch (Exception value)
		{
			GUI.ClearPendingWindows();
			Console.Error.WriteLine($"[ImguiDispatch] OnGUI threw: {value}");
		}
		finally
		{
			backend.EndPass();
		}
	}

	private void BeginBehaviourGUI(object behaviour)
	{
		GUI.matrix = Matrix4x4.identity;
		backend.BalanceGroups();
		if (!controlIdSlots.TryGetValue(behaviour, out StrongBox<int> value))
		{
			controlIdSlots.Add(behaviour, value = new StrongBox<int>(nextControlIdSlot++));
		}
		GUIUtility.ResetControlIds(1000 + value.Value % 20000 * 100000);
		GUILayoutUtility.BeginBehaviourGUI(((UnityEngine.Object)behaviour).GetInstanceID());
	}

	private static EventModifiers ReadModifiers()
	{
		EventModifiers eventModifiers = EventModifiers.None;
		if (RawKeys.GetKey(KeyCode.LeftShift) || RawKeys.GetKey(KeyCode.RightShift))
		{
			eventModifiers |= EventModifiers.Shift;
		}
		if (RawKeys.GetKey(KeyCode.LeftAlt) || RawKeys.GetKey(KeyCode.RightAlt))
		{
			eventModifiers |= EventModifiers.Alt;
		}
		if (RawKeys.GetKey(KeyCode.LeftControl) || RawKeys.GetKey(KeyCode.RightControl))
		{
			eventModifiers |= EventModifiers.Control;
		}
		if (RawKeys.GetKey(KeyCode.LeftApple) || RawKeys.GetKey(KeyCode.RightApple))
		{
			eventModifiers |= EventModifiers.Command;
		}
		return eventModifiers;
	}
}
