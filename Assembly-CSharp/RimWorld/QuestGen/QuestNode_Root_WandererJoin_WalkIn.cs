using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003BC4 RID: 15300
	public class QuestNode_Root_WandererJoin_WalkIn : QuestNode_Root_WandererJoin
	{
		// Token: 0x06014C76 RID: 85110 RVA: 0x00639638 File Offset: 0x00637838
		protected override void RunInt()
		{
			base.RunInt();
			Quest quest = QuestGen.quest;
			quest.Delay(60000, delegate
			{
				quest.End(QuestEndOutcome.Fail, 0, null, null, QuestPart.SignalListenMode.OngoingOnly, false, false);
			}, null, null, null, false, null, null, false, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly, false);
		}

		// Token: 0x06014C77 RID: 85111 RVA: 0x00639688 File Offset: 0x00637888
		public override Pawn GeneratePawn()
		{
			Slate slate = QuestGen.slate;
			Gender? gender = null;
			PawnGenerationRequest request;
			if (!slate.TryGet<PawnGenerationRequest>("overridePawnGenParams", out request, false))
			{
				PawnKindDef villager = PawnKindDefOf.Villager;
				Faction faction = null;
				PawnGenerationContext context = PawnGenerationContext.NonPlayer;
				Gender? fixedGender = gender;
				request = new PawnGenerationRequest(villager, faction, context, null, true, false, false, true, false, 20f, false, true, true, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, fixedGender, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, true, false, false, -1, 0, false);
			}
			if (Find.Storyteller.difficulty.ChildrenAllowed)
			{
				request.AllowedDevelopmentalStages |= DevelopmentalStage.Child;
			}
			Pawn pawn = PawnGenerator.GeneratePawn(request);
			if (!pawn.IsWorldPawn())
			{
				Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Decide);
			}
			return pawn;
		}

		// Token: 0x06014C78 RID: 85112 RVA: 0x00639780 File Offset: 0x00637980
		protected override void AddSpawnPawnQuestParts(Quest quest, Map map, Pawn pawn)
		{
			this.signalAccept = QuestGenUtility.HardcodedSignalWithQuestID("Accept");
			this.signalReject = QuestGenUtility.HardcodedSignalWithQuestID("Reject");
			quest.Signal(this.signalAccept, delegate
			{
				quest.SetFaction(Gen.YieldSingle<Pawn>(pawn), Faction.OfPlayer, null);
				quest.PawnsArrive(Gen.YieldSingle<Pawn>(pawn), null, map.Parent, null, false, null, null, null, null, null, false, false, true);
				quest.End(QuestEndOutcome.Success, 0, null, null, QuestPart.SignalListenMode.OngoingOnly, false, false);
			}, null, QuestPart.SignalListenMode.OngoingOnly);
			quest.Signal(this.signalReject, delegate
			{
				quest.GiveDiedOrDownedThoughts(pawn, PawnDiedOrDownedThoughtsKind.DeniedJoining, null);
				quest.End(QuestEndOutcome.Fail, 0, null, null, QuestPart.SignalListenMode.OngoingOnly, false, false);
			}, null, QuestPart.SignalListenMode.OngoingOnly);
		}

		// Token: 0x06014C79 RID: 85113 RVA: 0x00639808 File Offset: 0x00637A08
		public override void SendLetter(Quest quest, Pawn pawn)
		{
			TaggedString label = "LetterLabelWandererJoins".Translate(pawn.Named("PAWN")).AdjustedFor(pawn, "PAWN", true);
			TaggedString taggedString = "LetterWandererJoins".Translate(pawn.Named("PAWN")).AdjustedFor(pawn, "PAWN", true);
			QuestNode_Root_WandererJoin_WalkIn.AppendCharityInfoToLetter("JoinerCharityInfo".Translate(pawn), ref taggedString);
			PawnRelationUtility.TryAppendRelationsWithColonistsInfo(ref taggedString, ref label, pawn);
			if (pawn.DevelopmentalStage.Juvenile())
			{
				string arg = (pawn.ageTracker.AgeBiologicalYears * 3600000).ToStringTicksToPeriod(true, false, true, true, false);
				taggedString += "\n\n" + "RefugeePodCrash_Child".Translate(pawn.Named("PAWN"), arg.Named("AGE"));
			}
			QuestNode_Root_WandererJoin_WalkIn.ApplyBestSkillInfoToLetter(ref taggedString, pawn);
			ChoiceLetter_AcceptJoiner choiceLetter_AcceptJoiner = (ChoiceLetter_AcceptJoiner)LetterMaker.MakeLetter(label, taggedString, LetterDefOf.AcceptJoiner, null, null);
			choiceLetter_AcceptJoiner.signalAccept = this.signalAccept;
			choiceLetter_AcceptJoiner.signalReject = this.signalReject;
			choiceLetter_AcceptJoiner.quest = quest;
			choiceLetter_AcceptJoiner.StartTimeout(60000);
			Find.LetterStack.ReceiveLetter(choiceLetter_AcceptJoiner, null, 0, true);
		}

		// Token: 0x06014C7A RID: 85114 RVA: 0x00639934 File Offset: 0x00637B34
		public static void AppendCharityInfoToLetter(TaggedString charityInfo, ref TaggedString letterText)
		{
			if (ModsConfig.IdeologyActive)
			{
				IEnumerable<Pawn> source = IdeoUtility.AllColonistsWithCharityPrecept();
				if (source.Any<Pawn>())
				{
					letterText += "\n\n" + charityInfo + "\n\n" + "PawnsHaveCharitableBeliefs".Translate() + ":";
					foreach (IGrouping<Ideo, Pawn> grouping in from c in source
					group c by c.Ideo)
					{
						letterText += "\n  - " + "BelieversIn".Translate(grouping.Key.name.Colorize(grouping.Key.TextColor), grouping.Select((Pawn f) => f.NameShortColored.Resolve()).ToCommaList(false, false));
					}
				}
			}
		}

		// Token: 0x06014C7B RID: 85115 RVA: 0x00639A6C File Offset: 0x00637C6C
		public static void ApplyBestSkillInfoToLetter(ref TaggedString letterText, Pawn pawn)
		{
			if (pawn.skills == null || !pawn.skills.skills.Any<SkillRecord>())
			{
				return;
			}
			SkillRecord skillRecord = pawn.skills.skills.MaxBy((SkillRecord s) => s.Level);
			if (skillRecord == null)
			{
				return;
			}
			letterText += "\n\n" + "BestSkillLetterLabel".Translate(pawn.Named("PAWN")) + ": " + skillRecord.def.LabelCap + " (" + "BestSkillInfoLevel".Translate(skillRecord.Level) + ")";
		}

		// Token: 0x0400E743 RID: 59203
		private const int TimeoutTicks = 60000;

		// Token: 0x0400E744 RID: 59204
		public const float RelationWithColonistWeight = 20f;

		// Token: 0x0400E745 RID: 59205
		private string signalAccept;

		// Token: 0x0400E746 RID: 59206
		private string signalReject;
	}
}
