using System;
using UnityStubs.Internal;

namespace UnityEngine.InputSystem.Controls;

public class Vector2Control : InputControl<Vector2>
{
	private Vector2 _value;

	public new int valueSizeInBytes => 8;

	public new Type valueType => typeof(Vector2);

	public Vector2Control(string path, string displayName)
		: base(path, displayName)
	{
	}

	internal void SetValue(Vector2 value)
	{
		_value = value;
	}

	internal override Vector2 ReadValueCore()
	{
		return _value;
	}

	public new unsafe bool CompareValue(void* firstStatePtr, void* secondStatePtr)
	{
		return false;
	}

	public new unsafe object ReadValueFromBufferAsObject(void* buffer, int bufferSize)
	{
		return ReadValue();
	}

	public new unsafe object ReadValueFromStateAsObject(void* statePtr)
	{
		return ReadValue();
	}

	public new unsafe void ReadValueFromStateIntoBuffer(void* statePtr, void* bufferPtr, int bufferSize)
	{
		StubWarner.LogStub("ReadValueFromStateIntoBuffer", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\Vector2Control.cs", 29);
	}

	public unsafe Vector2 ReadUnprocessedValueFromState(void* p0)
	{
		return ReadValue();
	}
}
