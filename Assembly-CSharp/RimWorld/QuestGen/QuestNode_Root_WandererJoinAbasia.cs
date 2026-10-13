using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003BC1 RID: 15297
	public class QuestNode_Root_WandererJoinAbasia : QuestNode_Root_WandererJoin
	{
		// Token: 0x06014C6C RID: 85100 RVA: 0x00639320 File Offset: 0x00637520
		protected override void RunInt()
		{
			base.RunInt();
			Quest quest = QuestGen.quest;
			quest.Delay(60000, delegate
			{
				quest.End(QuestEndOutcome.Fail, 0, null, null, QuestPart.SignalListenMode.OngoingOnly, false, false);
			}, null, null, null, false, null, null, false, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly, false);
		}

		// Token: 0x06014C6D RID: 85101 RVA: 0x00639370 File Offset: 0x00637570
		public override Pawn GeneratePawn()
		{
			PawnGenerationRequest request = new PawnGenerationRequest(PawnKindDefOf.Refugee, null, PawnGenerationContext.NonPlayer, null, true, false, false, true, false, 20f, false, true, true, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, true, false, false, -1, 0, false);
			if (Find.Storyteller.difficulty.ChildrenAllowed)
			{
				request.AllowedDevelopmentalStages |= DevelopmentalStage.Child;
			}
			Pawn pawn = PawnGenerator.GeneratePawn(request);
			pawn.health.AddHediff(HediffDefOf.Abasia, null, null, null);
			if (!pawn.IsWorldPawn())
			{
				Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Decide);
			}
			return pawn;
		}

		// Token: 0x06014C6E RID: 85102 RVA: 0x00639468 File Offset: 0x00637668
		protected override void AddSpawnPawnQuestParts(Quest quest, Map map, Pawn pawn)
		{
			base.AddSpawnPawnQuestParts(quest, map, pawn);
			this.signalAccept = QuestGenUtility.HardcodedSignalWithQuestID("Accept");
			this.signalReject = QuestGenUtility.HardcodedSignalWithQuestID("Reject");
			quest.Signal(this.signalAccept, delegate
			{
				quest.SetFaction(Gen.YieldSingle<Pawn>(pawn), Faction.OfPlayer, null);
				quest.End(QuestEndOutcome.Success, 0, null, null, QuestPart.SignalListenMode.OngoingOnly, false, false);
			}, null, QuestPart.SignalListenMode.OngoingOnly);
			quest.Signal(this.signalReject, delegate
			{
				quest.GiveDiedOrDownedThoughts(pawn, PawnDiedOrDownedThoughtsKind.DeniedJoining, null);
				quest.End(QuestEndOutcome.Fail, 0, null, null, QuestPart.SignalListenMode.OngoingOnly, false, false);
			}, null, QuestPart.SignalListenMode.OngoingOnly);
		}

		// Token: 0x06014C6F RID: 85103 RVA: 0x006394FC File Offset: 0x006376FC
		public override void SendLetter(Quest quest, Pawn pawn)
		{
			TaggedString label = "LetterLabelWandererJoinsAbasia".Translate(pawn);
			TaggedString text = "LetterTextWandererJoinsAbasia".Translate(pawn, HediffDefOf.Abasia);
			QuestNode_Root_WandererJoin_WalkIn.AppendCharityInfoToLetter("AfflictedJoinerChartityInfo".Translate(pawn, this.AllowKilledBeforeTicks.ToStringTicksToPeriod(true, false, true, true, false)), ref text);
			PawnRelationUtility.TryAppendRelationsWithColonistsInfo(ref text, ref label, pawn);
			QuestNode_Root_WandererJoin_WalkIn.ApplyBestSkillInfoToLetter(ref text, pawn);
			ChoiceLetter_AcceptJoiner choiceLetter_AcceptJoiner = (ChoiceLetter_AcceptJoiner)LetterMaker.MakeLetter(label, text, LetterDefOf.AcceptJoiner, pawn, null, null, null);
			choiceLetter_AcceptJoiner.signalAccept = this.signalAccept;
			choiceLetter_AcceptJoiner.signalReject = this.signalReject;
			choiceLetter_AcceptJoiner.quest = quest;
			choiceLetter_AcceptJoiner.StartTimeout(60000);
			Find.LetterStack.ReceiveLetter(choiceLetter_AcceptJoiner, null, 0, true);
		}

		// Token: 0x0400E73C RID: 59196
		private const int TimeoutTicks = 60000;

		// Token: 0x0400E73D RID: 59197
		private const float RelationWithColonistWeight = 20f;

		// Token: 0x0400E73E RID: 59198
		private string signalAccept;

		// Token: 0x0400E73F RID: 59199
		private string signalReject;
	}
}
