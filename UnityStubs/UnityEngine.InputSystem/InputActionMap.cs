using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem.Utilities;
using UnityStubs.Internal;

namespace UnityEngine.InputSystem;

public class InputActionMap : ICloneable, ISerializationCallbackReceiver, IInputActionCollection2, IInputActionCollection, IEnumerable<InputAction>, IEnumerable, IDisposable
{
	private readonly List<InputAction> _actions = new List<InputAction>();

	public InputBinding? bindingMask { get; set; }

	public ReadOnlyArray<InputControlScheme> controlSchemes
	{
		get
		{
			StubWarner.LogStub("controlSchemes", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\InputActionMap.cs", 51);
			return default(ReadOnlyArray<InputControlScheme>);
		}
	}

	public ReadOnlyArray<InputDevice>? devices { get; set; }

	public IEnumerable<InputBinding> bindings => Array.Empty<InputBinding>();

	public bool enabled { get; private set; }

	public string name { get; }

	public bool Contains(InputAction action)
	{
		return _actions.Contains(action);
	}

	public void Disable()
	{
		enabled = false;
		for (int i = 0; i < _actions.Count; i++)
		{
			_actions[i].SetEnabled(enabled: false);
		}
	}

	public void Dispose()
	{
		for (int i = 0; i < _actions.Count; i++)
		{
			_actions[i].Dispose();
		}
		_actions.Clear();
	}

	public void Enable()
	{
		enabled = true;
		for (int i = 0; i < _actions.Count; i++)
		{
			_actions[i].SetEnabled(enabled: true);
		}
	}

	public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
	{
		for (int i = 0; i < _actions.Count; i++)
		{
			if (_actions[i].name == actionNameOrId)
			{
				return _actions[i];
			}
		}
		if (throwIfNotFound)
		{
			throw new InvalidOperationException($"Input action '{actionNameOrId}' was not found in map '{name}'.");
		}
		return null;
	}

	public int FindBinding(InputBinding mask, out InputAction action)
	{
		action = null;
		return -1;
	}

	public IEnumerator<InputAction> GetEnumerator()
	{
		return _actions.GetEnumerator();
	}

	public void OnAfterDeserialize()
	{
		StubWarner.LogStub("OnAfterDeserialize", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\InputActionMap.cs", 57);
	}

	public void OnBeforeSerialize()
	{
		StubWarner.LogStub("OnBeforeSerialize", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\InputActionMap.cs", 59);
	}

	public InputActionMap(string name)
	{
		this.name = name;
	}

	internal InputAction AddActionInternal(string actionName, InputActionType type)
	{
		InputAction inputAction = new InputAction(actionName, type);
		inputAction.SetEnabled(enabled);
		_actions.Add(inputAction);
		return inputAction;
	}

	public object Clone()
	{
		return this;
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}
}
