using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E85 RID: 7813
	public sealed class StoryWatcher : IExposable
	{
		// Token: 0x0600AD7F RID: 44415 RVA: 0x00318662 File Offset: 0x00316862
		public void StoryWatcherTick()
		{
			this.watcherAdaptation.AdaptationWatcherTick();
			this.watcherPopAdaptation.PopAdaptationWatcherTick();
		}

		// Token: 0x0600AD80 RID: 44416 RVA: 0x0031867C File Offset: 0x0031687C
		public void ExposeData()
		{
			Scribe_Deep.Look<StatsRecord>(ref this.statsRecord, "statsRecord", Array.Empty<object>());
			Scribe_Deep.Look<StoryWatcher_Adaptation>(ref this.watcherAdaptation, "watcherAdaptation", Array.Empty<object>());
			Scribe_Deep.Look<StoryWatcher_PopAdaptation>(ref this.watcherPopAdaptation, "watcherPopAdaptation", Array.Empty<object>());
			BackCompatibility.PostExposeData(this);
		}

		// Token: 0x04007762 RID: 30562
		public StatsRecord statsRecord = new StatsRecord();

		// Token: 0x04007763 RID: 30563
		public StoryWatcher_Adaptation watcherAdaptation = new StoryWatcher_Adaptation();

		// Token: 0x04007764 RID: 30564
		public StoryWatcher_PopAdaptation watcherPopAdaptation = new StoryWatcher_PopAdaptation();
	}
}
