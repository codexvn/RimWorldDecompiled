using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02002706 RID: 9990
	public class RitualObligationTargetWorker_IdeoBuildingOrRitualSpot : RitualObligationTargetFilter
	{
		// Token: 0x0600DB37 RID: 56119 RVA: 0x00408B0F File Offset: 0x00406D0F
		public RitualObligationTargetWorker_IdeoBuildingOrRitualSpot()
		{
		}

		// Token: 0x0600DB38 RID: 56120 RVA: 0x00408B17 File Offset: 0x00406D17
		public RitualObligationTargetWorker_IdeoBuildingOrRitualSpot(RitualObligationTargetFilterDef def) : base(def)
		{
		}

		// Token: 0x0600DB39 RID: 56121 RVA: 0x0040B351 File Offset: 0x00409551
		public override IEnumerable<TargetInfo> GetTargets(RitualObligation obligation, Map map)
		{
			return Enumerable.Empty<TargetInfo>();
		}

		// Token: 0x0600DB3A RID: 56122 RVA: 0x0040B358 File Offset: 0x00409558
		protected override RitualTargetUseReport CanUseTargetInternal(TargetInfo target, RitualObligation obligation)
		{
			if (!ModLister.CheckIdeology("Ideo building target"))
			{
				return false;
			}
			Building building = target.Thing as Building;
			if (building == null || building.Faction == null || !building.Faction.IsPlayer)
			{
				return false;
			}
			if (building.def == ThingDefOf.RitualSpot)
			{
				return true;
			}
			for (int i = 0; i < this.parent.ideo.PreceptsListForReading.Count; i++)
			{
				Precept_Building precept_Building = this.parent.ideo.PreceptsListForReading[i] as Precept_Building;
				if (precept_Building != null && building.def == precept_Building.ThingDef)
				{
					return true;
				}
			}
			if (building.TryGetComp<CompGatherSpot>() != null)
			{
				return true;
			}
			return false;
		}

		// Token: 0x0600DB3B RID: 56123 RVA: 0x0040B421 File Offset: 0x00409621
		public override IEnumerable<string> GetTargetInfos(RitualObligation obligation)
		{
			int num;
			for (int i = 0; i < this.parent.ideo.PreceptsListForReading.Count; i = num + 1)
			{
				Precept_Building precept_Building = this.parent.ideo.PreceptsListForReading[i] as Precept_Building;
				if (precept_Building != null)
				{
					yield return precept_Building.LabelCap;
				}
				num = i;
			}
			yield return ThingDefOf.RitualSpot.label;
			yield return "RitualTargetGatherSpotInfo".Translate();
			yield break;
		}

		// Token: 0x0600DB3C RID: 56124 RVA: 0x00002C42 File Offset: 0x00000E42
		public override List<string> MissingTargetBuilding(Ideo ideo)
		{
			return null;
		}
	}
}
