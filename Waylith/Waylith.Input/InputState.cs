using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;

namespace Waylith.Input;

public static class InputState
{
	private static readonly bool[] keyDown = new bool[120];

	private static readonly bool[] keyPressedThisFrame = new bool[120];

	private static readonly bool[] keyReleasedThisFrame = new bool[120];

	private const int ButtonCount = 5;

	private static readonly bool[] buttonDown = new bool[5];

	private static readonly bool[] buttonPressedThisFrame = new bool[5];

	private static readonly bool[] buttonReleasedThisFrame = new bool[5];

	private static readonly List<InputEvent> eventsThisFrame = new List<InputEvent>();

	private static readonly Stack<InputEvent> eventPool = new Stack<InputEvent>();

	private static readonly Stopwatch clock = Stopwatch.StartNew();

	private const double DoubleClickTime = 0.3;

	private const float DoubleClickMaxDist = 6f;

	private static readonly double[] lastClickTime = new double[5];

	private static readonly Vector2[] lastClickPos = new Vector2[5];

	private static readonly int[] consecutiveClicks = new int[5];

	private static float scrollNotchAccumulator;

	private static double lastScrollTime;

	private const double ScrollNotchResetTime = 0.5;

	public static Vector2 MousePosition { get; private set; }

	public static Vector2 MouseDelta { get; private set; }

	public static Vector2 ScrollDelta { get; private set; }

	public static IReadOnlyList<InputEvent> EventsThisFrame => eventsThisFrame;

	public static bool ShiftDown
	{
		get
		{
			if (!keyDown[53])
			{
				return keyDown[54];
			}
			return true;
		}
	}

	public static bool CtrlDown
	{
		get
		{
			if (!keyDown[55])
			{
				return keyDown[56];
			}
			return true;
		}
	}

	public static bool AltDown
	{
		get
		{
			if (!keyDown[57])
			{
				return keyDown[58];
			}
			return true;
		}
	}

	public static bool MetaDown
	{
		get
		{
			if (!keyDown[59])
			{
				return keyDown[60];
			}
			return true;
		}
	}

	public static bool AnyKeyDown
	{
		get
		{
			for (int i = 1; i < keyDown.Length; i++)
			{
				if (keyDown[i])
				{
					return true;
				}
			}
			return false;
		}
	}

	public static event Action<char>? TextInput;

	public static bool IsKeyDown(Key key)
	{
		if ((uint)key < 120u)
		{
			return keyDown[(int)key];
		}
		return false;
	}

	public static bool WasKeyPressedThisFrame(Key key)
	{
		if ((uint)key < 120u)
		{
			return keyPressedThisFrame[(int)key];
		}
		return false;
	}

	public static bool WasKeyReleasedThisFrame(Key key)
	{
		if ((uint)key < 120u)
		{
			return keyReleasedThisFrame[(int)key];
		}
		return false;
	}

	public static bool IsMouseButtonDown(MouseButton button)
	{
		if ((uint)button < 5u)
		{
			return buttonDown[(int)button];
		}
		return false;
	}

	public static bool WasMouseButtonPressedThisFrame(MouseButton button)
	{
		if ((uint)button < 5u)
		{
			return buttonPressedThisFrame[(int)button];
		}
		return false;
	}

	public static bool WasMouseButtonReleasedThisFrame(MouseButton button)
	{
		if ((uint)button < 5u)
		{
			return buttonReleasedThisFrame[(int)button];
		}
		return false;
	}

	public static void BeginFrame()
	{
		for (int i = 0; i < eventsThisFrame.Count; i++)
		{
			eventPool.Push(eventsThisFrame[i]);
		}
		eventsThisFrame.Clear();
		Array.Clear(keyPressedThisFrame);
		Array.Clear(keyReleasedThisFrame);
		Array.Clear(buttonPressedThisFrame);
		Array.Clear(buttonReleasedThisFrame);
		MouseDelta = Vector2.Zero;
		ScrollDelta = Vector2.Zero;
	}

	public static void ResetAll()
	{
		Array.Clear(keyDown);
		Array.Clear(buttonDown);
		scrollNotchAccumulator = 0f;
	}

	private static InputEvent Acquire(InputEventType type)
	{
		if (eventPool.Count > 0)
		{
			InputEvent inputEvent = eventPool.Pop();
			inputEvent.Reset(type);
			return inputEvent;
		}
		return new InputEvent(type);
	}

	private static void StampModifiers(InputEvent evt)
	{
		evt.shift = ShiftDown;
		evt.control = CtrlDown;
		evt.alt = AltDown;
		evt.command = MetaDown;
	}

	internal static void OnKey(Key key, bool pressed)
	{
		if ((uint)key < 120u && key != 0 && keyDown[(int)key] != pressed)
		{
			keyDown[(int)key] = pressed;
			if (pressed)
			{
				keyPressedThisFrame[(int)key] = true;
			}
			else
			{
				keyReleasedThisFrame[(int)key] = true;
			}
			InputEvent inputEvent = Acquire(pressed ? InputEventType.KeyDown : InputEventType.KeyUp);
			inputEvent.key = key;
			inputEvent.MousePosition = MousePosition;
			StampModifiers(inputEvent);
			eventsThisFrame.Add(inputEvent);
		}
	}

	internal static void OnMouseMove(Vector2 position)
	{
		MouseDelta += position - MousePosition;
		MousePosition = position;
	}

	internal static void OnMouseButton(MouseButton button, bool pressed, Vector2 position)
	{
		if ((uint)button >= 5u || buttonDown[(int)button] == pressed)
		{
			return;
		}
		MousePosition = position;
		buttonDown[(int)button] = pressed;
		if (pressed)
		{
			buttonPressedThisFrame[(int)button] = true;
		}
		else
		{
			buttonReleasedThisFrame[(int)button] = true;
		}
		InputEvent inputEvent = Acquire(pressed ? InputEventType.MouseDown : InputEventType.MouseUp);
		inputEvent.mouseButton = button;
		inputEvent.MousePosition = position;
		StampModifiers(inputEvent);
		if (pressed)
		{
			double totalSeconds = clock.Elapsed.TotalSeconds;
			if (totalSeconds - lastClickTime[(int)button] < 0.3 && Vector2.Distance(position, lastClickPos[(int)button]) < 6f)
			{
				consecutiveClicks[(int)button]++;
			}
			else
			{
				consecutiveClicks[(int)button] = 1;
			}
			lastClickTime[(int)button] = totalSeconds;
			lastClickPos[(int)button] = position;
			inputEvent.clickCount = consecutiveClicks[(int)button];
		}
		eventsThisFrame.Add(inputEvent);
	}

	internal static void OnScroll(Vector2 delta, Vector2 position)
	{
		MousePosition = position;
		ScrollDelta += delta;
		InputEvent inputEvent = Acquire(InputEventType.ScrollWheel);
		inputEvent.scrollDelta = delta;
		inputEvent.scrollNotches = AccumulateScrollNotches(delta.Y);
		inputEvent.MousePosition = position;
		StampModifiers(inputEvent);
		eventsThisFrame.Add(inputEvent);
	}

	private static int AccumulateScrollNotches(float deltaY)
	{
		double totalSeconds = clock.Elapsed.TotalSeconds;
		if (totalSeconds - lastScrollTime > 0.5 || deltaY * scrollNotchAccumulator < 0f)
		{
			scrollNotchAccumulator = 0f;
		}
		lastScrollTime = totalSeconds;
		scrollNotchAccumulator += deltaY;
		int num = (int)scrollNotchAccumulator;
		scrollNotchAccumulator -= num;
		return num;
	}

	internal static void OnTextInput(char character)
	{
		InputState.TextInput?.Invoke(character);
	}
}
