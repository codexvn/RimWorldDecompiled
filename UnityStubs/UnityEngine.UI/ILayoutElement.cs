namespace UnityEngine.UI;

public interface ILayoutElement
{
	float flexibleHeight { get; }

	float flexibleWidth { get; }

	int layoutPriority { get; }

	float minHeight { get; }

	float minWidth { get; }

	float preferredHeight { get; }

	float preferredWidth { get; }

	void CalculateLayoutInputHorizontal();

	void CalculateLayoutInputVertical();
}
