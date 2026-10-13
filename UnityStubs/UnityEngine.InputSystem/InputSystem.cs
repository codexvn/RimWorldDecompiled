using System;
using System.Collections.Generic;
using UnityEngine.InputSystem.LowLevel;

namespace UnityEngine.InputSystem;

public static class InputSystem
{
	private static readonly List<Action<InputEventPtr, InputDevice>> _callbacks = new List<Action<InputEventPtr, InputDevice>>();

	public const string kAssemblyVersion = "1.19.0";

	public const string kDocUrl = "https://docs.unity3d.com/Packages/com.unity.inputsystem@1.19";

	public static InputEventListener onEvent
	{
		get
		{
			return default(InputEventListener);
		}
		set
		{
		}
	}

	internal static void AddOnEventCallback(Action<InputEventPtr, InputDevice> callback)
	{
		if (!_callbacks.Contains(callback))
		{
			_callbacks.Add(callback);
		}
	}

	internal static void RemoveOnEventCallback(Action<InputEventPtr, InputDevice> callback)
	{
		_callbacks.Remove(callback);
	}

	internal static void NotifyOnEvent(InputEventPtr eventPtr, InputDevice device)
	{
		if (_callbacks.Count != 0)
		{
			Action<InputEventPtr, InputDevice>[] array = _callbacks.ToArray();
			for (int i = 0; i < array.Length; i++)
			{
				array[i](eventPtr, device);
			}
		}
	}

	public static void BeginFrame()
	{
		InputRuntime.BeginFrame();
	}

	public static void ResetAllInput()
	{
		InputRuntime.ResetAllInput();
	}

	public static void QueueKeyChange(Key key, bool isPressed)
	{
		InputRuntime.QueueKeyChange(key, isPressed);
	}

	public static void QueueKeyRepeat(Key key)
	{
		InputRuntime.QueueKeyRepeat(key);
	}

	public static void QueueTextInput(char character)
	{
		InputRuntime.QueueTextInput(character);
	}

	public static void QueueMouseMove(Vector2 position)
	{
		InputRuntime.QueueMouseMove(position);
	}

	public static void QueueMouseButton(int buttonIndex, bool isPressed, Vector2 position)
	{
		InputRuntime.QueueMouseButton(buttonIndex, isPressed, position);
	}

	public static void QueueMouseScroll(Vector2 scrollDelta, Vector2 position)
	{
		InputRuntime.QueueMouseScroll(scrollDelta, position);
	}
}
