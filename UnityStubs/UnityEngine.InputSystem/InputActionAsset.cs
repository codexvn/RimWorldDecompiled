using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem.Utilities;
using UnityStubs.Internal;

namespace UnityEngine.InputSystem;

public class InputActionAsset : ScriptableObject, IInputActionCollection2, IInputActionCollection, IEnumerable<InputAction>, IEnumerable
{
	public const string Extension = "inputactions";

	public const string kDefaultAssetLayoutJson = "{}";

	public InputBinding? bindingMask
	{
		get
		{
			StubWarner.LogStub("bindingMask", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\InputActionAsset.cs", 10);
			return null;
		}
		set
		{
			StubWarner.LogStub("bindingMask", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\InputActionAsset.cs", 10);
		}
	}

	public IEnumerable<InputBinding> bindings
	{
		get
		{
			StubWarner.LogStub("bindings", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\InputActionAsset.cs", 11);
			return null;
		}
	}

	public ReadOnlyArray<InputControlScheme> controlSchemes
	{
		get
		{
			StubWarner.LogStub("controlSchemes", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\InputActionAsset.cs", 12);
			return default(ReadOnlyArray<InputControlScheme>);
		}
	}

	public ReadOnlyArray<InputDevice>? devices
	{
		get
		{
			StubWarner.LogStub("devices", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\InputActionAsset.cs", 13);
			return null;
		}
		set
		{
			StubWarner.LogStub("devices", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\InputActionAsset.cs", 13);
		}
	}

	public bool Contains(InputAction action)
	{
		StubWarner.LogStub("Contains", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\InputActionAsset.cs", 5);
		return false;
	}

	public void Disable()
	{
		StubWarner.LogStub("Disable", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\InputActionAsset.cs", 6);
	}

	public void Enable()
	{
		StubWarner.LogStub("Enable", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\InputActionAsset.cs", 7);
	}

	public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
	{
		StubWarner.LogStub("FindAction", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\InputActionAsset.cs", 8);
		return null;
	}

	public int FindBinding(InputBinding mask, out InputAction action)
	{
		action = null;
		return 0;
	}

	public IEnumerator<InputAction> GetEnumerator()
	{
		StubWarner.LogStub("GetEnumerator", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\InputActionAsset.cs", 14);
		return null;
	}

	public InputActionMap FindActionMap(string nameOrId, bool throwIfNotFound = false)
	{
		StubWarner.LogStub("FindActionMap", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\InputActionAsset.cs", 17);
		return null;
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		StubWarner.LogStub("GetEnumerator", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\InputActionAsset.cs", 18);
		return null;
	}
}
