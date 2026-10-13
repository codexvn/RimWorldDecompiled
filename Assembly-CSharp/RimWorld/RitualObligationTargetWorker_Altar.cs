using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020026F4 RID: 9972
	public class RitualObligationTargetWorker_Altar : RitualObligationTargetFilter
	{
		// Token: 0x0600DAAD RID: 55981 RVA: 0x00408B0F File Offset: 0x00406D0F
		public RitualObligationTargetWorker_Altar()
		{
		}

		// Token: 0x0600DAAE RID: 55982 RVA: 0x00408B17 File Offset: 0x00406D17
		public RitualObligationTargetWorker_Altar(RitualObligationTargetFilterDef def) : base(def)
		{
		}

		// Token: 0x0600DAAF RID: 55983 RVA: 0x00409A2B File Offset: 0x00407C2B
		public override IEnumerable<TargetInfo> GetTargets(RitualObligation obligation, Map map)
		{
			if (!ModLister.CheckIdeology("Altar target"))
			{
				yield break;
			}
			Ideo ideo = this.parent.ideo;
			foreach (TargetInfo targetInfo in RitualObligationTargetWorker_Altar.GetTargetsWorker(obligation, map, ideo))
			{
				yield return targetInfo;
			}
			IEnumerator<TargetInfo> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600DAB0 RID: 55984 RVA: 0x00409A49 File Offset: 0x00407C49
		public static IEnumerable<TargetInfo> GetTargetsWorker(RitualObligation obligation, Map map, Ideo ideo)
		{
			int num;
			for (int i = 0; i < ideo.PreceptsListForReading.Count; i = num + 1)
			{
				Precept_Building precept_Building = ideo.PreceptsListForReading[i] as Precept_Building;
				if (precept_Building != null && precept_Building.ThingDef.isAltar)
				{
					foreach (Thing t in precept_Building.presenceDemand.AllBuildings(map))
					{
						yield return t;
					}
					IEnumerator<Thing> enumerator = null;
				}
				num = i;
			}
			yield break;
			yield break;
		}

		// Token: 0x0600DAB1 RID: 55985 RVA: 0x00409A60 File Offset: 0x00407C60
		protected override RitualTargetUseReport CanUseTargetInternal(TargetInfo target, RitualObligation obligation)
		{
			return RitualObligationTargetWorker_Altar.CanUseTargetWorker(target, obligation, this.parent.ideo);
		}

		// Token: 0x0600DAB2 RID: 55986 RVA: 0x00409A7C File Offset: 0x00407C7C
		public static bool CanUseTargetWorker(TargetInfo target, RitualObligation obligation, Ideo ideo)
		{
			Building building = target.Thing as Building;
			return building != null && building.Faction != null && building.Faction.IsPlayer && RitualObligationTargetWorker_Altar.GetTargetsWorker(obligation, building.Map, ideo).Contains(building);
		}

		// Token: 0x0600DAB3 RID: 55987 RVA: 0x00409ACD File Offset: 0x00407CCD
		public override IEnumerable<string> GetTargetInfos(RitualObligation obligation)
		{
			foreach (string text in RitualObligationTargetWorker_Altar.GetTargetInfosWorker(this.parent.ideo))
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600DAB4 RID: 55988 RVA: 0x00409ADD File Offset: 0x00407CDD
		public static IEnumerable<string> GetTargetInfosWorker(Ideo ideo)
		{
			int num;
			for (int i = 0; i < ideo.PreceptsListForReading.Count; i = num + 1)
			{
				Precept_Building precept_Building = ideo.PreceptsListForReading[i] as Precept_Building;
				if (precept_Building != null && precept_Building.ThingDef.isAltar)
				{
					yield return precept_Building.LabelCap;
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x0600DAB5 RID: 55989 RVA: 0x00409AED File Offset: 0x00407CED
		public override List<string> MissingTargetBuilding(Ideo ideo)
		{
			if (!this.GetTargetInfos(null).Any<string>())
			{
				return new List<string>
				{
					"Altar".Translate()
				};
			}
			return null;
		}
	}
}
