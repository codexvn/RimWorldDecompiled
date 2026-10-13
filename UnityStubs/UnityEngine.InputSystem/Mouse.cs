using System;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;
using UnityStubs.Internal;

namespace UnityEngine.InputSystem;

public class Mouse : Pointer, IInputStateCallbackReceiver
{
	private readonly ButtonControl _leftButton;

	private readonly ButtonControl _rightButton;

	private readonly ButtonControl _middleButton;

	private readonly ButtonControl _forwardButton;

	private readonly ButtonControl _backButton;

	private readonly DeltaControl _scroll;

	public new int valueSizeInBytes => 0;

	public new Type valueType => typeof(object);

	public ButtonControl backButton => _backButton;

	public static Mouse current
	{
		get
		{
			InputRuntime.EnsureInitialized();
			return InputRuntime.Mouse;
		}
	}

	public ButtonControl forwardButton => _forwardButton;

	public ButtonControl leftButton => _leftButton;

	public ButtonControl middleButton => _middleButton;

	public ButtonControl rightButton => _rightButton;

	public DeltaControl scroll => _scroll;

	public Mouse()
		: base("<Mouse>", "Mouse")
	{
		_leftButton = new ButtonControl("<Mouse>/leftButton", "Left Button");
		_rightButton = new ButtonControl("<Mouse>/rightButton", "Right Button");
		_middleButton = new ButtonControl("<Mouse>/middleButton", "Middle Button");
		_forwardButton = new ButtonControl("<Mouse>/forwardButton", "Forward Button");
		_backButton = new ButtonControl("<Mouse>/backButton", "Back Button");
		_scroll = new DeltaControl("<Mouse>/scroll", "Scroll");
		InputRuntime.RegisterControl(_leftButton);
		InputRuntime.RegisterControl(_rightButton);
		InputRuntime.RegisterControl(_middleButton);
		InputRuntime.RegisterControl(_forwardButton);
		InputRuntime.RegisterControl(_backButton);
		InputRuntime.RegisterControl(_scroll);
	}

	internal new void BeginFrame()
	{
		base.BeginFrame();
		_leftButton.BeginFrame();
		_rightButton.BeginFrame();
		_middleButton.BeginFrame();
		_forwardButton.BeginFrame();
		_backButton.BeginFrame();
		_scroll.SetValue(Vector2.zero);
	}

	internal new void ResetState()
	{
		base.ResetState();
		_leftButton.ResetState();
		_rightButton.ResetState();
		_middleButton.ResetState();
		_forwardButton.ResetState();
		_backButton.ResetState();
		_scroll.SetValue(Vector2.zero);
	}

	internal void AccumulateScroll(Vector2 delta)
	{
		_scroll.SetValue(_scroll.ReadValue() + delta);
	}

	internal ButtonControl GetButtonControl(int buttonIndex)
	{
		return buttonIndex switch
		{
			0 => _leftButton, 
			1 => _rightButton, 
			2 => _middleButton, 
			3 => _forwardButton, 
			4 => _backButton, 
			_ => null, 
		};
	}

	public new unsafe bool CompareValue(void* firstStatePtr, void* secondStatePtr)
	{
		return false;
	}

	public new void OnNextUpdate()
	{
		StubWarner.LogStub("OnNextUpdate", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\Mouse.cs", 78);
	}

	public new void OnStateEvent(InputEventPtr eventPtr)
	{
		StubWarner.LogStub("OnStateEvent", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\Mouse.cs", 80);
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
		StubWarner.LogStub("ReadValueFromStateIntoBuffer", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\Mouse.cs", 87);
	}

	public new bool GetStateOffsetForEvent(InputControl control, InputEventPtr eventPtr, ref uint offset)
	{
		offset = 0u;
		return false;
	}
}
