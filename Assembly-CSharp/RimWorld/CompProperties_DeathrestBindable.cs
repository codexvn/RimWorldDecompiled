using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002A62 RID: 10850
	public class CompProperties_DeathrestBindable : CompProperties
	{
		// Token: 0x0600F134 RID: 61748 RVA: 0x0045F837 File Offset: 0x0045DA37
		public CompProperties_DeathrestBindable()
		{
			this.compClass = typeof(CompDeathrestBindable);
		}

		// Token: 0x0600F135 RID: 61749 RVA: 0x0045F868 File Offset: 0x0045DA68
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats(StatRequest req)
		{
			if (!Mathf.Approximately(this.deathrestEffectivenessFactor, 1f))
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Building, "StatsReport_DeathrestEffectiveness".Translate(), this.deathrestEffectivenessFactor.ToStringPercent(), "StatsReport_DeathrestEffectiveness_Desc".Translate(), 900, null, null, false, false);
			}
			if (!this.mustBeLayingInToBind)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Building, "StatsReport_DeathrestConnectionLimit".Translate(), (this.stackLimit <= 0) ? "Unlimited".Translate().ToString() : this.stackLimit.ToString(), "StatsReport_DeathrestConnectionLimit_Desc".Translate(), 910, null, null, false, false);
			}
			yield break;
		}

		// Token: 0x04009F18 RID: 40728
		public int stackLimit;

		// Token: 0x04009F19 RID: 40729
		public bool countsTowardsBuildingLimit = true;

		// Token: 0x04009F1A RID: 40730
		public bool displayTimeActive = true;

		// Token: 0x04009F1B RID: 40731
		public float deathrestEffectivenessFactor = 1f;

		// Token: 0x04009F1C RID: 40732
		public bool mustBeLayingInToBind;

		// Token: 0x04009F1D RID: 40733
		public float hemogenLimitOffset;

		// Token: 0x04009F1E RID: 40734
		public HediffDef hediffToApply;

		// Token: 0x04009F1F RID: 40735
		public SoundDef soundWorking;

		// Token: 0x04009F20 RID: 40736
		public SoundDef soundStart;

		// Token: 0x04009F21 RID: 40737
		public SoundDef soundEnd;
	}
}
