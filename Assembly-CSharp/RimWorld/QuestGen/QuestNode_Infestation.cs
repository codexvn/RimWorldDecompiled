using System;
using Verse;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x02003BF3 RID: 15347
	public class QuestNode_Infestation : QuestNode
	{
		// Token: 0x06014D1F RID: 85279 RVA: 0x0063C7D4 File Offset: 0x0063A9D4
		protected override bool TestRunInt(Slate slate)
		{
			if (!Find.Storyteller.difficulty.allowViolentQuests)
			{
				return false;
			}
			if (!slate.Exists("map", false))
			{
				return false;
			}
			if (Faction.OfInsects == null)
			{
				return false;
			}
			Map map = slate.Get<Map>("map", null, false);
			IntVec3 intVec;
			return InfestationCellFinder.TryFindCell(out intVec, map);
		}

		// Token: 0x06014D20 RID: 85280 RVA: 0x0063C828 File Offset: 0x0063AA28
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			Map map = QuestGen.slate.Get<Map>("map", null, false);
			if (map == null)
			{
				return;
			}
			QuestPart_Infestation questPart = new QuestPart_Infestation();
			questPart.mapParent = map.Parent;
			questPart.hivesCount = this.hivesCount.GetValue(slate);
			questPart.tag = QuestGenUtility.HardcodedTargetQuestTagWithQuestID(this.tag.GetValue(slate));
			if (!this.customLetterLabel.GetValue(slate).NullOrEmpty() || this.customLetterLabelRules.GetValue(slate) != null)
			{
				QuestGen.AddTextRequest("root", delegate(string x)
				{
					questPart.customLetterLabel = x;
				}, QuestGenUtility.MergeRules(this.customLetterLabelRules.GetValue(slate), this.customLetterLabel.GetValue(slate), "root"));
			}
			if (!this.customLetterText.GetValue(slate).NullOrEmpty() || this.customLetterTextRules.GetValue(slate) != null)
			{
				QuestGen.AddTextRequest("root", delegate(string x)
				{
					questPart.customLetterText = x;
				}, QuestGenUtility.MergeRules(this.customLetterTextRules.GetValue(slate), this.customLetterText.GetValue(slate), "root"));
			}
			questPart.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			QuestGen.quest.AddPart(questPart);
		}

		// Token: 0x0400E7D9 RID: 59353
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x0400E7DA RID: 59354
		public SlateRef<int> hivesCount;

		// Token: 0x0400E7DB RID: 59355
		public SlateRef<string> tag;

		// Token: 0x0400E7DC RID: 59356
		public SlateRef<string> customLetterLabel;

		// Token: 0x0400E7DD RID: 59357
		public SlateRef<string> customLetterText;

		// Token: 0x0400E7DE RID: 59358
		public SlateRef<RulePack> customLetterLabelRules;

		// Token: 0x0400E7DF RID: 59359
		public SlateRef<RulePack> customLetterTextRules;

		// Token: 0x0400E7E0 RID: 59360
		private const string RootSymbol = "root";
	}
}
