using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003BEF RID: 15343
	public class QuestNode_GetExampleRaid : QuestNode
	{
		// Token: 0x06014D13 RID: 85267 RVA: 0x0061EFF5 File Offset: 0x0061D1F5
		protected override bool TestRunInt(Slate slate)
		{
			return slate.Exists("map", false);
		}

		// Token: 0x06014D14 RID: 85268 RVA: 0x0063C544 File Offset: 0x0063A744
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			Map target = slate.Get<Map>("map", null, false);
			PawnGroupMakerParms pawnGroupMakerParms = new PawnGroupMakerParms();
			pawnGroupMakerParms.groupKind = PawnGroupKindDefOf.Combat;
			pawnGroupMakerParms.raidStrategy = RaidStrategyDefOf.ImmediateAttack;
			Faction faction;
			if ((faction = this.faction.GetValue(slate)) == null)
			{
				faction = (from x in Find.FactionManager.GetFactions(false, false, true, TechLevel.Industrial, false)
				where x.HostileTo(Faction.OfPlayer)
				select x).RandomElement<Faction>();
			}
			pawnGroupMakerParms.faction = faction;
			PawnGroupMakerParms pawnGroupMakerParms2 = pawnGroupMakerParms;
			pawnGroupMakerParms2.points = IncidentWorker_Raid.AdjustedRaidPoints(this.points.GetValue(slate), PawnsArrivalModeDefOf.EdgeWalkIn, RaidStrategyDefOf.ImmediateAttack, pawnGroupMakerParms2.faction, PawnGroupKindDefOf.Combat, target, null);
			IEnumerable<PawnKindDef> pawnKinds = PawnGroupMakerUtility.GeneratePawnKindsExample(pawnGroupMakerParms2);
			slate.Set<string>(this.storeAs.GetValue(slate), PawnUtility.PawnKindsToLineList(pawnKinds, "  - ", ColoredText.ThreatColor), false);
		}

		// Token: 0x0400E7CC RID: 59340
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x0400E7CD RID: 59341
		public SlateRef<Faction> faction;

		// Token: 0x0400E7CE RID: 59342
		public SlateRef<float> points;
	}
}
