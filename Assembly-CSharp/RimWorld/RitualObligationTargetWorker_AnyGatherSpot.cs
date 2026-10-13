using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020026F1 RID: 9969
	public class RitualObligationTargetWorker_AnyGatherSpot : RitualObligationTargetFilter
	{
		// Token: 0x0600DA98 RID: 55960 RVA: 0x00408B0F File Offset: 0x00406D0F
		public RitualObligationTargetWorker_AnyGatherSpot()
		{
		}

		// Token: 0x0600DA99 RID: 55961 RVA: 0x00408B17 File Offset: 0x00406D17
		public RitualObligationTargetWorker_AnyGatherSpot(RitualObligationTargetFilterDef def) : base(def)
		{
		}

		// Token: 0x0600DA9A RID: 55962 RVA: 0x0040974F File Offset: 0x0040794F
		public override IEnumerable<TargetInfo> GetTargets(RitualObligation obligation, Map map)
		{
			List<Thing> partySpot = map.listerThings.ThingsOfDef(ThingDefOf.PartySpot);
			int num;
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
		}

		// Token: 0x0600DA9B RID: 55963 RVA: 0x00409760 File Offset: 0x00407960
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
			CompGatherSpot compGatherSpot = thing.TryGetComp<CompGatherSpot>();
			if (compGatherSpot != null && compGatherSpot.Active)
			{
				return true;
			}
			return false;
		}

		// Token: 0x0600DA9C RID: 55964 RVA: 0x004097E1 File Offset: 0x004079E1
		public override IEnumerable<string> GetTargetInfos(RitualObligation obligation)
		{
			yield return "RitualTargetGatherSpotInfo".Translate();
			yield break;
		}
	}
}
