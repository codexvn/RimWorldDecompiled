using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002702 RID: 9986
	public class RitualObligationTargetWorker_AnyRitualSpotIdeogramOrAltar : RitualObligationTargetWorker_AnyGatherSpot
	{
		// Token: 0x0600DB1C RID: 56092 RVA: 0x0040A153 File Offset: 0x00408353
		public RitualObligationTargetWorker_AnyRitualSpotIdeogramOrAltar()
		{
		}

		// Token: 0x0600DB1D RID: 56093 RVA: 0x0040A15B File Offset: 0x0040835B
		public RitualObligationTargetWorker_AnyRitualSpotIdeogramOrAltar(RitualObligationTargetFilterDef def) : base(def)
		{
		}

		// Token: 0x0600DB1E RID: 56094 RVA: 0x0040ADD7 File Offset: 0x00408FD7
		public override IEnumerable<TargetInfo> GetTargets(RitualObligation obligation, Map map)
		{
			if (!ModLister.CheckIdeology("Execution target"))
			{
				yield break;
			}
			foreach (TargetInfo targetInfo in RitualObligationTargetWorker_Altar.GetTargetsWorker(obligation, map, this.parent.ideo))
			{
				yield return targetInfo;
			}
			IEnumerator<TargetInfo> enumerator = null;
			List<Thing> ideograms = map.listerThings.ThingsOfDef(ThingDefOf.Ideogram);
			int num;
			for (int i = 0; i < ideograms.Count; i = num + 1)
			{
				yield return ideograms[i];
				num = i;
			}
			List<Thing> ritualSpots = map.listerThings.ThingsOfDef(ThingDefOf.RitualSpot);
			for (int i = 0; i < ritualSpots.Count; i = num + 1)
			{
				yield return ritualSpots[i];
				num = i;
			}
			yield break;
			yield break;
		}

		// Token: 0x0600DB1F RID: 56095 RVA: 0x0040ADF8 File Offset: 0x00408FF8
		protected override RitualTargetUseReport CanUseTargetInternal(TargetInfo target, RitualObligation obligation)
		{
			if (!target.HasThing)
			{
				return false;
			}
			Thing thing = target.Thing;
			if (thing.def == ThingDefOf.RitualSpot || thing.def == ThingDefOf.Ideogram)
			{
				return true;
			}
			return RitualObligationTargetWorker_Altar.CanUseTargetWorker(target, obligation, this.parent.ideo);
		}

		// Token: 0x0600DB20 RID: 56096 RVA: 0x0040AE55 File Offset: 0x00409055
		public override IEnumerable<string> GetTargetInfos(RitualObligation obligation)
		{
			foreach (string text in RitualObligationTargetWorker_Altar.GetTargetInfosWorker(this.parent.ideo))
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			yield return ThingDefOf.RitualSpot.label;
			yield break;
			yield break;
		}
	}
}
