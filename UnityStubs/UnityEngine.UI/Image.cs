using UnityStubs.Internal;

namespace UnityEngine.UI;

public class Image : MaskableGraphic
{
	public enum Type
	{
		Simple,
		Sliced,
		Tiled,
		Filled
	}

	public enum FillMethod
	{
		Horizontal,
		Vertical,
		Radial90,
		Radial180,
		Radial360
	}

	public enum OriginHorizontal
	{
		Left,
		Right
	}

	public enum OriginVertical
	{
		Bottom,
		Top
	}

	public enum Origin90
	{
		BottomLeft,
		TopLeft,
		TopRight,
		BottomRight
	}

	public enum Origin180
	{
		Bottom,
		Left,
		Top,
		Right
	}

	public enum Origin360
	{
		Bottom,
		Right,
		Top,
		Left
	}

	public Sprite sprite { get; set; }

	public Sprite overrideSprite { get; set; }

	public Type type { get; set; }

	public bool preserveAspect { get; set; }

	public bool fillCenter { get; set; } = true;

	public FillMethod fillMethod { get; set; }

	public float fillAmount { get; set; } = 1f;

	public bool fillClockwise { get; set; } = true;

	public int fillOrigin { get; set; }

	public bool useSpriteMesh { get; set; }

	public float pixelsPerUnitMultiplier { get; set; } = 1f;

	public override Texture mainTexture => sprite?.texture;

	public void SetNativeSize()
	{
		StubWarner.LogStub("SetNativeSize", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\Image.cs", 25);
	}
}
