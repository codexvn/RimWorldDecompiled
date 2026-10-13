using System;
using UnityStubs.Internal;

namespace UnityEngine.InputSystem.Controls;

public class IntegerControl : InputControl<int>
{
	private int _value;

	public int value => ReadValue();

	public new int valueSizeInBytes => 4;

	public new Type valueType => typeof(int);

	public IntegerControl(string path, string displayName)
		: base(path, displayName)
	{
	}

	internal void SetValue(int value)
	{
		_value = value;
	}

	internal override int ReadValueCore()
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
		StubWarner.LogStub("ReadValueFromStateIntoBuffer", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\IntegerControl.cs", 31);
	}
}
