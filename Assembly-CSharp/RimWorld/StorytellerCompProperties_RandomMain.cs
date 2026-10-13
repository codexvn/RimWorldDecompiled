using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E63 RID: 7779
	public class StorytellerCompProperties_RandomMain : StorytellerCompProperties
	{
		// Token: 0x0600ACE3 RID: 44259 RVA: 0x003159B0 File Offset: 0x00313BB0
		public StorytellerCompProperties_RandomMain()
		{
			this.compClass = typeof(StorytellerComp_RandomMain);
		}

		// Token: 0x040076E8 RID: 30440
		public float mtbDays;

		// Token: 0x040076E9 RID: 30441
		public List<IncidentCategoryEntry> categoryWeights = new List<IncidentCategoryEntry>();

		// Token: 0x040076EA RID: 30442
		public float maxThreatBigIntervalDays = 99999f;

		// Token: 0x040076EB RID: 30443
		public FloatRange randomPointsFactorRange = new FloatRange(0.5f, 1.5f);

		// Token: 0x040076EC RID: 30444
		public bool skipThreatBigIfRaidBeacon;

		// Token: 0x040076ED RID: 30445
		public float spaceMtbDayFactor = 2f;

		// Token: 0x040076EE RID: 30446
		public float spaceMinSpacingDays = 2f;
	}
}
