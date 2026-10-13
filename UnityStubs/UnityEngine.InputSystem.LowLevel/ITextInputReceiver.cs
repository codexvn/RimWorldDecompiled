namespace UnityEngine.InputSystem.LowLevel;

public interface ITextInputReceiver
{
	void OnIMECompositionChanged(IMECompositionString compositionString);

	void OnTextInput(char character);
}
