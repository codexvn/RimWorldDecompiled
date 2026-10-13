namespace UnityEngine;

public class WaitForSecondsRealtime : CustomYieldInstruction
{
	private float waitUntilTime = -1f;

	public float waitTime { get; set; }

	public override bool keepWaiting
	{
		get
		{
			if (waitUntilTime < 0f)
			{
				waitUntilTime = Time.realtimeSinceStartup + waitTime;
			}
			bool num = Time.realtimeSinceStartup < waitUntilTime;
			if (!num)
			{
				Reset();
			}
			return num;
		}
	}

	public WaitForSecondsRealtime(float time)
	{
		waitTime = time;
	}

	public new void Reset()
	{
		waitUntilTime = -1f;
	}
}
