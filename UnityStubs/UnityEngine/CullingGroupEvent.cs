namespace UnityEngine;

public struct CullingGroupEvent
{
	public int index { get; internal set; }

	public bool isVisible { get; internal set; }

	public bool wasVisible { get; internal set; }

	public bool hasBecomeVisible
	{
		get
		{
			if (isVisible)
			{
				return !wasVisible;
			}
			return false;
		}
	}

	public bool hasBecomeInvisible
	{
		get
		{
			if (!isVisible)
			{
				return wasVisible;
			}
			return false;
		}
	}

	public int currentDistance { get; internal set; }

	public int previousDistance { get; internal set; }
}
