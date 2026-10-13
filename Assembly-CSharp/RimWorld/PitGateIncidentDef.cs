using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001AB0 RID: 6832
	public class PitGateIncidentDef : Def
	{
		// Token: 0x040067E5 RID: 26597
		public Type workerClass = typeof(PitGateIncidentWorker);

		// Token: 0x040067E6 RID: 26598
		public float baseChance;

		// Token: 0x040067E7 RID: 26599
		public IntRange durationRangeTicks;

		// Token: 0x040067E8 RID: 26600
		public bool usesThreatPoints;

		// Token: 0x040067E9 RID: 26601
		public int disableEnteringTicks;

		// Token: 0x040067EA RID: 26602
		[MustTranslate]
		public string disableEnteringReason;

		// Token: 0x040067EB RID: 26603
		[MustTranslate]
		public string letterText;

		// Token: 0x040067EC RID: 26604
		[MustTranslate]
		public string letterLabel;
	}
}
