using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E89 RID: 7817
	public class StoryWatcher_PopAdaptation : IExposable
	{
		// Token: 0x17001A87 RID: 6791
		// (get) Token: 0x0600AD8D RID: 44429 RVA: 0x003189A5 File Offset: 0x00316BA5
		public float AdaptDays
		{
			get
			{
				return this.adaptDays;
			}
		}

		// Token: 0x0600AD8E RID: 44430 RVA: 0x003189B0 File Offset: 0x00316BB0
		public void Notify_PawnEvent(Pawn p, PopAdaptationEvent ev)
		{
			if (!p.RaceProps.Humanlike)
			{
				return;
			}
			if (DebugViewSettings.writeStoryteller)
			{
				Log.Message("PopAdaptation event: " + ev.ToString() + " - " + ((p != null) ? p.ToString() : null));
			}
			if (ev == PopAdaptationEvent.GainedColonist)
			{
				this.adaptDays = 0f;
			}
		}

		// Token: 0x0600AD8F RID: 44431 RVA: 0x00318A10 File Offset: 0x00316C10
		public void PopAdaptationWatcherTick()
		{
			if (Find.TickManager.TicksGame % 30000 == 171)
			{
				float num = 0.5f;
				this.adaptDays += num;
			}
		}

		// Token: 0x0600AD90 RID: 44432 RVA: 0x00318A48 File Offset: 0x00316C48
		public void ExposeData()
		{
			Scribe_Values.Look<float>(ref this.adaptDays, "adaptDays", 0f, false);
		}

		// Token: 0x0600AD91 RID: 44433 RVA: 0x00318A60 File Offset: 0x00316C60
		public void Debug_OffsetAdaptDays(float days)
		{
			this.adaptDays += days;
		}

		// Token: 0x0600AD92 RID: 44434 RVA: 0x00318A70 File Offset: 0x00316C70
		public void ResetAdaptDays()
		{
			this.adaptDays = 0f;
		}

		// Token: 0x0400776F RID: 30575
		private float adaptDays;

		// Token: 0x04007770 RID: 30576
		private const int UpdateInterval = 30000;
	}
}
