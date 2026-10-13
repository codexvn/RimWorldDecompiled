using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003B3B RID: 15163
	public class QuestNode_Root_Bossgroup : QuestNode
	{
		// Token: 0x060149E1 RID: 84449 RVA: 0x006278F0 File Offset: 0x00625AF0
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			Quest quest = QuestGen.quest;
			Map map = slate.Get<Map>("map", null, false);
			ThingDef thingDef = slate.Get<ThingDef>("reward", null, false);
			BossgroupDef bossgroupDef = slate.Get<BossgroupDef>("bossgroup", null, false);
			int timesSummoned = slate.Get<int>("wave", 0, false);
			Faction faction = Faction.OfMechanoids;
			if (faction == null)
			{
				List<FactionRelation> list = new List<FactionRelation>();
				foreach (Faction other in Find.FactionManager.AllFactionsListForReading)
				{
					list.Add(new FactionRelation
					{
						other = other,
						kind = FactionRelationKind.Hostile
					});
				}
				FactionDef mechanoid = FactionDefOf.Mechanoid;
				bool? hidden = new bool?(true);
				faction = FactionGenerator.NewGeneratedFactionWithRelations(new FactionGeneratorParms(mechanoid, default(IdeoGenerationParms), hidden), list);
				faction.temporary = true;
				Find.FactionManager.Add(faction);
			}
			List<Pawn> list2 = new List<Pawn>();
			List<Pawn> list3 = new List<Pawn>();
			int waveIndex = bossgroupDef.GetWaveIndex(timesSummoned);
			BossGroupWave wave = bossgroupDef.GetWave(waveIndex);
			PawnGenerationRequest request = new PawnGenerationRequest(bossgroupDef.boss.kindDef, faction, PawnGenerationContext.NonPlayer, null, true, false, false, true, false, 1f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false, false, false, -1, 0, false);
			for (int i = 0; i < wave.bossCount; i++)
			{
				Pawn pawn = PawnGenerator.GeneratePawn(request);
				if (!wave.bossApparel.NullOrEmpty<ThingDef>())
				{
					for (int j = 0; j < wave.bossApparel.Count; j++)
					{
						Apparel newApparel = (Apparel)ThingMaker.MakeThing(wave.bossApparel[j], null);
						pawn.apparel.Wear(newApparel, true, true);
					}
				}
				Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Decide);
				list3.Add(pawn);
			}
			for (int k = 0; k < wave.escorts.Count; k++)
			{
				PawnGenerationRequest request2 = new PawnGenerationRequest(wave.escorts[k].kindDef, faction, PawnGenerationContext.NonPlayer, null, true, false, false, true, false, 1f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false, false, false, -1, 0, false);
				for (int l = 0; l < wave.escorts[k].count; l++)
				{
					Pawn pawn2 = PawnGenerator.GeneratePawn(request2);
					list2.Add(pawn2);
					Find.WorldPawns.PassToWorld(pawn2, PawnDiscardDecideMode.Decide);
				}
			}
			slate.Set<MapParent>("mapParent", map.Parent, false);
			slate.Set<List<Pawn>>("escortees", list3.ToList<Pawn>(), false);
			IntVec3 intVec = DropCellFinder.FindRaidDropCenterDistant(map, false, true);
			IEnumerable<Pawn> enumerable = list3.Concat(list2);
			foreach (Pawn t in enumerable)
			{
				map.attackTargetsCache.UpdateTarget(t);
			}
			string text = QuestGen.GenerateNewSignal("BossgroupArrives", true);
			QuestPart_BossgroupArrives questPart_BossgroupArrives = new QuestPart_BossgroupArrives();
			questPart_BossgroupArrives.mapParent = map.Parent;
			questPart_BossgroupArrives.bossgroupDef = bossgroupDef;
			questPart_BossgroupArrives.minDelay = QuestNode_Root_Bossgroup.MinDelayTicksRange.RandomInRange;
			questPart_BossgroupArrives.maxDelay = QuestNode_Root_Bossgroup.MaxDelayTicksRange.RandomInRange;
			questPart_BossgroupArrives.inSignalEnable = QuestGen.slate.Get<string>("inSignal", null, false);
			questPart_BossgroupArrives.outSignalsCompleted.Add(text);
			quest.AddPart(questPart_BossgroupArrives);
			Quest quest4 = quest;
			MapParent parent = map.Parent;
			string text2 = text;
			quest4.DropPods(parent, enumerable, null, null, null, null, new bool?(false), false, false, false, text2, null, QuestPart.SignalListenMode.OngoingOnly, new IntVec3?(intVec), true, false, false, Faction.OfMechanoids);
			Quest quest2 = quest;
			LetterDef neutralEvent = LetterDefOf.NeutralEvent;
			string inSignal = null;
			string chosenPawnSignal = null;
			text2 = "LetterLabelBossgroupSummoned".Translate(bossgroupDef.boss.kindDef.LabelCap);
			string text3 = "LetterBossgroupSummoned".Translate(faction.NameColored.ToString()).ToString();
			quest2.Letter(neutralEvent, inSignal, chosenPawnSignal, Faction.OfMechanoids, null, false, QuestPart.SignalListenMode.OngoingOnly, null, false, text3, null, text2, null, null);
			Quest quest3 = quest;
			LetterDef bossgroup = LetterDefOf.Bossgroup;
			text3 = "LetterLabelBossgroupArrived".Translate(bossgroupDef.boss.kindDef.LabelCap);
			string inSignal2 = text;
			string chosenPawnSignal2 = null;
			text2 = "LetterBossgroupArrived".Translate(faction.NameColored.ToString(), bossgroupDef.LeaderDescription, bossgroupDef.boss.kindDef.label, faction.def.pawnsPlural, bossgroupDef.GetWaveDescription(waveIndex)).ToString();
			quest3.Letter(bossgroup, inSignal2, chosenPawnSignal2, Faction.OfMechanoids, null, false, QuestPart.SignalListenMode.OngoingOnly, enumerable, false, text2, null, text3, null, null);
			QuestPart_Bossgroup questPart_Bossgroup = new QuestPart_Bossgroup();
			questPart_Bossgroup.pawns.AddRange(enumerable);
			questPart_Bossgroup.faction = Faction.OfMechanoids;
			questPart_Bossgroup.mapParent = map.Parent;
			questPart_Bossgroup.bosses.AddRange(list3);
			questPart_Bossgroup.stageLocation = intVec;
			questPart_Bossgroup.inSignal = text;
			quest.AddPart(questPart_Bossgroup);
			quest.Alert("AlertBossgroupIncoming".Translate(bossgroupDef.boss.kindDef.LabelCap), "AlertBossgroupIncomingDesc".Translate(bossgroupDef.boss.kindDef.label), null, true, false, null, text);
			string inSignal3 = QuestGenUtility.HardcodedSignalWithQuestID("escortees.KilledLeavingsLeft");
			quest.ThingAnalyzed(thingDef, delegate
			{
				quest.Letter(LetterDefOf.PositiveEvent, null, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly, null, false, "[bossDefeatedLetterText]", null, "[bossDefeatedLetterLabel]", null, null);
			}, delegate
			{
				quest.Letter(LetterDefOf.PositiveEvent, null, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly, null, false, "[bossDefeatedStudyChipLetterText]", null, "[bossDefeatedLetterLabel]", null, null);
			}, inSignal3, null, null, QuestPart.SignalListenMode.OngoingOnly);
			quest.AnyPawnAlive(list3, null, delegate
			{
				quest.End(QuestEndOutcome.Unknown, 0, null, null, QuestPart.SignalListenMode.OngoingOnly, false, false);
			}, QuestGenUtility.HardcodedSignalWithQuestID("escortees.Killed"), null, null, null, QuestPart.SignalListenMode.OngoingOnly);
			quest.End(QuestEndOutcome.Unknown, 0, null, QuestGenUtility.HardcodedSignalWithQuestID("mapParent.Destroyed"), QuestPart.SignalListenMode.OngoingOnly, false, false);
		}

		// Token: 0x060149E2 RID: 84450 RVA: 0x00628008 File Offset: 0x00626208
		protected override bool TestRunInt(Slate slate)
		{
			return slate.Exists("wave", false) && slate.Exists("bossgroup", false) && slate.Exists("map", false) && slate.Exists("reward", false);
		}

		// Token: 0x0400E4E6 RID: 58598
		private static readonly IntRange MaxDelayTicksRange = new IntRange(60000, 180000);

		// Token: 0x0400E4E7 RID: 58599
		private static readonly IntRange MinDelayTicksRange = new IntRange(2500, 5000);
	}
}
