using System;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;
using UnityStubs.Internal;

namespace UnityEngine.InputSystem;

public class Pointer : InputDevice, IInputStateCallbackReceiver
{
	private readonly Vector2Control _position;

	private readonly DeltaControl _delta;

	private readonly IntegerControl _displayIndex;

	public new int valueSizeInBytes => 0;

	public new Type valueType => typeof(object);

	public DeltaControl delta => _delta;

	public Vector2Control position => _position;

	public IntegerControl displayIndex => _displayIndex;

	protected Pointer(string path, string displayName)
		: base(path, displayName)
	{
		_position = new Vector2Control(path + "/position", "Position");
		_delta = new DeltaControl(path + "/delta", "Delta");
		_displayIndex = new IntegerControl(path + "/displayIndex", "Display Index");
		InputRuntime.RegisterControl(_position);
		InputRuntime.RegisterControl(_delta);
		InputRuntime.RegisterControl(_displayIndex);
	}

	internal void BeginFrame()
	{
		_delta.SetValue(Vector2.zero);
	}

	internal void ResetState()
	{
		_delta.SetValue(Vector2.zero);
	}

	internal void SetPosition(Vector2 position)
	{
		_position.SetValue(position);
	}

	internal void AccumulateDelta(Vector2 delta)
	{
		_delta.SetValue(_delta.ReadValue() + delta);
	}

	internal void SetDisplayIndex(int displayIndex)
	{
		_displayIndex.SetValue(displayIndex);
	}

	public new unsafe bool CompareValue(void* firstStatePtr, void* secondStatePtr)
	{
		return false;
	}

	public void OnNextUpdate()
	{
		StubWarner.LogStub("OnNextUpdate", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\Pointer.cs", 51);
	}

	public void OnStateEvent(InputEventPtr eventPtr)
	{
		StubWarner.LogStub("OnStateEvent", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\Pointer.cs", 53);
	}

	public new unsafe object ReadValueFromBufferAsObject(void* buffer, int bufferSize)
	{
		return null;
	}

	public new unsafe object ReadValueFromStateAsObject(void* statePtr)
	{
		return null;
	}

	public new unsafe void ReadValueFromStateIntoBuffer(void* statePtr, void* bufferPtr, int bufferSize)
	{
		StubWarner.LogStub("ReadValueFromStateIntoBuffer", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\Pointer.cs", 59);
	}

	public bool GetStateOffsetForEvent(InputControl control, InputEventPtr eventPtr, ref uint offset)
	{
		offset = 0u;
		return false;
	}
}
