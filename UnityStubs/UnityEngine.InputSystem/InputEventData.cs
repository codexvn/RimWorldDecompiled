using System;
using System.Collections.Generic;

namespace UnityEngine.InputSystem;

internal sealed class InputEventData
{
	private readonly List<InputControl> _changedControls = new List<InputControl>();

	private readonly Dictionary<InputControl, object> _values = new Dictionary<InputControl, object>();

	public InputDevice Device { get; }

	public InputEventKind Kind { get; }

	public InputEventData(InputDevice device, InputEventKind kind)
	{
		Device = device;
		Kind = kind;
	}

	public void SetValue(InputControl control, object value, bool changed)
	{
		_values[control] = value;
		if (changed && !_changedControls.Contains(control))
		{
			_changedControls.Add(control);
		}
	}

	public bool HasValueChange(InputControl control)
	{
		return _changedControls.Contains(control);
	}

	public InputControl[] GetChangedControls()
	{
		if (_changedControls.Count != 0)
		{
			return _changedControls.ToArray();
		}
		return Array.Empty<InputControl>();
	}

	public bool TryGetValue<TValue>(InputControl control, out TValue value)
	{
		if (_values.TryGetValue(control, out var value2) && value2 is TValue val)
		{
			value = val;
			return true;
		}
		value = default(TValue);
		return false;
	}
}
