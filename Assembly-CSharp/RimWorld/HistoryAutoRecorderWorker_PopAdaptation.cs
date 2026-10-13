using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B68 RID: 7016
	public class HistoryAutoRecorderWorker_PopAdaptation : HistoryAutoRecorderWorker
	{
		// Token: 0x06009CA8 RID: 40104 RVA: 0x002D72AB File Offset: 0x002D54AB
		public override float PullRecord()
		{
			return Find.StoryWatcher.watcherPopAdaptation.AdaptDays;
		}
	}
}
