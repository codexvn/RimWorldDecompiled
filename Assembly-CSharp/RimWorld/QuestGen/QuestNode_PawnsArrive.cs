using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x02003ADF RID: 15071
	public class QuestNode_PawnsArrive : QuestNode
	{
		// Token: 0x06014893 RID: 84115 RVA: 0x00620959 File Offset: 0x0061EB59
		protected override bool TestRunInt(Slate slate)
		{
			return slate.Exists("map", false);
		}

		// Token: 0x06014894 RID: 84116 RVA: 0x0062096C File Offset: 0x0061EB6C
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			PawnsArrivalModeDef pawnsArrivalModeDef = this.arrivalMode.GetValue(slate) ?? PawnsArrivalModeDefOf.EdgeWalkIn;
			QuestPart_PawnsArrive pawnsArrive = new QuestPart_PawnsArrive();
			pawnsArrive.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			pawnsArrive.pawns.AddRange(this.pawns.GetValue(slate));
			pawnsArrive.arrivalMode = pawnsArrivalModeDef;
			pawnsArrive.joinPlayer = this.joinPlayer.GetValue(slate);
			pawnsArrive.mapParent = QuestGen.slate.Get<Map>("map", null, false).Parent;
			pawnsArrive.customLetterDef = this.customLetterDef.GetValue(slate);
			if (pawnsArrivalModeDef.walkIn)
			{
				pawnsArrive.spawnNear = (this.walkInSpot.GetValue(slate) ?? (QuestGen.slate.Get<IntVec3?>("walkInSpot", null, false) ?? IntVec3.Invalid));
			}
			if (!this.customLetterLabel.GetValue(slate).NullOrEmpty() || this.customLetterLabelRules.GetValue(slate) != null)
			{
				QuestGen.AddTextRequest("root", delegate(string x)
				{
					pawnsArrive.customLetterLabel = x;
				}, QuestGenUtility.MergeRules(this.customLetterLabelRules.GetValue(slate), this.customLetterLabel.GetValue(slate), "root"));
			}
			if (!this.customLetterText.GetValue(slate).NullOrEmpty() || this.customLetterTextRules.GetValue(slate) != null)
			{
				QuestGen.AddTextRequest("root", delegate(string x)
				{
					pawnsArrive.customLetterText = x;
				}, QuestGenUtility.MergeRules(this.customLetterTextRules.GetValue(slate), this.customLetterText.GetValue(slate), "root"));
			}
			QuestGen.quest.AddPart(pawnsArrive);
			if (this.isSingleReward.GetValue(slate))
			{
				QuestPart_Choice questPart_Choice = new QuestPart_Choice();
				questPart_Choice.inSignalChoiceUsed = pawnsArrive.inSignal;
				QuestPart_Choice.Choice choice = new QuestPart_Choice.Choice();
				choice.questParts.Add(pawnsArrive);
				foreach (Pawn pawn in pawnsArrive.pawns)
				{
					choice.rewards.Add(new Reward_Pawn
					{
						pawn = pawn,
						detailsHidden = this.rewardDetailsHidden.GetValue(slate)
					});
				}
				questPart_Choice.choices.Add(choice);
				QuestGen.quest.AddPart(questPart_Choice);
			}
		}

		// Token: 0x0400E3D4 RID: 58324
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x0400E3D5 RID: 58325
		public SlateRef<IEnumerable<Pawn>> pawns;

		// Token: 0x0400E3D6 RID: 58326
		public SlateRef<PawnsArrivalModeDef> arrivalMode;

		// Token: 0x0400E3D7 RID: 58327
		public SlateRef<bool> joinPlayer;

		// Token: 0x0400E3D8 RID: 58328
		public SlateRef<IntVec3?> walkInSpot;

		// Token: 0x0400E3D9 RID: 58329
		public SlateRef<string> customLetterLabel;

		// Token: 0x0400E3DA RID: 58330
		public SlateRef<string> customLetterText;

		// Token: 0x0400E3DB RID: 58331
		public SlateRef<RulePack> customLetterLabelRules;

		// Token: 0x0400E3DC RID: 58332
		public SlateRef<RulePack> customLetterTextRules;

		// Token: 0x0400E3DD RID: 58333
		public SlateRef<LetterDef> customLetterDef;

		// Token: 0x0400E3DE RID: 58334
		public SlateRef<bool> isSingleReward;

		// Token: 0x0400E3DF RID: 58335
		public SlateRef<bool> rewardDetailsHidden;

		// Token: 0x0400E3E0 RID: 58336
		private const string RootSymbol = "root";
	}
}
