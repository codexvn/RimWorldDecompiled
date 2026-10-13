using System;
using UnityStubs.Internal;

namespace UnityEngine.InputSystem.Controls;

public class AnyKeyControl : ButtonControl
{
	public new int valueSizeInBytes => 4;

	public new Type valueType => typeof(float);

	public AnyKeyControl(string path, string displayName)
		: base(path, displayName)
	{
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
		StubWarner.LogStub("ReadValueFromStateIntoBuffer", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\AnyKeyControl.cs", 18);
	}

	public new unsafe float ReadUnprocessedValueFromState(void* p0)
	{
		return ReadValue();
	}
}
