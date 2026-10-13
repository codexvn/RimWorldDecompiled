using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02002716 RID: 10006
	public class RitualObligationTargetWorker_AnimaTree : RitualObligationTargetFilter
	{
		// Token: 0x0600DB9B RID: 56219 RVA: 0x00408B0F File Offset: 0x00406D0F
		public RitualObligationTargetWorker_AnimaTree()
		{
		}

		// Token: 0x0600DB9C RID: 56220 RVA: 0x00408B17 File Offset: 0x00406D17
		public RitualObligationTargetWorker_AnimaTree(RitualObligationTargetFilterDef def) : base(def)
		{
		}

		// Token: 0x0600DB9D RID: 56221 RVA: 0x0040B351 File Offset: 0x00409551
		public override IEnumerable<TargetInfo> GetTargets(RitualObligation obligation, Map map)
		{
			return Enumerable.Empty<TargetInfo>();
		}

		// Token: 0x0600DB9E RID: 56222 RVA: 0x0040C49C File Offset: 0x0040A69C
		protected override RitualTargetUseReport CanUseTargetInternal(TargetInfo target, RitualObligation obligation)
		{
			CompPsylinkable compPsylinkable = target.Thing.TryGetComp<CompPsylinkable>();
			if (compPsylinkable == null)
			{
				return false;
			}
			bool flag = false;
			bool flag2 = false;
			foreach (Pawn pawn in target.Map.mapPawns.FreeColonistsSpawned)
			{
				if (compPsylinkable.CanPsylink(pawn, null, false))
				{
					flag2 = true;
				}
				if (compPsylinkable.Props.requiredFocus.CanPawnUse(pawn))
				{
					flag = true;
				}
			}
			if (compPsylinkable.CompSubplant.SubplantsForReading.Count < compPsylinkable.Props.requiredSubplantCountPerPsylinkLevel[0])
			{
				return "RitualTargetAnimaTreeNotEnoughAnimaGrass".Translate(compPsylinkable.Props.requiredSubplantCountPerPsylinkLevel[0]);
			}
			if (!flag)
			{
				return "RitualTargetAnimaTreeNoPawnsWithNatureFocus".Translate();
			}
			if (!flag2)
			{
				return "RitualTargetAnimaTreeNoPawnsToLink".Translate();
			}
			return true;
		}

		// Token: 0x0600DB9F RID: 56223 RVA: 0x0040C5B8 File Offset: 0x0040A7B8
		public override IEnumerable<string> GetTargetInfos(RitualObligation obligation)
		{
			yield return "RitualTargetAnimaTreeInfo".Translate();
			yield break;
		}
	}
}
