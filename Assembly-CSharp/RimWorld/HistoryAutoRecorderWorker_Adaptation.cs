using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B67 RID: 7015
	public class HistoryAutoRecorderWorker_Adaptation : HistoryAutoRecorderWorker
	{
		// Token: 0x06009CA6 RID: 40102 RVA: 0x002D729A File Offset: 0x002D549A
		public override float PullRecord()
		{
			return Find.StoryWatcher.watcherAdaptation.AdaptDays;
		}
	}
}
