namespace UnityEngine.UI;

public class GridLayoutGroup : LayoutGroup
{
	public enum Corner
	{
		UpperLeft,
		UpperRight,
		LowerLeft,
		LowerRight
	}

	public enum Axis
	{
		Horizontal,
		Vertical
	}

	public enum Constraint
	{
		Flexible,
		FixedColumnCount,
		FixedRowCount
	}

	public Corner startCorner { get; set; }

	public Axis startAxis { get; set; }

	public Vector2 cellSize { get; set; } = new Vector2(100f, 100f);

	public Vector2 spacing { get; set; }

	public Constraint constraint { get; set; }

	public int constraintCount { get; set; } = 2;

	public override void CalculateLayoutInputVertical()
	{
	}

	public override void SetLayoutHorizontal()
	{
	}

	public override void SetLayoutVertical()
	{
	}
}
