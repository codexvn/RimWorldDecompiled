namespace UnityEngine;

public class WaitForSeconds : YieldInstruction
{
	public readonly float Seconds;

	public WaitForSeconds(float seconds)
	{
		Seconds = seconds;
	}
}
