using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003B3D RID: 15165
	public class QuestNode_Root_Creepjoiner_Arrival : QuestNode
	{
		// Token: 0x060149E9 RID: 84457 RVA: 0x006280E8 File Offset: 0x006262E8
		protected override void RunInt()
		{
			Quest quest = QuestGen.quest;
			Slate slate = QuestGen.slate;
			Map map = QuestGen_Get.GetMap(false, null, false);
			Pawn pawn = this.SpawnPawn(map, quest.points);
			Pawn_CreepJoinerTracker creepjoiner = pawn.creepjoiner;
			slate.Set<Pawn>("pawn", pawn, false);
			this.SendLetter(pawn);
			string text = QuestGenUtility.HardcodedSignalWithQuestID("Accept");
			string text2 = QuestGenUtility.HardcodedSignalWithQuestID("Reject");
			string text3 = QuestGenUtility.HardcodedSignalWithQuestID("Capture");
			string text4 = QuestGenUtility.HardcodedSignalWithQuestID("SpokeTo");
			string text5 = QuestGenUtility.HardcodedSignalWithQuestID("Timeout");
			string inSignal = QuestGenUtility.HardcodedSignalWithQuestID("pawn.Killed");
			string text6 = QuestGenUtility.HardcodedSignalWithQuestID("pawn.TookDamageFromPlayer");
			string inSignal2 = QuestGenUtility.HardcodedSignalWithQuestID("pawn.LeftMap");
			string inSignal3 = QuestGenUtility.HardcodedSignalWithQuestID("pawn.Recruited");
			string inSignal4 = QuestGenUtility.HardcodedSignalWithQuestID("pawn.Despawned");
			creepjoiner.quest = quest;
			creepjoiner.spokeToSignal = text4;
			creepjoiner.timeoutAt = GenTicks.TicksAbs + 60000;
			QuestPart_CreepJoinerOutcomes part = new QuestPart_CreepJoinerOutcomes
			{
				pawn = pawn,
				timeout = 60000,
				signalAccept = text,
				signalReject = text2,
				signalCapture = text3,
				signalAttacked = text6,
				signalShow = text4,
				signalTimeout = text5,
				signalListenMode = QuestPart.SignalListenMode.Always
			};
			quest.AddPart(part);
			quest.Signal(text, delegate
			{
				quest.SetFaction(Gen.YieldSingle<Pawn>(pawn), Faction.OfPlayer, null);
				quest.End(QuestEndOutcome.Success, 0, null, null, QuestPart.SignalListenMode.OngoingOnly, false, false);
			}, null, QuestPart.SignalListenMode.OngoingOnly);
			quest.Signal(text2, delegate
			{
				quest.GiveDiedOrDownedThoughts(pawn, PawnDiedOrDownedThoughtsKind.DeniedJoining, null);
				quest.End(QuestEndOutcome.Fail, 0, null, null, QuestPart.SignalListenMode.OngoingOnly, false, false);
			}, null, QuestPart.SignalListenMode.OngoingOnly);
			quest.End(QuestEndOutcome.Unknown, 0, null, text3, QuestPart.SignalListenMode.OngoingOnly, false, false);
			quest.End(QuestEndOutcome.Unknown, 0, null, text6, QuestPart.SignalListenMode.OngoingOnly, false, false);
			quest.End(QuestEndOutcome.Unknown, 0, null, inSignal, QuestPart.SignalListenMode.OngoingOnly, false, false);
			quest.End(QuestEndOutcome.Unknown, 0, null, inSignal2, QuestPart.SignalListenMode.OngoingOnly, false, false);
			quest.End(QuestEndOutcome.Unknown, 0, null, inSignal3, QuestPart.SignalListenMode.OngoingOnly, false, false);
			quest.End(QuestEndOutcome.Unknown, 0, null, inSignal4, QuestPart.SignalListenMode.OngoingOnly, false, false);
			quest.Delay(60000, delegate
			{
				quest.End(QuestEndOutcome.Fail, 0, null, null, QuestPart.SignalListenMode.OngoingOnly, false, false);
			}, null, null, text5, false, null, null, false, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly, false);
		}

		// Token: 0x060149EA RID: 84458 RVA: 0x00628334 File Offset: 0x00626534
		protected override bool TestRunInt(Slate slate)
		{
			return QuestGen_Get.GetMap(false, null, false) != null;
		}

		// Token: 0x060149EB RID: 84459 RVA: 0x00628354 File Offset: 0x00626554
		private Pawn SpawnPawn(Map map, float combatPoints = 0f)
		{
			Slate slate = QuestGen.slate;
			CreepJoinerFormKindDef form;
			CreepJoinerBenefitDef benefit;
			CreepJoinerDownsideDef downside;
			CreepJoinerAggressiveDef aggressive;
			CreepJoinerRejectionDef rejection;
			if (slate.TryGet<CreepJoinerFormKindDef>("form", out form, false) && slate.TryGet<CreepJoinerBenefitDef>("benefit", out benefit, false) && slate.TryGet<CreepJoinerDownsideDef>("downside", out downside, false) && slate.TryGet<CreepJoinerAggressiveDef>("aggressive", out aggressive, false) && slate.TryGet<CreepJoinerRejectionDef>("rejection", out rejection, false))
			{
				return CreepJoinerUtility.GenerateAndSpawn(form, benefit, downside, aggressive, rejection, QuestGen_Get.GetMap(false, null, false));
			}
			return CreepJoinerUtility.GenerateAndSpawn(map, combatPoints);
		}

		// Token: 0x060149EC RID: 84460 RVA: 0x006283DC File Offset: 0x006265DC
		private void SendLetter(Pawn pawn)
		{
			string str = pawn.GetKindLabelSingular().CapitalizeFirst();
			TaggedString taggedString = pawn.creepjoiner.form.letterLabel.Formatted(pawn.Named("PAWN"));
			taggedString += "\n\n" + "LetterCreeperAppearedAppended".Translate(pawn.Named("PAWN")).CapitalizeFirst();
			ChoiceLetter let = LetterMaker.MakeLetter(str, taggedString, LetterDefOf.NeutralEvent, pawn, null, null, null);
			Find.LetterStack.ReceiveLetter(let, null, 0, true);
		}

		// Token: 0x0400E4E9 RID: 58601
		private const int TimeoutTicks = 60000;
	}
}
