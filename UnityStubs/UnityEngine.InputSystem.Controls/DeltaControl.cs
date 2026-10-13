using System;
using UnityStubs.Internal;

namespace UnityEngine.InputSystem.Controls;

public class DeltaControl : Vector2Control
{
	public new int valueSizeInBytes => 8;

	public new Type valueType => typeof(Vector2);

	public DeltaControl(string path, string displayName)
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
		StubWarner.LogStub("ReadValueFromStateIntoBuffer", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\DeltaControl.cs", 17);
	}

	public new unsafe Vector2 ReadUnprocessedValueFromState(void* p0)
	{
		return ReadValue();
	}
}
