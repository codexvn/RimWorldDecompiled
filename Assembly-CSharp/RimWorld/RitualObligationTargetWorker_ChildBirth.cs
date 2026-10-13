using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02002718 RID: 10008
	public class RitualObligationTargetWorker_ChildBirth : RitualObligationTargetFilter
	{
		// Token: 0x0600DBA8 RID: 56232 RVA: 0x00408B0F File Offset: 0x00406D0F
		public RitualObligationTargetWorker_ChildBirth()
		{
		}

		// Token: 0x0600DBA9 RID: 56233 RVA: 0x00408B17 File Offset: 0x00406D17
		public RitualObligationTargetWorker_ChildBirth(RitualObligationTargetFilterDef def) : base(def)
		{
		}

		// Token: 0x0600DBAA RID: 56234 RVA: 0x0040C66F File Offset: 0x0040A86F
		public override IEnumerable<TargetInfo> GetTargets(RitualObligation obligation, Map map)
		{
			foreach (Pawn p in this.ColonistsInLabor(map))
			{
				foreach (Building_Bed t in PregnancyUtility.BedsForBirth(p))
				{
					yield return t;
				}
				IEnumerator<Building_Bed> enumerator2 = null;
			}
			IEnumerator<Pawn> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600DBAB RID: 56235 RVA: 0x0040C688 File Offset: 0x0040A888
		protected override RitualTargetUseReport CanUseTargetInternal(TargetInfo target, RitualObligation obligation)
		{
			if (!target.HasThing)
			{
				return false;
			}
			Building_Bed building_Bed = target.Thing as Building_Bed;
			if (building_Bed == null)
			{
				return false;
			}
			if (!building_Bed.def.building.bed_humanlike)
			{
				return false;
			}
			if (this.def.colonistThingsOnly && (target.Thing.Faction == null || !target.Thing.Faction.IsPlayer))
			{
				return false;
			}
			foreach (Pawn pawn in this.ColonistsInLabor(target.Map))
			{
				Lord lord = pawn.GetLord();
				if (!(((lord != null) ? lord.LordJob : null) is LordJob_Ritual_ChildBirth) && PregnancyUtility.IsUsableBedFor(pawn, pawn, building_Bed))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600DBAC RID: 56236 RVA: 0x0040C780 File Offset: 0x0040A980
		protected IEnumerable<Pawn> ColonistsInLabor(Map map)
		{
			foreach (Pawn pawn in map.mapPawns.FreeColonistsAndPrisoners)
			{
				if (pawn.health.hediffSet.HasHediff(HediffDefOf.PregnancyLabor, false))
				{
					yield return pawn;
				}
			}
			List<Pawn>.Enumerator enumerator = default(List<Pawn>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x0600DBAD RID: 56237 RVA: 0x0040C790 File Offset: 0x0040A990
		public override IEnumerable<string> GetTargetInfos(RitualObligation obligation)
		{
			yield return "RitualTargetChildBirth".Translate();
			yield break;
		}
	}
}
