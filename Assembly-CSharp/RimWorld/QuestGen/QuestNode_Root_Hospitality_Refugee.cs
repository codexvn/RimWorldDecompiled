using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003B54 RID: 15188
	public class QuestNode_Root_Hospitality_Refugee : QuestNode
	{
		// Token: 0x06014A41 RID: 84545 RVA: 0x0062A6CC File Offset: 0x006288CC
		protected override void RunInt()
		{
			if (!ModLister.CheckRoyalty("Hospitality refugee"))
			{
				return;
			}
			Quest quest = QuestGen.quest;
			Slate slate = QuestGen.slate;
			Map map = QuestGen_Get.GetMap(false, null, false);
			int lodgerCount = QuestNode_Root_Beggars.LodgerCountFromPopulation(slate, map);
			int num = 0;
			bool var = false;
			if (Find.Storyteller.difficulty.ChildrenAllowed && lodgerCount >= 2)
			{
				List<ValueTuple<int, float>> list = new List<ValueTuple<int, float>>();
				list.Add(new ValueTuple<int, float>(0, 0.7f));
				list.Add(new ValueTuple<int, float>(Rand.Range(1, lodgerCount / 2), 0.2f));
				list.Add(new ValueTuple<int, float>(lodgerCount - 1, 0.1f));
				ValueTuple<int, float> valueTuple;
				list.TryRandomElementByWeight((ValueTuple<int, float> p) => p.Item2, out valueTuple);
				num = valueTuple.Item1;
				var = (num == lodgerCount - 1);
			}
			int questDurationDays = QuestNode_Root_Hospitality_Refugee.QuestDurationDaysRange.RandomInRange;
			int questDurationTicks = questDurationDays * 60000;
			List<FactionRelation> list2 = new List<FactionRelation>();
			foreach (Faction faction3 in Find.FactionManager.AllFactionsListForReading)
			{
				if (!faction3.def.PermanentlyHostileTo(FactionDefOf.OutlanderRefugee))
				{
					list2.Add(new FactionRelation
					{
						other = faction3,
						kind = FactionRelationKind.Neutral
					});
				}
			}
			FactionDef outlanderRefugee = FactionDefOf.OutlanderRefugee;
			bool? flag = new bool?(true);
			FactionGeneratorParms factionGeneratorParms = new FactionGeneratorParms(outlanderRefugee, default(IdeoGenerationParms), flag);
			if (ModsConfig.IdeologyActive)
			{
				factionGeneratorParms.ideoGenerationParms = new IdeoGenerationParms(factionGeneratorParms.factionDef, false, (from p in DefDatabase<PreceptDef>.AllDefs
				where p.proselytizes || p.approvesOfCharity
				select p).ToList<PreceptDef>(), null, null, false, false, false, false, "", null, null, false, "", false);
			}
			Faction faction = FactionGenerator.NewGeneratedFactionWithRelations(factionGeneratorParms, list2);
			faction.temporary = true;
			Find.FactionManager.Add(faction);
			string lodgerRecruitedSignal = QuestGenUtility.HardcodedSignalWithQuestID("lodgers.Recruited");
			string text = QuestGenUtility.HardcodedSignalWithQuestID("lodgers.Arrested");
			string item = QuestGenUtility.HardcodedSignalWithQuestID("lodgers.BecameMutant");
			string inSignalDestroyed = QuestGenUtility.HardcodedSignalWithQuestID("lodgers.Destroyed");
			string inSignalKidnapped = QuestGenUtility.HardcodedSignalWithQuestID("lodgers.Kidnapped");
			string inSignalLeftMap = QuestGenUtility.HardcodedSignalWithQuestID("lodgers.LeftMap");
			string inSignalLeftBehind = QuestGenUtility.HardcodedSignalWithQuestID("lodgers.LeftBehind");
			string inSignalBanished = QuestGenUtility.HardcodedSignalWithQuestID("lodgers.Banished");
			string lodgerArrestedOrRecruited = QuestGen.GenerateNewSignal("Lodger_ArrestedOrRecruited", true);
			quest.AnySignal(new List<string>
			{
				lodgerRecruitedSignal,
				text
			}, null, new List<string>
			{
				lodgerArrestedOrRecruited
			}, QuestPart.SignalListenMode.OngoingOnly);
			List<Pawn> pawns = new List<Pawn>();
			Action <>9__7;
			for (int i = 0; i < lodgerCount; i++)
			{
				DevelopmentalStage developmentalStages = (i > 0 && i >= lodgerCount - num) ? DevelopmentalStage.Child : DevelopmentalStage.Adult;
				Pawn pawn = quest.GeneratePawn(PawnKindDefOf.Refugee, faction, true, null, 0f, true, null, 0f, 0f, false, true, developmentalStages, true);
				pawns.Add(pawn);
				Quest quest7 = quest;
				Pawn pawn3 = pawn;
				string letterLabel = "LetterJoinOfferLabel".Translate(pawn.Named("PAWN"));
				string letterTitle = "LetterJoinOfferTitle".Translate(pawn.Named("PAWN"));
				string letterText = "LetterJoinOfferText".Translate(pawn.Named("PAWN"), map.Parent.Named("MAP"));
				Action accepted = delegate()
				{
					quest.JoinPlayer(map.Parent, Gen.YieldSingle<Pawn>(pawn), true, false, null);
					Quest quest6 = quest;
					LetterDef positiveEvent = LetterDefOf.PositiveEvent;
					string inSignal = null;
					string chosenPawnSignal = null;
					Faction relatedFaction = null;
					MapParent useColonistsOnMap = null;
					bool useColonistsFromCaravanArg = false;
					QuestPart.SignalListenMode signalListenMode = QuestPart.SignalListenMode.OngoingOnly;
					IEnumerable<object> lookTargets = null;
					bool filterDeadPawnsFromLookTargets = false;
					string label = "LetterLabelMessageRecruitSuccess".Translate() + ": " + pawn.LabelShortCap;
					quest6.Letter(positiveEvent, inSignal, chosenPawnSignal, relatedFaction, useColonistsOnMap, useColonistsFromCaravanArg, signalListenMode, lookTargets, filterDeadPawnsFromLookTargets, "MessageRecruitJoinOfferAccepted".Translate(pawn.Named("RECRUITEE")), null, label, null, null);
					quest.SignalPass(null, null, lodgerRecruitedSignal);
				};
				Action rejected;
				if ((rejected = <>9__7) == null)
				{
					rejected = (<>9__7 = delegate()
					{
						quest.RecordHistoryEvent(HistoryEventDefOf.CharityRefused_ThreatReward_Joiner, null, QuestPart.SignalListenMode.OngoingOnly);
					});
				}
				quest7.PawnJoinOffer(pawn3, letterLabel, letterTitle, letterText, accepted, rejected, null, null, null, true, false);
			}
			slate.Set<List<Pawn>>("lodgers", pawns, false);
			faction.leader = pawns.First<Pawn>();
			Pawn asker = pawns.First<Pawn>();
			quest.SetFactionHidden(faction, false, null);
			QuestPart_ExtraFaction extraFactionPart = quest.ExtraFaction(faction, pawns, ExtraFactionType.MiniFaction, false, new List<string>
			{
				lodgerRecruitedSignal,
				item
			});
			quest.PawnsArrive(pawns, null, map.Parent, null, true, null, "[lodgersArriveLetterLabel]", "[lodgersArriveLetterText]", null, null, false, false, true);
			QuestPart_Choice questPart_Choice = quest.RewardChoice(null, null);
			QuestPart_Choice.Choice choice = new QuestPart_Choice.Choice();
			choice.rewards.Add(new Reward_VisitorsHelp());
			choice.rewards.Add(new Reward_PossibleFutureReward());
			if (ModsConfig.IdeologyActive && Faction.OfPlayer.ideos.FluidIdeo != null)
			{
				choice.rewards.Add(new Reward_DevelopmentPoints(quest));
			}
			questPart_Choice.choices.Add(choice);
			quest.SetAllApparelLocked(pawns, null);
			string assaultColonySignal = QuestGen.GenerateNewSignal("AssaultColony", true);
			Action <>9__8;
			Action item2 = delegate()
			{
				int num2 = Mathf.FloorToInt(QuestNode_Root_Hospitality_Refugee.MutinyTimeRange.RandomInRange * (float)questDurationTicks);
				Quest quest6 = quest;
				int delayTicks = num2;
				Action inner;
				if ((inner = <>9__8) == null)
				{
					inner = (<>9__8 = delegate()
					{
						quest.Letter(LetterDefOf.ThreatBig, null, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly, null, false, "[mutinyLetterText]", null, "[mutinyLetterLabel]", null, null);
						quest.SignalPass(null, null, assaultColonySignal);
						quest.End(QuestEndOutcome.Unknown, 0, null, null, QuestPart.SignalListenMode.OngoingOnly, false, false);
					});
				}
				quest6.Delay(delayTicks, inner, null, null, null, false, null, null, false, null, null, "Mutiny (" + num2.ToStringTicksToDays("F1") + ")", false, QuestPart.SignalListenMode.OngoingOnly, false);
			};
			Action item3 = delegate()
			{
				int num2 = Mathf.FloorToInt(QuestNode_Root_Hospitality_Refugee.BetrayalOfferTimeRange.RandomInRange * (float)questDurationTicks);
				Pawn factionOpponent = quest.GetPawn(QuestNode_Root_Hospitality_Refugee.FactionOpponentPawnParams);
				quest.Delay(num2, delegate
				{
					QuestPart_AddQuest_RefugeeBetrayal questPart_AddQuest_RefugeeBetrayal = new QuestPart_AddQuest_RefugeeBetrayal();
					questPart_AddQuest_RefugeeBetrayal.acceptee = quest.AccepterPawn;
					questPart_AddQuest_RefugeeBetrayal.lodgers = pawns;
					questPart_AddQuest_RefugeeBetrayal.refugeeFaction = extraFactionPart.extraFaction;
					questPart_AddQuest_RefugeeBetrayal.factionOpponent = factionOpponent;
					questPart_AddQuest_RefugeeBetrayal.inSignal = QuestGen.slate.Get<string>("inSignal", null, false);
					questPart_AddQuest_RefugeeBetrayal.inSignalRemovePawn = lodgerArrestedOrRecruited;
					questPart_AddQuest_RefugeeBetrayal.parent = quest;
					questPart_AddQuest_RefugeeBetrayal.asker = asker;
					questPart_AddQuest_RefugeeBetrayal.mapParent = map.Parent;
					questPart_AddQuest_RefugeeBetrayal.sendAvailableLetter = true;
					quest.AddPart(questPart_AddQuest_RefugeeBetrayal);
				}, null, null, null, false, null, null, false, null, null, "BetrayalOffer (" + num2.ToStringTicksToDays("F1") + ")", false, QuestPart.SignalListenMode.OngoingOnly, false);
			};
			if (Find.Storyteller.difficulty.allowViolentQuests)
			{
				List<Tuple<float, Action>> list3 = new List<Tuple<float, Action>>();
				if (!Find.Storyteller.difficulty.ChildRaidersAllowed && num > 0)
				{
					list3.Add(Tuple.Create<float, Action>(0.25f, delegate()
					{
					}));
				}
				else
				{
					list3.Add(Tuple.Create<float, Action>(0.25f, item2));
				}
				Pawn pawn2;
				if (QuestGen_Pawns.GetPawnTest(QuestNode_Root_Hospitality_Refugee.FactionOpponentPawnParams, out pawn2))
				{
					list3.Add(Tuple.Create<float, Action>(0.25f, item3));
				}
				list3.Add(Tuple.Create<float, Action>(0.5f, delegate()
				{
				}));
				Tuple<float, Action> tuple;
				if (list3.TryRandomElementByWeight((Tuple<float, Action> t) => t.Item1, out tuple))
				{
					tuple.Item2();
				}
			}
			QuestPart_RefugeeInteractions questPart_RefugeeInteractions = new QuestPart_RefugeeInteractions();
			questPart_RefugeeInteractions.inSignalEnable = QuestGen.slate.Get<string>("inSignal", null, false);
			questPart_RefugeeInteractions.inSignalDestroyed = inSignalDestroyed;
			questPart_RefugeeInteractions.inSignalArrested = text;
			questPart_RefugeeInteractions.inSignalSurgeryViolation = QuestGenUtility.HardcodedSignalWithQuestID("lodgers.SurgeryViolation");
			questPart_RefugeeInteractions.inSignalPsychicRitualTarget = QuestGenUtility.HardcodedSignalWithQuestID("lodgers.PsychicRitualTarget");
			questPart_RefugeeInteractions.inSignalKidnapped = inSignalKidnapped;
			questPart_RefugeeInteractions.inSignalRecruited = lodgerRecruitedSignal;
			questPart_RefugeeInteractions.inSignalAssaultColony = assaultColonySignal;
			questPart_RefugeeInteractions.inSignalLeftMap = inSignalLeftMap;
			questPart_RefugeeInteractions.inSignalLeftBehind = inSignalLeftBehind;
			questPart_RefugeeInteractions.inSignalBanished = inSignalBanished;
			questPart_RefugeeInteractions.outSignalDestroyed_AssaultColony = QuestGen.GenerateNewSignal("LodgerDestroyed_AssaultColony", true);
			questPart_RefugeeInteractions.outSignalDestroyed_LeaveColony = QuestGen.GenerateNewSignal("LodgerDestroyed_LeaveColony", true);
			questPart_RefugeeInteractions.outSignalDestroyed_BadThought = QuestGen.GenerateNewSignal("LodgerDestroyed_BadThought", true);
			questPart_RefugeeInteractions.outSignalArrested_AssaultColony = QuestGen.GenerateNewSignal("LodgerArrested_AssaultColony", true);
			questPart_RefugeeInteractions.outSignalArrested_LeaveColony = QuestGen.GenerateNewSignal("LodgerArrested_LeaveColony", true);
			questPart_RefugeeInteractions.outSignalArrested_BadThought = QuestGen.GenerateNewSignal("LodgerArrested_BadThought", true);
			questPart_RefugeeInteractions.outSignalLeftBehind_LeaveColony = QuestGen.GenerateNewSignal("LodgerLeftBehind_LeaveColony", true);
			questPart_RefugeeInteractions.outSignalLeftBehind_BadThought = QuestGen.GenerateNewSignal("LodgerLeftBehind_BadThought", true);
			questPart_RefugeeInteractions.outSignalSurgeryViolation_AssaultColony = QuestGen.GenerateNewSignal("LodgerSurgeryViolation_AssaultColony", true);
			questPart_RefugeeInteractions.outSignalSurgeryViolation_LeaveColony = QuestGen.GenerateNewSignal("LodgerSurgeryViolation_LeaveColony", true);
			questPart_RefugeeInteractions.outSignalSurgeryViolation_BadThought = QuestGen.GenerateNewSignal("LodgerSurgeryViolation_BadThought", true);
			questPart_RefugeeInteractions.outSignalPsychicRitualTarget_AssaultColony = QuestGen.GenerateNewSignal("LodgerPsychicRitualTarget_AssaultColony", true);
			questPart_RefugeeInteractions.outSignalPsychicRitualTarget_LeaveColony = QuestGen.GenerateNewSignal("LodgerPsychicRitualTarget_LeaveColony", true);
			questPart_RefugeeInteractions.outSignalPsychicRitualTarget_BadThought = QuestGen.GenerateNewSignal("LodgerPsychicRitualTarget_BadThought", true);
			questPart_RefugeeInteractions.outSignalLast_Destroyed = QuestGen.GenerateNewSignal("LastLodger_Destroyed", true);
			questPart_RefugeeInteractions.outSignalLast_Arrested = QuestGen.GenerateNewSignal("LastLodger_Arrested", true);
			questPart_RefugeeInteractions.outSignalLast_Kidnapped = QuestGen.GenerateNewSignal("LastLodger_Kidnapped", true);
			questPart_RefugeeInteractions.outSignalLast_Recruited = QuestGen.GenerateNewSignal("LastLodger_Recruited", true);
			questPart_RefugeeInteractions.outSignalLast_LeftMapAllHealthy = QuestGen.GenerateNewSignal("LastLodger_LeftMapAllHealthy", true);
			questPart_RefugeeInteractions.outSignalLast_LeftMapAllNotHealthy = QuestGen.GenerateNewSignal("LastLodger_LeftMapAllNotHealthy", true);
			questPart_RefugeeInteractions.outSignalLast_Banished = QuestGen.GenerateNewSignal("LastLodger_Banished", true);
			questPart_RefugeeInteractions.outSignalLast_LeftBehind = QuestGen.GenerateNewSignal("LastLodger_LeftBehind", true);
			questPart_RefugeeInteractions.pawns.AddRange(pawns);
			questPart_RefugeeInteractions.faction = faction;
			questPart_RefugeeInteractions.mapParent = map.Parent;
			questPart_RefugeeInteractions.signalListenMode = QuestPart.SignalListenMode.Always;
			quest.AddPart(questPart_RefugeeInteractions);
			Action <>9__13;
			quest.Delay(questDurationTicks, delegate
			{
				Quest quest6 = quest;
				Faction faction2 = faction;
				Action action = null;
				Action outAction;
				if ((outAction = <>9__13) == null)
				{
					outAction = (<>9__13 = delegate()
					{
						quest.Letter(LetterDefOf.PositiveEvent, null, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly, null, false, "[lodgersLeavingLetterText]", null, "[lodgersLeavingLetterLabel]", null, null);
					});
				}
				quest6.SignalPassWithFaction(faction2, action, outAction, null, null);
				quest.Leave(pawns, null, false, false, lodgerArrestedOrRecruited, true);
			}, null, null, null, false, null, null, false, "GuestsDepartsIn".Translate(), "GuestsDepartsOn".Translate(), "QuestDelay", false, QuestPart.SignalListenMode.OngoingOnly, false);
			quest.Letter(LetterDefOf.NegativeEvent, questPart_RefugeeInteractions.outSignalDestroyed_BadThought, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly, null, false, "[lodgerDiedMemoryThoughtLetterText]", null, "[lodgerDiedMemoryThoughtLetterLabel]", null, null);
			quest.Letter(LetterDefOf.NegativeEvent, questPart_RefugeeInteractions.outSignalDestroyed_AssaultColony, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly, null, false, "[lodgerDiedAttackPlayerLetterText]", null, "[lodgerDiedAttackPlayerLetterLabel]", null, null);
			quest.Letter(LetterDefOf.NegativeEvent, questPart_RefugeeInteractions.outSignalDestroyed_LeaveColony, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly, null, false, "[lodgerDiedLeaveMapLetterText]", null, "[lodgerDiedLeaveMapLetterLabel]", null, null);
			quest.Letter(LetterDefOf.NegativeEvent, questPart_RefugeeInteractions.outSignalLast_Destroyed, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly, null, false, "[lodgersAllDiedLetterText]", null, "[lodgersAllDiedLetterLabel]", null, null);
			quest.Letter(LetterDefOf.NegativeEvent, questPart_RefugeeInteractions.outSignalArrested_BadThought, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly, null, false, "[lodgerArrestedMemoryThoughtLetterText]", null, "[lodgerArrestedMemoryThoughtLetterLabel]", null, null);
			quest.Letter(LetterDefOf.NegativeEvent, questPart_RefugeeInteractions.outSignalArrested_AssaultColony, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly, null, false, "[lodgerArrestedAttackPlayerLetterText]", null, "[lodgerArrestedAttackPlayerLetterLabel]", null, null);
			quest.Letter(LetterDefOf.NegativeEvent, questPart_RefugeeInteractions.outSignalArrested_LeaveColony, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly, null, false, "[lodgerArrestedLeaveMapLetterText]", null, "[lodgerArrestedLeaveMapLetterLabel]", null, null);
			quest.Letter(LetterDefOf.NegativeEvent, questPart_RefugeeInteractions.outSignalLast_Arrested, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly, null, false, "[lodgersAllArrestedLetterText]", null, "[lodgersAllArrestedLetterLabel]", null, null);
			quest.Letter(LetterDefOf.NegativeEvent, questPart_RefugeeInteractions.outSignalLeftBehind_BadThought, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly, null, false, "[lodgerLeftBehindMemoryThoughtLetterText]", null, "[lodgerLeftBehindMemoryThoughtLetterLabel]", null, null);
			quest.Letter(LetterDefOf.NegativeEvent, questPart_RefugeeInteractions.outSignalLeftBehind_LeaveColony, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly, null, false, "[lodgerLeftBehindLeaveMapLetterText]", null, "[lodgerLeftBehindLeaveMapLetterLabel]", null, null);
			quest.Letter(LetterDefOf.NegativeEvent, questPart_RefugeeInteractions.outSignalSurgeryViolation_BadThought, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly, null, false, "[lodgerViolatedMemoryThoughtLetterText]", null, "[lodgerViolatedMemoryThoughtLetterLabel]", null, null);
			quest.Letter(LetterDefOf.NegativeEvent, questPart_RefugeeInteractions.outSignalSurgeryViolation_AssaultColony, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly, null, false, "[lodgerViolatedAttackPlayerLetterText]", null, "[lodgerViolatedAttackPlayerLetterLabel]", null, null);
			quest.Letter(LetterDefOf.NegativeEvent, questPart_RefugeeInteractions.outSignalSurgeryViolation_LeaveColony, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly, null, false, "[lodgerViolatedLeaveMapLetterText]", null, "[lodgerViolatedLeaveMapLetterLabel]", null, null);
			quest.Letter(LetterDefOf.NegativeEvent, questPart_RefugeeInteractions.outSignalPsychicRitualTarget_BadThought, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly, null, false, "[lodgerPsychicRitualTargetMemoryThoughtLetterText]", null, "[lodgerPsychicRitualTargetMemoryThoughtLetterLabel]", null, null);
			quest.Letter(LetterDefOf.NegativeEvent, questPart_RefugeeInteractions.outSignalPsychicRitualTarget_AssaultColony, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly, null, false, "[lodgerPsychicRitualTargetAttackPlayerLetterText]", null, "[lodgerPsychicRitualTargetAttackPlayerLetterLabel]", null, null);
			quest.Letter(LetterDefOf.NegativeEvent, questPart_RefugeeInteractions.outSignalPsychicRitualTarget_LeaveColony, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly, null, false, "[lodgerPsychicRitualTargetLeaveMapLetterText]", null, "[lodgerPsychicRitualTargetLeaveMapLetterLabel]", null, null);
			Quest quest2 = quest;
			IEnumerable<Pawn> pawns5 = pawns;
			ThoughtDef otherTravelerDied = ThoughtDefOf.OtherTravelerDied;
			string outSignalDestroyed_BadThought = questPart_RefugeeInteractions.outSignalDestroyed_BadThought;
			Pawn otherPawn = null;
			flag = null;
			quest2.AddMemoryThought(pawns5, otherTravelerDied, outSignalDestroyed_BadThought, otherPawn, flag);
			Quest quest3 = quest;
			IEnumerable<Pawn> pawns2 = pawns;
			ThoughtDef otherTravelerArrested = ThoughtDefOf.OtherTravelerArrested;
			string outSignalArrested_BadThought = questPart_RefugeeInteractions.outSignalArrested_BadThought;
			Pawn otherPawn2 = null;
			flag = null;
			quest3.AddMemoryThought(pawns2, otherTravelerArrested, outSignalArrested_BadThought, otherPawn2, flag);
			Quest quest4 = quest;
			IEnumerable<Pawn> pawns3 = pawns;
			ThoughtDef otherTravelerSurgicallyViolated = ThoughtDefOf.OtherTravelerSurgicallyViolated;
			string outSignalSurgeryViolation_BadThought = questPart_RefugeeInteractions.outSignalSurgeryViolation_BadThought;
			Pawn otherPawn3 = null;
			flag = null;
			quest4.AddMemoryThought(pawns3, otherTravelerSurgicallyViolated, outSignalSurgeryViolation_BadThought, otherPawn3, flag);
			Quest quest5 = quest;
			IEnumerable<Pawn> pawns4 = pawns;
			ThoughtDef otherTravelerLeftBehind = ThoughtDefOf.OtherTravelerLeftBehind;
			string outSignalLeftBehind_BadThought = questPart_RefugeeInteractions.outSignalLeftBehind_BadThought;
			Pawn otherPawn4 = null;
			flag = null;
			quest5.AddMemoryThought(pawns4, otherTravelerLeftBehind, outSignalLeftBehind_BadThought, otherPawn4, flag);
			quest.End(QuestEndOutcome.Fail, 0, null, questPart_RefugeeInteractions.outSignalDestroyed_AssaultColony, QuestPart.SignalListenMode.OngoingOnly, true, false);
			quest.End(QuestEndOutcome.Fail, 0, null, questPart_RefugeeInteractions.outSignalDestroyed_LeaveColony, QuestPart.SignalListenMode.OngoingOnly, true, false);
			quest.End(QuestEndOutcome.Fail, 0, null, questPart_RefugeeInteractions.outSignalLast_Destroyed, QuestPart.SignalListenMode.OngoingOnly, false, false);
			quest.End(QuestEndOutcome.Fail, 0, null, questPart_RefugeeInteractions.outSignalArrested_AssaultColony, QuestPart.SignalListenMode.OngoingOnly, true, false);
			quest.End(QuestEndOutcome.Fail, 0, null, questPart_RefugeeInteractions.outSignalArrested_LeaveColony, QuestPart.SignalListenMode.OngoingOnly, true, false);
			quest.End(QuestEndOutcome.Fail, 0, null, questPart_RefugeeInteractions.outSignalLast_Arrested, QuestPart.SignalListenMode.OngoingOnly, false, false);
			quest.End(QuestEndOutcome.Fail, 0, null, questPart_RefugeeInteractions.outSignalLeftBehind_LeaveColony, QuestPart.SignalListenMode.OngoingOnly, true, false);
			quest.End(QuestEndOutcome.Fail, 0, null, questPart_RefugeeInteractions.outSignalLast_LeftBehind, QuestPart.SignalListenMode.OngoingOnly, false, false);
			quest.End(QuestEndOutcome.Fail, 0, null, questPart_RefugeeInteractions.outSignalSurgeryViolation_AssaultColony, QuestPart.SignalListenMode.OngoingOnly, true, false);
			quest.End(QuestEndOutcome.Fail, 0, null, questPart_RefugeeInteractions.outSignalSurgeryViolation_LeaveColony, QuestPart.SignalListenMode.OngoingOnly, true, false);
			quest.End(QuestEndOutcome.Fail, 0, null, questPart_RefugeeInteractions.outSignalPsychicRitualTarget_AssaultColony, QuestPart.SignalListenMode.OngoingOnly, true, false);
			quest.End(QuestEndOutcome.Fail, 0, null, questPart_RefugeeInteractions.outSignalPsychicRitualTarget_LeaveColony, QuestPart.SignalListenMode.OngoingOnly, true, false);
			quest.End(QuestEndOutcome.Fail, 0, null, questPart_RefugeeInteractions.outSignalLast_Kidnapped, QuestPart.SignalListenMode.OngoingOnly, true, false);
			quest.End(QuestEndOutcome.Fail, 0, null, questPart_RefugeeInteractions.outSignalLast_Banished, QuestPart.SignalListenMode.OngoingOnly, true, false);
			quest.End(QuestEndOutcome.Success, 0, null, questPart_RefugeeInteractions.outSignalLast_Recruited, QuestPart.SignalListenMode.OngoingOnly, true, false);
			quest.End(QuestEndOutcome.Success, 0, null, questPart_RefugeeInteractions.outSignalLast_LeftMapAllNotHealthy, QuestPart.SignalListenMode.OngoingOnly, true, false);
			quest.SignalPass(delegate
			{
				if (Rand.Chance(0.5f))
				{
					float val = (float)(lodgerCount * questDurationDays) * 55f;
					FloatRange marketValueRange = new FloatRange(0.7f, 1.3f) * val * Find.Storyteller.difficulty.EffectiveQuestRewardValueFactor;
					quest.AddQuestRefugeeDelayedReward(quest.AccepterPawn, faction, pawns, marketValueRange, null);
				}
				quest.End(QuestEndOutcome.Success, 0, null, null, QuestPart.SignalListenMode.OngoingOnly, true, false);
			}, questPart_RefugeeInteractions.outSignalLast_LeftMapAllHealthy, null);
			slate.Set<int>("lodgerCount", lodgerCount, false);
			slate.Set<int>("lodgersCountMinusOne", lodgerCount - 1, false);
			slate.Set<Pawn>("asker", asker, false);
			slate.Set<Map>("map", map, false);
			slate.Set<int>("questDurationTicks", questDurationTicks, false);
			slate.Set<Faction>("faction", faction, false);
			slate.Set<int>("childCount", num, false);
			slate.Set<bool>("allButOneChildren", var, false);
		}

		// Token: 0x06014A42 RID: 84546 RVA: 0x0062B5F4 File Offset: 0x006297F4
		protected override bool TestRunInt(Slate slate)
		{
			return QuestGen_Get.GetMap(false, null, false) != null;
		}

		// Token: 0x0400E541 RID: 58689
		private static readonly QuestGen_Pawns.GetPawnParms FactionOpponentPawnParams = new QuestGen_Pawns.GetPawnParms
		{
			mustBeWorldPawn = true,
			mustBeFactionLeader = true,
			mustBeNonHostileToPlayer = true
		};

		// Token: 0x0400E542 RID: 58690
		private const float MidEventSelWeight_None = 0.5f;

		// Token: 0x0400E543 RID: 58691
		private const float MidEventSelWeight_Mutiny = 0.25f;

		// Token: 0x0400E544 RID: 58692
		private const float MidEventSelWeight_BetrayalOffer = 0.25f;

		// Token: 0x0400E545 RID: 58693
		private const float RewardPostLeaveChance = 0.5f;

		// Token: 0x0400E546 RID: 58694
		private const float RewardFactor_Postleave = 55f;

		// Token: 0x0400E547 RID: 58695
		public const float RewardFactor_BetrayalOffer = 300f;

		// Token: 0x0400E548 RID: 58696
		public const int BetrayalOfferGoodwillReward = 10;

		// Token: 0x0400E549 RID: 58697
		private static FloatRange BetrayalOfferTimeRange = new FloatRange(0.25f, 0.5f);

		// Token: 0x0400E54A RID: 58698
		private static FloatRange MutinyTimeRange = new FloatRange(0.2f, 1f);

		// Token: 0x0400E54B RID: 58699
		private static IntRange QuestDurationDaysRange = new IntRange(5, 20);
	}
}
