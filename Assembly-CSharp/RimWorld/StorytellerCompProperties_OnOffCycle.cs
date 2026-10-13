using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E55 RID: 7765
	public class StorytellerCompProperties_OnOffCycle : StorytellerCompProperties
	{
		// Token: 0x17001A58 RID: 6744
		// (get) Token: 0x0600ACA5 RID: 44197 RVA: 0x00314D97 File Offset: 0x00312F97
		public IncidentCategoryDef IncidentCategory
		{
			get
			{
				if (this.incident != null)
				{
					return this.incident.category;
				}
				return this.category;
			}
		}

		// Token: 0x0600ACA6 RID: 44198 RVA: 0x00314DB3 File Offset: 0x00312FB3
		public StorytellerCompProperties_OnOffCycle()
		{
			this.compClass = typeof(StorytellerComp_OnOffCycle);
		}

		// Token: 0x0600ACA7 RID: 44199 RVA: 0x00314DD6 File Offset: 0x00312FD6
		public override IEnumerable<string> ConfigErrors(StorytellerDef parentDef)
		{
			if (this.incident != null && this.category != null)
			{
				yield return "incident and category should not both be defined";
			}
			if (this.onDays <= 0f)
			{
				yield return "onDays must be above zero";
			}
			if (this.numIncidentsRange.TrueMax <= 0f)
			{
				yield return "numIncidentRange not configured";
			}
			if (this.minSpacingDays * this.numIncidentsRange.TrueMax > this.onDays * 0.9f)
			{
				yield return "minSpacingDays too high compared to max number of incidents.";
			}
			yield break;
		}

		// Token: 0x040076BA RID: 30394
		public float onDays;

		// Token: 0x040076BB RID: 30395
		public float offDays;

		// Token: 0x040076BC RID: 30396
		public float onDaysNoTreeConnectors;

		// Token: 0x040076BD RID: 30397
		public float offDaysNoTreeConnectors;

		// Token: 0x040076BE RID: 30398
		public float minSpacingDays;

		// Token: 0x040076BF RID: 30399
		public FloatRange numIncidentsRange = FloatRange.Zero;

		// Token: 0x040076C0 RID: 30400
		public SimpleCurve acceptFractionByDaysPassedCurve;

		// Token: 0x040076C1 RID: 30401
		public SimpleCurve acceptPercentFactorPerThreatPointsCurve;

		// Token: 0x040076C2 RID: 30402
		public SimpleCurve acceptPercentFactorPerProgressScoreCurve;

		// Token: 0x040076C3 RID: 30403
		public IncidentDef incident;

		// Token: 0x040076C4 RID: 30404
		private IncidentCategoryDef category;

		// Token: 0x040076C5 RID: 30405
		public float forceRaidEnemyBeforeDaysPassed;
	}
}
