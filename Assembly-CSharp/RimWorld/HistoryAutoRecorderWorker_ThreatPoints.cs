using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B66 RID: 7014
	public class HistoryAutoRecorderWorker_ThreatPoints : HistoryAutoRecorderWorker
	{
		// Token: 0x06009CA4 RID: 40100 RVA: 0x002D7273 File Offset: 0x002D5473
		public override float PullRecord()
		{
			if (Find.AnyPlayerHomeMap == null)
			{
				return 0f;
			}
			return StorytellerUtility.DefaultThreatPointsNow(Find.AnyPlayerHomeMap) / 10f;
		}
	}
}
