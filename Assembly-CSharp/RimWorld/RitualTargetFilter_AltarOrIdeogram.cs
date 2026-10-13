using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020027E0 RID: 10208
	public class RitualTargetFilter_AltarOrIdeogram : RitualTargetFilter_Altar
	{
		// Token: 0x0600DF73 RID: 57203 RVA: 0x0041A12B File Offset: 0x0041832B
		public RitualTargetFilter_AltarOrIdeogram()
		{
		}

		// Token: 0x0600DF74 RID: 57204 RVA: 0x0041A133 File Offset: 0x00418333
		public RitualTargetFilter_AltarOrIdeogram(RitualTargetFilterDef def) : base(def)
		{
		}

		// Token: 0x0600DF75 RID: 57205 RVA: 0x0041A13C File Offset: 0x0041833C
		public override bool CanStart(TargetInfo initiator, TargetInfo selectedTarget, out string rejectionReason)
		{
			TargetInfo targetInfo = this.BestTarget(initiator, selectedTarget);
			rejectionReason = "";
			if (!targetInfo.IsValid)
			{
				rejectionReason = "AbilityDisabledNoAltarIdeogramOrRitualsSpot".Translate();
				return false;
			}
			return true;
		}

		// Token: 0x0600DF76 RID: 57206 RVA: 0x0041A176 File Offset: 0x00418376
		public override IEnumerable<string> GetTargetInfos(TargetInfo initiator)
		{
			yield return "RitualTargetGatherAltarIdeogramOrRitualSpotInfo".Translate();
			yield break;
		}

		// Token: 0x0600DF77 RID: 57207 RVA: 0x0041A17F File Offset: 0x0041837F
		protected override IEnumerable<Building> CandidateBuildings(Ideo ideo, Map map)
		{
			if (map == null)
			{
				yield break;
			}
			foreach (Precept_Building precept_Building in from b in ideo.cachedPossibleBuildings
			where b.ThingDef.isAltar || b.ThingDef == ThingDefOf.Ideogram
			select b)
			{
				foreach (Building building in map.listerBuildings.AllBuildingsColonistOfDef(precept_Building.ThingDef))
				{
					yield return building;
				}
				List<Building>.Enumerator enumerator2 = default(List<Building>.Enumerator);
			}
			IEnumerator<Precept_Building> enumerator = null;
			yield break;
			yield break;
		}
	}
}
