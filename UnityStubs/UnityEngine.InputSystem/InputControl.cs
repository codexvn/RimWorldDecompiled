using System;
using UnityStubs.Internal;

namespace UnityEngine.InputSystem;

public abstract class InputControl
{
	public string displayName { get; }

	public string path { get; }

	protected InputControl(string path, string displayName)
	{
		this.path = path;
		this.displayName = displayName;
	}
}
public abstract class InputControl<TValue> : InputControl
{
	public int valueSizeInBytes => 0;

	public Type valueType => typeof(TValue);

	protected InputControl(string path, string displayName)
		: base(path, displayName)
	{
	}

	internal abstract TValue ReadValueCore();

	public unsafe bool CompareValue(void* firstStatePtr, void* secondStatePtr)
	{
		return false;
	}

	public TValue ReadValue()
	{
		return ReadValueCore();
	}

	public unsafe object ReadValueFromBufferAsObject(void* buffer, int bufferSize)
	{
		return ReadValue();
	}

	public unsafe object ReadValueFromStateAsObject(void* statePtr)
	{
		return ReadValue();
	}

	public unsafe void ReadValueFromStateIntoBuffer(void* statePtr, void* bufferPtr, int bufferSize)
	{
		StubWarner.LogStub("ReadValueFromStateIntoBuffer", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\UnityEngine.InputSystem.InputControl_1.cs", 21);
	}
}
