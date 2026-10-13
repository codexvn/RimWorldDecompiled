using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020026FF RID: 9983
	public class RitualObligationTargetWorker_AnyRitualSpotOrAltar : RitualObligationTargetWorker_AnyGatherSpot
	{
		// Token: 0x0600DB05 RID: 56069 RVA: 0x0040A153 File Offset: 0x00408353
		public RitualObligationTargetWorker_AnyRitualSpotOrAltar()
		{
		}

		// Token: 0x0600DB06 RID: 56070 RVA: 0x0040A15B File Offset: 0x0040835B
		public RitualObligationTargetWorker_AnyRitualSpotOrAltar(RitualObligationTargetFilterDef def) : base(def)
		{
		}

		// Token: 0x0600DB07 RID: 56071 RVA: 0x0040A967 File Offset: 0x00408B67
		public override IEnumerable<TargetInfo> GetTargets(RitualObligation obligation, Map map)
		{
			if (!ModLister.CheckIdeology("Altar target"))
			{
				yield break;
			}
			foreach (TargetInfo targetInfo in RitualObligationTargetWorker_Altar.GetTargetsWorker(obligation, map, this.parent.ideo))
			{
				yield return targetInfo;
			}
			IEnumerator<TargetInfo> enumerator = null;
			List<Thing> ritualSpots = map.listerThings.ThingsOfDef(ThingDefOf.RitualSpot);
			int num;
			for (int i = 0; i < ritualSpots.Count; i = num + 1)
			{
				yield return ritualSpots[i];
				num = i;
			}
			yield break;
			yield break;
		}

		// Token: 0x0600DB08 RID: 56072 RVA: 0x0040A988 File Offset: 0x00408B88
		protected override RitualTargetUseReport CanUseTargetInternal(TargetInfo target, RitualObligation obligation)
		{
			if (!target.HasThing)
			{
				return false;
			}
			if (target.Thing.def == ThingDefOf.RitualSpot)
			{
				return true;
			}
			return RitualObligationTargetWorker_Altar.CanUseTargetWorker(target, obligation, this.parent.ideo);
		}

		// Token: 0x0600DB09 RID: 56073 RVA: 0x0040A9D6 File Offset: 0x00408BD6
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
