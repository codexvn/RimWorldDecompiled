using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020026FC RID: 9980
	public class RitualObligationTargetWorker_AnyRitualSpot : RitualObligationTargetWorker_AnyGatherSpot
	{
		// Token: 0x0600DAF0 RID: 56048 RVA: 0x0040A153 File Offset: 0x00408353
		public RitualObligationTargetWorker_AnyRitualSpot()
		{
		}

		// Token: 0x0600DAF1 RID: 56049 RVA: 0x0040A15B File Offset: 0x0040835B
		public RitualObligationTargetWorker_AnyRitualSpot(RitualObligationTargetFilterDef def) : base(def)
		{
		}

		// Token: 0x0600DAF2 RID: 56050 RVA: 0x0040A757 File Offset: 0x00408957
		public override IEnumerable<TargetInfo> GetTargets(RitualObligation obligation, Map map)
		{
			if (!ModLister.CheckIdeology("Ritual spot target"))
			{
				yield break;
			}
			List<Thing> ritualSpots = map.listerThings.ThingsOfDef(ThingDefOf.RitualSpot);
			int num;
			for (int i = 0; i < ritualSpots.Count; i = num + 1)
			{
				yield return ritualSpots[i];
				num = i;
			}
			yield break;
		}

		// Token: 0x0600DAF3 RID: 56051 RVA: 0x0040A767 File Offset: 0x00408967
		protected override RitualTargetUseReport CanUseTargetInternal(TargetInfo target, RitualObligation obligation)
		{
			if (!target.HasThing)
			{
				return false;
			}
			return target.Thing.def == ThingDefOf.RitualSpot;
		}

		// Token: 0x0600DAF4 RID: 56052 RVA: 0x0040A791 File Offset: 0x00408991
		public override IEnumerable<string> GetTargetInfos(RitualObligation obligation)
		{
			yield return ThingDefOf.RitualSpot.label;
			yield break;
		}
	}
}
