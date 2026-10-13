namespace UnityEngine;

public abstract class GridBrushBase : ScriptableObject
{
	public enum Tool
	{
		Select,
		Move,
		Paint,
		Box,
		Pick,
		Erase,
		FloodFill
	}

	public enum RotationDirection
	{
		Clockwise,
		CounterClockwise
	}

	public enum FlipAxis
	{
		X,
		Y
	}

	public virtual void Paint(GridLayout gridLayout, GameObject brushTarget, Vector3Int position)
	{
	}

	public virtual void Erase(GridLayout gridLayout, GameObject brushTarget, Vector3Int position)
	{
	}

	public virtual void BoxFill(GridLayout gridLayout, GameObject brushTarget, BoundsInt position)
	{
	}

	public virtual void BoxErase(GridLayout gridLayout, GameObject brushTarget, BoundsInt position)
	{
	}

	public virtual void FloodFill(GridLayout gridLayout, GameObject brushTarget, Vector3Int position)
	{
	}

	public virtual void Rotate(RotationDirection direction, GridLayout.CellLayout layout)
	{
	}

	public virtual void Flip(FlipAxis flip, GridLayout.CellLayout layout)
	{
	}

	public virtual void Select(GridLayout gridLayout, GameObject brushTarget, BoundsInt position)
	{
	}

	public virtual void Move(GridLayout gridLayout, GameObject brushTarget, BoundsInt from, BoundsInt to)
	{
	}

	public virtual void Pick(GridLayout gridLayout, GameObject brushTarget, BoundsInt position, Vector3Int pivot)
	{
	}

	public virtual void ChangeZPosition(int change)
	{
	}

	public virtual void ResetZPosition()
	{
	}
}
