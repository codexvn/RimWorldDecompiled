using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003BED RID: 15341
	public class QuestNode_GenerateThreats : QuestNode
	{
		// Token: 0x06014D0D RID: 85261 RVA: 0x0063C334 File Offset: 0x0063A534
		protected override bool TestRunInt(Slate slate)
		{
			return Find.Storyteller.difficulty.allowViolentQuests && slate.Get<Map>("map", null, false) != null;
		}

		// Token: 0x06014D0E RID: 85262 RVA: 0x0063C35C File Offset: 0x0063A55C
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			Map map = slate.Get<Map>("map", null, false);
			QuestPart_ThreatsGenerator questPart_ThreatsGenerator = new QuestPart_ThreatsGenerator();
			questPart_ThreatsGenerator.threatStartTicks = this.threatStartTicks.GetValue(slate);
			questPart_ThreatsGenerator.inSignalEnable = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalEnable.GetValue(slate)) ?? slate.Get<string>("inSignal", null, false));
			questPart_ThreatsGenerator.inSignalDisable = QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalDisable.GetValue(slate));
			ThreatsGeneratorParams value = this.parms.GetValue(slate);
			value.faction = (this.faction.GetValue(slate) ?? value.faction);
			questPart_ThreatsGenerator.parms = value;
			questPart_ThreatsGenerator.mapParent = map.Parent;
			QuestGen.quest.AddPart(questPart_ThreatsGenerator);
			if (!this.storeThreatExampleAs.GetValue(slate).NullOrEmpty())
			{
				PawnGroupMakerParms pawnGroupMakerParms = new PawnGroupMakerParms();
				pawnGroupMakerParms.groupKind = PawnGroupKindDefOf.Combat;
				pawnGroupMakerParms.raidStrategy = RaidStrategyDefOf.ImmediateAttack;
				PawnGroupMakerParms pawnGroupMakerParms2 = pawnGroupMakerParms;
				Faction faction;
				if ((faction = value.faction) == null)
				{
					faction = (from x in Find.FactionManager.GetFactions(false, false, true, TechLevel.Industrial, false)
					where x.HostileTo(Faction.OfPlayer)
					select x).RandomElement<Faction>();
				}
				pawnGroupMakerParms2.faction = faction;
				float num = value.threatPoints ?? (StorytellerUtility.DefaultThreatPointsNow(map) * value.currentThreatPointsFactor);
				if (value.minThreatPoints != null)
				{
					num = Mathf.Max(num, value.minThreatPoints.Value);
				}
				pawnGroupMakerParms.points = IncidentWorker_Raid.AdjustedRaidPoints(num, PawnsArrivalModeDefOf.EdgeWalkIn, RaidStrategyDefOf.ImmediateAttack, pawnGroupMakerParms.faction, PawnGroupKindDefOf.Combat, map, null);
				IEnumerable<PawnKindDef> pawnKinds = PawnGroupMakerUtility.GeneratePawnKindsExample(pawnGroupMakerParms);
				slate.Set<string>(this.storeThreatExampleAs.GetValue(slate), PawnUtility.PawnKindsToLineList(pawnKinds, "  - ", ColoredText.ThreatColor), false);
			}
		}

		// Token: 0x0400E7C4 RID: 59332
		[NoTranslate]
		public SlateRef<string> inSignalEnable;

		// Token: 0x0400E7C5 RID: 59333
		[NoTranslate]
		public SlateRef<string> inSignalDisable;

		// Token: 0x0400E7C6 RID: 59334
		[NoTranslate]
		public SlateRef<string> storeThreatExampleAs;

		// Token: 0x0400E7C7 RID: 59335
		[NoTranslate]
		public SlateRef<int> threatStartTicks;

		// Token: 0x0400E7C8 RID: 59336
		public SlateRef<ThreatsGeneratorParams> parms;

		// Token: 0x0400E7C9 RID: 59337
		public SlateRef<Faction> faction;
	}
}
