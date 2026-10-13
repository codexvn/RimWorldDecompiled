namespace UnityEngine.InputSystem.LowLevel;

public interface IInputStateCallbackReceiver
{
	bool GetStateOffsetForEvent(InputControl control, InputEventPtr eventPtr, ref uint offset);

	void OnNextUpdate();

	void OnStateEvent(InputEventPtr eventPtr);
}
