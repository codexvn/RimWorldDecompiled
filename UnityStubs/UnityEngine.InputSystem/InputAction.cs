using System;
using System.Collections.Generic;
using UnityEngine.InputSystem.Controls;

namespace UnityEngine.InputSystem;

public class InputAction : ICloneable, IDisposable
{
	private sealed class ActionBinding
	{
		public string MainPath;

		public string ModifierPath1;

		public string ModifierPath2;
	}

	private readonly List<ActionBinding> _bindings = new List<ActionBinding>();

	private readonly InputActionType _type;

	private bool _enabled;

	private bool _isPressed;

	private bool _wasPressedThisFrame;

	private bool _wasReleasedThisFrame;

	private InputControl _activeControl;

	public InputControl activeControl
	{
		get
		{
			RefreshState();
			return _activeControl;
		}
	}

	public string name { get; }

	internal InputAction(string name, InputActionType type)
	{
		this.name = name;
		_type = type;
		InputRuntime.RegisterAction(this);
	}

	public void Dispose()
	{
		InputRuntime.UnregisterAction(this);
	}

	public object Clone()
	{
		return this;
	}

	public bool IsPressed()
	{
		RefreshState();
		return _isPressed;
	}

	public T0 ReadValue<T0>()
	{
		RefreshState();
		if (_activeControl is InputControl<T0> inputControl)
		{
			return inputControl.ReadValue();
		}
		if (typeof(T0) == typeof(float) && _activeControl is ButtonControl buttonControl)
		{
			return (T0)(object)buttonControl.ReadValue();
		}
		return default(T0);
	}

	public bool WasPressedThisFrame()
	{
		return _wasPressedThisFrame;
	}

	public bool WasReleasedThisFrame()
	{
		return _wasReleasedThisFrame;
	}

	internal void SetEnabled(bool enabled)
	{
		_enabled = enabled;
		RefreshState();
		_wasPressedThisFrame = false;
		_wasReleasedThisFrame = false;
	}

	internal void AddBinding(string path)
	{
		_bindings.Add(new ActionBinding
		{
			MainPath = path
		});
	}

	internal int AddCompositeBinding(string composite)
	{
		_bindings.Add(new ActionBinding());
		return _bindings.Count - 1;
	}

	internal void SetCompositeBindingPart(int bindingIndex, string name, string bindingPath)
	{
		if (bindingIndex >= 0 && bindingIndex < _bindings.Count)
		{
			ActionBinding actionBinding = _bindings[bindingIndex];
			switch (name)
			{
			case "Binding":
				actionBinding.MainPath = bindingPath;
				break;
			case "Modifier":
			case "Modifier1":
				actionBinding.ModifierPath1 = bindingPath;
				break;
			case "Modifier2":
				actionBinding.ModifierPath2 = bindingPath;
				break;
			}
		}
	}

	internal void BeginFrame()
	{
		_wasPressedThisFrame = false;
		_wasReleasedThisFrame = false;
		RefreshState();
	}

	internal void ForceResetState()
	{
		_isPressed = false;
		_wasPressedThisFrame = false;
		_wasReleasedThisFrame = false;
		_activeControl = null;
	}

	internal void ProcessStateChange()
	{
		bool isPressed = _isPressed;
		RefreshState();
		if (_isPressed != isPressed)
		{
			if (_isPressed)
			{
				_wasPressedThisFrame = true;
			}
			else
			{
				_wasReleasedThisFrame = true;
			}
		}
	}

	private void RefreshState()
	{
		if (!_enabled)
		{
			_isPressed = false;
			_activeControl = null;
			return;
		}
		for (int i = 0; i < _bindings.Count; i++)
		{
			if (TryResolveActiveControl(_bindings[i], out var inputControl))
			{
				_isPressed = true;
				_activeControl = inputControl;
				return;
			}
		}
		_isPressed = false;
		_activeControl = null;
	}

	private static bool TryResolveActiveControl(ActionBinding binding, out InputControl activeControl)
	{
		activeControl = null;
		InputControl inputControl = InputRuntime.FindControl(binding.MainPath);
		if (!IsControlPressed(inputControl))
		{
			return false;
		}
		if (!string.IsNullOrEmpty(binding.ModifierPath1) && !IsControlPressed(InputRuntime.FindControl(binding.ModifierPath1)))
		{
			return false;
		}
		if (!string.IsNullOrEmpty(binding.ModifierPath2) && !IsControlPressed(InputRuntime.FindControl(binding.ModifierPath2)))
		{
			return false;
		}
		activeControl = inputControl;
		return true;
	}

	private static bool IsControlPressed(InputControl control)
	{
		if (control is ButtonControl buttonControl)
		{
			return buttonControl.isPressed;
		}
		return false;
	}
}
