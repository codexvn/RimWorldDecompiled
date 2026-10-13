using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x02003AC9 RID: 15049
	public class QuestNode_GiveNearPawn : QuestNode
	{
		// Token: 0x06014848 RID: 84040 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x06014849 RID: 84041 RVA: 0x0061F760 File Offset: 0x0061D960
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.contents.GetValue(slate).EnumerableNullOrEmpty<Thing>() && this.contentsDefs.GetValue(slate).EnumerableNullOrEmpty<ThingDefCountClass>())
			{
				return;
			}
			QuestPart_GiveNearPawn give = new QuestPart_GiveNearPawn();
			give.nearPawn = this.nearPawn.GetValue(slate);
			give.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? slate.Get<string>("inSignal", null, false));
			if (!this.customDropPodsLetterLabel.GetValue(slate).NullOrEmpty() || this.customDropPodsLetterLabelRules.GetValue(slate) != null)
			{
				QuestGen.AddTextRequest("root", delegate(string x)
				{
					give.customDropPodsLetterLabel = x;
				}, QuestGenUtility.MergeRules(this.customDropPodsLetterLabelRules.GetValue(slate), this.customDropPodsLetterLabel.GetValue(slate), "root"));
			}
			if (!this.customDropPodsLetterText.GetValue(slate).NullOrEmpty() || this.customDropPodsLetterTextRules.GetValue(slate) != null)
			{
				QuestGen.AddTextRequest("root", delegate(string x)
				{
					give.customDropPodsLetterText = x;
				}, QuestGenUtility.MergeRules(this.customDropPodsLetterTextRules.GetValue(slate), this.customDropPodsLetterText.GetValue(slate), "root"));
			}
			if (!this.customCaravanInventoryLetterLabel.GetValue(slate).NullOrEmpty() || this.customCaravanInventoryLetterLabelRules.GetValue(slate) != null)
			{
				QuestGen.AddTextRequest("root", delegate(string x)
				{
					give.customCaravanInventoryLetterLabel = x;
				}, QuestGenUtility.MergeRules(this.customCaravanInventoryLetterLabelRules.GetValue(slate), this.customCaravanInventoryLetterLabel.GetValue(slate), "root"));
			}
			if (!this.customCaravanInventoryLetterText.GetValue(slate).NullOrEmpty() || this.customCaravanInventoryLetterTextRules.GetValue(slate) != null)
			{
				QuestGen.AddTextRequest("root", delegate(string x)
				{
					give.customCaravanInventoryLetterText = x;
				}, QuestGenUtility.MergeRules(this.customCaravanInventoryLetterTextRules.GetValue(slate), this.customCaravanInventoryLetterText.GetValue(slate), "root"));
			}
			give.sendStandardLetter = (this.sendStandardLetter.GetValue(slate) ?? give.sendStandardLetter);
			give.joinPlayer = this.joinPlayer.GetValue(slate);
			give.makePrisoners = this.makePrisoners.GetValue(slate);
			give.Things = this.contents.GetValue(slate);
			if (this.contentsDefs.GetValue(slate) != null)
			{
				give.thingDefs.AddRange(this.contentsDefs.GetValue(slate));
			}
			QuestGen.quest.AddPart(give);
		}

		// Token: 0x0400E365 RID: 58213
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x0400E366 RID: 58214
		public SlateRef<IEnumerable<Thing>> contents;

		// Token: 0x0400E367 RID: 58215
		public SlateRef<IEnumerable<ThingDefCountClass>> contentsDefs;

		// Token: 0x0400E368 RID: 58216
		public SlateRef<Pawn> nearPawn;

		// Token: 0x0400E369 RID: 58217
		public SlateRef<bool> joinPlayer;

		// Token: 0x0400E36A RID: 58218
		public SlateRef<bool> makePrisoners;

		// Token: 0x0400E36B RID: 58219
		public SlateRef<bool?> sendStandardLetter;

		// Token: 0x0400E36C RID: 58220
		public SlateRef<string> customDropPodsLetterLabel;

		// Token: 0x0400E36D RID: 58221
		public SlateRef<string> customDropPodsLetterText;

		// Token: 0x0400E36E RID: 58222
		public SlateRef<string> customCaravanInventoryLetterLabel;

		// Token: 0x0400E36F RID: 58223
		public SlateRef<string> customCaravanInventoryLetterText;

		// Token: 0x0400E370 RID: 58224
		public SlateRef<RulePack> customDropPodsLetterLabelRules;

		// Token: 0x0400E371 RID: 58225
		public SlateRef<RulePack> customDropPodsLetterTextRules;

		// Token: 0x0400E372 RID: 58226
		public SlateRef<RulePack> customCaravanInventoryLetterLabelRules;

		// Token: 0x0400E373 RID: 58227
		public SlateRef<RulePack> customCaravanInventoryLetterTextRules;

		// Token: 0x0400E374 RID: 58228
		private const string RootSymbol = "root";
	}
}
