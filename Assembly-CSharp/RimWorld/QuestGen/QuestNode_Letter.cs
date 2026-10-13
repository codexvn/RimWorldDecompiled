using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x02003AD6 RID: 15062
	public class QuestNode_Letter : QuestNode
	{
		// Token: 0x06014879 RID: 84089 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0601487A RID: 84090 RVA: 0x006202F8 File Offset: 0x0061E4F8
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_Letter questPart_Letter = new QuestPart_Letter();
			questPart_Letter.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? slate.Get<string>("inSignal", null, false));
			LetterDef letterDef = this.letterDef.GetValue(slate) ?? LetterDefOf.NeutralEvent;
			if (typeof(ChoiceLetter).IsAssignableFrom(letterDef.letterClass))
			{
				ChoiceLetter choiceLetter = LetterMaker.MakeLetter("error", "error", letterDef, QuestGenUtility.ToLookTargets(this.lookTargets, slate), this.relatedFaction.GetValue(slate), QuestGen.quest, null);
				questPart_Letter.letter = choiceLetter;
				QuestGen.AddTextRequest("root", delegate(string x)
				{
					choiceLetter.Label = x;
				}, QuestGenUtility.MergeRules(this.labelRules.GetValue(slate), this.label.GetValue(slate), "root"));
				QuestGen.AddTextRequest("root", delegate(string x)
				{
					choiceLetter.Text = x;
				}, QuestGenUtility.MergeRules(this.textRules.GetValue(slate), this.text.GetValue(slate), "root"));
			}
			else
			{
				questPart_Letter.letter = LetterMaker.MakeLetter(letterDef);
				questPart_Letter.letter.lookTargets = QuestGenUtility.ToLookTargets(this.lookTargets, slate);
				questPart_Letter.letter.relatedFaction = this.relatedFaction.GetValue(slate);
			}
			questPart_Letter.chosenPawnSignal = QuestGenUtility.HardcodedSignalWithQuestID(this.chosenPawnSignal.GetValue(slate));
			questPart_Letter.useColonistsOnMap = this.useColonistsOnMap.GetValue(slate);
			questPart_Letter.useColonistsFromCaravanArg = this.useColonistsFromCaravanArg.GetValue(slate);
			questPart_Letter.acceptedVisitorsSignal = QuestGenUtility.HardcodedSignalWithQuestID(this.acceptedVisitorsSignal.GetValue(slate));
			questPart_Letter.visitors = this.visitors.GetValue(slate);
			questPart_Letter.signalListenMode = this.signalListenMode.GetValue(slate).GetValueOrDefault();
			questPart_Letter.filterDeadPawnsFromLookTargets = this.filterDeadPawnsFromLookTargets.GetValue(slate);
			QuestGen.quest.AddPart(questPart_Letter);
		}

		// Token: 0x0400E3AA RID: 58282
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x0400E3AB RID: 58283
		public SlateRef<Faction> relatedFaction;

		// Token: 0x0400E3AC RID: 58284
		public SlateRef<LetterDef> letterDef;

		// Token: 0x0400E3AD RID: 58285
		public SlateRef<string> label;

		// Token: 0x0400E3AE RID: 58286
		public SlateRef<string> text;

		// Token: 0x0400E3AF RID: 58287
		public SlateRef<RulePack> labelRules;

		// Token: 0x0400E3B0 RID: 58288
		public SlateRef<RulePack> textRules;

		// Token: 0x0400E3B1 RID: 58289
		public SlateRef<IEnumerable<object>> lookTargets;

		// Token: 0x0400E3B2 RID: 58290
		public SlateRef<QuestPart.SignalListenMode?> signalListenMode;

		// Token: 0x0400E3B3 RID: 58291
		[NoTranslate]
		public SlateRef<string> chosenPawnSignal;

		// Token: 0x0400E3B4 RID: 58292
		public SlateRef<MapParent> useColonistsOnMap;

		// Token: 0x0400E3B5 RID: 58293
		public SlateRef<bool> useColonistsFromCaravanArg;

		// Token: 0x0400E3B6 RID: 58294
		[NoTranslate]
		public SlateRef<string> acceptedVisitorsSignal;

		// Token: 0x0400E3B7 RID: 58295
		public SlateRef<List<Pawn>> visitors;

		// Token: 0x0400E3B8 RID: 58296
		public SlateRef<bool> filterDeadPawnsFromLookTargets;

		// Token: 0x0400E3B9 RID: 58297
		private const string RootSymbol = "root";
	}
}
