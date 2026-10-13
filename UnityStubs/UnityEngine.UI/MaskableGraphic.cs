using UnityStubs.Internal;

namespace UnityEngine.UI;

public abstract class MaskableGraphic : Graphic, IClippable, IMaskable, IMaterialModifier
{
	public void Cull(Rect clipRect, bool validRect)
	{
		StubWarner.LogStub("Cull", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\MaskableGraphic.cs", 5);
	}

	public Material GetModifiedMaterial(Material baseMaterial)
	{
		StubWarner.LogStub("GetModifiedMaterial", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\MaskableGraphic.cs", 6);
		return null;
	}

	public void RecalculateClipping()
	{
		StubWarner.LogStub("RecalculateClipping", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\MaskableGraphic.cs", 7);
	}

	public void RecalculateMasking()
	{
		StubWarner.LogStub("RecalculateMasking", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\MaskableGraphic.cs", 8);
	}

	public void SetClipRect(Rect clipRect, bool validRect)
	{
		StubWarner.LogStub("SetClipRect", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\MaskableGraphic.cs", 9);
	}

	public void SetClipSoftness(Vector2 clipSoftness)
	{
		StubWarner.LogStub("SetClipSoftness", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\MaskableGraphic.cs", 10);
	}
}
