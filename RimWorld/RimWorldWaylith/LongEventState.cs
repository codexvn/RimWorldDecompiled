using Verse;

namespace RimWorldWaylith;

public static class LongEventState
{
	public static bool EventWithoutStandardWindowRunning
	{
		get
		{
			LongEventHandler.QueuedLongEvent currentEvent = LongEventHandler.currentEvent;
			if (currentEvent != null)
			{
				return !currentEvent.UseStandardWindow;
			}
			return false;
		}
	}
}
