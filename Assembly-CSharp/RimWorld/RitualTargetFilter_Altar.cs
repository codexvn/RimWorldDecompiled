using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020027DC RID: 10204
	public class RitualTargetFilter_Altar : RitualTargetFilter
	{
		// Token: 0x0600DF58 RID: 57176 RVA: 0x00419458 File Offset: 0x00417658
		public RitualTargetFilter_Altar()
		{
		}

		// Token: 0x0600DF59 RID: 57177 RVA: 0x00419460 File Offset: 0x00417660
		public RitualTargetFilter_Altar(RitualTargetFilterDef def) : base(def)
		{
		}

		// Token: 0x0600DF5A RID: 57178 RVA: 0x00419C48 File Offset: 0x00417E48
		public override bool CanStart(TargetInfo initiator, TargetInfo selectedTarget, out string rejectionReason)
		{
			TargetInfo targetInfo = this.BestTarget(initiator, selectedTarget);
			rejectionReason = "";
			if (!targetInfo.IsValid)
			{
				rejectionReason = "AbilityDisabledNoAltarOrRitualsSpot".Translate();
				return false;
			}
			return true;
		}

		// Token: 0x0600DF5B RID: 57179 RVA: 0x00419C84 File Offset: 0x00417E84
		public override TargetInfo BestTarget(TargetInfo initiator, TargetInfo selectedTarget)
		{
			Pawn pawn = initiator.Thing as Pawn;
			if (pawn == null)
			{
				return null;
			}
			Thing thing = null;
			float num = 99999f;
			foreach (Building building in this.CandidateBuildings(pawn.Ideo, initiator.Thing.Map))
			{
				if (building.def.isAltar && pawn.CanReach(building, PathEndMode.Touch, pawn.NormalMaxDanger(), false, false, TraverseMode.ByPawn))
				{
					int lengthHorizontalSquared = (pawn.Position - building.Position).LengthHorizontalSquared;
					if ((float)lengthHorizontalSquared < num)
					{
						thing = building;
						num = (float)lengthHorizontalSquared;
					}
				}
			}
			if (thing == null && this.def.fallbackToRitualSpot && pawn.Map != null)
			{
				foreach (Thing thing2 in pawn.Map.listerThings.ThingsOfDef(ThingDefOf.RitualSpot))
				{
					if (pawn.CanReach(thing2, PathEndMode.Touch, pawn.NormalMaxDanger(), false, false, TraverseMode.ByPawn))
					{
						int lengthHorizontalSquared2 = (pawn.Position - thing2.Position).LengthHorizontalSquared;
						if ((float)lengthHorizontalSquared2 < num)
						{
							thing = thing2;
							num = (float)lengthHorizontalSquared2;
						}
					}
				}
			}
			return thing;
		}

		// Token: 0x0600DF5C RID: 57180 RVA: 0x00419E04 File Offset: 0x00418004
		public override IEnumerable<string> GetTargetInfos(TargetInfo initiator)
		{
			yield return "RitualTargetGatherAltarOrRitualSpotInfo".Translate();
			yield break;
		}

		// Token: 0x0600DF5D RID: 57181 RVA: 0x00419E0D File Offset: 0x0041800D
		protected virtual IEnumerable<Building> CandidateBuildings(Ideo ideo, Map map)
		{
			if (map == null)
			{
				yield break;
			}
			foreach (Precept_Building precept_Building in from b in ideo.cachedPossibleBuildings
			where b.ThingDef.isAltar
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
