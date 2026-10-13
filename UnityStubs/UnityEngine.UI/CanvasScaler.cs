using UnityEngine.EventSystems;

namespace UnityEngine.UI;

public class CanvasScaler : UIBehaviour
{
	public enum ScaleMode
	{
		ConstantPixelSize,
		ScaleWithScreenSize,
		ConstantPhysicalSize
	}

	public enum ScreenMatchMode
	{
		MatchWidthOrHeight,
		Expand,
		Shrink
	}

	public enum Unit
	{
		Centimeters,
		Millimeters,
		Inches,
		Points,
		Picas
	}

	public ScaleMode uiScaleMode { get; set; }

	public float referencePixelsPerUnit { get; set; } = 100f;

	public float scaleFactor { get; set; } = 1f;

	public Vector2 referenceResolution { get; set; } = new Vector2(800f, 600f);

	public ScreenMatchMode screenMatchMode { get; set; }

	public float matchWidthOrHeight { get; set; }

	public Unit physicalUnit { get; set; } = Unit.Points;

	public float fallbackScreenDPI { get; set; } = 96f;

	public float defaultSpriteDPI { get; set; } = 96f;

	public float dynamicPixelsPerUnit { get; set; } = 1f;
}
