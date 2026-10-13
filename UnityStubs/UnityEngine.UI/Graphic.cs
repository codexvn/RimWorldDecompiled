using UnityEngine.EventSystems;
using UnityStubs.Internal;

namespace UnityEngine.UI;

public abstract class Graphic : UIBehaviour, ICanvasElement
{
	public RectTransform rectTransform
	{
		get
		{
			StubWarner.LogStub("rectTransform", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\Graphic.cs", 5);
			return null;
		}
	}

	public virtual Color color { get; set; } = Color.white;

	public virtual bool raycastTarget { get; set; } = true;

	public virtual Material material { get; set; }

	public virtual Texture mainTexture => null;

	public virtual void SetAllDirty()
	{
	}

	public virtual void SetVerticesDirty()
	{
	}

	public virtual void SetMaterialDirty()
	{
	}

	public virtual void SetLayoutDirty()
	{
	}

	public void GraphicUpdateComplete()
	{
		StubWarner.LogStub("GraphicUpdateComplete", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\Graphic.cs", 14);
	}

	public void LayoutComplete()
	{
		StubWarner.LogStub("LayoutComplete", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\Graphic.cs", 15);
	}

	public void Rebuild(CanvasUpdate update)
	{
		StubWarner.LogStub("Rebuild", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\Graphic.cs", 16);
	}
}
