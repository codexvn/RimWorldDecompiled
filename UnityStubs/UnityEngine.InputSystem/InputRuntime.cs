using System;
using System.Collections.Generic;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;

namespace UnityEngine.InputSystem;

internal static class InputRuntime
{
	private static readonly Dictionary<string, InputControl> _controlsByPath = new Dictionary<string, InputControl>(StringComparer.OrdinalIgnoreCase);

	private static readonly List<InputAction> _actions = new List<InputAction>();

	private static Keyboard _keyboard;

	private static Mouse _mouse;

	public static Keyboard Keyboard => _keyboard ?? (_keyboard = new Keyboard());

	public static Mouse Mouse => _mouse ?? (_mouse = new Mouse());

	public static void EnsureInitialized()
	{
		_ = Keyboard;
		_ = Mouse;
	}

	public static void RegisterControl(InputControl control)
	{
		if (!string.IsNullOrEmpty(control.path))
		{
			_controlsByPath[control.path] = control;
		}
	}

	public static InputControl FindControl(string path)
	{
		if (string.IsNullOrEmpty(path))
		{
			return null;
		}
		EnsureInitialized();
		_controlsByPath.TryGetValue(path, out var value);
		return value;
	}

	public static void RegisterAction(InputAction action)
	{
		if (!_actions.Contains(action))
		{
			_actions.Add(action);
		}
	}

	public static void UnregisterAction(InputAction action)
	{
		_actions.Remove(action);
	}

	public static void BeginFrame()
	{
		EnsureInitialized();
		Keyboard.BeginFrame();
		Mouse.BeginFrame();
		for (int i = 0; i < _actions.Count; i++)
		{
			_actions[i].BeginFrame();
		}
	}

	public static void ResetAllInput()
	{
		EnsureInitialized();
		Keyboard.ResetState();
		Mouse.ResetState();
		for (int i = 0; i < _actions.Count; i++)
		{
			_actions[i].ForceResetState();
		}
	}

	public static void QueueKeyChange(Key key, bool isPressed)
	{
		EnsureInitialized();
		KeyControl keyControl = Keyboard[key];
		if (keyControl != null && keyControl.isPressed != isPressed)
		{
			InputEventData inputEventData = new InputEventData(Keyboard, InputEventKind.State);
			inputEventData.SetValue(keyControl, isPressed ? 1f : 0f, changed: true);
			NotifyEvent(inputEventData);
			keyControl.SetPressed(isPressed);
			Keyboard.UpdateAnyKeyState();
			ProcessActions();
		}
	}

	public static void QueueKeyRepeat(Key key)
	{
		EnsureInitialized();
		KeyControl keyControl = Keyboard[key];
		if (keyControl != null && keyControl.isPressed)
		{
			keyControl.MarkRepeated();
		}
	}

	public static void QueueTextInput(char character)
	{
		EnsureInitialized();
		Keyboard.OnTextInput(character);
	}

	public static void QueueMouseMove(Vector2 position)
	{
		EnsureInitialized();
		Vector2 vector = Mouse.position.ReadValue();
		Vector2 vector2 = position - vector;
		InputEventData inputEventData = new InputEventData(Mouse, InputEventKind.Delta);
		inputEventData.SetValue(Mouse.position, position, changed: true);
		inputEventData.SetValue(Mouse.delta, vector2, changed: true);
		NotifyEvent(inputEventData);
		Mouse.SetPosition(position);
		Mouse.AccumulateDelta(vector2);
		ProcessActions();
	}

	public static void QueueMouseButton(int buttonIndex, bool isPressed, Vector2 position)
	{
		EnsureInitialized();
		ButtonControl buttonControl = Mouse.GetButtonControl(buttonIndex);
		if (buttonControl != null && buttonControl.isPressed != isPressed)
		{
			InputEventData inputEventData = new InputEventData(Mouse, InputEventKind.State);
			inputEventData.SetValue(buttonControl, isPressed ? 1f : 0f, changed: true);
			inputEventData.SetValue(Mouse.position, position, changed: false);
			NotifyEvent(inputEventData);
			Mouse.SetPosition(position);
			buttonControl.SetPressed(isPressed);
			ProcessActions();
		}
	}

	public static void QueueMouseScroll(Vector2 scrollDelta, Vector2 position)
	{
		EnsureInitialized();
		InputEventData inputEventData = new InputEventData(Mouse, InputEventKind.Delta);
		inputEventData.SetValue(Mouse.scroll, scrollDelta, changed: true);
		inputEventData.SetValue(Mouse.position, position, changed: false);
		NotifyEvent(inputEventData);
		Mouse.SetPosition(position);
		Mouse.AccumulateScroll(scrollDelta);
		ProcessActions();
	}

	private static void NotifyEvent(InputEventData data)
	{
		InputSystem.NotifyOnEvent(new InputEventPtr(data), data.Device);
	}

	private static void ProcessActions()
	{
		for (int i = 0; i < _actions.Count; i++)
		{
			_actions[i].ProcessStateChange();
		}
	}
}
