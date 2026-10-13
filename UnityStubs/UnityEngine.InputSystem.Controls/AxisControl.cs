using System;
using UnityStubs.Internal;

namespace UnityEngine.InputSystem.Controls;

public class AxisControl : InputControl<float>
{
	private float _value;

	public new int valueSizeInBytes => 4;

	public new Type valueType => typeof(float);

	public AxisControl(string path, string displayName)
		: base(path, displayName)
	{
	}

	internal void SetValue(float value)
	{
		_value = value;
	}

	internal override float ReadValueCore()
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
		StubWarner.LogStub("ReadValueFromStateIntoBuffer", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\AxisControl.cs", 29);
	}

	public unsafe float ReadUnprocessedValueFromState(void* p0)
	{
		return ReadValue();
	}
}
