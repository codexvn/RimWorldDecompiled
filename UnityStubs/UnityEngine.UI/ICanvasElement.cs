namespace UnityEngine.UI;

public interface ICanvasElement
{
	Transform transform { get; }

	void GraphicUpdateComplete();

	bool IsDestroyed();

	void LayoutComplete();

	void Rebuild(CanvasUpdate executing);
}
