using System;
using Verse;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x02003AC2 RID: 15042
	public class QuestNode_EndGame : QuestNode
	{
		// Token: 0x06014834 RID: 84020 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x06014835 RID: 84021 RVA: 0x0061F354 File Offset: 0x0061D554
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_EndGame endGame = new QuestPart_EndGame();
			endGame.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? slate.Get<string>("inSignal", null, false));
			endGame.signalListenMode = this.signalListenMode.GetValue(slate).GetValueOrDefault();
			QuestGen.AddTextRequest("root", delegate(string x)
			{
				endGame.introText = x;
			}, QuestGenUtility.MergeRules(this.introTextRules.GetValue(slate), this.introText.GetValue(slate), "root"));
			QuestGen.AddTextRequest("root", delegate(string x)
			{
				endGame.endingText = x;
			}, QuestGenUtility.MergeRules(this.endingTextRules.GetValue(slate), this.endingText.GetValue(slate), "root"));
			QuestGen.quest.AddPart(endGame);
		}

		// Token: 0x0400E348 RID: 58184
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x0400E349 RID: 58185
		public SlateRef<QuestPart.SignalListenMode?> signalListenMode;

		// Token: 0x0400E34A RID: 58186
		public SlateRef<string> introText;

		// Token: 0x0400E34B RID: 58187
		public SlateRef<string> endingText;

		// Token: 0x0400E34C RID: 58188
		public SlateRef<RulePack> introTextRules;

		// Token: 0x0400E34D RID: 58189
		public SlateRef<RulePack> endingTextRules;

		// Token: 0x0400E34E RID: 58190
		private const string RootSymbol = "root";
	}
}
