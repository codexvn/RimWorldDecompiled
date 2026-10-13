using System;

namespace RimWorld
{
	// Token: 0x02001B69 RID: 7017
	public class HistoryAutoRecorderWorker_PopIntent : HistoryAutoRecorderWorker
	{
		// Token: 0x06009CAA RID: 40106 RVA: 0x002D72BC File Offset: 0x002D54BC
		public override float PullRecord()
		{
			return StorytellerUtilityPopulation.PopulationIntent * 10f;
		}
	}
}
