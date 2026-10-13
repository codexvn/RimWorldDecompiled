using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002E03 RID: 11779
	public class CompProperties_Serum : CompProperties
	{
		// Token: 0x0601093A RID: 67898 RVA: 0x004C7375 File Offset: 0x004C5575
		public CompProperties_Serum()
		{
			this.compClass = typeof(CompSerum);
		}

		// Token: 0x0601093B RID: 67899 RVA: 0x004C738D File Offset: 0x004C558D
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats(StatRequest req)
		{
			foreach (StatDrawEntry statDrawEntry in base.SpecialDisplayStats(req))
			{
				yield return statDrawEntry;
			}
			IEnumerator<StatDrawEntry> enumerator = null;
			ThingDef thingDef = req.Def as ThingDef;
			HediffDef hediffDef = (thingDef != null) ? thingDef.GetCompProperties<CompProperties_UseEffectAddHediff>().hediffDef : null;
			HediffCompProperties_DisappearsDisableable hediffCompProperties_DisappearsDisableable = (hediffDef != null) ? hediffDef.CompProps<HediffCompProperties_DisappearsDisableable>() : null;
			if (hediffCompProperties_DisappearsDisableable != null)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Serum, "StatsReport_SerumDuration".Translate(), hediffCompProperties_DisappearsDisableable.disappearsAfterTicks.min.ToStringTicksToPeriod(true, false, true, true, false), "StatsReport_SerumDuration_Desc".Translate(), 1000, null, null, false, false);
			}
			yield break;
			yield break;
		}
	}
}
