using System;
using UnityStubs.Internal;

namespace UnityEngine.InputSystem;

public class InputDevice : InputControl
{
	public const int InvalidDeviceId = 0;

	public const int kControlIndexBits = 10;

	public const int kInvalidDeviceIndex = -1;

	public const int kLocalParticipantId = 0;

	public const int kStateOffsetBits = 13;

	public const int kStateSizeBits = 9;

	public int valueSizeInBytes => 0;

	public Type valueType => typeof(object);

	protected InputDevice(string path, string displayName)
		: base(path, displayName)
	{
	}

	public unsafe bool CompareValue(void* firstStatePtr, void* secondStatePtr)
	{
		return false;
	}

	public unsafe object ReadValueFromBufferAsObject(void* buffer, int bufferSize)
	{
		return null;
	}

	public unsafe object ReadValueFromStateAsObject(void* statePtr)
	{
		return null;
	}

	public unsafe void ReadValueFromStateIntoBuffer(void* statePtr, void* bufferPtr, int bufferSize)
	{
		StubWarner.LogStub("ReadValueFromStateIntoBuffer", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\InputDevice.cs", 17);
	}
}
