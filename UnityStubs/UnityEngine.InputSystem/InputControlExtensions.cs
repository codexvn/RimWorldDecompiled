using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem.LowLevel;

namespace UnityEngine.InputSystem;

public static class InputControlExtensions
{
	public struct InputEventControlCollection : IEnumerable<InputControl>, IEnumerable
	{
		private readonly InputControl[] _controls;

		internal InputEventControlCollection(InputControl[] controls)
		{
			_controls = controls ?? Array.Empty<InputControl>();
		}

		IEnumerator<InputControl> IEnumerable<InputControl>.GetEnumerator()
		{
			return GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public InputEventControlEnumerator GetEnumerator()
		{
			return new InputEventControlEnumerator(_controls);
		}
	}

	public struct InputEventControlEnumerator : IEnumerator<InputControl>, IEnumerator, IDisposable
	{
		private readonly InputControl[] _controls;

		private int _index;

		public InputControl Current => _controls[_index];

		object IEnumerator.Current => Current;

		internal InputEventControlEnumerator(InputControl[] controls)
		{
			_controls = controls ?? Array.Empty<InputControl>();
			_index = -1;
		}

		public void Dispose()
		{
		}

		public bool MoveNext()
		{
			_index++;
			return _index < _controls.Length;
		}

		public void Reset()
		{
			_index = -1;
		}
	}

	public static InputEventControlCollection EnumerateChangedControls(this InputEventPtr eventPtr, InputDevice device = null, float magnitudeThreshold = 0f)
	{
		if (eventPtr.Data == null)
		{
			return new InputEventControlCollection(Array.Empty<InputControl>());
		}
		if (device != null && eventPtr.Data.Device != device)
		{
			return new InputEventControlCollection(Array.Empty<InputControl>());
		}
		return new InputEventControlCollection(eventPtr.Data.GetChangedControls());
	}

	public static bool HasValueChangeInEvent(this InputControl control, InputEventPtr eventPtr)
	{
		return eventPtr.Data?.HasValueChange(control) ?? false;
	}

	public static T0 ReadValueFromEvent<T0>(this InputControl<T0> control, InputEventPtr inputEvent)
	{
		if (inputEvent.Data != null && inputEvent.Data.TryGetValue<T0>(control, out var value))
		{
			return value;
		}
		return control.ReadValue();
	}
}
