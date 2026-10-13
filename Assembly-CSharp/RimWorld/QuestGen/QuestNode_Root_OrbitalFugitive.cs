using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003B7A RID: 15226
	public class QuestNode_Root_OrbitalFugitive : QuestNode
	{
		// Token: 0x06014B01 RID: 84737 RVA: 0x00624C29 File Offset: 0x00622E29
		protected override bool TestRunInt(Slate slate)
		{
			return ModsConfig.OdysseyActive;
		}

		// Token: 0x06014B02 RID: 84738 RVA: 0x0062F4BC File Offset: 0x0062D6BC
		protected override void RunInt()
		{
			Quest quest = QuestGen.quest;
			Slate slate = QuestGen.slate;
			float a = slate.Get<float>("points", 0f, false);
			Site site = slate.Get<Site>("site", null, false);
			string inSignal = QuestGenUtility.HardcodedSignalWithQuestID("site.MapGenerated");
			string inSignal2 = QuestGenUtility.HardcodedSignalWithQuestID("site.MapRemoved");
			Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(PawnKindDefOf.Salvager_Elite, Faction.OfSalvagers, PawnGenerationContext.NonPlayer, null, false, false, false, true, true, 1f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false, false, false, -1, 0, false));
			slate.Set<Pawn>("fugitive", pawn, false);
			IEnumerable<Pawn> collection = PawnGroupMakerUtility.GeneratePawns(new PawnGroupMakerParms
			{
				groupKind = PawnGroupKindDefOf.Combat,
				faction = Faction.OfSalvagers,
				tile = site.Tile,
				points = Mathf.Max(a, Faction.OfSalvagers.def.MinPointsToGeneratePawnGroup(PawnGroupKindDefOf.Combat, null) * 1.05f)
			}, true);
			List<Pawn> list = new List<Pawn>();
			list.Add(pawn);
			list.AddRange(collection);
			quest.AddPart(new QuestPart_SpawnPawnsInStructure(list, inSignal));
			quest.End(QuestEndOutcome.Fail, 0, null, inSignal2, QuestPart.SignalListenMode.OngoingOnly, false, false);
		}
	}
}
