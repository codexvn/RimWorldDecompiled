namespace UnityEngine;

public class RangeAttribute : PropertyAttribute
{
	public readonly float min;

	public readonly float max;

	public RangeAttribute(float min, float max)
	{
		this.min = min;
		this.max = max;
	}
}
