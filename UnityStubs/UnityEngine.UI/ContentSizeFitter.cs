using UnityEngine.EventSystems;

namespace UnityEngine.UI;

public class ContentSizeFitter : UIBehaviour, ILayoutSelfController, ILayoutController
{
	public enum FitMode
	{
		Unconstrained,
		MinSize,
		PreferredSize
	}

	public FitMode horizontalFit { get; set; }

	public FitMode verticalFit { get; set; }

	public void SetLayoutHorizontal()
	{
	}

	public void SetLayoutVertical()
	{
	}
}
