using System;
using UnityStubs.Internal;

namespace UnityEngine.InputSystem.Controls;

public class KeyControl : ButtonControl
{
	public new int valueSizeInBytes => 4;

	public new Type valueType => typeof(float);

	public Key keyCode { get; }

	public KeyControl(Key keyCode, string path, string displayName)
		: base(path, displayName)
	{
		this.keyCode = keyCode;
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
		StubWarner.LogStub("ReadValueFromStateIntoBuffer", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\KeyControl.cs", 20);
	}

	public new unsafe float ReadUnprocessedValueFromState(void* p0)
	{
		return ReadValue();
	}
}
