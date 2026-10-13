namespace UnityEngine.InputSystem.LowLevel;

public interface IEventPreProcessor
{
	bool PreProcessEvent(InputEventPtr currentEventPtr);
}
