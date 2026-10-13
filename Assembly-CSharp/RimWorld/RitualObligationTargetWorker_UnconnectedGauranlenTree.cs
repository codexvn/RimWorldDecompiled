using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002713 RID: 10003
	public class RitualObligationTargetWorker_UnconnectedGauranlenTree : RitualObligationTargetFilter
	{
		// Token: 0x0600DB86 RID: 56198 RVA: 0x00408B0F File Offset: 0x00406D0F
		public RitualObligationTargetWorker_UnconnectedGauranlenTree()
		{
		}

		// Token: 0x0600DB87 RID: 56199 RVA: 0x00408B17 File Offset: 0x00406D17
		public RitualObligationTargetWorker_UnconnectedGauranlenTree(RitualObligationTargetFilterDef def) : base(def)
		{
		}

		// Token: 0x0600DB88 RID: 56200 RVA: 0x0040C1F0 File Offset: 0x0040A3F0
		public override IEnumerable<TargetInfo> GetTargets(RitualObligation obligation, Map map)
		{
			List<Thing> trees = map.listerThings.ThingsOfDef(ThingDefOf.Plant_TreeGauranlen);
			int num;
			for (int i = 0; i < trees.Count; i = num + 1)
			{
				CompTreeConnection compTreeConnection = trees[i].TryGetComp<CompTreeConnection>();
				if (compTreeConnection != null && !compTreeConnection.Connected && !compTreeConnection.ConnectionTorn)
				{
					yield return trees[i];
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x0600DB89 RID: 56201 RVA: 0x0040C200 File Offset: 0x0040A400
		protected override RitualTargetUseReport CanUseTargetInternal(TargetInfo target, RitualObligation obligation)
		{
			Thing thing = target.Thing;
			CompTreeConnection compTreeConnection = thing.TryGetComp<CompTreeConnection>();
			if (compTreeConnection == null)
			{
				return false;
			}
			if (compTreeConnection.Connected)
			{
				return "RitualTargetConnectedGauranlenTree".Translate(thing.Named("TREE"), compTreeConnection.ConnectedPawn.Named("PAWN"));
			}
			if (compTreeConnection.ConnectionTorn)
			{
				return "RitualTargetConnectionTornGauranlenTree".Translate(thing.Named("TREE"), compTreeConnection.UntornInDurationTicks.ToStringTicksToPeriod(true, false, true, true, false)).Resolve().CapitalizeFirst();
			}
			return true;
		}

		// Token: 0x0600DB8A RID: 56202 RVA: 0x0040C2A4 File Offset: 0x0040A4A4
		public override IEnumerable<string> GetTargetInfos(RitualObligation obligation)
		{
			yield return "RitualTargetUnconnectedGaruanlenTreeInfo".Translate();
			yield break;
		}
	}
}
