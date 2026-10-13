namespace UnityEngine;

public class GridLayout : Behaviour
{
	public enum CellLayout
	{
		Rectangle,
		Hexagon,
		Isometric,
		IsometricZAsY
	}

	public enum CellSwizzle
	{
		XYZ,
		XZY,
		YXZ,
		YZX,
		ZXY,
		ZYX
	}

	public Vector3 cellSize { get; set; } = Vector3.one;

	public Vector3 cellGap { get; set; }

	public CellLayout cellLayout { get; set; }

	public CellSwizzle cellSwizzle { get; set; }
}
