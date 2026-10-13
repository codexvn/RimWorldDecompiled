using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x02003ABF RID: 15039
	public class QuestNode_DropPods : QuestNode
	{
		// Token: 0x0601482B RID: 84011 RVA: 0x0061EFF5 File Offset: 0x0061D1F5
		protected override bool TestRunInt(Slate slate)
		{
			return slate.Exists("map", false);
		}

		// Token: 0x0601482C RID: 84012 RVA: 0x0061F094 File Offset: 0x0061D294
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.contents.GetValue(slate).EnumerableNullOrEmpty<Thing>() && this.contentsDefs.GetValue(slate).EnumerableNullOrEmpty<ThingDefCountClass>())
			{
				return;
			}
			QuestPart_DropPods dropPods = new QuestPart_DropPods();
			dropPods.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			if (!this.customLetterLabel.GetValue(slate).NullOrEmpty() || this.customLetterLabelRules.GetValue(slate) != null)
			{
				QuestGen.AddTextRequest("root", delegate(string x)
				{
					dropPods.customLetterLabel = x;
				}, QuestGenUtility.MergeRules(this.customLetterLabelRules.GetValue(slate), this.customLetterLabel.GetValue(slate), "root"));
			}
			if (!this.customLetterText.GetValue(slate).NullOrEmpty() || this.customLetterTextRules.GetValue(slate) != null)
			{
				QuestGen.AddTextRequest("root", delegate(string x)
				{
					dropPods.customLetterText = x;
				}, QuestGenUtility.MergeRules(this.customLetterTextRules.GetValue(slate), this.customLetterText.GetValue(slate), "root"));
			}
			dropPods.sendStandardLetter = (this.sendStandardLetter.GetValue(slate) ?? dropPods.sendStandardLetter);
			dropPods.useTradeDropSpot = this.useTradeDropSpot.GetValue(slate);
			dropPods.dropSpot = (this.dropSpot.GetValue(slate) ?? IntVec3.Invalid);
			dropPods.joinPlayer = this.joinPlayer.GetValue(slate);
			dropPods.makePrisoners = this.makePrisoners.GetValue(slate);
			dropPods.mapParent = QuestGen.slate.Get<Map>("map", null, false).Parent;
			dropPods.Things = this.contents.GetValue(slate);
			if (this.contentsDefs.GetValue(slate) != null)
			{
				dropPods.thingDefs.AddRange(this.contentsDefs.GetValue(slate));
			}
			if (this.thingsToExcludeFromHyperlinks.GetValue(slate) != null)
			{
				dropPods.thingsToExcludeFromHyperlinks.AddRange(from t in this.thingsToExcludeFromHyperlinks.GetValue(slate)
				select t.GetInnerIfMinified().def);
			}
			QuestGen.quest.AddPart(dropPods);
		}

		// Token: 0x0400E337 RID: 58167
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x0400E338 RID: 58168
		public SlateRef<IEnumerable<Thing>> contents;

		// Token: 0x0400E339 RID: 58169
		public SlateRef<IEnumerable<ThingDefCountClass>> contentsDefs;

		// Token: 0x0400E33A RID: 58170
		public SlateRef<IntVec3?> dropSpot;

		// Token: 0x0400E33B RID: 58171
		public SlateRef<bool> useTradeDropSpot;

		// Token: 0x0400E33C RID: 58172
		public SlateRef<bool> joinPlayer;

		// Token: 0x0400E33D RID: 58173
		public SlateRef<bool> makePrisoners;

		// Token: 0x0400E33E RID: 58174
		public SlateRef<bool?> sendStandardLetter;

		// Token: 0x0400E33F RID: 58175
		public SlateRef<string> customLetterLabel;

		// Token: 0x0400E340 RID: 58176
		public SlateRef<string> customLetterText;

		// Token: 0x0400E341 RID: 58177
		public SlateRef<RulePack> customLetterLabelRules;

		// Token: 0x0400E342 RID: 58178
		public SlateRef<RulePack> customLetterTextRules;

		// Token: 0x0400E343 RID: 58179
		public SlateRef<IEnumerable<Thing>> thingsToExcludeFromHyperlinks;

		// Token: 0x0400E344 RID: 58180
		private const string RootSymbol = "root";
	}
}
