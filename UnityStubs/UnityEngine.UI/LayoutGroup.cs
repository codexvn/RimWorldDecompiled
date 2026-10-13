using UnityEngine.EventSystems;

namespace UnityEngine.UI;

public abstract class LayoutGroup : UIBehaviour, ILayoutElement, ILayoutGroup, ILayoutController
{
	public RectOffset padding { get; set; } = new RectOffset();

	public TextAnchor childAlignment { get; set; }

	public float minWidth { get; protected set; }

	public float preferredWidth { get; protected set; }

	public float flexibleWidth { get; protected set; }

	public float minHeight { get; protected set; }

	public float preferredHeight { get; protected set; }

	public float flexibleHeight { get; protected set; }

	public int layoutPriority => 0;

	public virtual void CalculateLayoutInputHorizontal()
	{
	}

	public abstract void CalculateLayoutInputVertical();

	public abstract void SetLayoutHorizontal();

	public abstract void SetLayoutVertical();
}
