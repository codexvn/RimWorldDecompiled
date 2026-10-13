using System;
using UnityStubs.Internal;

namespace UnityEngine.InputSystem.Controls;

public class ButtonControl : AxisControl
{
	private bool _wasPressedThisFrame;

	private bool _wasReleasedThisFrame;

	private bool _wasRepeatedThisFrame;

	public const float kMinButtonPressPoint = 0.0001f;

	internal bool WasPressedThisFrame => _wasPressedThisFrame;

	internal bool WasReleasedThisFrame => _wasReleasedThisFrame;

	internal bool WasRepeatedThisFrame => _wasRepeatedThisFrame;

	public new int valueSizeInBytes => 4;

	public new Type valueType => typeof(float);

	public bool isPressed => ReadValue() >= pressPointOrDefault;

	public float pressPointOrDefault => 0.5f;

	public ButtonControl(string path, string displayName)
		: base(path, displayName)
	{
	}

	internal void BeginFrame()
	{
		_wasPressedThisFrame = false;
		_wasReleasedThisFrame = false;
		_wasRepeatedThisFrame = false;
	}

	internal void ResetState()
	{
		SetValue(0f);
		_wasPressedThisFrame = false;
		_wasReleasedThisFrame = false;
		_wasRepeatedThisFrame = false;
	}

	internal void MarkRepeated()
	{
		_wasRepeatedThisFrame = true;
	}

	internal void SetPressed(bool pressed)
	{
		bool flag = isPressed;
		SetValue(pressed ? 1f : 0f);
		if (pressed != flag)
		{
			if (pressed)
			{
				_wasPressedThisFrame = true;
			}
			else
			{
				_wasReleasedThisFrame = true;
			}
		}
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
		StubWarner.LogStub("ReadValueFromStateIntoBuffer", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\ButtonControl.cs", 59);
	}

	public new unsafe float ReadUnprocessedValueFromState(void* p0)
	{
		return ReadValue();
	}
}
