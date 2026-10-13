using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020026F9 RID: 9977
	public class RitualObligationTargetWorker_AnyGatherSpotOrAltar : RitualObligationTargetWorker_AnyGatherSpot
	{
		// Token: 0x0600DAD9 RID: 56025 RVA: 0x0040A153 File Offset: 0x00408353
		public RitualObligationTargetWorker_AnyGatherSpotOrAltar()
		{
		}

		// Token: 0x0600DADA RID: 56026 RVA: 0x0040A15B File Offset: 0x0040835B
		public RitualObligationTargetWorker_AnyGatherSpotOrAltar(RitualObligationTargetFilterDef def) : base(def)
		{
		}

		// Token: 0x0600DADB RID: 56027 RVA: 0x0040A164 File Offset: 0x00408364
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
			List<Thing> partySpot = map.listerThings.ThingsOfDef(ThingDefOf.PartySpot);
			for (int i = 0; i < partySpot.Count; i = num + 1)
			{
				yield return partySpot[i];
				num = i;
			}
			for (int i = 0; i < map.gatherSpotLister.activeSpots.Count; i = num + 1)
			{
				yield return map.gatherSpotLister.activeSpots[i].parent;
				num = i;
			}
			yield break;
			yield break;
		}

		// Token: 0x0600DADC RID: 56028 RVA: 0x0040A184 File Offset: 0x00408384
		protected override RitualTargetUseReport CanUseTargetInternal(TargetInfo target, RitualObligation obligation)
		{
			if (!target.HasThing)
			{
				return false;
			}
			Thing thing = target.Thing;
			if (this.def.colonistThingsOnly && (thing.Faction == null || !thing.Faction.IsPlayer))
			{
				return false;
			}
			if (thing.def == ThingDefOf.PartySpot)
			{
				return true;
			}
			if (thing.def == ThingDefOf.RitualSpot)
			{
				return true;
			}
			CompGatherSpot compGatherSpot = thing.TryGetComp<CompGatherSpot>();
			if (compGatherSpot != null && compGatherSpot.Active)
			{
				return true;
			}
			return RitualObligationTargetWorker_Altar.CanUseTargetWorker(target, obligation, this.parent.ideo);
		}

		// Token: 0x0600DADD RID: 56029 RVA: 0x0040A22A File Offset: 0x0040842A
		public override IEnumerable<string> GetTargetInfos(RitualObligation obligation)
		{
			yield return "RitualTargetGatherSpotInfo".Translate();
			foreach (string text in RitualObligationTargetWorker_Altar.GetTargetInfosWorker(this.parent.ideo))
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			yield return ThingDefOf.RitualSpot.LabelCap;
			yield break;
			yield break;
		}
	}
}
